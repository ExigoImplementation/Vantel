using System;

namespace ExigoService
{
    public class WaitingRoomNode : IWaitingRoomNode
    {
        public int CustomerID { get; set; }
        public int? EnrollerID { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string FullName { get { return this.FirstName + " " + this.LastName; } }
    }
}