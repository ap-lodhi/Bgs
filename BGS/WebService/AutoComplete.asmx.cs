using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace BGS.WebService
{
    /// <summary>
    /// Summary description for AutoComplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AutoComplete : System.Web.Services.WebService
    {
        BGSEntities bgs_entity = new BGSEntities();
        public AutoComplete()
        { }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string[] GetQuarters(string prefix)
        {
            List<string> quaters = new List<string>();
            foreach (var item in bgs_entity.BGS_GetQuarterName(prefix))
            {
                quaters.Add(string.Format("{0}-{1}", item.Name, item.QuarterID));
            }
            return quaters.ToArray();
        }

        //[WebMethod]
        //public string[] GetBrokerName(string prefixText)
        //{
        //    List<string> Broker = new List<string>();
        //    foreach (var item in bgs_entity.BGS_GetBrokerOrFirmName(prefixText))
        //    {
        //        string item1 = 
        //        Broker.Add(item1);
        //    }
        //    return Broker.ToArray();
        //}
        [WebMethod]
        public string[] BrokerSearch(string prefixText)
        {
            List<string> Broker = new List<string>();
            try
            {
                foreach (var item in bgs_entity.BGS_GetBrokerNameForBrokerMster(prefixText))
                {
                    string item1 = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(item.BrokerName, item.BrokerNameID.ToString());
                    Broker.Add(item1);
                }
            }
            catch(Exception ex) {
                WriteLog("BrokerSearch" + ex.Message);
            }
            return Broker.ToArray();
        }
        private void WriteLog(string message)
        {
            string ErrorLogDir = ConfigurationManager.AppSettings["ErrorLogFile"];
            if (!Directory.Exists(ErrorLogDir))
                Directory.CreateDirectory(ErrorLogDir);

            ErrorLogDir += "\\EmailUtilityErrorLog_" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";

            using (StreamWriter sw = new StreamWriter(ErrorLogDir, true))
            {
                sw.WriteLine(DateTime.Now.ToString("dd-MMM-yy HH:mm:ss") + "\t" + message);
            }
        }
        [WebMethod]
        public string[] PopulateBrokerName(string prefixText)
        {
            List<string> Broker = new List<string>();
           
                foreach (var item in bgs_entity.BGS_GetBrokerName(prefixText))
                {
                    string item1 = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(item.BrokerName, item.BrokerNameID.ToString());
                    Broker.Add(item1);
                }
            return Broker.ToArray();
        }
        [WebMethod]
        public string[] PopulateReasons(string prefixText)
        {
            List<string> Reason = new List<string>();

            foreach (var item in bgs_entity.BGS_GetDOD_DOFAReason(prefixText))
            {
                string item1 = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(item.ReasonName, item.ReasonID.ToString());
                Reason.Add(item1);
            }
            return Reason.ToArray();
        }
        [WebMethod]
        public string[] UserSearch(string prefixText)
        {

            List<string> User = new List<string>();

            foreach (var item in bgs_entity.BGS_SearchUser(prefixText))
            {
                User.Add(item);
            }
            return User.ToArray();
        }

    }
}