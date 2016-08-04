<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowAllTable.aspx.cs" Inherits="数据调整综合工具.ShowAllTable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="css/comm.css" rel="stylesheet" />
    <link href="css/base.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.8.3.min.js"></script>
    <script src="Scripts/comm.js"></script>




</head>
<body>
    <form id="form1" runat="server">
    <div><a href="Tables/RowsCount.aspx">跳到行数
    

        <asp:GridView ID="GridView1" runat="server" CssClass="tb01" style="margin-top: 0px" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="name" HeaderText="表名" />
                <asp:BoundField DataField="GlobalName" HeaderText="中文名" />
                <asp:BoundField DataField="crdate" HeaderText="创建时间" />
                <asp:BoundField DataField="refdate" HeaderText="修改时间" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="ShowColumns.aspx?tablename={0}" HeaderText="查看列" Text="查看&gt;&gt;" Target="_blank" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="CreateCSharpClass.aspx?tablename={0}" HeaderText="创建类" Target="_blank" Text="创建&gt;&gt;&gt;" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="CreateMapperXml.aspx?tablename={0}" HeaderText="映射文件" Target="_blank" Text="创建映射&gt;&gt;&gt;" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="EditColumns.aspx?tablename={0}" HeaderText="修改字段" Text="修改&gt;&gt;" Target="_blank" />
                <asp:HyperLinkField DataNavigateUrlFields="name" DataNavigateUrlFormatString="Tables/Index.aspx?tablename={0}" HeaderText="更多功能" Text="进入表工具&gt;&gt;" Target="_blank" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
        跳转到存储过程&gt;&gt;&gt;  
        <br />
        <br />
            <a href="Tables/RowsCount.aspx">跳到行数统计>>>>
        <br />
            <a href="Tables/LoadSqlDesc.aspx">跳到表格说明（只支持sql server）>>>>
        <br />
            <a href="EditTableInfo.aspx">编辑表信息&gt;&gt;&gt;</a><br />
        <br />
              <a href="ShowAllViews.aspx">查看所有的视图&gt;&gt;&gt;</a><br />
  

    </div>
    </form>
</body>
</html>
