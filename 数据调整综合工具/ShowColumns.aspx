<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowColumns.aspx.cs" Inherits="数据调整综合工具.ShowColumns" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

        <script src="Scripts/jquery-1.8.3.min.js"></script>


    <script src="Scripts/comm.js"></script>


        <script type="text/javascript">

            $(function () {

                $('.bt_ShowTools').toggle(function () { Debug('click 1 '); $('.block4Fields').show(); }, function () { $('.block4Fields').hide(); });
            });

    </script>


    <style type="text/css">
        .block {
            height: 150px;
        }
        .bt_ShowTools{ color:blue;}

        .block4Fields{display:none;}



    </style>


</head>
<body>
    <form id="form1" runat="server">
    <div>
    
       <h3>表[<%=this.TableInfo.ObjectName %>](<%=this.TableInfo.GlobalName %>)的列信息</h3>
            <asp:GridView ID="GridView2" runat="server" Style="margin-top: 0px" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="ColumnName" HeaderText="列名" />
                    <asp:BoundField DataField="Description" HeaderText="字段说明" />
                    <asp:BoundField DataField="主键" HeaderText="主键" />
                    <asp:BoundField DataField="ColumnType" HeaderText="类型" />
                    <asp:BoundField DataField="允空" HeaderText="允空" />
                    <asp:BoundField DataField="默认值" HeaderText="默认值" />
                    <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                    <asp:BoundField DataField="Summary" HeaderText="简要说明" />
                    <asp:BoundField DataField="Remarks" HeaderText="备注" />
                </Columns>
            </asp:GridView>

            <br />
        <br />
        <br />
        原始数据：<asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <h3><a href="javascript:void();" class="bt_ShowTools">字段工具</a></h3>
        <div class="block block4Fields">


            <ul>
                <li><a href="Output/SayColumns2TextArea.aspx?tablename=<%=TableInfo.ObjectName  %>&tmid=5" target="_blank">使用模板生成代码(模板5)</a></li>
                <li><a href="Output/SayColumns2TextArea.aspx?tablename=<%=TableInfo.ObjectName  %>&tmid=6" target="_blank">使用模板生成代码(模板6)</a></li>
                <li><a href="Output/SayColumns2TextArea.aspx?tablename=<%=TableInfo.ObjectName  %>&tmid=7" target="_blank">使用模板生成代码(模板7)</a></li>
            </ul>

        </div>



    </div>

    </form>
</body>
</html>
