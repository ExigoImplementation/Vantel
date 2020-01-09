using System.Web.Mvc;

namespace Common.Providers
{
    public class CheckLogicResult
    {
        public static CheckLogicResult Failure(ActionResult nextAction)
        {
            return new CheckLogicResult
            {
                IsValid = false,
                NextAction = nextAction
            };
        }
        public static CheckLogicResult Success(ActionResult nextAction)
        {
            return new CheckLogicResult
            {
                IsValid = true,
                NextAction = nextAction
            };
        }

        public bool IsValid { get; set; }
        public ActionResult NextAction { get; set; }
    }
}
