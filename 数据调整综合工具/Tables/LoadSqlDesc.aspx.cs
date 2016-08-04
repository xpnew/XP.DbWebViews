using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;
using XP.Comm;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.JSON;
using XP.Util.WebUtils;

namespace 数据调整综合工具.Tables
{
    public partial class LoadSqlDesc : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ("GetDesc" == Request["act"])
            {
                GetDesc();
                return;
            }

            if ("GetFieldsDetails" == Request["act"])
            {
                GetFieldsDetails();
                return;
            }
            if ("Save" == Request["act"])
            {
                Save();
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


            if (null != ListTable && 0 != ListTable.Count)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string TableName = row["name"].ToString();
                    var SameNameList = ListTable.Where(m => m.ObjectName == TableName);
                    if (null != SameNameList && SameNameList.Any())
                    {
                        row["GlobalName"] = SameNameList.First().GlobalName;
                    }
                }
            }



            GridView1.DataSource = dt;

            GridView1.DataBind();

        }

        /// <summary>
        /// 查询sql里面表的 MS_Description
        /// </summary>
        protected void GetDesc()
        {
            var TableName = Request["TableName"];
            if (null == TableName)
            {
                x.Say("null tabel: ");

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

            string sql_TM = @"select ds.value 
from sys.extended_properties ds  left join sysobjects  tbs  
on ds.major_id =  tbs.id

where ds.minor_id=0 and tbs.type='u' and tbs.name='{TM:TableName}'";

            string sql = $"select Count(1) from [{TableName}]";
            string sql_curr = sql_TM.Replace("{TM:TableName}", TableName);

            var r = CurrentProvider.SingleColumn(sql_curr);
            if (null != r && DBNull.Value != r)
            {
                PageMsg.Body = r.ToString();

                SayJsonOk("ok ");
                return;



            }

            JsonErr("ERROR");

        }
        /// <summary>
        /// 
        /// </summary>
        protected void GetFieldsDetails()
        {
            var TableName = Request["TableName"];
            if (null == TableName)
            {
                x.Say("null tabel: ");

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

            string sql = $@"SELECT objtype, objname, name, value  
FROM fn_listextendedproperty ('MS_Description', 'schema', 'dbo', 'table', '{TableName}', 'column', default) ";

            //返回结果 示例： 
            //objtype objname name value
            //COLUMN IsNotUpdate MS_Description 是否是不可以修改
            //COLUMN MailAttach  MS_Description 附件内容
            var dt = CurrentProvider.Select(sql);

            var lst = new List<ColumnDtoItem>();
            if (null == dt || 0 == dt.Rows.Count)
            {
                PageMsg.Status = false;
                PageMsg.StatusCode = 0;

                SayJson();

                return;
            }

            foreach (DataRow row in dt.Rows)
            {

                var NewItem = new ColumnDtoItem()
                {

                    ColumnName = row["objname"].ToString(),
                    GlobalName = row["value"].ToString(),

                };


                lst.Add(NewItem);
            }



            TruantDataMsg ReturnMsg = new TruantDataMsg()
            {
                DataInfo = lst
            };

            ReturnMsg.SetOk("");

            SayJson(ReturnMsg);
            return;

            //var r = CurrentProvider.SingleColumn(sql);
            //if (null != r && DBNull.Value != r)
            //{
            //    PageMsg.Body = r.ToString();

            //    SayJsonOk("ok ");
            //    return;



            //}
            //SayJsonOk("ok ");
            //return;

            //JsonErr("ERROR");

        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        protected void Save()
        {

            var MsgDict = RequestUtil.FindModel4Json<SaveRequestItem>();

            if (MsgDict == null && 0 == MsgDict.TableList.Count)
            {


                JsonErr("ERROR 参数不对！");

                return;
            }
            ModelRecognize<DbObjectT> ModelEngine = new ModelRecognize<DbObjectT>("Model");
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);
            TableName2IdDict = new Dictionary<string, int>();
            int TotalLine = MsgDict.TableList.Count;
            PageMsg.AddLog("总表格行数：" + TotalLine);
            int SuccessLine = 0;
            foreach (var tbgl in MsgDict.TableList)
            {

                var Exist = dal.GetOneBySql($" [ObjectName]='{tbgl.TableName}' ");

                if (null != Exist)
                {
                    TableName2IdDict.Add(Exist.ObjectName, Exist.Id);
                    continue;
                }

                var NewModel = new DbObjectT();

                NewModel.ProviderId = CurrentProviderSet.Id;
                NewModel.ObjectName = tbgl.TableName;
                NewModel.GlobalName = tbgl.GlobalName;
                NewModel.Remarks = "(中文名从数据库备注复制)";
                int Id = dal.InsertEntity(NewModel);
                NewModel.Id = Id;

                TableName2IdDict.Add(NewModel.ObjectName, NewModel.Id);

                SuccessLine++;
            }

            PageMsg.AddLog("已经完成表格行数：" + SuccessLine);
            TotalLine = MsgDict.TableList.Count;
            SuccessLine = 0;
            PageMsg.AddLog("总 字段 行数：" + TotalLine);
            foreach (var tbgl in MsgDict.FieldList)
            {
                var tbid =  GetTableObjId(tbgl.TableName);
                if (Constant.NotExistInt == tbid || Constant.ErrorInt == tbid) continue;


                var NewModel = new DbObjectT();

                NewModel.ProviderId = CurrentProviderSet.Id;
                NewModel.ObjectName = tbgl.ColumnName;
                NewModel.GlobalName = tbgl.GlobalName;
                NewModel.ParentId = tbid;
                NewModel.Remarks = "(中文名从数据库备注复制)";
                int Id = dal.InsertEntity(NewModel);
                NewModel.Id = Id;

                //TableName2IdDict.Add(NewModel.ObjectName, NewModel.Id);



            }

            PageMsg.AddLog("已经完成 字段 行数：" + SuccessLine);



            SayJsonOk("ok ");


        }

        protected int GetTableObjId(string tbname)
        {
            if (TableName2IdDict.Keys.Contains(tbname))
            {
                return TableName2IdDict[tbname];
            }
            string sql = $"select Id from DbObjectT where  [ProviderId]={CurrentProviderSet.Id}  AND [ParentId]=0  AND [ObjectName]='{tbname}'";
          string sqlWhere=   $" [ProviderId]={CurrentProviderSet.Id}  AND [ParentId]=0  AND [ObjectName]='{tbname}'";
            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

          
            var ExistDBModel = dal.GetOneBySql(sqlWhere);
            if (null != ExistDBModel)
            {
                int id = ExistDBModel.Id;
                TableName2IdDict.Add(tbname, id);
                return id;



            }
            return XP.Comm.Constant.NotExistInt;

        }
        protected Dictionary<string, int> TableName2IdDict { get; set; } = new Dictionary<string, int>();




        /// <summary>
        /// 保存接口使用的请求对象
        /// </summary>
        public class SaveRequestItem
        {

            public List<GlobalSaveItem> TableList { get; set; }
            public List<GlobalSaveItem> FieldList { get; set; }


        }
        public class GlobalSaveItem
        {

            public string GlobalName { get; set; }
            public string TableName { get; set; }

            public string ColumnName { get; set; }

        }

    }
}