﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using System.Threading;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace BGS
{
    public partial class BrokerEmailListing : PageBase
    {
        BGSEntities bgs_entity = new BGSEntities();
        Datalayer dl = new Datalayer();
        string StrPassword = string.Empty;
        public int QuarterId
        {
            get
            {
                return Request.QueryString["QuarterId"] != null ? Convert.ToInt32(Request.QueryString["QuarterId"]) : 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmailList();
                bindexcellist();
            }
        }
        protected void gvBrokerRankReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (string.IsNullOrEmpty(e.Row.Cells[3].Text.Replace("&nbsp;", "")))
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ff6700");
                    e.Row.Cells[0].Font.Bold.ToString();
                }

                if (!string.IsNullOrEmpty(e.Row.Cells[3].Text.Replace("&nbsp;", "")))
                {
                    if (Convert.ToInt32(e.Row.Cells[6].Text) == 0)
                    {
                        e.Row.Cells[6].Text = "";
                    }
                }
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    Label lbltxt = (Label)e.Row.FindControl("lblVoterShare");
                //    if (lbltxt.Text == "")
                //    {
                //        e.Row.BackColor = System.Drawing.Color.LightCoral;
                //        e.Row.Cells[0].Font.Bold.ToString();
                //    }
                //}
            }
        }
        protected void SendEmail_Click(object sender, EventArgs e)
        {
            try
            {
                Session["Email"] = null;
                string EmailNotSent = string.Empty;
                foreach (GridViewRow row in gvBrokerEmailList.Rows)
                {
                    if ((row.FindControl("chkEmail") as CheckBox).Checked)
                    {
                        string email = (row.FindControl("txtEmail") as TextBox).Text;
                        string userName = (row.FindControl("hfUserName") as HiddenField).Value;
                        string brokerName = (row.FindControl("txtBrokerName") as TextBox).Text;
                        int brokerNameId = !String.IsNullOrEmpty((row.FindControl("hfBrokerNameId") as HiddenField).Value) ? Convert.ToInt32((row.FindControl("hfBrokerNameId") as HiddenField).Value) : 0;
                        //  ViewState["BrokerId"] = brokerNameId;
                        dl.InsertBrokerConfigforBrokerEmail(brokerNameId, QuarterId, true);
                        GetDataforAttached(brokerNameId, brokerName);
                        //System.Threading.Thread threadSendMails;
                        //threadSendMails = new System.Threading.Thread(delegate() { SendMail(email, true, userName, brokerName); }); threadSendMails.IsBackground = true;
                        //threadSendMails.Start();
                        SendMail(email, true, userName, brokerName);
                        bgs_entity.BGS_UpdateEmailStatus(true, brokerNameId);
                    }
                }
                if (Session["Email"] != null)
                {
                    EmailNotSent = Session["Email"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Email not sent  " + EmailNotSent + " successfully.');document.location.reload(true);", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Email sent successfully.');document.location.reload(true);", true);
                }

            }
            catch (SmtpException ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        public void SendMail(string strToEmailAdd, bool allowHTML, string name, string brokerName)
        {
            try
            {
                string MailNotSent = string.Empty;
                string strGetLastDayMonth = string.Empty;
                string strGetMovementRankString = string.Empty;
                string strGetPreviousRank = string.Empty;
                string str = string.Empty;
                string strresult = String.Empty;
                string quarterName = (from c in bgs_entity.BGS_GetQuarterNameById(QuarterId) select c).FirstOrDefault().Name;
                string quarterName2 = (from c in bgs_entity.BGS_GetQuarterName2ById(QuarterId) select c).FirstOrDefault().Name;
                // string quarterName2 = from c in  bgs_entity.BGS_GetQuarterNameById(QuarterId) select c)
                strGetLastDayMonth = GetLastDayMonth(quarterName);
                strGetMovementRankString = GetMovementRankString(quarterName, true);
                strGetPreviousRank = GetPreviousRank(quarterName);
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
                //Paragraph paragraph = new Paragraph("Note(1):'Market Share' represents market share for your analyst for that sector only. Please note that our analyst's vote is not necessarily equally weighted across the different sectors covered by them. Thus for example, if Cement and FMCG are covered by the same analyst, she may choose to assign her votes in a 30:70 ratio, indicating her priorities for that vote-period. Other votes are given by Fund Managers. The last column indicates the number of analysts voted for on the sell-side in a sector.");
                //Paragraph paragraph1 = new Paragraph("Note(2): Attached Tabular Information represents votes polled of Equity Funds & Equity Research, however Final Votes, Rankings & Brokerage are determined by the above as well as votes given by the Dealing Team.");
                //Paragraph paragraph2 = new Paragraph("Note(3):  As always for all meetings, you are requested to send a calendar invite to all attendees so that we have a record of all the meetings. Also please attach all reports and presentations to be discussed, in the calendar invite. Any new/ unlisted Company management meeting, pl attach detailed financials (Latest Quarterly results, Balance Sheet & cash flow statements) and Company presentation, if available, to enable preparation/ constructive discussion.");
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
                iTextSharp.text.Font fontTable = FontFactory.GetFont("Times New Roman", 12, iTextSharp.text.Font.BOLD);
                PdfPTable table1 = new PdfPTable(3);
                table1.DefaultCell.Phrase = new Phrase() { Font = fontTable };
                //iTextSharp.text.Table table1 = new iTextSharp.text.Table(3);
                //table1.Cellspacing = 1;
                //table1.Cellpadding = 2;
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
                        table1.AddCell(new PdfPCell(new Phrase(lblMov2.Text, fontTable)));
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
                            //cell.HorizontalAlignment = Element.ALIGN_CENTER;
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
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter.GetInstance(pdfDoc, memoryStream);
                pdfDoc.Open();
                pdfDoc.Add(tableImage);
                pdfDoc.Add(table1);
                pdfDoc.Add(table4);
                pdfDoc.Add(table5);
                pdfDoc.Add(table6);
                pdfDoc.Add(tableH);
                pdfDoc.Add(table);
                pdfDoc.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                string strMailServer = ConfigurationManager.AppSettings.Get("MailServer").ToString();
                string strSenderMailID = ConfigurationManager.AppSettings.Get("SenderMailID").ToString();
                string strHeader = ConfigurationManager.AppSettings["HeaderKey"].ToString();
                string strFooter = ConfigurationManager.AppSettings["FooterKey"].ToString();
                string strMailBody = ConfigurationManager.AppSettings["MailToNextLevelApproversBody"].ToString();
                string port = ConfigurationManager.AppSettings["port"].ToString();
                string AppUrl = ConfigurationManager.AppSettings["AppUrl"].ToString();
                string ISUTI = ConfigurationManager.AppSettings["ISUTI"].ToString();
                string strToMailAdd = string.Empty;
                string strInstructionPdf = string.Empty;

                strInstructionPdf = Server.MapPath("~") + "\\AttachPDFWithMail\\Howtoreadvotes.pdf";


                SmtpClient emailClient = new SmtpClient(strMailServer);
                MailMessage message = new MailMessage();
                message.IsBodyHtml = allowHTML;
                MailAddress maSender = new MailAddress(strSenderMailID, "UTI Mutual Fund");
                message.Sender = maSender;
                message.From = maSender;


                MailAddress addressBCC = new MailAddress("sharwan.goyal@uti.co.in");
                MailAddress addressBca = new MailAddress("lokesh.kulthia@uti.co.in");
                //MailAddress addressCC = new MailAddress("lokesh.kulthia@uti.co.in");

                message.Bcc.Add(addressBca);
                message.Bcc.Add(addressBCC);
                message.Subject = string.Format("UTI Mutual Fund - Vote by Equity Funds & Equity Research ({0})", (quarterName2));//GetPreviousRank by anshu
                //string lastQuarterName = (from c in bgs_entity.BGS_GetLastQuarterNameById(QuarterId) select c).FirstOrDefault().Name;
                CommonFunctions cf = new CommonFunctions();
                strHeader = cf.PopulateBody("BrokerEmailSend");
                strHeader = strHeader.Replace("{#LastQuarterMonth}", strGetLastDayMonth);
                strHeader = strHeader.Replace("{#Last1QuarterMonthYearName}", strGetMovementRankString);
                strHeader = strHeader.Replace("{#CurrentQuarterMonthYearName}", strGetPreviousRank);
                message.Body = strHeader;
                message.To.Add(strToEmailAdd);
                message.Attachments.Add(new Attachment(new MemoryStream(bytes), string.Format("{0}_{1}.pdf", quarterName, brokerName)));

                message.Attachments.Add(new Attachment(strInstructionPdf));
                emailClient.UseDefaultCredentials = true;
                emailClient.Send(message);
                strresult = "Mail send to " + strToEmailAdd;
            }
            catch (SmtpException ex)
            {
                Session["Email"] += brokerName + "::" + strToEmailAdd + ",";
                BGS.Error.EventLogProcess(ex);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        { }
        private string GetMovementRankString(string quarter, bool isyear)
        {
            string[] split = quarter.Split('_');
            string final = string.Empty;
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
            if (isyear)
            {
                return string.Format(final + " {0}", split[1].Substring(2, 2));
            }
            else
            {
                return final;
            }
        }
        private string GetPreviousRank(string quarter)
        {
            string[] split = quarter.Split('_');
            int year = Convert.ToInt32(split[1].Substring(2, 2));
            string final = string.Empty;
            switch (split[2])
            {
                case "Q1":
                    final = "Apr-Jun";
                    break;
                case "Q2":
                    final = "Jul-Sep";
                    break;
                case "Q3":
                    final = "Oct-Dec";
                    break;
                case "Q4":
                    final = "Jan-Mar";
                    year = year + 1;
                    break;
            }
            return string.Format(final + " {0}", year);
        }
        private string GetLastDayMonth(string quarterName)
        {
            if (!string.IsNullOrEmpty(quarterName))
            {

                string quarterMonth = GetMovementRankString(quarterName, false);
                int year = Convert.ToInt32(quarterName.Substring(3, 4));
                int lastDay = 0;
                string monthName = string.Empty;
                switch (quarterMonth)
                {
                    case "Jan-Mar":
                        lastDay = DateTime.DaysInMonth(year, 03);
                        monthName = "March";
                        break;
                    case "Apr-Jun":
                        lastDay = DateTime.DaysInMonth(year, 06);
                        monthName = "June";
                        break;
                    case "Jul-Sep":
                        lastDay = DateTime.DaysInMonth(year, 09);
                        monthName = "September";
                        break;
                    case "Oct-Dec":
                        lastDay = DateTime.DaysInMonth(year, 12);
                        monthName = "December";
                        break;
                }
                return string.Format("{0} {1}", lastDay, monthName);
            }
            else
            {
                return string.Empty;
            }
        }
        private void GetDataforAttached(int BrokerNameId, string brokerName)
        {
            Datalayer dl = new Datalayer();
            DataSet dsrank = new DataSet();
            dsrank = dl.GetPerBrokerRank(QuarterId, BrokerNameId);
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
                // lblFirmName.Text = brokerName;
                lblMov2.Text = brokerName;
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

               
                Pdfpnl.Visible = true;
                DataSet DsCurrent = new DataSet();
                DataSet DsLast = new DataSet();
                string CurrentQuarter = string.Empty;
                string LastQuarter = string.Empty;
                DsCurrent = dl.GetBrokerWiseRanKCurrentQuarter(QuarterId, brokerName);
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
                        int ShareSumCurrent = Convert.ToInt32(Math.Round(Convert.ToDecimal(DsCurrent.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString())));
                        //string ShareSumCurrent = DsCurrent.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString();
                        CurrentQuarter = DsCurrent.Tables[0].Rows[0]["QuarterName"].ToString();

                        DataRow newRow = DsCurrent.Tables[0].NewRow();
                        newRow["QuarterName"] = CurrentQuarter;
                        newRow["TotalBrokerScore"] = ShareSumCurrent;
                        DsCurrent.Tables[0].Rows.Add(newRow);
                    }
                }
                DsLast = dl.GetBrokerWiseRanKLastQuarter(QuarterId, brokerName);
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
                        int ShareSumLast = Convert.ToInt32(Math.Round(Convert.ToDecimal(DsLast.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString())));
                        // string ShareSumLast = DsLast.Tables[0].Compute("Sum(TotalBrokerScore)", "").ToString();
                        LastQuarter = DsLast.Tables[0].Rows[0]["QuarterName"].ToString();
                        DataRow newRow1 = DsLast.Tables[0].NewRow();
                        newRow1["QuarterName"] = LastQuarter;
                        newRow1["TotalBrokerScore"] = ShareSumLast;
                        DsLast.Tables[0].Rows.Add(newRow1);
                    }
                }
                DsCurrent.Merge(DsLast);
                gvBrokerRankReport.DataSource = DsCurrent;
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
        }
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
       private void BindEmailList()
        {
            gvBrokerEmailList.DataSource = bgs_entity.BGS_GetEmailListing(QuarterId);
            gvBrokerEmailList.DataBind();
            
        }

        private void bindexcellist()
        {
            gridviewexcel.DataSource = bgs_entity.ExcelListing(QuarterId);
            gridviewexcel.DataBind();
        }
        private void SubmitSaveForLater()
        {
            try
            {
                string UserEmailID = string.Empty;
                string BrokerName = string.Empty;
                string UserName = string.Empty;
                string UserID = string.Empty;
                int BrokerNameId = 0;
                string fmComments = string.Empty;
                string RMComment = string.Empty; //anshu 12/may/23
                DataTable dtadd = new DataTable();
                dtadd.Columns.Add(new DataColumn("BrokerNameID", typeof(int)));
                dtadd.Columns.Add(new DataColumn("BrokerName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("EmailID", typeof(string)));
                dtadd.Columns.Add(new DataColumn("UserName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                dtadd.Columns.Add(new DataColumn("UpdatedBy", typeof(string)));
                foreach (GridViewRow row in gvBrokerEmailList.Rows)
                {
                    BrokerNameId = Convert.ToInt32((row.FindControl("hfBrokerNameId") as HiddenField).Value == "" ? ("0") : (row.FindControl("hfBrokerNameId") as HiddenField).Value);
                    UserName = (row.FindControl("hfUserName") as HiddenField).Value == "" ? ("") : (row.FindControl("hfUserName") as HiddenField).Value;
                    UserID = (row.FindControl("hfUserID") as HiddenField).Value == "" ? ("") : (row.FindControl("hfUserID") as HiddenField).Value;
                    BrokerName = ((TextBox)row.FindControl("txtBrokerName")).Text;
                    UserEmailID = ((TextBox)row.FindControl("txtEmail")).Text;

                    //string s1=UserEmailID.before
                    string[] s = UserEmailID.Split('@');
                    string[] s3 = s[0].Split('.');
                    dtadd.Rows.Add(UserID, BrokerNameId, BrokerName, UserEmailID, UserName, Session["EmpID"].ToString(), null);
                    //Brokerfirm
                }
                ViewState["CurrentTable"] = dtadd;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void SaveUserEmailID(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                //dt.Columns.Remove("BrokerName");
                dl.InsertupdateUserEmmailId(dt);
            }
        }
        public static string CreateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }

            return new string(chars);
        }
        protected void btnSaveUserEmail_Click(object sender, EventArgs e)
        {
            try
            {
                string UserEmailID = string.Empty;
                string BrokerName = string.Empty;
                string UserName = string.Empty;
                string UserID = string.Empty;
                string UpdatedUserName = string.Empty;
                int BrokerNameId = 0;
                string fmComments = string.Empty;
                string[] s3, s;
                DataTable dtadd = new DataTable();
                dtadd.Columns.Add(new DataColumn("BrokerNameID", typeof(int)));
                dtadd.Columns.Add(new DataColumn("BrokerName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("UserName", typeof(string)));
                dtadd.Columns.Add(new DataColumn("EmailID", typeof(string)));
                dtadd.Columns.Add(new DataColumn("CreatedBy", typeof(string)));
                dtadd.Columns.Add(new DataColumn("UpdatedBy", typeof(string)));
                dtadd.Columns.Add(new DataColumn("Password", typeof(string)));
                foreach (GridViewRow row in gvBrokerEmailList.Rows)
                {
                    BrokerNameId = Convert.ToInt32((row.FindControl("hfBrokerNameId") as HiddenField).Value == "" ? ("0") : (row.FindControl("hfBrokerNameId") as HiddenField).Value);
                    UserName = (row.FindControl("hfUserName") as HiddenField).Value == "" ? (" ") : (row.FindControl("hfUserName") as HiddenField).Value;
                    BrokerName = ((TextBox)row.FindControl("txtBrokerName")).Text;
                    UserEmailID = ((TextBox)row.FindControl("txtEmail")).Text;

                    //if ((row.FindControl("hfUserName") as HiddenField).Value == "")
                    //{
                    if ((row.FindControl("hfUserName") as HiddenField).Value == "")
                    {
                        s = UserEmailID.Split('@');
                        if (s[0].Contains("."))
                        {
                            s3 = s[0].Split('.');
                            var FirstName = Regex.Replace(s3[0], @"[\d-]", string.Empty);
                            var LastName = Regex.Replace(s3[1], @"[\d-]", string.Empty);
                            UserName = FirstName + " " + LastName;
                            StrPassword = CreateRandomPassword(10);
                        }
                        else
                        {
                            var FirstName = Regex.Replace(s[0], @"[\d-]", string.Empty);
                            UserName = FirstName;
                        }

                        // dtadd.Rows.Add(BrokerNameId, BrokerName, UserName, UserEmailID, Session["EmpID"].ToString(), Session["EmpID"].ToString());
                    }
                    dtadd.Rows.Add(BrokerNameId, BrokerName, UserName, UserEmailID, Session["EmpID"].ToString(), Session["EmpID"].ToString(), StrPassword);
                    //else {
                    //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please input all Email ID..');", true);
                    //    return;
                    //}

                    //}
                    //else
                    //    dtadd.Rows.Add(BrokerNameId, BrokerName, UserName, UserEmailID, Session["EmpID"].ToString(), Session["EmpID"].ToString());

                }
                ViewState["CurrentTable"] = dtadd;
                SaveUserEmailID((DataTable)ViewState["CurrentTable"]);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Email IDs Save Successfully.');", true);
                BindEmailList();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        protected void txtEmail_TextChanged(object sender, EventArgs e)
        {
            TextBox txtEmail = (TextBox)sender;
            GridViewRow Row = (GridViewRow)txtEmail.NamingContainer;
            TextBox ddlGridBrokerAnalyst = (TextBox)Row.FindControl("EmailID");

            SqlParameter[] Param = { new SqlParameter("@EmailId", txtEmail.Text) };
            DataSet ds = dl.ExecuteProcedure("CheckEmailIdIsExist", Param);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                //DataTable dt = ds.Tables[0];
                string Message = dt.Rows[0][0].ToString();
                Message = "This Email ID Already Allocated by :"+Message ;
                //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + Message + "');", true);
                txtEmail.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('This Email ID Already Exists');", true);
            }
        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=RankReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gridviewexcel.AllowPaging = false;
                gridviewexcel.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gridviewexcel.HeaderRow.Cells)
                {
                    cell.BackColor = gridviewexcel.HeaderStyle.BackColor;
                    // gridviewexcel.HeaderRow.Cells[0].Visible = false;
                }
                foreach (GridViewRow row in gridviewexcel.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gridviewexcel.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gridviewexcel.RowStyle.BackColor;
                        }
                        // row.Cells[0].Visible = false;
                        // cell.CssClass = "textmode";

                    }
                }

                gridviewexcel.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }


        }
    }
}