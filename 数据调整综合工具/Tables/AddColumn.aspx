<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddColumn.aspx.cs" Inherits="数据调整综合工具.Tables.AddColumn" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../css/jquery.jgrowl.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../scripts/jquery.jgrowl.min.js"></script>
    <script src="../scripts/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <link href="../css/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" media="screen" />
    <script src="../scripts/comm.js"></script>
    <title></title>
     <script type="text/javascript">
         $(function () {
             AddEvent2Link();
         });

         function AddEvent2Link() {
             $("a.uploadInIframe").fancybox({
                 'type': 'iframe',
                 'width': '50%',
                 'height': '50%',
                 'autoDimensions': false,
                 'autoScale': false
             });
             $("a.Link4Iframe").fancybox({
                 'type': 'iframe',
                 'width': '75%',
                 'height': '75%',
                 'autoDimensions': false,
                 'autoScale': false
             });
             $(".Link4Iframe a").fancybox({
                 'type': 'iframe',
                 'width': '75%',
                 'height': '75%',
                 'autoDimensions': false,
                 'autoScale': false
             });
         }
         function ClosePop() {
             $.fancybox.close();
         }
         function ChildWinClose() {
             try {
                 if (parent.$.fancybox) {
                     parent.$.fancybox.close();
                     parent.location.reload();
                 }
                 if ($.fancybox) {
                     $.fancybox.close();
                     location.reload();
                 }
             }
             catch (err) {
                 window.close();
             }
         }


    </script>
    <script type="text/javascript">
        $(document).ready(function () {

            $('#bt_save').click(function () {

                var tb_name = $('#tb_name').val();
                var col_name = $('#col_name').val();
                var col_defined = $('#col_defined').val();
                var col_fill = $('#col_fill').val();
                var Providers = $('#Providers').val();
                var _d = Math.round();

                if (CheckNull(tb_name) || CheckNull(col_name) || CheckNull(col_defined)) {
                    $.jGrowl("对不起参数错误 ", { life: 3000 });


                    return;

                }
                var submitData = {
                    tablename: '<%=TableName%>',
                    tb_name: tb_name,
                    col_name: col_name,
                    Providers:Providers,
                    col_defined: col_defined,
                    col_fill: col_fill,
                    _d: _d
                };
                $.jGrowl("------准备发送 数据 ", { life: 3000 });
                $.post('?act=save', submitData, function (data) {
                    $.jGrowl("------数据已经返回 数据 " + data, { life: 2000 });
                    if (0 < data.StatusCode) {

                        $.jGrowl("------操作完成！ ", { life: 8000 });
                        $.jGrowl("日志内容 " + data.Log, { life: 5000 });
                        //window.location.href = "";

                    } else {
                        alert(data.Title);
                    }
                }, 'json');

            });


        });
    </script>
    <style type="text/css">
        .auto-style1 {
            width: 96px;
            height: 32px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <div>
                <fieldset>

                    <legend>添加行</legend>
                alter table [diancai_shopinfo] ADD ShopOrderTypeId int null Default(28001);
                       <hr />


                    操作的目标：<input id="ProvidersName" type="text" name="ProvidersName" />    
                    <input type="hidden" name="Providers" id="Providers" />
                      <a class="Link4Iframe" href="../JsSelector/SelectProvider.aspx?jsonctrl=Providers&amp;namectrl=ProvidersName&amp;multi=1&amp;act=add"
                                        title="选择一个数据库">
                                        <img src="../images/bt/bt_select.png" style="vertical-align: middle;" border="0"
                                            alt="上传图片" />
                                    </a>
                    <br /> 
                表名：<input id="tb_name" type="text" name="tb_name" value="<%= TableInfo.ObjectName %>" />
                (例：[dingdan_manage],不要有方括号，程序会自动添加方括号 ）
        <br />                列名：<input id="col_name" type="text" name="col_name" />
                (例：[IsPrivateRoom] ）
                <br />
                列定义<input id="col_defined" type="text" name="col_defined" />
                （例： bit null Default(0)）
                <br />


                </fieldset>

                
        <hr />

            </div>
            <div>
                 <fieldset>
                    <legend>填充 空字段的默认值 ：</legend>
               
        （完整语句： UPDATE [wx_diancai_dingdan_manage] SET [IsPrivateRoom] =0 WHERE [IsPrivateRoom] IS NULL） 
                        <hr />
                <input id="col_fill" type="text" name="col_fill" />

                （例： 0；例如 '是'，如果填写了单引号，会被自动修改成双引号：）

                </fieldset>

     
                <input id="bt_save" type="button" value="创建列并且填充空值" />
                 <input id="bt_fill" type="button" value="填充空值" />
            </div>
        </div>
    </form>
</body>
</html>
