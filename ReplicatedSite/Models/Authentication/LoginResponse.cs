using Common;

namespace ReplicatedSite.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            this.Status = true;
        }
        public LoginResponse(bool success, string errormessage = "")
        {
            this.Status = success;
            this.ErrorMessage = errormessage;
        }

        /// <summary>
        /// Specifies whether the login was successful.
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Specifies a URL the login should redirect to upon successfully logging in. Used for silent logins.
        /// </summary>
        public string RedirectUrl { get; set; }

        /// <summary>
        /// Any errors that occurred during login
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// A dynamic property that can hold any additional data the developer wishes to return.
        /// </summary>
        public object Data { get; set; }

        public void Success()
        {
            Status = true;
            ErrorMessage = string.Empty;
        }
        public void Fail(string errormessage = "")
        {
            Status = false;
            ErrorMessage = GlobalUtilities.Coalesce(errormessage, "Invalid username/password");
        }
    }
}