using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 数据调整综合工具.Tables
{
    public partial class Index : TablesBass
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            TableName = Request["TableName"];




        }
    }
}