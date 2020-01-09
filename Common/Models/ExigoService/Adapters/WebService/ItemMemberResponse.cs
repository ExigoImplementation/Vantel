namespace Common.Api.ExigoWebService
{
    public partial class ItemMemberResponse
    {
        public static explicit operator ExigoService.ItemGroupMember(ItemMemberResponse item)
        {
            var model = new ExigoService.ItemGroupMember();
            if (item == null) return model;

            model.ItemCode             = item.ItemCode;
            model.MemberDescription    = item.MemberDescription;

            return model;
        }
    }
}