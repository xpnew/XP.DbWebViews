<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="数据调整综合工具._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            选择你的操作：</div>

        <div>
            <br />
            <br />
            <a href="ShowAllTable.aspx" target="_blank">查看所有的表&gt;&gt;&gt;</a><br />
            <br />
            <a href="ShowAllViews.aspx" target="_blank">查看所有的视图&gt;&gt;&gt;</a><br />
            <br />
            <a href="SelectPorvider.aspx" target="_blank">重新选择一个数据库。</a><br />
            <br />
            <br />
            <br />
            <a href="EditTableInfo.aspx">修改数据库、表的中文名称，或者添加一些注释信息。。。</a><br />
            <br />
            <br />
            <a href="Sets/Template.aspx" target="_blank">功能设置-模板设置</a>
            <br />
            <br />
            <br />

        </div>
    </form>
</body>
</html>
