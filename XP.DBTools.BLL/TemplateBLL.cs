using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using XP.DB.DbEntity;
using XP.DB.ProviderManage;

namespace XP.DBTools.BLL
{
    [System.ComponentModel.DataObject]
    public class TemplateBLL : BassDLL<TemplateDAL,TemplateT>
    {
        //TemplateDAL dal = new TemplateDAL();


        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<TemplateT> GetALL()
        {
            return AutoDAL.GetAll();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TemplateT GetItemById(int id)
        {
            return AutoDAL.GetItemById(id);
        }

        [DataObjectMethod(DataObjectMethodType.Update, false)]
        public int Update(TemplateT model)
        {
            model.Cot = model.Cot.Replace("'", "''");
            return AutoDAL.UpdateModel(model);
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public int Insert(TemplateT model)
        {
            model.Cot = model.Cot.Replace("'", "''");
            return AutoDAL.InsertModel(model);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int DeleteById(int id)
        {
            return AutoDAL.DeleteById(id);
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public int DeleteByIdList(List<int> idList)
        {
            return AutoDAL.DeleteByIdList(idList);
        }

    }
}
