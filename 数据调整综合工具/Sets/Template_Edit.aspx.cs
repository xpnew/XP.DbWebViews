using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 数据调整综合工具.Sets
{
    public partial class Template_Edit : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (null != Request["Id"])
                {
                    DetailsView1.DefaultMode = DetailsViewMode.Edit;

                }

            }
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {

            var ss = e.Values[""];

        }
    }
}