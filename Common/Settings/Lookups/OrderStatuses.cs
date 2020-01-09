namespace Common
{
	public static class OrderStatuses
    {
	/// <summary>
	///	Order Status 0
	/// </summary>
	public const int Incomplete = 0;
	/// <summary>
	///	Order Status 1
	/// </summary>
	public const int Pending = 1;
	/// <summary>
	///	Order Status 2
	/// </summary>
	public const int CCDeclined = 2;
	/// <summary>
	///	Order Status 3
	/// </summary>
	public const int ACHDeclined = 3;
	/// <summary>
	///	Order Status 4
	/// </summary>
	public const int Cancelled = 4;
	/// <summary>
	///	Order Status 5
	/// </summary>
	public const int CCPending = 5;
	/// <summary>
	///	Order Status 6
	/// </summary>
	public const int ACHPending = 6;
	/// <summary>
	///	Order Status 7
	/// </summary>
	public const int Accepted = 7;
	/// <summary>
	///	Order Status 8
	/// </summary>
	public const int Printed = 8;
	/// <summary>
	///	Order Status 9
	/// </summary>
	public const int Shipped = 9;
	/// <summary>
	///	Order Status 10
	/// </summary>
	public const int PendingInventory = 10;
		}
}