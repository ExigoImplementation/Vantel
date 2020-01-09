namespace ExigoService
{
    public class CustomerSocialNetwork : ICustomerSocialNetwork
    {
        public int SocialNetworkID { get; set; }
        public int CustomerID { get; set; }
        public string SocialNetworkDescription { get; set; }
        public string Url { get; set; }
    }
}