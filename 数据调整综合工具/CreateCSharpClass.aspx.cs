using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.DbEntity;
using XP.DB.ProviderManage;
using XP.Util.WebUtils;

namespace 数据调整综合工具
{
  
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public partial class CreateCSharpClass : HasProviderBase
    {

        private string _ClassTm = @"

    /// <summary>
    /// {TM:TableGlobalName} （数据库 {TM:ClassName}表）
    /// </summary>
    [Serializable]
    public class  {TM:ClassName}: Comm.Entity.IntIdEntityBase
    {
        //注意处理Id 字段
        
        {TM:AllProperty}

    }


";
        private string _PropertyTM = @"
        /// <summary>{TM:ColumnGlobalName} </summary>{TM:Remarks}
        public {TM:PropertyType} {TM:PropertyTypeName} { get; set; }
";
        private string _RemarkTM = @"
        /// <remarks>{TM:RemarkLine}
        /// </remarks>";



        public DbObjectT TableInfo { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {


            BindPage();
            InitPage();

        }


        protected void InitPage()
        {

            string TableName = RequestUtil.FindString("tablename");

            if (String.IsNullOrEmpty(TableName))
            {
                //SayError("需要指定一个表名。", "EditTableInfo.aspx");
                TableInfo = new DbObjectT();
            }

            DbObjectDAL dal = new DbObjectDAL(SiteProvider);


            TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);
            dal = null;

        }

        private void BindPage()
        {

            var Provider = GetProvider();

            var ss = Provider.Analyzer;

            string TableName = RequestUtil.FindString("tablename");
            var dt = ss.GetColumn(TableName);


            if (null == dt)
            {

                Alert00Close("获取数据失败！");
            }

            var dal = new XP.DB.ProviderManage.DbObjectDAL(SiteProvider);

            var TableInfo = dal.GetTable(CurrentProviderSet.Id, TableName);



            var ListTable = dal.GetMembers(CurrentProviderSet.Id, TableInfo.Id);

            //dt.ForEach(m => m.ColumnGlobalName = ListTable.Where(s => s.ObjectName == m.ColumnName).Select(s => s.ColumnGlobalName).FirstOrDefault());


            StringBuilder sb = new StringBuilder();

            foreach (var col in dt)
            {

                string PropertyString = _PropertyTM.Replace("{TM:PropertyTypeName}", col.ColumnName);
                //PropertyString = PropertyString.Replace("{TM:PropertyType}", col.PropertyType.Name);
                string ColumnGlobalName = col.ColumnName;
                string PropertyRemarks = String.Empty;
                string RemarkLine = String.Empty;
                string PropertyTypeName = col.PropertyTypeName;
                if (col.IsNullable && "string" != PropertyTypeName && "String" != PropertyTypeName)
                {
                    PropertyTypeName += "?";
                }
                if (!String.IsNullOrEmpty(col.GlobalName))
                {
                    ColumnGlobalName = col.GlobalName;
                }
                var ExistGlobaleInfo = ListTable.Where(s => s.ObjectName == col.ColumnName);
                if (ExistGlobaleInfo.Any())
                {
                    var GlobaleInfo = ExistGlobaleInfo.First();
                    if (!String.IsNullOrEmpty(GlobaleInfo.GlobalName))
                    {
                        ColumnGlobalName = GlobaleInfo.GlobalName;
                    }

                    if (!String.IsNullOrEmpty(GlobaleInfo.Summary))
                    {
                        RemarkLine = "\n/// 说明： " + GlobaleInfo.Summary; 
                    }
                    if (!String.IsNullOrEmpty(GlobaleInfo.Remarks))
                    {
                        RemarkLine += "\n/// 备注： " + GlobaleInfo.Remarks;
                    }
                }

                if (!String.IsNullOrEmpty(RemarkLine))
                {
                    PropertyRemarks = _RemarkTM.Replace("{TM:RemarkLine}", RemarkLine);
                }

                PropertyString = PropertyString.Replace("{TM:ColumnGlobalName}", ColumnGlobalName);
                PropertyString = PropertyString.Replace("{TM:PropertyType}", PropertyTypeName);

                PropertyString = PropertyString.Replace("{TM:Remarks}", PropertyRemarks);

                sb.Append(PropertyString);
            }

            string AllPropertyString = sb.ToString();
            string ClassString = _ClassTm.Replace("{TM:ClassName}", TableName);

            if (!String.IsNullOrEmpty(TableInfo.GlobalName))
            {
                ClassString = ClassString.Replace("{TM:TableGlobalName}", TableInfo.GlobalName);
            }
            else
            {
                ClassString = ClassString.Replace("{TM:TableGlobalName}", TableName + "(中名暂缺)");
            }
            ClassString = ClassString.Replace("{TM:AllProperty}", AllPropertyString);



            TextBox1.Text = ClassString;

        }






    }
}