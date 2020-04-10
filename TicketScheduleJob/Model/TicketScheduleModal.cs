using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScheduleJob
{
    public class TicketScheduleModal
    {
      
        public int ScheduleID { get; set; }
        
        public int TenantID { get; set; }

        public string ScheduleFor { get; set; }

        public int ScheduleType { get; set; }

        public string ScheduleTime { get; set; }
        
        public bool IsDaily { get; set; }
        
        public int NoOfDay { get; set; }

        public bool IsWeekly { get; set; }

        public int NoOfWeek { get; set; }

        public string DayIds { get; set; }

        public bool IsDailyForMonth { get; set; }

        public int NoOfDaysForMonth { get; set; }

        public int NoOfMonthForMonth { get; set; }
        
        public bool IsWeeklyForMonth { get; set; }
        
        public int NoOfMonthForWeek { get; set; }

        public int NoOfWeekForWeek { get; set; }

        public string NameOfDayForWeek { get; set; }

        public bool IsWeeklyForYear { get; set; }

        public int NoOfWeekForYear { get; set; }

        public string NameOfDayForYear { get; set; }

        public string NameOfMonthForYear { get; set; }

        public bool IsDailyForYear { get; set; }

        public string NameOfMonthForDailyYear { get; set; }

        public int NoOfDayForDailyYear { get; set; }

        public string SearchInputParams { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int ModifyBy { get; set; }

        public DateTime ModifyDate { get; set; }

        public int ScheduleFrom { get; set; }

        public string CreatedByEmailId { get; set; }

        public string CreatedByFirstName { get; set; }

        public string CreatedByLastName { get; set; }

        public string SendToEmailID { get; set; }



        public string SearchOutputFileName { get; set; }

        public string Emailsubject { get; set; }

        public string Emailbody { get; set; }

        public int Alert_TypeID { get; set; }

        public SMTPDetails SMTPDetails { get; set; }
    }

}
