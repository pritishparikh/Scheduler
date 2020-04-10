using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketScheduleJob
{
    public class SearchTicketModel
    {
        public int TenantID { get; set; }

        public int HeaderStatusId { get; set; }

        public int AssigntoId { get; set; }

        public int ActiveTabId { get; set; }

        /*Model for the each tab */
        public SearchDataByDate searchDataByDate { get; set; }

        public SearchDataByCustomerType searchDataByCustomerType { get; set; }

        public SearchDataByTicketType searchDataByTicketType { get; set; }

        public SearchDataByCategoryType searchDataByCategoryType { get; set; }

        public SearchDataByAll searchDataByAll { get; set; }
    }

    public class SearchResponse
    {
        public double totalpages { get; set; }
        public int ticketID { get; set; }
        public string ticketStatus { get; set; }
        public string Message { get; set; }
        public string Category { get; set; }
        public string subCategory { get; set; }
        public string IssueType { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public string CreatedOn { get; set; }
        public int isEscalation { get; set; }
        public string ClaimStatus { get; set; }
        public string TaskStatus { get; set; }
        public int TicketCommentCount { get; set; }

        public string createdBy { get; set; }
        public string createdago { get; set; }
        public string assignedTo { get; set; }
        public string assignedago { get; set; }
        public string updatedBy { get; set; }
        public string updatedago { get; set; }
        public string responseTimeRemainingBy { get; set; }
        public string responseOverdueBy { get; set; }
        public string resolutionOverdueBy { get; set; }
        public string ticketSourceType { get; set; }
        public int? ticketSourceTypeID { get; set; }
        public bool IsReassigned { get; set; }
        public bool IsSLANearBreach { get; set; }
    }
}
