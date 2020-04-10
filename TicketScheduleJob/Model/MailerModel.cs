using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScheduleJob
{
    public class SMTPDetails
    {
        /// <summary>
        /// Frome Email Id
        /// </summary>
        public string FromEmailId { get; set; }

        /// <summary>
        /// Email Sender Name
        /// </summary>
        public string EmailSenderName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Enable SSL
        /// </summary>
        public bool EnableSsl { get; set; }

        /// <summary>
        /// SMTP Port
        /// </summary>
        public string SMTPPort { get; set; }

        /// <summary>
        /// SMTP Server
        /// </summary>
        public string SMTPServer { get; set; }

        /// <summary>
        /// Is body HTML
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// SMTP Host
        /// </summary>
        public string SMTPHost { get; set; }
    }

    public class SchedulerMailResultModel
    {
        public string EMailID { get; set; }

        public int SchedulerID { get; set; }

        public string SchedulerType { get; set; }

        public bool IsSend { get; set; }

        public string Message { get; set; }
    }
}
