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
    public partial class EditAllViews : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if ("save" == Request["act"])
            {
                Save();
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
                        PageMsg.Name += ".";
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
        private void BindPage()
        {


            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            var ss = CurrentProvider.Analyzer;
            var dt = ss.AllViews();


            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var ListTable = dal.GetTables(CurrentProviderSet.Id);

            dt.Columns.Add(new System.Data.DataColumn() { Caption = "GlobalName", ColumnName = "GlobalName", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Summary", ColumnName = "Summary", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Remarks", ColumnName = "Remarks", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ObjectId", ColumnName = "ObjectId", DataType = typeof(string) });




            if (null != ListTable && 0 != ListTable.Count)
            {
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
                }
            }



            GridView1.DataSource = dt;

            GridView1.DataBind();

        }
    }
}