using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace 数据调整综合工具.JsSelector
{
    public class SelectPageBase : HasProviderBase
    {

        public bool IsMulti { get; set; }


        protected override void Page_PreLoad(object sender, EventArgs e)
        {
            base.Page_PreLoad(sender, e);

            if (String.IsNullOrEmpty(Request["multi"]))
            {
                IsMulti = false;
            }
            else
            {
                string MultiSet = Request["multi"];
                if ("1" == MultiSet || "true" == MultiSet)
                {
                    IsMulti = true;
                }
                else
                {
                    IsMulti = false;
                }
            }
            
        }
    }
}