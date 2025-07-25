using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

namespace 数据调整综合工具.Columns
{
    public partial class ExportName : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //TestAutoDtName();
        }

        private void TestAutoDtName()
        {
            var ds = new DataSet();

            for (int i = 0; i < 3; i++)
            {
                var dt = new DataTable();
                dt.TableName = "Mytable";//A DataTable named 'Mytable' already belongs to this DataSet.
                dt.TableName = "";
                ds.Tables.Add(dt);

            }

            foreach (var dt in ds.Tables)
            {

                Response.Write(dt.ToString() + "<br />");
            }
        }
    }
}