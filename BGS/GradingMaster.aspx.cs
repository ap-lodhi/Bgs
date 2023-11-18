using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Threading;
using System.Globalization;

namespace BGS
{
    public partial class GradingMaster : PageBase
    {
        #region Properties
        Datalayer dl = new Datalayer();
        BGSEntities bgs_entity = new BGSEntities();
        public static string NewQuarterName;
        public static string StartDate;
        public static string EndDate;

        public string sQuaterId
        {
            get { return !string.IsNullOrEmpty(Convert.ToString(Request.Form[hfQuarterId.UniqueID])) ? Request.Form[hfQuarterId.UniqueID].ToString() : "0"; }
            set { Request.Form[hfQuarterId.UniqueID] = value; }
        }
        public string QuarterId
        {
            get
            {
                return ViewState["QuarterId"] != null ? (string)ViewState["QuarterId"] : "0";
            }
            set
            {
                ViewState["QuarterId"] = value;
            }
        }
        public string QuarterName
        {
            get
            {
                return ViewState["QuarterName"] != null ? (string)ViewState["QuarterName"] : "0";
            }
            set
            {
                ViewState["QuarterName"] = value;
            }
        }
        public int IsRankSent
        {
            get
            {
                return ViewState["IsRankSent"] != null ? (int)ViewState["IsRankSent"] : 0;
            }
            set
            {
                ViewState["IsRankSent"] = value;
            }
        }
        #endregion

        int yrdiff = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Monthbind();
                RankSent();
                GetQuarterNameDueDate();
                GetGradingMaster(null);
                PopulateDepartments();
                // txtQuarter.Attributes.Add("onchange", "javascript:ConfirmMessage();");//Comment by shashi 19 June 2018
                //ddlStartDate.Attributes.Add("onchange", "javascript:ConfirmMessage();");
                selectDropdownQuarter();
                DeptWiseRecord(btnDept, new EventArgs());
            }
        }
        private void Monthbind()
        {
            var Query = (from C in bgs_entity.BGS_GetQuarterNameDuteDate()
                         select C).FirstOrDefault();
            yrdiff = 0;
            int cnt_startdt = 0;
            int cnt_enddt = 0;

            if (Query != null)
            {
                //ddlStartDate.Items.FindByText(Convert.ToDateTime(Query.EndDate).AddMonths(1).ToString("MMM yyyy")).Selected = true;
                int enddate = Convert.ToInt32(Query.EndDate.ToString().Substring(Query.EndDate.Count() - 4, 4));
                yrdiff = DateTime.Now.Year - enddate;

                DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
                for (int i = 1; i < 13; i++)
                {
                    cnt_startdt = cnt_startdt + 1;
                    ddlStartDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (DateTime.Now.Year - yrdiff), i.ToString()));
                }
                for (int i = 1; i < 13; i++)
                {
                    cnt_enddt = cnt_enddt + 1;
                    ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (DateTime.Now.Year - yrdiff), i.ToString()));
                }
                //var Current = DateTime.Now.AddYears(1).ToString("yyyy");
                var Current = DateTime.Now.ToString("yyyy");
                var StartCurrent = DateTime.Now.AddYears(0).ToString("yyyy");

                for (int i = 1; i < 13; i++)
                {

                    ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (StartCurrent), i.ToString() + "_" + StartCurrent));
                    //ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Current)));

                }
                //var StartCurrent = DateTime.Now.AddYears(1).ToString("yyyy");
                for (int i = 1; i < 13; i++)
                {
                    ddlStartDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (StartCurrent), i.ToString() + "_" + StartCurrent));
                    // ddlStartDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Current)));
                    //ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (StartCurrent), i.ToString() + "_" + StartCurrent));
                }

                //for (int i = 1; i < 13; i++)
                //{
                //    cnt_enddt = cnt_enddt + 1;
                //    ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Current), cnt_enddt.ToString()));

                //}
                //var StartCurrent = DateTime.Now.AddYears(1).ToString("yyyy");
                //for (int i = 1; i < 13; i++)
                //{
                //    cnt_startdt = cnt_startdt + 1;
                //    ddlStartDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Current), cnt_startdt.ToString()));
                //}

            }
        }
        protected void selectDropdownQuarter()

        {
            var query = bgs_entity.BGS_AuthenticateGrading();
            int count = Convert.ToInt32(query.FirstOrDefault().Value);
            if (count == 0)
            {
                SetQuarter();
            }
            else
            {
                //Deependra Comment  ddlStartDate.Enabled = false;
                //Deependra Comment   ddlEndDate.Enabled = false;
                GetQuarterNameDueDate();
            }
        }
        protected void SetQuarter()
        {

            //Deependra --This SP getting null Start date and End date 
            var Query = (from C in bgs_entity.BGS_GetQuarterNameDuteDate()
                         select C).FirstOrDefault();
            if (Query != null)
            {
                //txtQuarter.Text = Query.Name;
                txtDueDate.Text = Convert.ToString(Query.DueDate);
                this.QuarterId = Convert.ToString(Query.QuarterID);
                this.QuarterName = Query.Name;
                ddlStartDate.ClearSelection();
                ddlEndDate.ClearSelection();
                if (Convert.ToString(Query.StartDate) != null)
                {

                    ddlStartDate.Items.FindByText(Convert.ToDateTime(hfStartDate.Value).ToString("MMM yyyy")).Selected = true;
                    //if (Convert.ToString(Convert.ToDateTime(Query.StartDate).ToString("yyyy")) == (DateTime.Now.Year -yrdiff) .ToString())
                    //{
                    //    ddlStartDate.Items.FindByText(Convert.ToDateTime(Query.StartDate).AddMonths(1).ToString("MMM yyyy")).Selected = true;
                    //    //ddlStartDate.SelectedItem.Text = Query.EndDate;
                    //}
                    //else
                    //    ddlStartDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                    //    //ddlStartDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);
                }
                else
                    ddlStartDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                //ddlStartDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);

                if (Convert.ToString(Query.EndDate) != null)
                {
                    ddlEndDate.Items.FindByText(Convert.ToDateTime(hfEndDate.Value).ToString("MMM yyyy")).Selected = true;

                    //if (Convert.ToString(Convert.ToDateTime(Query.EndDate).ToString("yyyy")) == (DateTime.Now.Year-yrdiff) .ToString())
                    //{
                    //    ddlEndDate.Items.FindByText(Convert.ToDateTime(Query.EndDate).AddMonths(1).ToString("MMM yyyy")).Selected = true;
                    //    //ddlEndDate.SelectedItem.Text = Query.EndDate;
                    //}
                    //else
                    //{
                    //    ddlEndDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                    //    //ddlEndDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);
                    //}
                }


                else
                    ddlEndDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                //ddlEndDate.SelectedItem.Text= string.Format("{0:MMM yyyy}", DateTime.Now);

                //hfStartDate.Value = ddlStartDate.SelectedItem.Text;
                //hfEndDate.Value = ddlEndDate.SelectedItem.Text;
                //hfDueDate.Value = txtDueDate.Text;
                int month = (Convert.ToDateTime(Query.EndDate).AddMonths(1)).Month;
                int Year = (Convert.ToDateTime(Query.EndDate).AddMonths(1)).Year;

                //DateTimeFormatInfo info = DateTimeFormatInfo.GetInstance(null);
                ////======================remove End Date====================
                //List<ListItem> StartdeletedItems = new List<ListItem>();
                //foreach (ListItem item in ddlStartDate.Items)
                //{
                //    StartdeletedItems.Add(item);

                //}
                //foreach (ListItem item in StartdeletedItems)
                //{
                //    ddlStartDate.Items.Remove(item);
                //}

                //for (int i = month; i < 13; i++)
                //{
                //    //deependra--because table have null value than its not displaying year
                //    ddlStartDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Year), i.ToString()));
                //}
                ////=========================remove End Date====================

                //List<ListItem> EnddeletedItems = new List<ListItem>();
                //foreach (ListItem item in ddlEndDate.Items)
                //{
                //    EnddeletedItems.Add(item);
                //}
                //foreach (ListItem item in EnddeletedItems)
                //{
                //    ddlEndDate.Items.Remove(item);
                //}
                //for (int i = month; i < 13; i++)
                //{
                //    ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Year), i.ToString()));
                //}
                //var Current = DateTime.Now.AddYears(1).ToString("yyyy");
                //for (int i = 1; i < month; i++)
                //{
                //    ddlEndDate.Items.Add(new ListItem(info.GetAbbreviatedMonthName(i) + " " + (Current), i.ToString()));

                //}
                ////for (int i = -11; i <= 0; i++)
                ////{
                ////    var d = DateTime.Now.AddMonths(i);
                ////    var dStartingFromDayOne = new DateTime(d.Year, d.Month, 1);
                ////    var ds = dStartingFromDayOne.ToString("MMMM yyyy");
                ////    Console.WriteLine(ds);
                ////}
                //// Monthbind();
            }
        }
        //Deependra-- Get quater name and create new quater if date is proper available
        protected void Go_Click(object sender, EventArgs e)
        {
            try
            {
                ddlStartDate.Enabled = false;
                ddlEndDate.Enabled = false;
                int start_dt_val;
                int end_dt_val;
                ////string a = ddlStartDate.SelectedValue;
                //if (Convert.ToInt32(ddlStartDate.SelectedItem.Value)>12)
                //{
                //    ddlStartDate.SelectedItem.Value = Convert.ToString(Convert.ToInt32(ddlStartDate.SelectedItem.Value) - 12);
                //}
                //if(Convert.ToInt32(ddlEndDate.SelectedItem.Value)>12)
                //{
                //    ddlEndDate.SelectedItem.Value = Convert.ToString(Convert.ToInt32(ddlEndDate.SelectedItem.Value) - 12);
                //}


                int nStartMonth = Convert.ToInt32(ddlStartDate.SelectedItem.Value.Split('_')[0]);
                string startmonth = ddlStartDate.SelectedItem.Text;
                //string s= ddlStartDate.SelectedValue;
                //int s1 = ddlStartDate.SelectedIndex;

                int StartYear = Convert.ToInt32(startmonth.Substring(startmonth.Length - 4));
                var firstDayOfMonthStart = new DateTime(StartYear, nStartMonth, 1).ToString("dd");
                StartDate = firstDayOfMonthStart + "-" + startmonth.Substring(0, 3) + "-" + startmonth.Substring(startmonth.Length - 4);

                int nEndMonth = Convert.ToInt32(ddlEndDate.SelectedItem.Value.Split('_')[0]);
                string endmonth = ddlEndDate.SelectedItem.Text;
                EndDate = new DateTime(DateTime.Now.Year, nEndMonth, 1).AddMonths(1).AddDays(-1).Day + "-" + endmonth.Substring(0, 3) + "-" + endmonth.Substring(endmonth.Length - 4);

                DataTable dt = new DataTable();

                dt = dl.GenarateQuarterByStartEndDate(StartDate, EndDate);
                if (dt.Rows[0]["QuarterName"].ToString() != "")
                {
                    NewQuarterName = dt.Rows[0]["QuarterName"].ToString();
                    try
                    {
                        var query = bgs_entity.BGS_AuthenticateGrading();

                        int count = Convert.ToInt32(query.FirstOrDefault().Value);

                        //if (hfQuarter.Value == "Yes")
                        //{
                        if (count == 0)
                        {
                            string LastQyear = string.Empty;
                            string LastQuarter = string.Empty;
                            string LastYear = string.Empty;
                            string Currentyear = string.Empty;
                            string CurrentQuarter = string.Empty;
                            string getCurrentYear = string.Empty;
                            string[] LastQuarterArr = this.QuarterName.Split('_');
                            LastQyear = LastQuarterArr[1].ToString();
                            LastQuarter = LastQuarterArr[2].ToString().Replace("Q", "");
                            string[] CurrentQuarterArr = NewQuarterName.Split('_');
                            Currentyear = CurrentQuarterArr[1].ToString();
                            CurrentQuarter = CurrentQuarterArr[2].ToString().Replace("Q", "");
                            getCurrentYear = (DateTime.Now.Year).ToString();
                            if (Convert.ToInt32(CurrentQuarter) <= 4 && getCurrentYear == Currentyear)
                            {

                                if (LastQyear == Currentyear && Convert.ToInt32(CurrentQuarter) < Convert.ToInt32(LastQuarter))
                                {

                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);
                                    ddlStartDate.ClearSelection();
                                    ddlEndDate.ClearSelection();
                                    GetQuarterNameDueDate();
                                    //ddlStartDate.Enabled = true;
                                    //ddlEndDate.Enabled = true;
                                }
                                else if (LastQyear != Currentyear && Convert.ToInt32(CurrentQuarter) > 4)
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);


                                    ddlStartDate.ClearSelection();
                                    ddlEndDate.ClearSelection();
                                    GetQuarterNameDueDate();



                                    //ddlStartDate.Enabled = true;
                                    //ddlEndDate.Enabled = true;
                                }
                                else
                                {
                                    var result = DateTime.Compare(Convert.ToDateTime(txtDueDate.Text), DateTime.Today); //28-09-2022 changes by anshu
                                    //var result = DateTime.Compare(DateTime.Today, Convert.ToDateTime(txtDueDate.Text));
                                    if (result <= 0)
                                    {
                                        string message = "alert('Due date should not be less than current date..')";
                                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);

                                        //changes starts here  orig:uncomment code
                                        ddlStartDate.ClearSelection();
                                        ddlEndDate.ClearSelection();
                                        GetQuarterNameDueDate();
                                        //changes ends here 

                                        //ddlStartDate.Enabled = true;
                                        //ddlEndDate.Enabled = true;
                                        //added to display all record after quarter initiate
                                        Monthbind();
                                        RankSent();
                                        GetQuarterNameDueDate();
                                        GetGradingMaster(null);
                                        PopulateDepartments();
                                        selectDropdownQuarter();
                                        DeptWiseRecord(btnDept, new EventArgs());
                                        //ended here
                                    }
                                    else
                                    {
                                        bgs_entity.BGS_InsertNewQuarterGrading(NewQuarterName, Convert.ToDateTime(txtDueDate.Text), Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
                                        RankSent();
                                        GetQuarterNameDueDate();
                                        GetGradingMaster(null);
                                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Saved Successfull now you initiate Value.');", true);
                                        //ddlStartDate.Enabled = false;
                                        //ddlEndDate.Enabled = false;
                                    }

                                }
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid quarter name.');", true);
                                ddlStartDate.ClearSelection();
                                ddlEndDate.ClearSelection();
                                GetQuarterNameDueDate();
                                ddlStartDate.Enabled = true;
                                ddlEndDate.Enabled = true;
                            }

                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Current Quarter voting is not completed So you can not initiate next quarter grading');", true);
                            ddlStartDate.ClearSelection();
                            ddlEndDate.ClearSelection();
                            GetQuarterNameDueDate();
                            //ddlStartDate.Enabled = true;
                            //ddlEndDate.Enabled = true;

                        }
                        //}
                        //else
                        //{
                        //    GetQuarterNameDueDate();
                        //    return;
                        //}
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + ex.Message + "');", true);
                    }
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Already generated Quarter');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Already generated Quarter');", true);
                    return;
                }

            }
            catch (Exception ex)
            {

            }
        }
        //protected void Insert()
        //{
        //    try
        //    {
        //        var query = bgs_entity.BGS_AuthenticateGrading();
        //        int count = query.FirstOrDefault().Value;

        //        //if (hfQuarter.Value == "Yes")
        //        //{
        //            if (count == 0)
        //            {
        //                string LastQyear = string.Empty;
        //                string LastQuarter = string.Empty;
        //                string LastYear = string.Empty;
        //                string Currentyear = string.Empty;
        //                string CurrentQuarter = string.Empty;
        //                string getCurrentYear = string.Empty;
        //                string[] LastQuarterArr = this.QuarterName.Split('_');
        //                LastQyear = LastQuarterArr[1].ToString();
        //                LastQuarter = LastQuarterArr[2].ToString().Replace("Q", "");
        //                string[] CurrentQuarterArr = NewQuarterName.Split('_');
        //                Currentyear = CurrentQuarterArr[1].ToString();
        //                CurrentQuarter = CurrentQuarterArr[2].ToString().Replace("Q", "");
        //                getCurrentYear = (DateTime.Now.Year).ToString();
        //                if (Convert.ToInt32(CurrentQuarter) <= 4 && getCurrentYear == Currentyear)
        //                {

        //                    if (LastQyear == Currentyear && Convert.ToInt32(CurrentQuarter) < Convert.ToInt32(LastQuarter))
        //                    {

        //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);
        //                        GetQuarterNameDueDate();
        //                    }
        //                    else if (LastQyear != Currentyear && Convert.ToInt32(CurrentQuarter) > 4)
        //                    {
        //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);
        //                        GetQuarterNameDueDate();
        //                    }
        //                    else
        //                    {
        //                        //  var result = DateTime.Compare(Convert.ToDateTime(txtDueDate.Text), DateTime.Today);
        //                        var result = DateTime.Compare(Convert.ToDateTime(txtDueDate.Text), DateTime.Today);
        //                        if (result <= 0)
        //                        {
        //                        //this.RegisterStartupScript("scheck", "<script>alert('Invalid due date...');");
        //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"key", "alert('Invalid due date..')", true);
        //                        //ScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "clentscript", "alert('Invalid due date...');", true);
        //                        // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid due date.');", true);
        //                        GetQuarterNameDueDate();
        //                        }
        //                        else
        //                        {
        //                            //int nStartMonth = Convert.ToInt32(ddlStartDate.SelectedItem.Value);
        //                            //string startmonth = ddlStartDate.SelectedItem.Text;
        //                            //string StartDate = new DateTime(DateTime.Now.Year, nStartMonth, 1).AddMonths(1).AddDays(-1).Day + "-" + startmonth.Substring(0, 3) + "-" + DateTime.Now.Year;

        //                            //int nEndMonth = Convert.ToInt32(ddlEndDate.SelectedItem.Value);
        //                            //string endmonth = ddlEndDate.SelectedItem.Text;
        //                            //string EndDate = new DateTime(DateTime.Now.Year, nEndMonth, 1).AddMonths(1).AddDays(-1).Day + "-" + endmonth.Substring(0, 3) + "-" + DateTime.Now.Year;

        //                            bgs_entity.BGS_InsertNewQuarterGrading(NewQuarterName, Convert.ToDateTime(txtDueDate.Text),Convert.ToDateTime(StartDate), Convert.ToDateTime(EndDate));
        //                            RankSent();
        //                            GetQuarterNameDueDate();
        //                            GetGradingMaster(null);
        //                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Saved Successfull now you initiate Value.');", true);
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid quarter name.');", true);
        //                    GetQuarterNameDueDate();
        //                }

        //            }
        //            else
        //            {
        //                GetQuarterNameDueDate();
        //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Current Quarter voting is not completed So you can not initiate next quarter grading');", true);
        //            }
        //        //}
        //        //else
        //        //{
        //        //    GetQuarterNameDueDate();
        //        //    return;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + ex.Message + "');", true);
        //    }
        //}

        //Commented by shashi 25 June 2018
        //protected void txtQuarter_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var query = bgs_entity.BGS_AuthenticateGrading();
        //        int count = query.FirstOrDefault().Value;

        //        if (hfQuarter.Value == "Yes")
        //        {
        //            if (count == 0)
        //            {
        //                string LastQyear = string.Empty;
        //                string LastQuarter = string.Empty;
        //                string LastYear = string.Empty;
        //                string Currentyear = string.Empty;
        //                string CurrentQuarter = string.Empty;
        //                string getCurrentYear = string.Empty;
        //                string[] LastQuarterArr = this.QuarterName.Split('_');
        //                LastQyear = LastQuarterArr[1].ToString();
        //                LastQuarter = LastQuarterArr[2].ToString().Replace("Q", "");
        //                string[] CurrentQuarterArr = txtQuarter.Text.Split('_');
        //                Currentyear = CurrentQuarterArr[1].ToString();
        //                CurrentQuarter = CurrentQuarterArr[2].ToString().Replace("Q", "");
        //                getCurrentYear = (DateTime.Now.Year).ToString();
        //                if (Convert.ToInt32(CurrentQuarter) <= 4 && getCurrentYear == Currentyear)
        //                {

        //                    if (LastQyear == Currentyear && Convert.ToInt32(CurrentQuarter) < Convert.ToInt32(LastQuarter))
        //                    {

        //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);
        //                        GetQuarterNameDueDate();
        //                    }
        //                    else if (LastQyear != Currentyear && Convert.ToInt32(CurrentQuarter) > 4)
        //                    {
        //                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Quarter already exist.');", true);
        //                        GetQuarterNameDueDate();
        //                    }
        //                    else
        //                    {
        //                        var result = DateTime.Compare(Convert.ToDateTime(txtDueDate.Text), DateTime.Today);
        //                        if (result <= 0)
        //                        {
        //                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid due date.');", true);
        //                            GetQuarterNameDueDate();
        //                        }
        //                        else
        //                        {
        //                            bgs_entity.BGS_InsertNewQuarterGrading(txtQuarter.Text.Trim(), Convert.ToDateTime(txtDueDate.Text),Convert.ToDateTime("1190-01-01"), Convert.ToDateTime("1190-01-01"));
        //                            RankSent();
        //                            GetQuarterNameDueDate();
        //                            GetGradingMaster(null);
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid quarter name.');", true);
        //                    GetQuarterNameDueDate();
        //                }

        //            }
        //            else
        //            {
        //                GetQuarterNameDueDate();
        //                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Current Quarter voting is not completed So you can not initiate next quarter grading');", true);
        //            }
        //        }
        //        else
        //        {
        //            GetQuarterNameDueDate();
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + ex.Message + "');", true);
        //    }


        //}
        protected void SaveGradingMaster_Click(object sender, EventArgs e)
        {
            string email = string.Empty;
            int Msg = 0;
            int Save = 0;
            try
            {

                foreach (GridViewRow row in gvGradingrMaster.Rows)
                {
                    if ((row.FindControl("chkIsConfirm") as CheckBox).Checked)
                    {
                        Msg++;
                        if ((row.FindControl("hfIsSend") as HiddenField).Value.ToLower() == "False".ToLower())
                        {
                            int GradingID = Convert.ToInt32(gvGradingrMaster.DataKeys[row.RowIndex].Value);
                            string empId = (row.FindControl("hfEmpId") as HiddenField).Value;
                            #region Email Script
                            string name = (row.FindControl("lblName") as Label).Text;
                            email = (row.FindControl("lblEmail") as Label).Text;
                            try
                            {
                                if (email != "" && email != null)
                                {
                                    SendMail(email, true, name);
                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Invalid Email ID');", true);
                                    BGS.Error.EventLogProcess(email);
                                }
                            }
                            catch (Exception ex1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Could not send mail to following ids:" + email + "');", true);
                                BGS.Error.EventLogProcess(ex1);
                            }
                            #endregion
                            try
                            {
                                bgs_entity.BGS_SaveGradingMaster(GradingID, sQuaterId == "0" ? Convert.ToInt32(QuarterId) : Convert.ToInt32(sQuaterId), NewQuarterName == "" ? this.QuarterName.Trim() : NewQuarterName, Convert.ToDateTime(txtDueDate.Text.Trim()), (int)EnumStatus.Open, empId, true, this.USERNAME, this.USERNAME);
                                Save++;
                            }
                            catch (Exception ex2)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Could not initiate grading for following User:" + name + "');", true);
                                BGS.Error.EventLogProcess(ex2);
                            }

                        }
                    }
                }
                //Monthbind();
                //RankSent();
                //GetQuarterNameDueDate();
                //GetGradingMaster(null);
                //PopulateDepartments();
                //selectDropdownQuarter();
                DeptWiseRecord(btnDept, new EventArgs());

                if (Save > 0)
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Submit successfully.');", true);
                else if (Msg > 0)
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Already Initiate Vote.');", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please check at least one send mail.');", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Cannot save data');", true);
                BGS.Error.EventLogProcess(ex);
            }

        }

        protected void ReopenEditGrading_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Convert.ToInt32((sender as LinkButton).CommandArgument);
                int gradingId = Convert.ToInt32((sender as LinkButton).CommandName);
                bgs_entity.BGS_ReopenGradingprocess((int)EnumStatus.WIP, gradingId);
                string name = (gvGradingrMaster.Rows[index].FindControl("lblName") as Label).Text;
                string email = (gvGradingrMaster.Rows[index].FindControl("lblEmail") as Label).Text;
                SendMailReOpen(email, true, name);
                //GetGradingMaster(null);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Reopened Vote for following User:" + name + "');", true);
                GetGradingMaster(null);
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Could not  sent Mail:');", true);
                BGS.Error.EventLogProcess(ex);
                this.Response.Redirect(this.Request.Url.AbsoluteUri);
            }
        }

        protected void CalculateRank(object sender, EventArgs e)
        {
            this.Response.Redirect("~/CalculateRank.aspx?QuarterId=" + (sQuaterId == "0" ? QuarterId : sQuaterId));
        }
        protected void DeptWiseRecord(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                DataSet ds = new DataSet();
                string chktxt = string.Empty;
                string chkValue = string.Empty;
                txtDept.Text = string.Empty;
                for (int i = 0; i < chkDept.Items.Count; i++)
                {

                    if (chkDept.Items[i].Selected == true)
                    {
                        count++;
                    }
                    if (i == (chkDept.Items.Count - 1) && count == 0)
                    {
                        for (int j = 0; j < chkDept.Items.Count; j++)
                        {
                            chkDept.Items[j].Selected = true;
                        }

                    }
                }
                for (int i = 0; i < chkDept.Items.Count; i++)
                {

                    if (chkDept.Items[i].Selected == true)
                    {
                        chktxt += chkDept.Items[i].Text.ToString() + ",";
                        chkValue += chkDept.Items[i].Value.ToString() + ",";
                    }
                }
                if (chktxt.Length > 0)
                {
                    txtDept.Text = chktxt.Substring(0, chktxt.Length - 1);
                    chkValue = chkValue.Substring(0, chkValue.Length - 1);
                    ds = dl.GetGradingDeptWise(txtDept.Text);
                    gvGradingrMaster.DataSource = ds;
                    gvGradingrMaster.DataBind();
                }
                else if (chktxt == "")
                {
                    txtDept.Text = "Select Department";
                    ds = dl.GetGradingDeptWise(null);
                    gvGradingrMaster.DataSource = ds;
                    gvGradingrMaster.DataBind();
                }
                else
                {
                    txtDept.Text = "Select Department";
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please select department');", true);
                }

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }

        }

        protected void Cancel(object sender, EventArgs e)
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }

        #region Private Methods

        /// <summary>
        /// Gets Grading Master
        /// </summary>
        protected void GetGradingMaster(string DeptID)
        {
            try
            {
                gvGradingrMaster.DataSource = bgs_entity.BGS_GetGradingMaster(DeptID);         // bgs_entity.BGS_GetGradingMaster_NEW(DeptID);
                gvGradingrMaster.DataBind();
                if (this.IsRankSent == 1)
                {
                    gvGradingrMaster.Columns[6].Visible = false;
                }
                else
                {
                    gvGradingrMaster.Columns[6].Visible = true;
                }

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// <summary>
        /// Ge Departemnet 
        /// </summary>
        private void PopulateDepartments()
        {
            DataTable dt = new DataTable();

            dt = dl.GetDepartments();

            chkDept.DataSource = dt;
            chkDept.DataTextField = "DeptName";
            chkDept.DataValueField = "DeptID";
            chkDept.DataBind();
            // chkDept.Items.Insert(0, (new ListItem { Text = "Select All", Value ="0" }));
        }
        /// <summary>
        /// Send Email
        /// </summary>
        /// <param name="strToEmailAdd"></param>
        /// <param name="strBody"></param>
        /// <param name="strSubject"></param>
        /// <param name="strattachFile"></param>
        /// <param name="allowHTML"></param>
        /// <param name="strSenderMailAdd"></param>
        /// <param name="name"></param>
        private void SendMail(string strToEmailAdd, bool allowHTML, string name)
        {
            try
            {
                string strMailServer = ConfigurationManager.AppSettings.Get("MailServer").ToString();
                string strSenderMailID = ConfigurationManager.AppSettings.Get("SenderMailID").ToString();
                string strHeader = ConfigurationManager.AppSettings["HeaderKey"].ToString();
                string strFooter = ConfigurationManager.AppSettings["FooterKey"].ToString();
                string strMailBody = ConfigurationManager.AppSettings["MailToNextLevelApproversBody"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                string AppUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();
                string ISUTI = ConfigurationManager.AppSettings["ISUTI"].ToString();


                //string strMailSub = ConfigurationManager.AppSettings["MailToNextLevelApproversSub"].ToString();
                //StringBuilder sbTable = new StringBuilder();
                //sbTable.Append("<HTML>");
                //sbTable.Append("<BODY>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</p>");
                //sbTable.Append(string.Format("{0}{1}{2}", "<h4>Please initiate the process of Broker Grading for ", txtQuarter.Text.Trim(), " and complete it by " + txtDueDate.Text.Trim() + "</h4>"));
                //sbTable.Append("<h4>Please contact system administrator for any clarification. </h4>");
                //sbTable.Append("<h4>This is autogenerated mail and please do not reply to this email  </h4>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("<h4>Regards,</h4>");
                //sbTable.Append("<h4>UTI Mutual Fund</h4>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</BODY>");
                //sbTable.Append("</HTML>");
                //strHeader = "Dear" + " " + name.ToUpper();
                string strToMailAdd = strToEmailAdd;//string.Empty;
                SmtpClient emailClient = new SmtpClient(strMailServer);
                MailMessage message = new MailMessage();
                message.IsBodyHtml = allowHTML;
                MailAddress maSender = new MailAddress(strSenderMailID);
                message.Sender = maSender;
                message.From = maSender;
                //message.Subject = string.Format("{0} [Select {1}]", "Request for initiating Broker Grading for", txtQuarter.Text.Trim());//Commented by shashi 25-June -2018
                //message.Subject = string.Format("{0} [Select {1}]", "Request for initiating Broker Grading for", this.QuarterName.Trim()); // commented on ghanshyam  15-10-2019
                message.Subject = string.Format("{0}", "Request for initiating Broker Grading");
                //message.Subject = strSubject;
                CommonFunctions cf = new CommonFunctions();
                strHeader = cf.PopulateBody("IG");
                strHeader = strHeader.Replace("{#URL}", AppUrl);
                //strHeader = strHeader.Replace("{#SQ}", txtQuarter.Text.Trim());//Commented by shashi 25-June -2018
                strHeader = strHeader.Replace("{#SQ}", this.QuarterName.Trim());
                strHeader = strHeader.Replace("{#DD}", txtDueDate.Text.Trim());
                message.Body = strHeader;
                //message.Body = strBody;
                message.To.Add(strToMailAdd);
                //System.Net.Mail.Attachment attach1 = new System.Net.Mail.Attachment(strattachFile);
                //message.Attachments.Add(attach1);
                //emailClient.Credentials = new System.Net.NetworkCredential(strSenderMailID, "Password@2");
                // emailClient.UseDefaultCredentials = true;
                emailClient.Send(message);
            }
            catch (SmtpException ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        private void GetQuarterNameDueDate()
        {
            try
            {
                var Query = (from C in bgs_entity.BGS_GetQuarterNameDuteDate()
                             select C).FirstOrDefault();
                if (Query != null)
                {
                    //txtQuarter.Text = Query.Name;
                    txtDueDate.Text = Convert.ToString(Query.DueDate);
                    this.QuarterId = Convert.ToString(Query.QuarterID);
                    this.QuarterName = Query.Name;
                    if (Convert.ToString(Query.StartDate) != null)
                    {
                        if (Convert.ToDateTime(Query.StartDate.ToString()).ToString("yyyy") == (DateTime.Now.Year - yrdiff).ToString())
                        {
                            ddlStartDate.Items.FindByText(Convert.ToDateTime(Query.StartDate).ToString("MMM yyyy")).Selected = true;
                            //ddlStartDate.SelectedItem.Text = Query.StartDate;
                        }
                        else
                        {
                            ddlStartDate.Items.FindByText(Convert.ToDateTime(DateTime.Now.Date).ToString("MMM yyyy")).Selected = true;
                            //ddlStartDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);
                        }
                    }

                    else
                        ddlStartDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                    //ddlStartDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);
                    if (Convert.ToString(Query.EndDate) != null)
                    {
                        if (Convert.ToDateTime(Query.EndDate.ToString()).ToString("yyyy") == (DateTime.Now.Year - yrdiff).ToString())
                        {
                            ddlEndDate.Items.FindByText(Convert.ToDateTime(Query.EndDate).ToString("MMM yyyy")).Selected = true;
                            // ddlEndDate.SelectedItem.Text = Query.StartDate;
                        }
                        else
                        {
                            ddlEndDate.Items.FindByText(Convert.ToDateTime(DateTime.Now.Date).ToString("MMM yyyy")).Selected = true;
                            // ddlEndDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);
                        }
                    }

                    else
                        ddlEndDate.Items.FindByText(DateTime.Now.ToString("MMM yyyy")).Selected = true;
                    //ddlEndDate.SelectedItem.Text = string.Format("{0:MMM yyyy}", DateTime.Now);

                    hfStartDate.Value = ddlStartDate.SelectedItem.Text;
                    hfEndDate.Value = ddlEndDate.SelectedItem.Text;
                    hfDueDate.Value = txtDueDate.Text;
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void RankSent()
        {
            var SentRank = bgs_entity.BGS_IsRankSent(null);
            int count = SentRank.FirstOrDefault().Value;
            this.IsRankSent = count;
            //Deependra --Both Rank master and Grading Master have same max no. thats why it is false.
            if (count == 1)
            {
                btnVoting.Enabled = false;
            }
            else
            {
                btnVoting.Enabled = true;
            }
            // btnVoting.Enabled = (count == 1) ? false : true;

        }
        private void SendMailReOpen(string strToEmailAdd, bool allowHTML, string name)
        {
            try
            {
                string strMailServer = ConfigurationManager.AppSettings.Get("MailServer").ToString();
                string strSenderMailID = ConfigurationManager.AppSettings.Get("SenderMailID").ToString();
                string strHeader = ConfigurationManager.AppSettings["HeaderKey"].ToString();
                string strFooter = ConfigurationManager.AppSettings["FooterKey"].ToString();
                string strMailBody = ConfigurationManager.AppSettings["MailToNextLevelApproversBody"].ToString();
                string AppUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                string ISUTI = ConfigurationManager.AppSettings["ISUTI"].ToString();

                //string strMailSub = ConfigurationManager.AppSettings["MailToNextLevelApproversSub"].ToString();
                //StringBuilder sbTable = new StringBuilder();
                //sbTable.Append("<HTML>");
                //sbTable.Append("<BODY>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</p>");
                //sbTable.Append(string.Format("{0}{1}{2}", "<h4>Please initiate the process of Broker Grading for ", txtQuarter.Text.Trim(), " and complete it by " + txtDueDate.Text.Trim() + "</h4>"));
                //sbTable.Append("<h4>Please contact system administrator for any clarification. </h4>");
                //sbTable.Append("<h4>This is autogenerated mail and please do not reply to this email  </h4>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("<h4>Regards,</h4>");
                //sbTable.Append("<h4>UTI Mutual Fund</h4>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</br>");
                //sbTable.Append("</BODY>");
                //sbTable.Append("</HTML>");
                //strHeader = "Dear" + " " + name.ToUpper();
                string strToMailAdd = string.Empty;
                SmtpClient emailClient = new SmtpClient(strMailServer);
                MailMessage message = new MailMessage();
                message.IsBodyHtml = allowHTML;
                MailAddress maSender = new MailAddress(strSenderMailID);
                message.Sender = maSender;
                message.From = maSender;
                //message.Subject = string.Format("{0} ,{1}", "Voting Re-Opened for", txtQuarter.Text.Trim());//Commented by shashi 25-June -2018
                message.Subject = string.Format("{0} ,{1}", "Voting Re-Opened for", this.QuarterName.Trim());
                //message.Subject = strSubject;
                CommonFunctions cf = new CommonFunctions();

                strHeader = cf.PopulateBody("ReOpenVoting");
                strHeader = strHeader.Replace("{#URL}", AppUrl);
                //strHeader = strHeader.Replace("{#SQ}", txtQuarter.Text.Trim());//Commented by shashi 25-June -2018
                strHeader = strHeader.Replace("{#SQ}", this.QuarterName.Trim());
                message.Body = strHeader;
                //message.Body = strBody;
                message.To.Add(strToEmailAdd);
                //System.Net.Mail.Attachment attach1 = new System.Net.Mail.Attachment(strattachFile);
                //message.Attachments.Add(attach1);
                emailClient.UseDefaultCredentials = true;
                emailClient.Send(message);
                //strresult = "Mail send to " + strToEmailAdd;
            }
            catch (SmtpException ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }





        #endregion

        protected void ddlStartDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str1 = ddlStartDate.SelectedItem.Value;
        }

        protected void ddlEndDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = ddlEndDate.SelectedItem.Value;
        }
    }

}