<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template.aspx.cs" Inherits="数据调整综合工具.Sets.Template" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <link href="../css/comm.css" rel="stylesheet" />
    <link href="../css/base.css" rel="stylesheet" />
    <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../Scripts/comm.js"></script>


    <script type="text/javascript">

        $(function () {
            $('#DetailsView1 td select').each(function(){
                $(this).siblings('input').val($(this).val());
            });
            $('#DetailsView1 td select').change(function(){
                $(this).siblings('input').val( $(this).val());
            
            });
        });
            </script>
</head>
<body>
    <form id="form1" runat="server">
        <p>
           <a href="Template_Edit.aspx" target="_blank">添加&gt;&gt;&gt;&gt;</a> </p>
        <p>
            <br />
            <asp:GridView ID="GridView1" runat="server" CssClass="tb01" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" OnRowDataBound="GridView1_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                    <asp:BoundField DataField="Tit" HeaderText="Tit" SortExpression="Tit" />
                    <asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary" />
                    <asp:BoundField DataField="Cot" HeaderText="Cot" SortExpression="Cot" />
                    <asp:BoundField DataField="FileNameTemplate" HeaderText="FileNameTemplate" SortExpression="FileNameTemplate" />
                    <asp:BoundField DataField="CreateTime" HeaderText="CreateTime" SortExpression="CreateTime" />
                    <asp:BoundField DataField="UpdateTime" HeaderText="UpdateTime" SortExpression="UpdateTime" />
                    <asp:BoundField DataField="LoopType" HeaderText="LoopType" SortExpression="LoopType" />
                    <asp:BoundField DataField="OutType" HeaderText="OutType" SortExpression="OutType" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="Template_Edit.aspx?id={0}" DataTextField="Tit" DataTextFormatString="修改[{0}]" HeaderText="修改" Target="_blank" />
                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="?act=del&amp;id={0}" HeaderText="删除" Text="删除" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetAll" TypeName="XP.DBTools.BLL.TemplateBLL"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetALL" TypeName="XP.DBTools.BLL.TemplateBLL"></asp:ObjectDataSource>
        </p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
        <p>
            &nbsp;</p>
    <div>
    
    </div>
    </form>
</body>
</html>
