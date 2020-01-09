using Backoffice.Models;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backoffice.ViewModels
{
    public class TreeViewerViewModel
    {
        public TreeViewerViewModel()
        {
            this.Ranks = new List<Rank>();
        }

        public List<Rank> Ranks { get; set; }
        public int TopCustomerID { get; set; }
        public string Token { get; set; }
        public int OwnerCustomerID { get; set; }
        public string OwnerName { get; set; }
    }
}