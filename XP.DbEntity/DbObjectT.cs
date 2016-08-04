using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.Comm.Attributes;

namespace XP.DB.DbEntity
{
    [Serializable]
    [IdentityClass]
    public class DbObjectT
    {

        [IdentityFieldAttribute]
        [PrimaryKey]
        public int Id { get; set; }


        public int ProviderId { get; set; }

        public int ParentId { get; set; }
        public string ObjectName { get; set; }
        public string Summary { get; set; }
        public string Remarks { get; set; }
        public string GlobalName { get; set; }

    }
}
