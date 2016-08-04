using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Data;



namespace 数据调整综合工具
{
    public partial class 填充CreateTime : System.Web.UI.Page
    {

        private string dbname = "qy179";


        private SqlConnection _Conn;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitPage();
        }


        private void InitPage()
        {

            string ConnStr = "server=192.168.1.179;database=FreshGoodHQ;uid=sa;pwd=zxb";
            _Conn = new SqlConnection(ConnStr);

            _Conn.Open();


            SqlCommand cmd1 = new SqlCommand("select * from  FJ_Goods  order by [GoodCode]", _Conn);

            SqlDataAdapter ad = new SqlDataAdapter(cmd1);

            DataTable dt = new DataTable();

            ad.Fill(dt);


            _Conn.Close();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];
                _Conn.Open();
                sb.Append("<li>");
                string sql = "update [FJ_Goods] set [CreateTime] = '" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff") + "' where [KeyId]='" + row["KeyId"] + "'";

                var cmd2 = new SqlCommand(sql, _Conn);

                int Return = cmd2.ExecuteNonQuery();
                Trace.IsEnabled = true;
                Trace.Write(Return.ToString());

                sb.Append(sql);
                sb.Append("</li>");
                    
                _Conn.Close();

                System.Threading.Thread.Sleep(17);

            }





            Response.Write(sb.ToString());


        }
    }
}