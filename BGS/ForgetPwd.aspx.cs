using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Text;

namespace BGS
{
    public partial class ForgetPwd : System.Web.UI.Page
    {


        // string strFromMailAdd = ConfigurationSettings.AppSettings["SenderAddressKey"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtLoginID.Attributes.Add("onkeydown", "JavaScript:return isNumericKeyStroke();");
            }
            // DecisionLayer dLayer = new DecisionLayer();
            // dLayer.InsertUserAuditDetails(Session["EmpID"].ToString(), "Forgot password");
        }
        static string StrPassword = string.Empty;
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                Datalayer dl = new Datalayer();
                string strAttachFile = string.Empty;
                StrPassword = CreateRandomPassword(8);
                dt = dl.GetUSer(txtLoginID.Text, StrPassword);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string strbody = "Your New Password for BGS Application is " + StrPassword;
                    sendEmail(dt.Rows[0]["UserName"].ToString(), dt.Rows[0]["EmailID"].ToString(), strbody, "Employee Password for BGS", strAttachFile, true);
                   // string appUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();
                   // String strscript = "<script>alert('New password has been E_mailed to you.');window.location.href='" + appUrl + "'</script>";
                    //if (!ClientScript.IsClientScriptBlockRegistered("clientscript"))
                    //{
                    //    string Message = "New password ans url has been E_mailed to you";
                    //    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Message + "');", true);
                    //    ClientScript.RegisterStartupScript(this.GetType(), "clientscript", strscript);
                    //}
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('New password ans url has been E_mailed to you');", true);
                }
                else
                {
                    string Message = "Login ID does not Exist";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Message + "');", true);
                }
            }
            catch (Exception ex)
            {
                string Message = ex.Message.ToString();
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + Message + "');", true);
            }
           // Response.Redirect("~/LoginMaster.aspx");
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

        public class StringCollection : System.Collections.Specialized.StringCollection
        {

            public string[] ToArray()
            {
                string[] result = new string[Count];
                CopyTo(result, 0);
                return result;
            }
        }

        static string strMailServer = ConfigurationManager.AppSettings.Get("MailServer").ToString();
        static string strSenderMailID = ConfigurationManager.AppSettings.Get("SenderMailID").ToString();
        static string strHeader = ConfigurationSettings.AppSettings["HeaderKey"].ToString();
        static string strFooter = ConfigurationSettings.AppSettings["FooterKey"].ToString();
        static string strMailBody = ConfigurationSettings.AppSettings["MailToNextLevelApproversBody"].ToString();
        static string strMailSub = ConfigurationSettings.AppSettings["MailToNextLevelApproversSub"].ToString();
        static string strresult = String.Empty;

        public String SendMail(string userName, string strToEmailAdd, string strBody, string strSubject, string strattachFile, bool allowHTML, string strSenderMailAdd)
        {
            //   if (strCode == "CYLSYS12@!")
            {
                try
                {
                   // string strMailSub = ConfigurationManager.AppSettings["ISUTI"].ToString();
                    //DecisionLayer dsl = new DecisionLayer();
                    //DataLayer dl = new DataLayer();
                    StringBuilder sbTable = new StringBuilder();
                    sbTable.Append("<HTML>");
                    sbTable.Append("<BODY>");
                    sbTable.Append("</br>");
                    sbTable.Append("</br>");
                    sbTable.Append("</p>");
                    //sbTable.Append("<h4>Your Password has been reset and your new password is :" + dsl.DecryptConnectionString(StrPassword) + "</h4>");
                    sbTable.Append("Your Password has been reset and your new password is :" + StrPassword + "");
                    sbTable.Append("</br>");
                    sbTable.Append("Please click on below link to login and change password");
                    sbTable.Append("</br>");
                    sbTable.Append("http://10.11.32.88/BGS/LoginMaster.aspx");
                    sbTable.Append("</br>");
                    sbTable.Append("Please contact system administrator for any clarification.");
                    sbTable.Append("</br>");
                    sbTable.Append("This is auto generated mail and please do not reply to this email");
                    sbTable.Append("</br>");
                    sbTable.Append("</br>");
                    sbTable.Append("</br>");
                    sbTable.Append("</BODY>");
                    sbTable.Append("</HTML>");

                    strHeader = "Dear " + userName;
                    string strToMailAdd = string.Empty;
                    strMailSub = string.Format("Reset password:");
                    strMailBody = string.Format(strMailBody, DateTime.Today.ToString("dd-MMM-yyyy"), sbTable.ToString());
                    strBody = strHeader + sbTable + strFooter;
                    SmtpClient emailClient = new SmtpClient(strMailServer);
                    MailMessage message = new MailMessage();
                    message.IsBodyHtml = allowHTML;
                    MailAddress maSender = new MailAddress(strSenderMailAdd);
                    message.Sender = maSender;
                    message.From = maSender;
                    message.Subject = strMailSub;
                    message.Body = strBody;

                    message.To.Add(strToEmailAdd);
                    emailClient.UseDefaultCredentials = true;
                    emailClient.Send(message);
                    strresult = "Mail send to " + strToEmailAdd;

                }
                catch (SmtpException ex)
                {
                    strresult = ex + "Error Sending Mail";
                }
            }

            return strresult;
        }


        public String sendEmail(string userName, string strToEmailAdd, string strBody, string strSubject, string strattachFile, bool allowHTML)
        {
            String strresult = String.Empty;
            try
            {
                strresult = SendMail(userName, strToEmailAdd, strBody, strSubject, strattachFile, allowHTML, strSenderMailID);
            }
            catch (Exception ex)
            {
                //  EventLogProcess(ex);
            }
            return strresult;
        }
    }
}
