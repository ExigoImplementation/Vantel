namespace Common
{
    /// <summary>
    /// Advise that you must create these manually on new client integration, there are none by default
    /// 
    /// For testing purposes if they are missing, run this in sync:
    /// 
    /// insert into PartyStatuses(PartyStatusID, PartyStatusDescription) values (1, 'Open')
    /// insert into PartyStatuses(PartyStatusID, PartyStatusDescription) values(2, 'Closed')
    /// insert into PartyStatuses(PartyStatusID, PartyStatusDescription) values(3, 'Canceled')
    /// </summary>
    public static class PartyStatusTypes
    {
        /// <summary>
        /// Party is Open and Active, able to be shopped under
        /// </summary>
        public const int Open = 1;

        /// <summary>
        /// Party is considered Closed and no longer able to be shopped under
        /// </summary>
        public const int Closed = 2;

        /// <summary>
        /// Party is Canceled, same as Closed Status as far as functionality
        /// </summary>
        public const int Canceled = 3;
    }
}