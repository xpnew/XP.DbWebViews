using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.Util.WebUtils;

namespace 数据调整综合工具
{
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if ("save" == Request["act"])
            {
                Save();

            }
        }

        private void Save()
        {

            if (!String.IsNullOrEmpty(Request["Id"]))
            {
                int Id = int.Parse(Request["Id"]);

                string sql = "select * from ProviderT where Id=" + Id;

                string ConnStr = XP.Util.Conf.ConnStr;

                var Provider = new XP.DB.Future.OleDb.OleProvider(ConnStr);
                var dt = Provider.Select(sql);

                if (null == dt)
                {
                    XP.Util.WebUtils.PageUtil.xpnewAlert("没找到数据");
                }
                

                var Copyer = new XP.Util.DataTable2Entity<XP.DB.DbEntity.ProviderInfo>();

                ProviderInfo CurrentProvider = Copyer.CopyData(dt);

                if (null != CurrentProvider)
                {
                    Session["ProviderId"] = Id;
                    Session["CurrentProvider"] = CurrentProvider;

                    string UpdateSql = "UPDATE [ProviderT] SET [IsCurrent]=0";

                    Provider.ExecuteSql(UpdateSql);
                    UpdateSql = "UPDATE [ProviderT] SET [IsCurrent]=1  WHERE [Id]=" + Id;
                    Provider.ExecuteSql(UpdateSql);

                    PageUtil.xpnewAlert("您已经选择一种数据库：" + CurrentProvider.AliasName + "(" + CurrentProvider.DbTypeName + "）"  + ",即将跳转！", "ShowAllTable.aspx");
                    return;
                }
            }

            XP.Util.WebUtils.PageUtil.xpnewAlert("出现了错误，无法保存！");
        }

    }
}