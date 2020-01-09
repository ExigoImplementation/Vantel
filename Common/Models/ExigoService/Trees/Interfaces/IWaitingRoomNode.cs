using System;

namespace ExigoService
{
    interface IWaitingRoomNode
    {
        int CustomerID { get; set; }
        int? EnrollerID { get; set; }
        DateTime EnrollmentDate { get; set; }
    }
}