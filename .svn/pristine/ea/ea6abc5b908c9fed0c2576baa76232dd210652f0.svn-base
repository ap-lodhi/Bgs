using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

namespace BGS
{
    public partial class BrokerRanking : System.Web.UI.Page
    {
        BGSEntities bgs_entity = new BGSEntities();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvBrokerEmailList.DataSource = bgs_entity.BGS_GetBrokerRanking();
                gvBrokerEmailList.DataBind();
            }
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=RankReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvBrokerEmailList.AllowPaging = false;
                gvBrokerEmailList.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gvBrokerEmailList.HeaderRow.Cells)
                {
                    cell.BackColor = gvBrokerEmailList.HeaderStyle.BackColor;
                    // gvBrokerEmailList.HeaderRow.Cells[0].Visible = false;
                }
                foreach (GridViewRow row in gvBrokerEmailList.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvBrokerEmailList.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvBrokerEmailList.RowStyle.BackColor;
                        }
                        // row.Cells[0].Visible = false;
                       // cell.CssClass = "textmode";
                       
                    }
                }

                gvBrokerEmailList.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }


        }

        protected void btnpdf_Click(object sender, EventArgs e)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    //To Export all pages
                    gvBrokerEmailList.AllowPaging = false;
                  //  gvBrokerEmailList.Columns[0].Visible = false;
                    gvBrokerEmailList.HeaderRow.BackColor = System.Drawing.Color.LightCoral;
                    //gvBrokerEmailList.Caption = "Your caption";
                    gvBrokerEmailList.HeaderRow.Style.Add("width", "15%");
                    gvBrokerEmailList.HeaderRow.Style.Add("font-size", "10px");
                    gvBrokerEmailList.Style.Add("text-decoration", "none");
                    gvBrokerEmailList.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
                    gvBrokerEmailList.Style.Add("font-size", "8px");
                    gvBrokerEmailList.RenderControl(hw);
                    StringReader sr = new StringReader(sw.ToString());
                    Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                    pdfDoc.Open();
                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=RankReport.pdf");
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