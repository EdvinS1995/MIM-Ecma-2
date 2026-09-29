using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureRepo.Entity
{
    public class PostMessage
    {
        /// <summary>
        /// UPN of user posting the message
        /// </summary>
        public string RequesterUPN { get; set; }


        /// <summary>
        /// Source of message - usually name of application
        /// </summary>
        public string Source { get; set; } = string.Empty;


        /// <summary>
        /// List of samaccount separated with ; to send message to
        /// </summary>
        public List<string> ToSamAccountList { get; set; } = new List<string>();

        /// <summary>
        /// List of rot user guid separated with ; to send message to
        /// </summary>
        //public string? ToROTUserGuidList { get; set; }

        /// <summary>
        /// List of email separated with ; to send message to
        /// </summary>
        public List<string> ToEmailList { get; set; } = new List<string>();

        /// <summary>
        /// List of UPN separated with ; to send message to
        /// </summary>
        public List<string> ToUPNList { get; set; } = new List<string>();


        /// <summary>
        /// If true the messsage will be sent now, if false the message will be in INC Drafts to make changes
        /// </summary>
        public bool SendNow { get; set; }

        /// <summary>
        /// Set sender email - will be noreply@intility.no if empty
        /// </summary>
        public string SenderEmail { get; set; } = "noreply@intility.no";

        public List<PostLanguage> MessageLanguages { get; set; }
    }

    /// <summary>
    /// List of messages by language to send different texts by users Language
    /// If empty to and subject from main will be used
    /// </summary>
    public class PostLanguage
    {
        /// <summary>
        /// Language to set right message by users Language
        /// Can be Norwegian or English for now.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Subject/Title of email - if not empty message will be sent as email
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Content of email - MUST be HTML!
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// SMS text to send. If not empty message will be sent as sms
        /// </summary>
        public string SMSText { get; set; }


        public bool SendMail { get; set; }

        public bool SendSMS { get; set; }

    }
}
