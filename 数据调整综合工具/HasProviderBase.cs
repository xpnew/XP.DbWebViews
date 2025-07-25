using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.Future;

namespace 数据调整综合工具
{
    public class HasProviderBase : BasePage
    {

        private ProviderInfo _CurrentProviderSet;
        public ProviderInfo CurrentProviderSet
        {

            get
            {
                if (null == _CurrentProviderSet)
                {
                    _CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;
                }
                return _CurrentProviderSet;
            }
            set
            {
                _CurrentProviderSet = value;
            }
        }

        private IProvider _CurrentProvider;

        public IProvider CurrentProvider
        {
            get {
                if (null == _CurrentProvider)
                {
                    _CurrentProvider = GetProvider();
                }
                
                return _CurrentProvider; }
            set { _CurrentProvider = value; }
        }

        public IProvider SiteProvider
        {
            get
            {

                string ConnStr = XP.Util.Conf.ConnStr;

                var Provider = new XP.DB.Future.OleDb.OleProvider(ConnStr);
                return Provider;
            }
        }

        protected override void Page_PreLoad(object sender, EventArgs e)
        {
            base.Page_PreLoad(sender, e);
            if (null == Session["ProviderId"] || null == Session["CurrentProvider"])
            {

                if (!GetDBCurrent())
                    SayError("需要选择一个数据库", "default.aspx");
            }
        }

        public IProvider GetProvider()
        {

            ProviderInfo CurrentProvider = Session["CurrentProvider"] as ProviderInfo;

            var Provider = DbFactory.CreateProvider(CurrentProvider);


            return Provider;
        }

        private bool GetDBCurrent()
        {
            string sql = "select * from [ProviderT] WHERE [IsCurrent]=1";

            string ConnStr = XP.Util.Conf.ConnStr;

            var Provider = new XP.DB.Future.OleDb.OleProvider(ConnStr);
            var dt = Provider.Select(sql);

            if (null == dt)
            {
                XP.Util.WebUtils.PageUtil.xpnewAlert("没找到数据");
            }

            var Copyer = new XP.Util.DataTable2Entity<ProviderInfo>();

            ProviderInfo CurrentProvider = Copyer.CopyData(dt);

            if (null != CurrentProvider)
            {
                Session["ProviderId"] = CurrentProvider.Id;
                Session["CurrentProvider"] = CurrentProvider;

            }
            else
            {
                return false;
            }


            return true;
        }

    }
}