namespace ExigoService
{
    public interface ICustomerSocialNetwork
    {
        int CustomerID { get; set; }
        int SocialNetworkID { get; set; }
        string SocialNetworkDescription { get; set; }
        string Url { get; set; }
    }
}