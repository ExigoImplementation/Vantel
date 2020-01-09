namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Determines if the User can create, edit, delete, or update Calendar Events.
        /// </summary>
        /// <param name="rank">Current Customers TypeID.</param>
        /// <returns>True: Can Edit. False: Can NOT Edit.</returns>
        public static bool CalendarPermissions(int rank)
        {
            return (rank > 0);
        }
    }
}