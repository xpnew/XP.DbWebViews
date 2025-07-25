using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace 数据调整综合工具.Sets
{
    public partial class Template : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var row = e.Row;

            var Cells = row.Cells;

            foreach (TableCell cell in Cells)
            {
                string Text = cell.Text;

                if (Text != null && 120 < Text.Length)
                {

                    cell.Text = Text.Cut(120, "......");
                }

            }
        }
    }
}