using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;
using XP.DB.DbEntity;
using XP.DB.Future;

namespace 数据调整综合工具.Tables
{
    public partial class RowsCount : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ("GetRowCount" == Request["act"])
            {
                GetRowCount();
                return;
            }

            BindPage();
        }

        private void BindPage()
        {


            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            var ss = CurrentProvider.Analyzer;
            var dt = ss.AllTables();


            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var ListTable = dal.GetTables(CurrentProviderSet.Id);

            dt.Columns.Add(new System.Data.DataColumn() { Caption = "GlobalName", ColumnName = "GlobalName", DataType = typeof(string) });

            string sql_TM = @"select ds.value 
from sys.extended_properties ds  left join sysobjects  tbs  
on ds.major_id =  tbs.id

where ds.minor_id=0 and tbs.type='u' and tbs.name='{TM:TableName}'";

            //if (null != ListTable && 0 != ListTable.Count)
            //{
            //}
            foreach (DataRow row in dt.Rows)
            {
                string TableName = row["name"].ToString();
                var SameNameList = ListTable.Where(m => m.ObjectName == TableName);
                if (null != SameNameList && SameNameList.Any())
                {
                    row["GlobalName"] = SameNameList.First().GlobalName;
                }
                else
                {
                    string sql_curr = sql_TM.Replace("{TM:TableName}", TableName);
                    var ooo = CurrentProvider.SingleColumn(sql_curr);
                    if (null != ooo && DBNull.Value != ooo)
                    {
                        row["GlobalName"] = ooo.ToString();
                    }
                }
            }



            GridView1.DataSource = dt;

            GridView1.DataBind();

        }






        protected void GetRowCount()
        {
            var TableName = Request["TableName"];
            if (null == TableName)
            {
                x.Say("null tabel: " );

                SayJsonOk("hack ", 5);

                return;

            }

            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            //if ( 0 < TableName.Length)
            //{
            //    x.Say("will tabel: " + TableName);

            //    Thread.Sleep(5 * 1000);
            //    x.Say("Done  tabel: " + TableName);

            //    SayJsonOk("hack ", 0);

            //    return;
            //}


            string sql = $"select Count(1) from [{TableName}]";

            var r = CurrentProvider.SingleColumn(sql);
            if (null != r && DBNull.Value != r)
            {
                if (vbs.IsInt(r))
                {
                    int Count = int.Parse(r.ToString());

                    //if (Count > 1000)
                    //{
                    //    x.Say("大表格： " + TableName + "\n" + Count);
                    //}


                    SayJsonOk("ok ", Count);
                    return;
                }




            }

            JsonErr("ERROR");

        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}   