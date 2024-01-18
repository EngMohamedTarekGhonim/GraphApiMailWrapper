using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Dto
{
    public class MessageHeader
    {
        public string From_Name { get; set; }
        public string From_Address { get; set; }
        public List<string> To { get; set; }
        public List<string> CcRecipients { get; set; }
        public List<string> BccRecipients { get; set; }

        public string Subject { get; set; }
        public DateTime Date { get; set; }

        public string Body { get; set; }

        public int Size { get; set; }
        public string ID { get; set; }

        public List<Attachments> FindAllAttachments { get; set; }
    }
    public class Attachments
    {
        public string AttachmentFileName { get; set; }
        public byte[] AttachmentBinaryContent { get; set; }
    }
}
