namespace ExigoService
{
    /// <summary>
    /// Object based on Database structure from [dbo].[ItemStaticKitMembers]
    /// </summary>
    public class ItemStaticKitMember
    {
        public string MasterItemCode { get; set; }
        public string ItemCode { get; set; }
        public decimal Quantity { get; set; }
    }
}