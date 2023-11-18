using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BGS
{
    public partial class weightMaster : System.Web.UI.Page
    {
        #region Properties

        //SRRMS_PRODEntities srrms_entity = new SRRMS_PRODEntities();
        BGSEntities bgs_entity = new BGSEntities();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.PopulateWeightMaster();
            }
        }

        protected void UpdateWeightMaster(object sender, EventArgs e)
        {
            string weightId = (sender as Button).CommandArgument;
            foreach (GridViewRow row in gvWeightMater.Rows)
            {
                string weight = row.Cells[3].Text;
                string Empid = row.FindControl("hfEmpId").ToString();
               // bgs_entity.BGS_WeightMaster();
            }
           
        }

        #region Private Methods

        private void PopulateWeightMaster()
        {
            gvWeightMater.DataSource = bgs_entity.BGS_GetWeightMaster();
            gvWeightMater.DataBind();
        }

        #endregion

    }
}