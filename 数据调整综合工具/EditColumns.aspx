<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditColumns.aspx.cs" Inherits="数据调整综合工具.EditColumns" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="Scripts/jquery-1.8.3.min.js"></script>

    <script src="Scripts/comm.js"></script>

    <script type="text/javascript">

        var ColumnsNameChsIndex = 2;

        $(function () {
            FindColumnNameChsIndex();
            $('.OneObjectSubmit').click(function () {
                SaveOne(this);
            });
            $('.import').click(function () {
              
                $('#act').val('import');
                

                $('#form1').submit();
            });

            $('.clearall').click(function () {
                
                var Path = '?act=clearall&tablename=' + getRequestQueryString('tablename');
                Path += '&_dx=' + Math.random();
                $.get(Path, {}, function (data) {
                    if (null != data) {
                        if (typeof (data) == 'string') {
                            data = $.parseJSON(data);
                        }
                        if (null != data.StatusCode) {
                            Debug('StatusCode ' + data.StatusCode);
                            Debug('Name ' + data.Name);
                            Debug('Title ' + data.Title);
                            Debug('Body ' + data.Body);
                            if (0 < data.StatusCode) {
                                Debug('成功返回！');
                                alert('操作成功！');
                                location.reload();
                            } else if (0 > data.StatusCode) {
                                alert('操作失败');
                            } else {
                                alert('返回异常');
                            }
                            return;
                        }
                        alert(data);

                    } else {
                        alert("返回错误");
                    }
                });
            });
        });

        function FindColumnNameChsIndex() {
            Debug('old  ColumnsNameChsIndex : ' + ColumnsNameChsIndex);
            var FirstLine = $('#GridView1>tbody>tr').eq(0);
            if (null == FirstLine) {
                return;
            }
            var FindTh = false;

            var ThList = FirstLine.find('th');
            if (null != ThList && 0 < ThList.size()) {
                ThList = true;
                FirstLine.find('th').each(function (i, e) {
                    if ($(e).text() == '中文名') {
                        ColumnsNameChsIndex = i;
                        return true;
                    }
                });
            } else {
                FirstLine.find('td').each(function (i, e) {
                    if ($(e).text() == '中文名') {
                        ColumnsNameChsIndex = i;
                        return true;
                    }
                });
            }
            Debug('new  ColumnsNameChsIndex : ' + ColumnsNameChsIndex);
        }

            function SaveOne(sender) {
                var GlobalName = $(sender).siblings('input[name="GlobalName"]').val();
                var Summary = $(sender).siblings('input[name="Summary"]').val();
                var Remarks = $(sender).siblings('input[name="Remarks"]').val();
                var ObjectId = $(sender).siblings('input[name="ObjectId"]').val();
                var ObjectName = $(sender).siblings('input[name="ObjectName"]').val();

                var Model = { GlobalName: GlobalName, Summary: Summary, Remarks: Remarks, Id: ObjectId, ObjectName: ObjectName };


                if ('' == GlobalName) {
                    alert('表单【中名】不能为空！');
                    return;
                }


                var Path = '?act=save&tablename=' + getRequestQueryString('tablename');
                Path += '&_dx=' + Math.random();
                $.get(Path, {Model:Model}, function (data) {
                    if (null != data) {

                        if ('ok' == data.toLowerCase()) {
                            //alert($(sender).parent().parent().children('td').length);
                            $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex).text(GlobalName);
                            $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex+1).text(Summary);
                            $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex+2).text(Remarks);
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
                                    $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex).text(GlobalName);
                                    $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex+1).text(Summary);
                                    $(sender).parent().parent().children('td').eq(ColumnsNameChsIndex+2).text(Remarks);
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
            <asp:GridView ID="GridView1" runat="server" CssClass="tb01" Style="margin-top: 0px" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ColumnName" HeaderText="列名" />
                    <asp:BoundField DataField="Description" HeaderText="字段说明" />
                    <asp:BoundField DataField="ColumnType" HeaderText="类型" />
                    <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                    <asp:BoundField DataField="Summary" HeaderText="简要说明" />
                    <asp:BoundField DataField="Remarks" HeaderText="备注" />
                    <asp:TemplateField HeaderText="提交">
                        <ItemTemplate>
                            中名：<input id="Text1" name="GlobalName" type="text" value="<%#Eval("GlobalName") %>" />
                            摘要：<input id="Text2" name="Summary" type="text" value="<%#Eval("Summary") %>" /><br />
                            备注：<input id="Text3" name="Remarks" type="text" value="<%#Eval("Remarks") %>" />
                            <input id="Hidden5" name="ObjectId" type="hidden" value="<%#Eval("ObjectId") %>" />
                            <input id="Hidden6" name="ObjectName" type="hidden" value="<%#Eval("ColumnName") %>" />
                            <input id="Button1" class="OneObjectSubmit" type="button" value="修改" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:HyperLinkField DataNavigateUrlFields="ColumnName" DataNavigateUrlFormatString="EditColumns.aspx?tablename={0}" HeaderText="修改字段" Text="修改&gt;&gt;" Target="_blank" />
                </Columns>
            </asp:GridView>

            <input type="button" class="clearall" value="清除已经有的字段信息（当前表）" /><br />
            
        <asp:FileUpload ID="FileUpload1" runat="server" />   <input type="button" class="import" value="导入字段字段信息（当前表）" />
               <input id="act" name="act" type="hidden" value="" />
        </div>
    </form>
</body>
</html>
