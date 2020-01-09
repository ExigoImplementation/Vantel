namespace Common.Api.ExigoWebService
{
    public partial class OrderDetailResponse
    {
        public static explicit operator ExigoService.OrderDetail(OrderDetailResponse detail)
        {
            var model = new ExigoService.OrderDetail();
            if (detail == null) return model;

            model.OrderDetailID   = detail.OrderLine;
            model.OrderID         = 0;

            model.ItemID          = 0;
            model.ItemCode        = detail.ItemCode;
            model.ItemDescription = detail.Description;
            model.Quantity        = detail.Quantity;
            model.PriceEach       = detail.PriceEach;
            model.PriceTotal      = detail.PriceTotal;
            model.Tax             = detail.Tax;
            model.WeightEach      = detail.WeightEach;
            model.Weight          = detail.Weight;
            model.BVEach          = detail.BusinessVolumeEach;
            model.BV              = detail.BusinesVolume;
            model.CVEach          = detail.CommissionableVolumeEach;
            model.CV              = detail.CommissionableVolume;

            model.Other1Each      = detail.Other1Each;
            model.Other1          = detail.Other1;
            model.Other2Each      = detail.Other2Each;
            model.Other2          = detail.Other2;
            model.Other3Each      = detail.Other3Each;
            model.Other3          = detail.Other3;
            model.Other4Each      = detail.Other4Each;
            model.Other4          = detail.Other4;
            model.Other5Each      = detail.Other5Each;
            model.Other5          = detail.Other5;
            model.Other6Each      = detail.Other6Each;
            model.Other6          = detail.Other6;
            model.Other7Each      = detail.Other7Each;
            model.Other7          = detail.Other7;
            model.Other8Each      = detail.Other8Each;
            model.Other8          = detail.Other8;
            model.Other9Each      = detail.Other9Each;
            model.Other9          = detail.Other9;
            model.Other10Each     = detail.Other10Each;
            model.Other10         = detail.Other10;

            return model;
        }
    }
}