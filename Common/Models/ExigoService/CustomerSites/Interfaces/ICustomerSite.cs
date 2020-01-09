namespace ExigoService
{
    public interface ICustomerSite
    {
        int CustomerID { get; set; }
        string WebAlias { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Company { get; set; }

        string Email { get; set; }
        string PrimaryPhone { get; set; }
        string SecondaryPhone { get; set; }
        string Fax { get; set; }

        Address Address { get; set; }

        string Notes1 { get; set; }
        string Notes2 { get; set; }
        string Notes3 { get; set; }
        string Notes4 { get; set; }
    }
}