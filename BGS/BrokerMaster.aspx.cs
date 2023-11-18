﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Data.OleDb;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Office;

namespace BGS
{
    public partial class BrokerMaster : PageBase
    {
        #region Properties
        //SRRMS_PRODEntities srrms_entity = new SRRMS_PRODEntities();
        BGSEntities bgs_entity = new BGSEntities();
        Datalayer obj = new Datalayer();
        /// <summary>
        /// Gets or sets BrokerNameID
        /// </summary>
        public int BNID
        {
            get
            {
                return ViewState["BNID"] != null ? (int)ViewState["BNID"] : 0;
            }
            set
            {
                ViewState["BNID"] = value;
            }
        }
        /// <summary>
        /// Gets or sets BrokerNameId
        /// </summary>
        public int BrokerId
        {
            get
            {
                return ViewState["BrokerId"] != null ? (int)ViewState["BrokerId"] : 0;
            }
            set
            {
                ViewState["BrokerId"] = value;
            }
        }
        /// <summary>
        /// Gets or sets BrokerNameID
        /// </summary>
        public int BNameID
        {
            get
            {
                return ViewState["BNameID"] != null ? (int)ViewState["BNameID"] : 0;
            }
            set
            {
                ViewState["BNameID"] = value;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.PopulateBrokerTypeMaster();
                this.PopulateSectorName();
                GetBrokerNameMaster();
                this.PopulateBroker();
                tdsearch.Visible = (UserTypeName == EnumUserType.Admin) ? true : false;
                btnUpload.Visible = (UserTypeName == EnumUserType.Broker) ? true : false;
                btnDownload.Visible = (UserTypeName == EnumUserType.Broker) ? true : false;
                //GetMobileData(null);
                //assignallgrids();


            }
            // UploadPannel.Attributes["style"] = "visibility: hidden";
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }
        protected void ddlBrokerTypeMaster_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //dvSectorValue.Visible = (ddlBrokerTypeMaster.SelectedItem.Text.ToUpper().Trim() == "Analyst".ToUpper().Trim());
                ddlSectorName.ClearSelection();
                
                if (ddlBrokerTypeMaster.SelectedItem.Text.ToUpper().Trim() == "Analyst".ToUpper().Trim())
                {
                    tdSectorName.Style.Add("display", "grid");
                    tdSectorValue.Style.Add("display", "grid");
                    if (EnumUserType.Admin == this.UserTypeName)
                    {
                        lnkAddSectorBtn.Visible = true;
                        lnkEditBtn.Visible = true;
                            
                    }
                    else
                    {
                        lnkAddSectorBtn.Visible = false;
                        lnkEditBtn.Visible = false;
                    }
                }
                else
                {
                    tdSectorName.Style.Add("display", "none");
                    tdSectorValue.Style.Add("display", "none");
                    rfvSectorName.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void AddBroker(object sender, EventArgs e)
        {
            try
            {
                Clear();
                GetBrokerNameMaster();
                pnlAddBroker.Visible = true;
                UploadPannel.Visible = false;
                //pnlAddNewFirm.Visible = false;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                UploadPannel.Visible = true;
                UploadPannel.Attributes["style"] = "visibility: visible";
                pnlAddBroker.Visible = false;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        //protected void btnAddNewFirm_Click(object sender, EventArgs e)
        //{
        //    //this.Response.Redirect("~/FirmMaster.aspx");
        //    //Clear();
        //    //GetBrokerNameMaster();
        //    pnlAddBroker.Visible = false;
        //    UploadPannel.Visible = false;
        //    pnlAddNewFirm.Visible = true;
        //}
        //protected void UserManual(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Response.Redirect("~\\UserMannualAssosiate.html");
        //        //string strFilePath;
        //        //strFilePath = "~\\DownloadFile\\User_Manual_for_Associates.doc";
        //        //string path = Server.MapPath(strFilePath);
        //        //System.IO.FileInfo file = new System.IO.FileInfo(path);
        //        //if (file.Exists)
        //        //{
        //        //    Response.Clear();
        //        //    Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
        //        //    Response.AddHeader("Content-Length", file.Length.ToString());
        //        //    Response.ContentType = "application/octet-stream";
        //        //    Response.WriteFile(file.FullName);
        //        //    HttpContext.Current.Response.Flush();
        //        //    HttpContext.Current.Response.SuppressContent = true;
        //        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //        //}
        //        //else
        //        //{
        //        //    Response.Write("This file does not exist.");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //protected void UserManualUpload(object sender, EventArgs e)
        //{
        //    object missingType = Type.Missing;
        //    object readOnly = true;
        //    object isVisible = false;
        //    object documentFormat = 8;
        //    //string randomName = DateTime.Now.Ticks.ToString();
        //    //object htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";
        //    //string directoryPath = Server.MapPath("~/Temp/") + randomName + "_files";

        //    ////Upload the word document and save to Temp folder
        //    //FileUpload FileUpload1 = new FileUpload();
        //    //FileUpload1.SaveAs(Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName));
        //    object fileName = "filename";

        //    //Open the word document in background
        //    Microsoft.Office.Interop.Word.ApplicationClass applicationclass = new Microsoft.Office.Interop.Word.ApplicationClass();
        //    applicationclass.Documents.Open(ref fileName,
        //                                    ref readOnly,
        //                                    ref missingType, ref missingType, ref missingType,
        //                                    ref missingType, ref missingType, ref  missingType,
        //                                    ref missingType, ref missingType, ref isVisible,
        //                                    ref missingType, ref missingType, ref missingType,
        //                                    ref missingType, ref missingType);
        //    applicationclass.Visible = false;
        //    Microsoft.Office.Interop.Word.Document document = applicationclass.ActiveDocument;

        //    //Save the word document as HTML file
        //    document.SaveAs(ref htmlFilePath, ref documentFormat, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType, ref missingType, ref missingType,
        //                    ref missingType);

        //    //Close the word document
        //    document.Close(ref missingType, ref missingType, ref missingType);

        //    //Delete the Uploaded Word File
        //    File.Delete(Server.MapPath("~/Temp/") + Path.GetFileName(FileUpload1.PostedFile.FileName));

        //    //Read the Html File as Byte Array and Display it on browser
        //    byte[] bytes;
        //    using (FileStream fs = new FileStream(htmlFilePath.ToString(), FileMode.Open, FileAccess.Read))
        //    {
        //        BinaryReader reader = new BinaryReader(fs);
        //        bytes = reader.ReadBytes((int)fs.Length);
        //        fs.Close();
        //    }
        //    Response.BinaryWrite(bytes);
        //    Response.Flush();

        //    //Delete the Html File
        //    File.Delete(htmlFilePath.ToString());
        //    foreach (string file in Directory.GetFiles(directoryPath))
        //    {
        //        File.Delete(file);
        //    }
        //    Directory.Delete(directoryPath);
        //    Response.End();
        //}

        protected void upload(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt1 = new System.Data.DataTable();
                string excelConnectionString = "";
                string path = uploadexcel.PostedFile.FileName;
                string Extension = Path.GetExtension(path);
                GetBrokerNameMaster();
                //if (Extension.ToUpper() == ".XLSX")
                //{
                //    path = path.ToUpper().Replace(".xlsx".ToUpper(), ".csv");
                //}
                //else if(Extension.ToUpper() == ".XLS")
                //{
                //    path = path.ToUpper().Replace(".xls".ToUpper(), ".csv");
                //}

                //path = string.Concat(Server.MapPath("~/UploadFile/ " + path));
                path = Server.MapPath("~//UploadFile//" + path);
                Extension = Path.GetExtension(path);
                if (Extension.ToUpper() == ".xlsx".ToUpper() || Extension.ToUpper() == ".xls".ToUpper())
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    uploadexcel.PostedFile.SaveAs(path);
                    switch (Extension.ToUpper())
                    {
                        case ".XLS": //Excel 97-03
                            excelConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + path + ";Extended Properties=Excel 12.0");
                            //OleDbConnection oconn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1;';");
                            // excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
                            break;

                        case ".XLSX": //Excel 07
                            // excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
                            excelConnectionString = ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + path + ";Extended Properties=Excel 12.0");
                            break;
                    }
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    OleDbCommand cmdExcel = new OleDbCommand();
                    OleDbDataAdapter oda = new OleDbDataAdapter();
                    DataTable dt = new DataTable();
                    DataTable finalData = new DataTable();
                    DataRow[] dr;
                    cmdExcel.Connection = excelConnection;
                    //Get the name of First Sheet
                    if (excelConnection.State == ConnectionState.Closed)
                    {
                        excelConnection.Open();
                    }

                    DataTable dtExcelSchema;
                    dtExcelSchema = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    excelConnection.Close();
                    //Read Data from First Sheet
                    excelConnection.Open();
                    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
                    oda.SelectCommand = cmdExcel;
                    oda.FillSchema(dt, SchemaType.Source);
                    dt.Columns[4].DataType = typeof(double);
                    oda.Fill(dt);
                    dr = dt.Select("BrokerType = 'Analyst' AND AnalystName <> '' AND AnalystName IS NOT NULL AND Sector <> '' AND Sector IS NOT NULL");
                    //TO ADD FINAL DATA TO DATABASE for analyst
                    finalData = dt.Clone();
                    foreach (DataRow row in dr)
                    {
                        finalData.ImportRow(row);
                    }
                    dr = dt.Select("BrokerType <> 'Analyst' AND BrokerType IS NOT NULL AND AnalystName <> '' AND AnalystName IS NOT NULL AND Sector = ''");
                    //TO ADD FINAL DATA TO DATABASE FOR NOT Analyst
                    foreach (DataRow row in dr)
                    {
                        finalData.ImportRow(row);
                    }
                    var rowsOnlyInDt1 = from r in dt.AsEnumerable()
                                            //make sure there aren't any matching names in dt2
                                        where !finalData.AsEnumerable().Any(r2 => r["BrokerType"].ToString().Trim().ToLower() == r2["BrokerType"].ToString().Trim().ToLower() && r["Sector"].ToString().Trim().ToLower() == r2["Sector"].ToString().Trim().ToLower() && r["AnalystName"].ToString().Trim().ToLower() == r2["AnalystName"].ToString().Trim().ToLower() && r["Email"].ToString().Trim().ToLower() == r2["Email"].ToString().Trim().ToLower() && r["ContactNumber"].ToString().Trim().ToLower() == r2["ContactNumber"].ToString().Trim().ToLower())
                                        select r;
                    if (rowsOnlyInDt1.FirstOrDefault() != null)
                    {
                        DataTable result = rowsOnlyInDt1.CopyToDataTable();
                        try
                        {
                            DownloadWrongData(result);
                        }
                        catch { }
                    }
                    excelConnection.Close();
                    if (finalData != null && finalData.Rows.Count > 0)
                    {
                        dt1 = obj.UploadBrokerAnalystDetails(finalData, lblBName.Text);
                    }
                    try
                    {
                        DownloadWrongData(dt1);
                    }
                    catch { }
                    this.Response.Redirect(this.Request.Url.AbsoluteUri);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('You can upload excel file only.');", true);
                }
                // PopulateBroker();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('There is error in excel file upload.');", true);
            }

        }

        private void DownloadWrongData(DataTable dt1)
        {
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                GridView gvExcelWrongData = new GridView();
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BrokerUploadErrorFile.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                gvExcelWrongData.DataSource = dt1;
                gvExcelWrongData.DataBind();
                using (StringWriter sw = new StringWriter())
                {
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    gvExcelWrongData.HeaderRow.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in gvExcelWrongData.HeaderRow.Cells)
                    {
                        cell.BackColor = gvExcelWrongData.HeaderStyle.BackColor;
                        // gvBrokerEmailList.HeaderRow.Cells[0].Visible = false;
                    }
                    foreach (GridViewRow row in gvExcelWrongData.Rows)
                    {
                        row.BackColor = System.Drawing.Color.White;
                        foreach (TableCell cell in row.Cells)
                        {
                            if (row.RowIndex % 2 == 0)
                            {
                                cell.BackColor = gvExcelWrongData.AlternatingRowStyle.BackColor;
                            }
                            else
                            {
                                cell.BackColor = gvExcelWrongData.RowStyle.BackColor;
                            }
                            // row.Cells[0].Visible = false;
                            // cell.CssClass = "textmode";

                        }
                    }

                    gvExcelWrongData.RenderControl(hw);
                    string style = @"<style> .textmode { } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    HttpContext.Current.Response.Flush();
                    HttpContext.Current.Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please check excel data are not uploaded');", true);
                // GetBrokerNameMaster();
                //PopulateBroker();
            }
            //else
            //{
            //    // GetBrokerNameMaster();
            //    //  PopulateBroker();
            //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('All records saved successfully.');", true);
            //    this.PopulateBroker();
            //}
        }
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath;
                strFilePath = "~\\DownloadFile\\BrokerUploadFormat.xls";
                DataTable dt = new DataTable();
                dt = obj.GetBrokerMaster(this.EmployeeId, (!string.IsNullOrEmpty(txtSerach.Text.Trim()) ? txtSerach.Text.Trim() : lblBName.Text.Trim()));
                //DownloadBrokerFile(dt, strFilePath);
                Download_File(strFilePath);

            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }

        }

        protected void btnExportExcel_Click(object sender, EventArgs e)
        {
            gvExcel.Visible = true;
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=BrokerList.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            var Query = bgs_entity.BGS_GetBrokerMaster(this.EmployeeId, (!string.IsNullOrEmpty(txtSerach.Text.Trim()) ? txtSerach.Text.Trim() : lblBName.Text.Trim())).ToList();
            gvExcel.DataSource = Query;
            gvExcel.DataBind();
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                gvExcel.AllowPaging = false;
                gvExcel.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in gvExcel.HeaderRow.Cells)
                {
                    cell.BackColor = gvExcel.HeaderStyle.BackColor;
                    // gvBrokerEmailList.HeaderRow.Cells[0].Visible = false;
                }
                foreach (GridViewRow row in gvExcel.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvExcel.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvExcel.RowStyle.BackColor;
                        }
                        // row.Cells[0].Visible = false;
                        // cell.CssClass = "textmode";

                    }
                }

                gvExcel.RenderControl(hw);
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }

        protected void Reset(object sender, EventArgs e)
        {
            try
            {
                txtSerach.Text = "";
                this.PopulateBroker();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void DeleteBrokerMaster(object sender, EventArgs e)
        {
            try
            {
                string Brokerid = (sender as Button).CommandArgument;

                var query = bgs_entity.BGS_DeleteBrokerMaster(Convert.ToInt32(Brokerid));
                int count = query.FirstOrDefault().Value;
                if (count == 1)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record already used so you can not delete ');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record deleted successfully.');", true);

                }
                this.PopulateBroker();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Broker is already in use so you can not delete this broker');", true);
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void EditBroker(object sender, EventArgs e)
        {
            try
            {
                ddlBrokerTypeMaster.ClearSelection();
                ddlSectorName.ClearSelection();
                int index = Convert.ToInt32((sender as Button).CommandArgument);
                this.BrokerId = Convert.ToInt32((sender as Button).CommandName);
                HiddenField BrokerNameID = (gvBrokerMaster.Rows[index].FindControl("hfBrokerNameIDEdit") as HiddenField);
                this.BNameID = Convert.ToInt32(BrokerNameID.Value);
                // Added cond EnumUserType.voter == this.UserTypeName by Prakash on 19 Dec 2017 
                if (EnumUserType.Admin == this.UserTypeName || EnumUserType.voter == this.UserTypeName)
                {
                    txtBrokerName.Text = Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[0].Text);
                }
                else
                {
                    lblBName.Text = Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[0].Text);
                }
                if (!string.IsNullOrEmpty(Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[2].Text).Trim()))
                    ddlSectorName.Items.FindByText(Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[2].Text).Trim()).Selected = true;
                ddlBrokerTypeMaster.Items.FindByText(gvBrokerMaster.Rows[index].Cells[3].Text).Selected = true;
                //dvSectorValue.Visible = (gvBrokerMaster.Rows[index].Cells[2].Text.Trim().ToUpper() == "Analyst".ToUpper());
                if (gvBrokerMaster.Rows[index].Cells[3].Text.Trim().ToUpper() == "Analyst".ToUpper())
                {
                    tdSectorName.Style.Add("display", "grid");
                    tdSectorValue.Style.Add("display", "grid");
                }
                else
                {
                    tdSectorName.Style.Add("display", "none");
                    tdSectorValue.Style.Add("display", "none");
                    rfvSectorName.Enabled = false;
                }

                txtName.Text = Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[4].Text);
                txtNumber.Text = Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[5].Text);
                txtEmail.Text = Server.HtmlDecode(gvBrokerMaster.Rows[index].Cells[6].Text);
                // txtMaxLimit.Text = gvBrokerMaster.Rows[index].Cells[6].Text;
                // txtMaxLimit.Text = (gvBrokerMaster.Rows[index].FindControl("lblMaxLimit") as Label).Text;
                btnSave.Text = "Update";

                pnlAddBroker.Visible = true;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }

        }
        protected void btnSaveBrokerMaster(object sender, EventArgs e)
        {
            try
            {
                if (btnSave.Text.Trim().ToUpper() != "Update".ToUpper())
                {
                    this.BrokerId = 0;
                }
                int BrokerNameID = 0;
                string brokerName = string.Empty;
                if (EnumUserType.Broker == this.UserTypeName)
                {
                    BrokerNameID = (this.BNID);
                    brokerName = (lblBName.Text.Length > 0) ? (lblBName.Text) : (null);
                }
                else
                {
                    
                   // if(string.IsNullOrWhiteSpace(Request.Form[hfBrokerNameID.UniqueID].ToString()))
                    BrokerNameID = btnSave.Text == "Save" ? Convert.ToInt16(Request.Form[hfBrokerNameID.UniqueID].ToString()) : this.BNameID;
                    brokerName = (txtBrokerName.Text.Length > 0) ? (txtBrokerName.Text) : (null);
                }
                int strSectorId = (ddlSectorName.SelectedItem.Value.Length > 0) ? int.Parse(ddlSectorName.SelectedItem.Value) : 0;
                int strBrokerTYpeId = (ddlBrokerTypeMaster.SelectedItem.Value.Length > 0) ? int.Parse(ddlBrokerTypeMaster.SelectedItem.Value) : 0;
                // int strMaxLimit = (txtMaxLimit.Text.Length > 0) ? int.Parse(txtMaxLimit.Text) : 0;
                string strName = (txtName.Text.Length > 0) ? (txtName.Text) : (null);
                string strNumber = (txtNumber.Text.Length > 0) ? (txtNumber.Text) : (null);
                string strEmail = (txtEmail.Text.Length > 0) ? (txtEmail.Text) : (null);
                //System.Data.Entity.Core.Objects.ObjectParameter output = new System.Data.Entity.Core.Objects.ObjectParameter("Bit", typeof(bool));
                //bool AutenticateBroker = Convert.ToBoolean(bgs_entity.BGS_AuthenticateBrokerMaster(BrokerNameID, strBrokerTYpeId, strName, strSectorId,output));

                var query = bgs_entity.BGS_AuthenticateBrokerMaster(BrokerNameID, strBrokerTYpeId, strName, strSectorId, this.BrokerId);

                int count = query.FirstOrDefault().Value;

                if (count == 0)
                {
                    bgs_entity.BGS_BrokerMaster("INSERT", this.BrokerId, strSectorId, strBrokerTYpeId, strName, strNumber, strEmail, this.USERNAME, this.USERNAME, BrokerNameID);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record saved successfully.');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record already exist:');", true);
                }
                pnlAddBroker.Visible = false;
                this.PopulateBroker();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void CancelBrokerGrading(object sender, EventArgs e)
        {
            try
            {
                this.Response.Redirect(this.Request.Url.AbsoluteUri);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void txtSerach_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var data = bgs_entity.BGS_GetBrokerMaster(this.EmployeeId, txtSerach.Text).ToList();
                gvBrokerMaster.AllowPaging = false;
                gvBrokerMaster.DataSource = data;
                gvBrokerMaster.DataBind();
                lblrowcount.Text = Convert.ToString(data.Count);
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        protected void BrokerPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvBrokerMaster.PageIndex = e.NewPageIndex;
                PopulateBroker();
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        #region Private Methods
        /// <summary>
        /// Populates Sector Name
        /// </summary>
        private void PopulateSectorName()
        {
            try
            {
                ddlSectorName.Items.Clear();
                ddlSectorName.DataSource = from n in bgs_entity.BGS_GetSector() select new { n.SectorID, n.SectorName };
                ddlSectorName.DataTextField = "SectorName";
                ddlSectorName.DataValueField = "SectorId";
                ddlSectorName.DataBind();
                ddlSectorName.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        /// <summary>
        /// Populates Broker
        /// </summary>
        private void PopulateBroker()
        {
            try
            {
                var Query = bgs_entity.BGS_GetBrokerMaster(this.EmployeeId, (!string.IsNullOrEmpty(txtSerach.Text.Trim()) ? txtSerach.Text.Trim() : lblBName.Text.Trim())).ToList();
                gvBrokerMaster.DataSource = Query;
                gvBrokerMaster.DataBind();
                if (UserTypeName != EnumUserType.Admin)
                {
                    gvBrokerMaster.Columns[6].Visible = false;
                }

                lblrowcount.Text = Convert.ToString(Query.Count());
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        /// <summary>
        /// Populates BrokerType Master
        /// </summary>
        private void PopulateBrokerTypeMaster()
        {
            try
            {
                ddlBrokerTypeMaster.Items.Clear();
                ddlBrokerTypeMaster.DataSource = from n in bgs_entity.BGS_GetBrokerTypeMasterData() select new { n.BrokerTypeID, n.BrokerType };
                ddlBrokerTypeMaster.DataTextField = "BrokerType";
                ddlBrokerTypeMaster.DataValueField = "BrokerTypeID";
                ddlBrokerTypeMaster.DataBind();
                ddlBrokerTypeMaster.Items.Insert(0, (new ListItem { Text = "Please Select", Value = string.Empty }));
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        /// <summary>
        /// GET Broker NAME AND BROKER NAME ID
        /// </summary>
        private void GetBrokerNameMaster()
        {
            try
            {
                if (EnumUserType.Broker == this.UserTypeName)
                {
                    var query = (from C in bgs_entity.BGD_GetBrokerNameMaster(this.EmployeeId)
                                 select new { C.BrokerName, C.BrokerNameID }).FirstOrDefault();
                    lblBName.Text = query.BrokerName;
                    this.BNID = query.BrokerNameID;
                    lblBName.Visible = ((EnumUserType.Broker == this.UserTypeName) ? true : false);
                    txtBrokerName.Visible = ((EnumUserType.Admin == this.UserTypeName) ? true : false);
                    //AddSector.Visible = ((EnumUserType.Admin == this.UserTypeName) ? true : false);
                }
                else
                {
                    lblBName.Visible = ((EnumUserType.Admin == this.UserTypeName) || (EnumUserType.voter == this.UserTypeName) ? false : true);
                    txtBrokerName.Visible = ((EnumUserType.Admin == this.UserTypeName) || (EnumUserType.voter == this.UserTypeName) ? true : false);
                }
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
        private void Clear()
        {
            try
            {
                txtBrokerName.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtName.Text = string.Empty;
                txtNumber.Text = string.Empty;
                ddlBrokerTypeMaster.ClearSelection();
                ddlSectorName.ClearSelection();
                lblBName.Text = string.Empty;
                txtSerach.Text = string.Empty;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }

        public void Download_File(string FilePath)
        {
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            Response.WriteFile(FilePath);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void DownloadBrokerFile(DataTable dt, string FilePath)
        {
            //using (XLWorkbook wb = new XLWorkbook())
            //{
            //    wb.Worksheets.Add(dt, "BrokerData");

            //    Response.Clear();
            //    Response.Buffer = true;
            //    Response.Charset = "";
            //    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    Response.ContentType = ContentType;
            //    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(FilePath));
            //    using (MemoryStream MyMemoryStream = new MemoryStream())
            //    {
            //        wb.SaveAs(MyMemoryStream);
            //        MyMemoryStream.WriteTo(Response.OutputStream);
            //        Response.Flush();
            //        HttpContext.Current.Response.SuppressContent = true;
            //        HttpContext.Current.ApplicationInstance.CompleteRequest();
            //    }
            //}
        }

        //protected void btnSaveFirmMaster_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (txtFirmName.Text != "" && btnSaveFirmMaster.Text == "Save")
        //        {
        //            bgs_entity.BGS_InsertUpdateFirmMaster(0, txtFirmName.Text.Trim(), Session["EmpID"].ToString(), DateTime.Now, null, null, ChkDelStatus.Checked, Convert.ToInt32(txtMaxLimit.Text.Trim()), ChkEmailStatus.Checked, Convert.ToInt32(txtDofaCode.Text.Trim()));
        //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record saved successfully.');", true);
        //        }
        //        else if (txtFirmName.Text != "" && btnSaveFirmMaster.Text == "Update")
        //        {
        //            bgs_entity.BGS_InsertUpdateFirmMaster(1, txtFirmName.Text.Trim(), null, null, Session["EmpID"].ToString(), DateTime.Now, ChkDelStatus.Checked, Convert.ToInt32(txtMaxLimit.Text.Trim()), ChkEmailStatus.Checked, Convert.ToInt32(txtDofaCode.Text.Trim()));
        //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Record Update successfully.');", true);
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please input Firm Name');", true);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BGS.Error.EventLogProcess(ex);
        //    }
        //}
        //protected void btnCancelFirmMaster_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        this.Response.Redirect(this.Request.Url.AbsoluteUri);
        //    }
        //    catch (Exception ex)
        //    {
        //        BGS.Error.EventLogProcess(ex);
        //    }
        //}


        #endregion

        //protected void txtFirmName_TextChanged(object sender, EventArgs e)
        //{
        //    if (txtFirmName.Text != "")
        //    {
        //        var query = (from C in bgs_entity.BGS_GetBrokerNameForBrokerMster(txtFirmName.Text)
        //                     select new { C }).FirstOrDefault();

        //        if (query != null)
        //        {
        //            ChkDelStatus.Checked = Convert.ToBoolean(query.C.DelStatus);
        //            txtMaxLimit.Text = query.C.MaxLimit.ToString();
        //            ChkEmailStatus.Checked = Convert.ToBoolean(query.C.EmailStatus);
        //            txtDofaCode.Text = query.C.Broker_DofaCode.ToString();
        //        }
        //        btnSaveFirmMaster.Text = "Update";
        //    }
        //}
        //void assignallgrids()
        //{
        //    //if (gvFax != null && gvFax.Rows.Count > 0)
        //    //{
        //    //    TextBox Txtvalue = (TextBox)gvFax.HeaderRow.FindControl("txtFaxgv");
        //    //    if (Txtvalue != null && Txtvalue.Text != String.Empty)
        //    //    {
        //    //        DataTable dtResult = (DataTable)ViewState["myFaxTable"];
        //    //        dtResult.Rows.Add(Txtvalue.Text);
        //    //        ViewState["myFaxTable"] = dtResult;
        //    //        Txtvalue.Text = "";
        //    //    }
        //    //}
        //    if (gvSectorName != null && gvSectorName.Rows.Count > 0)
        //    {
        //        TextBox TxtvalueM = (TextBox)gvSectorName.HeaderRow.FindControl("txtSectorNamegv");
        //        if (TxtvalueM != null && TxtvalueM.Text != String.Empty)
        //        {
        //            DataTable dtResult = (DataTable)ViewState["mySectorNameTable"];
        //            dtResult.Rows.Add(TxtvalueM.Text);
        //            ViewState["mySectorNameTable"] = dtResult;
        //            TxtvalueM.Text = "";
        //        }
        //    }

        //    //if (gvEmail != null && gvEmail.Rows.Count > 0)
        //    //{
        //    //    TextBox TxtvalueE = (TextBox)gvEmail.HeaderRow.FindControl("txtEmailgv");
        //    //    if (TxtvalueE != null && TxtvalueE.Text != String.Empty)
        //    //    {
        //    //        DataTable dtResult = (DataTable)ViewState["myEmailTable"];
        //    //        dtResult.Rows.Add(TxtvalueE.Text);
        //    //        ViewState["myEmailTable"] = dtResult;
        //    //        TxtvalueE.Text = "";
        //    //    }
        //    //}

        //    //DataTable dtFax = (DataTable)ViewState["myFaxTable"];
        //    //gvFax.DataSource = dtFax;
        //    //gvFax.DataBind();

        //    //DataTable dtEmail = (DataTable)ViewState["myEmailTable"];
        //    //gvEmail.DataSource = dtEmail;
        //    //gvEmail.DataBind();

        //    DataTable dtMobile = (DataTable)ViewState["mySectorNameTable"];
        //    gvSectorName.DataSource = dtMobile;
        //    gvSectorName.DataBind();
        //}
        //protected void gvMobile_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    //if (e.CommandName == "SectorNameDelete")
        //    //{
        //    //    int index = Convert.ToInt32(e.CommandArgument);
        //    //    if (ViewState["mySectorNameTable"] != null)
        //    //    {
        //    //        DataTable dtResult = (DataTable)ViewState["mySectorNameTable"];
        //    //        if (dtResult.Rows.Count > 0)
        //    //        {
        //    //            dtResult.Rows.RemoveAt(index);
        //    //            if (dtResult.Rows.Count == 0)
        //    //            {
        //    //                dtResult.Rows.Add("");
        //    //                //dtResult.Rows.Add("Please add Mobile");
        //    //            }
        //    //            ViewState["mySectorNameTable"] = dtResult;
        //    //            assignallgrids();
        //    //        }
        //    //    }
        //    //}
        //    if (e.CommandName == "SectorNameAdd")
        //    {
        //        Label lblvalue = (Label)gvSectorName.HeaderRow.FindControl("rdSectorName");
        //        TextBox Txtvalue = (TextBox)gvSectorName.HeaderRow.FindControl("txtSectorNamegv");
        //        if (Txtvalue != null && Txtvalue.Text == String.Empty)
        //        {

        //            lblvalue.Visible = true;
        //            //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Please enter value');", true);
        //            return;
        //        }
        //        if (gvSectorName.Rows.Count > 9)
        //        {
        //            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Only 10 entires are allowed');", true);
        //            return;
        //        }
        //        // \w+([-+.']\w+)*@\w+([-.]\w+)*\.(com|co\.(in|uk|sg)|org|in\sg)$

        //        lblvalue.Visible = false;
        //        if (Txtvalue.Text != null)
        //        { 
        //                obj.InsetSectorNameData(Txtvalue.Text.Trim().ToUpper(), Session["EmpID"].ToString(), null);
        //                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Saved sector name Sucessfully..');", true);
        //                return;

        //        }
        //        //if (ViewState["mySectorNameTable"] != null)
        //        //{
        //        //    DataTable dtResult = (DataTable)ViewState["mySectorNameTable"];
        //        //    if (dtResult.Rows.Count > 0)
        //        //    {
        //        //        int introwcount = 0;
        //        //        for (int i = 0; i <= introwcount; i++)
        //        //        {
        //        //            if (dtResult.Rows[i][0].ToString() == "")

        //        //            //if (dtResult.Rows[i][0].ToString() == "Please add Mobile")
        //        //            {
        //        //                dtResult.Rows.RemoveAt(i);
        //        //                --introwcount;
        //        //            }
        //        //        }

        //        //        //dtResult.Rows.Add(Txtvalue.Text);
        //        //        //ViewState["myMobileTable"] = dtResult;
        //        //        //assignallgrids();
        //        //    }
        //        //}
        //    }
        //}

        //protected void gvMobile_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lblvalue = (Label)e.Row.Cells[0].FindControl("lblSectorNamegv");
        //        //Label lblvalue = (Label)gvFax.Rows[e.Row.RowIndex][0].FindControl("lblFaxgv");
        //        //if (lblvalue != null && lblvalue.Text == "")
        //        //{
        //        //    LinkButton lnkbtn = (LinkButton)e.Row.Cells[0].FindControl("btnDelete");
        //        //    lnkbtn.Visible = false;
        //        //}

        //    }
        //}
        protected void txtSectorName_TextChanged(object sender, EventArgs e)
        {
            if (txtSectorName.Text != null)
            {
                DataTable DT = new DataTable();
                DT = obj.CheckSectorName(txtSectorName.Text.Trim());
                if (DT.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('All ready exists sector');", true);
                }
            }

        }
        //void GetMobileData(SqlDataReader reader)
        //{
        //    bool blnempty = false;

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("SectorName");

        //    if (reader != null)
        //    {
        //        for (int i = 1; i <= 10; i++)
        //        {
        //            if (!String.IsNullOrEmpty(reader["SectorName" + i.ToString()].ToString()))
        //            {
        //                dt.Rows.Add(reader["SectorName" + i.ToString()].ToString());
        //                blnempty = true;
        //            }
        //        }


        //    }
        //    if (!blnempty)
        //    {
        //        dt.Rows.Add("");
        //        //dt.Rows.Add("Please add Mobile");
        //    }


        //    ViewState["mySectorNameTable"] = dt;

        //}
        protected void AddSector_Click(object sender, EventArgs e)
        {
            try
            {
                if (EnumUserType.Admin == this.UserTypeName)
                {
                    //tdSectortext.Style.Add("display", "grid");
                    tdaddSector.Style.Add("display", "grid");
                    lnkAddBtn.Visible = true;
                    //lnkAddSectorBtn.Visible = false;
                    lnkUpdateBtn.Visible = false;
                    lnkDeleteBtn.Visible = false;
                }
                else
                {
                    //tdSectortext.Style.Add("display", "none");
                    tdaddSector.Style.Add("display", "none");
                    lnkAddBtn.Visible = false;
                    lnkAddSectorBtn.Visible = true;
                }
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('" + ex.Message + "');", true);
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('" + ex.Message + "');", true);
            }

        }

        protected void lnkEditBtn_Click(object sender, EventArgs e)
        {
            if (ddlSectorName.SelectedItem.Text != "Please Select")
            {
                if (EnumUserType.Admin == this.UserTypeName)
                {
                    txtSectorName.Text = ddlSectorName.SelectedItem.Text;
                    tdaddSector.Style.Add("display", "grid");
                    lnkAddBtn.Visible = false;
                    lnkUpdateBtn.Visible = true;
                    lnkDeleteBtn.Visible = true;
                }
                else
                {
                    //tdSectortext.Style.Add("display", "none");
                    tdaddSector.Style.Add("display", "none");
                    lnkUpdateBtn.Visible = false;
                    lnkDeleteBtn.Visible = false;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Please select Sector name!!');", true);
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Please select Sector name!!');", true);
            }
        }

        //protected void ddlSectorName_TextChanged(object sender, EventArgs e)
        //{
        //    if (EnumUserType.Admin == this.UserTypeName)
        //    {
        //        lnkEditBtn.Visible = true;
        //        lnkAddSectorBtn.Visible = false;
        //    }
        //    else
        //    {
        //        lnkAddSectorBtn.Visible = true;
        //        lnkEditBtn.Visible = false;
        //    }
        //}

        protected void lnkUpdateBtn_Click(object sender, EventArgs e)
        {
           // lnkEditBtn.Visible = false;
            if (txtSectorName.Text != "")
            {
                DataTable DT = new DataTable();
                DT = obj.CheckSectorName(txtSectorName.Text.Trim());
                if (DT.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('All ready exists sector');", true);
                }
                else
                {
                    obj.InsetUpdateSectorNameData(Convert.ToInt32(ddlSectorName.SelectedItem.Value), txtSectorName.Text.Trim().ToUpper(), null, Session["EmpID"].ToString());
                    //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Updated sector name Successfully..');", true);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Updated sector name Successfully.');", true);
                    PopulateSectorName();
                    tdaddSector.Style.Add("display", "none");
                    txtSectorName.Text = "";
                    //lnkAddSectorBtn.Visible = true;
                }
            }
        }
        protected void lnkDeleteBtn_Click(object sender, EventArgs e)
        {
           //lnkEditBtn.Visible = false;
            if (txtSectorName.Text != "")
            {
                obj.DeleteSectorNameData(Convert.ToInt32(ddlSectorName.SelectedItem.Value));
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Deleted sector name Successfully..');", true);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('Deleted sector name Successfully.');", true);
                PopulateSectorName();
                tdaddSector.Style.Add("display", "none");
                txtSectorName.Text = "";
                lnkAddSectorBtn.Visible = true;
            }

        }
        protected void lnkAddBtn_Click(object sender, EventArgs e)
        {
            if (txtSectorName.Text != "")
            {
                DataTable DT = new DataTable();
                DT = obj.CheckSectorName(txtSectorName.Text.Trim());
                if (DT.Rows.Count > 0)
                {
                    Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('All ready exists sector');", true);
                }
                else
                {
                    if (ddlSectorName.SelectedItem.Text == "Please Select")
                    {
                        ddlSectorName.SelectedItem.Value = "0";
                    }
                    obj.InsetUpdateSectorNameData(Convert.ToInt32(ddlSectorName.SelectedItem.Value), txtSectorName.Text.Trim().ToUpper(), Session["EmpID"].ToString(), null);
                    //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "scriptKey", "alert('Saved sector Successfully..');", true);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "alter", "alert('sector saved successfully.');", true);
                    PopulateSectorName();
                    tdaddSector.Style.Add("display", "none");
                    txtSectorName.Text = "";
                }
            }
        }
    }
}