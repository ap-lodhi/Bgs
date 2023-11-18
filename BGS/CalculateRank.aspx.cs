using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BGS
{
    public partial class CalculateRank : PageBase
    {
        #region Properties
        DataSet ds = new DataSet();
        BGSEntities bgs_entity = new BGSEntities();
        Datalayer dl = new Datalayer();
        public string QuarterId
        {
            get { return !string.IsNullOrEmpty(Convert.ToString(Request.QueryString["QuarterId"])) ? Request.QueryString["QuarterId"].ToString() : "0"; }
            set { Request.QueryString["QuarterId"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvCalculateRank.DataSource = bgs_entity.BGS_CalculateRank(Convert.ToInt32(QuarterId));
                gvCalculateRank.DataBind();
                GetCuurentQuarterStatus(Convert.ToInt32(QuarterId));
            }
        }
        protected void SubmitRank(object sender, EventArgs e)
        {
            var q = bgs_entity.BGS_CheckRankInsert(Convert.ToInt32(QuarterId));
            int count =Convert.ToInt32(q.FirstOrDefault().Value);
            if (count == 0)
            {

                foreach (GridViewRow gr in gvCalculateRank.Rows)
                {
                    int BrokerNameID = Convert.ToInt32(((HiddenField)gvCalculateRank.Rows[gr.RowIndex].FindControl("hfBrokerNameID")).Value);
                    int QuarterID = Convert.ToInt32(((HiddenField)gvCalculateRank.Rows[gr.RowIndex].FindControl("hfQuarterID")).Value);

                    int Rank = Convert.ToInt32(((Label)gvCalculateRank.Rows[gr.RowIndex].FindControl("lblRank")).Text);
                    double TotalScore = Convert.ToDouble(gvCalculateRank.Rows[gr.RowIndex].Cells[2].Text);
                    dl.InsertRank(BrokerNameID, QuarterID, 0, 0, Rank, this.USERNAME, TotalScore);
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('All data for this quarter Saved succesfully.');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Already Submitted for this quarter: ');", true);
            }
        }
        protected void ExportToExcel(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=CalculateRank.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvCalculateRank.AllowPaging = false;
                gvCalculateRank.DataSource = bgs_entity.BGS_CalculateRank(Convert.ToInt32(QuarterId));
                gvCalculateRank.DataBind();
                gvCalculateRank.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in gvCalculateRank.HeaderRow.Cells)
                {
                    cell.BackColor = gvCalculateRank.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvCalculateRank.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvCalculateRank.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvCalculateRank.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }
               
                gvCalculateRank.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void Back(object sender, EventArgs e)
        {
            this.Response.Redirect("~/GradingMaster.aspx");
        }
        #region Methods
        private void GetCuurentQuarterStatus( int quarterID)
        {
            ds = dl.GetCuurentQuarterVoterStatus(quarterID);
            int cnt=0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows[i]["StatusName"].ToString() == EnumStatus.Closed.ToString() && Convert.ToDecimal(ds.Tables[0].Rows[i]["Vote"].ToString()) == Convert.ToInt32(EnumTotalVotes.TotalVotes))
                    {
                        cnt++;
                    }
                }
                if (cnt == ds.Tables[0].Rows.Count)
                {
                    btnSubmit.Enabled = true;
                }
                else
                {
                    btnSubmit.Enabled =false;
                }
            }
        }
        #endregion
    }
}