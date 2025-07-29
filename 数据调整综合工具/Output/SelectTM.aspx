<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectTM.aspx.cs" Inherits="数据调整综合工具.Output.SelectTM" %>

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
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    <asp:BoundField DataField="Tit" HeaderText="Tit" SortExpression="Tit" />
                    <asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary" />
                    <asp:BoundField DataField="FileNameTemplate" HeaderText="FileNameTemplate" SortExpression="FileNameTemplate" />
                    <asp:BoundField DataField="UpdateTime" HeaderText="UpdateTime" SortExpression="UpdateTime" />
                    <asp:BoundField DataField="LoopType" HeaderText="LoopType" SortExpression="LoopType" />
                    <asp:BoundField DataField="OutType" HeaderText="OutType" SortExpression="OutType" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="?act=go&id={0}" DataTextField="Tit" DataTextFormatString="生成[{0}]" HeaderText="生成" Target="_blank" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="SayTables2File.aspx?id={0}" DataTextField="Tit" DataTextFormatString="输出[{0}]" HeaderText="输出文件" Target="_blank" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <a href="<%# BuildUrl( (XP.DB.DbEntity.TemplateT)Container.DataItem) %>"  target="_blank">预览</a>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAll" TypeName="XP.DBTools.BLL.TemplateBLL"></asp:ObjectDataSource>
    <div>
    
    </div>
    </form>
</body>
</html>
