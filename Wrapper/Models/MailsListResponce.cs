﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MailService.Models.shared;

namespace MailService.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class MailsListResponce
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        public List<Value> value { get; set; }
        [JsonProperty("@odata.nextLink")]
        public string odatanextLink { get; set; }
    }

    public class Value
    {
        [JsonProperty("@odata.etag")]
        public string odataetag { get; set; }
        public string id { get; set; }
        public DateTime createdDateTime { get; set; }
        public DateTime lastModifiedDateTime { get; set; }
        public string changeKey { get; set; }
        public List<object> categories { get; set; }
        public DateTime receivedDateTime { get; set; }
        public DateTime sentDateTime { get; set; }
        public bool hasAttachments { get; set; }
        public string internetMessageId { get; set; }
        public string subject { get; set; }
        public string bodyPreview { get; set; }
        public string importance { get; set; }
        public string parentFolderId { get; set; }
        public string conversationId { get; set; }
        public string conversationIndex { get; set; }
        public object isDeliveryReceiptRequested { get; set; }
        public bool isReadReceiptRequested { get; set; }
        public bool isRead { get; set; }
        public bool isDraft { get; set; }
        public string webLink { get; set; }
        public string inferenceClassification { get; set; }
        public Body body { get; set; }
        public Sender sender { get; set; }
        public From from { get; set; }
        public List<ToRecipient> toRecipients { get; set; }
        public List<object> ccRecipients { get; set; }
        public List<object> bccRecipients { get; set; }
        public List<object> replyTo { get; set; }
        public Flag flag { get; set; }
        public AttachmentsResponce AttachmentsResponce { get; set; }
    }


}
