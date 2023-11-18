using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BGS.UserControls
{
    public partial class Login : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            //string flag = UserLogin.Get_UserLogin(txtuser.Text, txtpwd.Text);
            //if (flag != "0")
            //{
            //    Session["UserName"] = txtuser.Text.Trim();
            //    Session["Role"] = flag;
            //    if (flag.Trim().ToUpper() == "TRIAGE")
            //        Response.Redirect("CaseStartup.aspx");
            //    else if (flag.Trim().ToUpper() == "TL")
            //        Response.Redirect("CaseAsign.aspx");
            //    else if (flag.Trim().ToUpper() == "PM")
            //        Response.Redirect("ProjectAllocation.aspx");
            //    else
            //        Response.Redirect("AdminHomePage.aspx");
            //}
            //else
            //{
            //    lblErrMsg.Text = "Login Failed";
            //}
        }
    }
}