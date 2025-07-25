using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.MiniEF;

namespace XP.DBTools.BLL
{
    public class BassDLL<TDal, TEntity>
        where TDal : AutoBuildBaseDAL<TEntity>, new()
        where TEntity : class,new()
    {
        public string ConnStr { get; set; }
        public TDal AutoDAL { get; set; }




        public BassDLL()
            : this(null)
        {

        }
        public BassDLL(string connStr)
        {

            if (String.IsNullOrEmpty(connStr))
            {
                ConnStr = XP.Util.Conf.ConnStr;

            }
            else
            {
                ConnStr = connStr;
            }

            var Provider = new XP.DB.Future.OleDb.OleProvider(ConnStr);
            AutoDAL = new TDal(); ;
            AutoDAL.Provider = Provider;

        }

    }
}
