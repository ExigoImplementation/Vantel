namespace ExigoService
{
    public class CustomerStatus : ICustomerStatus
    {
        public CustomerStatus()
        { }

        public CustomerStatus(int customerStatusID, string description)
        {
            this.CustomerStatusID = customerStatusID;
            this.CustomerStatusDescription = description;
        }

        public int CustomerStatusID { get; set; }
        public string CustomerStatusDescription { get; set; }
    }
}