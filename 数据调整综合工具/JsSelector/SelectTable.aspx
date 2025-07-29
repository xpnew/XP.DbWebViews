<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTable.aspx.cs" Inherits="数据调整综合工具.JsSelector.SelectTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="/css/comm.css" rel="stylesheet" />
    <link href="/css/base.css" rel="stylesheet" />
    <link href="../css/jquery.jgrowl.min.css" rel="stylesheet" />
    <link href="../css/jquery.jgrowl.min.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../scripts/jquery.jgrowl.min.js"></script>
    <script src="../scripts/jquery.jgrowl.min.js"></script>
    <script src="../scripts/comm.js"></script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {
            //ReturnValCtrl = FindCtrl('valctrl');

            $('#TableSelector').change(function () {
                var txt = $(this).find("option:selected").text();
                var val = $(this).val();
                if (CheckNull(val)) {
                    return;
                }
                Debug("选择了：" + val);
                Debug("选择了：" + txt );
                
                $('#TargeObj').text(txt);
                ClearRight();
                SelectTable(val);

            });
            ClearRight();
        });


        function ClearRight() {


            $('#GridView2 input.ColumnNameChecked').prop('checked', false);
            $('#GridView2 td.ColumnName').text('');
            $('#GridView2 td.Description').text('');
            $('#GridView2 td.GlobalName').text('');
            $('#GridView2 td.Summary').text('');
            $('#GridView2 td.Remarks').text('');

        }

        function SelectTable(tablename) {
            var Path = '?act=ViewCols&tablename=' + tablename;
            Path += '&_dx=' + Math.random();
            $.get(Path, function (data) {
                Debug('返回数据：', data);
                if (null != data) {
                    if (typeof (data) == 'string') {
                        data = $.parseJSON(data);
                    }
                    if (null != data.StatusCode) {
                        if (0 < data.StatusCode) {

                            Debug('成功返回！');
                            SetRight(data.DataInfo);
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



        }
        function SetRight(lst) {
            //Debug('列表：', lst);
            $.each(lst, function (index, item) {
                Debug(index + ': ' + item);
                //{"ColumnName":"TypeName","Description":"名称","ColumnType":"nvarchar(100)","GlobalName":"名称","Remarks":"(中文名从数据库备注复制)","ObjectId":"2165"}


                var line = FindRightLine(item.ColumnName);

                if (null != line) {
                    line.find('input.ColumnNameChecked').prop('checked', true);;
                    
                    line.find('td.ColumnName').text(item.ColumnName);
                    line.find('td.Description').text(item.Description);
                    line.find('td.GlobalName').text(item.GlobalName);
                    line.find('td.Summary').text(item.Summary);
                    line.find('td.Remarks').text(item.Remarks);
                }
               

            });

        }
        function FindRightLine(name) {
            var chk = $('#GridView2 input.ColumnNameChecked[value="' + name + '"]');
            if (0 < chk.size()) {
                cur = chk.eq(0);
                Debug('has find ' + name);
               
                return  cur.closest('.rightline'); 
            } else {
                Debug('not find ' + name);
            }
            return null;
        }

    </script>


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
                $('#GridView2 input.ColumnNameChecked:checked').each(function (index, elm) {
                    IdArr.push($(this).val());
                    var line = $(elm).closest('.rightline');
                    var NewItem = {
                        ColumnName: $(this).val(), GlobalName: line.find('td.GlobalName').text()
                        , Summary: line.find('td.Summary').text(), Remarks: line.find('td.Remarks').text()
                    };
                    NameArr.push(NewItem.GlobalName);

                    JsonArr.push(NewItem);
                });
                var IdValue = IdArr.join(',');
                var NameValue = NameArr.join(',');
                var JsonValue = JSON.stringify(JsonArr);
                if (window.opener) {
                    Debug('存在上级');
                    window.opener.FileLinkage(IdValue, JsonArr);
                    window.close();


                } else {
                    Debug('不存在上级');
                    if (null != ReturnValCtrl) {
                        ReturnValCtrl.value = IdValue;
                    }
                    if (null != ReturnNameCtrl) {
                        ReturnNameCtrl.value = NameValue;
                    }
                    if (null != ReturnJsonCtrl) {
                        ReturnJsonCtrl.value = JsonValue;
                    }
                    parent.FileLinkage(IdValue, JsonArr);
                    //parent.returnValue = document.getElementById('FilenameHidden').value;
                    parent.ClosePop();
                }
            }

            function GetSelectCol() {
                var IdArr = new Array();
                var NameArr = new Array();
                var JsonArr = new Array();
                $('#GridView2 input.ColumnNameChecked:checked').each(function (index, elm) {
                    IdArr.push($(this).val());
                    var line = $(elm).closest('.rightline');
                    var NewItem = {
                        ColumnName: $(this).val(), GlobalName: line.find('td.GlobalName').text()
                        , Summary: line.find('td.Summary').text(), Remarks: line.find('td.Remarks').text()
                    };
                    NameArr.push(NewItem.GlobalName);
                    
                    JsonArr.push(NewItem);
                });

                PageDebug("找到的id " + IdArr.join(','));
                PageDebug("找到的name " + NameArr.join(','));

                PageDebug("找到的json " + JSON.stringify(JsonArr));

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
                var InputControlName = getRequestQueryString('uploadInput');
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
    <div>


<table  >
    <thead>
        <tr>
        <th>当前表[<%= TableInfo.ObjectName %> (<%= TableInfo.GlobalName %>)]</th>
        <th>选择的表<span id="TargeObj"></span></th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
            <asp:GridView ID="GridView1" runat="server" CssClass="tb01" Style="margin-top: 0px" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ColumnName" HeaderText="列名" />
                    <asp:BoundField DataField="Description" HeaderText="字段说明" />
                    <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                    <asp:BoundField DataField="Summary" HeaderText="简要说明" />
                    <asp:BoundField DataField="Remarks" HeaderText="备注" />
                </Columns>
            </asp:GridView>

            </td>
            <td>
            <asp:GridView ID="GridView2" runat="server" CssClass="tb01" Style="margin-top: 0px" AutoGenerateColumns="False" RowStyle-CssClass="rightline">
                <Columns>
                    <asp:TemplateField HeaderText="选择">
                        <ItemTemplate>
                          <input type="checkbox" class="ColumnNameChecked" checked="checked" value="<%# Eval("ColumnName") %>"   />
                            

                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ColumnName" HeaderText="列名" ItemStyle-CssClass="ColumnName"/>
                    <asp:BoundField DataField="Description" HeaderText="字段说明" ItemStyle-CssClass="Description"/>
                    <asp:BoundField DataField="GlobalName"  HeaderText="中文名" ControlStyle-CssClass="GlobalName" ItemStyle-CssClass="GlobalName" />
                    <asp:BoundField DataField="Summary" HeaderText="简要说明" ControlStyle-CssClass="Summary" ItemStyle-CssClass="Summary" />
                    <asp:BoundField DataField="Remarks" HeaderText="备注" ControlStyle-CssClass="Remarks" ItemStyle-CssClass="Remarks" />
                </Columns>
            </asp:GridView>

            </td>
        </tr>

    </tbody>


</table>

    </div>
      
    <div>
    <select id="TableSelector">
        <option value="">选择一个对象</option>
        <optgroup label="各种表">
         <asp:Repeater ID="Repeater1" runat="server">
            <ItemTemplate>
             <option value="<%# DataBinder.Eval(Container.DataItem, "ObjectName") %>"><%# DataBinder.Eval(Container.DataItem, "ObjectName") %>（<%# DataBinder.Eval(Container.DataItem, "GlobalName") %>）</option>
            </ItemTemplate>
        </asp:Repeater>
      </optgroup>


    </select>


    </div>
    <fieldset>
        <legend>操作</legend>
        <input type="button" value="完成&关闭" name="bt_close" onclick="CloseSelf();" />

         <input type="button" value="Test Args:Inputid" name="bt_test1" onclick="TestGetCtrl();"  />

         <input type="button" value="测试获取到的值" name="bt_test1" onclick="GetSelectCol();" />
    </fieldset>       
    </form>
</body>
</html>

