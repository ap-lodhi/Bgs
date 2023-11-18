using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BGS
{
    public partial class ScreenAllocation : System.Web.UI.UserControl
    {
        #region Properties
        Datalayer objDL = new Datalayer();

        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        public string EmployeeId
        {
            get
            {
                return Request.QueryString["empid"] != null ? Request.QueryString["empid"] : string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name
        {
            get
            {
                return Request.QueryString["Name"] != null ? Request.QueryString["Name"] : string.Empty;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUserName.Text = this.Name;
                this.PoulateScreenModule();
            }
        }

        /// <summary>
        /// Poulates Screen Module
        /// </summary>
        private void PoulateScreenModule()
        {
            try
            {
                if (!string.IsNullOrEmpty(EmployeeId))
                {
                    DataTable dtView = new DataTable();
                    dtView = objDL.GetAssignScreen(EmployeeId);
                    grdScreens.DataSource = dtView;
                    grdScreens.DataBind();
                }
            }
            catch
            {
                this.Response.Redirect(this.Request.Url.AbsoluteUri);
            }

        }

        protected void BackButton(object sender, EventArgs e)
        {
            this.Response.Redirect("~/UserMaster.aspx");
        }

        protected void AssignScreenToUser(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdScreens.Rows)
            {
                string screenId = (row.FindControl("hfScreenId") as HiddenField).Value;
                string moduleId = (row.FindControl("hfModuleId") as HiddenField).Value;
                string screenAssigned = (row.FindControl("hfScreenAssigned") as HiddenField).Value;
                string strEmpID = "1";//Session["EmpID"]
                if ((row.FindControl("chkselect") as CheckBox).Checked)
                {
                    objDL.AssignScreenToUser(!string.IsNullOrEmpty(EmployeeId) ? EmployeeId : "0", !string.IsNullOrEmpty(screenId) ? int.Parse(screenId) : 0, !string.IsNullOrEmpty(moduleId) ? int.Parse(moduleId) : 0, !string.IsNullOrEmpty(screenAssigned) ? int.Parse(screenAssigned) : 0, true, strEmpID);
                }
                else
                {
                    objDL.AssignScreenToUser(!string.IsNullOrEmpty(EmployeeId) ? EmployeeId : "0", !string.IsNullOrEmpty(screenId) ? int.Parse(screenId) : 0, !string.IsNullOrEmpty(moduleId) ? int.Parse(moduleId) : 0, !string.IsNullOrEmpty(screenAssigned) ? int.Parse(screenAssigned) : 0, false, strEmpID);
                }
            }
            objDL.AssignScreenLinking(!string.IsNullOrEmpty(EmployeeId) ? EmployeeId : "0");
            //this.Response.Redirect(this.Request.Url.AbsoluteUri);
            this.Response.Redirect("~/UserMaster.aspx");
        }
    }
}