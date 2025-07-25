using System;
using System.Collections.Generic;
using System.Text;

namespace XP.DB.Future
{
    public class ProviderItem
    {

        /// <summary>
        /// Id 数字库里的主键 
        /// </summary>
        public int Id { get; set; }
        /// <summary>别名  </summary>
        public string AliasName { get; set; }


        /// <summary>连接字符串  </summary>
        public string ConnString { get; set; }


        /// <summary>是否当前使用的  </summary>
        public bool IsCurrent { get; set; }


        /// <summary>数据库类型  </summary>
        public DBTypeDefined DbType { get; set; }



        /// <summary>代码里使用的名字  </summary>
        public string Name2Code { get; set; }



    }
}
