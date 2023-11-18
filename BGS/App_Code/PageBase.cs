using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BGS
{
    public class PageBase : System.Web.UI.Page
    {
        /// <summary>
        /// Gets or sets EmployeeId
        /// </summary>
        public string EmployeeId
        {
            get
            {
                return Session["EmpId"] != null ? Convert.ToString(Session["EmpId"]) : null;
            }
            set
            {
                Session["EmpId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets ISUTI
        /// </summary>
        public bool ISUTI
        {
            get
            {
                return Session["ISUTI"] != null ? Convert.ToBoolean(Session["ISUTI"]) : false;
            }
            set
            {
                Session["ISUTI"] = value;
            }
        }

        /// <summary>
        /// Gets or sets ISFORCE
        /// </summary>
        public bool ISFORCE
        {
            get
            {
                return Session["ISFORCE"] != null ? Convert.ToBoolean(Session["ISFORCE"]) : false;
            }
            set
            {
                Session["ISFORCE"] = value;
            }
        }

        /// <summary>
        /// Gets or sets ROLEID
        /// </summary>
        public int ROLEID
        {
            get
            {
                return Session["ROLEID"] != null ? Convert.ToInt32(Session["ROLEID"]) : 0;
            }
            set
            {
                Session["ROLEID"] = value;
            }
        }

        /// <summary>
        /// Gets or sets USERNAME
        /// </summary>
        public string USERNAME
        {
            get
            {
                return Session["USERNAME"] != null ? Session["USERNAME"].ToString() : null;
            }
            set
            {
                Session["USERNAME"] = value;
            }
        }

        /// <summary>
        /// Gets or sets GradingId
        /// </summary>
        public int GradingId
        {
            get
            {
                return Session["GradingId"] != null ? Convert.ToInt32(Session["GradingId"]) : 0;
            }
            set
            {
                Session["GradingId"] = value;
            }
        }

        /// <summary>
        /// Gets or sets UserTypeName
        /// </summary>
        public string UserTypeName
        {
            get
            {
                return Session["UserTypeName"] != null ? Session["UserTypeName"].ToString() : null;
            }
            set
            {
                Session["UserTypeName"] = value;
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (this.Request.Url.Segments[this.Request.Url.Segments.Length - 1].ToLower() != "LoginMaster.aspx".ToLower())
            {
                if (Session["Users"] == null)
                {
                    Response.Redirect(LoginMaster_Page.GetUrl());
                }
            }
        }
    }
}