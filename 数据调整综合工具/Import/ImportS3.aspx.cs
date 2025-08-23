using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;
using XP.DB.DbEntity;
using XP.DB.ProviderManage;
using XP.IO.ExcelUtil;

namespace 数据调整综合工具.Import
{
    public partial class ImportS3 : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindPage();

        }

        protected  void BindPage()
        {
            string Path = "~/App_Data/S3_Ext1.xlsx";
            Path = Server.MapPath(Path);

            var Reader = new MultiTableReader(Path);
            //Reader.LineIndex = 1049;


            //if (Reader.HasMerged())
            //{

            //    return;
            //}

            Reader.GetTables();


            foreach (TableInfo tb  in Reader.ResultItems)
            {
                x.Say($"表名：{tb.Name } 中文： {tb.GlobalName}");

                SaveTable(tb);


            }


        }

        private int S3ProviderId = 9;

        private void SaveTable(TableInfo tb) {
            DbObjectT NewModel = new DbObjectT() { 
                ProviderId = S3ProviderId,
                ObjectName  =tb.Name,
                GlobalName =tb.GlobalName,          
                ParentId = 0
            };
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            var Exist =   dal.GetTable(S3ProviderId, tb.Name);
            int Id = -1;
            if (null == Exist)
            {
                 Id = dal.InsertEntity(NewModel);

            }
            else
            {
                Id = Exist.Id;
                Exist.GlobalName = tb.GlobalName;
                dal.UpdateModel(Exist);
            }
            foreach (var col in tb.Columns)
            {

                SaveCol(col, Id);
            }

        }
        private void SaveCol(ColumnInfo col,int tbid)
        {
            DbObjectT NewModel = new DbObjectT()
            {
                ProviderId = S3ProviderId,
                ObjectName = col.Name,
                GlobalName = col.GlobalName,
                ParentId = tbid,
            };
            DbObjectDAL dal = new DbObjectDAL(SiteProvider);

            var Exist = dal.GetOneBySql($" ProviderId={S3ProviderId} and ParentId ={tbid} and ObjectName='{col.Name}'");  //dal.GetTable(S3ProviderId, col.Name);
            int Id = -1;

            if (null == Exist)
            {
                Id = dal.InsertEntity(NewModel);

            }
            else
            {
                Exist.GlobalName = col.GlobalName;
                dal.UpdateModel(Exist);

            }


        }


    }
}