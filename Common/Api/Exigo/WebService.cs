using Common.Api.ExigoWebService;
using Common;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static ExigoApi WebService(int sandboxID = 0)
        {
            return GetWebServiceContext(((sandboxID > 0) ? sandboxID : GlobalSettings.Exigo.Api.SandboxID));
        }
    }
}