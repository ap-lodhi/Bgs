using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.SqlClient;

namespace BGS
{
    public partial class BrokerAdjustedScore : PageBase
    {
        #region Properties
        Datalayer dl = new Datalayer();
        DataTable dt = new DataTable();
        BGSEntities bgs_entity = new BGSEntities();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int quarterid = this.GetQuarterName();
                BindBrokerOrFirmName();
                // string quartername = ddlQuarter.SelectedItem.Text!=null ? ddlQuarter.SelectedItem.Text.ToString() : null;
                //BindDepartmentName();
                this.BindReport(quarterid, null, null);
            }

        }
        private int GetQuarterName()
        {
            int quarterid = 0;
            try
            {
                //var Query = (from C in bgs_entity.BGS_GetQuarterNameDuteDate()
                //             select C).FirstOrDefault();
                //if (Query != null)
                //{
                //    string quarter = Query.StartDate.ToString() + " - " + Query.EndDate.ToString();
                //    ddlQuarter.Items.FindByText(quarter).Selected = true;
                //}


                ddlQuarter.DataSource = bgs_entity.BGS_BindQuarter();
                ddlQuarter.DataTextField = "QuarterDate";
                ddlQuarter.DataValueField = "QuarterID";
                ddlQuarter.DataBind();
                ddlQuarter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select", "0"));
                var query = bgs_entity.BGS_BindQuarter().LastOrDefault();
                if (query != null)
                {
                    for (int i = 0; i < ddlQuarter.Items.Count; i++)
                    {
                        if (query.QuarterDate.ToString() == ddlQuarter.Items[i].Text.ToString())
                        {
                            ddlQuarter.Items.FindByText(query.QuarterDate).Selected = true;
                            quarterid = Convert.ToInt32(query.QuarterID);
                        }

                    }

                }
                // ddlQuarter.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            return quarterid;
        }

        private void BindBrokerOrFirmName()
        {
            try
            {
                ddlBrokerName.Items.Clear();
                ddlBrokerName.DataSource = from n in bgs_entity.BGS_BindBrokerName() select new { n.BrokerName, n.BrokerNameID };
                ddlBrokerName.DataTextField = "BrokerName";
                ddlBrokerName.DataValueField = "BrokerNameID";
                ddlBrokerName.DataBind();
                ddlBrokerName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select", "0"));
                // ddlBrokerName.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        //private void BindDepartmentName()
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = dl.GetDepartmentName();
        //        ddlDepartmentName.DataSource = dt;
        //        ddlDepartmentName.DataTextField = "DepartmentID";
        //        ddlDepartmentName.DataTextField = "DepartmentName";
        //        ddlDepartmentName.DataBind();
        //        ddlDepartmentName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select", "0"));

        //    }
        //    catch (Exception ex)
        //    {
        //        BGS.Error.EventLogProcess(ex);
        //    }
        //}

        private void BindReport(int QuarterValue, string BrokerName,string DepartmentName)
        {
            DataTable dt = new DataTable();
            dt = dl.GetBrokerAdjustedReports(QuarterValue, BrokerName, DepartmentName);
            gvBGSReport.DataSource = dt;
           // dt.Columns["DepartmentName"].v
            gvBGSReport.DataBind();
           
        }

        //public DataTable BindGridView(dt)
        //{
        //    DataTable dt1 = new DataTable();
        //    dt1.Columns.Add(new System.Data.DataColumn("Quarter Name", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Broker Name", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Weighted Score of DFM & DOSR", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Broker Adjusted Score of DFM & DOSR", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Weighted Score of DOD", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Broker Adjusted Score of DOD", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Weighted Score of DOFA", System.Type.GetType("System.String")));
        //    dt1.Columns.Add(new System.Data.DataColumn("Quarter Name", System.Type.GetType("System.String")));

        //}

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
            string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
            string departmentName = null;
           // string departmentName = ddlDepartmentName.SelectedIndex > 0 ? ddlDepartmentName.SelectedItem.Text : null;
            this.BindReport(quarterID, BrokerName,departmentName);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlBrokerName.ClearSelection();
            ddlQuarter.ClearSelection();
           // ddlDepartmentName.ClearSelection();           
            this.BindReport(0, null, null);
        }

        protected void btnExcell_Click(object sender, EventArgs e)
        {
            string Quarter = string.Empty;
            string QuarterName = string.Empty;
            Quarter = ddlQuarter.SelectedItem.Value;
            QuarterName = ddlQuarter.SelectedItem.Text;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + QuarterName + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvBGSReport.AllowPaging = false;
                int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
                string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
                // string DepartmentName = ddlDepartmentName.SelectedIndex > 0 ? ddlDepartmentName.SelectedItem.Text : null;
                string DepartmentName = null;
                this.BindReport(quarterID, BrokerName, DepartmentName);
                gvBGSReport.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gvBGSReport.HeaderRow.Cells)
                {
                    cell.BackColor = gvBGSReport.HeaderStyle.BackColor;
                    // gvBGSReport.HeaderRow.Cells[0].Visible = false;
                }
                foreach (GridViewRow row in gvBGSReport.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvBGSReport.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvBGSReport.RowStyle.BackColor;
                        }
                        // row.Cells[0].Visible = false;
                        cell.CssClass = "textmode";
                    }
                }

                gvBGSReport.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }

        }

        protected void btnPdf_Click(object sender, EventArgs e)
        {
            string Quarter = string.Empty;
            Quarter = ddlQuarter.SelectedItem.Value;
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    //To Export all pages
                    gvBGSReport.AllowPaging = false;
                    int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
                    string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
                    // string DepartmentName = ddlDepartmentName.SelectedIndex > 0 ? ddlDepartmentName.SelectedItem.Text : null;                    
                    string DepartmentName = null;
                    this.BindReport(quarterID, BrokerName,DepartmentName);
                    gvBGSReport.Columns[0].Visible = false;
                    gvBGSReport.HeaderRow.BackColor = System.Drawing.Color.LightCoral;
                    gvBGSReport.Caption = "Your caption";
                    gvBGSReport.HeaderRow.Style.Add("width", "25%");
                    gvBGSReport.HeaderRow.Style.Add("font-size", "10px");
                    gvBGSReport.Style.Add("text-decoration", "none");
                    gvBGSReport.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    gvBGSReport.Style.Add("font-size", "8px");
                    gvBGSReport.Style.Add("align", "Left");
                    tdlogo.Visible = true;
                    tdlogo.RenderControl(hw);
                    gvBGSReport.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + Quarter + ".pdf");
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.Write(pdfDoc);
                    Response.End();
                }
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

    }
}