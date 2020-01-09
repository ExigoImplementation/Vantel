using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class GuestDetailViewModel
    {
        public GuestDetailViewModel()
        {
        }

        public Guest Guest { get; set; }
        public Customer Customer { get; set; }
    }
}