using System;
using System.Collections.Generic;
using System.Text;

using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace XP.DB.Future.SqlDb
{
    public class SqlTableAnalyze : ITableAnalyze
    {
        private SqlConnection _Conn;

        private SqlProvider _Provider;

        public SqlTableAnalyze(SqlConnection conn)
        {
            this._Conn = conn;
        }

        public SqlTableAnalyze(SqlProvider provider)
        {
            this._Provider = provider;
        }


        public List<ColumnDtoItem> GetColumn(string tablename)
        {
            List<ColumnDtoItem> Result = new List<ColumnDtoItem>();

            string sql = "select * from information_schema.columns where TABLE_NAME ='" + tablename + "'";


            var Provider = new SqlProvider(_Conn);


            var dt = Provider.Select(sql);

            foreach (DataRow row in dt.Rows)
            {

                var NewItem = new ColumnDtoItem();

                string Name = row["COLUMN_NAME"].ToString();

                NewItem.ColumnName = Name;
                NewItem.ColumnTypeName = row["DATA_TYPE"].ToString();

                NewItem.IsNullable = null == row["IS_NULLABLE"] ? true : (bool)row["IS_NULLABLE"];

                if (DBNull.Value != row["CHARACTER_MAXIMUM_LENGTH"])
                {
                    NewItem.CharLength = (int)row["CHARACTER_MAXIMUM_LENGTH"];
                }
                if (DBNull.Value != row["NUMERIC_PRECISION"])
                {
                    NewItem.NumericPrecision = (int)row["NUMERIC_PRECISION"];
                }
                if (DBNull.Value != row["NUMERIC_SCALE"])
                {
                    NewItem.NumericScale = (int)row["NUMERIC_SCALE"];
                }

                MkType(NewItem);

                Result.Add(NewItem);
            }


            return Result;

        }
        public void MkType(ColumnDtoItem item)
        {
            Type t;
            string typename = item.ColumnTypeName;
            typename = typename.ToLower();
            switch (typename)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    t = typeof(string);
                    break;
                case "int":
                case "smallint":
                    t = typeof(int);
                    break;

                case "bigint":
                    t = typeof(long);
                    break;
                case "bit":
                    t = typeof(bool);
                    break;
                case "decimal":
                    t = typeof(decimal);
                    break;
                case "":
                default:
                    t = typeof(object);
                    break;
            }


            if (typeof(string) == t)
            {
                item.ColumnTypeVSLength = typename + "(" + item.CharLength + ")";
            }
            else if (typeof(decimal) == t)
            {
                item.ColumnTypeVSLength = typename + "(" + item.NumericPrecision + "," + item.NumericScale + ")";

            }
            else if (typeof(int) == t)
            {
                item.ColumnTypeVSLength = typename + "(" + item.NumericPrecision + ")";
            }
            else
            {
                item.ColumnTypeVSLength = typename;
            }

        }


        public static Type TransSqlType(DbType dbtype)
        {
            Type t;

            switch (dbtype)
            {

                default:
                    t = typeof(object);
                    break;
            }
            return t;
        }



        public static Type TransSqlType(string typename, int length, int decimalLenght)
        {
            Type t;
            typename = typename.ToLower();
            switch (typename)
            {
                case "varchar":
                case "nvarchar":
                case "char":
                case "nchar":
                case "text":
                case "ntext":
                    t = typeof(string);
                    break;
                case "int":
                case "smallint":
                    t = typeof(int);
                    break;

                case "bigint":
                    t = typeof(long);
                    break;
                case "bit":
                    t = typeof(bool);
                    break;
                case "decimal":
                    t = typeof(decimal);

                    break;

                case "":

                default:
                    t = typeof(object);
                    break;
            }
            return t;
        }
    }
}
