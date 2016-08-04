using System;
using System.Collections.Generic;
using System.Text;

using System.Data.Common;
using System.Data;



namespace XP.DB.Future
{
    public class BaseProvider : IProvider
    {
        private ITableAnalyze _AnaLyzer;

        public ITableAnalyze AnaLyzer
        {
            get {
                if (null == _AnaLyzer)
                {
                    _AnaLyzer = GreateTableAnalyzer();
                }
                
                return _AnaLyzer; }
            set { _AnaLyzer = value; }
        }


        ITableAnalyze IProvider.Analyzer
        {
            get
            {
                return this.AnaLyzer;
            }
            set
            {
                this.AnaLyzer = value;
            }
            
        }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnStr { get; set; }

        public DbConnection Conn { get; set; }


        public DbCommand Cmd { get; set; }

       

        public BaseProvider()
        {

            Init();
        }

        public BaseProvider(string connString)
            : this()
        {
            InitConn(connString);

        }


        protected void InitDefault()
        {

        }
        protected void Init()
        {
            InitDefault();

        }



        protected virtual void InitConn(string connStr)
        {
            this.ConnStr = connStr;

        }

        public DataTable Select(string sql)
        {


            DataAdapter da = CreateAdapter(sql);

            var dt = new DataSet();

            da.Fill(dt);

            return dt.Tables[0];

        }
        public int ExecuteSql(string sql)
        {

            var cmd = CreateCommand(sql);

            int Result = cmd.ExecuteNonQuery();

            return Result;

        }





        public virtual DbConnection CreateConn(string connStr)
        {
            throw new NotImplementedException();
        }


        public virtual DbCommand CreateCommand(string sql)
        {
            throw new NotImplementedException();
        }

        public virtual DataAdapter CreateAdapter(DbCommand cmd)
        {
            throw new NotImplementedException();
        }

        public virtual DataAdapter CreateAdapter(string sql)
        {
            throw new NotImplementedException();
        }
        public virtual ITableAnalyze GreateTableAnalyzer()
        {
            throw new NotImplementedException();

        }


    }
}
