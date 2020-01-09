using ExigoService;

namespace Backoffice.ViewModels
{
    public class NodeViewModel
    {
        public NestedTreeNode Node { get; set; }
        public Rank Rank { get; set; }
        public bool PersonallyEnrolled 
        {
            get
            {
                return (true);
            }
            set { }
        }
        public bool HasAutoOrder { get; set; }              
    }
   
}