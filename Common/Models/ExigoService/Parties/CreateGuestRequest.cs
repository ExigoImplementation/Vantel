using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class CreateGuestRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }

        public int FacebookID { get; set; }
        public string FacebookUsername { get; set; }

        public string TwitterUsername { get; set; }

        public string ImageUrl { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}