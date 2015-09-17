<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddNewProduct.aspx.cs" Inherits="Exercises.WebForm.AddNewProduct" %>--%>
<%@ Page Title="Add New Product" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddNewProduct.aspx.cs" Inherits="Exercises.WebForm.AddNewProduct" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <h2>Add New Product</h2>
    
    <%-- submit button --%>

    <div class="row">
        <div class="col-md-2">
            <p>Product name: </p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="ProductName" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <p>Product price: </p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="ProductPrice" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <p>Category: </p>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="CategoryID" runat="server" DataSourceID="CatListRecordset" DataTextField="CategoryName" DataValueField="CategoryID">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <p>Units in stock: </p>
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="UnitsInStock" runat="server"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-md-2">
            <p>Discontinued: </p>
        </div>
        <div class="col-md-3">
            <asp:CheckBox ID="Discontinued" runat="server" />
        </div>
    </div>
    
    <%-- empty cell, align button under input --%>
    
    <div class="row">
        <div class="col-md-2">
            <%-- data source --%>
        </div>
        <div class="col-md-3">
            <%-- key word here is OnClick, it will then as to create a method for you... ---%>
            <asp:Button ID="ButtonAddProduct" runat="server" Text="Add Product" OnClick="ButtonAddProduct_OnClick"/>
        </div>
    </div>

<%-- data source --%>

<asp:SqlDataSource ID="CatListRecordset" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT [CategoryID], [CategoryName] FROM [Categories]"></asp:SqlDataSource>

<asp:SqlDataSource ID="AddNewProductSQL" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" InsertCommand="INSERT INTO Products(ProductName, CategoryID, UnitPrice, UnitsInStock, Discontinued) VALUES (@ProductName, @CategoryID, @ProductPrice, @UnitsInStock, @Discontinued)">
    <InsertParameters>
        <asp:ControlParameter ControlID="ProductName" Name="ProductName" PropertyName="Text" />
        <asp:ControlParameter ControlID="CategoryID" Name="CategoryID" PropertyName="SelectedValue" />
        <asp:ControlParameter ControlID="ProductPrice" Name="ProductPrice" PropertyName="Text" />
        <asp:ControlParameter ControlID="UnitsInStock" Name="UnitsInStock" PropertyName="Text" />
        <asp:ControlParameter ControlID="Discontinued" Name="Discontinued" PropertyName="Checked" />
    </InsertParameters>
</asp:SqlDataSource>

</asp:Content>
