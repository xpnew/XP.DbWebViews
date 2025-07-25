using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using XP.Comm.Attributes;
using XP.Comm.Filters;
using XP.DB.Comm;

namespace XP.DB.DbEntity
{
    [Serializable]
    [IdentityClass]
    [System.Data.Linq.Mapping.Table(Name = "ProviderT")]
    public class ProviderInfo 
    {
         /// <summary>
        /// Id 数字库里的主键 
        /// </summary>
        [IdentityFieldAttribute]
        [PrimaryKey]
        [Column(DbType = "Int NOT NULL", CanBeNull = false, IsPrimaryKey=true, IsDbGenerated= true)]
        public int Id { get; set; }
        /// <summary>别名  </summary>
        [Column(DbType = "nvarchar(50)", CanBeNull = true)]
        public string AliasName { get; set; }


        /// <summary>连接字符串  </summary>
        [Column(DbType = "nvarchar(255)", CanBeNull = true)]
        public string ConnString { get; set; }


        /// <summary>是否当前使用的  </summary>
        [Column(DbType = "Int NULL", CanBeNull = true)]
        public int IsCurrent { get; set; }


        /// <summary>数据库类型  </summary>
        [Column(DbType = "Int NULL", CanBeNull = true)]
        public DBTypeDefined DbType { get; set; }



        /// <summary>代码里使用的名字  </summary>
        [Column(DbType = "nvarchar(50)", CanBeNull = true)]
        public string Name2Code { get; set; }


        private string _DbTypeName;

        [NotDataFilter]
        [UIMember]
        public string DbTypeName
        {
            get
            {
                if (null == _DbTypeName)
                {
                    _DbTypeName = System.Enum.GetName(typeof(DBTypeDefined), this.DbType);
                }
                return _DbTypeName;
            }
            set { _DbTypeName = value; }
        }
        

    }

    [Serializable]
    [IdentityClass]
    public class ProviderT
    {
        /// <summary>
        /// Id 数字库里的主键 
        /// </summary>
        [IdentityFieldAttribute]
        [PrimaryKey]     
        public int Id { get; set; }
        /// <summary>别名  </summary>
        public string AliasName { get; set; }


        /// <summary>连接字符串  </summary>
        public string ConnString { get; set; }




        public bool Current { get; set; }
        /// <summary>是否当前使用的  </summary>
        [Column(DbType = "Int NULL", CanBeNull = true, IsDiscriminator = true)]
        //[NotMapped]
        public int IsCurrent { get; set; }


        /// <summary>数据库类型  </summary>
        [Column(DbType = "Int NULL", CanBeNull = true, IsDiscriminator = true)]
        public DBTypeDefined DbType { get; set; }



        /// <summary>代码里使用的名字  </summary>
        public string Name2Code { get; set; }


        private string _DbTypeName;
        [NotDataFilter]
        [UIMember]
        public string DbTypeName
        {
            get
            {
                if (null == _DbTypeName)
                {
                    _DbTypeName = System.Enum.GetName(typeof(DBTypeDefined), this.DbType);
                }
                return _DbTypeName;
            }
            set { _DbTypeName = value; }
        }
        
    }
}