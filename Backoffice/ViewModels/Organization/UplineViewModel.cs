using System.Collections.Generic;

namespace Backoffice.ViewModels
{
    public class UplineViewModel
    {
        public UplineViewModel()
        {
            this.UplineNodes = new List<UplineNodeViewModel>();
        }

        public List<UplineNodeViewModel> UplineNodes { get; set; }
    }
    public class UplineNodeViewModel
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Level { get; set; }

        public string FullName
        {
            get { return string.Join(" ", this.FirstName, this.LastName); }
        }
    }
}