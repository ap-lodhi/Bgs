using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.DirectoryServices;
using System.Configuration;
using System.Linq;


namespace BGS
{
    public partial class LoginMaster_Page : PageBase
    {
        //Boolean flag = false;

        BGSEntities bgs_entities = new BGSEntities();
        Datalayer dl = new Datalayer();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Session.Abandon();
            }
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (captchacode.Text=="")
                {
                    lblErrorMsg.Text = "Please Enter captcha";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                    return;
                }
                    if (captchacode.Text == Session["sessionCaptcha"].ToString())
                {

                    BGS.Error.EventLogProcess("Ok");

                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();
                    bool isUTI = false;
                    bool isActive;
                    Session["EmpID"] = txtLoginID.Text;
                    var query = (from c in bgs_entities.Check_UserAuthentication(txtLoginID.Text, txtPassword.Text) select new { c.EMPID, c.ISFORCE, c.ISUTI, c.ROLEID, c.Isactive, c.USERNAME, c.UserTypeName, c.GradingID }).FirstOrDefault();

                    BGS.Error.EventLogProcess("Query response :" + query);
                    //var query = (from c in bgs_entities.Check_UserAuthentication(txtLoginID.Text, null) select new { c.EMPID, c.ISFORCE, c.ISUTI, c.ROLEID, c.Isactive, c.USERNAME, c.UserTypeName, c.GradingID }).FirstOrDefault();
                    if (query != null)
                    {
                        isActive = Convert.ToBoolean(query.Isactive);
                        if (isActive == true)
                        {
                            Session["Users"] = query;
                            this.ISUTI = query.ISUTI.Value;
                            this.ISFORCE = query.ISFORCE.Value;
                            this.ROLEID = query.ROLEID.HasValue ? query.ROLEID.Value : 0;
                            this.USERNAME = query.USERNAME;
                            this.EmployeeId = query.EMPID;
                            this.UserTypeName = query.UserTypeName;
                            this.GradingId = query.GradingID;


                            if (CheckUserAccessebility())
                            {
                                isUTI = query.ISUTI.Value;

                                if (isUTI)
                                {
                                    UTIUserAuthenticate(this.UserTypeName);
                                }
                                else
                                {
                                    OtherUserAuthenticate(this.UserTypeName);
                                }
                            }
                            else
                            {
                                Session.Abandon();
                                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('Voting not started');", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('User is deactivated');", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('Invalid UserID or password');", true);
                    }
                    //for captcha
                }
                else
                {
                    lblErrorMsg.Text = "Invalid captcha code Please correct Code.";
                    lblErrorMsg.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        #region Private Methods
        /// <summary>
        /// UTI User Authenticate
        /// </summary>
        private void UTIUserAuthenticate(string type)
        {
            System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("Bit", typeof(bool));
            bool AuthenticateIsUti = Convert.ToBoolean(bgs_entities.BGS_CheckUtiUser(this.EmployeeId, output));
            //if (AuthenticateIsUti != true)
            if (!CheckUser()) // Remove Later for ADP
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('Invalid UserID');", true);
                return;
            }
            else
            {
                RedirectToRespectivePage(type);
            }
        }

        private void RedirectToRespectivePage(string type)
        {
            switch (type)
            {
                case EnumUserType.Broker:
                    Response.Redirect("~/BrokerMaster.aspx", false);
                    break;
                case EnumUserType.voter:
                    Response.Redirect("~/VoterScreen.aspx", false);
                    break;
                default:
                    Response.Redirect("~/GradingMaster.aspx", false);
                    break;
            }
        }

        /// <summary>
        /// Other User Authenticate
        /// </summary>
        private void OtherUserAuthenticate(string type)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = dl.IsAuthenticateBroker(txtLoginID.Text, txtPassword.Text);
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isValid"].ToString() == "1")
                    {
                        RedirectToRespectivePage(type);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('Invalid UserId and Password.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, typeof(UpdatePanel), " Message", "alert('Please enter correct user name and password.');", true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Is Voter Valid For Login
        /// </summary>
        /// <returns></returns>
        private bool IsVoterValidForLogin()
        {
            var isBool = (from c in bgs_entities.BGS_IsVendorValid(this.EmployeeId) select new { c.Value }).FirstOrDefault().Value;
            return isBool;
        }

        /// <summary>
        /// Check User Accessebility
        /// </summary>
        /// <returns></returns>
        private bool CheckUserAccessebility()
        {
            bool isAccess = false;
            switch (this.UserTypeName)
            {
                case EnumUserType.Admin:
                    isAccess = true;
                    break;
                case EnumUserType.Broker:
                    isAccess = true;
                    break;
                case EnumUserType.voter:
                    isAccess = IsVoterValidForLogin();
                    break;
            }
            return isAccess;
        }
       
        bool CheckUser()
        {
            return true;
            // CustomErrorLogs objEL = new CustomErrorLogs();
            bool status = false;
            DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["ADConnectionString"].ToString(), txtLoginID.Text.Trim().ToString(), txtPassword.Text.Trim().ToString());
            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + txtLoginID.Text.Trim().ToString() + ")";

                search.PropertiesToLoad.Add("cn");

                SearchResult result = search.FindOne();

                if (null == result)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
                BGS.Error.EventLogProcess(ex);
                // objEL.WriteToEventLog(ex.Message + ex.Source + ex.InnerException);
            }
            return status;
        }

        #endregion

        #region General Methods

        public static string GetUrl()
        {
            return "~/LoginMaster.aspx";
        }

        #endregion
    }
}
