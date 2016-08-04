using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XP.DB.DbEntity;

namespace XP.DB.ProviderManage
{
    [System.ComponentModel.DataObject]
    public class Enum2List
    {


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Dictionary<int, string> GetTemplateLoopType()
        {
            return GetDict<TemplateLoopType>();
        }


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Dictionary<int, string> GetTemplateOutType()
        {

            Dictionary<int, string> Result = new Dictionary<int, string>();

            var Arr = Enum.GetNames(typeof(XP.DB.DbEntity.TemplateOutType));
            var ValueArr = Enum.GetValues(typeof(TemplateOutType));

            foreach (var em in ValueArr)
            {
                var name = Enum.GetName(typeof(TemplateOutType),em);
                name += "[" + em.ToString() + "]";
                Result.Add((int)em, name);
            }

            //for (int i = 0; i < Arr.Length; i++)
            //{
            //    Result.Add(ValueArr[i], Arr[i]);

            //}

                return Result;
        }


        public Dictionary<int, string> GetDict<T>() where T : struct
        {
            Dictionary<int, string> Result = new Dictionary<int, string>();

            if (typeof(Enum) != typeof(T).BaseType)
            {
                return Result;
            }
            var EnumType = typeof(T);
            var Arr = Enum.GetNames(EnumType);
            var ValueArr = Enum.GetValues(EnumType);

            foreach (var em in ValueArr)
            {
                var name = Enum.GetName(EnumType, em);
                name += "[" + em.ToString() + "]";
                Result.Add((int)em, name);
            }

            //for (int i = 0; i < Arr.Length; i++)
            //{
            //    Result.Add(ValueArr[i], Arr[i]);

            //}

            return Result;
        }

    }
}
