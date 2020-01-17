using Common;
using Common.Api.ExigoWebService;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Runtime.Caching;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static Customer GetCustomer(int customerID, bool realtime = false)
        {
            if (!MemoryCache.Default.Contains("Customer_" + customerID))
            {
                Customer customer = null;
                if (!realtime)
                {
                    using (var context = ExigoDAL.Sql())
                    {
                        customer = context.Query<Customer, Address, Address, Address, Customer>(@"                                
                                     SELECT 
                                        c.CustomerID
                                        ,c.FirstName
                                        ,c.MiddleName
                                        ,c.LastName
                                        ,c.NameSuffix
                                        ,c.Company
                                        ,c.CustomerTypeID
                                        ,c.CustomerStatusID
                                        ,c.Email
                                        ,c.Phone  AS PrimaryPhone	
                                        ,c.Phone2 AS SecondaryPhone	
                                        ,c.MobilePhone
                                        ,c.Fax
                                        ,c.CanLogin
                                        ,c.LoginName
                                        ,c.PasswordHash
                                        ,c.RankID
                                        ,c.EnrollerID
                                        ,c.SponsorID
                                        ,c.BirthDate
                                        ,c.CurrencyCode
                                        ,c.PayableToName
                                        ,c.DefaultWarehouseID
                                        ,c.PayableTypeID
                                        ,c.CheckThreshold
                                        ,c.LanguageID
                                        ,c.Gender
                                        ,c.TaxCode AS TaxID
                                        ,c.TaxCodeTypeID
                                        ,c.IsSalesTaxExempt
                                        ,c.SalesTaxCode
                                        ,c.SalesTaxExemptExpireDate
                                        ,c.VatRegistration
                                        ,c.BinaryPlacementTypeID
                                        ,c.UseBinaryHoldingTank
                                        ,c.IsInBinaryHoldingTank
                                        ,c.IsEmailSubscribed
                                        ,c.EmailSubscribeIP
                                        ,c.Notes
                                        ,c.Field2
                                        ,c.Field3
                                        ,c.Field4
                                        ,c.Field5
                                        ,c.Field6
                                        ,c.Field7
                                        ,c.Field8
                                        ,c.Field9
                                        ,c.Field10
                                        ,c.Field11
                                        ,c.Field12
                                        ,c.Field13
                                        ,c.Field14
                                        ,c.Field15
                                        ,c.Date1
                                        ,c.Date2
                                        ,c.Date3
                                        ,c.Date4
                                        ,c.Date5
                                        ,c.CreatedDate
                                        ,c.ModifiedDate
                                        ,c.CreatedBy
                                        ,c.ModifiedBy
	                                    ,cs.CustomerStatusDescription
                                        ,ct.CustomerTypeDescription
                                        ,c.MainAddress1	AS Address1
                                        ,c.MainAddress2	AS Address2
                                        ,c.MainAddress3 AS Address3
                                        ,c.MainCity AS City
                                        ,c.MainState AS State
                                        ,c.MainZip AS Zip
                                        ,c.MainCountry AS Country
                                        ,c.MainCounty AS County
                                        ,c.MainVerified AS isVerified
                                        ,c.MailAddress1	AS Address1 
                                        ,c.MailAddress2	AS Address2 
                                        ,c.MailAddress3	AS Address3 
                                        ,c.MailCity AS City
                                        ,c.MailState AS State
                                        ,c.MailZip AS Zip
                                        ,c.MailVerified AS isVerified
                                        ,c.MailCountry AS Country
                                        ,c.MailCounty AS County
                                        ,c.OtherAddress1 AS Address1 
                                        ,c.OtherAddress2 AS Address2 
                                        ,c.OtherAddress3 AS Address3 
                                        ,c.OtherCity AS City
                                        ,c.OtherState AS State
                                        ,c.OtherZip	AS Zip		
                                        ,c.OtherCountry	AS Country	
                                        ,c.OtherCounty AS County
                                        ,c.OtherVerified AS isVerified
                                    FROM Customers c
	                                    LEFT JOIN CustomerStatuses cs
		                                    ON c.CustomerStatusID = cs.CustomerStatusID
	                                    LEFT JOIN CustomerTypes ct
		                                    ON c.CustomerTypeID = ct.CustomerTypeID
                                    WHERE c.CustomerID = @CustomerID
                    ", (cust, main, mail, other) =>
                        {
                            main.AddressType = AddressType.Main;
                            cust.MainAddress = main;
                            mail.AddressType = AddressType.Mailing;
                            cust.MailingAddress = mail;
                            other.AddressType = AddressType.Other;
                            cust.OtherAddress = other;
                            return cust;
                        },
                             param: new
                             {
                                 CustomerID = customerID
                             }, splitOn: "Address1, Address1, Address1"
                             ).FirstOrDefault();
                    }
                }

                if (customer == null)
                {
                    var customerResponse = ExigoDAL.WebService().GetCustomers(new GetCustomersRequest { CustomerID = customerID }).Customers.FirstOrDefault();

                    customer = (Customer)customerResponse;
                    customer.RankID = customerResponse.RankID;
                    customer.CustomerStatusDescription = CommonResources.CustomerStatuses(customerResponse.CustomerStatus);
                }
                if (customer == null)
                {
                    return null;
                }
                MemoryCache.Default.Add("Customer_" + customerID, customer, DateTime.Now.AddMinutes(15));

                return customer;
            }
            else
            {
                return MemoryCache.Default.Get("Customer_" + customerID) as Customer;
            }
        }

        public static bool UpdateCustomer(UpdateCustomerRequest request)
        {
            if (MemoryCache.Default.Contains("Customer_" + request.CustomerID))
            {
                PurgeCustomer(request.CustomerID);
            }
            var response = ExigoDAL.WebService().UpdateCustomer(request);
            return response.Result.Status == ResultStatus.Success;
        }

        public static void PurgeCustomer(int customerID)
        {
            MemoryCache.Default.Remove("Customer_" + customerID);
        }

        public static List<Customer> GetCustomers(List<int> customerIDs)
        {
            var customers = new List<Customer>();

            if (customerIDs.Count > 0)
            {
                using (var context = ExigoDAL.Sql())
                {
                    customers = context.Query<Customer, ShippingAddress, Address, Address, Customer>(@"
                                    SELECT c.CustomerID
                                          ,c.FirstName
                                          ,c.MiddleName
                                          ,c.LastName
                                          ,c.NameSuffix
                                          ,c.Company
                                          ,c.CustomerTypeID
                                          ,c.CustomerStatusID
                                          ,c.Email
                                          ,PrimaryPhone = c.Phone
                                          ,SecondaryPhone = c.Phone2
                                          ,c.MobilePhone
                                          ,c.Fax
                                          ,c.CanLogin
                                          ,c.LoginName
                                          ,c.PasswordHash
                                          ,c.RankID
                                          ,c.EnrollerID
                                          ,c.SponsorID
                                          ,c.BirthDate
                                          ,c.CurrencyCode
                                          ,c.PayableToName
                                          ,c.DefaultWarehouseID
                                          ,c.PayableTypeID
                                          ,c.CheckThreshold
                                          ,c.LanguageID
                                          ,c.Gender
                                          ,c.TaxCode
                                          ,c.TaxCodeTypeID
                                          ,c.IsSalesTaxExempt
                                          ,c.SalesTaxCode
                                          ,c.SalesTaxExemptExpireDate
                                          ,c.VatRegistration
                                          ,c.BinaryPlacementTypeID
                                          ,c.UseBinaryHoldingTank
                                          ,c.IsInBinaryHoldingTank
                                          ,c.IsEmailSubscribed
                                          ,c.EmailSubscribeIP
                                          ,c.Notes
                                          ,c.Field1
                                          ,c.Field2
                                          ,c.Field3
                                          ,c.Field4
                                          ,c.Field5
                                          ,c.Field6
                                          ,c.Field7
                                          ,c.Field8
                                          ,c.Field9
                                          ,c.Field10
                                          ,c.Field11
                                          ,c.Field12
                                          ,c.Field13
                                          ,c.Field14
                                          ,c.Field15
                                          ,c.Date1
                                          ,c.Date2
                                          ,c.Date3
                                          ,c.Date4
                                          ,c.Date5
                                          ,c.CreatedDate
                                          ,c.ModifiedDate
                                          ,c.CreatedBy
                                          ,c.ModifiedBy
	                                      ,cs.CustomerStatusDescription
                                          ,ct.CustomerTypeDescription
                                          ,Address1 = c.MainAddress1
                                          ,Address2 = c.MainAddress2
                                          ,Address3 = c.MainAddress3
                                          ,City = c.MainCity
                                          ,State = c.MainState
                                          ,Zip = c.MainZip
                                          ,Country = c.MainCountry
                                          ,County = c.MainCounty
                                          ,Address1 = c.MailAddress1
                                          ,Address2 = c.MailAddress2
                                          ,Address3 = c.MailAddress3
                                          ,City = c.MailCity
                                          ,State = c.MailState
                                          ,Zip = c.MailZip
                                          ,Country = c.MailCountry
                                          ,County = c.MailCounty
                                          ,Address1 = c.OtherAddress1
                                          ,Address2 = c.OtherAddress2
                                          ,Address3 = c.OtherAddress3
                                          ,City = c.OtherCity
                                          ,State = c.OtherState
                                          ,Zip = c.OtherZip
                                          ,Country = c.OtherCountry
                                          ,County = c.OtherCounty
                                    FROM Customers c
	                                    LEFT JOIN CustomerStatuses cs
		                                    ON c.CustomerStatusID = cs.CustomerStatusID
	                                    LEFT JOIN CustomerTypes ct
		                                    ON c.CustomerTypeID = ct.CustomerTypeID
                                    WHERE c.CustomerID in @customerIDs
                        ", (cust, main, mail, other) =>
                    {
                        cust.MainAddress = main;
                        cust.MailingAddress = mail;
                        cust.OtherAddress = other;
                        return cust;
                    },
                     param: new
                     {
                         customerIDs = customerIDs
                     }, splitOn: "Address1, Address1, Address1"
                     ).ToList();
                }
            }

            return customers;
        }
        public static List<Customer> GetCustomersFromEmail(string email)
        {
            var customers = new List<Customer>();

            using (var context = ExigoDAL.Sql())
            {
                customers = context.Query<Customer>(@"
                            SELECT c.CustomerID
                                ,c.FirstName
                                ,c.MiddleName
                                ,c.LastName
                                ,c.NameSuffix
                                ,c.Company
                                ,c.CustomerTypeID
                                ,c.CustomerStatusID
                                ,c.Email
                                ,PrimaryPhone = c.Phone
                                ,SecondaryPhone = c.Phone2
                                ,c.MobilePhone
                                ,c.Fax
                                ,c.CanLogin
                                ,c.LoginName
                                ,c.PasswordHash
                                ,c.RankID
                                ,c.EnrollerID
                                ,c.SponsorID
                                ,c.BirthDate
                                ,c.CurrencyCode
                                ,c.PayableToName
                                ,c.DefaultWarehouseID
                                ,c.PayableTypeID
                                ,c.CheckThreshold
                                ,c.LanguageID
                                ,c.Gender
                                ,c.TaxCode
                                ,c.TaxCodeTypeID
                                ,c.IsSalesTaxExempt
                                ,c.SalesTaxCode
                                ,c.SalesTaxExemptExpireDate
                                ,c.VatRegistration
                                ,c.BinaryPlacementTypeID
                                ,c.UseBinaryHoldingTank
                                ,c.IsInBinaryHoldingTank
                                ,c.IsEmailSubscribed
                                ,c.EmailSubscribeIP
                                ,c.CreatedDate
                                ,c.ModifiedDate
                                ,c.CreatedBy
                                ,c.ModifiedBy
	                            ,cs.CustomerStatusDescription
                                ,ct.CustomerTypeDescription
                            FROM Customers c
	                        LEFT JOIN CustomerStatuses cs
		                        ON c.CustomerStatusID = cs.CustomerStatusID
	                        LEFT JOIN CustomerTypes ct
		                        ON c.CustomerTypeID = ct.CustomerTypeID
                            WHERE c.Email = @email", 
                        new
                        {
                            email = email
                        }).ToList();
            }


            return customers;
        }
        public static string GetCustomerEmail(int customerID)
        {
            string email;

            using (var context = ExigoDAL.Sql())
            {
                email = context.Query<string>(@"select top 1 Email = isnull(Email, '') from Customers where CustomerID = @customerID ", new { customerID = customerID }).FirstOrDefault();
            }

            return email;
        }
        public static string GetCustomerPassword(int customerID)
        {
            try
            {
                if (GlobalSettings.Parties.CustomReportID_GetCustomerPassword > 0)
                {
                    //Create Request
                    GetCustomReportRequest request = new GetCustomReportRequest { ReportID = GlobalSettings.Parties.CustomReportID_GetCustomerPassword };

                    //Add Parameters
                    List<ParameterRequest> parameters = new List<ParameterRequest>();

                    parameters.Add(new ParameterRequest
                    {
                        ParameterName = "PassCheck",
                        // Unique passkey to ensure only we are calling this report and it is not accessed from others with API credentials
                        Value = "!00000000000000"
                    });

                    parameters.Add(new ParameterRequest
                    {
                        ParameterName = "CID",
                        Value = customerID
                    });

                    //Now attach the list to the request
                    request.Parameters = parameters.ToArray();

                    //Send Request to Server and Get Response
                    GetCustomReportResponse response = ExigoDAL.WebService().GetCustomReport(request);

                    string password = response.ReportData.Tables[0].Rows[0].ItemArray[0].ToString();
                    //Now examine the results:
                    return password;
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public static IEnumerable<CustomerWallItem> GetCustomerRecentActivity(GetCustomerRecentActivityRequest request)
        {
            List<CustomerWallItem> wallItems;
            using (var context = ExigoDAL.Sql())
            {
                wallItems = context.Query<CustomerWallItem>(@"
                                SELECT CustomerWallItemID
                                      ,CustomerID
                                      ,EntryDate
                                      ,Text
                                      ,Field1
                                      ,Field2
                                      ,Field3
                                FROM CustomerWall
                                WHERE CustomerID = @CustomerID
								ORDER BY EntryDate DESC
                    ", new
                     {
                         CustomerID = request.CustomerID
                     }).ToList();
            }

            if (request.StartDate != null)
            {
                wallItems = wallItems.Where(c => c.EntryDate >= request.StartDate).ToList();
            }

            return wallItems;
        }

        public static CustomerStatus GetCustomerStatus(int customerStatusID)
        {
            var customerStatus = new CustomerStatus();
            using (var context = ExigoDAL.Sql())
            {
                customerStatus = context.Query<CustomerStatus>(@"
                                SELECT CustomerStatusID
                                      ,CustomerStatusDescription
                                FROM CustomerStatuses
                                WHERE CustomerStatusID = @CustomerStatusID
                    ", new
                     {
                         CustomerStatusID = customerStatusID
                     }).FirstOrDefault();
            }

            if (customerStatus == null) return null;

            return customerStatus;
        }
        public static CustomerType GetCustomerType(int customerTypeID)
        {
            var customerType = new CustomerType();
            using (var context = ExigoDAL.Sql())
            {
                customerType = context.Query<CustomerType>(@"
                                SELECT CustomerTypeID
                                      ,CustomerTypeDescription
                                      ,PriceTypeID
                                FROM CustomerTypes
                                WHERE CustomerTypeID = @CustomerTypeID
                    ", new
                     {
                         CustomerTypeID = customerTypeID
                     }).FirstOrDefault();
            }

            if (customerType == null) return null;

            return customerType;
        }

        public static List<CustomerSocialNetwork> GetCustomerSocialNetwork(int customerID)
        {
            List<CustomerSocialNetwork> customerSocialNetworks = new List<CustomerSocialNetwork>();
            using (var context = ExigoDAL.Sql())
            {
                customerSocialNetworks = context.Query<CustomerSocialNetwork>(@"
                                SELECT  csc.SocialNetworkID, 
                                        csc.Url, 
                                        sc.SocialNetworkDescription
                                FROM customersocialNetworks csc 
                                        inner join SocialNetworks sc on csc.SocialNetworkID=sc.SocialNetworkID
                                WHERE csc.CustomerID = @customerID
                    ", new
                    {
                        CustomerID = customerID
                    }).ToList();
            }

            if (customerSocialNetworks == null) return new List<CustomerSocialNetwork>();

            return customerSocialNetworks;
        }

        public static void SetCustomerPreferredLanguage(int customerID, int languageID)
        {
            ExigoDAL.WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                LanguageID = languageID
            });
        }

        public static bool IsEmailAvailable(int customerID, string email)
        {
            var isEmailAvailable = true;

            var customer = ExigoDAL.WebService().GetCustomers(new GetCustomersRequest()
            {
                Email = email
            }).Customers
            .Where(c => c.CustomerID != customerID).Count();

            if (customer > 0)
            {
                isEmailAvailable = false;
            }

            return isEmailAvailable;
        }
        public static bool IsEmailCustomersEmail(string email, int hostID)
        {
            var isEmailAvailable = false;

            using (var context = ExigoDAL.Sql())
            {
                var customers = context.Query<int>(@"
                                select CustomerID from Customers
                                where (Email = @email or Loginname = @email)
                                and CustomerID = @customerID
                                ", new { email = email, customerID = hostID }).ToList();

                if (customers.Count() > 0)
                {
                    isEmailAvailable = true;
                }
            }

            return isEmailAvailable;
        }
        public static bool IsLoginNameAvailable(string loginname, int customerID = 0)
        {
            if (customerID > 0)
            {
                // Get the current login name to see if it matches what we passed. If so, it's still valid.
                var currentLoginName = ExigoDAL.GetCustomer(customerID).LoginName;
                if (loginname.Equals(currentLoginName, StringComparison.InvariantCultureIgnoreCase)) return true;
            }

            // Validate the login name
            // cannot use SQL due to delay in update to replicated database
            var apiCustomer = ExigoDAL.WebService().GetCustomers(new GetCustomersRequest()
            {
                LoginName = loginname
            }).Customers.FirstOrDefault();

            return (apiCustomer == null);
        }

        public static void SendEmailVerification(int customerID, string email)
        {
            // Create the publicly-accessible verification link
            string sep = "&";
            if (!GlobalSettings.Emails.VerifyEmailUrl.Contains("?")) sep = "?";

            string encryptedValues = Security.Encrypt(new
            {
                CustomerID = customerID,
                Email = email,
                Date = DateTime.Now
            });

            var verifyEmailUrl = GlobalSettings.Emails.VerifyEmailUrl + sep + "token=" + encryptedValues;


            // Send the email
            ExigoDAL.SendEmail(new SendEmailRequest
            {
                To = new[] { email },
                From = GlobalSettings.Emails.NoReplyEmail,
                ReplyTo = new[] { GlobalSettings.Emails.NoReplyEmail },
                Subject = "{0} - Verify your email".FormatWith(GlobalSettings.Company.Name),
                Body = @"
                    <p>
                        {1} has received a request to enable this email account to receive email notifications from {1} and your upline.
                    </p>

                    <p> 
                        To confirm this email account, please click the following link:<br />
                        <a href='{0}'>{0}</a>
                    </p>

                    <p>
                        If you did not request email notifications from {1}, or believe you have received this email in error, please contact {1} customer service.
                    </p>

                    <p>
                        Sincerely, <br />
                        {1} Customer Service
                    </p>"
                    .FormatWith(verifyEmailUrl, GlobalSettings.Company.Name)
            });
        }
        public static void OptInCustomer(string token)
        {
            var decryptedToken = Security.Decrypt(token);

            var customerID = Convert.ToInt32(decryptedToken.CustomerID);
            var email = decryptedToken.Email.ToString();

            OptInCustomer(customerID, email);
        }
        public static void OptInCustomer(int customerID, string email)
        {
            ExigoDAL.WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                Email = email,
                SubscribeToBroadcasts = true,
                SubscribeFromIPAddress = GlobalUtilities.GetClientIP()
            });
        }
        public static void OptOutCustomer(int customerID)
        {
            ExigoDAL.WebService().UpdateCustomer(new UpdateCustomerRequest
            {
                CustomerID = customerID,
                SubscribeToBroadcasts = false
            });
        }

        //Only Used for the Dashboard Card... Really all this does is return the customerID's then lets the customer model pull the avatar URL.
        public static List<Customer> GetNewestDistributors(GetNewestDistributorsRequest request)
        {
            var newestDistributors = new List<Customer>();
            var customerTypes = request.CustomerTypes;
            var customerStatuses = request.CustomerStatuses;
            using (var context = ExigoDAL.Sql())
            {
                newestDistributors = context.Query<Customer>(@"
                                SELECT TOP (@RowCount) un.DownlineCustomerID
	                                  , c.CustomerID
                                      , c.FirstName
                                      , c.MiddleName
                                      , c.LastName
                                      , c.CreatedDate
                                      , c.Phone AS PrimaryPhone
                                      , c.Email
                                FROM UniLevelDownline un
	                                LEFT JOIN Customers c
		                                ON un.CustomerID = c.CustomerID
                                WHERE un.DownlineCustomerID = @CustomerID
                                    AND c.CustomerID <> @CustomerID
	                                AND un.Level <= @Level
                                    AND c.CustomerTypeID IN @CustomerTypes
                                    AND c.CustomerStatusID IN @CustomerStatuses
                                    AND c.CreatedDate >= CASE 
                                        WHEN @days > 0 
                                        THEN getdate()-@Days 
                                        ELSE c.CreatedDate 
                                        END
                                ORDER BY CreatedDate
                    ", new
                     {
                        CustomerID       = request.CustomerID,
                        Level            = request.MaxLevel,
                        RowCount         = request.RowCount,
                        CustomerTypes    = customerTypes,
                        CustomerStatuses = customerStatuses,
                        Days             = request.Days

                     }).ToList();
            }

            return newestDistributors;
        }

        public static CustomerRankCollection GetCustomerRanks(GetCustomerRanksRequest request)
        {
            var result = new CustomerRankCollection();
            try
            {
            
                var periodID = (request.PeriodID != null) ? request.PeriodID : ExigoDAL.GetCurrentPeriod(request.PeriodTypeID).PeriodID;
                
                //Get the highest paid rank in any period from the customer record
                var highestRankAchieved = new Rank();

                using (var context = ExigoDAL.Sql())
                {
                    highestRankAchieved = context.Query<Rank>(@"
                            SELECT 
	                            c.RankID
	                            ,r.RankDescription	

                            FROM
	                            Customers c
	                            INNER JOIN Ranks r
		                            ON r.RankID = c.RankID

                            WHERE
	                            c.CustomerID = @customerid                       
                            ", new
                    {
                        customerid = request.CustomerID
                    }).FirstOrDefault();

                    if (highestRankAchieved != null)
                    {
                        result.HighestPaidRankInAnyPeriod = highestRankAchieved;
                    }
                }

                //Get the current period rank for the period/period type specified
                var currentPeriodRank = new Rank();

                using (var context = ExigoDAL.Sql())
                {
                    currentPeriodRank = context.Query<Rank>(@"
                            SELECT 
	                            RankID = pv.PaidRankID
	                            ,r.RankDescription	

                            FROM
	                            PeriodVolumes pv
	                            INNER JOIN Ranks r
		                            ON r.RankID = pv.PaidRankID	

                            WHERE
	                            pv.CustomerID = @customerid
	                            AND pv.PeriodTypeID = @periodtypeid
	                            AND pv.PeriodID = @periodid                      
                            ", new
                    {
                        customerid = request.CustomerID,
                        periodtypeid = request.PeriodTypeID,
                        periodid = periodID
                    }).FirstOrDefault();

                    if (currentPeriodRank != null)
                    {
                        result.CurrentPeriodRank = currentPeriodRank;
                    }
                }

                //Get the highest paid rank up to the specified period
                var highestPaidRankUpToPeriod = new Rank();

                using (var context = ExigoDAL.Sql())
                {
                    highestPaidRankUpToPeriod = context.Query<Rank>(@"
                            SELECT 
	                            pv.RankID
	                            ,r.RankDescription	

                            FROM
	                            PeriodVolumes pv
	                            INNER JOIN Ranks r
		                            ON r.RankID = pv.RankID	

                            WHERE
	                            pv.CustomerID = @customerid
	                            AND pv.PeriodTypeID = @periodtypeid
	                            AND pv.PeriodID = @periodid                      
                            ", new
                    {
                        customerid = request.CustomerID,
                        periodtypeid = request.PeriodTypeID,
                        periodid = periodID
                    }).FirstOrDefault();

                    if (highestPaidRankUpToPeriod != null)
                    {
                        result.HighestPaidRankUpToPeriod = highestPaidRankUpToPeriod;
                    }
                }
                return result;
            }
            catch(Exception ex)
            {
                var error = ex.Message;
                return result;
            }

        }

        public static DateTime GetCustomerCreatedDate(int customerID)
        {
            using (var context = ExigoDAL.Sql())
            {
                return context.ExecuteScalar<DateTime>(@"
                    SELECT
                        CreatedDate
                    FROM
                        Customers
                    WHERE
                        CustomerID = @CustomerID
                ", new
                {
                    CustomerID = customerID
                });
            }
        }

        public static bool IsCustomerInlDownline(int topCustomerID, int customerID, string downlineTree = "default")
        {
            var results = new List<dynamic>();
            string treeToUse = string.Empty;
            if (downlineTree == "default")
            {
                treeToUse = GlobalSettings.Exigo.UseBinary ? "BinaryDownline" : "UnilevelDownline";
            }
            else
            {
                treeToUse = "EnrollerDownline";
            }

            using (var context = ExigoDAL.Sql())
            {
                results = context.Query($@"
                        SELECT 
	                        d.CustomerID	

                        FROM
	                        {treeToUse} d

                        WHERE
	                        d.DownlineCustomerID = @topcustomerID 
	                        AND d.CustomerID = @customerID              
                        ", new
                {
                    topcustomerid = topCustomerID,
                    customerid = customerID
                }).ToList();
            }

            return results.Count() != 0;
        }
    }
}
