namespace Common.Api.ExigoWebService
{
    public partial class AutoOrderDetailResponse
    {
        public static explicit operator ExigoService.AutoOrderDetail(AutoOrderDetailResponse detail)
        {
            var model = new ExigoService.AutoOrderDetail();
            if (detail == null) return model;

            model.AutoOrderDetailID                = 0;
            model.AutoOrderID                      = 0;

            model.ItemCode                         = detail.ItemCode;
            model.ItemDescription                  = detail.Description;
            model.Quantity                         = detail.Quantity;
            model.ParentItemCode                   = detail.ParentItemCode;

            model.PriceEach                        = detail.PriceEach;
            model.PriceEachOverride                = detail.PriceEachOverride;
            model.PriceTotal                       = detail.PriceTotal;

            model.BV                               = detail.BusinesVolume;
            model.BVEach                           = detail.BusinessVolumeEach;
            model.BVEachOverride                   = detail.BusinessVolumeEachOverride;

            model.CV                               = detail.CommissionableVolume;
            model.CVEach                           = detail.CommissionableVolumeEach;
            model.CVEachOverride                   = detail.CommissionableVolumeEachOverride;

            model.TaxableEachOverride              = detail.TaxableEachOverride;
            model.ShippingPriceEachOverride        = detail.ShippingPriceEachOverride;
            model.Reference1                       = detail.Reference1;

            return model;
        }
    }
}