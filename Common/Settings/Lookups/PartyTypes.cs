namespace Common
{
    /// <summary>
    /// Advise that you must create these manually on new client integration, there are none by default
    /// 
    /// For testing purposes if they are missing, run this in sync:
    /// 
    /// insert into PartyTypes(PartyTypeID, PartyTypeDescription) values(1, 'Standard')
    /// insert into PartyTypes(PartyTypeID, PartyTypeDescription) values(2, 'Virtual')
    /// </summary>
    public static class PartyTypes
    {
        /// <summary>
        /// Normal Party that includes a Party Address
        /// </summary>
        public const int Standard = 1;

        /// <summary>
        /// Party that does not have a physical address
        /// </summary>
        public const int Virtual = 2;
    }
}