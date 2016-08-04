<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAllViews.aspx.cs" Inherits="数据调整综合工具.EditAllViews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <style type="text/css">
        .input_remark {border-color:#cccccc; height:16px;}
        .input_remark_auto{border-color:#3333ff; word-wrap:break-word;word-break:break-all; width:200px; height:auto; min-height:120px;}
    </style>
       <script src="Scripts/jquery-1.8.3.min.js"></script>
    <script src="Scripts/comm.js"></script>

    <script type="text/javascript">

        $(function () {
            
            $('.OneObjectSubmit').click(function () {
                SaveOne(this);
            });

            $('.input_remark').focus(function () {
                $(this).addClass('input_remark_auto');
            });
            $('.input_remark').blur(function () {
                $(this).removeClass('input_remark_auto');
            });
        });


        function SaveOne(sender) {
            var GlobalName = $(sender).siblings('input[name="GlobalName"]').val();
            var Summary = $(sender).siblings('input[name="Summary"]').val();
            var Remarks = $(sender).siblings('textarea[name="Remarks"]').val();
            var ObjectId = $(sender).siblings('input[name="ObjectId"]').val();
            var ObjectName = $(sender).siblings('input[name="ObjectName"]').val();

            var Model = { GlobalName: GlobalName, Summary: Summary, Remarks: Remarks, Id: ObjectId, ObjectName: ObjectName };


            if ('' == GlobalName) {
                alert('表单【中名】不能为空！');
                return;
            }


            var Path = '?act=save'
            Path += '&_dx=' + Math.random();
            $.get(Path, {Model:Model}, function (data) {
                if (null != data) {

                    if ('ok' == data.toLowerCase()) {
                        //alert($(sender).parent().parent().children('td').length);
                        $(sender).parent().parent().children('td').eq(1).text(GlobalName);
                        $(sender).parent().parent().children('td').eq(2).text(Summary);
                        $(sender).parent().parent().children('td').eq(3).text(Remarks);
                    } else {
                        if (typeof (data) == 'string') {
                            data = $.parseJSON(data);
                        }
                        if (null != data.StatusCode) {
                            Debug('StatusCode ' + data.StatusCode);
                            Debug('Name ' + data.Name);
                            Debug('Title ' + data.Title);
                            Debug('Body ' + data.Body);
                            if (0 < data.StatusCode) {
                                if (0 <= data.Name.toLowerCase().indexOf('insert')) {
                                    Debug('准备更新ID ：' + data.Body);
                                    $(sender).siblings('input[name="ObjectId"]').val(data.Body);
                                    Debug('更新之后，重新获取的表单Id  ' + $(sender).siblings('input[name="ObjectId"]').val());
                                }
                                $(sender).parent().parent().children('td').eq(1).text(GlobalName);
                                $(sender).parent().parent().children('td').eq(2).text(Summary);
                                $(sender).parent().parent().children('td').eq(3).text(Remarks);
                                Debug('成功返回！');
                            } else if (0 > data.StatusCode) {
                                alert('操作失败');
                            } else {
                                alert('返回异常');
                            }
                            return;
                        }

                        alert(data);
                    }

                } else {
                    alert("返回错误");
                }
            });

        }

    </script>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" style="margin-top: 0px" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="视图" />
                <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                <asp:BoundField DataField="Summary" HeaderText="简要说明" />
                <asp:BoundField DataField="Remarks" HeaderText="备注" />
                <asp:BoundField DataField="Id" HeaderText="对象Id" />
                <asp:TemplateField HeaderText="提交">
                    <ItemTemplate>
                        中名：<input id="Text1" name="GlobalName" type="text" value="<%#Eval("GlobalName") %>"/>
                        摘要：<input id="Text2" name="Summary"  type="text" value="<%#Eval("Summary") %>" /><br /> 
                        备注：<textarea id="Text3" name="Remarks" class="input_remark" ><%#Eval("Remarks") %></textarea>
                        <input id="Hidden5" name="ObjectId"  type="hidden" value="<%#Eval("ObjectId") %>"  />
                        <input id="Hidden6" name="ObjectName"  type="hidden" value="<%#Eval("name") %>"  />
                        <input id="Button1" class="OneObjectSubmit"  type="button" value="修改" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="EditColumns.aspx?tablename={0}" HeaderText="修改字段" Text="修改&gt;&gt;" Target="_blank" />
            </Columns>
        </asp:GridView>
                <a href="ShowAllViews.aspx">返回 视图查看 &gt;&gt;&gt;</a><br />

    </div>
    </form>
</body>
</html>
