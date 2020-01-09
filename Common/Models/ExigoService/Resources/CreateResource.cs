using Common;
using System;
using System.Collections.Generic;

namespace ExigoService
{
    public class CreateResource
    {
        
        public string Title { get; set; }
        public List<Guid> CategoryID { get; set; }
        public string Url { get; set; }
        public string UrlThumbnail { get; set; }
        public Guid TypeID { get; set; }
        public Guid StatusID { get; set; }
        public DateTime? PostDate { get; set; }
        public List<string> Keywords { get; set; }
        public List<string> Markets { get; set; }
        public string ItemDescription { get; set; }
        public string Language { get; set; }

    }
}
