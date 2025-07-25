using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XP.DB.MiniEF;
using XP.DB.DbEntity;
using XP.DB.Comm;
namespace XP.DB.ProviderManage
{
    public class DbObjectDAL : AutoBuildBaseDAL<DbObjectT>
    {


        public DbObjectDAL() : base() { }





        public DbObjectDAL(IProvider provider) : base(provider) { }


        public List<DbObjectT> GetTables(int providerid)
        {
            string sql = "select * from [DbObjectT] where [ParentId]=0 AND  [ProviderId]=" + providerid;

            return Select2Model<DbObjectT>(this.Provider, sql);
        }


        public List<DbObjectV> GetViews(int providerid)
        {
            string sql = "select * from [DbObjectV] where [ProviderId]=" + providerid;

            return Select2Model<DbObjectV>(this.Provider, sql);
        }



        public List<DbObjectV> GetColumnViewsByTableName(int providerid, string tablename)
        {
            DbObjectT NewTable = GetTable(providerid, tablename);

            string sql = "select * from [DbObjectV]  where  [ProviderId]=" + providerid + " and  [ParentId]=" + NewTable.Id + "";

            return Select2Model<DbObjectV>(this.Provider, sql);
        }


        public List<DbObjectT> GetMembers(int providerid, string tablename)
        {
            DbObjectT NewTable;
            string TableWhereSql = "[ProviderId]=" + providerid + " and  [ObjectName]='" + tablename + "'";

            NewTable = GetOneBySql(TableWhereSql);

            if (null == NewTable)
            {
                return new List<DbObjectT>();
            }
            string sql = "select * from [DbObjectT] where [ProviderId]=" + providerid + " and  [ParentId]=" + NewTable.Id;

            return Select2Model<DbObjectT>(this.Provider, sql);            
        }

        public List<DbObjectT> GetMembers(int providerid, int tableId)
        {
             string sql = "select * from [DbObjectT] where [ProviderId]=" + providerid + " and  [ParentId]=" + tableId;

            return Select2Model<DbObjectT>(this.Provider, sql);
        }


        public DbObjectT GetTable(int providerid, string tablename)
        {
            DbObjectT NewTable;
            string TableWhereSql = "[ProviderId]=" + providerid + " and  [ObjectName]='" + tablename + "'";

            NewTable = GetOneBySql(TableWhereSql);

            if (null == NewTable || 0 == NewTable.Id)
            {
                NewTable = new DbObjectT() { ProviderId = providerid, ObjectName = tablename, ParentId = 0 };
                string InsertSql = this.Model2InsertSql(NewTable);
                int Id = base.InsertAndId(InsertSql);
                if (0 > Id)
                {
                    NewTable = GetOneBySql("[ProviderId]=" + providerid + " and  [ObjectName]='" + tablename + "'");
                }
                else
                {
                    NewTable.Id = Id;
                }
            }

            return NewTable;

        }




        public DbObjectV GetViewById(int id)
        {
            string sql = "select * from [DbObjectV] ";

            var l = Select2Model<DbObjectV>(this.Provider, sql);

            if (null != l && 0 < l.Count)
            {
                return l.First();
            }
            return null;
        }

        public int ClearOtherTableName(int providerId,int excentTableId, string tableName)
        {

            string TableWhereSql = " [ProviderId]=" + providerId + " AND [Id]<>"+excentTableId+" AND  [ObjectName]='" + tableName + "' ";


            return base.DeleteModel(TableWhereSql);
        }


        public int ClearColumnsByTableName(int providerId, string tableName)
        {

            string TableWhereSql = " [ProviderId]=" + providerId + " and [ParentId] IN (SELECT [Id] FROM [DbObjectT] WHERE  [ObjectName]='" + tableName + "' )";


            return base.DeleteModel(TableWhereSql);
        }

    }
}
