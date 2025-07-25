<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RowsDataCopy.aspx.cs" Inherits="数据调整综合工具.Tables.RowsDataCopy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h1>复制行数据</h1>

    <div class="set">

<table width="80%" border="1">
  <tr>
    <td colspan="5">参数设置</td>
  </tr>
  <tr>
    <td>复制参照</td>
    <td>第一字段</td>
    <td>&nbsp;</td>
    <td>
        <select id="Select1" name="D1">
            <option value="=">等于</option>
            <option value="=">大于</option>
            <option value="=">小于</option>
            <option value="=">大于等于</option>
            <option value="=">小于等于</option>
            <option value="=">包含（字符串字段）</option>
        </select></td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>第二字段</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>自增列</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
  <tr>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
    <td>&nbsp;</td>
  </tr>
</table>
    
    </div>

<div id="LogBox">
    <textarea id="DebugBox" rows="40" style="width: 80%"></textarea>

</div>

    </form>
</body>
</html>
