using MailService.Dto;
using MailService.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailService
{
    public class GraphApiClient
    {
        public bool Connected { get; set; }
        public string Token { get; set; }

        public  void Connect(Credentials Credentials)
        {

            var client = new RestClient($"https://login.microsoftonline.com/{Credentials.TenantId}/oauth2/token");
            //  client.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            //  request.AddHeader("Cookie", "buid=0.AXQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA.AQABAAEAAAD--DLA3VO7QrddgJg7WevrZWR8dUFe2g3MKCUye_9Z4rPF_MulzMKNPNa69r2oD3PWimwdRE1W5QN51q1UEL6bVVKPUjJQP2bm-ILolguWj-NGRiajdgoQAkb6IQ9XStMgAA; fpc=Aj6kAL1OoyNLpqXldo1Btygvrd_sAQAAAOdrIdwOAAAA; stsservicecookie=estsfd; x-ms-gateway-slice=estsfd");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", Credentials.ClientId);
            request.AddParameter("client_secret", Credentials.ClientSecretValue);
            request.AddParameter("resource", "https://graph.microsoft.com");
            request.AddParameter("username", Credentials.Email);
            request.AddParameter("password", Credentials.Password);
            RestResponse response = client.Execute(request);
            dynamic data = JObject.Parse(response.Content);

            if(data.access_token != null)
            {
                Connected = true;
                Token = data.access_token;
            }
            else 
            {
                Connected = false;
            }
        }
        public void Close() 
        {
            Token = null;
        }
        public int GetMessageCount()
        {
            if (Token != null)
            {
               List<string> mailsList = GetMessageUids();
               return mailsList.Count;
            }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }

        public List<string> GetMessageUids() 
        {
            if (Token != null)
            {
               
                string parameter = "?$select=id";
                string uri = $"https://graph.microsoft.com/v1.0/me/messages{parameter}";
                List<string> uidsList = new List<string>();
                bool MoreMails = false;
                do
                {
                    GetMessagePatchResponce responce = GetMessagePatch(uri);
                    uidsList.AddRange(responce.uidsPatch);
                    if (responce.MoreMails) 
                    {
                        uri = responce.nextUri;
                        MoreMails=true;
                    }
                    else
                    {
                        MoreMails = false;
                    }


                }
                while (MoreMails);

                return uidsList;
                }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }

        public MessageHeader GetMessageHeaders(String messageID)
        {
            if (Token != null)
            { 
                // get mail-------------
                var client = new RestClient($"https://graph.microsoft.com/v1.0/me/messages/{messageID}");
                // client.Timeout = -1;
                string accessToken = Token;
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AlwaysMultipartFormData = true;
                RestResponse response = client.Execute(request);
                var mail = JsonConvert.DeserializeObject<GetMailResponce>(response.Content);

                if (mail.hasAttachments == true)
                {
                    mail.AttachmentsResponce = GetAttachments(mail.id);
                }
                // map to mail header 
                MessageHeader messageHeader = mapMail(mail);
                return messageHeader;
            }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }
        public void DeleteMessage(string mailId)
        {
            if (Token != null)
            {
                var client = new RestClient($"https://graph.microsoft.com/v1.0/me/messages/{mailId}");
                string accessToken = Token;
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AlwaysMultipartFormData = true;
                RestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<AttachmentsResponce>(response.Content);
                //return result;
            }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }


        //------------------------------------------
        public AttachmentsResponce GetAttachments(string mailId)
        {
            if (Token != null)
            {
                var client = new RestClient($"https://graph.microsoft.com/v1.0/me/messages/{mailId}/attachments");
                string accessToken = Token;
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AlwaysMultipartFormData = true;
                RestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<AttachmentsResponce>(response.Content);
                return result;
            }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }

        public GetMessagePatchResponce GetMessagePatch(string uri)
        {
            if (Token != null)
            {

                // get mails-------------
                //string parameter = "?$select=sender,subject";
                //string parameter = "?$select=id";
                //string parameter = "";
                var client = new RestClient(uri);
                // client.Timeout = -1;
                string accessToken = Token;
                var request = new RestRequest();
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AlwaysMultipartFormData = true;
                RestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<MailsListResponce>(response.Content);
                List<string> uidsList = new List<string>();
                foreach (var u in result.value)
                {
                    uidsList.Add(u.id);
                }

                GetMessagePatchResponce getMessagePatchResponce = new GetMessagePatchResponce();
                getMessagePatchResponce.uidsPatch = uidsList;
                if (result.odatanextLink != null)
                {
                    getMessagePatchResponce.MoreMails = true;
                    getMessagePatchResponce.nextUri = result.odatanextLink;
                }

                return getMessagePatchResponce;
            }
            else
            {
                throw new Exception("Please Start Connection");
            }
        }

        public MessageHeader mapMail(GetMailResponce getMailResponce)
        {
            var messageHeader = new MessageHeader();
            messageHeader.From_Address = getMailResponce.from.emailAddress.address;
            messageHeader.From_Name = getMailResponce.from.emailAddress.name;
            messageHeader.To = new List<string>();
            foreach (var to in getMailResponce.toRecipients)
            {
                messageHeader.To.Add(to.emailAddress.address);
            }
            messageHeader.BccRecipients = new List<string>();
            foreach (var to in getMailResponce.bccRecipients)
            {
               messageHeader.To.Add(to.emailAddress.address);
            }
            messageHeader.CcRecipients = new List<string>();
            foreach (var to in getMailResponce.ccRecipients)
            {
               messageHeader.To.Add(to.emailAddress.address);
            }
            messageHeader.ID = getMailResponce.id;
            messageHeader.Date = getMailResponce.sentDateTime;
           // messageHeader.Size = getMailResponce;
            messageHeader.Subject = getMailResponce.subject;
            messageHeader.Body = getMailResponce.body.content;
            if (getMailResponce.hasAttachments)
            {
                messageHeader.FindAllAttachments = mapAttachment(getMailResponce.AttachmentsResponce);
            }

            return messageHeader;
        }

        public List<Attachments> mapAttachment(AttachmentsResponce attachmentsResponce)
        {
           List<Attachments> attachmentsList = new List<Attachments>();
            foreach(var att in attachmentsResponce.value)
            {
                Attachments attachments = new Attachments();
                attachments.AttachmentBinaryContent = Convert.FromBase64String(att.contentBytes);
                attachments.AttachmentFileName = att.name;
                attachmentsList.Add(attachments);
            }
           
            return attachmentsList;

        }
    }

    public class Credentials
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecretValue { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class GetMessagePatchResponce
    {
        public List<string> uidsPatch { get; set; } 
        public bool MoreMails  { get; set; }
        public string nextUri { get; set; }
    }
}
