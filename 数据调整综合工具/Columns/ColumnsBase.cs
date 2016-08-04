using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XP.DB.DbEntity;
using XP.DB.ProviderManage;
using XP.Util.WebUtils;

namespace 数据调整综合工具.Columns
{
    public class ColumnsBase : HasProviderBase
    {
        public string TableName { get; set; }

        public DbObjectT TableInfo { get; set; }


        protected override void Page_PreLoad(object sender, EventArgs e)
        {
            base.Page_PreLoad(sender, e);
            _InitPage();

        }

        protected void _InitPage()
        {

            TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                SayError("需要指定一个表名。", "../index.aspx");
            }

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);

            dal = null;
            if (null == TableInfo)
            {
                SayError("对不起这个表不存在", "../index.aspx");
            }
        }

    }
}