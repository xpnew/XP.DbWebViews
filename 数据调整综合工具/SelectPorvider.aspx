<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPorvider.aspx.cs" Inherits="数据调整综合工具.SelectPorvider" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
         <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>
      <script src="../js/comm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">

        <input type="button" value="test close" name="bt_clost" />

        <input type="button" value="Test Args:Inputid" name="bt_test1" />


        <div>
            选择数据库
        </div>

        <div>
            <asp:AccessDataSource ID="AccessDataSource1" runat="server" DataFile="~/App_Data/db.mdb" SelectCommand="SELECT * FROM [ProviderT]"></asp:AccessDataSource>

            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="AliasName" HeaderText="AliasName" SortExpression="AliasName" />
                    <asp:BoundField DataField="ConnString" HeaderText="ConnString" SortExpression="ConnString" />
                    <asp:BoundField DataField="IsCurrent" HeaderText="IsCurrent" SortExpression="IsCurrent" />
                    <asp:BoundField DataField="DBType" HeaderText="DBType" SortExpression="DBType" />
                    <asp:BoundField DataField="Name2Code" HeaderText="Name2Code" SortExpression="Name2Code" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="?act=save&amp;id={0}" DataTextField="AliasName" HeaderText="选择数据库" DataTextFormatString="选择【{0}】》》" />
                </Columns>
            </asp:GridView>
            <br />
            <br />

        </div>
    </form>
</body>
</html>
