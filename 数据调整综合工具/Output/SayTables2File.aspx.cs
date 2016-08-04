using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Util.JSON;
using XP.Util.TextFile;


namespace 数据调整综合工具.Output
{
    public partial class SayTables2File : HasProviderBase
    {
        private string _Dir = "OutFiles";


        private string _FilenameTm = "{TM:ObjectName}.txt";



        private TemplateT _Template;
         

        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == _Template)
            {

                GetTM();
            }



            SayFiles();


        }


        protected void BindPage()
        {



        }


        protected void GetTM()
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
            _Template = Model;
        }


        protected void SayFiles()
        {

            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            var ss = CurrentProvider.Analyzer;

            var dt = ss.AllTables();


            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var ListTable = dal.GetTables(CurrentProviderSet.Id);

            if (!XP.Util.WebUtils.PathUtil.ExistDir(_Dir,true))
            {
                return;
            }
                       if (!String.IsNullOrEmpty(_Template.FileNameTemplate))
                {
                    _FilenameTm = _Template.FileNameTemplate;
                }
     string DirPath = XP.Util.WebUtils.PathUtil.GetRootPath() + "\\" + _Dir;
            foreach (DataRow row in dt.Rows)
            {
                string ObjectName = row["name"].ToString();
                string CompactName = GetCompactName(ObjectName);
                string FilePath = _FilenameTm;
                FilePath = DirPath + "\\" + _FilenameTm;
                FilePath = FilePath.Replace("{TM:CompactName}", CompactName);
                FilePath = FilePath.Replace("{TM:ObjectName}", ObjectName);


                string Cot = _Template.Cot;
                Cot = Cot.Replace("{TM:CompactName}", CompactName);
                Cot = Cot.Replace("{TM:ObjectName}", ObjectName);


                BaseWriter.Write2File(Cot, FilePath);


            }


        }


        private string GetCompactName(string input)
        {
            string Output = input;
            if (String.IsNullOrEmpty(input))
                return input;

            if (2 < Output.Length)
            {
                if ('_' == Output[1])
                {
                    Output = Output.Substring(2);
                }
            }
            Output = Output.Replace("_", "");
            return Output;
        }

    }
}