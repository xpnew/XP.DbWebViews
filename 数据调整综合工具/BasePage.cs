using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XP.Comm;
using XP.Comm.Msgs;
using XP.DB.Comm;
using XP.DB.Future;
using XP.Util.Json;
using XP.Util.WebUtils;

namespace 数据调整综合工具
{
    public class BasePage : System.Web.UI.Page
    {
        protected WebMsg _PageMsg;

        public WebMsg PageMsg
        {
            get
            {
                if (null == _PageMsg)
                    _PageMsg = new WebMsg() { StatusCode = 1 };
                return _PageMsg;
            }
            set
            {
                _PageMsg = value;
            }
        }


        //public IProvider SiteProvider
        //{

        //    get
        //    {
        //        string ConnStr = XP.Util.Config.ConnStr;

        //        var Provider = new XP.DB.Future.OleDb.OleProvider(ConnStr);

        //        return Provider;

        //    }
        //}

        protected virtual void Page_PreLoad(object sender, EventArgs e)
        {


            string PageName = Request.Url.LocalPath;

            string ActName = Request["act"];


            if (String.IsNullOrEmpty(ActName))
            {
                ActName = Request["action"];
            }

            if (String.IsNullOrEmpty(ActName))
            {
                PageMsg.Name = PageName;
            }
            else
            {
                PageMsg.Name = PageName + "." + ActName;
            }

        }

        protected void SayError(string msg)
        {

            XP.Util.WebUtils.PageUtil.xpnewAlert(msg);
        }

        protected void SayError(string msg, string url)
        {
            XP.Util.WebUtils.PageUtil.xpnewAlert(msg, url);

        }


        protected void Say(string msg)
        {

            XP.Util.WebUtils.PageUtil.xpnewAlert(msg);
        }
        /// <summary>
        /// 弹出提示并且关闭窗口
        /// </summary>
        /// <param PropertyTypeName="msg"></param>
        protected void Alert00Close(string msg)
        {
            XP.Util.WebUtils.PageUtil.xpnewClose(msg);

        }


        protected void SayJson(string title, string body)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            PageMsg.Body = body;
            SayJson();

        }
        protected void SayJson(string title)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            SayJson();
        }
        protected void SayJsonOk(string title)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            SayJson();
        }

        protected void SayJsonOk(string title, int code)
        {
            PageMsg.StatusCode = 1;
            PageMsg.Title = title;
            var msg = MsgBase.CloneMsg<TruantDataMsg>(PageMsg);
            msg.DataInfo = code;

            SayJson(msg);
        }
        protected void SayJson()
        {
            PageUtil.Json(PageMsg.ToString());
        }

        protected void SayJson(CommMsg msg)
        {

            PageUtil.Json(JsonHelper.ToJson(msg));

        }
        protected void JsonErr(string err)
        {
            var Msg = new WebMsg();
            Msg.StatusCode = -1;
            Msg.Title = err;
            PageUtil.Json(Msg.ToString());
        }
    }
}