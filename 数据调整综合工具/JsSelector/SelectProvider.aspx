<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectProvider.aspx.cs" Inherits="数据调整综合工具.JsSelector.SelectProvider" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link href="../css/jquery.jgrowl.min.css" rel="stylesheet" />
    <link href="../css/jquery.jgrowl.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../scripts/jquery.jgrowl.min.js"></script>
    <script src="../scripts/jquery.jgrowl.min.js"></script>
    <script src="../scripts/comm.js"></script>

    <script type="text/javascript" language="javascript">

        var ReturnValCtrl = null;
        var ReturnNameCtrl = null;
        var ReturnJsonCtrl = null;
        

        $(document).ready(function () {
            ReturnValCtrl = FindCtrl('valctrl');
            ReturnNameCtrl = FindCtrl('namectrl');
            ReturnJsonCtrl = FindCtrl('jsonctrl');
        });
     function TestGetCtrl() {
         ReturnValCtrl = FindCtrl('valctrl');
         ReturnNameCtrl = FindCtrl('namectrl');
         ReturnJsonCtrl = FindCtrl('jsonctrl');
         PageDebug("找到的传ID控件 " + ReturnValCtrl);
         PageDebug("找到的传名控件 " + ReturnNameCtrl);
         PageDebug("找到的传JSON控件 " + ReturnJsonCtrl);
     }
     function FindCtrl(ctrlName) {
         var InputControlName = getRequestQueryString(ctrlName);
         if (null != InputControlName) {
             //父页面上传文件保存的控件名称
             var ParentInput = parent.document.getElementById(InputControlName);
             if (null == ParentInput) {
                 PageDebug("----符全条件的控件数量：  " + $('input[name$="' + InputControlName + '"]', window.parent.document).length);
                 ParentInput = $('input[name$="' + InputControlName + '"]', window.parent.document).get(0);
             }
             if (null != ParentInput) {
                 //alert(ParentInput.outerHTML);
                 Debug(ParentInput.outerHTML)
                 $.jGrowl(ctrlName + "----已经找到了！  ");
                 return ParentInput;
             } else {
                 PageDebug("------虽然传递了参数 ，但是没有找到个对象 ");
             }
         } else {
             PageDebug("------没有找到参数  ");
         }
         return null;
     }
        
     function CloseSelf() {
         var IdArr = new Array();
         var NameArr = new Array();
         var JsonArr = new Array();
         $('input[name="SelectId"]:checked').each(function (index, elm) {
             IdArr.push($(this).val());
             NameArr.push($(this).attr('ref'));
             var NewItem = { Id: $(this).val(), AliasName: $(this).attr('ref') };
             JsonArr.push(NewItem);
         });
         var IdValue = IdArr.join(',');
         var NameValue = NameArr.join(',');
         var JsonValue = JSON.stringify(JsonArr);
         if (window.opener) {
             window.opener.FileLinkage(IdValue, NameValue, JsonValue);
             window.close();


         } else {
             if (null != ReturnValCtrl) {
                 ReturnValCtrl.value = IdValue;
             }
             if (null != ReturnNameCtrl) {
                 ReturnNameCtrl.value = NameValue;
             }
             if (null != ReturnJsonCtrl) {
                 ReturnJsonCtrl.value = JsonValue;
             }
             //parent.returnValue = document.getElementById('FilenameHidden').value;
             parent.ClosePop();
         }
     }

     function GetSelectId() {
         var IdArr = new Array();
         var NameArr = new Array();
         var JsonArr = new Array();
         $('input[name="SelectId"]:checked').each(function (index, elm) {
             IdArr.push($(this).val());
             NameArr.push($(this).attr('ref'));
             var NewItem = {Id : $(this).val(), AliasName : $(this).attr('ref')};
             JsonArr.push(NewItem);
         });

         PageDebug("找到的id " + IdArr.join(','));
         PageDebug("找到的name " + NameArr.join(','));
         
         PageDebug("找到的name " +JSON.stringify(JsonArr) );

     }
     function GetSelectName() {


     }

     function PageDebug(msg) {
         $.jGrowl(msg, { life: 3000 });
     }
     function PageSay(msg) {
         $.jGrowl(msg, { life: 3000 });
     }

        function CloseThis() {
            //alert(window.opener);
            var  InputControlName =  getRequestQueryString('uploadInput');
            if (null != InputControlName) {
                   //父页面上传文件保存的控件名称
                var ParentInput = parent.document.getElementById(InputControlName);
                if (null == ParentInput) {
                    ParentInput = $('input[name$="' + InputControlName + '"]', window.parent.document).get(0);
                }
                if (null != ParentInput)
                    ParentInput.value = document.getElementById('FilenameHidden').value;
                parent.ClosePop();
                return;
            }
            if (window.opener) {
                //            var key = getRequestQueryString("key");
                //            var act = getRequestQueryString("act");
                //            var result = window.opener.document.getElementById(key);
                //            if(result)
                //            {
                //                if(act=='add' && result.value != '')
                //                {
                //					
                //                    result.value += '|' + document.getElementById('FilenameHidden').value;
                //                }
                //                else
                //                {
                //                    result.value = document.getElementById('FilenameHidden').value;
                //                
                //                }
                //            
                //            }
                if (document.getElementById('FilenameHidden') && document.getElementById('FilenameHidden').value != '')
                    window.opener.FileLinkage(document.getElementById('FilenameHidden').value);
                    window.close();

                } else {
                    parent.returnValue = document.getElementById('FilenameHidden').value;
                    parent.close();
                }
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="test_area" >

           <input type="button" value="test finish close" name="bt_close" onclick="CloseSelf();" />

        <input type="button" value="Test Args:Inputid" name="bt_test1" onclick="TestGetCtrl();"  />
            
        <input type="button" value="刷新" name="bt_test1" onclick="window.location.reload();" />


                 <input type="button" value="测试获取到的值" name="bt_test1" onclick="GetSelectId();" />
        </div>
    <div>
         选择您要操作的数据库（Providers），查看各个数据库的明细请看<a href="../SelectPorvider.aspx"  target="_blank">这里&gt;&gt;</a>
        <ul>
 <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
                <li>
                    <% if(this.IsMulti){ %>
                    <input type="checkbox" name="SelectId" id="SelectId_<%# Eval("Id") %>" value="<%# Eval("Id") %>" ref="<%#Eval("AliasName") %>" />
                    <%}else{ %>
                     <input type="radio" name="SelectId" id="SelectId_<%# Eval("Id") %>" value="<%# Eval("Id") %>" ref="<%#Eval("AliasName") %>" />
                    <%} %>
                    <label for="SelectId_<%# Eval("Id") %>" >
                        <%#Eval("AliasName") %> ( <%#Eval("Id") %>, <%#Eval("IsCurrent") %>,<%#Eval("DbTypeName") %>)
                    </label>
                </li>


            </ItemTemplate>
        </asp:Repeater>
            </ul>

           <input type="button" value="完成&关闭" name="bt_test1" onclick="CloseThis();" />

    </div>
    </form>
</body>
</html>
