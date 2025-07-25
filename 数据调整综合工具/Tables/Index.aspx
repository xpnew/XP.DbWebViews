<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="数据调整综合工具.Tables.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>表处理工具</h1>
        <h2>您现在可以对表【<%= TableInfo.ObjectName %>/<%= TableInfo.GlobalName %>】进行以下处理</h2>
    <div>

             <fieldset>
            <legend>表架构调整（列）</legend>
            <ul>
                  <li><a href="AddColumn.aspx?tablename=<%= TableInfo.ObjectName %>">添加列</a></li>
                </ul>
                 </fieldset>


        <ul>
            <li><a href="RowsDataCopy.aspx?tablename=<%= TableInfo.ObjectName %>">复制行数据</a></li>
        </ul>

            <fieldset>
            <legend>各种模板输出</legend>
            <ul>

                <li>   <a href="../Output/SelectTM.aspx?tablename=<%= TableInfo.ObjectName %>">选择一个模板</a>   </li>

            </ul>

        </fieldset>

        <fieldset>
            <legend>IBatis 辅助（XML映射、标准增删改查等等）</legend>
            <ul>

                <li>           <a href="../CreateMapperXml.aspx?tablename=<%= TableInfo.ObjectName %>">映射文件</a>      </li>

            </ul>

        </fieldset>







        
        <h3><a href="../Default.aspx">返回首页</a></h3>
    </div>
    </form>
</body>
</html>
