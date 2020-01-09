
namespace ExigoService
{
        public class Host
        {
            public int CustomerID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Zip { get; set; }
            public string Country { get; set; }

            public string FullName
            {
                get { return string.Join(" ", this.FirstName, this.LastName); }
            }
        }
}