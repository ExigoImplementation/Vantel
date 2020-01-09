using System.Web.Mvc;

namespace Common.Providers
{
    public interface ILogicProvider
    {
        Controller Controller { get; set; }

        CheckLogicResult CheckLogic();
        ActionResult GetNextAction();

        bool IsAuthenticated();
    }
}
