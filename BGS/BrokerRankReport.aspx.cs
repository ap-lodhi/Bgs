using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

namespace BGS
{
    public partial class BrokerRankReport : PageBase
    {
        #region Properties
        BGSEntities bgs_entity = new BGSEntities();
        Datalayer dl = new Datalayer();
        DataSet ds = new DataSet();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetQuarterName();
                BindBrokerOrFirmName();

            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                if (ddlBrokerName.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select Broker/Firm name');", true);
                    return;
                }

                if (ddlQuarter.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select quarter');", true);
                    return;
                }
                DataSet dsrank = new DataSet();
                dsrank = dl.GetPerBrokerRank(ddlQuarter.SelectedIndex > 0 ? Convert.ToInt32(ddlQuarter.SelectedItem.Value) : 0, ddlBrokerName.SelectedIndex > 0 ? Convert.ToInt32(ddlBrokerName.SelectedItem.Value) : 0);
                if (dsrank.Tables[1].Rows.Count > 0)
                {
                    if (dsrank.Tables[0].Rows.Count > 1)
                    {
                        if (Convert.ToInt32(dsrank.Tables[0].Rows[0][0].ToString()) > Convert.ToInt32(dsrank.Tables[0].Rows[1][0].ToString()))
                        {
                            lblReveiew1.Text = dsrank.Tables[0].Rows[0]["PeriodReview"].ToString();
                            lblReveiew2.Text = dsrank.Tables[0].Rows.Count > 1 ? dsrank.Tables[0].Rows[1]["PeriodReview"].ToString() : null;
                        }
                        else
                        {
                            lblReveiew1.Text = dsrank.Tables[0].Rows[1]["PeriodReview"].ToString();
                            lblReveiew2.Text = dsrank.Tables[0].Rows.Count > 1 ? dsrank.Tables[0].Rows[0]["PeriodReview"].ToString() : null;
                        }

                    }
                    else
                    {
                        lblReveiew1.Text = dsrank.Tables[0].Rows[0]["PeriodReview"].ToString();
                        lblReveiew2.Text = null;

                    }
                    if (dsrank.Tables[1].Rows.Count > 1)
                    {
                        if (Convert.ToInt32(dsrank.Tables[1].Rows[0][0].ToString()) > Convert.ToInt32(dsrank.Tables[1].Rows[1][0].ToString()))
                        {
                            lblRank1.Text = (dsrank.Tables[1].Rows.Count > 0) ? String.Format("{0} Group", dsrank.Tables[1].Rows[0]["RankGroup"].ToString()) : null;
                            lblRank2.Text = (dsrank.Tables[1].Rows.Count > 1) ? String.Format("{0} Group", dsrank.Tables[1].Rows[1]["RankGroup"].ToString()) : null;//dsrank.Tables[1].Rows[0]["RankGroup"].ToString();
                        }
                        else
                        {
                            lblRank1.Text = (dsrank.Tables[1].Rows.Count > 0) ? String.Format("{0} Group", dsrank.Tables[1].Rows[1]["RankGroup"].ToString()) : null;
                            lblRank2.Text = (dsrank.Tables[1].Rows.Count > 1) ? String.Format("{0} Group", dsrank.Tables[1].Rows[0]["RankGroup"].ToString()) : null;
                        }
                    }
                    else
                    {
                        lblRank1.Text = String.Format("{0} Group", dsrank.Tables[1].Rows[0]["RankGroup"].ToString());
                        lblRank2.Text = null;

                    }
                    // lblFirmName.Text = ddlBrokerName.SelectedItem.Text;
                    lblMov2.Text = ddlBrokerName.SelectedItem.Text;
                    if (dsrank.Tables[2].Rows[0]["RankDifference"].ToString() == "0")
                    {
                        lblMov1.Text = "No Change";
                    }
                    else if (dsrank.Tables[2].Rows[0]["RankDifference"].ToString().Contains('-'))
                    {
                        if (dsrank.Tables[2].Rows[0]["RankDifference"].ToString() == "-1")
                        {
                            lblMov1.Text = "Up" + dsrank.Tables[2].Rows[0]["RankDifference"].ToString().Replace('-', ' ') + " Rank";
                        }
                        else
                        {
                            lblMov1.Text = "Up " + dsrank.Tables[2].Rows[0]["RankDifference"].ToString().Replace('-', ' ') + " Ranks";
                        }
                    }
                    else if (string.IsNullOrEmpty(dsrank.Tables[2].Rows[0]["RankDifference"].ToString()))
                    {
                        lblMov1.Text = "---";
                    }
                    else if (Convert.ToInt32(dsrank.Tables[2].Rows[0]["RankDifference"].ToString()) == 1)
                    {
                        lblMov1.Text = "Down " + dsrank.Tables[2].Rows[0]["RankDifference"].ToString() + " Rank";
                    }
                    else
                    {
                        lblMov1.Text = "Down " + dsrank.Tables[2].Rows[0]["RankDifference"].ToString() + " Ranks";
                    }
                    // lblMov1.Text = dsrank.Tables[2].Rows[0]["RankDifference"].ToString() != "0" ? dsrank.Tables[2].Rows[0]["RankDifference"].ToString() : "No Change";

                    //if (lblRank1.Text.Trim() == lblRank2.Text.Trim())
                    //{
                    //    lblMov1.Text = "No Change";
                    //}
                    //else
                    //{
                    //    if (dsrank.Tables[1].Rows.Count > 1)
                    //    {
                    //        if (Convert.ToInt32(dsrank.Tables[1].Rows[0][0].ToString()) > Convert.ToInt32(dsrank.Tables[1].Rows[1][0].ToString()))
                    //        {
                    //            lblMov1.Text = Convert.ToString(Convert.ToInt32(dsrank.Tables[1].Rows[1]["Rank"].ToString()) - Convert.ToInt32(dsrank.Tables[1].Rows[0]["Rank"].ToString()));
                    //        }
                    //        else
                    //        {
                    //            lblMov1.Text = Convert.ToString(Convert.ToInt32(dsrank.Tables[1].Rows[0]["Rank"].ToString()) - Convert.ToInt32(dsrank.Tables[1].Rows[1]["Rank"].ToString()));

                    //        }
                    //    }
                    //    else
                    //    {
                    //        lblMov1.Text = (dsrank.Tables[1].Rows.Count > 0) ? dsrank.Tables[1].Rows[0]["Rank"].ToString() : null; //dsrank.Tables[1].Rows[1]["Rank"].ToString();
                    //    }
                    //}
                    Pdfpnl.Visible = true;
                    DataSet DsCurrent = new DataSet();
                    DataSet DsLast = new DataSet();
                    string CurrentQuarter = string.Empty;
                    string LastQuarter = string.Empty;
                    DsCurrent = dl.GetBrokerWiseRanKCurrentQuarter(Convert.ToInt32(ddlQuarter.SelectedItem.Value), ddlBrokerName.SelectedItem.Text);
                    if (DsCurrent.Tables.Count > 0)
                    {
                        if (DsCurrent.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < DsCurrent.Tables[0].Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    DsCurrent.Tables[0].Rows[i]["QuarterName"] = string.Empty;
                                }
                            }
                        }
                    }
                    if (DsCurrent.Tables.Count > 0)
                    {
                        if (DsCurrent.Tables[0].Rows.Count > 0)
                        {
                            // int ShareSumCurrent = Convert.ToInt32(Math.Round(Convert.ToDecimal(DsCurrent.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString())));
                            string ShareSumCurrent = DsCurrent.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString();
                            CurrentQuarter = DsCurrent.Tables[0].Rows[0]["QuarterName"].ToString();

                            DataRow newRow = DsCurrent.Tables[0].NewRow();
                            newRow["QuarterName"] = CurrentQuarter;
                            newRow["TotalBrokerScore"] = ShareSumCurrent;
                            DsCurrent.Tables[0].Rows.Add(newRow);
                        }
                    }

                    DsLast = dl.GetBrokerWiseRanKLastQuarter(Convert.ToInt32(ddlQuarter.SelectedItem.Value), ddlBrokerName.SelectedItem.Text);
                    if (DsLast.Tables.Count > 0)
                    {
                        if (DsLast.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < DsLast.Tables[0].Rows.Count; i++)
                            {
                                if (i > 0)
                                {
                                    DsLast.Tables[0].Rows[i]["QuarterName"] = string.Empty;
                                }

                            }
                        }
                    }
                    if (DsLast.Tables.Count > 0)
                    {
                        if (DsLast.Tables[0].Rows.Count > 0)
                        {

                            //int ShareSumLast = Convert.ToInt32(Math.Round(Convert.ToDecimal(DsLast.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString())));
                            string ShareSumLast = DsLast.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString();
                            LastQuarter = DsLast.Tables[0].Rows[0]["QuarterName"].ToString();
                            DataRow newRow1 = DsLast.Tables[0].NewRow();
                            newRow1["QuarterName"] = LastQuarter;
                            newRow1["TotalBrokerScore"] = ShareSumLast;
                            DsLast.Tables[0].Rows.Add(newRow1);
                        }
                    }
                    DsCurrent.Merge(DsLast);
                    gvBrokerRankReport.DataSource = DsCurrent;
                    ViewState["dt"] = DsCurrent;
                    gvBrokerRankReport.DataBind();
                    if (string.IsNullOrEmpty(LastQuarter))
                    {
                        lblMov.Text = string.Format("Movement of Rank Over {0}", "---");
                    }
                    else
                    {
                        lblMov.Text = string.Format("Movement of Rank Over {0}", GetMovementRankString(LastQuarter));
                    }

                }
                else
                {
                    Pdfpnl.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('No record found.');", true);
                }
            }

            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);

            }
            // GetBrokerRank(Convert.ToInt32(ddlQuarter.SelectedItem.Value), ddlBrokerName.SelectedItem.Text);
        }
        protected void btnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlBrokerName.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select Broker/Firm name');", true);
                    return;
                }

                if (ddlQuarter.SelectedIndex <= 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select quarter');", true);
                    return;
                }

                string Quarter = string.Empty;
                string firm = string.Empty;
                string filename = string.Empty;
                Quarter = ddlQuarter.SelectedItem.Text.Substring(3, ddlQuarter.SelectedItem.Text.Count() - 3);
                // string[] Splitfrim = ddlBrokerName.SelectedItem.Text.Split(' ');
                // firm = Splitfrim[0].ToString();
                //  firm = ddlBrokerName.SelectedItem.Text;
                filename = Quarter + "_" + ddlBrokerName.SelectedItem.Text + ".pdf";
                iTextSharp.text.Table table2 = new iTextSharp.text.Table(1);
                int[] widths2 = new int[1];
                Paragraph paragraph = new Paragraph();
                Chunk big = new Chunk("Note(1)");
                big.Font.SetStyle("bold");
                paragraph.Add(big);
                Chunk para1 = new Chunk(": 'Market Share' represents market share for your analyst for that sector only. Please note that our analyst's vote is not necessarily equally weighted across the different sectors covered by them. Thus for example, if Cement and FMCG are covered by the same analyst, she may choose to assign her votes in a 30:70 ratio, indicating her priorities for that vote-period. Other votes are given by Fund Managers. The last column indicates the number of analysts voted for on the sell-side in a sector.");
                paragraph.Add(para1);
                Paragraph paragraph1 = new Paragraph();
                Chunk big1 = new Chunk("Note(2)");
                big1.Font.SetStyle("bold");
                paragraph1.Add(big1);
                Chunk para2 = new Chunk(": Attached Tabular Information represents votes polled of Equity Funds & Equity Research, however Final Votes, Rankings & Brokerage are determined by the above as well as votes given by the Dealing Team.");
                paragraph1.Add(para2);
                Paragraph paragraph2 = new Paragraph();
                Chunk big2 = new Chunk("Note(3)");
                big2.Font.SetStyle("bold");
                paragraph2.Add(big2);
                Chunk para3 = new Chunk(": As always for all meetings, you are requested to send a calendar invite to all attendees so that we have a record of all the meetings. Also please attach all reports and presentations to be discussed, in the calendar invite. For any new/ unlisted Company management meeting, Please attach detailed financials (Latest Quarterly results, Balance Sheet & Cash Flow statements) and Company presentation, if available, to enable preparation/ constructive discussion.");
                paragraph2.Add(para3);

                ////   ///////////////Added by prakash as discussed with Ankita on 08/03/2016///////////////////////////////////

                ////   ////int[] widthsc = new int[1];
                ////   Paragraph paragraphConf1 = new Paragraph();
                ////   Chunk paraConf1 = new Chunk("Confidential");

                ////   paragraphConf1.Add(paraConf1);
                ////   paragraphConf1.IndentationLeft = Element.ALIGN_LEFT;
                ////   paragraphConf1.SpacingAfter = 10;
                ////  // paragraphConf1.Alignment = Element.ALIGN_LEFT;
                //////   paragraphConf1.Alignment = Element.ALIGN_BASELINE;


                ////   Paragraph paragraphConf2 = new Paragraph();
                ////   Chunk paraConf2 = new Chunk("VOTE BY FUNDS AND RESEARCH");
                ////   paragraphConf2.Add(paraConf2);
                ////   paragraphConf2.Alignment = Element.ALIGN_RIGHT;

                ////   var tableConfidential = new PdfPTable(1);
                ////   var cellConfidential = new PdfPCell { };
                ////   //cellConfidential.Colspan = 2;
                ////   cellConfidential.AddElement(paragraphConf1);
                ////   cellConfidential.AddElement(paragraphConf2);

                ////   tableConfidential.AddCell(cellConfidential);

                ////   ////////////////Added by prakash as discussed with Ankita on 08/03/2016///////////////////////////////////////

                var table4 = new PdfPTable(1);
                var cellp = new PdfPCell { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                var table5 = new PdfPTable(1);
                var cellp1 = new PdfPCell { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                var table6 = new PdfPTable(1);
                var cellp2 = new PdfPCell { PaddingLeft = 5, PaddingTop = 5, PaddingBottom = 5, PaddingRight = 5 };
                cellp.AddElement(paragraph);
                cellp1.AddElement(paragraph1);
                cellp2.AddElement(paragraph2);
                table4.AddCell(cellp);
                table5.AddCell(cellp1);
                table6.AddCell(cellp2);
                PdfPTable tableImage = new PdfPTable(1);
                string imageURL = Server.MapPath(".") + "/Images/uti_mutual_fund.jpg";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.ScalePercent(50);
                jpg.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                PdfPCell imageCell = new PdfPCell(jpg);
                imageCell.PaddingBottom = 5;
                imageCell.PaddingLeft = 10;
                imageCell.PaddingTop = 5;
                tableImage.AddCell(imageCell);
                //iTextSharp.text.Table table1 = new iTextSharp.text.Table(3);
                iTextSharp.text.Font fontTable = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fontTable1 = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                PdfPTable table1 = new PdfPTable(3);
                table1.DefaultCell.PaddingTop = 2;
                table1.DefaultCell.PaddingBottom = 2;
                table1.DefaultCell.Phrase = new Phrase() { Font = fontTable };
                table1.DefaultCell.Phrase = new Phrase() { Font = fontTable1 };


                // table1.SpacingBefore = 45f;
                // table1.SpacingAfter = 45f;              
                // table1.TotalWidth = 216f;
                // table1.Cellspacing = 1;
                // table1.Cellpadding = 2;
                int[] widths1 = new int[3];
                for (int x = 0; x < 3; x++)
                {
                    widths1[x] = 20;
                    if (x == 0)
                    {


                        table1.AddCell(new PdfPCell(new Phrase(lblReveiew.Text)));
                        table1.AddCell(new PdfPCell(new Phrase(lblReveiew1.Text, fontTable)));
                        table1.AddCell(new PdfPCell(new Phrase(lblReveiew2.Text, fontTable)));
                        //string cellReveiew = lblReveiew.Text;
                        //string cellReveiew1 = lblReveiew1.Text;
                        //string cellReveiew2 = lblReveiew2.Text;
                        //iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell(cellReveiew);
                        //iTextSharp.text.Cell cell2 = new iTextSharp.text.Cell(cellReveiew1);
                        //iTextSharp.text.Cell cell3 = new iTextSharp.text.Cell(cellReveiew2);
                        //table1.AddCell(cellReveiew);
                        //table1.AddCell(cellReveiew1);
                        //table1.AddCell(cellReveiew2);
                    }
                    if (x == 1)
                    {

                        table1.AddCell(new PdfPCell(new Phrase(lblRank.Text)));
                        table1.AddCell(new PdfPCell(new Phrase(lblRank1.Text, fontTable)));
                        table1.AddCell(new PdfPCell(new Phrase(lblRank2.Text, fontTable)));
                        //string cellReveiew = lblRank.Text;
                        //string cellReveiew1 = lblRank1.Text;
                        //string cellReveiew2 = lblRank2.Text;
                        //iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell(cellReveiew);
                        //iTextSharp.text.Cell cell2 = new iTextSharp.text.Cell(cellReveiew1);
                        //iTextSharp.text.Cell cell3 = new iTextSharp.text.Cell(cellReveiew2);
                        //table1.AddCell(cellReveiew);
                        //table1.AddCell(cellReveiew1);
                        //table1.AddCell(cellReveiew2);
                    }
                    if (x == 2)
                    {
                        table1.AddCell(new PdfPCell(new Phrase(lblMov.Text)));
                        table1.AddCell(new PdfPCell(new Phrase(lblMov1.Text, fontTable)));
                        table1.AddCell(new PdfPCell(new Phrase(lblMov2.Text, fontTable1)));
                        //string cellReveiew = lblMov.Text;
                        //string cellReveiew1 = lblMov1.Text;
                        //string cellReveiew2 = lblMov2.Text;
                        //iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell(cellReveiew);
                        //iTextSharp.text.Cell cell2 = new iTextSharp.text.Cell(cellReveiew1);
                        //iTextSharp.text.Cell cell3 = new iTextSharp.text.Cell(cellReveiew2);
                        //table1.AddCell(cellReveiew);
                        //table1.AddCell(cellReveiew1);
                        //table1.AddCell(cellReveiew2);
                    }
                }

                iTextSharp.text.Table table = new iTextSharp.text.Table(gvBrokerRankReport.Columns.Count);
                iTextSharp.text.Table tableH = new iTextSharp.text.Table(30);
                table.Cellspacing = 1;
                table.Cellpadding = 1;
                tableH.Cellspacing = 1;
                tableH.Cellpadding = 1;
                int[] widths = new int[gvBrokerRankReport.Columns.Count];
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
                iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);
                //BaseFont bf1 = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
                //iTextSharp.text.Font font1 = new iTextSharp.text.Font(bf1, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);
                iTextSharp.text.Cell cellH = new iTextSharp.text.Cell(new Phrase(14, "", font));
                iTextSharp.text.Cell cellH1 = new iTextSharp.text.Cell(new Phrase(14, "Share Of", font));
                iTextSharp.text.Cell cellH2 = new iTextSharp.text.Cell(new Phrase(14, "Research Sector", font));
                cellH.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#425290"));
                cellH1.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#425290"));
                cellH2.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#425290"));
                cellH.Colspan = 18;
                cellH1.Colspan = 6;
                cellH1.HorizontalAlignment = Element.ALIGN_CENTER;
                cellH1.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellH2.Colspan = 6;
                cellH2.HorizontalAlignment = Element.ALIGN_CENTER;
                cellH2.VerticalAlignment = Element.ALIGN_MIDDLE;
                tableH.AddCell(cellH);
                tableH.AddCell(cellH1);
                tableH.AddCell(cellH2);

                for (int x = 0; x < gvBrokerRankReport.Columns.Count; x++)
                {

                    widths[x] = (int)gvBrokerRankReport.Columns[x].ItemStyle.Width.Value;
                    //BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, BaseFont.EMBEDDED);
                    //iTextSharp.text.Font font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE);
                    string cellText = Server.HtmlDecode(gvBrokerRankReport.HeaderRow.Cells[x].Text);
                    iTextSharp.text.Cell cell = new iTextSharp.text.Cell(new Phrase(14, cellText, font));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#425290"));
                    table.AddCell(cell);

                }

                table.SetWidths(widths);
                int ctn = 0;
                ctn = gvBrokerRankReport.Rows.Count;
                for (int i = 0; i < gvBrokerRankReport.Rows.Count; i++)
                {
                    string cellText = string.Empty;
                    if (gvBrokerRankReport.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        for (int j = 0; j < gvBrokerRankReport.Columns.Count; j++)
                        {
                            cellText = Server.HtmlDecode(gvBrokerRankReport.Rows[i].Cells[j].Text);
                            iTextSharp.text.Cell cell = new iTextSharp.text.Cell(cellText);
                            // cell.HorizontalAlignment = Element.ALIGN_CENTER;

                            if (j == 1 || j == 2)
                            {
                                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            else if (j == 0)
                            {
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            }
                            else
                            {
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            if (string.IsNullOrEmpty((gvBrokerRankReport.Rows[i].Cells[3].Text).Replace("&nbsp;", "")))
                            {

                                cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#fa8258"));
                            }
                            if (ctn == i + 1)
                            {
                                cell.BackgroundColor = new iTextSharp.text.Color(System.Drawing.ColorTranslator.FromHtml("#bdbdbd"));
                            }
                            table.AddCell(cell);
                        }
                    }

                }
                tbl.Attributes.Add("align", "Center");
                tdlogo.Visible = true;
                tdlogo.Visible = false;
                //Create the PDF Document
                Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
                pdfDoc.SetPageSize(iTextSharp.text.PageSize.A2.Rotate());
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                pdfDoc.Open();

                ////////////////////////////
                //pdfDoc.Add(tableConfidential);
                ///////////////////////////
                pdfDoc.Add(tableImage);



                pdfDoc.Add(table1);
                pdfDoc.Add(table4);
                pdfDoc.Add(table5);
                pdfDoc.Add(table6);
                pdfDoc.Add(tableH);
                pdfDoc.Add(table);
                pdfDoc.Close();
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;" + "filename=" + string.Format("{0}.pdf", Quarter));
                // Response.AddHeader("content-disposition", "attachment;filename="+ filename);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(pdfDoc);
                Response.End();
                //using (StringWriter sw = new StringWriter())
                //{
                //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                //    {
                //        string Quarter = string.Empty;
                //        string firm = string.Empty;
                //        Quarter = ddlQuarter.SelectedItem.Text.Substring(3, ddlQuarter.SelectedItem.Text.Count() - 3);
                //        firm = ddlBrokerName.SelectedItem.Text;
                //        //tbl.Attributes.Add("style", "font-size:8px !important");
                //        // string [] Splitfrim = ddlBrokerName.SelectedItem.Text.Split(' ');
                //        // frim = Splitfrim[0].ToString();
                //        // gvBrokerRankReport.Caption = "Your caption";
                //        //  gvBrokerRankReport.HeaderRow.Cells[0].Style.Add("width", "700");
                //        // gvBrokerRankReport.Columns[1].HeaderStyle.Width = 800;
                //        gvBrokerRankReport.Rows[1].Cells[1].Style.Add("width", "100%");
                //        // gvBrokerRankReport.HeaderRow.Cells[1].Width = 700;
                //        // gvBrokerRankReport.HeaderRow.Style.Add("width", "5%");
                //        gvBrokerRankReport.HeaderRow.Style.Add("font-size", "10px");
                //        // gvBrokerRankReport.HeaderRow.Style.Add("background-color", "wheat");
                //        gvBrokerRankReport.HeaderRow.Style.Add("background-color", "#425290");
                //        // gvBrokerRankReport.Columns[1].ItemStyle.Width = 700;
                //        gvBrokerRankReport.Style.Add("text-decoration", "none");
                //        gvBrokerRankReport.Style.Add("font-family", "Arial, Helvetica, sans-serif");
                //        gvBrokerRankReport.Style.Add("font-size", "10px !important");
                //        gvBrokerRankReport.Style.Add("text-align", "center");
                //        tbl.Attributes.Add("align", "Center");
                //        tdlogo.Visible = true;
                //        tbl.RenderControl(hw);
                //        tdlogo.Visible = false;
                //        gvBrokerRankReport.RenderControl(hw);
                //        StringReader sr = new StringReader(sw.ToString());
                //        Document pdfDoc = new Document(PageSize.A2, 100f, 100f, 10f, 0f);
                //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                //        pdfDoc.Open();
                //        htmlparser.Parse(sr);
                //        pdfDoc.Close();

                //        Response.ContentType = "application/pdf";
                //        Response.AddHeader("content-disposition", "attachment;filename=" + Quarter + "_" + firm + ".pdf");
                //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //        Response.Write(pdfDoc);
                //        Response.End();
                //    }
                //}
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }
        protected void gvBrokerRankReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Label lbltxt = (Label)e.Row.FindControl("lblVoterShare");
                if (string.IsNullOrEmpty(e.Row.Cells[4].Text.Replace("&nbsp;", ""))) //change by anshu
                {
                    //e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff6700");
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#fa8258");
                    e.Row.Cells[0].Font.Bold.ToString();
                }
                if (!string.IsNullOrEmpty(e.Row.Cells[3].Text.Replace("&nbsp;", "")))
                {
                    if (Convert.ToInt32(e.Row.Cells[6].Text) == 0)
                    {
                        e.Row.Cells[6].Text = "";
                    }
                }
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlQuarter.ClearSelection();
            ddlBrokerName.ClearSelection();
        }
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            if (ddlQuarter.SelectedIndex > 0)
            {
                this.Response.Redirect("BrokerEmailListing.aspx?QuarterId=" + ddlQuarter.SelectedItem.Value);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alert", "alert('Please select quarter.');", true);
            }
        }
        //protected void ddlQuarter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    GetBrokerRank(Convert.ToInt32(ddlQuarter.SelectedItem.Value));
        //}
        #region Method
        private void GetQuarterName()
        {
            ddlQuarter.DataSource = bgs_entity.BGS_BindQuarter().ToList();
            ddlQuarter.DataTextField = "QuarterDate";
            ddlQuarter.DataValueField = "QuarterID";
            ddlQuarter.DataBind();
            ddlQuarter.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Please Select", "0"));
            ddlQuarter.Items.FindByValue(ddlQuarter.Items[ddlQuarter.Items.Count - 1].Value).Selected = true;
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
        private void GetBrokerRank(int quarter, string BrokerName)
        {
            //DataSet ds1 = new DataSet();
            //ds = dl.GetBrokerWiseRanKCurrentQuarter(quarter, BrokerName);
            //gvBrokerRankReport.DataSource = ds;
            //gvBrokerRankReport.DataBind();

        }
        //private string GetMovementRankString(string quarter)
        //{
        //    string[] split = quarter.Split('_');
        //    string final = string.Empty;
        //    switch (split[2])
        //    {
        //        case "Q1":
        //            final = "Jan-Mar";
        //            break;
        //        case "Q2":
        //            final = "Apr-Jun";
        //            break;
        //        case "Q3":
        //            final = "Jul-Sep";
        //            break;
        //        case "Q4":
        //            final = "Oct-Dec";
        //            break;
        //    }
        //    return string.Format(final + " {0}", split[1].Substring(2, 2));
        //}

        private string GetMovementRankString(string quarter)
        {
            string[] split = quarter.Split('_');
            string final = string.Empty;
            int length = split.Length;
            if (length == 2)
            {
                string split2 = split[2];
            }
            else
            {
                final = split[0].ToString();
            }

            try
            {
                if (length == 2)
                {
                    switch (split[2])
                    {
                        case "Q1":
                            final = "Jan-Mar";
                            break;
                        case "Q2":
                            final = "Apr-Jun";
                            break;
                        case "Q3":
                            final = "Jul-Sep";
                            break;
                        case "Q4":
                            final = "Oct-Dec";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
            if (length == 2)
            {
                return string.Format(final + " {0}", split[1].Substring(2, 2));
            }
            else
            {
                return string.Format(final);
            }
        }

        #endregion
        protected void gvBrokerRankReport_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {

                GridViewRow HeaderRow = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell2 = new TableCell();
                HeaderCell2.Text = "";
                HeaderCell2.ColumnSpan = 4;
                HeaderRow.Cells.Add(HeaderCell2);
                HeaderCell2.Style.Add("color", "white");
                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "Share Of";
                HeaderCell2.ColumnSpan = 2;
                HeaderRow.Cells.Add(HeaderCell2);
                HeaderCell2.Style.Add("color", "white");
                HeaderCell2 = new TableCell();
                HeaderCell2.Text = "Research Sector";
                HeaderCell2.ColumnSpan = 2;
                HeaderRow.Cells.Add(HeaderCell2);
                HeaderCell2.Style.Add("color", "white");
                gvBrokerRankReport.Controls[0].Controls.AddAt(0, HeaderRow);
                GridViewRow HeaderRow1 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "Year";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Analyst Name";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Voter Comment";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Sector Name";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Voters Vote";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Vote to Broker";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "Market Share";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderCell.Style.Add("color", "white");
                HeaderCell = new TableCell();
                HeaderCell.Text = "No. of Analysts";
                HeaderRow1.Cells.Add(HeaderCell);
                HeaderRow1.Style.Add("background-color", "#425290");
                HeaderRow.Style.Add("background-color", "#425290");
                HeaderCell.Style.Add("color", "white");
                // HeaderRow.Style.Add("background-color", "#425290");
                //HeaderRow.Style.Add("text-color", "White");
                // HeaderRow.Style.Add("text-color", "White");
                gvBrokerRankReport.Controls[0].Controls.AddAt(1, HeaderRow1);
            }

        }

    }
}