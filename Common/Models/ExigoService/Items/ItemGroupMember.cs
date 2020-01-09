namespace ExigoService
{
    public class ItemGroupMember : IItemGroupMember
    {
        public string ItemCode { get; set; }
        public string MasterItemCode { get; set; }

        public string MemberDescription { get; set; }
        public int SortOrder { get; set; }

        public Item Item { get; set; }
    }
}
