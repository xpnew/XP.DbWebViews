<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowAllViews.aspx.cs" Inherits="数据调整综合工具.ShowAllViews" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    

        <asp:GridView ID="GridView1" runat="server" style="margin-top: 0px" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="表名" />
                <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                <asp:BoundField DataField="crdate" HeaderText="创建时间" />
                <asp:BoundField DataField="refdate" HeaderText="修改时间" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="ShowColumns.aspx?tablename={0}" HeaderText="查看列" Text="查看&gt;&gt;" Target="_blank" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="CreateCSharpClass.aspx?tablename={0}" HeaderText="创建类" Target="_blank" Text="创建&gt;&gt;&gt;" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="CreateMapperXml.aspx?tablename={0}" HeaderText="映射文件" Target="_blank" Text="创建映射&gt;&gt;&gt;" />
                 <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="EditColumns.aspx?tablename={0}" HeaderText="修改字段" Text="修改&gt;&gt;" Target="_blank" />
           </Columns>
        </asp:GridView>
        <br />
        <br />
            <br />
            <br />
            <a href="ShowAllTable.aspx">查看所有的表&gt;&gt;&gt;</a><br />
        <br />
            <a href="EditAllViews.aspx">修改视图信息&gt;&gt;&gt;</a><br />
    </div>
    </form>
</body>
</html>
