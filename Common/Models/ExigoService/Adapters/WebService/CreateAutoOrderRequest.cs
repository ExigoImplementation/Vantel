using Common.Models.ShoppingService.Interfaces;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Api.ExigoWebService
{
    public partial class CreateAutoOrderRequest
    {
        public CreateAutoOrderRequest() { }
        public CreateAutoOrderRequest(AutoOrder autoOrder)
        {
            if (autoOrder == null) return;

            CustomerID = autoOrder.CustomerID;

            if (autoOrder.AutoOrderID != 0)
            {
                ExistingAutoOrderID = autoOrder.AutoOrderID;
                OverwriteExistingAutoOrder = true;
            }

            Description  = autoOrder.Description;
            StartDate    = autoOrder.StartDate;
            StopDate     = autoOrder.StopDate;
            CurrencyCode = autoOrder.CurrencyCode;
            WarehouseID  = autoOrder.WarehouseID;
            ShipMethodID = autoOrder.ShipMethodID;
            PriceType    = PriceTypes.Wholesale;
            Frequency    = ExigoDAL.GetFrequencyType(autoOrder.FrequencyTypeID);
            PaymentType  = ExigoDAL.GetAutoOrderPaymentType(autoOrder.AutoOrderPaymentTypeID);
            ProcessType  = ExigoDAL.GetAutoOrderProcessType(autoOrder.AutoOrderProcessTypeID);
            Details = autoOrder.Details.Select(c => new OrderDetailRequest()
            {
                ItemCode                         = c.ItemCode,
                Quantity                         = c.Quantity,
                ParentItemCode                   = c.ParentItemCode,
                BusinessVolumeEachOverride       = c.BVEachOverride,
                CommissionableVolumeEachOverride = c.CVEachOverride,
                DescriptionOverride              = c.ItemDescription,
                PriceEachOverride                = c.PriceEachOverride,
                ShippingPriceEachOverride        = c.ShippingPriceEachOverride,
                TaxableEachOverride              = c.TaxableEachOverride
            }).ToArray();

            if (autoOrder.ShippingAddress != null)
            {
                FirstName  = autoOrder.ShippingAddress.FirstName;
                MiddleName = autoOrder.ShippingAddress.MiddleName;
                LastName   = autoOrder.ShippingAddress.LastName;
                Company    = autoOrder.ShippingAddress.Company;
                Email      = autoOrder.ShippingAddress.Email;
                Phone      = autoOrder.ShippingAddress.Phone;
                Address1   = autoOrder.ShippingAddress.Address1;
                Address2   = autoOrder.ShippingAddress.Address2;
                City       = autoOrder.ShippingAddress.City;
                State      = autoOrder.ShippingAddress.State;
                Zip        = autoOrder.ShippingAddress.Zip;
                Country    = autoOrder.ShippingAddress.Country;
            }

            Notes   = autoOrder.Notes;
            Other11 = autoOrder.Other11;
            Other12 = autoOrder.Other12;
            Other13 = autoOrder.Other13;
            Other14 = autoOrder.Other14;
            Other15 = autoOrder.Other15;
            Other16 = autoOrder.Other16;
            Other17 = autoOrder.Other17;
            Other18 = autoOrder.Other18;
            Other19 = autoOrder.Other19;
            Other20 = autoOrder.Other20;
        }


        public CreateAutoOrderRequest(IOrderConfiguration configuration, AutoOrderPaymentType paymentType, DateTime startDate, int shipMethodID, IEnumerable<IShoppingCartItem> items, ShippingAddress address)
        {
            WarehouseID  = configuration.WarehouseID;
            PriceType    = configuration.PriceTypeID;
            CurrencyCode = configuration.CurrencyCode;
            StartDate    = startDate;
            PaymentType  = paymentType;
            ProcessType  = AutoOrderProcessType.AlwaysProcess;
            ShipMethodID = shipMethodID;

            // Add all kit children under their parents. Add missing kit children
            // Will throw exception if dynamic kit children cannot be auto populated due to insuficient cart info
            IItemConverterService shoppingService = new Common.Services.ShoppingService(configuration);
            var details = shoppingService.GetOrderDetails(items.ToList());
            Details = details.ToArray();

            FirstName  = address.FirstName;
            MiddleName = address.MiddleName;
            LastName   = address.LastName;
            Company    = address.Company;
            Email      = address.Email;
            Phone      = address.Phone;
            Address1   = address.Address1;
            Address2   = address.Address2;
            City       = address.City;
            State      = address.State;
            Zip        = address.Zip;
            Country    = address.Country;
        }
    }
}