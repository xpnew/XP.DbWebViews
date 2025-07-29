using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.Comm;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.WebUtils;

namespace 数据调整综合工具.JsSelector
{
    public partial class SelectTable : SelectPageBase
    {
        public DbObjectT TableInfo { get; set; }


        public DataTable ColDt { get; set; }    

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPage();
            


            
            GetColData();


            if ("ViewCols" == Request["act"])
            {
                ViewCols();
                return;
            }



            BindPage();
        }





        protected void InitPage()
        {

            string TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                SayError("需要指定一个表名。", "EditTableInfo.aspx");
            }

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);
            dal = null;



        }

        private void GetColData()
        {

            var Provider = GetProvider();

            var ss = Provider.Analyzer;



            var List = ss.GetColumn(TableInfo.ObjectName);

            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var dt = new DataTable();

            if (null == TableInfo)
            {
                SayError("无法找到这个表的数据。", "EditTableInfo.aspx");
            }

            var ListTable = dal.GetMembers(CurrentProviderSet.Id, TableInfo.Id);

            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ColumnName", ColumnName = "ColumnName", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Description", ColumnName = "Description", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ColumnType", ColumnName = "ColumnType", DataType = typeof(string) });



            dt.Columns.Add(new System.Data.DataColumn() { Caption = "GlobalName", ColumnName = "GlobalName", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Summary", ColumnName = "Summary", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "Remarks", ColumnName = "Remarks", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ObjectId", ColumnName = "ObjectId", DataType = typeof(string) });

            foreach (var ColumnItem in List)
            {
                DataRow NewRow = dt.NewRow();

                NewRow["ColumnName"] = ColumnItem.ColumnName;
                NewRow["Description"] = ColumnItem.GlobalName;
                NewRow["ColumnType"] = ColumnItem.ColumnTypeVSLength;

                var SameNameList = ListTable.Where(m => m.ObjectName == ColumnItem.ColumnName);
                if (null != SameNameList && SameNameList.Any())
                {
                    NewRow["GlobalName"] = SameNameList.First().GlobalName;
                    NewRow["Summary"] = SameNameList.First().Summary;
                    NewRow["Remarks"] = SameNameList.First().Remarks;
                    NewRow["ObjectId"] = SameNameList.First().Id;
                }
                dt.Rows.Add(NewRow);
            }

            ColDt = dt;

        }


        protected void ViewCols()
        {
            if (null != ColDt)
            {
                TruantDataMsg ReturnMsg = new TruantDataMsg() {
                    DataInfo = ColDt,                    
                };
                ReturnMsg.SetOk();
                SayJson(ReturnMsg);
                return;
            }

            JsonErr("没找到表数据");

        }
        protected void BindPage()
        {
            GridView1.DataSource = ColDt;
            GridView2.DataSource = ColDt;
            GridView1.DataBind();
            GridView2.DataBind();
            GetAllTable();
        }

        protected void GetAllTable()
        {
            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            var ss = CurrentProvider.Analyzer;
            var dt = ss.AllTables();


            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var ListTable = dal.GetTables(CurrentProviderSet.Id);

            Repeater1.DataSource = ListTable;
            Repeater1.DataBind();
        }

    }
}