using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;

using System.Data;
using XP.DB.ProviderManage;
using System.Threading.Tasks;
using XP.Util.Threadings;

namespace 数据调整综合工具
{
    public partial class ShowAllTable : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //测试提交 

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
                    string GlobalName = SameNameList.First().GlobalName;
                    if (!String.IsNullOrEmpty(GlobalName))
                        row["GlobalName"] = SameNameList.First().GlobalName;
                    else
                    {
                        string sql_curr = sql_TM.Replace("{TM:TableName}", TableName);
                        var ooo = CurrentProvider.SingleColumn(sql_curr);
                        if (null != ooo && DBNull.Value != ooo)
                        {
                            GlobalName = ooo.ToString();
                            row["GlobalName"] = GlobalName;

                            AsyncTask.BuildBGAsync(() =>
                            {
                                UpdateTable(SameNameList.First().Id, TableName, GlobalName);
                            });

                        }
                    }
                }
                else
                {
                    string sql_curr = sql_TM.Replace("{TM:TableName}", TableName);
                    var ooo = CurrentProvider.SingleColumn(sql_curr);
                    if (null != ooo && DBNull.Value != ooo)
                    {
                        string GlobalName = ooo.ToString();
                        row["GlobalName"] = GlobalName;

                        AsyncTask.BuildBGAsync(() =>
                        {
                            UpdateTable(-999999, TableName, GlobalName);
                        });

                    }
                }
            }



            GridView1.DataSource = dt;

            GridView1.DataBind();

        }


        protected void UpdateTable(int tableId, string tableName, string tableGlobal)
        {
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            var Model = dal.GetItemById(tableId);


            if (null == Model)
            {
                var NewModel = new DbObjectT();
                NewModel.ProviderId = CurrentProviderSet.Id;
                NewModel.ObjectName = tableName;
                NewModel.GlobalName = tableGlobal;
                NewModel.Remarks = "(从数据库说明复制)";
                int Id = dal.InsertEntity(NewModel);

                return;
            }
            Model.GlobalName = tableGlobal;

            string sql = $"UPDATE [DbObjectT] SET [GlobalName]='{tableGlobal}'  WHERE  [Id]={tableId}";


            int Return = dal.ExcuteSql(sql);



        }


    }
}