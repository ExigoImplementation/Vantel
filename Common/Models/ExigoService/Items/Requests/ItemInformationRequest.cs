using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class ItemInformationRequest : IItemInformationRequest, IItemDetailFilterRequest
    {
        /// <summary>
        /// handle defaults and allow parameterless item creation
        /// </summary>
        public ItemInformationRequest()
        {

        }
        /// <summary>
        /// Capture configuration details and pass up to default constructor to handle defaults
        /// </summary>
        public ItemInformationRequest(IOrderConfiguration configuration) : this()
        {
            this.WarehouseID  = configuration.WarehouseID;
            this.PriceTypeID  = configuration.PriceTypeID;
            this.CurrencyCode = configuration.CurrencyCode;
        }
        /// <summary>
        /// Capture language and pass up to sibling constructor to capture configuration details
        /// </summary>
        public ItemInformationRequest(IOrderConfiguration configuration, int? languageID) : this(configuration)
        {
            this.LanguageID = languageID;
        }
        /// <summary>
        /// Capture IItemDetailFilterRequest details and pass up to sibling constructor to capture language/configuration
        /// </summary>
        public ItemInformationRequest(GetItemsRequest request): this(request.Configuration, request.LanguageID)
        {
            request.CopyPropertiesTo<IItemDetailFilterRequest>(this);
        }
        /// <summary>
        /// Capture IItemInformationRequest details and pass up to default constructor to handle defaults
        /// <para>Used to create a copy of existing IItemInformationRequest requests</para>
        /// </summary>
        public ItemInformationRequest(IItemInformationRequest request) : this()
        {
            request.CopyPropertiesTo<IItemInformationRequest>(this);
        }

        // IItemInformationRequest
        public int WarehouseID { get; set; }
        public string CurrencyCode { get; set; }
        public int PriceTypeID { get; set; }
        public int? LanguageID { get; set; }

        // IItemDetailFilterRequest
        public bool IncludeShortDescriptions { get; set; }
        public bool IncludeLongDescriptions { get; set; }
        public bool IncludeExtendedFields { get; set; }
        public bool IncludeExtendedlItemPrices { get; set; }
    }
}