using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.JSON;

namespace 数据调整综合工具
{
    public partial class EditTableInfo : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if ("save" == Request["act"])
            {
                Save();
                return;
            } 
            
            if ("loadone" == Request["act"])
            {
                LoadOne();
                return;
            }


            BindPage();
        }
        private void Save()
        {
            var Model = Request["Model"];

            ModelRecognize<DbObjectT> ModelEngine = new ModelRecognize<DbObjectT>("Model");
            var NewModel = ModelEngine.GetModel();


            if (0 < ModelEngine.FoundNamesCount)
            {

                NewModel.ProviderId = CurrentProviderSet.Id;
                DbObjectDAL dal = new DbObjectDAL(SiteProvider);

                if (0 == NewModel.Id)
                {
                    if (null != PageMsg.Name)
                    {
                        PageMsg.Name += "." ;
                    }
                    PageMsg.Name += "Insert";

                    int Id = dal.InsertEntity(NewModel);
                    NewModel.Id = Id;
                    PageMsg.Body = Id.ToString();
                }
                else
                {
                    if (null != PageMsg.Name)
                    {
                        PageMsg.Name += ".";
                    }
                    PageMsg.Name += "Update";

                    int Return = dal.UpdateModel(NewModel);
                }


                SayJson("OK");
                //XP.Util.Web.PageMsg.Write("OK");
            }

            JsonErr("ERROR");

        }

        protected void  LoadOne()
        {
            string TableName = Request["TableName"];
            string sql_TM = @"select ds.value 
from sys.extended_properties ds  left join sysobjects  tbs  
on ds.major_id =  tbs.id

where ds.minor_id=0 and tbs.type='u' and tbs.name='{TM:TableName}'";

            string sql_curr = sql_TM.Replace("{TM:TableName}", TableName);
            var ooo = CurrentProvider.SingleColumn(sql_curr);
            if (null != ooo && DBNull.Value != ooo)
            {

                SayJson( (string)ooo);
            }
            JsonErr("返回数据为空");
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
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Summary", ColumnName = "Summary", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Remarks", ColumnName = "Remarks", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ObjectId", ColumnName = "ObjectId", DataType = typeof(string) });



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
                    row["Summary"] = SameNameList.First().Summary;
                    row["Remarks"] = SameNameList.First().Remarks;
                    row["ObjectId"] = SameNameList.First().Id;
                }
                else
                {
                    string sql_curr =  sql_TM.Replace("{TM:TableName}", TableName);
                    var ooo = CurrentProvider.SingleColumn(sql_curr);
                    if (null != ooo && DBNull.Value !=  ooo )
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