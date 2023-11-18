using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace BGS
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        Datalayer dl = new Datalayer();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void UserChangePassword(object sender, EventArgs e)
        {
            try
            {
                string Message = string.Empty;
                Message = dl.ChagePassword(txtEmpId.Text.ToString().Trim(), txtOldPwd.Text.ToString().Trim(), txtNewPwd.Text.ToString().Trim());
                ShowAlert(Message);
                txtEmpId.Text = string.Empty;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void Back(object sender, EventArgs e)
        {
            //Response.Redirect("~/Home.aspx");
            Response.Redirect("~/BrokerMaster.aspx");
        }
            
        #region Methods
        public static void ShowAlert(Page currentPage, string message)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert('");
            sb.Append(message);
            sb.Append("');");
            currentPage.ClientScript.RegisterStartupScript(typeof(ChangePassword), "showalert", sb.ToString(), true);
        }
        public static void ShowAlert(string message)
        {
            Page currentPage = HttpContext.Current.Handler as Page;
            if (currentPage != null)
                ShowAlert(currentPage, message);
        }
        #endregion 
    }
}