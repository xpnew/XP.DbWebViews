using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.ProviderManage;
using XP.Util.WebUtils;

namespace 数据调整综合工具
{
    public partial class ShowColumns : HasProviderBase
    {
        public DbObjectT TableInfo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            Init();
            BindPage();
            BindPage_Original();
        }

        protected void Init()
        {

            string TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                SayError("需要指定一个表名。", "ShowAllTable.aspx");
            }

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);
            dal = null;

        }

        private void BindPage()
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
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "主键", ColumnName = "主键", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "ColumnType", ColumnName = "ColumnType", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "允空", ColumnName = "允空", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "默认值", ColumnName = "默认值", DataType = typeof(string) });
            dt.Columns.Add(new System.Data.DataColumn() { Caption = "合成说明", ColumnName = "合成说明", DataType = typeof(string) });

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

                if (ColumnItem.IsPk)
                {
                    NewRow["主键"] = "@";
                }
                if (ColumnItem.IsNullable)
                {
                    NewRow["允空"] = "Y";
                }
                NewRow["合成说明"] = ColumnItem.GlobalName;
                string DbDefault = ColumnItem.DefaultRules;
                if (!String.IsNullOrEmpty(DbDefault))
                {
                    if (DbDefault.StartsWith("((") && DbDefault.EndsWith("))"))
                    {
                        DbDefault = DbDefault.Replace("((", "").Replace("))", "");
                    }
                }
                NewRow["默认值"] = DbDefault;

                var SameNameList = ListTable.Where(m => m.ObjectName == ColumnItem.ColumnName);
                if (null != SameNameList && SameNameList.Any())
                {
                    var ExistCol = SameNameList.First();
                    NewRow["GlobalName"] = ExistCol.GlobalName;
                    NewRow["Summary"] = ExistCol.Summary;
                    NewRow["Remarks"] = ExistCol.Remarks;
                    NewRow["ObjectId"] = ExistCol.Id;
                    string IngertDescription = ExistCol.GlobalName;

                    if(!String.IsNullOrEmpty(ExistCol.Summary))
                    {
                        IngertDescription += " \n" + ExistCol.Summary;
                    }   
                    if(!String.IsNullOrEmpty(ExistCol.Remarks))
                    {
                        IngertDescription += " \n" + ExistCol.Remarks;
                    }

                    NewRow["合成说明"] = IngertDescription;
                }
                dt.Rows.Add(NewRow);
            }


            GridView2.DataSource = dt;
            GridView2.DataBind();         
            GridView3.DataSource = dt;
            GridView3.DataBind();

        }

        private void BindPage_Original()
        {

            var Provider = GetProvider();

            var ss = Provider.Analyzer;

            string TableName = RequestUtil.FindString("tablename");
            var List = ss.GetColumn(TableName);





            GridView1.DataSource = List;

            GridView1.DataBind();

        }

    }
}