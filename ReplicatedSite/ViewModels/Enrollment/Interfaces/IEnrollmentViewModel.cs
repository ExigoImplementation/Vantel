using ReplicatedSite.Models;

namespace ReplicatedSite.ViewModels
{
    public interface IEnrollmentViewModel
    {
        EnrollmentPropertyBag PropertyBag { get; set; }
        string[] Errors { get; set; }        
    }
}