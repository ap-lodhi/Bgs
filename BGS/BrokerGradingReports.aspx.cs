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
    public partial class BrokerGradingReports :PageBase
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
                int quarterid=this.GetQuarterName();
                GetVoterName();
                PopulateSectorName();
                BindBrokerOrFirmName();
               // string quartername = ddlQuarter.SelectedItem.Text!=null ? ddlQuarter.SelectedItem.Text.ToString() : null;
                this.BindReport(quarterid, null, null,null);
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
            string VoterName = ddlvoterName.SelectedIndex > 0 ? ddlvoterName.SelectedItem.Text : null;
            string SectorName = ddlSector.SelectedIndex > 0 ? ddlSector.SelectedItem.Text : null;
            string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
            this.BindReport(quarterID, VoterName, SectorName, BrokerName);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlBrokerName.ClearSelection();
            ddlQuarter.ClearSelection();
            ddlSector.ClearSelection();
            ddlvoterName.ClearSelection();
            this.BindReport(0, null, null, null);
        }
        protected void btnExcell_Click(object sender, EventArgs e)
        {
            string Quarter = string.Empty;
            Quarter = ddlQuarter.SelectedItem.Text.Substring(0, ddlQuarter.SelectedItem.Text.Count());
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + Quarter + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvBGSReport.AllowPaging = false;
                int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
                string VoterName = ddlvoterName.SelectedIndex > 0 ? ddlvoterName.SelectedItem.Text : null;
                string SectorName = ddlSector.SelectedIndex > 0 ? ddlSector.SelectedItem.Text : null;
                string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
                this.BindReport(quarterID, VoterName, SectorName, BrokerName);
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
            Quarter = ddlQuarter.SelectedItem.Text.Substring(0, ddlQuarter.SelectedItem.Text.Count());
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    //To Export all pages
                    gvBGSReport.AllowPaging = false;
                    int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
                    string VoterName = ddlvoterName.SelectedIndex > 0 ? ddlvoterName.SelectedItem.Text : null;
                    string SectorName = ddlSector.SelectedIndex > 0 ? ddlSector.SelectedItem.Text : null;
                    string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
                    this.BindReport(quarterID, VoterName, SectorName, BrokerName);
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

        #region Method
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
                        if(query.QuarterDate.ToString()==ddlQuarter.Items[i].Text.ToString())
                        {
                            ddlQuarter.Items.FindByText(query.QuarterDate).Selected = true;
                            quarterid=Convert.ToInt32(query.QuarterID);
                        }
                        
                    }
                   
                }
                // ddlQuarter.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
               
            }
            catch(Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            return quarterid;
        }

        private void BindReport(int QuarterValue,string voterName,string SectorName,string BrokerName)
        {
            DataTable dt = new DataTable();
            dt = dl.GetQuarterReports(QuarterValue, voterName, SectorName, BrokerName);
            gvBGSReport.DataSource = dt;

            gvBGSReport.DataBind();
        }
        private DataTable AdjustedWeightReport(int QuarterValue, string voterName, string SectorName, string BrokerName)
        {
            DataTable dt = new DataTable();
            dt = dl.GetAdjustedWeightReports(QuarterValue, voterName, SectorName, BrokerName);
            return dt;
        }

        /// <summary>
        ///get Voter Name
        /// </summary>
        private void GetVoterName()
        {
            try
            {
                ddlvoterName.DataSource = bgs_entity.BGS_BindVoter();
                ddlvoterName.DataTextField = "UserName";
                ddlvoterName.DataValueField = "EmpID";
                ddlvoterName.DataBind();
                ddlvoterName.Items.Insert(0,new System.Web.UI.WebControls.ListItem("Please Select", "0"));
               // ddlvoterName.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
                // btnRowAdd.Visible = (EnumUserType.Admin == this.UserTypeName ? false : true);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        /// <summary>
        /// Populates Sector Name
        /// </summary>
        private void PopulateSectorName()
        {
            try
            {
                ddlSector.Items.Clear();
                ddlSector.DataSource = from n in bgs_entity.BGS_GetSector() select new { n.SectorID, n.SectorName };
                ddlSector.DataTextField = "SectorName";
                ddlSector.DataValueField = "SectorId";
                ddlSector.DataBind();
                ddlSector.Items.Insert(0,new System.Web.UI.WebControls.ListItem("Please Select", "0"));
               // ddlSector.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        /// <summary>
        /// Populates Sector Name
        /// </summary>
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
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        #endregion

        protected void btnExcel1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string Quarter = string.Empty;
            Quarter = ddlQuarter.SelectedItem.Text.Substring(0, ddlQuarter.SelectedItem.Text.Count());
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + Quarter + ".xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvBGSReport.AllowPaging = false;
                int quarterID = !string.IsNullOrEmpty(ddlQuarter.SelectedItem.Value) ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0;
                string VoterName = ddlvoterName.SelectedIndex > 0 ? ddlvoterName.SelectedItem.Text : null;
                string SectorName = ddlSector.SelectedIndex > 0 ? ddlSector.SelectedItem.Text : null;
                string BrokerName = ddlBrokerName.SelectedIndex > 0 ? ddlBrokerName.SelectedItem.Text : null;
                dt= this.AdjustedWeightReport(quarterID, VoterName, SectorName, BrokerName);
             //   dt.Columns
                gvBGSReport.HeaderRow.BackColor = System.Drawing.Color.White;

                gvBGSReport.Rows[1].Cells[0].Text = "Department of DOSR and DOFM";
                gvBGSReport.Rows[1].Cells[0].Style.Add("background-color", "yellow");

                gvBGSReport.Rows[2].Cells[0].Text = "Voter";
                gvBGSReport.Rows[2].Cells[1].Text = "Weights";
                gvBGSReport.Rows[2].Cells[2].Text = "Adjusted Weights for 7000";

                gvBGSReport.Rows[2].Cells[4].Text = "Voter";
                gvBGSReport.Rows[2].Cells[5].Text = "Weights";
                gvBGSReport.Rows[2].Cells[6].Text = "Adjusted Weights for 2500";

                gvBGSReport.Rows[2].Cells[8].Text = "Voter";
                gvBGSReport.Rows[2].Cells[9].Text = "Weights";
                gvBGSReport.Rows[2].Cells[10].Text = "Single login with 500 weights";

                gvBGSReport.Rows[2].Cells[0].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[1].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[2].Style.Add("background-color", "yellow");

                gvBGSReport.Rows[2].Cells[4].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[5].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[6].Style.Add("background-color", "yellow");

                gvBGSReport.Rows[2].Cells[8].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[9].Style.Add("background-color", "yellow");
                gvBGSReport.Rows[2].Cells[10].Style.Add("background-color", "yellow");

                //      gvBGSReport.HeaderRow.Style.Add("background-color", "#366092");
                ////      gvBGSReport.HeaderRow.Cells[1].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Style.Add("background-color", "#ffff00");

                //      gvBGSReport.HeaderRow.Cells[0].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[1].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[2].Style.Add("background-color", "yellow");

                //      gvBGSReport.HeaderRow.Cells[4].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[5].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[6].Style.Add("background-color", "yellow");

                //      gvBGSReport.HeaderRow.Cells[8].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[9].Style.Add("background-color", "yellow");
                //      gvBGSReport.HeaderRow.Cells[10].Style.Add("background-color", "yellow");

                for (int i = 3; i < gvBGSReport.Rows.Count+3; i++)
                {
                    GridViewRow row = gvBGSReport.Rows[i];
                    //row.BackColor = System.Drawing.Color.White;
                    row.Attributes.Add("class", "textmode");
                    for(int j=0;j<=10;j++)
                    {
                        if(j==2||j==6||j==10)
                        {row.Cells[j].Style.Add("background-color", "yellow");}
                        else
                        { row.Cells[j].Style.Add("background-color", "white"); }

                        if (i == gvBGSReport.Rows.Count + 2)
                        {
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[0].Text = "Total";
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[0].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[1].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[2].Style.Add("background-color", "yellow");

                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[0].Text = "";
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[4].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[5].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[6].Style.Add("background-color", "yellow");

                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[0].Text = "";
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[8].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[9].Style.Add("background-color", "yellow");
                            gvBGSReport.Rows[gvBGSReport.Rows.Count + 3].Cells[10].Style.Add("background-color", "yellow");
                        }
                        
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



    }
}