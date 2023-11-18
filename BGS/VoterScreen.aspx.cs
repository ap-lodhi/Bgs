using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace BGS
{
    public partial class VoterScreen : PageBase
    {
        #region Properties
        DataTable dt = new DataTable();
        Datalayer obj = new Datalayer();
        //SRRMS_PRODEntities srrms_entity = new SRRMS_PRODEntities();
        BGSEntities bgs_entity = new BGSEntities();
        bool is_sorting_allowed = true;
        /// <summary>
        /// Gets or sets Vote ID
        /// </summary>
        public int VoteId
        {
            get
            {
                return ViewState["VoteId"] != null ? (int)ViewState["VoteId"] : 0;
            }
            set
            {
                ViewState["VoteId"] = value;
            }
        }
        /// <summary>
        /// Gets or sets Status ID
        /// </summary>
        public int? StatusId
        {
            get
            {
                return ViewState["StatusId"] != null ? (int)ViewState["StatusId"] : 0;
            }
            set
            {
                ViewState["StatusId"] = value;
            }
        }
        /// <summary>
        /// Gets or sets Weight
        /// </summary>
        public decimal Weight
        {
            get
            {
                return ViewState["Weight"] != null ? (decimal)ViewState["Weight"] : 0;
            }
            set
            {
                ViewState["Weight"] = value;
            }
        }
        /// <summary>
        /// Gets or sets BrokerNameId
        /// </summary>
        public int BrokerNameId
        {
            get
            {
                return ViewState["BrokerNameId"] != null ? (int)ViewState["BrokerNameId"] : 0;
            }
            set
            {
                ViewState["BrokerNameId"] = value;
            }
        }
        public int ReasonID
        {
            get
            {
                return ViewState["ReasonID"] != null ? (int)ViewState["ReasonID"] : 0;
            }
            set
            {
                ViewState["ReasonID"] = value;
            }
        }
        /// <summary>
        /// Gets or sets IsVoterSubmit
        /// </summary>
        public bool IsVoterSubmit
        {
            get
            {
                return ViewState["IsVoterSubmit"] != null ? (bool)ViewState["IsVoterSubmit"] : false;
            }
            set
            {
                ViewState["IsVoterSubmit"] = value;
            }
        }
        /// <summary>
        /// Gets or sets RoleTypeName
        /// </summary>
        public string RoleTypeName
        {
            get
            {
                return !String.IsNullOrEmpty(Convert.ToString(ViewState["RoleTypeName"])) ? ViewState["RoleTypeName"].ToString() : string.Empty;
            }
            set
            {
                ViewState["RoleTypeName"] = value;
            }
        }
        public string DepartmentName
        {
            get
            {
                return !String.IsNullOrEmpty(Convert.ToString(ViewState["DepartmentName"])) ? ViewState["DepartmentName"].ToString() : string.Empty;
            }
            set
            {
                ViewState["DepartmentName"] = value;
            }
        }
        public string SortingExpression
        {
            get
            {
                return !string.IsNullOrEmpty(Convert.ToString(ViewState["SortingExpression"])) ? ViewState["SortingExpression"].ToString().ToUpper() : "DESC";
            }
            set
            {
                ViewState["SortingExpression"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            //scriptManager.RegisterPostBackControl(this.ddlVoterName);
            if (!Page.IsPostBack)
            {
                if (obj.IsUserValidForAccess(Session["EmpId"].ToString(), this.Request.Url.Segments[this.Request.Url.Segments.Length - 1]) == 1)
                {
                    //EnableVoterMailNotSent();
                    BindBrokerGrid();
                    lblvoterNames.Visible = (this.UserTypeName == EnumUserType.Admin);
                    this.GetGradingMasterDetails();
                    this.GetVoteMaster();
                    this.GetVoterName();
                    //checkBrokerExist();
                    int k = gvVoterMaster.Rows.Count;
                    if (k <= 0)
                    {
                        if (EnumUserType.Admin != this.UserTypeName)
                        {
                            SetInitialRow();
                        }
                    }
                    btnView.Visible = this.RoleTypeName == "FM" ? true : false;
                    Enabledissable();
                    EnableVoterMailNotSent();
                    IsRankSent();
                }
            }

        }
        protected void SaveForLater(object sender, EventArgs e)
        {
            bool is_submit = false;
            
            try
            {
                GetGradingMasterDetails();
                SubmitSaveForLater(false);
                int i = 0;
                if (this.RoleTypeName == "FM")
                {
                    string brokername = string.Empty;
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    dt1 = (DataTable)ViewState["dtmatch"];
                    dt2 = (DataTable)ViewState["CurrentTable"];
                    var query1 = from C in dt1.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                    var query2 = from C in dt2.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                    var except = query2.Except(query1).AsEnumerable();
                    foreach (var arr in except)
                    {
                        brokername += arr + ",";
                        i++;
                        //changes starts here   orig:uncomment code
                        if (i > 3)
                        {
                            break;
                        }
                        //changes ends here
                    }

                    string Username = dt2.Rows[0]["CreatedBy"].ToString();
                    if (Username == "Deepak Puthran" && ddlVoterName.SelectedItem.Text != "Please Select")
                    {
                        Username = ddlVoterName.SelectedItem.Text;
                    }
                    if (Username == "J Praveen" || Username == "Sanjay Kumar" || Username == "Rajeev Gupta" || Username == "Kaushik Chatterjee")
                    {
                        //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                        SaveVoter((DataTable)ViewState["CurrentTable"]);
                        bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                        this.GetGradingMasterDetails();
                        this.GetVoteMaster();
                        Enabledissable();
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                    }
                    else
                    {
                        //changes starts here   orig:changed && to ||
                        //if (brokername != null && i <= 3)
                        //changes ends here

                        if (brokername != null && i <= 6)  // if (brokername != null || i <= 3)
                        {
                            //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                            SaveVoter((DataTable)ViewState["CurrentTable"]);
                            bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                            this.GetGradingMasterDetails();
                            this.GetVoteMaster();
                            Enabledissable();
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                        }
                        else
                        {
                            //this.GetVoteMaster();
                            SetBackGroundColor(gvVoterMaster, brokername);
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can vote against those broker/firm. whos broker/firm are available in list');", true);
                            return;
                        }
                    }


                }
                else
                {
                    //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                    SaveVoter((DataTable)ViewState["CurrentTable"]);
                    bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                    this.GetGradingMasterDetails();
                    this.GetVoteMaster();
                    Enabledissable();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                }
                ViewState["is_sorting_allowed"] = true;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void Submit(object sender, EventArgs e)
        {
            try
            {
                GetGradingMasterDetails();
                if (SubmitSaveForLater(true))
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Voter vote limit is 100 so you can not vote more than 100.');", true);
                    return;
                }
                int i = 0;
                if (this.RoleTypeName == "FM")
                {
                    string brokername = string.Empty;
                    DataTable dt1 = new DataTable();
                    DataTable dt2 = new DataTable();
                    dt1 = (DataTable)ViewState["dtmatch"];
                    dt2 = (DataTable)ViewState["CurrentTable"];
                    var query1 = from C in dt1.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                    var query2 = from C in dt2.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                    var except = query2.Except(query1).AsEnumerable();
                    foreach (var arr in except)
                    {
                        brokername += arr + ",";
                        i++;
                        //changes starts here   orig:uncomment code
                        if (i > 3)
                        {
                            break;
                        }
                        //changes ends here
                    }
                    string Username = dt2.Rows[0]["CreatedBy"].ToString();
                    if (Username == "Deepak Puthran" && ddlVoterName.SelectedItem.Text != "Please Select")
                    {
                        Username = ddlVoterName.SelectedItem.Text;
                    }
                    if (Username == "J Praveen" || Username == "Sanjay Kumar" || Username == "Rajeev Gupta" || Username == "Kaushik Chatterjee")
                    {
                        SaveVoter((DataTable)ViewState["CurrentTable"]);
                        string type = btnSubmit.Text;
                        bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                        this.GetGradingMasterDetails();
                        this.GetVoteMaster();
                        Enabledissable();
                        EnableVoterMailNotSent();
                        IsRankSent();
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);
                    }
                    else
                    {
                        //changes starts here   orig:changed && to ||
                        //if (brokername != null && i <= 3)
                        //changes ends here

                        if (brokername != null && i <= 6)  // if (brokername != null || i <= 3)
                        {
                            //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                            SaveVoter((DataTable)ViewState["CurrentTable"]);
                            string type = btnSubmit.Text;
                            bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                            this.GetGradingMasterDetails();
                            this.GetVoteMaster();
                            Enabledissable();
                            EnableVoterMailNotSent();
                            IsRankSent();
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);
                        }
                        else
                        {
                            //this.GetVoteMaster();
                            SetBackGroundColor(gvVoterMaster, brokername);
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can vote against those broker/firm. whos broker/firm are available in list');", true);
                            return;
                        }
                    }


                }
                else
                {

                    //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                    SaveVoter((DataTable)ViewState["CurrentTable"]);
                    string type = btnSubmit.Text;
                    bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                    this.GetGradingMasterDetails();
                    this.GetVoteMaster();
                    Enabledissable();
                    EnableVoterMailNotSent();
                    IsRankSent();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex, "Submit");
            }
        }


        /*  protected void Submit(object sender, EventArgs e)
          {
              try
              {

                    string broker = string.Empty;
                  decimal totalBrokervotes = 0;
                  decimal totalVotersvotes = 0;
                  int Maxlimit = 0;
                  // Button txtvote = (Button)sender;
                  //TextBox txtvote = (TextBox)sender;
                  //GridViewRow row = (GridViewRow)txtvote.NamingContainer;
                  //GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
                  Button txtvote = sender as Button;
                  //GridViewRow row = (GridViewRow)(((Control)sender).NamingContainer);
                 // GridViewRow row = gvVoterMaster.Rows[gvVoterMaster.SelectedIndex];
                  ///



                  if (ViewState["CurrentTable"] != null)
                  {
                      DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                      if (dtCurrentTable.Rows.Count > 0)
                      {
                          this.GetGradingMasterDetails();
                          for (int i = 1; i <= gvVoterMaster.Rows.Count; i++)
                          {
                              TextBox votes = (TextBox)gvVoterMaster.Rows[i - 1].Cells[4].FindControl("txtGridVote");
                              TextBox txtBrokerName = (TextBox)gvVoterMaster.Rows[i - 1].Cells[1].FindControl("txtGgridBrokerName");
                              TextBox txtMaxLimit = (TextBox)gvVoterMaster.Rows[i - 1].Cells[5].FindControl("txtGridMaxLimit");
                              //TextBox txtBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                              // TextBox votes = row.FindControl("txtGridVote") as TextBox;
                              //TextBox txtMaxLimit = row.FindControl("txtGridMaxLimit") as TextBox;
                              string brokerkername = txtBrokerName.Text;
                              Maxlimit = Convert.ToInt32(txtMaxLimit.Text);
                              foreach (GridViewRow r in gvVoterMaster.Rows)
                              {

                                  TextBox Gbroker = r.FindControl("txtGgridBrokerName") as TextBox;
                                  TextBox txtVotes = r.FindControl("txtGridVote") as TextBox;
                                  broker = Gbroker.Text;
                                  if (brokerkername == broker)
                                  {
                                      totalBrokervotes += Convert.ToDecimal(txtVotes.Text);
                                  }
                                  totalVotersvotes += Convert.ToDecimal(txtVotes.Text);
                              }
                              lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes));

                              if (totalVotersvotes > Convert.ToDecimal(EnumTotalVotes.TotalVotes))
                              {
                                  lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes - Convert.ToDecimal(votes.Text)));
                                  votes.Text = string.Empty;
                                  votes.Focus();
                                  ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Voter vote limit is 100 so you can not vote more than 100.');", true);
                              }
                              if (totalBrokervotes > Convert.ToDecimal(Maxlimit))
                              {
                                  lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes - Convert.ToDecimal(votes.Text)));
                                  votes.Text = string.Empty;
                                  votes.Focus();
                                  ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Broker max limit is: " + Maxlimit + "  so you con not vote more tham Broker max limit');", true);
                              }
                              Enabledissable();
                              EnableVoterMailNotSent();
                              IsRankSent();

                          }
                      }
                  }
              }
              catch (Exception ex)
              {
                  BGS.Error.EventLogProcess(ex);
              }
              try
              {
                  GetGradingMasterDetails();
                  SubmitSaveForLater();
                  int i = 0;
                  if (this.RoleTypeName == "FM")
                  {
                      string brokername = string.Empty;
                      DataTable dt1 = new DataTable();
                      DataTable dt2 = new DataTable();
                      dt1 = (DataTable)ViewState["dtmatch"];
                      dt2 = (DataTable)ViewState["CurrentTable"];
                      var query1 = from C in dt1.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                      var query2 = from C in dt2.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                      var except = query2.Except(query1).AsEnumerable();
                      foreach (var arr in except)
                      {
                          brokername += arr + ",";
                          i++;
                          //changes starts here   orig:uncomment code
                          if (i > 3)
                          {
                              break;
                          }
                          //changes ends here
                      }
                      string Username = dt2.Rows[0]["CreatedBy"].ToString();
                      if (Username == "Deepak Puthran" && ddlVoterName.SelectedItem.Text != "Please Select")
                      {
                          Username = ddlVoterName.SelectedItem.Text;
                      }
                      if (Username == "J Praveen" || Username == "Sanjay Kumar" || Username == "Rajeev Gupta" || Username == "Kaushik Chatterjee")
                      {
                          SaveVoter((DataTable)ViewState["CurrentTable"]);
                          string type = btnSubmit.Text;
                          bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                          this.GetGradingMasterDetails();
                          this.GetVoteMaster();
                          Enabledissable();
                          EnableVoterMailNotSent();
                          IsRankSent();
                          ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);
                      }
                      else
                      {
                          //changes starts here   orig:changed && to ||
                          //if (brokername != null && i <= 3)
                          //changes ends here

                          if (brokername != null && i <= 3)  // if (brokername != null || i <= 3)
                          {
                              //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                              SaveVoter((DataTable)ViewState["CurrentTable"]);
                              string type = btnSubmit.Text;
                              bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                              this.GetGradingMasterDetails();
                              this.GetVoteMaster();
                              Enabledissable();
                              EnableVoterMailNotSent();
                              IsRankSent();
                              ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);
                          }
                          else
                          {
                              //this.GetVoteMaster();
                              SetBackGroundColor(gvVoterMaster, brokername);
                              ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can vote against those broker/firm. whos broker/firm are available in list');", true);
                              return;
                          }
                      }


                  }
                  else
                  {

                      //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                      SaveVoter((DataTable)ViewState["CurrentTable"]);
                      string type = btnSubmit.Text;
                      bgs_entity.BGS_UpdateVoteMasterStatus(type, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                      this.GetGradingMasterDetails();
                      this.GetVoteMaster();
                      Enabledissable();
                      EnableVoterMailNotSent();
                      IsRankSent();
                      ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                  }
              }
              catch (Exception ex)
              {
                  BGS.Error.EventLogProcess(ex, "Submit");
              }
          }  //corection1//*/
        private void SetBackGroundColor(GridView grd, string brokerNameId)
        {
            string[] BrokerNameId = brokerNameId.Split(',');
            string s_brokerNameId = "0";
            for (int i = 0; i < BrokerNameId.Count(); i++)
            {
                s_brokerNameId = BrokerNameId[i].Replace("{ BrokerName =", "").Replace("}", "").Trim();
                foreach (GridViewRow grid in grd.Rows)
                {
                    if ((grid.FindControl("hfGRBrokerNameId") as HiddenField).Value == s_brokerNameId)
                    {
                        grid.BackColor = System.Drawing.Color.DarkGray;
                        break;
                    }
                }
            }
        }
        private void SaveVoter(DataTable dt)
        {
            //try
            //{
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("BrokerName");
                obj.Insertupdatevoter(dt);
            }
            //}
            //catch (Exception ex)
            //{
            //    BGS.Error.EventLogProcess(ex, "--SaveVoter--");
            //}
        }
        protected void Cancel(object sender, EventArgs e)
        {
            try
            {
                // this.GetVoteMaster();
                // Enabledissable();
                // EnableVoterMailNotSent();
                // IsRankSent();
                ddlVoterName_SelectedIndexChanged(sender, e);
                //this.Response.Redirect(this.Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void AddNewRecord(object sender, EventArgs e)
        {
            try
            {
                //GetGradingMasterDetails();
                //SubmitSaveForLater();
                //int i = 0;
                //if (this.RoleTypeName == "FM")
                //{
                //    string brokername = string.Empty;
                //    DataTable dt1 = new DataTable();
                //    DataTable dt2 = new DataTable();
                //    dt1 = (DataTable)ViewState["dtmatch"];
                //    dt2 = (DataTable)ViewState["CurrentTable"];
                //    var query1 = from C in dt1.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                //    var query2 = from C in dt2.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                //    var except = query2.Except(query1).AsEnumerable();
                //    foreach (var arr in except)
                //    {
                //        brokername += arr + ",";
                //        i++;
                //        if (i > 3)
                //        {
                //            break;
                //        }
                //    }

                //    if (brokername != null && i <= 3)
                //    {
                //        //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                //        SaveVoter((DataTable)ViewState["CurrentTable"]);
                //        bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                //        this.GetGradingMasterDetails();
                //        this.GetVoteMaster();
                //        Enabledissable();
                //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                //    }
                //    else
                //    {
                //        //this.GetVoteMaster();
                //        SetBackGroundColor(gvVoterMaster, brokername);
                //        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can vote against those broker/firm. whos broker/firm are available in list');", true);
                //        return;
                //    }
                //}
                //else
                //{
                //    //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                //    SaveVoter((DataTable)ViewState["CurrentTable"]);
                //    bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                //    this.GetGradingMasterDetails();
                //    this.GetVoteMaster();
                //    Enabledissable();
                //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                //}


                int index = Convert.ToInt32((sender as Button).CommandArgument);
                AddNewRowToGrid(index);
                Enabledissable();
                EnableVoterMailNotSent();
                IsRankSent();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        private void FillDataTable()
        {
            try
            {

            }
            catch (Exception ex)
            { }
        }

        protected void txtGgridBrokerName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                this.GetGradingMasterDetails();
                TextBox txtBrokerName = (TextBox)sender;
                GridViewRow Row = (GridViewRow)txtBrokerName.NamingContainer;
                DropDownList ddlGridBrokerAnalyst = (DropDownList)Row.FindControl("ddlGridBrokerAnalyst");
                ddlGridBrokerAnalyst.DataSource = obj.BindAnalystName(txtBrokerName.Text.Trim());
                ddlGridBrokerAnalyst.DataTextField = "Name";
                ddlGridBrokerAnalyst.DataValueField = "Name";
                ddlGridBrokerAnalyst.DataBind();
                ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                if (this.RoleTypeName == "FM")
                {
                    TextBox txtMaxLimit = (Row.FindControl("txtGridMaxLimit") as TextBox);
                    txtMaxLimit.Text = Convert.ToString(obj.GetMaxLimit(txtBrokerName.Text.Trim()));
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void ddlGridBrokerAnalyst_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlGridBrokerAnalyst = (DropDownList)sender;
                this.GetGradingMasterDetails();
                GridViewRow row = (GridViewRow)ddlGridBrokerAnalyst.NamingContainer;
                TextBox strBrokerName = (row.FindControl("txtGgridBrokerName") as TextBox);
                int BNID = obj.GetBrokerNameID(strBrokerName.Text.Trim());
                DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, ddlGridBrokerAnalyst.SelectedItem.Text);
                ddlGridSector.DataTextField = "SectorName";
                ddlGridSector.DataValueField = "SectorID";
                ddlGridSector.DataBind();
                ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                TextBox strVote = (row.FindControl("txtGridVote") as TextBox);
                TextBox strMaxLimit = (row.FindControl("txtGridMaxLimit") as TextBox);
                //this.BrokerNameId = Convert.ToInt32(ddlGridBrokerAnalyst.SelectedItem.Value);
                var query = (from C in bgs_entity.BGS_GetBrokerDetailsByBrokerNameBrokerAnalyst(strBrokerName.Text.Trim(), ddlGridBrokerAnalyst.SelectedItem.Text.Trim(), this.UserTypeName == EnumUserType.Admin ? ddlVoterName.SelectedItem.Value : this.EmployeeId)
                             select new { C.MaxLimit, C.Weight }).FirstOrDefault();
                if (query != null)
                {
                    //if (!string.IsNullOrEmpty(query.SectorName))
                    //{
                    //    ddlGridSector.Items.FindByText(query.SectorName).Selected = true;
                    //}
                    // strVote.Text = Convert.ToString(query.Vote);
                    // strVote.Text = "0";
                    strMaxLimit.Text = Convert.ToString(query.MaxLimit);
                    if (query.Weight == null)
                    {
                        this.Weight = 0;
                    }
                    else
                    {
                        this.Weight = Convert.ToDecimal(query.Weight);
                    }
                }

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void txtGridVote_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["is_sorting_allowed"] = false;
                string broker = string.Empty;
                decimal totalBrokervotes = 0;
                decimal totalVotersvotes = 0;
                int Maxlimit = 0;
                TextBox txtvote = (TextBox)sender;
                GridViewRow row = (GridViewRow)txtvote.NamingContainer;
                TextBox txtBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                TextBox votes = row.FindControl("txtGridVote") as TextBox;
                TextBox txtMaxLimit = row.FindControl("txtGridMaxLimit") as TextBox;
                string brokerkername = txtBrokerName.Text;
                Maxlimit = Convert.ToInt32(txtMaxLimit.Text);
                foreach (GridViewRow r in gvVoterMaster.Rows)
                {

                    TextBox Gbroker = r.FindControl("txtGgridBrokerName") as TextBox;
                    TextBox txtVotes = r.FindControl("txtGridVote") as TextBox;
                    broker = Gbroker.Text;
                    if (brokerkername == broker)
                    {
                        totalBrokervotes += Convert.ToDecimal(txtVotes.Text);
                    }
                    totalVotersvotes += Convert.ToDecimal(txtVotes.Text);
                }
                lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes));
                // commented by Anshu on 20-sept-2022 
                //if (totalVotersvotes > Convert.ToDecimal(EnumTotalVotes.TotalVotes))
                //{
                //    lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes - Convert.ToDecimal(votes.Text)));
                //    votes.Text = string.Empty;
                //    votes.Focus();
                //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Voter vote limit is 100 so you can not vote more than 100.');", true);
                //}
                if (totalBrokervotes > Convert.ToDecimal(Maxlimit))
                {
                    lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (totalVotersvotes - Convert.ToDecimal(votes.Text)));
                    votes.Text = string.Empty;
                    votes.Focus();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Broker max limit is: " + Maxlimit + "  so you con not vote more tham Broker max limit');", true);
                }
                Enabledissable();
                EnableVoterMailNotSent();
                IsRankSent();

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void ddlVoterName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                lblTotalVote.Text = "";
                this.GetGradingMasterDetails();
                DisplayAllDetails();
                int k = gvVoterMaster.Rows.Count;
                if (k <= 0 && ddlVoterName.SelectedIndex > 0)
                {
                    SetInitialRow();
                }
                //if (this.RoleTypeName != "FM")
                //{
                //    foreach (GridViewRow row in gvVoterMaster.Rows)
                //    {
                //        DropDownList ddlGridBrokerAnalyst = row.FindControl("ddlGridBrokerAnalyst") as DropDownList;
                //        TextBox txtGridBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                //        ddlGridBrokerAnalyst.DataSource = obj.BindAnalystName(txtGridBrokerName.Text.Trim());
                //        ddlGridBrokerAnalyst.DataTextField = "Name";
                //        ddlGridBrokerAnalyst.DataValueField = "Name";
                //        ddlGridBrokerAnalyst.DataBind();
                //        ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                //        ddlGridBrokerAnalyst.Items.FindByText((row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value).Selected = true;
                //        int BNID = obj.GetBrokerNameID(txtGridBrokerName.Text.Trim());
                //        string brokerAnalyst = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                //        string sector = (row.FindControl("hfSector") as HiddenField).Value;
                //        DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                //        ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, brokerAnalyst);
                //        ddlGridSector.DataTextField = "SectorName";
                //        ddlGridSector.DataValueField = "SectorID";
                //        ddlGridSector.DataBind();
                //        ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                //        if (!string.IsNullOrEmpty(sector))
                //        {
                //            ddlGridSector.Items.FindByValue(sector).Selected = true;
                //        }
                //    }
                //}
                btnView.Visible = this.RoleTypeName == "FM" ? true : false;
                lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
                GetGradingMasterDetails();
                tblDetails.Visible = ddlVoterName.SelectedIndex > 0;
                lblTotalVote.Visible = true;
                Enabledissable();
                EnableVoterMailNotSent();
                IsRankSent();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void ddlGridSector_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GetGradingMasterDetails();
            DropDownList ddlSector = (DropDownList)sender;
            GridViewRow Row = (GridViewRow)ddlSector.NamingContainer;
            TextBox txtBrokerName = (TextBox)(Row.FindControl("txtGgridBrokerName"));
            DropDownList ddlAnalystName = (DropDownList)(Row.FindControl("ddlGridBrokerAnalyst"));
            DropDownList ddlGridBrokerAnalyst = (DropDownList)Row.FindControl("ddlGridBrokerAnalyst");
            string voteId = (Row.FindControl("hfVoterID") as HiddenField).Value;
            int isVoteId = !string.IsNullOrEmpty(voteId) ? Convert.ToInt32(voteId) : 0;
            var query = bgs_entity.AuthenticateDublicateVoter(txtBrokerName.Text.Trim(), ddlAnalystName.SelectedItem.Text.Trim(), !string.IsNullOrEmpty(ddlSector.SelectedItem.Value.Trim()) ? Convert.ToInt32(ddlSector.SelectedItem.Value.Trim()) : 0, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value.Trim() : this.EmployeeId);
            int count = query.FirstOrDefault().Value;
            if ((count > 1 && isVoteId > 0) || (count > 0 && isVoteId == 0))
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record already exist:');", true);
                ddlSector.ClearSelection();
            }
        }
        protected void VoteMaster(object sender, EventArgs e)
        {
            try
            {
                TextBox txtVote = (gvVoterMaster.FooterRow.FindControl("txtVote") as TextBox);
                if ((Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (Convert.ToDecimal(txtVote.Text) + CalculateVotes())) >= 0)
                {
                    lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
                    GetGradingMasterDetails();
                    this.GetVoteMaster();

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);
                }
                else
                {

                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Total votes can not be more than 100.');", true);
                }
                Enabledissable();
                EnableVoterMailNotSent();
                IsRankSent();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }

        }
        protected void DeleteVote(object sender, EventArgs e)
        {
            try
            {

                Button btnDelete = (sender as Button);
                DataTable dtgrid = new DataTable();
                int index = Convert.ToInt32(btnDelete.CommandArgument);
                int voteId = !string.IsNullOrEmpty(btnDelete.CommandName) ? Convert.ToInt32(btnDelete.CommandName) : 0;
                this.GetGradingMasterDetails();

                if (voteId <= 0)
                {
                    dtgrid = (DataTable)ViewState["CurrentTable"];
                    dtgrid.Rows[index].Delete();
                    dtgrid.AcceptChanges();
                    gvVoterMaster.DataSource = dtgrid;
                    gvVoterMaster.DataBind();
                    bindrecordDel(dtgrid);
                    ViewState["CurrentTable"] = null;
                    ViewState["CurrentTable"] = dtgrid;
                    SetPreviousData();
                }
                else
                {
                    if (this.RoleTypeName != "FM")
                    {
                        DataTable dtadd = new DataTable();
                        dtadd.Columns.Add(new DataColumn("VoteID", typeof(int)));
                        dtadd.Columns.Add(new DataColumn("BrokerName", typeof(string)));
                        dtadd.Columns.Add(new DataColumn("AnalystName", typeof(string)));
                        dtadd.Columns.Add(new DataColumn("SectorName", typeof(string)));
                        dtadd.Columns.Add(new DataColumn("Vote", typeof(float)));
                        dtadd.Columns.Add(new DataColumn("MaxLimit", typeof(int)));
                        dtadd.Columns.Add(new DataColumn("BrokerNameId", typeof(int)));
                        dtadd.Columns.Add(new DataColumn("SectorId", typeof(int)));
                        dtadd.Columns.Add(new DataColumn("FMComments", typeof(string)));
                        dtadd.Columns.Add(new DataColumn("RMComment", typeof(string)));
                        dtadd.Columns.Add(new DataColumn("ReasonName", typeof(int)));
                        foreach (GridViewRow row in gvVoterMaster.Rows)
                        {
                            HiddenField BrokerNameId = (HiddenField)row.FindControl("hfGRBrokerNameId");
                            //  HiddenField hfBrokerAnalyst = (HiddenField)row.FindControl("hfGridBrokerAnalyst");
                            HiddenField hfSector = (HiddenField)row.FindControl("hfSector");
                            HiddenField VoterID = (HiddenField)row.FindControl("hfVoterID");
                            TextBox txtbrokerfirm = (TextBox)row.FindControl("txtGgridBrokerName");
                            DropDownList txtSector = (DropDownList)row.FindControl("ddlGridSector");
                            TextBox txtVote = (TextBox)row.FindControl("txtGridVote");
                            TextBox txtMaxLimit = (TextBox)row.FindControl("txtGridMaxLimit");
                            DropDownList ddlBrokerAnalyst = (row.FindControl("ddlGridBrokerAnalyst") as DropDownList);
                            TextBox txtDescription = (TextBox)row.FindControl("txtDescription");
                            string ReasonName = ((TextBox)row.FindControl("txtDescDOD_DOFA")).Text;
                            this.ReasonID = obj.GetReasonNameID(ReasonName);
                            if (ReasonName == null || ReasonID == 0)
                            {
                                ReasonName = "Better Execution - Trades";
                                ReasonID = 1;

                            }
                            dtadd.Rows.Add(!string.IsNullOrEmpty(VoterID.Value) ? Convert.ToInt32((VoterID.Value)) : 0, txtbrokerfirm.Text, !string.IsNullOrEmpty(ddlBrokerAnalyst.SelectedItem.Text) ? ddlBrokerAnalyst.SelectedItem.Text : null, !string.IsNullOrEmpty(txtSector.SelectedItem.Text) ? txtSector.SelectedItem.Text : null, Convert.ToDecimal(txtVote.Text), Convert.ToInt32(txtMaxLimit.Text), !string.IsNullOrEmpty(BrokerNameId.Value) ? Convert.ToInt32(BrokerNameId.Value) : 0, !string.IsNullOrEmpty(hfSector.Value) ? Convert.ToInt32(hfSector.Value) : 0, txtDescription.Text.Trim(), ReasonID);
                        }

                        dtadd.Rows[index].Delete();
                        dtadd.AcceptChanges();
                        this.bgs_entity.BGS_DeleteVoteMaster(voteId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value.Trim() : this.EmployeeId);
                        gvVoterMaster.DataSource = dtadd;
                        gvVoterMaster.DataBind();
                        bindrecordDel(dtadd);
                        ViewState["CurrentTable"] = null;
                        ViewState["CurrentTable"] = dtadd;
                    }
                    else
                    {
                        this.bgs_entity.BGS_DeleteVoteMaster(voteId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value.Trim() : this.EmployeeId);
                        GetVoteMaster();
                    }
                    // bindrecordDel(dtgrid);
                }
                lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
                Enabledissable();
                EnableVoterMailNotSent();
                IsRankSent();
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record deleted Successfully. ');", true);

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            //try
            //{
            //    Button btnDelete = (sender as Button);
            //    DataTable dtgrid = new DataTable();
            //    int index = Convert.ToInt32(btnDelete.CommandArgument);
            //    int voteId = !string.IsNullOrEmpty(btnDelete.CommandName) ? Convert.ToInt32(btnDelete.CommandName) : 0;
            //    this.GetGradingMasterDetails();

            //    if (voteId <= 0)
            //    {
            //        dtgrid = (DataTable)ViewState["CurrentTable"];
            //        dtgrid.Rows[index].Delete();
            //        dtgrid.AcceptChanges();
            //       // bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
            //        gvVoterMaster.DataSource = dtgrid;
            //        gvVoterMaster.DataBind();
            //        bindrecord();
            //    }
            //    else
            //    {
            //        this.bgs_entity.BGS_DeleteVoteMaster(voteId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value.Trim() : this.EmployeeId);
            //        bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
            //        this.GetVoteMaster();
            //        bindrecord();
            //    }
            //    lblTotalVote.Text = Convert.ToString(Convert.ToDouble(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
            //    Enabledissable();
            //    EnableVoterMailNotSent();
            //    IsRankSent();
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record deleted Successfully. ');", true);

            //}
            //catch (Exception ex)
            //{
            //    BGS.Error.EventLogProcess(ex);
            //}
        }
        #region [Method]
        private void SetInitialRow()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow dr = null;
                // dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));
                dt.Columns.Add(new DataColumn("BrokerName", typeof(string)));
                dt.Columns.Add(new DataColumn("AnalystName", typeof(string)));
                dt.Columns.Add(new DataColumn("SectorName", typeof(string)));
                dt.Columns.Add(new DataColumn("Vote", typeof(float)));
                dt.Columns.Add(new DataColumn("MaxLimit", typeof(int)));
                dt.Columns.Add(new DataColumn("BrokerID", typeof(int)));
                //dt.Columns.Add(new DataColumn("AnalystName", typeof(int)));
                dt.Columns.Add(new DataColumn("VoteID", typeof(int)));
                dt.Columns.Add(new DataColumn("SectorId", typeof(int)));
                dt.Columns.Add(new DataColumn("BrokerNameId", typeof(int)));
                dt.Columns.Add(new DataColumn("FMComments", typeof(string)));
                dt.Columns.Add(new DataColumn("RMComment", typeof(string)));
                dt.Columns.Add(new DataColumn("ReasonName", typeof(string)));
                dt.Columns.Add(new DataColumn("ReasonID", typeof(string)));

                dr = dt.NewRow();
                dr["BrokerName"] = string.Empty;
                dr["AnalystName"] = string.Empty;
                dr["SectorName"] = string.Empty;
                dr["Vote"] = 0;
                dr["MaxLimit"] = 0;
                dr["BrokerID"] = 0;
                dr["VoteID"] = 0;
                dr["FMComments"] = string.Empty;
                dr["RMComment"] = string.Empty;
                dr["ReasonName"] = string.Empty;
                dt.Rows.Add(dr);

                //Store the DataTable in ViewState
                ViewState["CurrentTable"] = dt;
                gvVoterMaster.DataSource = dt;
                //HiddenField BrokerGID = (gvVoterMaster.Rows[0].FindControl("hfGRBrokerId") as HiddenField);
                //BrokerGID.Value = "0";
                gvVoterMaster.DataBind();
                tblbutton.Visible = true;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void AddNewRowToGrid(int index)
        {
            try
            {
                int rowIndex = 0;

                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
                    DataRow drCurrentRow = null;
                    if (dtCurrentTable.Rows.Count > 0)
                    {
                        this.GetGradingMasterDetails();
                        for (int i = 1; i <= gvVoterMaster.Rows.Count; i++)
                        {

                            string hfGridBrokerAnalyst = (gvVoterMaster.Rows[i - 1].FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                            TextBox txtbrokerfirm = (TextBox)gvVoterMaster.Rows[i - 1].Cells[1].FindControl("txtGgridBrokerName");
                            DropDownList ddlSector = (DropDownList)gvVoterMaster.Rows[i - 1].Cells[3].FindControl("ddlGridSector");
                            TextBox txtVote = (TextBox)gvVoterMaster.Rows[i - 1].Cells[4].FindControl("txtGridVote");
                            TextBox txtMaxLimit = (TextBox)gvVoterMaster.Rows[i - 1].Cells[5].FindControl("txtGridMaxLimit");
                            drCurrentRow = dtCurrentTable.NewRow();
                            dtCurrentTable.Rows[i - 1]["BrokerName"] = txtbrokerfirm.Text;
                            if (this.RoleTypeName != "FM")
                            {
                                DropDownList ddlAnalystName = (DropDownList)gvVoterMaster.Rows[rowIndex].Cells[2].FindControl("ddlGridBrokerAnalyst");
                                dtCurrentTable.Rows[i - 1]["AnalystName"] = ddlAnalystName.SelectedItem.Text;
                                dtCurrentTable.Rows[i - 1]["SectorName"] = ddlSector.SelectedItem.Text;
                            }
                            else if (this.RoleTypeName == "FM" && (this.DepartmentName == "DOD" || this.DepartmentName == "DOFA"))
                            {
                                TextBox txtDescription = (TextBox)gvVoterMaster.Rows[i - 1].Cells[2].FindControl("txtDescDOD_DOFA");
                                dtCurrentTable.Rows[i - 1]["AnalystName"] = txtDescription.Text;
                            }

                            else
                            {
                                TextBox txtDescription = (TextBox)gvVoterMaster.Rows[i - 1].Cells[2].FindControl("txtDescription");
                                dtCurrentTable.Rows[i - 1]["AnalystName"] = txtDescription.Text;
                            }
                            dtCurrentTable.Rows[i - 1]["Vote"] = txtVote.Text;
                            dtCurrentTable.Rows[i - 1]["MaxLimit"] = txtMaxLimit.Text;

                            rowIndex++;
                        }
                        dtCurrentTable.Rows.InsertAt(drCurrentRow, index + 1);
                        ViewState["CurrentTable"] = dtCurrentTable;
                        gvVoterMaster.DataSource = dtCurrentTable;
                        gvVoterMaster.DataBind();

                    }
                }
                else
                {
                    Response.Write("No Data found");
                }
                SetPreviousData();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void SetPreviousData()
        {
            try
            {
                int rowIndex = 0;
                if (ViewState["CurrentTable"] != null)
                {
                    DataTable dt = (DataTable)ViewState["CurrentTable"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TextBox txtbrokerfirm = (TextBox)gvVoterMaster.Rows[rowIndex].Cells[1].FindControl("txtGgridBrokerName");
                            DropDownList ddlAnalystName = (DropDownList)gvVoterMaster.Rows[rowIndex].Cells[2].FindControl("ddlGridBrokerAnalyst");
                            DropDownList ddlBrokerAnalyst = (gvVoterMaster.Rows[i].FindControl("ddlGridBrokerAnalyst") as DropDownList);
                            DropDownList txtSector = (DropDownList)gvVoterMaster.Rows[rowIndex].Cells[3].FindControl("ddlGridSector");
                            TextBox txtDescription = (TextBox)gvVoterMaster.Rows[i].FindControl("txtDescription");
                            TextBox txtDescDOD_DOFA = (TextBox)gvVoterMaster.Rows[i].FindControl("txtDescDOD_DOFA");
                            TextBox txtVote = (TextBox)gvVoterMaster.Rows[rowIndex].Cells[4].FindControl("txtGridVote");
                            TextBox txtMaxLimit = (TextBox)gvVoterMaster.Rows[rowIndex].Cells[5].FindControl("txtGridMaxLimit");
                            if (this.RoleTypeName != "FM")
                            {

                                ddlBrokerAnalyst.DataSource = obj.BindAnalystName(txtbrokerfirm.Text.Trim());
                                ddlBrokerAnalyst.DataTextField = "Name";
                                ddlBrokerAnalyst.DataValueField = "Name";
                                ddlBrokerAnalyst.DataBind();
                                ddlBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                                txtbrokerfirm.Text = dt.Rows[i]["BrokerName"].ToString();

                                if (Server.HtmlDecode(dt.Rows[i]["AnalystName"].ToString().Trim()) != "")
                                {
                                    try
                                    {
                                        ddlAnalystName.Items.FindByText(dt.Rows[i]["AnalystName"].ToString()).Selected = true;
                                    }
                                    catch { }
                                }
                                else
                                {
                                    ddlAnalystName.SelectedItem.Text = "";
                                }
                                int BNID = obj.GetBrokerNameID(txtbrokerfirm.Text.Trim());
                                txtSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, dt.Rows[i]["AnalystName"].ToString());
                                txtSector.DataTextField = "SectorName";
                                txtSector.DataValueField = "SectorID";
                                txtSector.DataBind();
                                txtSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                                if (!string.IsNullOrEmpty(dt.Rows[i]["SectorName"].ToString()))
                                {
                                    try
                                    {
                                        txtSector.Items.FindByText(dt.Rows[i]["SectorName"].ToString()).Selected = true;
                                    }
                                    catch { }
                                }
                            }
                            else if (this.RoleTypeName == "FM" && (this.DepartmentName == "DOD" || this.DepartmentName == "DOFA"))
                            {
                                txtDescDOD_DOFA.Text = dt.Rows[i]["AnalystName"].ToString();
                            }
                            else
                            {

                                txtDescription.Text = dt.Rows[i]["AnalystName"].ToString();
                            }

                            txtVote.Text = dt.Rows[i]["Vote"].ToString();
                            txtMaxLimit.Text = dt.Rows[i]["MaxLimit"].ToString();
                            // dt.Rows[i]["BrokerName"] = txtbrokerfirm.Text;
                            //if (this.RoleTypeName != "FM")
                            //{
                            //    dt.Rows[i]["AnalystName"] = ddlAnalystName.SelectedItem.Text;
                            //    dt.Rows[i]["SectorName"] = txtSector.SelectedItem.Text;
                            //}
                            //else
                            //{
                            //    dt.Rows[i]["AnalystName"] = txtDescription.Text;
                            //    dt.Rows[i]["Vote"] = txtVote.Text;
                            //    dt.Rows[i]["MaxLimit"] = txtMaxLimit.Text;

                            //}
                            rowIndex++;
                        }
                        // ViewState["CurrentTable"]=null;
                        // ViewState["CurrentTable"] = dt;

                    }
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        /// get or set vote and Max limmit by Broker Analist
        /// </summary>
        private void GetGradingMasterDetails()
        {
            try
            {
                tblDetails.Visible = (this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0) || (this.UserTypeName == EnumUserType.voter);
                if (this.UserTypeName == EnumUserType.Admin)
                {
                    tblVoter.Visible = true;
                }

                var query = (from C in bgs_entity.BGS_GetUserDetailsForVoter(EnumUserType.Admin == this.UserTypeName && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId)
                             select new { C.GradingID, C.DueDate, C.EmpID, C.StatusID, C.RoleName, C.Name, C.IsVoterSubmit, C.StartDate, C.EndDate, C.DeptName }).FirstOrDefault();

                if (query != null)
                {
                    this.GradingId = query.GradingID;
                    lblVoterName.Text = (this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Text : this.USERNAME);
                    lblDueDate.Text = Convert.ToString(query.DueDate);
                    this.RoleTypeName = (query.RoleName).TrimStart().TrimEnd();
                    this.DepartmentName = (query.DeptName).TrimStart().TrimEnd();
                    //lblQuarter.Text = query.Name;
                    lblStartDate.Text = query.StartDate ; 
                    lblEndDate.Text = query.EndDate;
                    this.StatusId = query.StatusID;
                    this.IsVoterSubmit = query.IsVoterSubmit.Value;
                    //if (this.StatusId == 4)
                    //{
                    //    btnSaveForLater.Enabled = false;
                    //    btnSubmit.Enabled = true;
                    //    //btnCancel.Enabled = false;
                    //}
                    ViewState["is_sorting_allowed"] = true;
                }

                else
                {
                    lblTotalVote.Visible = false;
                    this.GradingId = 0;
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// <summary>
        ///get Voter Name
        /// </summary>
        private void GetVoterName()
        {
            try
            {
                ddlVoterName.DataSource = bgs_entity.BGS_GetVoterName(this.EmployeeId);
                ddlVoterName.DataTextField = "UserName";
                ddlVoterName.DataValueField = "EmpID";
                ddlVoterName.DataBind();
                ddlVoterName.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
                // btnRowAdd.Visible = (EnumUserType.Admin == this.UserTypeName ? false : true);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// <summary>
        ///get sector name
        /// </summary>
        /// <summary>
        private void PopulateSectorName(int BrokerNameID, string AnalystName)
        {
            try
            {
                foreach (GridViewRow gvrow in gvVoterMaster.Rows)
                {
                    DropDownList ddlGridSector = (gvrow.FindControl("ddlGridSector") as DropDownList);
                    ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BrokerNameID, AnalystName);
                    ddlGridSector.DataTextField = "SectorName";
                    ddlGridSector.DataValueField = "SectorID";
                    ddlGridSector.DataBind();
                    ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// Calculate Votes
        /// </summary> 
        private decimal CalculateVotes()
        {
            decimal Votes = 0;
            try
            {
                foreach (GridViewRow row in gvVoterMaster.Rows)
                {
                    Votes += Convert.ToDecimal((row.FindControl("txtGridVote") as TextBox).Text);
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            return Votes;
        }
        /// <summary>
        ///get Role On Voter Selecttion
        /// </summary> 
        private void GetGradingMasterDetails(string EmpId)
        {
            try
            {
                var query = (from C in bgs_entity.BGS_GetVoterByCurrentQuarter(EmpId)
                             select new { C.RoleName, C.Name }).FirstOrDefault();
                if (query != null)
                {
                    // lblRoleType.Text = query.RoleName;
                    //lblQuarter.Text = query.Name;
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void Enabledissable()
        {
            try
            {
                //if (lblTotalVote.Text.Length == 0) { lblTotalVote.Text = "0"; }
                if (Convert.ToDouble(lblTotalVote.Text == "" ? null : lblTotalVote.Text) == 0.00 && this.StatusId != 4)
                {
                    lblTotalVote.Text = "0.00";
                    //bool b = Convert.ToInt32(lblTotalVote.Text) == 0;
                    btnSubmit.Enabled = Convert.ToDouble(lblTotalVote.Text) == 0.00;
                    btnSaveForLater.Enabled = true;
                    btnCancel.Enabled = true;
                    gvVoterMaster.Enabled = true;
                }
                else if (this.StatusId == 4)
                {
                    this.DisableAll(false);
                }

                if (Convert.ToDouble(lblTotalVote.Text) > 0)
                {
                    if (this.StatusId == 4)
                    {
                        this.DisableAll(false);
                    }
                    else
                    {
                        //changes starts here     orig: uncomment it

                        btnSubmit.Enabled = Convert.ToDouble(lblTotalVote.Text) == 0.00;
                        //changes ends here

                        // btnSubmit.Enabled = false;
                        btnSaveForLater.Enabled = true;
                        btnCancel.Enabled = true;
                        gvVoterMaster.Enabled = true;
                    }
                }
                //changes starts here
                //if (this.StatusId != 4)
                //{
                //    bool b = Convert.ToInt32(lblTotalVote.Text) == 0;
                //    btnSubmit.Enabled = Convert.ToDouble(lblTotalVote.Text) == 0.00;
                //    btnSaveForLater.Enabled = true;
                //    btnCancel.Enabled = true;
                //    gvVoterMaster.Enabled = true;

                //}
                //changes ends here

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }


        private void GetVoteMaster(GridViewSortEventArgs sort = null)
        {




            try
            {
                DataTable dt = new DataTable();
                string broker = string.Empty;
                int BNID = 0;
                dt = obj.GetVoterByCurrentQuarter(this.UserTypeName == EnumUserType.Admin ? (ddlVoterName.SelectedItem != null ? ddlVoterName.SelectedItem.Value : "0") : this.EmployeeId);
                if (sort != null)
                {
                    dt.DefaultView.Sort = String.Concat(sort.SortExpression, " ", this.SortingExpression);
                    this.SortingExpression = this.SortingExpression == "DESC" ? "ASC" : "DESC";
                }
                ViewState["CurrentTable"] = null;
                ViewState["CurrentTable"] = dt;
                //if(this.RoleTypeName == "FM" && (this.DepartmentName == "DOD" || this.DepartmentName == "DOFA"))
                //{
                //    gvVoterMaster.DataSource = dt;
                //    gvVoterMaster.DataBind();
                //}
                gvVoterMaster.DataSource = dt;
                gvVoterMaster.DataBind();
                tblbutton.Visible = dt.Rows.Count > 0;

                if (this.RoleTypeName != "FM")
                {
                    foreach (GridViewRow row in gvVoterMaster.Rows)
                    {
                        try
                        {
                            broker = ((TextBox)row.FindControl("txtGgridBrokerName")).Text;
                            BNID = obj.GetBrokerNameID(broker.Trim());
                            DropDownList ddlGridBrokerAnalyst = row.FindControl("ddlGridBrokerAnalyst") as DropDownList;
                            TextBox txtGridBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                            ddlGridBrokerAnalyst.DataSource = obj.BindAnalystName(txtGridBrokerName.Text.Trim());
                            ddlGridBrokerAnalyst.DataTextField = "Name";
                            ddlGridBrokerAnalyst.DataValueField = "Name";
                            ddlGridBrokerAnalyst.DataBind();
                            ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                            if (!string.IsNullOrEmpty((row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value.Trim()))
                            {
                                try
                                {
                                    ddlGridBrokerAnalyst.Items.FindByText((row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value.Trim()).Selected = true;
                                }
                                catch { }
                            }
                            DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                            ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, ddlGridBrokerAnalyst.SelectedItem.Text);
                            ddlGridSector.DataTextField = "SectorName";
                            ddlGridSector.DataValueField = "SectorID";
                            ddlGridSector.DataBind();
                            //ddlGridSector.Items.FindByValue((row.FindControl("hfSector") as HiddenField).Value).Selected = true;
                            ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                            if (!string.IsNullOrEmpty((row.FindControl("hfSector") as HiddenField).Value))
                            {
                                try
                                {
                                    ddlGridSector.Items.FindByValue((row.FindControl("hfSector") as HiddenField).Value).Selected = true;
                                }
                                catch { }
                            }
                            //string hfGridselctor = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                            //if (!string.IsNullOrEmpty(hfGridselctor))
                            //{
                            //    ddlGridSector.Items.FindByText(hfGridselctor).Selected = true;
                            //}

                        }
                        catch (Exception ex)
                        {
                            Response.Write("<script>alert('Error')</script>");

                        }
                    }
                }

                if (Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()) == 100)
                {
                    lblTotalVote.Text = "";
                }
                else
                {
                    lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
                }


                //if (this.UserTypeName != EnumUserType.Admin)
                //{
                //    string[] arr = lblTotalVote.Text.ToString().Trim().Split('=');
                //    btnSubmit.Enabled = (arr[1].ToString().TrimStart() == "0" ? true : false);
                //}
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void DisplayAllDetails()
        {
            try
            {
                DataTable dt = new DataTable();
                string broker = string.Empty;
                int BNID = 0;
                dt = obj.GetVoterByCurrentQuarter(this.UserTypeName == EnumUserType.Admin ? (ddlVoterName.SelectedItem != null ? ddlVoterName.SelectedItem.Value : "0") : this.EmployeeId);
                ViewState["CurrentTable"] = null;
                ViewState["CurrentTable"] = dt;
                gvVoterMaster.DataSource = dt;
                gvVoterMaster.DataBind();
                tblbutton.Visible = dt.Rows.Count > 0;

                if (this.RoleTypeName != "FM")
                {
                    foreach (GridViewRow row in gvVoterMaster.Rows)
                    {
                        try
                        {
                            broker = ((TextBox)row.FindControl("txtGgridBrokerName")).Text;
                            BNID = obj.GetBrokerNameID(broker.Trim());
                            DropDownList ddlGridBrokerAnalyst = row.FindControl("ddlGridBrokerAnalyst") as DropDownList;
                            TextBox txtGridBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                            ddlGridBrokerAnalyst.DataSource = obj.BindAllAnalystName(txtGridBrokerName.Text.Trim());
                            ddlGridBrokerAnalyst.DataTextField = "Name";
                            ddlGridBrokerAnalyst.DataValueField = "Name";
                            ddlGridBrokerAnalyst.DataBind();
                            ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));

                            string str = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value.Trim();

                            ddlGridBrokerAnalyst.Items.FindByText((row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value.Trim()).Selected = true;

                            DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                            ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, ddlGridBrokerAnalyst.SelectedItem.Text);

                            ddlGridSector.DataTextField = "SectorName";
                            ddlGridSector.DataValueField = "SectorID";
                            ddlGridSector.DataBind();
                            //ddlGridSector.Items.FindByValue((row.FindControl("hfSector") as HiddenField).Value).Selected = true;
                            ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));

                            try
                            {
                                ddlGridSector.Items.FindByValue((row.FindControl("hfSector") as HiddenField).Value).Selected = true;
                            }
                            catch { }
                            //string hfGridselctor = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                            //if (!string.IsNullOrEmpty(hfGridselctor))
                            //{
                            //    ddlGridSector.Items.FindByText(hfGridselctor).Selected = true;
                            //}

                        }
                        catch (Exception ex)
                        {
                            //Response.Write("<script>alert('Error')</script>");
                            Response.Write(ex.Message);
                        }
                    }
                }

                if (Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()) == 100)
                {
                    lblTotalVote.Text = "";
                }
                else
                {
                    lblTotalVote.Text = Convert.ToString(Convert.ToDecimal(BGS.EnumTotalVotes.TotalVotes) - (CalculateVotes()));
                }


                //if (this.UserTypeName != EnumUserType.Admin)
                //{
                //    string[] arr = lblTotalVote.Text.ToString().Trim().Split('=');
                //    btnSubmit.Enabled = (arr[1].ToString().TrimStart() == "0" ? true : false);
                //}
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void PopulateBrokerAnalyst(String BrokerAnalyst)
        {
            try
            {
                foreach (GridViewRow gvrow in gvVoterMaster.Rows)
                {
                    DropDownList ddlBrokerAnalyst = (gvrow.FindControl("ddlGridBrokerAnalyst") as DropDownList);
                    ddlBrokerAnalyst.DataSource = obj.BindAnalystName(BrokerAnalyst);
                    ddlBrokerAnalyst.DataTextField = "Name";
                    ddlBrokerAnalyst.DataValueField = "Name";
                    ddlBrokerAnalyst.DataBind();
                    ddlBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void bindrecordDel(DataTable dt)
        {
            if (this.RoleTypeName != "FM")
            {
                foreach (GridViewRow row in gvVoterMaster.Rows)
                {
                    string brokerAnalyst = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                    string sector = dt.Rows[row.RowIndex]["SectorName"].ToString().Trim();
                    TextBox txtbrokerfirm = (TextBox)gvVoterMaster.Rows[row.RowIndex].Cells[1].FindControl("txtGgridBrokerName");
                    int BNID = obj.GetBrokerNameID(txtbrokerfirm.Text.Trim());
                    DropDownList ddlGridBrokerAnalyst = row.FindControl("ddlGridBrokerAnalyst") as DropDownList;
                    TextBox txtGridBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                    ddlGridBrokerAnalyst.DataSource = obj.BindAnalystName(txtGridBrokerName.Text.Trim());
                    ddlGridBrokerAnalyst.DataTextField = "Name";
                    ddlGridBrokerAnalyst.DataValueField = "Name";
                    ddlGridBrokerAnalyst.DataBind();
                    ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                    if (!string.IsNullOrEmpty(brokerAnalyst))
                    {
                        try
                        {
                            ddlGridBrokerAnalyst.Items.FindByText(brokerAnalyst).Selected = true;
                        }
                        catch { }
                        DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                        ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, brokerAnalyst);
                        ddlGridSector.DataTextField = "SectorName";
                        ddlGridSector.DataValueField = "SectorID";
                        ddlGridSector.DataBind();
                        ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                        if (!string.IsNullOrEmpty(sector))
                        {
                            // ddlGridSector.ClearSelection();
                            // ddlGridSector.SelectedItem.Text = sector;
                            try
                            {
                                ddlGridSector.Items.FindByText(sector).Selected = true;
                            }
                            catch { }
                        }
                    }
                }
            }
        }
        private void bindrecord()
        {
            if (this.RoleTypeName != "FM")
            {
                string sector = string.Empty;
                foreach (GridViewRow row in gvVoterMaster.Rows)
                {
                    string brokerAnalyst = (row.FindControl("hfGridBrokerAnalyst") as HiddenField).Value;
                    sector = (row.FindControl("hfSector") as HiddenField).Value;
                    TextBox txtbrokerfirm = (TextBox)gvVoterMaster.Rows[row.RowIndex].Cells[1].FindControl("txtGgridBrokerName");
                    int BNID = obj.GetBrokerNameID(txtbrokerfirm.Text.Trim());
                    DropDownList ddlGridBrokerAnalyst = row.FindControl("ddlGridBrokerAnalyst") as DropDownList;
                    TextBox txtGridBrokerName = row.FindControl("txtGgridBrokerName") as TextBox;
                    ddlGridBrokerAnalyst.DataSource = obj.BindAnalystName(txtGridBrokerName.Text.Trim());
                    ddlGridBrokerAnalyst.DataTextField = "Name";
                    ddlGridBrokerAnalyst.DataValueField = "Name";
                    ddlGridBrokerAnalyst.DataBind();
                    ddlGridBrokerAnalyst.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                    if (!string.IsNullOrEmpty(brokerAnalyst))
                    {
                        try
                        {
                            ddlGridBrokerAnalyst.Items.FindByText(brokerAnalyst).Selected = true;
                        }
                        catch { }
                        sector = (row.FindControl("hfSector") as HiddenField).Value;
                        DropDownList ddlGridSector = (row.FindControl("ddlGridSector") as DropDownList);
                        ddlGridSector.DataSource = bgs_entity.BGS_GetVoterSector(BNID, brokerAnalyst);
                        ddlGridSector.DataTextField = "SectorName";
                        ddlGridSector.DataValueField = "SectorID";
                        ddlGridSector.DataBind();
                        ddlGridSector.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
                        if (!string.IsNullOrEmpty(sector))
                        {
                            try
                            {
                                ddlGridSector.Items.FindByValue(sector).Selected = true;
                            }
                            catch { }
                        }
                    }
                }
            }
        }
        private bool SubmitSaveForLater(bool is_submit)
        {

            string EmpId = "";
            try
            {
                string Brokerfirm = string.Empty;
                string AnalystName = string.Empty;
                string SectorName = string.Empty;
                string UserName = string.Empty;
                string ReasonName = string.Empty;
                string BID = string.Empty;
                int Maxlimit = 0, VID = 0, SectorID = 0;
                decimal Vote = 0;
                string fmComments = string.Empty;
                string rmComment = string.Empty;
                //string fmDOD_DOFAComments = string.Empty;
                DataTable dtadd = new DataTable();
                dtadd.Columns.Add(new DataColumn("VoteID", typeof(int)));
                dtadd.Columns.Add(new DataColumn("Vote", typeof(float)));
                dtadd.Columns.Add(new DataColumn("BrokerID", typeof(string)));
                dtadd.Columns.Add(new DataColumn("GradingID", typeof(int)));
                dtadd.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                dtadd.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));
                dtadd.Columns.Add(new DataColumn("UpdatedBy", typeof(string)));
                dtadd.Columns.Add(new DataColumn("UpdatedOn", typeof(DateTime)));
                dtadd.Columns.Add(new DataColumn("DelStatus", typeof(bool)));
                dtadd.Columns.Add(new DataColumn("MaxLimit", typeof(int)));
                dtadd.Columns.Add(new DataColumn("AnalystName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("Score", typeof(float)));
                dtadd.Columns.Add(new DataColumn("SectorID", typeof(int)));
                dtadd.Columns.Add(new DataColumn("BrokerNameId", typeof(int)));
                dtadd.Columns.Add(new DataColumn("FMComments", typeof(string)));
                dtadd.Columns.Add(new DataColumn("RMComment", typeof(string)));
                dtadd.Columns.Add(new DataColumn("BrokerName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("ReasonID", typeof(int)));
                decimal total_votes = 0;
                foreach (GridViewRow row in gvVoterMaster.Rows)
                {
                    Decimal score;
                    EmpId = EnumUserType.Admin == this.UserTypeName && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedValue : this.EmployeeId;
                    this.Weight = obj.GetWeight(EmpId);
                    this.VoteId = Convert.ToInt32((row.FindControl("hfVoterID") as HiddenField).Value == "" ? ("0") : (row.FindControl("hfVoterID") as HiddenField).Value);
                    Brokerfirm = ((TextBox)row.FindControl("txtGgridBrokerName")).Text;
                    DropDownList ddlBrokerAnalyst = (row.FindControl("ddlGridBrokerAnalyst") as DropDownList);
                    DropDownList ddlSector = (row.FindControl("ddlGridSector") as DropDownList);
                    AnalystName = (ddlBrokerAnalyst.SelectedIndex > 0) ? ddlBrokerAnalyst.SelectedItem.Text : null;
                    SectorName = ddlSector.SelectedItem != null ? ddlSector.SelectedItem.Value : null;
                    Vote = Convert.ToDecimal(((TextBox)row.FindControl("txtGridVote")).Text);
                    total_votes += Vote;
                    Maxlimit = Convert.ToInt32(((TextBox)row.FindControl("txtGridMaxLimit")).Text);
                    fmComments = (row.FindControl("txtDescription") as TextBox).Text;
                    rmComment = (row.FindControl("commentid") as TextBox).Text;
                    //fmDOD_DOFAComments = (row.FindControl("txtDescDOD_DOFA") as TextBox).Text;
                    this.BrokerNameId = obj.GetBrokerNameID(Brokerfirm);
                    //if (AnalystName == null)
                    //{
                    //    AnalystName = ((TextBox)row.FindControl("txtDescDOD_DOFA")).Text;
                    //}
                    ReasonName = ((TextBox)row.FindControl("txtDescDOD_DOFA")).Text;
                    this.ReasonID = obj.GetReasonNameID(ReasonName);
                    if (ReasonName == null || ReasonID == 0)
                    {
                        ReasonName = "Better Execution - Trades";
                        ReasonID = 1;
                       // rmComment = "";

                    }
                    //this.ReasonID = Convert.ToInt32((row.FindControl("hdnReasonID") as HiddenField).Value == "" ? ("0") : (row.FindControl("hdnReasonID") as HiddenField).Value);
                    if (!string.IsNullOrEmpty(SectorName))
                    {
                        SectorID = Convert.ToInt32(ddlSector.SelectedItem.Value);
                    }
                    else
                    {
                        SectorID = 0;
                    }
                    // BID = ddlBrokerAnalyst.SelectedItem != null && !string.IsNullOrEmpty(ddlBrokerAnalyst.SelectedItem.Value) ? ddlBrokerAnalyst.SelectedItem.Value : null;                   
                    VID = this.VoteId;
                    score = (this.Weight) * (Convert.ToDecimal(Vote));
                    dtadd.Rows.Add(VID, Vote, null, this.GradingId, this.USERNAME, null, this.USERNAME, null, false, Maxlimit, AnalystName, score, SectorID, BrokerNameId, fmComments, rmComment, Brokerfirm, ReasonID);
                    //Brokerfirm
                }
                // Below condition added by Anshu to check 100 limits only on submit
                if (is_submit && total_votes > 100)
                {

                    return true;
                }
                ViewState["CurrentTable"] = dtadd;
            }
            catch (Exception ex)
            {

                BGS.Error.EventLogProcess(ex, "EMployee id: " + EmpId);
            }

            return false;
        }
        private void EnableVoterMailNotSent()
        {

            bool isAdmin = (ddlVoterName.SelectedIndex > 0 && this.UserTypeName == EnumUserType.Admin) || this.UserTypeName == EnumUserType.voter;
            if (isAdmin)
            {
                bool Istrue = false;
                var query = bgs_entity.GetGradingStatus(this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId);
                Istrue = Convert.ToBoolean(query.FirstOrDefault().Value);
                // DisableAll(Istrue);
                if (!Istrue)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Grading is not initiated');", true);
                    return;
                }
            }
        }
        private void IsRankSent()
        {
            bool isSentRank = Convert.ToBoolean(bgs_entity.BGS_IsRankSent(this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId).FirstOrDefault().Value);
            if (isSentRank)
            {
                this.DisableAll(isSentRank);
                // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Sorry! Current quarter grading is submitted.');", true);
                return;
            }
        }
        private void DisableAll(bool isTrue)
        {
            if (isTrue == false)
            {

                btnSaveForLater.Enabled = isTrue;
                btnCancel.Enabled = isTrue;
                gvVoterMaster.Enabled = isTrue;
                btnSubmit.Enabled = isTrue;

            }
            //else
            //{
            //    if (isTrue == true)
            //    {

            //        btnSaveForLater.Enabled = isTrue;
            //        btnCancel.Enabled = isTrue;
            //        gvVoterMaster.Enabled = isTrue;
            //        btn
            //        .Enabled = isTrue;
            //    }

            //}
        }
        private void BindBrokerGrid()
        {
            DataTable dtbroker = new DataTable();
            dtbroker = obj.BindBrokerData();
            gvBroker.DataSource = dtbroker;
            gvBroker.DataBind();
            ViewState["dtmatch"] = dtbroker;
        }
        #endregion

        protected void gvVoterMaster_Sorting(object sender, GridViewSortEventArgs e)
        {
            //if (ViewState["is_sorting_allowed"] != null)
            //{
                if (!(bool)ViewState["is_sorting_allowed"])
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please do save for later first then sorting.');", true);
                    return;
                }
            //}
            else
            {
                this.GetVoteMaster(e);
            }
            /*SaveVoter((DataTable)ViewState["CurrentTable"]);
            //SubmitSaveForLater(false);
            this.GetVoteMaster(e);*/
        }

        protected void txtDescDOD_DOFA_OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                //GetGradingMasterDetails();
                //SubmitSaveForLater();
                //int i = 0;
                //if (this.RoleTypeName == "FM")
                //{
                //    string brokername = string.Empty;
                //    DataTable dt1 = new DataTable();
                //    DataTable dt2 = new DataTable();
                //    dt1 = (DataTable)ViewState["dtmatch"];
                //    dt2 = (DataTable)ViewState["CurrentTable"];
                //    var query1 = from C in dt1.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                //    var query2 = from C in dt2.AsEnumerable() select new { BrokerName = C.Field<int>("BrokerNameId") };
                //    var except = query2.Except(query1).AsEnumerable();
                //    foreach (var arr in except)
                //    {
                //        brokername += arr + ",";
                //        i++;
                //        if (i > 3)
                //        {
                //            break;
                //        }
                //    }

                //    if (brokername != null && i <= 3)
                //    {
                //        //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                //        SaveVoter((DataTable)ViewState["CurrentTable"]);
                //        bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                //        this.GetGradingMasterDetails();
                //        this.GetVoteMaster();
                //        Enabledissable();
                //        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                //    }
                //    else
                //    {
                //        //this.GetVoteMaster();
                //        SetBackGroundColor(gvVoterMaster, brokername);
                //        //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can vote against those broker/firm. whos broker/firm are available in list');", true);
                //        return;
                //    }
                //}
                //else
                //{
                //    //obj.Insertupdatevoter((DataTable)ViewState["CurrentTable"]);
                //    SaveVoter((DataTable)ViewState["CurrentTable"]);
                //    bgs_entity.BGS_UpdateVoteMasterStatus(null, this.GradingId, this.UserTypeName == EnumUserType.Admin && ddlVoterName.SelectedIndex > 0 ? ddlVoterName.SelectedItem.Value : this.EmployeeId, this.UserTypeName);
                //    this.GetGradingMasterDetails();
                //    this.GetVoteMaster();
                //    Enabledissable();
                //    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record save successfully.');", true);

                //}

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        //protected void ddlDescDOD_DOFA_TextChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.GetGradingMasterDetails();
        //        TextBox txtBrokerName = (TextBox)sender;
        //        GridViewRow Row = (GridViewRow)txtBrokerName.NamingContainer;
        //        DropDownList ddlDescDOD_DOFA = (DropDownList)Row.FindControl("ddlDescDOD_DOFA");
        //        ddlDescDOD_DOFA.DataSource = obj.BindReasons();
        //        ddlDescDOD_DOFA.DataTextField = "ReasonName";
        //        ddlDescDOD_DOFA.DataValueField = "ReasonID";
        //        ddlDescDOD_DOFA.DataBind();
        //        ddlDescDOD_DOFA.Items.Insert(0, (new ListItem { Text = "Please Select ", Value = string.Empty }));
        //        if (this.RoleTypeName == "FM")
        //        {
        //            TextBox txtMaxLimit = (Row.FindControl("txtGridMaxLimit") as TextBox);
        //            txtMaxLimit.Text = Convert.ToString(obj.GetMaxLimit(txtBrokerName.Text.Trim()));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BGS.Error.EventLogProcess(ex);
        //    }
        //}

    }
}