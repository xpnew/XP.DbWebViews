using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.WebUtils;

namespace 数据调整综合工具.Output
{
    public partial class SelectTM : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if ("go" == Request["act"])
            {
                Go();
                return;
            }

        }

        protected void Go()
        {
            var dal = new XP.DBTools.BLL.TemplateBLL();

            int Id = XP.Util.WebUtils.RequestUtil.RequestInt("tmid");

            if (0 > Id)
            {
                Id = XP.Util.WebUtils.RequestUtil.RequestInt("id");
            }

            if (Id == XP.Util.WebUtils.RequestUtil.ErrorInputInt)
            {
                SayError("需要选择一个模板！", "SelectTM.aspx");

            }

            if (Id == XP.Util.WebUtils.RequestUtil.ErrorInputInt)
            {

                Alert00Close("模板参数不对！");
            }
            var Model = dal.GetItemById(Id);

            if (null == Model)
            {
                SayError("需要选择一个模板(没有找到)！", "SelectTM.aspx");

            }

            string GoUrl = "";

            if (Model.OutType == TemplateOutType.PageTextArea)
            {
                GoUrl = "SayTables2TextArea.aspx";

            }
            GoUrl += "?tmid=" + Id;

            Response.Redirect(GoUrl);


        }

        public string GetUrl(object id)
        {
            string TableName = RequestUtil.FindString("tablename");

            string Result = $"?tmid={id}";
            if (String.IsNullOrEmpty(TableName))
            {
               

            }
            else
            {
                Result += "&tablename=" + TableName;
            }
            return Result;
        }

        //public string BuildUrl(System.Data.DataRowView row)
        //{

        //    return null;
        //}
        public string BuildUrl(TemplateT row)
        {
            string TableName = RequestUtil.FindString("tablename");
            string Result = null;
            if (String.IsNullOrEmpty(TableName))
            {
                return null;
            }
            else
            {
                Result = $"ViewTM.aspx?tmid={row.Id}&tablename={TableName}" ;
            }
            return Result;
        }
    }
}