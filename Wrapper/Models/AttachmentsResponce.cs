using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Models
{
    public class AttachmentsResponce
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<AttachmentValue> value { get; set; }
    }

    public class AttachmentValue
    {
        [JsonProperty("@odata.type")]
        public string odatatype { get; set; }

        [JsonProperty("@odata.mediaContentType")]
        public string odatamediaContentType { get; set; }
        public string id { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string name { get; set; }
        public string contentType { get; set; }
        public int size { get; set; }
        public bool isInline { get; set; }
        public string contentId { get; set; }
        public object contentLocation { get; set; }
        public string contentBytes { get; set; }
    }

}
