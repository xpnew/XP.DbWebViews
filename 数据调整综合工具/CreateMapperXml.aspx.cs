using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP.DB.Comm;
using XP.DB.Future;
using XP.Util.WebUtils;

namespace 数据调整综合工具
{
    public partial class CreateMapperXml : HasProviderBase
    {
        private string _ResultBlockTm = @"
    <resultMap class=""{TM:ClassName}"" id=""{TM:ClassName}_Result"">
          {TM:AllProperty}
        </resultMap>

";
        private string _ResultLineTM = @"
    <result column=""{TM:PropertyTypeName}"" property=""{TM:PropertyTypeName}""/>";


        private string _InsertTM = @"
    <insert id=""Insert_{TM:ClassName}"" parameterClass=""{TM:ClassName}"">
      INSERT INTO [{TM:ClassName}]
      ({TM:InsertColumns})
      VALUES
      ({TM:InsertValues})
    </insert>
";


        private string _UpdateTM = @"
    <update id=""Update_{TM:ClassName}"" parameterClass=""{TM:ClassName}"">
        UPDATE [{TM:ClassName}] SET
      {TM:AllProperty}
         WHERE {TM:PkColumns}
    </update>
";

        private string _SelectOneTM = @"    <select id=""GetOne_{TM:ClassName}"" parameterClass=""guid"" resultMap=""{TM:ClassName}_Result"">
      select TOP 1 * from [{TM:ClassName}] where {TM:PkColumns}=#value#
    </select>
";
        private string _SelectAllTM = @"    <select id=""GetAll_{TM:ClassName}""  resultMap=""{TM:ClassName}_Result"">
      select  * from [{TM:ClassName}] 
    </select>
";

        protected void Page_Load(object sender, EventArgs e)
        {
            BindPage();
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




            StringBuilder sb = new StringBuilder();


            bool hasStart = false;
            foreach (var col in dt)
            {

                string PropertyString = _ResultLineTM.Replace("{TM:ColumnName}", col.ColumnName);
                //PropertyString = PropertyString.Replace("{TM:PropertyType}", col.PropertyType.Name);
                PropertyString = PropertyString.Replace("{TM:PropertyType}", col.PropertyTypeName);
                PropertyString = PropertyString.Replace("{TM:ColumnType}", col.ColumnTypeName);
                sb.Append(PropertyString);
            }

            string AllPropertyString = sb.ToString();
            string ClassString = _ResultBlockTm.Replace("{TM:ClassName}", TableName);


            ClassString = ClassString.Replace("{TM:AllProperty}", AllPropertyString);




            string InsertString = GetInsertMapper(dt, TableName);




            ClassString += InsertString;
            InsertString = GetInsertMapper_SkipDefault(dt, TableName);
            ClassString += InsertString;
            InsertString = GetInsertMapper_UseDefault(dt, TableName);
            ClassString += InsertString;


            string UpdateMapperString = GetUpdateMapper(dt, TableName);
            ClassString += UpdateMapperString;

            string SelectOneString= GetSelectOneMapper(dt,TableName);
            ClassString += SelectOneString;
            string SelectAllString = GetSelectAllMapper(dt, TableName);
            ClassString += SelectAllString;

            TextBox1.Text = ClassString;

        }


        public string GetSelectOneMapper(List<ColumnDtoItem> dt, string tablename)
        {
             bool hasStart = false;
            StringBuilder sbUpdateColumns = new StringBuilder();
            StringBuilder sbPkColumns = new StringBuilder();

            foreach (var col in dt)
            {
                if (hasStart)
                {
                    sbUpdateColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbUpdateColumns.Append("\n[");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("] = #");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("#");
            }
            hasStart = false;

            foreach (var col in dt.Where(u => u.IsPk))
            {
                if (hasStart)
                {
                    sbPkColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbPkColumns.Append(col.ColumnName);
            }
            if (!hasStart)
            {
                sbPkColumns.Append("没有找到 【主键】列，请手动指定  where  依据！！！");

            }

            var rss = _SelectOneTM;

            string ResultString = _SelectOneTM.Replace("{TM:ClassName}", tablename);
            ResultString = ResultString.Replace("{TM:AllProperty}", sbUpdateColumns.ToString());
            ResultString = ResultString.Replace("{TM:PkColumns}", sbPkColumns.ToString());

            return ResultString;

        }

        public string GetSelectAllMapper(List<ColumnDtoItem> dt, string tablename)
        {
            bool hasStart = false;
            StringBuilder sbUpdateColumns = new StringBuilder();
            StringBuilder sbPkColumns = new StringBuilder();

            foreach (var col in dt)
            {
                if (hasStart)
                {
                    sbUpdateColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbUpdateColumns.Append("\n[");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("] = #");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("#");
            }
            hasStart = false;

            foreach (var col in dt.Where(u => u.IsPk))
            {
                if (hasStart)
                {
                    sbPkColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbPkColumns.Append(col.ColumnName);
            }
            if (!hasStart)
            {
                sbPkColumns.Append("没有找到 【主键】列，请手动指定  where  依据！！！");

            }

            var rss = _SelectAllTM;

            string ResultString = _SelectAllTM.Replace("{TM:ClassName}", tablename);
            ResultString = ResultString.Replace("{TM:AllProperty}", sbUpdateColumns.ToString());
            ResultString = ResultString.Replace("{TM:PkColumns}", sbPkColumns.ToString());

            return ResultString;

        }


        public string GetInsertMapper(List<ColumnDtoItem> dt, string tablename)
        {
            StringBuilder sbInsertColumns = new StringBuilder();
            StringBuilder sbInsertValues = new StringBuilder();

            bool hasStart = false;
            foreach (var col in dt)
            {
                if (hasStart)
                {
                    sbInsertColumns.Append(",");
                    sbInsertValues.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbInsertColumns.Append("[");
                sbInsertColumns.Append(col.ColumnName);
                sbInsertColumns.Append("]");

                sbInsertValues.Append("#");
                sbInsertValues.Append(col.ColumnName);
                sbInsertValues.Append("#");
            }
            string InsertString = _InsertTM.Replace("{TM:ClassName}", tablename);
            InsertString = InsertString.Replace("{TM:InsertColumns}", sbInsertColumns.ToString());
            InsertString = InsertString.Replace("{TM:InsertValues}", sbInsertValues.ToString());

            InsertString = "全部列 \n" + InsertString;
            return InsertString;
        }

        public string GetUpdateMapper(List<ColumnDtoItem> dt, string tablename)
        {
            bool hasStart = false;
            StringBuilder sbUpdateColumns = new StringBuilder();
            StringBuilder sbPkColumns = new StringBuilder();

            foreach (var col in dt)
            {
                if (hasStart)
                {
                    sbUpdateColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbUpdateColumns.Append("\n[");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("] = #");
                sbUpdateColumns.Append(col.ColumnName);
                sbUpdateColumns.Append("#");
            }
            hasStart = false;

            foreach (var col in dt.Where(u => u.IsPk))
            {
                if (hasStart)
                {
                    sbPkColumns.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbPkColumns.Append("[");
                sbPkColumns.Append(col.ColumnName);
                sbPkColumns.Append("] = #");
                sbPkColumns.Append(col.ColumnName);
                sbPkColumns.Append("#");
            }
            if (!hasStart)
            {
                sbPkColumns.Append("没有找到 【主键】列，请手动指定更新依据！！！");

            }



            string ResultString = _UpdateTM.Replace("{TM:ClassName}", tablename);
            ResultString = ResultString.Replace("{TM:AllProperty}", sbUpdateColumns.ToString());
            ResultString = ResultString.Replace("{TM:PkColumns}", sbPkColumns.ToString());

            return ResultString;
        }




        public string GetInsertMapper_SkipDefault(List<ColumnDtoItem> dt, string tablename)
        {
            StringBuilder sbInsertColumns = new StringBuilder();
            StringBuilder sbInsertValues = new StringBuilder();

            bool hasStart = false;
            foreach (var col in dt)
            {

                if (null != col.DefaultRules)
                {
                    continue;
                }
                if (hasStart)
                {
                    sbInsertColumns.Append(",");
                    sbInsertValues.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbInsertColumns.Append("[");
                sbInsertColumns.Append(col.ColumnName);
                sbInsertColumns.Append("]");

                sbInsertValues.Append("#");
                sbInsertValues.Append(col.ColumnName);
                sbInsertValues.Append("#");
            }
            string InsertString = _InsertTM.Replace("{TM:ClassName}", tablename);
            InsertString = InsertString.Replace("{TM:InsertColumns}", sbInsertColumns.ToString());
            InsertString = InsertString.Replace("{TM:InsertValues}", sbInsertValues.ToString());

            InsertString = "跳过默认值  \n" + InsertString;
            return InsertString;
        }

        public string GetInsertMapper_UseDefault(List<ColumnDtoItem> dt, string tablename)
        {
            StringBuilder sbInsertColumns = new StringBuilder();
            StringBuilder sbInsertValues = new StringBuilder();

            bool hasStart = false;
            foreach (var col in dt)
            {
                if (hasStart)
                {
                    sbInsertColumns.Append(",");
                    sbInsertValues.Append(",");
                }
                else
                {
                    hasStart = true;
                }
                sbInsertColumns.Append("[");
                sbInsertColumns.Append(col.ColumnName);
                sbInsertColumns.Append("]");
                if (null != col.DefaultRules)
                {
                    sbInsertValues.Append(col.DefaultRules);
                }
                else
                {
                    sbInsertValues.Append("#");
                    sbInsertValues.Append(col.ColumnName);
                    sbInsertValues.Append("#");
                }
            }
            string InsertString = _InsertTM.Replace("{TM:ClassName}", tablename);
            InsertString = InsertString.Replace("{TM:InsertColumns}", sbInsertColumns.ToString());
            InsertString = InsertString.Replace("{TM:InsertValues}", sbInsertValues.ToString());

            InsertString = "使用默认值 \n" + InsertString;
            return InsertString;
        }


    }
}