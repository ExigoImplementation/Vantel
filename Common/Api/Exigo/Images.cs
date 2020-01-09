using System;
using System.IO;
using Common;
using System.Data.SqlClient;
using Serilog;
using Dapper;
using System.Linq;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static ExigoImageApi Images()
        {
            return new ExigoImageApi();
        }
    }

    public sealed class ExigoImageApi
    {
        private string LoginName = GlobalSettings.Exigo.Api.LoginName;
        private string Password = GlobalSettings.Exigo.Api.Password;

        public AvatarResponse GetCustomerAvatarResponse(int customerID, AvatarType type, bool cache = true, byte[] bytes = null)
        {
            var response = new AvatarResponse();
            response.Bytes = bytes;

            var path = "/customers/" + customerID.ToString();
            var filename = "avatar";
            switch (type)
            {
                //case AvatarType.Tiny: filename += "-xs"; break;
                case AvatarType.Small: filename += "-sm.png"; break;
                case AvatarType.Large: filename += "-lg.png"; break;
            }

            if (bytes == null)
            {
                using (var conn = new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting))
                {
                    conn.Open();

                    string query = "Select top 1 ImageData as Bytes, '' as FileType, '' as FileName, ModifiedDate From ImageFiles Where Path=@FilePath AND Name=@FileName";
                    var res = conn.Query<AvatarResponse>(query, new { FilePath = path, FileName = filename}).FirstOrDefault();
                    if (res != null)
                        response = res;
                }
            }

            // If we didn't find anything there, convert the default image (which is Base64) to a byte array.
            // We'll use that instead
            if (response.Bytes == null)
            {
                bytes = Convert.FromBase64String(GlobalSettings.Avatars.DefaultAvatarAsBase64);
                response.FileName = filename; //We will respond with the generic avatar filename so we can let the browser cache it.
                return GetCustomerAvatarResponse(customerID, type, cache, GlobalUtilities.ResizeImage(bytes, type));
            }
            else
            {
                var extension = Path.GetExtension(filename).ToLower();
                string contentType = "image/jpeg";
                switch (extension)
                {
                    case ".gif":
                        contentType = "image/gif";
                        break;
                    case ".jpeg":
                        contentType = "image/png";
                        break;
                    case ".bmp":
                        contentType = "image/bmp";
                        break;
                    case ".png":
                        contentType = "image/png";
                        break;
                    case ".jpg":
                    default:
                        contentType = "image/jpeg";
                        break;
                }
                response.FileName = customerID.ToString()+ "-" + Path.GetFileNameWithoutExtension(filename) + "-" + response.ModifiedDate.ToBinary() + extension; //If we have it, we will change the filename to the customerID and modifieddate so it will show up immediately.
                response.FileType = contentType;
            }

            return response;
        }
        
        public bool SetCustomerAvatar(int customerID, byte[] bytes, bool saveToHistory = false)
        {
            return ((GlobalUtilities.InsertOrUpdateAvatarToAPI(customerID, bytes) ? GlobalUtilities.InsertOrUpdateAvatarToReportingDatabase(customerID, bytes) : false));
        }

        public bool SaveImage(string path, string filename, byte[] bytes)
        {
            try
            {

                ExigoDAL.WebService().SetImageFile(new Common.Api.ExigoWebService.SetImageFileRequest
                {
                    Name = filename,
                    Path = path,
                    ImageData = bytes
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Saving Image: {Message}", ex.Message);
                return false;
            }

            // If we got here, everything should be good
            return true;
        }        
    }

    public class AvatarResponse
    {
        public byte[] Bytes { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}