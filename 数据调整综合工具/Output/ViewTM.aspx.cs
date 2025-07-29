using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.Future;
using XP.Text.Tags.Utils;
using XP.Util.Text;
using XP.Util.WebUtils;

namespace 数据调整综合工具.Output
{
    public partial class ViewTM : HasProviderBase
    {
        private string TableName { get; set; }
        public string ResultText { get; set; }
        public DbObjectT TableInfo { get; set; }

        private TemplateT _Template;


        private List<ColumnDtoItem> ColumnItems { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                SayError("需要指定一个表名。", "/ShowAllTable.aspx");
            }


            if (null == _Template)
            {

                GetTM();
            }
            SayResult();

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

            ProviderInfo CurrentProviderSet = Session["CurrentProvider"] as ProviderInfo;

            var CurrentProvider = DbFactory.CreateProvider(CurrentProviderSet);


            var ss = CurrentProvider.Analyzer;

            var dt = ss.AllTables();


            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var ListTable = dal.GetTables(CurrentProviderSet.Id);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);
            var List = ss.GetColumn(TableName);

            ColumnItems = List;



            string Cot = _Template.Cot;
            //Cot = Cot.Replace("{TM:CompactName}", CompactName);
            //Cot = Cot.Replace("{TM:ObjectName}", ObjectName);

            string NewText = Cot.Clone() as string;

            string CompactName = GetCompactName(TableInfo.ObjectName);
            //带大小写处理
            NewText = TMUtil.ReplaceTmItem(NewText, "TableName", TableInfo.ObjectName);
            NewText = TMUtil.ReplaceTmItem(NewText, "ObjectName", TableInfo.ObjectName);
            //NewText = TMUtil.ReplaceTmItem(NewText, "DBTableName", TextUtil.BuildDMUpperName(TableInfo.DBTableName));
           // NewText = TMUtil.ReplaceTmItem(NewText, "ParentModel", DbModel.ParentModel);



            NewText = NewText.Replace("{TM:TableGlobalName}", TableInfo.GlobalName);
            NewText = NewText.Replace("{TM:CompactName}", CompactName);
            NewText = NewText.Replace("{TM:Summary}", TableInfo.Summary);
            NewText = NewText.Replace("{TM:Remarks}", TableInfo.Remarks);

            NewText = RenderLoopContent(NewText);


            ResultText = NewText;
        }

        protected string RenderLoopContent(string input)
        {
            if (HasError) { return String.Empty; }
            string Result = input;


            var SubTm = LoopTagUtil.FindLoops(input);

            foreach (var tm in SubTm)
            {
                //sb.Append("\n=============");
                //sb.Append(tm.TagName);
                //sb.Append("==============\n");

                //int i = 0;
                //foreach (var col in AttributeList)
                //{
                //    string NewLineStr = tm.TagContent;
                //    foreach (var Pname in TypeCache.PropertyNames)
                //    {
                //        string ItemStr = "";
                //        var val = _PropertyValueGeter.GetValue(col, Pname);
                //        if (null != val)
                //        {
                //            ItemStr = val.ToString();
                //        }
                //        //NewLineStr = NewLineStr.Replace("{TM:" + Pname + "}", ItemStr);
                //        NewLineStr = ReplaceTmItem(NewLineStr, Pname, ItemStr);
                //    }
                //    //{TM:LoopNum}
                //    NewLineStr = NewLineStr.Replace("{TM:LoopNum}", i.ToString());
                //    sb.Append(NewLineStr);
                //    i++;

                //}

                //string FullTmString = tm.TagContent;

                //x.Say("找到的模板内容1 TagContent ：" + tm.TagContent);
                //x.Say("找到的模板内容2 NoteText (带模板标签) ：" + tm.NoteText);
                //x.Say("==============================");
                //x.Say("原始的内容：" + input);
                //x.Say("==============================");
                //x.Say("替换之后的内容：" + input.Replace(tm.NoteText, "**** 此处应类循环字段***  "));
                //x.Say("==============================");


                //替换用的第一段文本（源）
                string ReplaceText1 = tm.NoteText + "";
                //替换用的第二段文本（目标）
                string ReplaceText2 = String.Empty; // tm.NoteText +"";
                string tmText = tm.TagContent;
                int Num = 0;

                foreach (var col in ColumnItems)
                {
                    string coltext = tmText;

                    coltext = TMUtil.ReplaceTmItem(coltext, "ColumnName", col.ColumnName);
                    coltext = TMUtil.ReplaceTmItem(coltext, "ColumnType", col.ColumnTypeName);
                    coltext = TMUtil.ReplaceTmItem(coltext, "PropertyType", col.PropertyTypeName);
                    coltext = TMUtil.ReplaceTmItem(coltext, "PropertyTypeName", col.PropertyTypeName);
                    coltext = TMUtil.ReplaceTmItem(coltext, "ColGlbalName", col.GlobalName);
                    coltext = TMUtil.ReplaceTmItem(coltext, "GlobalName", col.GlobalName);


                    coltext = coltext.Replace("{TM:LoopNum}", Num.ToString());
                    //coltext = TMUtil.ReplaceTmItem(coltext, "ColRemark", col.ColRemark);

                    ReplaceText2 += coltext;
                    Num++;
                }

                x.Say("==============================");
                x.Say("原始的内容：" + input);
                x.Say("==============================");
                x.Say("准备替换的：" + Result);

                Result = Result.Replace(ReplaceText1, ReplaceText2);

                x.Say("==============================");
                x.Say("替换之后的内容：" + Result);


            }




            return Result;
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