using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
using System.Data.Common;

namespace XP.DB.Future.OleDb
{
    public class OleProvider : BaseProvider
    {
        private OleDbConnection _Conn;


        public DbConnection Conn
        {
            get { return _Conn; }
            set
            {
                if (value is OleDbConnection)
                {
                    _Conn = value as OleDbConnection;
                }
                else
                {
                    var NewConn = this.CreateConn(value.ConnectionString);
                    _Conn = NewConn as OleDbConnection;
                }
            }
        }


        public OleProvider(string connString)
            : base(connString)
        {

        }


        protected override void InitConn(string connStr)
        {
            base.InitConn(connStr);
            Conn = new OleDbConnection(connStr);

        }


        public override DbConnection CreateConn(string connStr)
        {
            return new OleDbConnection(connStr);
        }


        public override DbCommand CreateCommand(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);

            return cmd;

        }

        public override DataAdapter CreateAdapter(DbCommand cmd)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();

            da.SelectCommand = cmd as OleDbCommand;
            return da;
        }


        public override DataAdapter CreateAdapter(string sql)
        {
            OleDbCommand cmd = new OleDbCommand(sql, _Conn);
            OleDbDataAdapter da = new OleDbDataAdapter();

            da.SelectCommand = cmd;
            return da;
        }


        public override ITableAnalyze GreateTableAnalyzer()
        {
            //return base.GreateTableAnalyzer();

            return new OleTableAnalyze();
        }


    }
}
