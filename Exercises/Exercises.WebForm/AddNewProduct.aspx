<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewProduct.aspx.cs" Inherits="Exercises.WebForm.AddNewProduct" %>--%>
<%@ Page Title="Add New Product" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewProduct.aspx.cs" Inherits="Exercises.WebForm.AddNewProduct" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Add New Product</h2>
    
    <%-- submit button --%>

    <div class="row">
        <div class="col-md-2">
            <p>Product name:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="ProductName" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <p>Supplier:</p>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="SupplierID" runat="server" DataSourceID="SupplierRecordset" DataTextField="CompanyName" DataValueField="SupplierID"></asp:DropDownList>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-2">
            <p>Category:</p>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="CategoryID" runat="server" DataSourceID="CatListRecordset" DataTextField="CategoryName" DataValueField="CategoryID">
            </asp:DropDownList>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-2">
            <p>Quantity per unit:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="QuantityPerUnit" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <p>Unit price:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="UnitPrice" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <p>Units in stock:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="UnitsInStock" runat="server"></asp:TextBox>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-2">
            <p>Units on order:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="UnitsOnOrder" runat="server"></asp:TextBox>
        </div>
    </div>
    
    <div class="row">
        <div class="col-md-2">
            <p>Reorder level:</p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="ReorderLevel" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row">
        <div class="col-md-2">
            <p>Discontinued:</p>
        </div>
        <div class="col-md-3">
            <asp:CheckBox ID="Discontinued" runat="server" />
        </div>
    </div>
    
    <%-- button --%>
    
    <div class="row">
        <div class="col-md-2">
            <%-- empty cell, align button under input --%>
        </div>
        <div class="col-md-3">
            <%-- key word here is OnClick, it will then as to create a method for you... ---%>
            <asp:Button ID="ButtonAddProduct" runat="server" Text="Add Product" OnClick="ButtonAddProduct_OnClick"/>
        </div>
    </div>
    
    <br/>

    <asp:GridView ID="ProductGridView" runat="server" AutoGenerateColumns="False" DataKeyNames="ProductID" DataSourceID="ProductSQL">
        <Columns>
            <asp:BoundField DataField="ProductID" HeaderText="ProductID" InsertVisible="False" ReadOnly="True" SortExpression="ProductID" />
            <asp:BoundField DataField="ProductName" HeaderText="ProductName" SortExpression="ProductName" />
            <asp:BoundField DataField="SupplierID" HeaderText="SupplierID" SortExpression="SupplierID" />
            <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" SortExpression="CategoryID" />
            <asp:BoundField DataField="QuantityPerUnit" HeaderText="QuantityPerUnit" SortExpression="QuantityPerUnit" />
            <asp:BoundField DataField="UnitPrice" HeaderText="UnitPrice" SortExpression="UnitPrice" />
            <asp:BoundField DataField="UnitsInStock" HeaderText="UnitsInStock" SortExpression="UnitsInStock" />
            <asp:BoundField DataField="UnitsOnOrder" HeaderText="UnitsOnOrder" SortExpression="UnitsOnOrder" />
            <asp:BoundField DataField="ReorderLevel" HeaderText="ReorderLevel" SortExpression="ReorderLevel" />
            <asp:CheckBoxField DataField="Discontinued" HeaderText="Discontinued" SortExpression="Discontinued" />
        </Columns>
    </asp:GridView>

<%-- data source --%>

<asp:SqlDataSource ID="CatListRecordset" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [CategoryID], [CategoryName] FROM [Categories]"></asp:SqlDataSource>
<asp:SqlDataSource ID="SupplierRecordset" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [SupplierID], [CompanyName] FROM [Suppliers]"></asp:SqlDataSource>
<asp:SqlDataSource ID="AddNewProductSQL" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" InsertCommand="INSERT INTO Products(ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued) VALUES (@ProductName, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @UnitsInStock, @UnitsOnOrder, @ReorderLevel, @Discontinued)">
    <InsertParameters>
        <asp:ControlParameter ControlID="ProductName" Name="ProductName" PropertyName="Text" />
        <asp:ControlParameter ControlID="SupplierID" Name="SupplierID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="CategoryID" Name="CategoryID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="QuantityPerUnit" Name="QuantityPerUnit" PropertyName="Text" />
        <asp:ControlParameter ControlID="UnitPrice" Name="UnitPrice" PropertyName="Text" />
        <asp:ControlParameter ControlID="UnitsInStock" Name="UnitsInStock" PropertyName="Text" />
        <asp:ControlParameter ControlID="UnitsOnOrder" Name="UnitsOnOrder" PropertyName="Text" />
        <asp:ControlParameter ControlID="ReorderLevel" Name="ReorderLevel" PropertyName="Text" />
        <asp:ControlParameter ControlID="Discontinued" Name="Discontinued" PropertyName="Checked" />
    </InsertParameters>
</asp:SqlDataSource>
    <asp:SqlDataSource ID="ProductSQL" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [Products]"></asp:SqlDataSource>

</asp:Content>
