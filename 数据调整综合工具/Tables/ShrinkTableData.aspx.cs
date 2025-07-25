using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XP;

namespace 数据调整综合工具.Tables
{

    /// <summary>
    /// 收缩表格里面的数据
    /// </summary>
    public partial class ShrinkTableData : HasProviderBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if ("ByFile" == Request["act"])
            {
                ByFile();
                return;
            }



        }


        protected void ByFile()
        {
            var path = Server.MapPath("~/App_Data/BigTable.txt");


            using (var streamReader = new StreamReader(path))
            {

                if (null == streamReader)
                {
                    JsonErr("打开文件失败");

                }

                string LineText = streamReader.ReadLine();
                while (null != LineText)
                {
                    _PageMsg.AddLog("查到的数据：" + LineText);

                    if (String.IsNullOrWhiteSpace(LineText)) continue;

                    string TableName = LineText;

                    string sql = $"select TOP 1 Id from [{TableName}] order by id desc";

                    var r = CurrentProvider.SingleColumn(sql);
                    if (null != r && DBNull.Value != r)
                    {
                        if (vbs.IsInt(r))
                        {
                            int LastId  = int.Parse(r.ToString());

                            //if (Count > 1000)
                            //{
                            //    x.Say("大表格： " + TableName + "\n" + Count);
                            //}

                            sql = $"delete from [{TableName}]  where Id>100 and id < {LastId}";

                            x.Say("准备执行的sql : " +  sql);

                            CurrentProvider.ExecuteSql(sql);


                        }




                    }




                    //Biz_PayOrderT ExistOrder = Biz_PayOrderBLL.Self.GePayOrderByNo(LineText);

                    //if (ExistOrder != null)
                    //{

                    //    var flag4http = SeenMsgWuhan(true, Convert.ToString(Convert.ToDecimal(ExistOrder.PayAmountFee) / 100), ExistOrder.PlayerUserId, ExistOrder.OrderNo);

                    //    if (flag4http)
                    //    {
                    //        Loger.Info("商户属于武汉，发送通知成功" + ExistOrder.OrderNo);
                    //    }
                    //    else
                    //    {
                    //        Loger.Error("商户属于武汉，但发送通知失败" + ExistOrder.OrderNo);
                    //    }


                    //}


                    LineText = streamReader.ReadLine();
                }
            }
        }

    }
}