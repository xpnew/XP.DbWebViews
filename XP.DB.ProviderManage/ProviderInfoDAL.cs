using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.Comm;
using XP.DB.DbEntity;
using XP.DB.MiniEF;

namespace XP.DB.ProviderManage
{
    public class ProviderInfoDAL : AutoBuildBaseDAL<ProviderInfo>
    {

        public ProviderInfoDAL() : base() { }





        public ProviderInfoDAL(IProvider provider) : base(provider) { }



        public List<ProviderInfo> FindIdList(List<int> idList)
        {
            var Result = new List<ProviderInfo>();

            var AllList = GetAll();

            var list = AllList.Where(m => idList.Contains(m.Id));
            if (!list.Any())
            {
                return Result;
            }

            Result = list.ToList();
            return Result;
        }


    }
}
