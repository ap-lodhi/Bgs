using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BGS
{
    public partial class SectorMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BrokerPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
               // gvSectorName.PageIndex = e.NewPageIndex;
            }
            catch (Exception ex)
            {
                BGS.Error.EventLogProcess(ex);
            }
        }
    }
}