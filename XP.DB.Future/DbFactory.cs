using System;
using System.Collections.Generic;
using System.Text;

namespace XP.DB.Future
{
    public class DbFactory
    {


        public static IProvider CreateProvider(ProviderItem dbProvider)
        {
            if (dbProvider.DbType == DBTypeDefined.Access)
            {

                var Provider = new OleDb.OleProvider(dbProvider.ConnString);
                return Provider;

            }
            else
            {
                var Provider = new SqlDb.SqlProvider(dbProvider.ConnString);
                return Provider;
            }

        }


    }
}
