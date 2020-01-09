using System.Collections.Generic;

namespace ExigoService
{
    public class GetItemInformationRequest : ItemInformationRequest
    {
        public GetItemInformationRequest() : base()
        {
            this.ItemCodes = new List<string>();
        }
        public GetItemInformationRequest(IOrderConfiguration configuration) : base(configuration)
        {
            this.ItemCodes = new List<string>();
        }
        public GetItemInformationRequest(IOrderConfiguration configuration, int languageID) : base(configuration, languageID)
        {
            this.ItemCodes = new List<string>();
        }
        public GetItemInformationRequest(GetItemsRequest request): base(request)
        {
            this.ItemCodes = new List<string>();
        }
        public GetItemInformationRequest(IItemInformationRequest request) : base(request)
        {
            this.ItemCodes = new List<string>();
        }

        // custom
        public List<string> ItemCodes { get; set; }
    }
}