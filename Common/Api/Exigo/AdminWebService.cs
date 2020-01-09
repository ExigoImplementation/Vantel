using Common.Api.ExigoAdminWebService;
using Common;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static ExigoApiAdmin AdminWebService(int sandboxID = 0)
        {
            return GetAdminWebServiceContext(((sandboxID > 0) ? sandboxID : GlobalSettings.Exigo.Api.SandboxID));
        }
    }
}