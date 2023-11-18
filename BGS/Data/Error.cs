﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Text;
using BGS;

namespace BGS
{
    /// <summary>
    /// Summary description for CommanClass
    /// </summary>
    public class Error
    {
        static BGSEntities bgs_entity = new BGSEntities();
        public static string strSiteCurrentNodeUrl = String.Empty;
        public static string strItemSearchType = String.Empty;
        public static string strErrorOccure = String.Empty;
        public static string CreatedBy = String.Empty;
        public Error()
        {
        }
        #region Erro Log
        public static void EventLogProcess(Exception ex)
        {
            try
            {
                string strPath = string.Format("{0}//ErrorLogs", AppDomain.CurrentDomain.BaseDirectory);//ConfigurationManager.AppSettings["ErrorLoge"];
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strFileName = "/Error_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                int code = System.Runtime.InteropServices.Marshal.GetExceptionCode();
                string Created_By = string.Empty;
                Created_By = CreatedBy;
                strErrorOccure = HttpContext.Current.Request.Path;
                strErrorOccure = strErrorOccure.Substring(1, strErrorOccure.Length - 1);
                bgs_entity.BGS_GetError(strErrorOccure, code.ToString(), ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd").ToString(), Created_By);
                FileStream Fs = new FileStream(strPath + strFileName, FileMode.Append);
                FileInfo file = new FileInfo(strPath + strFileName);
                BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
                sb.AppendLine();
                sb.AppendLine(">>");
                sb.Append(" Error Occurred:--");
                sb.Append(Error.strSiteCurrentNodeUrl);
                sb.Append(DateTime.Now.ToString());
                sb.AppendLine(">>");
                sb.Append(ex.Message);
                sb.AppendFormat("\\");
                sb.AppendLine(">>");
                sb.Append(ex.StackTrace);
                sb.AppendFormat("\\");
                BWriter.Write(sb.ToString());
                Fs.Close();
            }
            catch(Exception EX)
            {
                EventLogProcess("ERROR IN"+ex);
            }
        }
        public static void EventLogProcess(Exception ex, string strUserMessage)
        {
            try
            {
                string strPath = ConfigurationManager.AppSettings["logfileloc"];
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strFileName = "/Error_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                FileStream Fs = new FileStream(strPath + strFileName, FileMode.Append);
                FileInfo file = new FileInfo(strPath + strFileName);
                BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
                sb.AppendLine();
                sb.AppendLine(">>");
                sb.Append(" Error Occurred:--");
                sb.Append(Error.strSiteCurrentNodeUrl);
                sb.Append(DateTime.Now.ToString());
                sb.AppendLine(">>");
                sb.Append(ex.Message);
                sb.AppendFormat("\\");
                sb.AppendLine(">>");
                sb.Append(ex.StackTrace + " " + strUserMessage);
                sb.AppendFormat("\\");
                BWriter.Write(sb.ToString());
                Fs.Close();
            }
            catch
            {
                EventLogProcess(ex);
            }
        }

        public static void EventLogProcess(string strMessge)
        {
            try
            {
                string strPath = ConfigurationManager.AppSettings["logfileloc"];
                StringBuilder sb = new StringBuilder();
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
                string strFileName = "/Error_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
                FileStream Fs = new FileStream(strPath + strFileName, FileMode.Append);
                FileInfo file = new FileInfo(strPath + strFileName);
                BinaryWriter BWriter = new BinaryWriter(Fs, Encoding.GetEncoding("UTF-8"));
                sb.AppendLine();
                sb.AppendLine(">>");
                sb.Append(" Error Occurred:--");
                sb.Append(Error.strSiteCurrentNodeUrl);
                sb.Append(DateTime.Now.ToString());
                sb.AppendLine(">>");
                sb.Append(strMessge);
                sb.AppendFormat("\\");
                sb.AppendLine(">>");
                sb.Append(strMessge);
                sb.AppendFormat("\\");
                BWriter.Write(sb.ToString());

                Fs.Close();

            }
            catch
            {

            }
        }
        public static void MailErrortoAdministrator(String strErrorMessage)
        {
            try
            {
                //MailMessage mail = new MailMessage();
                //mail.From = "pawas.goyal@cylsys.com";
                //mail.To = "pawas.goyal@cylsys.com";
                //mail.Subject = "Error in the TOLIA";
                //mail.Priority = MailPriority.High;
                //mail.BodyFormat = MailFormat.Text;
                //mail.Body = strErrorMessage;
                //SmtpMail.SmtpServer = "mail.cylsys.com";
                //SmtpMail.Send(mail);
            }
            catch (Exception ex)
            {
                EventLogProcess(ex);
            }
        }
        #endregion
    }
}