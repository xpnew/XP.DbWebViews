using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.ProviderManage;

namespace 数据调整综合工具.JsSelector
{
    public partial class SelectProvider : SelectPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

           // DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            var dal = new ProviderDAL(SiteProvider);

            var dt = dal.GetAll();

            Repeater1.DataSource = dt;

            Repeater1.DataBind();
        }
    }
}