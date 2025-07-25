using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;

namespace XP.DB.Future
{

    /// <summary>
    /// 数据库列映射单项
    /// </summary>
    public class ColumnDtoItem
    {

        public string ColumnName { get; set; }

        public System.Data.Common.DbParameter DbParameter { get; set; }


        public DbType ColumnType { get; set; }


        public Type PropeterType { get; set; }


        /// <summary>
        /// 字符长度
        /// </summary>
        public int? CharLength { get; set; }


        /// <summary>
        /// 数字精度，整数部分长度
        /// </summary>
        public int? NumericPrecision { get; set; }

        /// <summary>
        /// 数字标度，小数部分长度
        /// </summary>
        public int? NumericScale { get; set; }


        public string DefaultRules { get; set; }
        public string ColumnTypeName { get; set; }

        public string ColumnTypeVSLength { get; set; }


        public bool IsNullable { get; set; }

    }
}
