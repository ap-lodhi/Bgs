using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
namespace BGS
{
    public partial class QuarterMaster : System.Web.UI.Page
    {
        #region Properties
        //SRRMS_PRODEntities srrms_entity = new SRRMS_PRODEntities();
        BGSEntities bgs_entity = new BGSEntities();
        
        /// <summary>
        /// Gets or sets QuarterId
        /// </summary>
        public int QuarterId
        {
            get
            {
                return ViewState["QuarterId"] != null ? (int)ViewState["QuarterId"] : 0;
            }
            set
            {
                ViewState["QuarterId"] = value;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.GetYear();
                this.PopulateQuarter();
            }
        }
        protected void DeleteQuarter(object sender, EventArgs e)
        {
            try
            {
                string Quarterid = (sender as Button).CommandArgument;
                this.bgs_entity.BGS_DeleteQuarterMaster(Convert.ToInt32(Quarterid));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        protected void EditQuarter(object sender, EventArgs e)
        {
            try
            {
                ddlYear.ClearSelection();
                int index = Convert.ToInt32((sender as Button).CommandArgument);
                this.QuarterId = Convert.ToInt32((sender as Button).CommandName);
                txtQuarterName.Text = gvQuarterMaster.Rows[index].Cells[0].Text;
                ddlYear.Items.FindByText(gvQuarterMaster.Rows[index].Cells[1].Text).Selected = true;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            
        }
        protected void CancelQuarter(object sender, EventArgs e)
        {
            try
            {
                ddlYear.ClearSelection();
                ddlYear.Items[0].Selected = true;
                txtQuarterName.Text = string.Empty;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
           
        }
        protected void SaveQuarter(object sender, EventArgs e)
        {
            try
            {
                this.InsertQuarterMaster();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void QuarterPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvQuarterMaster.PageIndex = e.NewPageIndex;
                PopulateQuarter();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
           
        }
        #region Private Methods

        /// <summary>
        /// Inserts Quarter Master
        /// </summary>
        private void InsertQuarterMaster()
        {
            try
            {
            int strYear = (ddlYear.SelectedItem.Text.Length > 0) ? int.Parse(ddlYear.SelectedItem.Text) : 0;
            string strName = (txtQuarterName.Text.Length > 0) ? (txtQuarterName.Text) : (null);
            bgs_entity.BGS_QuarterMaster("INSERT", this.QuarterId, strName,strYear,null,null);   
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            this.Response.Redirect(this.Request.Url.AbsoluteUri);
        }
        /// <summary>
        /// Populates Quarter Master
        /// </summary>
        private void PopulateQuarter()
        {
            try
            {
                gvQuarterMaster.DataSource = bgs_entity.BGS_GetQuarterMaster();
                gvQuarterMaster.DataBind();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// <summary>
        /// Get Quarter  Year 
        /// </summary>
        private void GetYear()
        {
            try
            {
                ddlYear.Items.Clear();
                ListItem[] itens = new ListItem[5] {
                new ListItem(DateTime.Today.AddYears(-2).Year.ToString()),
                new ListItem(DateTime.Today.AddYears(-1).Year.ToString()),
                new ListItem(DateTime.Today.Year.ToString()),
                new ListItem(DateTime.Today.AddYears(+1).Year.ToString()),
                new ListItem(DateTime.Today.AddYears(+2).Year.ToString())
                };
                ddlYear.DataSource = itens;
                ddlYear.DataBind();
                ddlYear.Items.Insert(0, (new ListItem { Text = "Please select", Value = string.Empty }));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        #endregion
    }
}