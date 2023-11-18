using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;

namespace BGS
{
    public partial class UserMaster_Page : System.Web.UI.Page
    {
        #region Properties
        Datalayer dl = new Datalayer();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString());
        BGSEntities bgs_entity = new BGSEntities();
        #endregion

        #region Declaration
        string StrPassword = string.Empty;
        string Strempidmail = string.Empty;
        static string glob;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //btnSave.Attributes.Add("onclick", "javascript:return SaveDate();");
            //txtUserID.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            txtWeight.Attributes.Add("onkeydown", "return (event.keyCode!=13);");


            if (!this.IsPostBack)
            {
                mvTabs.SetActiveView(vwTab1);
                this.PopulateUserMasterGrid("0");
                //this.bindexcellist("0");
                this.PopulateRoleName();
                this.PopulateDepartments();
                this.PopulateUserTypes();
                lblName8.Visible = false;
                lblBrokerFirmname.Visible = false;
                txtBrokerName.Visible = false;
                rfvBrokerName.Visible = false;
                lblMaxlimit18.Visible = false;
                lblMaxLimit.Visible = false;
                txtMaxLimit.Visible = false;
                rfvMaxLimit.Visible = false;
                lblDofaCode.Visible = false;
                txtDofaCode.Visible = false;
                lblCode.Visible = false;
                
                //if (Session["EmpID"] == null)
                //{
                //    Response.Redirect("LoginMaster.aspx");
                //}
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void PopulateUserMasterGrid(string strE)

        {
            try
            {
                DataTable dt = new DataTable();
                grdUserRight.DataSource = null;
                grdUserRight.DataBind();

                Datalayer dl = new Datalayer();
                dt = dl.GetUsers(null);

                grdUserRight.DataSource = dt;
                grdUserRight.DataBind();

                mvTabs.SetActiveView(vwTab1);
                clearData();
            }
            catch (Exception ex)
            {
            }
        }

        private void bindexcellist()
        {
            DataTable dt = new DataTable();
            grdUserRight.DataSource = null;
            grdUserRight.DataBind();
            Datalayer dl = new Datalayer();
            dt = dl.GetUsers(null);
           
            gvExcel.DataBind();
        }

        /// <summary>
        /// redirect to create user page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CreateNewUser(object sender, EventArgs e)
        {
            clearData();

            chkIsActive.Checked = true;
            mvTabs.SetActiveView(vwTab2);

            txtUserID.Enabled = true;
            lblUserID.Enabled = true; 
            //txtOther.Enabled = true;
            txtPassword.Enabled = false;
            btnAddUser.Visible = false;
            btnAddUser.Attributes["NewUser"] = "Y";
            tblSearch.Visible = false;
            btnSave.Text = "Save";
        }

        public void clearData()
        {
            txtUserID.Text = string.Empty;
            txtOther.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtName.Text = string.Empty;
            txtBrokerName.Text = string.Empty;
            txtDofaCode.Text = string.Empty;
            chkIsActive.Checked = false;
            rblType.Enabled = true;
           
            //added by prakash on 26/05/2016
            if (rdbtnl.SelectedIndex == 1)
            {
                rblType.SelectedIndex = 1;
                ShowBroker_Firm();
                
            }
            else
            {
                rblType.SelectedIndex =0;
                ShowVoter();
            }
            //added by prakash on 26/05/2016
           // rblType.SelectedIndex = 0;
            ddlDept.SelectedIndex = 0;
            ddlRole.ClearSelection();
            ddlUserType.ClearSelection();
            txtWeight.Text = string.Empty;

        }
        public void ShowVoter()
        {
            trUTI.Visible = true;
            trOther.Visible = false;
            trWeight.Visible = true;
            lblRole.Visible = true;
            ddlRole.Visible = true;
            rfvRole.Visible = true;
            lblName7.Visible = true;
            lblDept.Visible = true;
            ddlDept.Visible = true;
            rfvDept.Visible = true;
            lblName9.Visible = true;
            lblName12.Visible = true;
            lblUserType.Visible = true;
            ddlUserType.Visible = true;
            rfvUserType.Visible = true;
            lblName8.Visible = false;
            lblBrokerFirmname.Visible = false;
            txtBrokerName.Visible = false;
            rfvBrokerName.Visible = false;
            lblMaxlimit18.Visible = false;
            lblMaxLimit.Visible = false;
            txtMaxLimit.Visible = false;
            rfvMaxLimit.Visible = false;
            //txtMaxLimit.Text = "30";
            lblCode.Visible = false;
            lblDofaCode.Visible = false;
            txtDofaCode.Visible = false;
            Label3.Visible = false;
            lblOtherID.Visible = false;
            txtOther.Visible = false;
            lblName.Text = "Name:";
        }
        public void ShowBroker_Firm()
        {
            trUTI.Visible = false;
            trOther.Visible = true;
            trWeight.Visible = false;
            lblRole.Visible = false;
            ddlRole.Visible = false;
            rfvRole.Visible = false;
            lblName7.Visible = false;
            lblDept.Visible = false;
            ddlDept.Visible = false;
            rfvDept.Visible = false;
            lblName9.Visible = false;
            lblName12.Visible = false;
            lblUserType.Visible = false;
            ddlUserType.Visible = false;
            rfvUserType.Visible = false;
            lblName8.Visible = true;
            lblBrokerFirmname.Visible = true;
            txtBrokerName.Visible = true;
            rfvBrokerName.Visible = true;
            lblMaxlimit18.Visible = true;
            lblMaxLimit.Visible = true;
            txtMaxLimit.Visible = true;
            txtMaxLimit.Text = "30";
            rfvMaxLimit.Visible = true;
            lblCode.Visible = false;
            lblDofaCode.Visible = true;
            txtDofaCode.Visible = true;
            Label3.Visible = false;
            lblOtherID.Visible = false;
            txtOther.Visible = false;
            lblName.Text = "Broker Contact Person Name:";
        }

        /// <summary>
        /// GET ROLES
        /// </summary>
        private void PopulateRoleName()
        {
            DataTable dt = new DataTable();
            ddlRole.DataSource = null;// GetRoleDeptDetails table[0]
            ddlRole.DataBind();

            Datalayer dl = new Datalayer();
            dt = dl.GetRoles();
            ddlRole.DataSource = dt;
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
        }

        /// <summary>
        /// GET USER TYPES
        /// </summary>
        private void PopulateUserTypes()
        {
            DataTable dt = new DataTable();
            ddlUserType.DataSource = null;// GetRoleDeptDetails table[0]
            ddlUserType.DataBind();

            Datalayer dl = new Datalayer();
            dt = dl.GetUserTypes(rblType.SelectedItem.Text);
            ddlUserType.DataSource = dt;
            ddlUserType.DataTextField = "UserType";
            ddlUserType.DataValueField = "UserTypeID";
            ddlUserType.DataBind();
            ddlUserType.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
        }

        /// <summary>
        /// GET DEPARTMENTS
        /// </summary>
        private void PopulateDepartments()
        {
            DataTable dt = new DataTable();
            ddlDept.DataSource = null;// GetRoleDeptDetails table[1]
            ddlDept.DataBind();

            Datalayer dl = new Datalayer();
            dt = dl.GetDepartments();

            ddlDept.DataSource = dt;
            ddlDept.DataTextField = "DeptName";
            ddlDept.DataValueField = "DeptID";
            ddlDept.DataBind();
            ddlDept.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
        }

        protected void CancelUserDetails(object sender, EventArgs e)
        {
            mvTabs.SetActiveView(vwTab1);
            btnAddUser.Attributes["NewUser"] = "";
            btnAddUser.Visible = true;
            tblSearch.Visible = true;
            txtUserSearch.Text = string.Empty;            
            rdbtnl.ClearSelection();
            //rdbtnl.SelectedIndex = 1;
            this.PopulateUserMasterGrid("0");
        }
        protected void btnreset_Click(object sender, EventArgs e)
        {
            txtUserSearch.Text = string.Empty;
            rdbtnl.ClearSelection();
            //rdbtnl.SelectedIndex = 1;
        }
        protected void SaveUserDetails(object sender, EventArgs e)
        {
            Boolean IsForce;
            int i = 1;
            int intBrokerNameID = 0;
            int intUserID = 0;
            if (btnAddUser.Attributes["NewUser"] == "Y")
            {
                IsForce = true;
                StrPassword = "UTIAMCI";
            }
            else
            {
                IsForce = false;
                StrPassword = txtPassword.Text;

            }
            if (rblType.SelectedItem.Value == "0")
            { StrPassword = CreateRandomPassword(10); }

            #region Insert in user master

            //string strEmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text : txtOther.Text;//cmt by shashi 07-08-2018
            string strEmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text : txtEmailID.Text;
            string strPWD = StrPassword;
            int intRole = (ddlRole.SelectedItem.Value.Length > 0) ? int.Parse(ddlRole.SelectedItem.Value) : 0;
            int intDept = (ddlDept.SelectedItem.Value.Length > 0) ? int.Parse(ddlDept.SelectedItem.Value) : 0;
            bool blActive = chkIsActive.Checked;
            string strEmailID = (txtEmailID.Text.Length > 0) ? txtEmailID.Text : null;
            string strName = (txtName.Text.Length > 0) ? (txtName.Text) : (null);
            bool blIsForce = IsForce;
            bool blIsUTI = Convert.ToBoolean(int.Parse(rblType.SelectedItem.Value));
            string strCreatedBy = "1";//Session["EmpID"].ToString();
            float fltWeight = (txtWeight.Text.Length > 0) ? float.Parse(txtWeight.Text) : (0);
            int intUType = (ddlUserType.SelectedItem.Value.Length > 0) ? int.Parse(ddlUserType.SelectedItem.Value) : 0;
            intBrokerNameID =Convert.ToInt32(Session["BrokerNameID"]) > 0? Convert.ToInt32(Session["BrokerNameID"]):0;
            intUserID = Convert.ToInt32(Session["UserID"]) > 0 ? Convert.ToInt32(Session["BrokerNameID"]):0;
            string strBtokerName = txtBrokerName.Text.Length > 0 ? txtBrokerName.Text.ToUpper().Trim() : null;
            int intDofaCode= txtDofaCode.Text!="" ? int.Parse(txtDofaCode.Text) : 0;
            try
            {
                if (rblType.Enabled != false)
                {
                    Datalayer dl = new Datalayer();
                    if (blIsUTI == true)
                    {
                        dl.InsertUpdateUser(strEmpID, strPWD, intRole, intDept, blActive, strEmailID, strName, blIsForce, blIsUTI, strCreatedBy, fltWeight, intUType, "", 0, 0, 0,0);

                    }
                    else
                    {
                        intUType = 3;
                        int maxlimit = Convert.ToInt32(txtMaxLimit.Text);
                        dl.InsertUpdateUser(strEmpID, strPWD, intRole, intDept, blActive, strEmailID, strName, blIsForce, blIsUTI, strCreatedBy, fltWeight, intUType, strBtokerName, maxlimit, intDofaCode, intBrokerNameID, intUserID);

                    }
                    #region Email Script
                    if (btnAddUser.Attributes["NewUser"] == "Y")
                    {
                        if (blIsUTI != true)
                        {
                            string strbody = StrPassword;
                            //string EmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text.Trim() : txtOther.Text.Trim();//cmt by shashi 07-08-2018
                            string EmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text.Trim() : txtEmailID.Text.Trim();
                            // sendEmail(txtEmailID.Text, strbody, "Employee Password", strAttachFile, true);
                            try
                            {
                                SendMail(txtEmailID.Text, true, txtName.Text, EmpID, strbody);
                                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record Saved Successfully');", true);
                            }
                            catch (Exception ex)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Could not send mail to following ids:" + txtEmailID.Text + "');", true);
                                BGS.Error.EventLogProcess(ex);
                            }
                        }
                    }
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record Saved Successfully');", true);
                    #endregion
                }
                else {
                    Datalayer dl = new Datalayer();
                    if (blIsUTI == true)
                    {
                        dl.InsertUpdateUser(strEmpID, strPWD, intRole, intDept, blActive, strEmailID, strName, blIsForce, blIsUTI, strCreatedBy, fltWeight, intUType, "", 0, 0, 0,0);

                    }
                    else
                    {
                        intUType = 3;
                        int maxlimit = Convert.ToInt32(txtMaxLimit.Text);
                        dl.InsertUpdateUser(strEmpID, strPWD, intRole, intDept, blActive, strEmailID, strName, blIsForce, blIsUTI, strCreatedBy, fltWeight, intUType, strBtokerName, maxlimit, intDofaCode, intBrokerNameID, intUserID);

                    }
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record Updated Successfully');", true);
                }

                //bgs_entity.BGS_InsertUpdateUsers("INSERT", this.BrokerId, brokerCode, strSectorId, strBrokerTYpeId, strMaxLimit, strName, strNumber, strEmail, null, null);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            #endregion
            Session["IsForce"] = "";

            //if (i == 1)
            //{
            ////string Message = "Record Saved Successfully";
            ////ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Message + "');", true);

            
           
           // ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record Saved Successfully');", true);
            
            CancelUserDetails(sender, e);// added by prakash on 25-05-2016

            //6 Response.Redirect("~/UserMaster.aspx");
        }
        protected void txtUserSearch_TextChanged(object sender, EventArgs e)
        {
            BindUser(txtUserSearch.Text.Trim(), null);
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }
        protected void EditUserDetails(object sender, EventArgs e)
        {
            String StrUserID = (sender as Button).CommandArgument.ToString();
            try
            {
                Datalayer dl = new Datalayer();
                DataTable dt = new DataTable();
                txtMaxLimit.Text = string.Empty;
                dt = dl.GetUsers(StrUserID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    rblType.ClearSelection();
                    rblType.Enabled = false;
                    //tblUser.Attributes.Add("style", "display:block");
                    rblType.Items.FindByValue(Convert.ToBoolean(dt.Rows[0]["IsUTI"]) == true ? "1" : "0").Selected = true;
                    txtPassword.Text = dt.Rows[0]["Password"].ToString();
                    if (Convert.ToBoolean(dt.Rows[0]["IsUTI"]))
                    {
                        txtUserID.Text = dt.Rows[0]["EmpID"].ToString();
                        txtUserID.Enabled = false;
                        trUTI.Visible = true;
                        trOther.Visible = false;
                    }
                    else
                    {
                        /* cmt by shashi 07-08-2018
                        txtOther.Text = dt.Rows[0]["EmpID"].ToString();
                        txtOther.Enabled = false;*/
                        trUTI.Visible = false;
                        trOther.Visible = true;
                    }
                    txtName.Text = dt.Rows[0]["UserName"].ToString();
                    txtEmailID.Text = dt.Rows[0]["EmailID"].ToString();

                    if (dt.Rows[0]["status"].ToString() == "Activate")
                    {
                        chkIsActive.Checked = false;
                    }
                    else
                    {
                        chkIsActive.Checked = true;
                    }
                    txtUserID.Enabled = false;
                    txtPassword.Enabled = true;
                }

                if (Convert.ToBoolean(dt.Rows[0]["IsUTI"].ToString()) != true)
                {
                    ddlRole.SelectedValue = null;
                    ddlDept.SelectedValue = null;
                    ddlUserType.SelectedValue = null;
                    txtBrokerName.Text = dt.Rows[0]["BrokerName"].ToString();
                    txtMaxLimit.Text = dt.Rows[0]["MaxLimit"].ToString();
                    Session["BrokerNameID"] = hfBrokerNameID.Value= dt.Rows[0]["BrokerNameID"].ToString()==""? "0":dt.Rows[0]["BrokerNameID"].ToString();
                    trWeight.Visible = false;
                    lblRole.Visible = false;
                    ddlRole.Visible = false;
                    rfvRole.Visible = false;
                    lblName7.Visible = false;
                    lblDept.Visible = false;
                    rfvDept.Visible = false;
                    ddlDept.Visible = false;
                    lblName9.Visible = false;
                    lblName12.Visible = false;
                    lblUserType.Visible = false;
                    ddlUserType.Visible = false;
                    rfvUserType.Visible = false;
                    lblName8.Visible = true;
                    lblBrokerFirmname.Visible = true;
                    txtBrokerName.Visible = true;
                    rfvBrokerName.Visible = true;
                    lblMaxlimit18.Visible = true;
                    lblMaxLimit.Visible = true;
                    txtMaxLimit.Visible = true;
                    rfvMaxLimit.Visible = true;
                    lblDofaCode.Visible = true;
                    txtDofaCode.Visible = true;
                    lblCode.Visible = false;
                    txtDofaCode.Text = dt.Rows[0]["DofaCode"].ToString();
                    Label3.Visible = false;
                    lblOtherID.Visible = false;
                    txtOther.Visible = false;
                    lblName.Text = "Broker Contact Person Name:";
                   // Session["UserID"] = dt.Rows[0]["UserID"].ToString() == "" ? "0" : dt.Rows[0]["UserID"].ToString();
                }
                else
                {
                    ddlRole.SelectedValue = dt.Rows[0]["RoleID"].ToString();
                    ddlDept.SelectedValue = dt.Rows[0]["DeptID"].ToString();
                    ddlUserType.SelectedValue = dt.Rows[0]["UserTypeID"].ToString();
                    Session["UserID"] = dt.Rows[0]["UserID"].ToString()==""?"0": dt.Rows[0]["UserID"].ToString();
                }
                txtWeight.Text = dt.Rows[0]["Weight"].ToString();
                mvTabs.SetActiveView(vwTab2);
                btnAddUser.Visible = false;
                tblSearch.Visible = false;
                btnSave.Text = "Update";
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                //string Message = ex.Message.ToString();
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Message + "');", true);
                //ScriptManager.RegisterStartupScript(Page, this.GetType(), "alert", string.Format("alert('{0}');", Message), true);
            }
            finally
            {
            }
        }
        protected void DeactivateUser(object sender, EventArgs e)
        {
            try
            {
                int i = 1;
                bool IsActive = false;
                Boolean IsForce = true;
                Datalayer dl = new Datalayer();
                DataTable dt = new DataTable();
                String StrUserID = (sender as Button).CommandArgument.ToString();
                string strCreatedBy = "1";// Session["EmpID"].ToString();
                dt = dl.GetUsers(StrUserID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["status"].ToString() == "Activate")
                    {
                        IsActive = true;
                    }
                    else
                    {
                        IsActive = false;
                    }

                    if (dt.Rows[0]["Isforce"].ToString() == "0")
                    {
                        IsForce = false;
                    }
                    else
                    {
                        IsForce = true;
                    }
                    dl.ActiveDeactiveUser(StrUserID, IsActive, IsForce, strCreatedBy);
                    PopulateUserMasterGrid(null);
                    //bgs_entity.BGS_InsertUpdateUsers(ds.Tables[0].Rows[0]["EmpID"].ToString(), ds.Tables[0].Rows[0]["Password"].ToString(), ds.Tables[0].Rows[0]["RoleID"].ToString(), ds.Tables[0].Rows[0]["DeptID"].ToString(), IsActive, ds.Tables[0].Rows[0]["EmailID"].ToString(), ds.Tables[0].Rows[0]["UserName"].ToString(), IsForce, Boolean.Parse(ds.Tables[0].Rows[0]["IsUTI"].ToString()), strCreatedBy);
                }
                if (dt.Rows[0]["status"].ToString() == "Activate")
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('User activated Successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('User deactivated Successfully.');", true);
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Data not saved Successfully.');", true);
            }
        }
        protected void CheckIsExist(object sender, EventArgs e)
        {
            //if (rblType.Enabled == true)
            //{
                //string strEmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text : txtOther.Text;
                string strEmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text : txtEmailID.Text;
                string emailId = txtEmailID.Text.Trim();
                //var query = bgs_entity.IsUserExists(strEmpID, emailId, !string.IsNullOrEmpty(hfBrokerNameID.Value) ? Convert.ToInt32(hfBrokerNameID.Value) : 0, !string.IsNullOrEmpty(txtDofaCode.Text) ? Convert.ToInt32(txtDofaCode.Text) : 0);//Cmt by shashi 16-08-2018
                var query = bgs_entity.IsUserExists(strEmpID, emailId, txtBrokerName.Text!="" ? txtBrokerName.Text : "", !string.IsNullOrEmpty(txtDofaCode.Text) ? Convert.ToInt32(txtDofaCode.Text) : 0);
                bool isExist = Convert.ToBoolean(query.FirstOrDefault().Value);
                trIsUserExists.Visible = isExist;
                //if(isExist==true)
                //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + Message + "'Record Saved Successfully');", true);
               // trButton.Visible = !isExist;
            //}
        }
        //protected void CheckDofaCodeIsExist(object sender, EventArgs e)
        //{
        //    string strEmpID = rblType.SelectedItem.Value == "1" ? txtUserID.Text : txtOther.Text;
        //    string emailId = txtEmailID.Text.Trim();
        //    var query = bgs_entity.IsUserExists(strEmpID, emailId, !string.IsNullOrEmpty(hfBrokerNameID.Value) ? Convert.ToInt32(hfBrokerNameID.Value) : 0,txtDofaCode.Text);
        //    bool isExist = Convert.ToBoolean(query.FirstOrDefault().Value);
        //    trIsUserExists.Visible = isExist;
        //    trButton.Visible = !isExist;
        //}
        protected void AssignScreens(object sender, EventArgs e)
        {
            string name = (sender as Button).CommandName;
            string empId = (sender as Button).CommandArgument;
            Response.Redirect("~/ScreenAllocation.aspx?empid=" + empId + "&name=" + name);
        }
        protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedItem.Value == "1")
            {
                trUTI.Visible = true;
                trOther.Visible = false;
                trWeight.Visible = true;
                lblName8.Visible = false;
                lblBrokerFirmname.Visible = false;
                txtBrokerName.Visible = false;
                rfvBrokerName.Visible = false;
                lblRole.Visible = true;
                ddlRole.Visible = true;
                rfvRole.Visible = true;
                lblName7.Visible = true;
                lblDept.Visible = true;
                ddlDept.Visible = true;
                rfvDept.Visible = true;
                lblName9.Visible = true;
                lblName12.Visible = true;
                lblUserType.Visible = true;
                ddlUserType.Visible = true;
                rfvUserType.Visible = true;
                lblMaxlimit18.Visible = false;
                lblMaxLimit.Visible = false;
                txtMaxLimit.Visible = false;
                rfvMaxLimit.Visible = false;
                lblDofaCode.Visible = false;
                txtDofaCode.Visible = false;
                lblCode.Visible = false;
                lblName.Text = "Name:";
            }
            else
            {
                trUTI.Visible = false;
                trOther.Visible = true;
                trWeight.Visible = false;
                lblRole.Visible = false;
                ddlRole.Visible = false;
                rfvRole.Visible = false;
                lblName7.Visible = false;
                lblDept.Visible = false;
                ddlDept.Visible = false;
                rfvDept.Visible = false;
                lblName9.Visible = false;
                lblName12.Visible = false;
                lblUserType.Visible = false;
                ddlUserType.Visible = false;
                rfvUserType.Visible = false;
                lblName8.Visible = true;
                lblBrokerFirmname.Visible = true;
                txtBrokerName.Visible = true;
                rfvBrokerName.Visible = true;
                lblMaxlimit18.Visible = true;
                lblMaxLimit.Visible = true;
                txtMaxLimit.Visible = true;
                txtMaxLimit.Text = "30";
                rfvMaxLimit.Visible = true;
                lblDofaCode.Visible = true;
                txtDofaCode.Visible = true;
                lblCode.Visible = false;
                Label3.Visible = false;
                lblOtherID.Visible = false;
                txtOther.Visible = false;
                lblName.Text = "Broker Contact Person Name:";
            }
            PopulateUserTypes();
        }
        protected void rdbtnl_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUserSearch.Text = string.Empty;
            BindUser(null, rdbtnl.SelectedValue);
            btnAddUser.Text = rdbtnl.SelectedValue == "1" ? "Create User" : "Create Broker";
        }
        private void SendMail(string strToEmailAdd, bool allowHTML, string name, string strloginId, string Password)
        {
            try
            {
                string strMailServer = ConfigurationManager.AppSettings.Get("MailServer").ToString();
                string strSenderMailID = ConfigurationManager.AppSettings.Get("SenderMailID").ToString();
                string strSenderMailPwd = ConfigurationManager.AppSettings.Get("SenderMailPwd").ToString();
                string strHeader = ConfigurationManager.AppSettings["HeaderKey"].ToString();
                string strFooter = ConfigurationManager.AppSettings["FooterKey"].ToString();
                string strMailBody = ConfigurationManager.AppSettings["MailToNextLevelApproversBody"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                string AppUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();
                string ISUTI = ConfigurationManager.AppSettings["ISUTI"].ToString();
                string strToMailAdd = string.Empty;
                SmtpClient emailClient = new SmtpClient(strMailServer,Convert.ToInt32(port));
                MailMessage message = new MailMessage();
                message.IsBodyHtml = allowHTML;
                MailAddress maSender = new MailAddress(strSenderMailID);
                message.Sender = maSender;
                message.From = maSender;
                message.Subject = string.Format("{0}", " New User Registration");
                CommonFunctions cf = new CommonFunctions();
                strHeader = cf.PopulateBody("NewBrokerRegistered");
                strHeader = strHeader.Replace("{#URL}", AppUrl);
                strHeader = strHeader.Replace("{#UID}", strloginId);
                strHeader = strHeader.Replace("{#pwd}", Password);
                message.Body = strHeader;
                message.To.Add(strToEmailAdd);
                //System.Net.Mail.Attachment attach1 = new System.Net.Mail.Attachment(strattachFile);
                // message.Attachments.Add(attach1); 
                emailClient.Credentials = new System.Net.NetworkCredential(strSenderMailID, strSenderMailPwd);               
                emailClient.UseDefaultCredentials = true;//cmt by shashi 10 Aug 2018
                emailClient.EnableSsl = true;
                emailClient.Send(message);
            }
            catch (SmtpException ex)
            {
                throw;
            }
        }
        private void BindUser(string Search, string Utype)
        {
            DataTable dtuser = new DataTable();
            dtuser = dl.BindUserBySearch(Search, Utype);
            grdUserRight.DataSource = dtuser;
            grdUserRight.DataBind();
        }

        protected void txtUserID_TextChanged(object sender, EventArgs e)
        {
            SqlParameter[] Param = { new SqlParameter("@UserId", txtUserID.Text) };
            DataSet ds = dl.ExecuteProcedure("CheckUserIdIsExist", Param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
               // DataTable dt = ds.Tables[0];
                string Message = dt.Rows[0][0].ToString();
                Message = "This Userid Already Allocated by :" + Message;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + Message + "');", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('This Userid Already Exists.');", true);
            }
        }

        public void btnExportExcel_Click(object sender, EventArgs e)
        {
            gvExcel.Visible = true;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=userList.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            Datalayer dl = new Datalayer();
            var Query = dl.GetUsers(null);
            gvExcel.DataSource = Query;
            gvExcel.DataBind();
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvExcel.AllowPaging = false;
                //gvExcel.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gvExcel.HeaderRow.Cells)
                {
                    cell.BackColor = gvExcel.HeaderStyle.BackColor;
                    // gvBrokerEmailList.HeaderRow.Cells[0].Visible = false;
                }
                foreach (GridViewRow row in gvExcel.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvExcel.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvExcel.RowStyle.BackColor;
                        }
                        // row.Cells[0].Visible = false;
                        // cell.CssClass = "textmode";

                    }
                }

                gvExcel.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        //protected void btnExportExcel_Click(object sender, EventArgs e)
        //{
        //    gvExcel.Visible = true;
        //    Response.Clear();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition", "attachment;filename=RankReport.xls");
        //    Response.Charset = "";
        //    Response.ContentType = "application/vnd.ms-excel";
        //    DataTable dt = new DataTable();
        //    gvExcel.DataSource = null;
        //    gvExcel.DataBind();
        //    Datalayer dl = new Datalayer();
        //    dt = dl.GetUsers(null);

        //    gvExcel.DataBind();
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        HtmlTextWriter hw = new HtmlTextWriter(sw);
        //        gvExcel.AllowPaging = false;
        //        gvExcel.HeaderRow.BackColor = System.Drawing.Color.White;
        //        foreach (TableCell cell in gvExcel.HeaderRow.Cells)
        //        {
        //            cell.BackColor = gvExcel.HeaderStyle.BackColor;
        //            // gridviewexcel.HeaderRow.Cells[0].Visible = false;
        //        }
        //        foreach (GridViewRow row in gvExcel.Rows)
        //        {
        //            row.BackColor = System.Drawing.Color.White;
        //            foreach (TableCell cell in row.Cells)
        //            {
        //                if (row.RowIndex % 2 == 0)
        //                {
        //                    cell.BackColor = gvExcel.AlternatingRowStyle.BackColor;
        //                }
        //                else
        //                {
        //                    cell.BackColor = gvExcel.RowStyle.BackColor;
        //                }
        //                // row.Cells[0].Visible = false;
        //                // cell.CssClass = "textmode";

        //            }
        //        }

        //        gvExcel.RenderControl(hw);
        //        string style = @"<style> .textmode { } </style>";
        //        Response.Write(style);
        //        Response.Output.Write(sw.ToString());
        //        Response.Flush();
        //        Response.End();
        //    }


        //}
    }

}