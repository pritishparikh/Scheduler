using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace TicketScheduleJob
{
    public class DAL
    {
       
        public static MySqlConnection conn = null;

        Exceptions exceptions;
        public DAL(string ConnectionString)
        {
            conn = new MySqlConnection(ConnectionString);
            exceptions = new Exceptions();
        }


        public List<TicketScheduleModal> getScheduleDetails()
        {
            DataSet ds = new DataSet();
            List<TicketScheduleModal> ticketschedulemodal = new List<TicketScheduleModal>();

            try
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("get_ScheduleSearchDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            TicketScheduleModal obj = new TicketScheduleModal()
                            {
                                ScheduleID = dr["ScheduleID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ScheduleID"]),
                                TenantID = dr["TenantID"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["TenantID"]),
                                ScheduleFor = dr["ScheduleFor"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["ScheduleFor"]),
                                ScheduleType = dr["ScheduleType"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ScheduleType"]),
                                ScheduleTime = Convert.ToString(dr["ScheduleTime"]),
                                IsDaily = Convert.ToBoolean(dr["IsDaily"]),
                                NoOfDay = dr["NoOfDay"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfDay"]),
                                IsWeekly = Convert.ToBoolean(dr["IsWeekly"]),
                                NoOfWeek = dr["NoOfWeek"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfWeek"]),
                                DayIds = dr["DayIds"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["DayIds"]),
                                IsDailyForMonth = Convert.ToBoolean(dr["IsDailyForMonth"]),
                                NoOfDaysForMonth = dr["NoOfDaysForMonth"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfDaysForMonth"]),
                                NoOfMonthForMonth = dr["NoOfMonthForMonth"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfMonthForMonth"]),
                                IsWeeklyForMonth = Convert.ToBoolean(dr["IsWeeklyForMonth"]),
                                NoOfMonthForWeek = dr["NoOfMonthForWeek"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfMonthForWeek"]),
                                NoOfWeekForWeek = dr["NoOfWeekForWeek"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfWeekForWeek"]),
                                NameOfDayForWeek = dr["NameOfDayForWeek"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["NameOfDayForWeek"]),
                                IsWeeklyForYear = Convert.ToBoolean(dr["IsWeeklyForYear"]),
                                NoOfWeekForYear = dr["NoOfWeekForYear"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfWeekForYear"]),
                                NameOfDayForYear = dr["NameOfDayForYear"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["NameOfDayForYear"]),
                                NameOfMonthForYear = dr["NameOfMonthForYear"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["NameOfMonthForYear"]),
                                IsDailyForYear = Convert.ToBoolean(dr["IsDailyForYear"]),
                                NameOfMonthForDailyYear = dr["NameOfMonthForDailyYear"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["NameOfMonthForDailyYear"]),
                                NoOfDayForDailyYear = dr["NoOfDayForDailyYear"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["NoOfDayForDailyYear"]),
                                SearchInputParams = dr["SearchInputParams"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["SearchInputParams"]),
                                IsActive = Convert.ToBoolean(dr["IsActive"]),
                                CreatedBy = dr["CreatedBy"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["CreatedBy"]),
                                CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                                ModifyBy = dr["ModifyBy"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ModifyBy"]),
                                ModifyDate = Convert.ToDateTime(dr["ModifyDate"]),
                                CreatedByEmailId = dr["CreatedByEmailId"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByEmailId"]),
                                CreatedByFirstName = dr["CreatedByFirstName"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByFirstName"]),
                                CreatedByLastName = dr["CreatedByLastName"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByLastName"]),
                                SendToEmailID = dr["SendToEmailID"] == System.DBNull.Value ? string.Empty : Convert.ToString(dr["SendToEmailID"]),
                                ScheduleFrom = dr["ScheduleFrom"] == System.DBNull.Value ? 0 : Convert.ToInt32(dr["ScheduleFrom"])
                            };

                            ticketschedulemodal.Add(obj);
                        }

                               
                    }
                }
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();
                conn.Close();
            }

            return ticketschedulemodal;
        }


        #region  DashboardTickets

        public List<SearchOutputDashBoard> GetDashboardTicketsOnSearch(SearchInputModel searchModel)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            List<SearchOutputDashBoard> objSearchResult = new List<SearchOutputDashBoard>();

            List<string> CountList = new List<string>();

            int rowStart = 0; // searchparams.pageNo - 1) * searchparams.pageSize;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.Connection = conn;

                /*Based on active tab stored procedure will call
                    1. SP_SearchTicketData_ByDate
                    2. SP_SearchTicketData_ByCustomerType
                    3. SP_SearchTicketData_ByTicketType
                    4. SP_SearchTicketData_ByCategoryType
                    5. SP_SearchTicketData_ByAll                 
                 */
                MySqlCommand sqlcmd = new MySqlCommand("", conn);

                // sqlcmd.Parameters.AddWithValue("HeaderStatus_Id", searchModel.HeaderStatusId);

                if (searchModel.ActiveTabId == 1)//ByDate
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByDate_ForDashboard";

                    sqlcmd.Parameters.AddWithValue("Ticket_CreatedOn", string.IsNullOrEmpty(searchModel.searchDataByDate.Ticket_CreatedOn) ? "" : searchModel.searchDataByDate.Ticket_CreatedOn);
                    sqlcmd.Parameters.AddWithValue("Ticket_ModifiedOn", string.IsNullOrEmpty(searchModel.searchDataByDate.Ticket_ModifiedOn) ? "" : searchModel.searchDataByDate.Ticket_ModifiedOn);
                    sqlcmd.Parameters.AddWithValue("SLA_DueON", searchModel.searchDataByDate.SLA_DueON);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByDate.Ticket_StatusID);
                }
                else if (searchModel.ActiveTabId == 2)//ByCustomerType
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByCustomerType_ForDashBoard";

                    sqlcmd.Parameters.AddWithValue("CustomerMobileNo", string.IsNullOrEmpty(searchModel.searchDataByCustomerType.CustomerMobileNo) ? "" : searchModel.searchDataByCustomerType.CustomerMobileNo);
                    sqlcmd.Parameters.AddWithValue("customerEmail", string.IsNullOrEmpty(searchModel.searchDataByCustomerType.CustomerEmailID) ? "" : searchModel.searchDataByCustomerType.CustomerEmailID);

                    if (string.IsNullOrEmpty(Convert.ToString(searchModel.searchDataByCustomerType.TicketID)) || Convert.ToString(searchModel.searchDataByCustomerType.TicketID) == "")
                        sqlcmd.Parameters.AddWithValue("TicketID", 0);
                    else
                        sqlcmd.Parameters.AddWithValue("TicketID", Convert.ToInt32(searchModel.searchDataByCustomerType.TicketID));

                    sqlcmd.Parameters.AddWithValue("TicketStatusID", searchModel.searchDataByCustomerType.TicketStatusID);
                }
                else if (searchModel.ActiveTabId == 3)//ByTicketType
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByTicketType_ForDashBoard";

                    sqlcmd.Parameters.AddWithValue("Priority_Id", searchModel.searchDataByTicketType.TicketPriorityID);
                    sqlcmd.Parameters.AddWithValue("TicketStatusID", searchModel.searchDataByTicketType.TicketStatusID);
                    sqlcmd.Parameters.AddWithValue("channelOfPurchaseIDs", string.IsNullOrEmpty(searchModel.searchDataByTicketType.ChannelOfPurchaseIds) ? "" : searchModel.searchDataByTicketType.ChannelOfPurchaseIds);
                    sqlcmd.Parameters.AddWithValue("ActionTypeIds", searchModel.searchDataByTicketType.ActionTypes);
                }
                else if (searchModel.ActiveTabId == 4) //ByCategory
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByCategory_Dashboard";

                    sqlcmd.Parameters.AddWithValue("Category_Id", searchModel.searchDataByCategoryType.CategoryId);
                    sqlcmd.Parameters.AddWithValue("SubCategory_Id", searchModel.searchDataByCategoryType.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("IssueType_Id", searchModel.searchDataByCategoryType.IssueTypeId);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByCategoryType.TicketStatusID);
                }
                else if (searchModel.ActiveTabId == 5)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByAll_ForDashBoard";

                    /*Column 1 (5)*/
                    sqlcmd.Parameters.AddWithValue("Ticket_CreatedOn", string.IsNullOrEmpty(searchModel.searchDataByAll.CreatedDate) ? "" : searchModel.searchDataByAll.CreatedDate);
                    sqlcmd.Parameters.AddWithValue("Ticket_ModifiedOn", string.IsNullOrEmpty(searchModel.searchDataByAll.ModifiedDate) ? "" : searchModel.searchDataByAll.ModifiedDate);
                    sqlcmd.Parameters.AddWithValue("Category_Id", searchModel.searchDataByAll.CategoryId);
                    sqlcmd.Parameters.AddWithValue("SubCategory_Id", searchModel.searchDataByAll.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("IssueType_Id", searchModel.searchDataByAll.IssueTypeId);

                    /*Column 2 (5) */
                    sqlcmd.Parameters.AddWithValue("TicketSourceType_ID", searchModel.searchDataByAll.TicketSourceTypeID);
                    sqlcmd.Parameters.AddWithValue("TicketIdORTitle", string.IsNullOrEmpty(searchModel.searchDataByAll.TicketIdORTitle) ? "" : searchModel.searchDataByAll.TicketIdORTitle);
                    sqlcmd.Parameters.AddWithValue("Priority_Id", searchModel.searchDataByAll.PriorityId);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByAll.TicketSatutsID);
                    sqlcmd.Parameters.AddWithValue("SLAStatus", string.IsNullOrEmpty(searchModel.searchDataByAll.SLAStatus) ? "" : searchModel.searchDataByAll.SLAStatus);

                    /*Column 3 (5)*/
                    sqlcmd.Parameters.AddWithValue("TicketClaim_ID", Convert.ToInt32(searchModel.searchDataByAll.ClaimId));
                    sqlcmd.Parameters.AddWithValue("InvoiceNumberORSubOrderNo", string.IsNullOrEmpty(searchModel.searchDataByAll.InvoiceNumberORSubOrderNo) ? "" : searchModel.searchDataByAll.InvoiceNumberORSubOrderNo);
                    sqlcmd.Parameters.AddWithValue("OrderItemId", string.IsNullOrEmpty(Convert.ToString(searchModel.searchDataByAll.OrderItemId)) ? 0 : Convert.ToInt32(searchModel.searchDataByAll.OrderItemId));
                    sqlcmd.Parameters.AddWithValue("IsVisitedStore", searchModel.searchDataByAll.IsVisitStore == "yes" ? 1 : 0);
                    sqlcmd.Parameters.AddWithValue("IsWantToVisitStore", searchModel.searchDataByAll.IsWantVistingStore == "yes" ? 1 : 0);

                    /*Column 4 (5)*/
                    sqlcmd.Parameters.AddWithValue("Customer_EmailID", searchModel.searchDataByAll.CustomerEmailID);
                    sqlcmd.Parameters.AddWithValue("CustomerMobileNo", string.IsNullOrEmpty(searchModel.searchDataByAll.CustomerMobileNo) ? "" : searchModel.searchDataByAll.CustomerMobileNo);
                    sqlcmd.Parameters.AddWithValue("AssignTo", searchModel.searchDataByAll.AssignTo);
                    sqlcmd.Parameters.AddWithValue("StoreCodeORAddress", searchModel.searchDataByAll.StoreCodeORAddress);
                    sqlcmd.Parameters.AddWithValue("WantToStoreCodeORAddress", searchModel.searchDataByAll.WantToStoreCodeORAddress);

                    //Row - 2 and Column - 1  (5)
                    sqlcmd.Parameters.AddWithValue("HaveClaim", searchModel.searchDataByAll.HaveClaim);
                    sqlcmd.Parameters.AddWithValue("ClaimStatusId", searchModel.searchDataByAll.ClaimStatusId);
                    sqlcmd.Parameters.AddWithValue("ClaimCategoryId", searchModel.searchDataByAll.ClaimCategoryId);
                    sqlcmd.Parameters.AddWithValue("ClaimSubCategoryId", searchModel.searchDataByAll.ClaimSubCategoryId);
                    sqlcmd.Parameters.AddWithValue("ClaimIssueTypeId", searchModel.searchDataByAll.ClaimIssueTypeId);

                    //Row - 2 and Column - 2  (4)
                    sqlcmd.Parameters.AddWithValue("HaveTask", searchModel.searchDataByAll.HaveTask);
                    sqlcmd.Parameters.AddWithValue("TaskStatus_Id", searchModel.searchDataByAll.TaskStatusId);
                    sqlcmd.Parameters.AddWithValue("TaskDepartment_Id", searchModel.searchDataByAll.TaskDepartment_Id);
                    sqlcmd.Parameters.AddWithValue("TaskFunction_Id", searchModel.searchDataByAll.TaskFunction_Id);
                }



                sqlcmd.Parameters.AddWithValue("CurrentUserId", searchModel.curentUserId);
                sqlcmd.Parameters.AddWithValue("Tenant_ID", searchModel.TenantID);
                sqlcmd.Parameters.AddWithValue("Assignto_IDs", searchModel.AssigntoId.TrimEnd(','));
                sqlcmd.Parameters.AddWithValue("Brand_IDs", searchModel.BrandId.TrimEnd(','));

                sqlcmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = sqlcmd;
                da.Fill(ds);

                if (ds != null && ds.Tables != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        //SearchOutputDashBoard obj = new SearchOutputDashBoard();

                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SearchOutputDashBoard obj = new SearchOutputDashBoard()
                            {
                                ticketID = Convert.ToInt32(dr["TicketID"] == DBNull.Value ? 0 : dr["TicketID"]),
                                ticketStatus = Convert.ToString((EnumMaster.TicketStatus)Convert.ToInt32(dr["StatusID"] == DBNull.Value ? 0 : dr["StatusID"])),
                                Message = dr["TicketDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TicketDescription"]),
                                Category = dr["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CategoryName"]),
                                subCategory = dr["SubCategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SubCategoryName"]),
                                IssueType = dr["IssueTypeName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["IssueTypeName"]),
                                Priority = dr["PriortyName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["PriortyName"]),
                                Assignee = dr["AssignedName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedName"]),
                                CreatedOn = dr["CreatedOn"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedOn"]),
                                createdBy = dr["CreatedByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByName"]),
                                createdago = dr["CreatedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["CreatedDate"]), "CreatedSpan"),
                                assignedTo = dr["AssignedName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedName"]),
                                assignedago = dr["AssignedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["AssignedDate"]), "AssignedSpan"),
                                updatedBy = dr["ModifyByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifyByName"]),
                                updatedago = dr["ModifiedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["ModifiedDate"]), "ModifiedSpan"),

                                responseTimeRemainingBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityRespond"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "RespondTimeRemainingSpan"),
                                responseOverdueBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityRespond"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResponseOverDueSpan"),

                                resolutionOverdueBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityResolve"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityResolve"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResolutionOverDueSpan"),

                                TaskStatus = dr["TaskDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TaskDetails"]),
                                ClaimStatus = dr["ClaimDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ClaimDetails"]),
                                TicketCommentCount = dr["ClaimDetails"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TicketComments"]),
                                isEscalation = dr["IsEscalated"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IsEscalated"])

                            };
                            objSearchResult.Add(obj);
                        }
                    }
                }

                //paging here
                //if (searchparams.pageSize > 0 && objSearchResult.Count > 0)
                //    objSearchResult[0].totalpages = objSearchResult.Count > searchparams.pageSize ? Math.Round(Convert.ToDouble(objSearchResult.Count / searchparams.pageSize)) : 1;

                //objSearchResult = objSearchResult.Skip(rowStart).Take(searchparams.pageSize).ToList();
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (ds != null) ds.Dispose(); conn.Close();
            }
            return objSearchResult;
        }

        public string SetCreationdetails(string time, string ColName)
        {
            string timespan = string.Empty;
            DateTime now = DateTime.Now;
            TimeSpan diff = new TimeSpan();
            string[] PriorityArr = null;

            try
            {
                if (ColName == "CreatedSpan" || ColName == "ModifiedSpan" || ColName == "AssignedSpan")
                {
                    diff = now - Convert.ToDateTime(time);
                    timespan = CalculateSpan(diff) + " ago";

                }
                else if (ColName == "RespondTimeRemainingSpan")
                {
                    PriorityArr = time.Split(new char[] { '|' })[0].Split(new char[] { '-' });

                    switch (PriorityArr[1])
                    {
                        case "D":
                            diff = (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddDays(Convert.ToDouble(PriorityArr[0]))) - now;

                            break;

                        case "H":
                            diff = (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddHours(Convert.ToDouble(PriorityArr[0]))) - now;

                            break;

                        case "M":
                            diff = (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddMinutes(Convert.ToDouble(PriorityArr[0]))) - now;

                            break;

                    }
                    timespan = CalculateSpan(diff);
                }
                else if (ColName == "ResponseOverDueSpan" || ColName == "ResolutionOverDueSpan")
                {
                    PriorityArr = time.Split(new char[] { '|' })[0].Split(new char[] { '-' });

                    switch (PriorityArr[1])
                    {
                        case "D":
                            diff = now - (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddDays(Convert.ToDouble(PriorityArr[0])));

                            break;

                        case "H":
                            diff = now - (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddHours(Convert.ToDouble(PriorityArr[0])));

                            break;

                        case "M":
                            diff = now - (Convert.ToDateTime(time.Split(new char[] { '|' })[1]).AddMinutes(Convert.ToDouble(PriorityArr[0])));

                            break;

                    }

                    timespan = CalculateSpan(diff);
                }

            }
            catch (Exception ex)
            {
              //  exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (PriorityArr != null && PriorityArr.Length > 0)
                    Array.Clear(PriorityArr, 0, PriorityArr.Length);
            }
            return timespan;
        }

        public string CalculateSpan(TimeSpan ts)
        {
            string span = string.Empty;
            try
            {
                if (Math.Abs(ts.Days) > 0)
                {
                    span = Convert.ToString(Math.Abs(ts.Days)) + " Days";
                }
                else if (Math.Abs(ts.Hours) > 0)
                {
                    span = Convert.ToString(Math.Abs(ts.Hours)) + " Hours";
                }
                else if (Math.Abs(ts.Minutes) > 0)
                {
                    span = Convert.ToString(Math.Abs(ts.Minutes)) + " Minutes";
                }
                else if (Math.Abs(ts.Seconds) > 0)
                {
                    span = Convert.ToString(Math.Abs(ts.Seconds)) + " Seconds";
                }
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            return span;
        }

        #endregion

        #region  Tickets

        public List<SearchResponse> GetTicketsOnSearch(SearchTicketModel searchModel)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            List<SearchResponse> objSearchResult = new List<SearchResponse>();
            List<SearchResponse> temp = new List<SearchResponse>(); //delete later
            List<string> CountList = new List<string>();

            int rowStart = 0; // searchparams.pageNo - 1) * searchparams.pageSize;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                cmd.Connection = conn;

                /*Based on active tab stored procedure will call
                    1. SP_SearchTicketData_ByDate
                    2. SP_SearchTicketData_ByCustomerType
                    3. SP_SearchTicketData_ByTicketType
                    4. SP_SearchTicketData_ByCategoryType
                    5. SP_SearchTicketData_ByAll                 
                 */
                MySqlCommand sqlcmd = new MySqlCommand("", conn);

                sqlcmd.Parameters.AddWithValue("HeaderStatus_Id", searchModel.HeaderStatusId);

                if (searchModel.ActiveTabId == 1)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByDate";

                    sqlcmd.Parameters.AddWithValue("Ticket_CreatedOn", string.IsNullOrEmpty(searchModel.searchDataByDate.Ticket_CreatedOn) ? "" : searchModel.searchDataByDate.Ticket_CreatedOn);
                    sqlcmd.Parameters.AddWithValue("Ticket_ModifiedOn", string.IsNullOrEmpty(searchModel.searchDataByDate.Ticket_ModifiedOn) ? "" : searchModel.searchDataByDate.Ticket_ModifiedOn);
                    sqlcmd.Parameters.AddWithValue("SLA_DueON", searchModel.searchDataByDate.SLA_DueON);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByDate.Ticket_StatusID);
                }
                else if (searchModel.ActiveTabId == 2)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByCustomerType";

                    sqlcmd.Parameters.AddWithValue("CustomerMobileNo", string.IsNullOrEmpty(searchModel.searchDataByCustomerType.CustomerMobileNo) ? "" : searchModel.searchDataByCustomerType.CustomerMobileNo);
                    sqlcmd.Parameters.AddWithValue("CustomerEmailID", string.IsNullOrEmpty(searchModel.searchDataByCustomerType.CustomerEmailID) ? "" : searchModel.searchDataByCustomerType.CustomerEmailID);
                    sqlcmd.Parameters.AddWithValue("Ticket_ID", searchModel.searchDataByCustomerType.TicketID == null ? 0 : searchModel.searchDataByCustomerType.TicketID);
                    sqlcmd.Parameters.AddWithValue("TicketStatusID", searchModel.searchDataByCustomerType.TicketStatusID);
                }
                else if (searchModel.ActiveTabId == 3)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByTicketType";

                    sqlcmd.Parameters.AddWithValue("Priority_Id", searchModel.searchDataByTicketType.TicketPriorityID);
                    sqlcmd.Parameters.AddWithValue("TicketStatusID", searchModel.searchDataByTicketType.TicketStatusID);
                    sqlcmd.Parameters.AddWithValue("ChannelOfPurchaseIDs", string.IsNullOrEmpty(searchModel.searchDataByTicketType.ChannelOfPurchaseIds) ? "" : searchModel.searchDataByTicketType.ChannelOfPurchaseIds);
                    sqlcmd.Parameters.AddWithValue("ActionTypeIds", searchModel.searchDataByTicketType.ActionTypes);
                }
                else if (searchModel.ActiveTabId == 4)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByCategory";

                    sqlcmd.Parameters.AddWithValue("Category_Id", searchModel.searchDataByCategoryType.CategoryId);
                    sqlcmd.Parameters.AddWithValue("SubCategory_Id", searchModel.searchDataByCategoryType.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("IssueType_Id", searchModel.searchDataByCategoryType.IssueTypeId);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByCategoryType.TicketStatusID);
                }
                else if (searchModel.ActiveTabId == 5)
                {
                    sqlcmd.CommandText = "SP_SearchTicketData_ByAll";

                    /*Column 1 (5)*/
                    sqlcmd.Parameters.AddWithValue("Ticket_CreatedOn", string.IsNullOrEmpty(searchModel.searchDataByAll.CreatedDate) ? "" : searchModel.searchDataByAll.CreatedDate);
                    sqlcmd.Parameters.AddWithValue("Ticket_ModifiedOn", string.IsNullOrEmpty(searchModel.searchDataByAll.ModifiedDate) ? "" : searchModel.searchDataByAll.ModifiedDate);
                    sqlcmd.Parameters.AddWithValue("Category_Id", searchModel.searchDataByAll.CategoryId);
                    sqlcmd.Parameters.AddWithValue("SubCategory_Id", searchModel.searchDataByAll.SubCategoryId);
                    sqlcmd.Parameters.AddWithValue("IssueType_Id", searchModel.searchDataByAll.IssueTypeId);

                    /*Column 2 (5) */
                    sqlcmd.Parameters.AddWithValue("TicketSourceType_ID", searchModel.searchDataByAll.TicketSourceTypeID);
                    sqlcmd.Parameters.AddWithValue("TicketIdORTitle", string.IsNullOrEmpty(searchModel.searchDataByAll.TicketIdORTitle) ? "" : searchModel.searchDataByAll.TicketIdORTitle);
                    sqlcmd.Parameters.AddWithValue("Priority_Id", searchModel.searchDataByAll.PriorityId);
                    sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.searchDataByAll.TicketSatutsID);
                    sqlcmd.Parameters.AddWithValue("SLAStatus", string.IsNullOrEmpty(searchModel.searchDataByAll.SLAStatus) ? "" : searchModel.searchDataByAll.SLAStatus);

                    /*Column 3 (5)*/
                    sqlcmd.Parameters.AddWithValue("TicketClaim_ID", searchModel.searchDataByAll.ClaimId);
                    sqlcmd.Parameters.AddWithValue("InvoiceNumberORSubOrderNo", string.IsNullOrEmpty(searchModel.searchDataByAll.InvoiceNumberORSubOrderNo) ? "" : searchModel.searchDataByAll.InvoiceNumberORSubOrderNo);
                    sqlcmd.Parameters.AddWithValue("OrderItemId", searchModel.searchDataByAll.OrderItemId);

                    /*All for to load all the data*/
                    if (searchModel.searchDataByAll.IsVisitStore.ToLower() != "all")
                        sqlcmd.Parameters.AddWithValue("IsVisitedStore", searchModel.searchDataByAll.IsVisitStore == "yes" ? 1 : 0);
                    else
                        sqlcmd.Parameters.AddWithValue("IsVisitedStore", -1);

                    if (searchModel.searchDataByAll.IsWantVistingStore.ToLower() != "all")
                        sqlcmd.Parameters.AddWithValue("IsWantToVisitStore", searchModel.searchDataByAll.IsWantVistingStore == "yes" ? 1 : 0);
                    else
                        sqlcmd.Parameters.AddWithValue("IsWantToVisitStore", -1);

                    /*Column 4 (5)*/
                    sqlcmd.Parameters.AddWithValue("Customer_EmailID", searchModel.searchDataByAll.CustomerEmailID);
                    sqlcmd.Parameters.AddWithValue("CustomerMobileNo", string.IsNullOrEmpty(searchModel.searchDataByAll.CustomerMobileNo) ? "" : searchModel.searchDataByAll.CustomerMobileNo);
                    sqlcmd.Parameters.AddWithValue("OtherAgentAssignTo", string.IsNullOrEmpty(Convert.ToString(searchModel.searchDataByAll.AssignTo)) ? 0 : Convert.ToInt32(searchModel.searchDataByAll.AssignTo));
                    sqlcmd.Parameters.AddWithValue("StoreCodeORAddress", searchModel.searchDataByAll.StoreCodeORAddress);
                    sqlcmd.Parameters.AddWithValue("WantToStoreCodeORAddress", string.IsNullOrEmpty(searchModel.searchDataByAll.WantToStoreCodeORAddress) ? "" : searchModel.searchDataByAll.WantToStoreCodeORAddress);

                    //Row - 2 and Column - 1  (5)
                    sqlcmd.Parameters.AddWithValue("HaveClaim", searchModel.searchDataByAll.HaveClaim);
                    sqlcmd.Parameters.AddWithValue("ClaimStatusId", searchModel.searchDataByAll.ClaimStatusId);
                    sqlcmd.Parameters.AddWithValue("ClaimCategoryId", searchModel.searchDataByAll.ClaimCategoryId);
                    sqlcmd.Parameters.AddWithValue("ClaimSubCategoryId", searchModel.searchDataByAll.ClaimSubCategoryId);
                    sqlcmd.Parameters.AddWithValue("ClaimIssueTypeId", searchModel.searchDataByAll.ClaimIssueTypeId);

                    //Row - 2 and Column - 2  (4)
                    sqlcmd.Parameters.AddWithValue("HaveTask", searchModel.searchDataByAll.HaveTask);
                    sqlcmd.Parameters.AddWithValue("TaskStatus_Id", searchModel.searchDataByAll.TaskStatusId);
                    sqlcmd.Parameters.AddWithValue("TaskDepartment_Id", searchModel.searchDataByAll.TaskDepartment_Id);
                    sqlcmd.Parameters.AddWithValue("TaskFunction_Id", searchModel.searchDataByAll.TaskFunction_Id);
                }

                sqlcmd.Parameters.AddWithValue("Tenant_ID", searchModel.TenantID);
                sqlcmd.Parameters.AddWithValue("Assignto_Id", searchModel.AssigntoId);

                sqlcmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = sqlcmd;
                da.Fill(ds);

                if (ds != null && ds.Tables != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SearchResponse obj = new SearchResponse()
                            {
                                ticketID = dr["TicketID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TicketID"]),
                                ticketStatus = dr["StatusID"] == DBNull.Value ? String.Empty : Convert.ToString((EnumMaster.TicketStatus)Convert.ToInt32(dr["StatusID"])),
                                Message = dr["TicketDescription"] == DBNull.Value ? String.Empty : Convert.ToString(dr["TicketDescription"]),
                                Category = dr["CategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["CategoryName"]),
                                subCategory = dr["SubCategoryName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["SubCategoryName"]),
                                IssueType = dr["IssueTypeName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["IssueTypeName"]),
                                Priority = dr["PriortyName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["PriortyName"]),
                                Assignee = dr["AssignedName"] == DBNull.Value ? String.Empty : Convert.ToString(dr["AssignedName"]),
                                CreatedOn = dr["CreatedOn"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedOn"]),
                                createdBy = dr["CreatedByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByName"]),
                                createdago = dr["CreatedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["CreatedDate"]), "CreatedSpan"),
                                assignedTo = dr["AssignedName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedName"]),
                                assignedago = dr["AssignedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["AssignedDate"]), "AssignedSpan"),
                                updatedBy = dr["ModifyByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifyByName"]),
                                updatedago = dr["ModifiedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["ModifiedDate"]), "ModifiedSpan"),

                                responseTimeRemainingBy = (dr["AssignedDate"] == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(dr["PriorityRespond"]))) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "RespondTimeRemainingSpan"),
                                responseOverdueBy = (dr["AssignedDate"] == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(dr["PriorityRespond"]))) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResponseOverDueSpan"),

                                resolutionOverdueBy = (dr["AssignedDate"] == DBNull.Value || string.IsNullOrEmpty(Convert.ToString(dr["PriorityResolve"]))) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityResolve"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResolutionOverDueSpan"),

                                TaskStatus = dr["TaskDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TaskDetails"]),
                                ClaimStatus = dr["ClaimDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ClaimDetails"]),
                                TicketCommentCount = dr["TicketComments"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TicketComments"]),
                                isEscalation = dr["IsEscalated"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IsEscalated"]),
                                ticketSourceType = dr["TicketSourceType"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TicketSourceType"]),
                                ticketSourceTypeID = dr["TicketSourceTypeID"] == DBNull.Value ? 0 : Convert.ToInt16(dr["TicketSourceTypeID"]),
                                IsReassigned = dr["IsReassigned"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsReassigned"]),
                                IsSLANearBreach = dr["IsSLANearBreach"] == DBNull.Value ? false : Convert.ToBoolean(dr["IsSLANearBreach"])
                            };

                            objSearchResult.Add(obj);
                        }
                    }
                }

                //paging here
                //if (searchparams.pageSize > 0 && objSearchResult.Count > 0)
                //    objSearchResult[0].totalpages = objSearchResult.Count > searchparams.pageSize ? Math.Round(Convert.ToDouble(objSearchResult.Count / searchparams.pageSize)) : 1;

                //objSearchResult = objSearchResult.Skip(rowStart).Take(searchparams.pageSize).ToList();
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
                //throw ex;
            }
            finally
            {
                if (ds != null) ds.Dispose(); conn.Close();
            }
            return objSearchResult;
        }

        #endregion

        #region  ReportService

        public List<SearchResponseReport> GetReportSearch(ReportSearchModel searchModel)
        {
            DataSet ds = new DataSet();
            MySqlCommand cmd = new MySqlCommand();
            List<SearchResponseReport> objSearchResult = new List<SearchResponseReport>();

            List<string> CountList = new List<string>();

            int resultCount = 0; // searchparams.pageNo - 1) * searchparams.pageSize;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                cmd.Connection = conn;

                /*Based on active tab stored procedure will call
                    1. SP_SearchTicketData_ByDate
                    2. SP_SearchTicketData_ByCustomerType
                    3. SP_SearchTicketData_ByTicketType
                    4. SP_SearchTicketData_ByCategoryType
                    5. SP_SearchTicketData_ByAll                 
                 */
                MySqlCommand sqlcmd = new MySqlCommand("", conn);

                // sqlcmd.Parameters.AddWithValue("HeaderStatus_Id", searchModel.HeaderStatusId);
                // sqlcmd.CommandText = "SP_SearchReportData";

                sqlcmd.CommandText = "SP_Report_SchedulerSearch";

                /*Column 1 (5)*/
                sqlcmd.Parameters.AddWithValue("Ticket_CreatedOn", string.IsNullOrEmpty(searchModel.reportSearch.CreatedDate) ? "" : searchModel.reportSearch.CreatedDate);
                sqlcmd.Parameters.AddWithValue("Ticket_ModifiedOn", string.IsNullOrEmpty(searchModel.reportSearch.ModifiedDate) ? "" : searchModel.reportSearch.ModifiedDate);
                sqlcmd.Parameters.AddWithValue("Category_Id", searchModel.reportSearch.CategoryId);
                sqlcmd.Parameters.AddWithValue("SubCategory_Id", searchModel.reportSearch.SubCategoryId);
                sqlcmd.Parameters.AddWithValue("IssueType_Id", searchModel.reportSearch.IssueTypeId);

                /*Column 2 (5) */
                sqlcmd.Parameters.AddWithValue("TicketSourceType_ID", searchModel.reportSearch.TicketSourceTypeID);
                sqlcmd.Parameters.AddWithValue("TicketIdORTitle", string.IsNullOrEmpty(searchModel.reportSearch.TicketIdORTitle) ? "" : searchModel.reportSearch.TicketIdORTitle);
                sqlcmd.Parameters.AddWithValue("Priority_Id", searchModel.reportSearch.PriorityId);
                sqlcmd.Parameters.AddWithValue("Ticket_StatusID", searchModel.reportSearch.TicketSatutsID);
                sqlcmd.Parameters.AddWithValue("SLAStatus", string.IsNullOrEmpty(searchModel.reportSearch.SLAStatus) ? "" : searchModel.reportSearch.SLAStatus);

                /*Column 3 (5)*/
                sqlcmd.Parameters.AddWithValue("TicketClaim_ID", Convert.ToInt32(searchModel.reportSearch.ClaimId == "" ? "0" : searchModel.reportSearch.ClaimId));
                sqlcmd.Parameters.AddWithValue("InvoiceNumberORSubOrderNo", string.IsNullOrEmpty(searchModel.reportSearch.InvoiceNumberORSubOrderNo) ? "" : searchModel.reportSearch.InvoiceNumberORSubOrderNo);
                sqlcmd.Parameters.AddWithValue("OrderItemId", string.IsNullOrEmpty(Convert.ToString(searchModel.reportSearch.OrderItemId)) ? 0 : Convert.ToInt32(searchModel.reportSearch.OrderItemId));
                sqlcmd.Parameters.AddWithValue("IsVisitedStore", searchModel.reportSearch.IsVisitStore == "yes" ? 1 : 0);
                sqlcmd.Parameters.AddWithValue("IsWantToVisitStore", searchModel.reportSearch.IsWantVistingStore == "yes" ? 1 : 0);

                /*Column 4 (5)*/
                sqlcmd.Parameters.AddWithValue("Customer_EmailID", searchModel.reportSearch.CustomerEmailID);
                sqlcmd.Parameters.AddWithValue("CustomerMobileNo", string.IsNullOrEmpty(searchModel.reportSearch.CustomerMobileNo) ? "" : searchModel.reportSearch.CustomerMobileNo);
                sqlcmd.Parameters.AddWithValue("AssignTo", searchModel.reportSearch.AssignTo);
                sqlcmd.Parameters.AddWithValue("StoreCodeORAddress", searchModel.reportSearch.StoreCodeORAddress);
                sqlcmd.Parameters.AddWithValue("WantToStoreCodeORAddress", searchModel.reportSearch.WantToStoreCodeORAddress);

                //Row - 2 and Column - 1  (5)
                sqlcmd.Parameters.AddWithValue("HaveClaim", searchModel.reportSearch.HaveClaim);
                sqlcmd.Parameters.AddWithValue("ClaimStatusId", searchModel.reportSearch.ClaimStatusId);
                sqlcmd.Parameters.AddWithValue("ClaimCategoryId", searchModel.reportSearch.ClaimCategoryId);
                sqlcmd.Parameters.AddWithValue("ClaimSubCategoryId", searchModel.reportSearch.ClaimSubCategoryId);
                sqlcmd.Parameters.AddWithValue("ClaimIssueTypeId", searchModel.reportSearch.ClaimIssueTypeId);

                //Row - 2 and Column - 2  (4)
                sqlcmd.Parameters.AddWithValue("HaveTask", searchModel.reportSearch.HaveTask);
                sqlcmd.Parameters.AddWithValue("TaskStatus_Id", searchModel.reportSearch.TaskStatusId);
                sqlcmd.Parameters.AddWithValue("TaskDepartment_Id", searchModel.reportSearch.TaskDepartment_Id);
                sqlcmd.Parameters.AddWithValue("TaskFunction_Id", searchModel.reportSearch.TaskFunction_Id);
                //     sqlcmd.Parameters.AddWithValue("Task_Priority", searchModel.reportSearch.TaskPriority);

                sqlcmd.Parameters.AddWithValue("CurrentUserId", searchModel.curentUserId);
                sqlcmd.Parameters.AddWithValue("Tenant_ID", searchModel.TenantID);
                sqlcmd.Parameters.AddWithValue("Assignto_IDs", searchModel.reportSearch.AssignTo.ToString());
                sqlcmd.Parameters.AddWithValue("Brand_IDs", searchModel.reportSearch.BrandID.ToString());

                sqlcmd.CommandType = CommandType.StoredProcedure;

                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = sqlcmd;
                da.Fill(ds);

                if (ds != null && ds.Tables != null)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        // resultCount = Convert.ToInt32(ds.Tables[0].Rows[0]["RowCount"]);
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            SearchResponseReport obj = new SearchResponseReport()
                            {
                                ticketID = Convert.ToInt32(dr["TicketID"]),
                                ticketStatus = Convert.ToString((EnumMaster.TicketStatus)Convert.ToInt32(dr["StatusID"])),
                                Message = dr["TicketDescription"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TicketDescription"]),
                                Category = dr["CategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CategoryName"]),
                                subCategory = dr["SubCategoryName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["SubCategoryName"]),
                                IssueType = dr["IssueTypeName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["IssueTypeName"]),
                                Priority = dr["PriortyName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["PriortyName"]),
                                Assignee = dr["AssignedName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedName"]),
                                CreatedOn = dr["CreatedOn"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedOn"]),
                                createdBy = dr["CreatedByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["CreatedByName"]),
                                createdago = dr["CreatedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["CreatedDate"]), "CreatedSpan"),
                                assignedTo = dr["AssignedName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["AssignedName"]),
                                assignedago = dr["AssignedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["AssignedDate"]), "AssignedSpan"),
                                updatedBy = dr["ModifyByName"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ModifyByName"]),
                                updatedago = dr["ModifiedDate"] == DBNull.Value ? string.Empty : SetCreationdetails(Convert.ToString(dr["ModifiedDate"]), "ModifiedSpan"),

                                responseTimeRemainingBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityRespond"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "RespondTimeRemainingSpan"),
                                responseOverdueBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityRespond"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityRespond"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResponseOverDueSpan"),

                                resolutionOverdueBy = (dr["AssignedDate"] == DBNull.Value || dr["PriorityResolve"] == DBNull.Value) ?
                            string.Empty : SetCreationdetails(Convert.ToString(dr["PriorityResolve"]) + "|" + Convert.ToString(dr["AssignedDate"]), "ResolutionOverDueSpan"),

                                TaskStatus = dr["TaskDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["TaskDetails"]),
                                ClaimStatus = dr["ClaimDetails"] == DBNull.Value ? string.Empty : Convert.ToString(dr["ClaimDetails"]),
                                TicketCommentCount = dr["ClaimDetails"] == DBNull.Value ? 0 : Convert.ToInt32(dr["TicketComments"]),
                                isEscalation = dr["IsEscalated"] == DBNull.Value ? 0 : Convert.ToInt32(dr["IsEscalated"]),
                                ticketSourceType = Convert.ToString(dr["TicketSourceType"]),
                                IsReassigned = Convert.ToBoolean(dr["IsReassigned"]),
                                ticketSourceTypeID = Convert.ToInt16(dr["TicketSourceTypeID"])
                            };
                            objSearchResult.Add(obj);
                        }
                    }
                }
                // return resultCount;
                //paging here
                //if (searchparams.pageSize > 0 && objSearchResult.Count > 0)
                //    objSearchResult[0].totalpages = objSearchResult.Count > searchparams.pageSize ? Math.Round(Convert.ToDouble(objSearchResult.Count / searchparams.pageSize)) : 1;

                //objSearchResult = objSearchResult.Skip(rowStart).Take(searchparams.pageSize).ToList();
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (ds != null) ds.Dispose(); conn.Close();
            }
            return objSearchResult;
        }

        #endregion

        public SMTPDetails GetSMTPDetails(int TenantID)
        {
            DataSet ds = new DataSet();
            SMTPDetails sMTPDetails = new SMTPDetails();

            try
            {
                MySqlCommand cmd = new MySqlCommand();

                conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_getSMTPDetails", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Tenant_ID", TenantID);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    sMTPDetails.EnableSsl = Convert.ToBoolean(ds.Tables[0].Rows[0]["EnabledSSL"]);
                    sMTPDetails.SMTPPort = Convert.ToString(ds.Tables[0].Rows[0]["SMTPPort"]);
                    sMTPDetails.FromEmailId = Convert.ToString(ds.Tables[0].Rows[0]["EmailUserID"]);
                    sMTPDetails.EmailSenderName = Convert.ToString(ds.Tables[0].Rows[0]["EmailSenderName"]);
                    sMTPDetails.IsBodyHtml = Convert.ToBoolean(ds.Tables[0].Rows[0]["IsBodyHtml"]);
                    sMTPDetails.Password = Convert.ToString(ds.Tables[0].Rows[0]["EmailPassword"]);
                    sMTPDetails.SMTPHost = Convert.ToString(ds.Tables[0].Rows[0]["SMTPHost"]);
                    sMTPDetails.SMTPServer = Convert.ToString(ds.Tables[0].Rows[0]["SMTPHost"]);
                }
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (ds != null)
                    ds.Dispose(); conn.Close();
            }

            return sMTPDetails;
        }

        public void GetMailContent(TicketScheduleModal ticketschedulemodal)
        {
            DataSet ds = new DataSet();
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                 conn.Open();
                cmd.Connection = conn;
                MySqlCommand cmd1 = new MySqlCommand("SP_getMailContent", conn);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@Alert_TypeID", ticketschedulemodal.Alert_TypeID);
                cmd1.Parameters.AddWithValue("@_ScheduleID", ticketschedulemodal.ScheduleID);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    ticketschedulemodal.Emailbody =  Convert.ToString(ds.Tables[0].Rows[0]["Content"]);
                   // ticketschedulemodal.Emailbody = ticketschedulemodal.Emailbody.Replace("@ScheduledBy", ticketschedulemodal.CreatedByFirstName + " " + ticketschedulemodal.CreatedByLastName);
                   // ticketschedulemodal.Emailbody = ticketschedulemodal.Emailbody.Replace("@ScheduledTime", ticketschedulemodal.ScheduleTime);

                    ticketschedulemodal.Emailsubject = Convert.ToString(ds.Tables[0].Rows[0]["Subject"]);
                }
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {

                if (ds != null)
                    ds.Dispose(); conn.Close();
            }

        }

        public void SchedulerMailResult(TicketScheduleModal ticketschedulemodal, bool isSend, string SchedulerType, string InnerExceptions, string Message, string StackTrace, string StatusCode = "")
        {
            try
            {
                string exceptionmsg = "InnerExceptions: " + InnerExceptions + ", Message: " + Message + ", StackTrace: " + StackTrace + ", StatusCode: " + StatusCode;
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SP_InsertSchedulerMailResult", conn);
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@_EMailID", ticketschedulemodal.CreatedByEmailId);
                cmd.Parameters.AddWithValue("@_SchedulerID", ticketschedulemodal.ScheduleID);
                cmd.Parameters.AddWithValue("@_SchedulerType", SchedulerType);
                cmd.Parameters.AddWithValue("@_IsSend", isSend);
                cmd.Parameters.AddWithValue("@_Message", exceptionmsg);
                cmd.CommandType = CommandType.StoredProcedure;
                int updatecount = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}
