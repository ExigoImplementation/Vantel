using System.Collections.Generic;
using System.Linq;
using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class AutoOrderResponse
    {
        public static explicit operator ExigoService.AutoOrder(AutoOrderResponse autoOrder)
        {
            var model = new ExigoService.AutoOrder();
            if (autoOrder == null) return model;

            model.AutoOrderID            = autoOrder.AutoOrderID;
            model.CustomerID             = autoOrder.CustomerID;
            model.Description            = autoOrder.Description;

            model.CurrencyCode           = autoOrder.CurrencyCode;
            model.WarehouseID            = autoOrder.WarehouseID;
            model.ShipMethodID           = autoOrder.ShipMethodID;
            model.AutoOrderStatusID      = (int)autoOrder.AutoOrderStatus;
            model.FrequencyTypeID        = ExigoDAL.GetFrequencyTypeID(autoOrder.Frequency);
            model.AutoOrderPaymentTypeID = ExigoDAL.GetAutoOrderPaymentTypeID(autoOrder.PaymentType);
            model.AutoOrderProcessTypeID = ExigoDAL.GetAutoOrderProcessTypeID(autoOrder.ProcessType);
            model.Notes                  = autoOrder.Notes;

            model.StartDate              = autoOrder.StartDate;
            model.StopDate               = autoOrder.StopDate;
            model.LastRunDate            = autoOrder.LastRunDate;
            model.NextRunDate            = autoOrder.NextRunDate;

            model.ShippingAddress              = new ExigoService.ShippingAddress
            {
                FirstName                = autoOrder.FirstName,
                MiddleName               = autoOrder.MiddleName,
                LastName                 = autoOrder.LastName,                
                Company                  = autoOrder.Company,
                AddressType              = ExigoService.AddressType.Other,
                Address1                 = autoOrder.Address1,
                Address2                 = autoOrder.Address2,                
                City                     = autoOrder.City,
                State                    = autoOrder.State,
                Zip                      = autoOrder.Zip,
                Country                  = autoOrder.Country,
                Email                    = autoOrder.Email,
                Phone                    = autoOrder.Phone
            };

            model.Details = new List<AutoOrderDetail>();
            foreach (var detail in autoOrder.Details)
            {
                model.Details.Add((AutoOrderDetail)detail);
            }

            model.Total                = autoOrder.Total;
            model.Subtotal             = autoOrder.SubTotal;
            model.TaxTotal             = autoOrder.TaxTotal;
            model.ShippingTotal        = autoOrder.ShippingTotal;
            model.DiscountTotal        = autoOrder.DiscountTotal;
            model.BVTotal              = autoOrder.BusinessVolumeTotal;
            model.CVTotal              = autoOrder.CommissionableVolumeTotal;

            model.Other11              = autoOrder.Other11;
            model.Other12              = autoOrder.Other12;
            model.Other13              = autoOrder.Other13;
            model.Other14              = autoOrder.Other14;
            model.Other15              = autoOrder.Other15;
            model.Other16              = autoOrder.Other16;
            model.Other17              = autoOrder.Other17;
            model.Other18              = autoOrder.Other18;
            model.Other19              = autoOrder.Other19;
            model.Other20              = autoOrder.Other20;

            return model;
        }
    }
}