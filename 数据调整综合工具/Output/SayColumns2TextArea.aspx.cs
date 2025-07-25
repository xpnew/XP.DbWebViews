using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.DB.ProviderManage;
using XP.Text.Tags.TMTags;
using XP.Util;
using XP.Util.JSON;
using XP.Util.TextFile;
using XP.Util.TypeCache;
using XP.Util.WebUtils;

namespace 数据调整综合工具.Output
{
    public partial class SayColumns2TextArea : HasProviderBase
    {

        private TemplateT _Template;

        public DbObjectT TableInfo { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            InitPage();
            if (null == _Template)
            {

                GetTM();
            }
            SayResult();
        }
        protected void InitPage()
        {

            string TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                SayError("需要指定一个表名。", "ShowAllTable.aspx");
                return;
                // TableInfo = new DbObjectT();
            }

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);

            dal = null;



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

        protected void SayResult()
        {
            var SubTm = FindLoops(_Template.Cot);

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);
            var ColList = dal.GetMembers(CurrentProviderSet.Id, TableInfo.Id);
            StringBuilder sb = new StringBuilder();
            var Provider = GetProvider();

            var ss = Provider.Analyzer;

            var List = ss.GetColumn(TableInfo.ObjectName);

            var GlobalList = from u in ColList
                             join m in List
                             on u.ObjectName equals m.ColumnName

                             select new { u, m };
            if (GlobalList.Any())
            {
                var ll = GlobalList.ToList();
                ll.ForEach(s => s.m.GlobalName = s.u.GlobalName);
            }


            EntityTypesCacheItem TypeCache;

            var CacheManage = EntityTypesCache.CreateInstance();

            var InnerType = typeof(ColumnDtoItem);

            TypeCache = CacheManage.GetItem(InnerType);

            var _PropertyValueGeter = new DynamicMethod<ColumnDtoItem>();


            foreach (var tm in SubTm)
            {
                sb.Append("\n=============");
                sb.Append(tm.TagName);
                sb.Append("==============\n");

                int i = 0;
                foreach (var col in List)
                {
                    string NewLineStr = tm.TagContent;
                    foreach (var Pname in TypeCache.PropertyNames)
                    {
                        string ItemStr = "";
                        var val = _PropertyValueGeter.GetValue(col, Pname);
                        if (null != val)
                        {
                            ItemStr = val.ToString();
                        }
                        //NewLineStr = NewLineStr.Replace("{TM:" + Pname + "}", ItemStr);
                        NewLineStr = ReplaceTmItem(NewLineStr, Pname, ItemStr);
                    }
                    //{TM:LoopNum}
                    NewLineStr = NewLineStr.Replace("{TM:LoopNum}", i.ToString());
                    sb.Append(NewLineStr);
                    i++;

                }
            }
            TextBox_Result.Text = sb.ToString();
        }

        /// <summary>
        /// 替换具体的数据项（新增大小写转换）
        /// </summary>
        /// <param name="input">需要识别的的数据</param>
        /// <param name="name">TM格式模板的项目名称</param>
        /// <param name="value">具体需要替换的数据</param>
        /// <returns></returns>
        private string ReplaceTmItem(string input, string name, string value)
        {
            string Result = input;
            string FirstCharLower, FirstCharUpper;
            if (String.IsNullOrEmpty(name) || String.IsNullOrEmpty(input))
            {
                return Result;
            }
            if (String.IsNullOrEmpty(value))
            {
                value = String.Empty;
                FirstCharLower = value;
                FirstCharUpper = value;
            }
            else
            {
                string FirstCharString = value[0].ToString();
                char FirstChar = value[0];

                char[] Value2Chars = value.ToCharArray();
                Value2Chars[0] = char.ToLower(FirstChar);
                FirstCharLower = new string(Value2Chars);
                Value2Chars[0] = char.ToUpper(FirstChar);
                FirstCharUpper = new string(Value2Chars);

            }

            Result = Result.Replace("{TM:" + name + "$FirstLower}", FirstCharLower);
            Result = Result.Replace("{TM:" + name + "$FirstUpper}", FirstCharUpper);
            Result = Result.Replace("{TM:" + name + "}", value);
            return Result;

        }


        public List<TMLoop> FindLoops(string input)
        {

            List<TMLoop> Result = new List<TMLoop>();


            //var LoopReg = new Regex("\\{Loop:([\\w-]+)}", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);

            //var m = LoopReg.Match(input);
            var LoopTagPattern = "\\{Loop:([\\w-]+)}";
            var m = Regex.Match(input, LoopTagPattern,
                   RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);
            var Tm1 = "\\{Loop:LoopName}(.+)\\{/Loop:LoopName}";
            while (m.Success)
            {
                Console.WriteLine("Found href " + m.Groups[1] + " at "
                   + m.Groups[1].Index); 
                x.Say("Found href " + m.Groups[1] + " at "
                   + m.Groups[1].Index);
                string SubLoopPartPattern = Tm1.Replace("LoopName", m.Groups[1].Value);
                var Match1 = Regex.Match(input, SubLoopPartPattern,
                   RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.Compiled);
                if (Match1.Success)
                {
                    TMLoop NewTag = new TMLoop()
                    {
                        TagName = m.Groups[1].Value,
                        TagContent = Match1.Groups[1].Value,
                        NoteText = Match1.Groups[0].Value,
                    };
                    Result.Add(NewTag);
                }
                m = m.NextMatch();
            }
            return Result;
        }
    }
}