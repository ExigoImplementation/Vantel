namespace Backoffice.ViewModels
{
    public class WaitingRoomSponsorViewModel
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }

        public string DisplayName
        {
            get { return "{0} {1}, ID# {2}".FormatWith(this.FirstName, this.LastName, this.CustomerID); }
        }
        public string Tags
        {
            get { return string.Join(",", this.CustomerID, this.FirstName, this.LastName, this.Company); }
        }
    }
}