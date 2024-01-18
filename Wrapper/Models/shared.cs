using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService.Models
{
    public class shared
    {

        public class Body
        {
            public string contentType { get; set; }
            public string content { get; set; }
        }

        public class EmailAddress
        {
            public string name { get; set; }
            public string address { get; set; }
        }

        public class Flag
        {
            public string flagStatus { get; set; }
        }

        public class From
        {
            public EmailAddress emailAddress { get; set; }
        }



        public class Sender
        {
            public EmailAddress emailAddress { get; set; }
        }

        public class ToRecipient
        {
            public EmailAddress emailAddress { get; set; }
        }

        public class CcRecipient
        {
            public EmailAddress emailAddress { get; set; }
        }
        public class BcRecipient
        {
            public EmailAddress emailAddress { get; set; }
        } public class ReplyTo
        {
            public EmailAddress emailAddress { get; set; }
        }
    }
}
