using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScheduleJob
{
    public class EnumMaster
    {
        public enum TicketStatus
        {
            /// <summary>
            ///TicketStatus - Draft
            /// </summary>
            [Description("Draft")]
            Draft = 100,

            /// <summary>
            ///TicketStatus - New
            /// </summary>
            [Description("New")]
            New = 101,

            /// <summary>
            ///TicketStatus - Open
            /// </summary>
            [Description("Open")]
            Open = 102,

            /// <summary>
            ///TicketStatus - Resolved
            /// </summary>
            [Description("Resolved")]
            Resolved = 103,

            /// <summary>
            ///TicketStatus - Closed
            /// </summary>
            [Description("Closed")]
            Closed = 104,

            /// <summary>
            ///TicketStatus - Re-Opened
            /// </summary>
            [Description("Re-Opened")]
            ReOpened = 105,

        }

        public enum Alert_TypeID
        {
            //Dashboard = 19,
            //Ticket = 20,
            //Report = 21

            Dashboard = 1,
            Ticket = 2,
            Report = 3
        }
    }
}
