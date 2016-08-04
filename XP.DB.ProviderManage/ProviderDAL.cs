using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.MiniEF;

namespace XP.DB.ProviderManage
{
    public class ProviderDAL : AutoBuildBaseDAL<ProviderT>
    {

         public ProviderDAL() : base() { }





         public ProviderDAL(IProvider provider) : base(provider) { }



         //public List<ProviderT> GetAll()
         //{

         //    string sql = "select * from [DbObjectT] where [ParentId]=0 AND  [ProviderId]=" + providerid;

         //    return Select2Model<ProviderT>(this.Provider, sql);
         //}

    }
}
