<%@ Page Language="vb" AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxCheckBox ID="EnableResizingCheckBox" runat="server" AutoPostBack="true" Text="Enable Resizing" />

        <dx:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" DataSourceID="ProductsDataSource" ClientInstanceName="grid" Width="100%" OnInit="Grid_Init">
            <Columns>
                <dx:GridViewDataTextColumn FieldName="ProductID" />
                <dx:GridViewDataTextColumn FieldName="ProductName" />
                <dx:GridViewDataComboBoxColumn Caption="Category" FieldName="CategoryID" GroupIndex="0">
                    <PropertiesComboBox DataSourceID="CategoriesDataSource" TextField="CategoryName" ValueField="CategoryID" ValueType="System.Int32" />
                </dx:GridViewDataComboBoxColumn>
                <dx:GridViewDataTextColumn FieldName="SupplierID" GroupIndex="1" />
                <dx:GridViewDataTextColumn FieldName="QuantityPerUnit" />
                <dx:GridViewDataTextColumn FieldName="UnitPrice" />
                <dx:GridViewDataTextColumn FieldName="UnitsOnOrder" />
                <dx:GridViewDataTextColumn FieldName="ReorderLevel" />
                <dx:GridViewDataCheckColumn FieldName="Discontinued" />
            </Columns>
            <Settings ShowGroupPanel="true" />
            <SettingsPager PageSize="20" />
            <GroupSummary>
                <dx:ASPxSummaryItem FieldName="UnitPrice" SummaryType="Min" />
                <dx:ASPxSummaryItem FieldName="UnitPrice" SummaryType="Max" />
                <dx:ASPxSummaryItem FieldName="UnitsOnOrder" SummaryType="Max" />
                <dx:ASPxSummaryItem FieldName="ProductID" SummaryType="Count" />
            </GroupSummary>
            <ClientSideEvents Init="Grid_Init" EndCallback="Grid_EndCallback" ColumnResized="Grid_ColumnResized" />
            <Styles>
                <Header CssClass="gridHeader" />
                <GroupRow CssClass="gridGroupRow" />
            </Styles>
        </dx:ASPxGridView>

        <asp:AccessDataSource ID="ProductsDataSource" runat="server" DataFile="~/App_Data/nwind.mdb" SelectCommand="SELECT * FROM [Products]" />
        <asp:AccessDataSource ID="CategoriesDataSource" runat="server" DataFile="~/App_Data/nwind.mdb" SelectCommand="SELECT * FROM [Categories]" />
    </form>
</body>
</html>