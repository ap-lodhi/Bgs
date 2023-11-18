using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data;

namespace BGS
{
    public partial class usctrl_UserDetails : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                lblWelcome.Text = string.Format("Welcome : {0}", string.IsNullOrEmpty(Session["USERNAME"].ToString()) ? "User" : Session["USERNAME"].ToString());
                lblUserType.Text = string.Format("User Type : {0}", string.IsNullOrEmpty(Session["UserTypeName"].ToString()) ? "User" : Session["UserTypeName"].ToString());
                lblDate.Text = string.Format("Date : {0}", DateTime.Now.ToShortDateString());
            }
        }

        protected void Logout(object sender, EventArgs e)
        {
            Response.Redirect(LoginMaster_Page.GetUrl());
        }
    }
}