using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.Comm;
using XP.DB.MiniEF;

namespace XP.DB.ProviderManage
{
    public class SchemaDAL : BaseDAL
    {

        public SchemaDAL() : base() { }





        public SchemaDAL(IProvider provider) : base(provider) { }


        protected void _Init()
        {
            //AllCol = 
        }


        public List<ColumnDtoItem> AllCol { get; set; }



        /// <summary>
        /// 是否存在指定的列，0表示 未知
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <returns></returns>
        public int ExistColumn(string tableName, string colName)
        {


            var collist = Provider.Analyzer.GetColumn(tableName);


            if (null == collist && 0 == collist.Count)
            {
                return 0;
            }

            if (collist.Any(m => m.ColumnName.Equals(colName)))
            {
                return 1;
            }

            return -1;

        }



        public bool ExistLine(string sql)
        {

            //var ds = DbHelperSQL.Query(sql);

            //if (null == ds)
            //{
            //    return false;
            //}
            //if (0 == ds.Tables.Count)
            //{
            //    return false;
            //}
            //var dt = ds.Tables[0];
            //if (0 == dt.Rows.Count)
            //{
            //    return false;
            //}
            return true;
        }

        public bool AddColumn(string sql)
        {
#if DEBUG
            x.TimerLog("代码调试，模拟执行sql : " + sql);

#endif



            int Result = Provider.ExecuteSql(sql);

            if (0 < Result)
            {
                return true;
            }
            return false;
        }


    }
}
