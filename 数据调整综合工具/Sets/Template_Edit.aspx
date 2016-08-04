<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Template_Edit.aspx.cs" Inherits="数据调整综合工具.Sets.Template_Edit" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
            <script src="../Scripts/jquery-1.8.3.min.js"></script>
    <script src="../Scripts/comm.js"></script>

    <script type="text/javascript">

        $(function () {
            $('#DetailsView1 td select').each(function () {
                var InputVal = $(this).siblings('input').val();
                if (null != InputVal && '' != InputVal) {
                    $(this).val(InputVal);
                    return;
                }
                $(this).siblings('input').val($(this).val());
            });
            $('#DetailsView1 td select').change(function(){
                var InputVal = $(this).siblings('input').val();
                $(this).siblings('input').val($(this).val());
            });
        });
            </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataSourceID="ObjectDataSource1" DefaultMode="Insert" Height="50px" Width="80%" OnItemInserting="DetailsView1_ItemInserting">
            <Fields>
                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="True" InsertVisible="False" ReadOnly="True" />
                <asp:BoundField DataField="Tit" HeaderText="Tit" SortExpression="Tit">
                <ControlStyle Width="60%" />
                </asp:BoundField>
                <asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary">
                <ControlStyle Width="80%" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Cot" SortExpression="Cot">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Rows="15" Text='<%# Bind("Cot") %>' TextMode="MultiLine" Width="80%"></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Rows="15" Text='<%# Bind("Cot") %>' TextMode="MultiLine" Width="80%"></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Cot") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FileNameTemplate" HeaderText="FileNameTemplate" SortExpression="FileNameTemplate">
                <ControlStyle Width="60%" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateTime" HeaderText="CreateTime" SortExpression="CreateTime" />
                <asp:BoundField DataField="UpdateTime" HeaderText="UpdateTime" SortExpression="UpdateTime" />
                <asp:TemplateField HeaderText="LoopType" SortExpression="LoopType">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="ObjectDataSource2" DataTextField="Value" DataValueField="Key">
                        </asp:DropDownList>

                        <asp:TextBox ID="TextBox_LoopType" runat="server" Text='<%# Bind("LoopType") %>'></asp:TextBox>

                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="ObjectDataSource2" DataTextField="Value" DataValueField="Key">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBox_LoopType" runat="server" Text='<%# Bind("LoopType") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("LoopType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OutType" SortExpression="OutType">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ObjectDataSource3" DataTextField="Value" DataValueField="Key">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBox_OutType" runat="server" Text='<%# Bind("OutType") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <InsertItemTemplate>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="ObjectDataSource3" DataTextField="Value" DataValueField="Key">
                        </asp:DropDownList>
                        <asp:TextBox ID="TextBox_OutType" runat="server" Text='<%# Bind("OutType") %>'></asp:TextBox>
                    </InsertItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("OutType") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ButtonType="Button" ShowEditButton="True" ShowInsertButton="True" />
            </Fields>
        </asp:DetailsView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" DataObjectTypeName="XP.DB.DbEntity.TemplateT" InsertMethod="Insert" SelectMethod="GetItemById" TypeName="XP.DBTools.BLL.TemplateBLL" UpdateMethod="Update">
            <SelectParameters>
                <asp:QueryStringParameter Name="id" QueryStringField="id" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource2" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetTemplateLoopType" TypeName="XP.DBTools.BLL.Enum2ListBLL"></asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjectDataSource3" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetTemplateOutType2" TypeName="XP.DBTools.BLL.Enum2ListBLL"></asp:ObjectDataSource>
        <br />

        
        <fieldset>
            <legend>Tips:更新</legend>
            现已经支持 字段值替换的时候灵活运用首字母的大小写功能了。具体是说，“{TM:ClumnName$FirstLower}”就可以在最终结果上把“PageIndex”变成“pageIndex”.现在支持的后缀是“$FirstLower”和“$FirstUpper”，然后自动选择原来内容的首母小写和大写。

        </fieldset>
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
