using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.Json;

namespace 数据调整综合工具.Tables
{
    public partial class AddColumn : TablesBass
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (null != Request["act"])
            {
                if ("save" == Request["act"])
                {
                    Save();


                }
            }


            var dal = new ProviderInfoDAL(this.SiteProvider);

            var list = dal.GetAll();
        }



        protected void TestAjax()
        {

            SayJson("完成！");


        }


        protected void Save()
        {

            string TableName = Request["tablename"];
            string ColumnName = Request["col_name"];
            string ColumnDefinedString = Request["col_defined"];
            string FillValue = Request["col_fill"];

            string ProvidersRequest = Request["Providers"];

            if (String.IsNullOrEmpty(TableName))
            {
                JsonErr("表名 不能为空！   ");
                return;
            }

            if (String.IsNullOrEmpty(ColumnName))
            {
                JsonErr("新列名 不能为空！   ");
                return;
            }

            if (String.IsNullOrEmpty(ColumnDefinedString))
            {
                JsonErr("列定义 不能为空！   ");
                return;
            }
            if (String.IsNullOrEmpty(ProvidersRequest))
            {
                JsonErr("必须指定操作的目标 不能为空！   ");
                return;
            }
            var Providers = JsonHelper.Deserialize<List<ProviderT>>(ProvidersRequest);
            if (null == Providers || 0 == Providers.Count)
            {
                JsonErr("必须指定操作的目标 不能为空！   ");
                return;
            }

            string AddColumnSql = @"
ALTER TABLE [{TM:TableName}] 
ADD  [{TM:ColumnName}] {TM:ColumnDefinedString};
";
            string FillColumnSql = @"UPDATE [{TM:TableName}] SET  [{TM:ColumnName}]={TM:FillValue}  WHERE  [{TM:ColumnName}] IS NULL";


            AddColumnSql = AddColumnSql.Replace("{TM:TableName}", TableName);
            AddColumnSql = AddColumnSql.Replace("{TM:ColumnName}", ColumnName);
            AddColumnSql = AddColumnSql.Replace("{TM:ColumnDefinedString}", ColumnDefinedString);



            FillColumnSql = FillColumnSql.Replace("{TM:TableName}", TableName);
            FillColumnSql = FillColumnSql.Replace("{TM:ColumnName}", ColumnName);
            FillColumnSql = FillColumnSql.Replace("{TM:FillValue} ", FillValue);

            PageMsg.Name = "_AddColumn";





            string ExistSql = "select * from information_schema.columns where [TABLE_NAME]='{TM:TableName}' AND [COLUMN_NAME]='{TM:ColumnName}'";
            ExistSql = ExistSql.Replace("{TM:TableName}", TableName);
            ExistSql = ExistSql.Replace("{TM:ColumnName}", ColumnName);

            var pvdal = new ProviderInfoDAL(this.SiteProvider);
            var ProviderList = pvdal.FindIdList(Providers.Select(m => m.Id).ToList());
            var BllList = new List<SchemaDAL>();
            PageMsg.AddLog("准备验证列存在。");
            foreach (var p in ProviderList)
            {

                var Provider = DbFactory.CreateProvider(p);

                var bll = new SchemaDAL(Provider);
                var Exist = bll.ExistColumn(TableName, ColumnName);

                if (0 == Exist)
                {
                    JsonErr("选定的数据库不支持某些技术接口（可能是因为access）。");
                    return;
                }
                if (0 < Exist)
                {
                    JsonErr("在数据库【" + p.AliasName + "】当中，指定的列已经存存。");
                    return;
                }
                BllList.Add(bll);
            }


            foreach (var bll in BllList)
            {
                PageMsg.AddLog("准备在库【" + bll.Provider.Conn.DataSource + "】生成列，代码是：" + AddColumnSql);
                bll.AddColumn(AddColumnSql);

                if (!String.IsNullOrEmpty(FillValue))
                {
                    PageMsg.AddLog("准备填充列数据，代码是：" + FillColumnSql);
                    bll.AddColumn(FillColumnSql);
                }
            }

            SayJson("完成！");
        }
    }
}