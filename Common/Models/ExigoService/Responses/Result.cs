using Common;

namespace ExigoService
{
    public class Result
    {
        public Result()
        {
            this.ErrorMessage = string.Empty;
            this.Success = true;
        }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

        public void Pass ()
        {
            this.Success = true;
        }
        public void Fail( string errorMessage = "")
        {
            this.Success = false;
            this.ErrorMessage = GlobalUtilities.Coalesce(errorMessage, "Failed");
        }
    }
}