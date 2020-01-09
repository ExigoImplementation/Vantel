using System.Collections.Generic;

namespace ExigoService
{
    public class PopulateAdditionalItemDataRequest : ItemInformationRequest, IChildItemRequest
    {
        public PopulateAdditionalItemDataRequest() : base()
        {
            this.Items = new List<Item>();
        }
        public PopulateAdditionalItemDataRequest(IOrderConfiguration configuration) : base(configuration)
        {
            this.Items = new List<Item>();
        }
        public PopulateAdditionalItemDataRequest(IOrderConfiguration configuration, int languageID) : base(configuration, languageID)
        {
            this.Items = new List<Item>();
        }
        public PopulateAdditionalItemDataRequest(GetItemsRequest request): base (request)
        {
            request.CopyPropertiesTo<IChildItemRequest>(this);
            this.Items = new List<Item>();
        }
        public PopulateAdditionalItemDataRequest(IItemInformationRequest request) : base(request) { }
        
        // custom
        public List<Item> Items { get; set; }
        public List<int> WebCategoryIDs { get; set; }

        // IChildItemRequest
        public bool IncludeDynamicKitChildren { get; set; }
        public bool IncludeStaticKitChildren { get; set; }
        public bool IncludeGroupMembers { get; set; }
    }
}