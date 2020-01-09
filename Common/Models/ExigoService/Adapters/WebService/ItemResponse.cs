using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class ItemResponse
    {
        public static explicit operator ExigoService.Item(ItemResponse item)
        {
            var model = new ExigoService.Item();
            if (item == null) return model;

            model.ItemID               = 0;
            model.ItemCode             = item.ItemCode;
            model.Type                 = ShoppingCartItemType.Order;

            model.ItemDescription      = item.Description;
            model.Price                = item.Price;
            model.CurrencyCode         = string.Empty;
            model.BV                   = item.BusinessVolume;
            model.CV                   = item.CommissionableVolume;

            model.OtherPrice1          = item.Other1Price;
            model.OtherPrice2          = item.Other2Price;
            model.OtherPrice3          = item.Other3Price;
            model.OtherPrice4          = item.Other4Price;
            model.OtherPrice5          = item.Other5Price;
            model.OtherPrice6          = item.Other6Price;
            model.OtherPrice7          = item.Other7Price;
            model.OtherPrice8          = item.Other8Price;
            model.OtherPrice9          = item.Other9Price;
            model.OtherPrice10         = item.Other10Price;

            model.TinyImageUrl         = GlobalUtilities.GetProductImagePath(item.TinyPicture);
            model.SmallImageUrl        = GlobalUtilities.GetProductImagePath(item.SmallPicture);
            model.LargeImageUrl        = GlobalUtilities.GetProductImagePath(item.LargePicture);

            model.ShortDetail1         = item.ShortDetail;
            model.ShortDetail2         = item.ShortDetail2;
            model.ShortDetail3         = item.ShortDetail3;
            model.ShortDetail4         = item.ShortDetail4;
            model.LongDetail1          = item.LongDetail;
            model.LongDetail2          = item.LongDetail2;
            model.LongDetail3          = item.LongDetail3;
            model.LongDetail4          = item.LongDetail4;

            model.IsVirtual            = item.IsVirtual;
            model.AllowOnAutoOrder     = item.AllowOnAutoOrder;

            model.Field1               = item.Field1;
            model.Field2               = item.Field2;
            model.Field3               = item.Field3;
            model.Field4               = item.Field4;
            model.Field5               = item.Field5;
            model.Field6               = item.Field6;
            model.Field7               = item.Field7;
            model.Field8               = item.Field8;
            model.Field9               = item.Field9;
            model.Field10              = item.Field10;

            model.OtherCheck1          = item.OtherCheck1;
            model.OtherCheck2          = item.OtherCheck2;
            model.OtherCheck3          = item.OtherCheck3;
            model.OtherCheck4          = item.OtherCheck4;
            model.OtherCheck5          = item.OtherCheck5;

            return model;
        }
    }
}