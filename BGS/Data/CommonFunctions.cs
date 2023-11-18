using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace BGS
{
    public class CommonFunctions
    {
        public string PopulateBody(String StrStage)
        {
            string body = string.Empty;

            String StrEmailPage = String.Empty;
            String TSEmailTemplatePath = ConfigurationManager.AppSettings["EmailTemplate"]; //HttpContext.Current.Request.Url.AbsolutePath; //ConfigurationManager.AppSettings["EmailTemplate"];
            switch (StrStage)
            {
                case "IG":
                    StrEmailPage = "InitiateGrading.html";
                    break;
                case "NewBrokerRegistered":
                    StrEmailPage = "UserCreation.html";
                    break;
                case "ReOpenVoting":
                    StrEmailPage = "ReOpen.html";
                    break;
                case "BrokerEmailSend":
                    StrEmailPage = "BrokerSendEmail.html";
                    break;
            }
            string path = AppDomain.CurrentDomain.BaseDirectory;
            using (StreamReader reader = new StreamReader(string.Format("{0}EmailTemplate\\{1}", path, StrEmailPage)))
            {
                body = reader.ReadToEnd();
            }
            return body;
        }
    }
}