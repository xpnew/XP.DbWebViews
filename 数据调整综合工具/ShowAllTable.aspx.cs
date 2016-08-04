using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;

using System.Data;

namespace 数据调整综合工具
{
    public partial class ShowAllTable : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {


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


    }
}