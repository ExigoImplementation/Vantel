/******************************************************
 * Version 1.0
 * 
 * General purpose caching utilities
 * 
 * Supports both redis and sql in-memory table based
 * caches.
 ******************************************************/
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Common;
using Newtonsoft.Json;

namespace Caching
{
    public class SqlInMemoryCacheProvider : ICacheProvider
    {
        private string _connectionString;

        public SqlInMemoryCacheProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Initialize()
        {
            var initializeSql = "";

            // NOTE: If nvarchar(max) is not supported, use NVARCHAR(4000) instead. This is for 2014 environments.
            #region In Memory SQL
            var inMemorySql = $@"
                -- Ensure the datacache schema
                IF NOT EXISTS (
	                SELECT  schema_name
	                FROM    information_schema.schemata
	                WHERE   schema_name = 'cache'
                ) 
                BEGIN
	                EXEC sp_executesql N'CREATE SCHEMA cache'
                END
                IF OBJECT_ID('cache.Store') is null  BEGIN   
	                CREATE TABLE [cache].[Store]   
	                (    
	                [ID] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,    
	                [Data] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,    
	                [CreatedDate] [datetime] NOT NULL,    
	                [SecondsToLive] [bigint] NOT NULL,
	                CONSTRAINT [PK_CacheStore]  PRIMARY KEY NONCLUSTERED HASH    ([ID]) 
                    WITH ( BUCKET_COUNT = 1048576)) 
	                WITH(MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_ONLY)  
                END
                declare @cmd nvarchar(max)
                if OBJECT_ID('cache.Get') is null  Begin  
	                SET @Cmd =  ' 
	                create procedure [cache].[Get]  (@ID nvarchar(1000))  
	                WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER  as  BEGIN ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT,LANGUAGE = N''English'')  
	                Select ID, Data, CreatedDate, SecondsToLive, CurrentDate = GetUtcDate() 
	                From cache.Store 
	                where ID = @ID  
	                END    '  
                exec sp_executesql @Cmd  End
                if OBJECT_ID('cache.Set') is null  Begin  
	                SET @Cmd  ='
	                create procedure [cache].[Set]  ( @ID nvarchar(1000), @SecondsToLive bigint, @Data nvarchar(max)  )  
	                WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER  as  
	                BEGIN ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT,LANGUAGE = N''English'')     
	                Update cache.Store Set Data = @Data, SecondstoLive=@SecondsToLive, CreatedDate=GetUtcDate()    
	                Where ID=@ID     
	                if @@Rowcount = 0 begin     
		                Insert [cache].[Store](ID, Data,CreatedDate,SecondsToLive)    
		                values(@ID, @Data, GetUtcDate(), @SecondsToLive)   
		                end   
	                END  '  exec sp_executesql @Cmd  
                End
                if OBJECT_ID('cache.Purge') is null  Begin  
	                SET @Cmd =  '
	                create procedure [cache].[Purge]  
	                WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER  as  
	                BEGIN ATOMIC WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT,LANGUAGE = N''English'')     
	                DELETE cache.Store    
	                WHERE DATEADD(ss,SecondsToLive,CreatedDate) < GetUtcDate()
	                END
                '  exec sp_executesql @Cmd  
                End
                ";
            #endregion

            #region Disk Space SQL
            var diskSpaceSql = $@"
                -- Ensure the datacache schema
                IF NOT EXISTS (
	                SELECT  schema_name
	                FROM    information_schema.schemata
	                WHERE   schema_name = 'cache'
                ) 
                BEGIN
	                EXEC sp_executesql N'CREATE SCHEMA cache'
                END
                IF OBJECT_ID('cache.Store') is null  BEGIN   
	                CREATE TABLE [cache].[Store]   
	                (    
	                [ID] [nvarchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,    
	                [Data] [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,    
	                [CreatedDate] [datetime] NOT NULL,    
	                [SecondsToLive] [bigint] NOT NULL,
	                CONSTRAINT [PK_CacheStore]  PRIMARY KEY NONCLUSTERED ([ID])) 
                END
                declare @cmd nvarchar(max)
                if OBJECT_ID('cache.Get') is null  Begin  
	                SET @Cmd =  ' 
	                create procedure [cache].[Get]  (@ID nvarchar(1000))  
                    as  
                    BEGIN
	                    Select ID, Data, CreatedDate, SecondsToLive, CurrentDate = GetUtcDate() 
	                    From cache.Store 
	                    where ID = @ID  
	                END    '  
                exec sp_executesql @Cmd  End
                if OBJECT_ID('cache.Set') is null  Begin  
	                SET @Cmd  ='
	                create procedure [cache].[Set]  ( @ID nvarchar(1000), @SecondsToLive bigint, @Data nvarchar(max)  )  
	                as  
	                BEGIN      
	                    Update cache.Store Set Data = @Data, SecondstoLive=@SecondsToLive, CreatedDate=GetUtcDate()    
	                    Where ID=@ID     
	                    if @@Rowcount = 0 
                        begin     
		                    Insert [cache].[Store](ID, Data,CreatedDate,SecondsToLive)    
		                    values(@ID, @Data, GetUtcDate(), @SecondsToLive)   
		                end   
	                END  '  exec sp_executesql @Cmd  
                End
                if OBJECT_ID('cache.Purge') is null  Begin  
	                SET @Cmd =  '
	                create procedure [cache].[Purge]  
	                as  
	                BEGIN      
	                    DELETE cache.Store    
	                    WHERE DATEADD(ss,SecondsToLive,CreatedDate) < GetUtcDate()
	                END
                '  exec sp_executesql @Cmd  
                End
                ";
            #endregion

            initializeSql = (GlobalSettings.Exigo.UserSession.UseOLTPInMemory) ? inMemorySql : diskSpaceSql;

            // Create the required schema and tables, if applicable
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(initializeSql, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            // Initialize the purge process
            Purge();
        }

        public void Purge()
        {
            var purgeInterval = 600; // in seconds             
            Task.Run(async delegate
            {
                while (true)
                {
                    await Task.Delay(purgeInterval * 1000);

                    try
                    {
                        PurgeCycle();
                    }
                    catch (Exception)
                    {
                        //next time around it will work?
                    }
                }
            });
        }

        public void PurgeCycle()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(@"cache.Purge", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool TryGet<T>(string key, out DateTime entryDate, out DateTime serverDate, out T result)
        {
            entryDate = DateTime.MinValue;
            serverDate = DateTime.MinValue;
            //secondsToLive = long.MinValue;


            string serializedData = string.Empty;
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("cache.Get", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar, 100).Value = key;


                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        serializedData = reader.GetString(1);
                        entryDate = reader.GetDateTime(2);
                        //secondsToLive = reader.GetInt64(3);
                        serverDate = reader.GetDateTime(4);
                    }
                }

            }

            // If we got data from the cache...
            if (!string.IsNullOrEmpty(serializedData))
            {
                // Return what we have
                result = JsonConvert.DeserializeObject<T>(serializedData);

                return true;
            }
            else
            {
                result = default(T);

                return false;
            }

        }

        public void Set<T>(string key, TimeSpan expiry, T o)
        {
            var serializedData = JsonConvert.SerializeObject(o);

            // Put the data into the database
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("cache.Set", connection))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Add("@id", System.Data.SqlDbType.NVarChar, 1000).Value = key;
                command.Parameters.Add("@data", System.Data.SqlDbType.NVarChar, -1).Value = serializedData;
                command.Parameters.Add("@secondstolive", System.Data.SqlDbType.BigInt).Value = expiry.TotalSeconds;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}