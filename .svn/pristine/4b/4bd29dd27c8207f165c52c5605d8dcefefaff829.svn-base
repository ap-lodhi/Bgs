using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Linq;


namespace BGS
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        DataTable dt = new DataTable();
        Datalayer DL = new Datalayer();
        BGSEntities bgs_entity = new BGSEntities();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["EmpId"] == null)
            {
                Response.Redirect("LoginMaster.aspx");
            }
            else
            {
                if (DL.IsUserValidForAccess(Session["EmpId"].ToString(), this.Request.Url.Segments[this.Request.Url.Segments.Length - 1]) != 1)
                {
                    string page = this.Request.Url.Segments[this.Request.Url.Segments.Length - 1].ToUpper();
                    if (page != "Home.aspx".ToUpper() && page != "ScreenAllocation.aspx".ToUpper() && page != "BrokerEmailListing.aspx".ToUpper() && page != "CalculateRank.aspx".ToUpper())
                    {
                      
                        Response.Redirect("LoginMaster.aspx");
                    }
                }          
               
            }
            
            if (!this.IsPostBack)
            {
                
                PopulateMenuItem(null);
                if (Session["UserTypeName"].ToString().ToUpper().Trim() == "Broker".ToUpper().Trim())
                {
                    lblLogotext.Text = "User Management System";
                }
            }
        }
        protected void btnLogOff_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginMaster.aspx");
        }
        private void createdataTable()
        {
            dt.Columns.Add("ModuleId", typeof(int));
            dt.Columns.Add("ModuleName", typeof(string));
            dt.Columns.Add("IsParent", typeof(Boolean));
            dt.Columns.Add("ScreenId", typeof(int));
            dt.Columns.Add("ScreenName", typeof(string));
            dt.Columns.Add("URL", typeof(string));
        }
        private void PopulateMenuItem(MenuItem parentMenuItem)
        {
            if (Session["EmpID"] == null)
            {
                return;
            }
            createdataTable();
            string currentPage = Path.GetFileName(this.Request.Url.AbsoluteUri);
            var q = (from c in bgs_entity.GetMenuItem_Employee(Session["EmpID"].ToString())
                     select c);
            q.ToList().ForEach((n) =>
            {
                DataRow row = dt.NewRow();
                row.SetField<int?>("ModuleId", n.ModuleId);
                row.SetField<string>("ModuleName", n.ModuleName);
                row.SetField<Boolean?>("IsParent", n.IsParent);
                row.SetField<int?>("ScreenId", n.ScreenId);
                row.SetField<string>("ScreenName", n.ScreenName);
                row.SetField<string>("URL", n.URL);
                dt.Rows.Add(row);
            });
            foreach (DataRow item in dt.Rows)
            {
                MenuItem menuItem = new MenuItem
               {
                   Value = item["ScreenId"].ToString(),
                   Text = item["ScreenName"].ToString(),
                   NavigateUrl = item["URL"].ToString(),
                   Selected = item["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
               };

                if (Convert.ToBoolean(item["IsParent"]))
                {
                    if (EnumUserType.voter.ToUpper().Trim() == Session["UserTypeName"].ToString().ToUpper().Trim())
                    {
                        if (item["URL"].ToString().ToLower() == "javascript:;".ToLower())
                        {
                            int moduleId = (int)item["ModuleId"];
                            DataRow[] rows = dt.Select("ModuleId = " + moduleId);
                            foreach (DataRow row in rows)
                            {
                                if (Convert.ToBoolean(row["IsParent"]) == false)
                                {
                                    MenuItem newmenuItem = new MenuItem
                                    {
                                        Value = row["ScreenId"].ToString(),
                                        Text = row["ScreenName"].ToString(),
                                        NavigateUrl = row["URL"].ToString(),
                                        Selected = row["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                                    };
                                    menuItem.ChildItems.Add(newmenuItem);
                                }
                            }
                        }
                        Menu1.Items.Add(menuItem);
                    }
                    else if (EnumUserType.Broker.ToUpper().Trim() == Session["UserTypeName"].ToString().ToUpper().Trim())
                    {
                        if (item["URL"].ToString().ToLower() == "javascript:;".ToLower())
                        {
                            int moduleId = (int)item["ModuleId"];
                            DataRow[] rows = dt.Select("ModuleId = " + moduleId);
                            foreach (DataRow row in rows)
                            {
                                if (Convert.ToBoolean(row["IsParent"]) == false)
                                {
                                    MenuItem newmenuItem = new MenuItem
                                    {
                                        Value = row["ScreenId"].ToString(),
                                        Text = row["ScreenName"].ToString(),
                                        NavigateUrl = row["URL"].ToString(),
                                        Selected = row["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                                    };
                                    menuItem.ChildItems.Add(newmenuItem);
                                }
                            }
                        }
                        Menu1.Items.Add(menuItem);
                    }
                    else if (EnumUserType.Admin.ToUpper().Trim() == Session["UserTypeName"].ToString().ToUpper().Trim())
                    {
                        if (item["URL"].ToString().ToLower() == "javascript:;".ToLower())
                        {
                            int moduleId = (int)item["ModuleId"];
                            DataRow[] rows = dt.Select("ModuleId = " + moduleId);
                            foreach (DataRow row in rows)
                            {
                                if (Convert.ToBoolean(row["IsParent"]) == false)
                                {
                                    MenuItem newmenuItem = new MenuItem
                                    {
                                        Value = row["ScreenId"].ToString(),
                                        Text = row["ScreenName"].ToString(),
                                        NavigateUrl = row["URL"].ToString(),
                                        Selected = row["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                                    };
                                    menuItem.ChildItems.Add(newmenuItem);
                                }
                            }
                        }
                        Menu1.Items.Add(menuItem);
                    }
                }
            }
        }
        protected void OnMenuItemDataBound(object sender, MenuEventArgs e)
        {
            if (SiteMap.CurrentNode != null)
            {
                if (e.Item.Text == SiteMap.CurrentNode.Title)
                {
                    if (e.Item.Parent != null)
                    {
                        e.Item.Parent.Text = "<span class='orange' >" + e.Item.Parent.Text + "</span>";
                        if (e.Item.Parent.Selected)
                        {
                            e.Item.Parent.Selected = true;
                        }
                    }
                    else
                    {
                        e.Item.Selected = true;
                    }
                }
            }
        }
      
    }
}


