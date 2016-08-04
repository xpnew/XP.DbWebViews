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
using XP.IO.ExcelUtil;
using XP.Util.JSON;
using XP.Util.WebUtils;


namespace 数据调整综合工具
{
    public partial class EditColumns : HasProviderBase
    {

        public DbObjectT TableInfo { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {

            InitPage();
            if ("save" == Request["act"])
            {
                Save();
                return;
            }

            if ("import" == Request["act"])
            {
                Import();
                return;
            }
            if ("clearall" == Request["act"])
            {
                ClearAllColumns();
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


        protected void ClearAllColumns()
        {

            int ReturnValue = DAL_ClearColumns();
            if (null != PageMsg.Name)
            {
                PageMsg.Name += ".";
            }
            PageMsg.Name += TableInfo.ObjectName;
            if (0 <= ReturnValue)
            {
                SayJson("OK");
            }
            else
            {
                JsonErr("ERROR");
            }


        }

        private int DAL_ClearColumns()
        {
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            int ReturnValue = 0;

            ReturnValue = dal.ClearColumnsByTableName(CurrentProviderSet.Id, TableInfo.ObjectName);
            return ReturnValue;
        }


        protected void Import()
        {
            if (0 == Request.Files.Count)
            {

                SayError("请选择一个上传文件！");
            }
            HttpPostedFile file = Request.Files[0] as HttpPostedFile;

            if (!(file.FileName.ToLower().Contains(".xls") || file.FileName.ToLower().Contains(".xlsx")))
                throw new Exception("上传文件必须是excel");
            var pu = new AutoDatePath("/Upload");
            string WebPath = pu.MakeFilePath(file.FileName);

            string ServerPath = Server.MapPath(WebPath);

            file.SaveAs(ServerPath);
            var er1 = new ExcelReader(ServerPath);
            er1.GetData();

            if (er1.ResultInfo.Success)
            {

                var dtFinder = er1.ResultTables.Where(m => m.TableName == TableInfo.ObjectName);
                if (dtFinder.Any())
                {
                    var dt = dtFinder.First();
                    SaveTable(dt,true);
                }
                XP.Util.WebUtils.PageUtil.xpnewAlert("提交成功",true);
            }
            else
            {
                SayError("发现了多条错误：" + er1.ResultInfo.ErrorList.Count);
            }
        }


        protected void SaveTable(DataTable dt, bool foreClean)
        {
            int EngNameIndex = -1;

            int ChsNameIndex = -1;

            var Col1 = dt.Columns["Name"];
            var Col2 = dt.Columns["Code"];

            if (null == Col1)
            {
                Col1 = dt.Columns["名称"];
            }
            if (null == Col1)
            {
                Col1 = dt.Columns["汉译"];
            }
            if (null != Col1)
            {
                ChsNameIndex = Col1.Ordinal;
            }
            if (null == Col2)
            {
                Col2 = dt.Columns["代码"];
            }

            if (null == Col2)
            {
                Col2 = dt.Columns["字段名"];
            }

            if (null == Col2)
            {
                Col2 = dt.Columns["字段"];
            }
            if (null != Col2)
            {
                EngNameIndex = Col2.Ordinal;
            }


            if (0 > ChsNameIndex || 0 > EngNameIndex)
            {
                SayError("无法识别的数据");
                return;
            }

            if (foreClean)
            {
                DAL_ClearColumns();
            }
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            int ReturnValue = 0;


            foreach (DataRow row in dt.Rows)
            {
                string EngName = row[EngNameIndex].ToString();
                string ChsName = row[ChsNameIndex].ToString();

                var NewItem = new DbObjectT()
                {
                    ProviderId = this.CurrentProviderSet.Id,
                    ParentId = this.TableInfo.Id,
                    ObjectName = EngName,
                    GlobalName = ChsName,

                };
              ReturnValue=  dal.InsertModel(NewItem);
            }


        }


        protected int FindColumnIndex(DataTable dt, List<string> colNames)
        {
            int Result = -1;

            return Result;


        }

        private void Save()
        {
            var Model = Request["Model"];

            ModelRecognize<DbObjectT> ModelEngine = new ModelRecognize<DbObjectT>("Model");
            var NewModel = ModelEngine.GetModel();


            if (0 < ModelEngine.FoundNamesCount)
            {

                DbObjectDAL dal = new DbObjectDAL(SiteProvider);
                NewModel.ProviderId = CurrentProviderSet.Id;
                NewModel.ParentId = TableInfo.Id;


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


            GridView1.DataSource = dt;

            GridView1.DataBind();

        }
    }
}