using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Attributes;

namespace XP.DB.DbEntity
{
    public class DbObjectV : DbObjectT
    {
        [IdentityFieldAttribute]
        [PrimaryKey]
        public int Id { get; set; }

        public string ParentName { get; set; }
        public string ParentGlobalName { get; set; }


    }
}
