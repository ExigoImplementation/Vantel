using ReplicatedSite.Models;
using Common.Api.ExigoWebService;
using System;

namespace ReplicatedSite.ViewModels
{
    public class AutoOrderSettingsViewModel : IShoppingViewModel
    {
        public DateTime AutoOrderStartDate { get; set; }
        public FrequencyType AutoOrderFrequencyType { get; set; }

        public ShoppingCartCheckoutPropertyBag PropertyBag { get; set; }
        public string[] Errors { get; set; }
    }
}