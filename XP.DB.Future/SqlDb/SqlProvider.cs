using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data.Common;

namespace XP.DB.Future.SqlDb
{
    public class SqlProvider : BaseProvider
    {
        private SqlConnection _Conn;


        public DbConnection Conn
        {
            get { return _Conn; }
            set
            {
                if (value is SqlConnection)
                {
                    _Conn = value as SqlConnection;
                }
                else
                {
                    var NewConn = this.CreateConn(value.ConnectionString);
                    _Conn = NewConn as SqlConnection;
                }
            }
        }

        public SqlProvider(string connString)
            : base(connString)
        {

        }


        public SqlProvider(SqlConnection conn)
        {
            this._Conn = conn;

            this.ConnStr = conn.ConnectionString;

        }

        protected override void InitConn(string connStr)
        {
            base.InitConn(connStr);
            Conn = new SqlConnection(connStr);

        }

        public override DbConnection CreateConn(string connStr)
        {
            return new SqlConnection(connStr);
        }


        public override DbCommand CreateCommand(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);

            return cmd;

        }

        public override DataAdapter CreateAdapter(DbCommand cmd)
        {
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd as SqlCommand;
            return da;
        }


        public override DataAdapter CreateAdapter(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, _Conn);
            SqlDataAdapter da = new SqlDataAdapter();

            da.SelectCommand = cmd;
            return da;
        }


        public override ITableAnalyze GreateTableAnalyzer()
        {
            //return base.GreateTableAnalyzer();

            return new SqlTableAnalyze(_Conn);
        }

    }
}
