using Common;
using Common.Api.ExigoAdminWebService;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        private static ExigoApiAdmin GetAdminWebServiceContext(int sandboxID = 0)
        {

            return CreateAdminWebServiceContext(sandboxID);
        }
        public static ExigoApiAdmin CreateAdminWebServiceContext(int sandboxID)
        {
            // Determine which URL we should use
            var url = GetAdminWebServiceUrl(sandboxID);

            // Create the context
            return new ExigoApiAdmin
            {
                ApiAuthenticationValue = new ApiAuthentication
                {
                    LoginName = GlobalSettings.Exigo.Api.LoginName,
                    Password  = GlobalSettings.Exigo.Api.Password,
                    Company   = GlobalSettings.Exigo.Api.CompanyKey
                },
                Url = url
            };
        }

        private static string GetAdminWebServiceUrl()
        {
            return GetAdminWebServiceUrl(GlobalSettings.Exigo.Api.SandboxID);
        }
        private static string GetAdminWebServiceUrl(int sandboxID)
        {
            var urlFormat = "http://{0}.exigo.com/admin/1.0/exigoapiadmin.asmx";
            var cname = "api";

            if (sandboxID > 0)
            {
                cname = "sandboxapi" + sandboxID;
            }

            return string.Format(urlFormat, cname);
        }
    }
}