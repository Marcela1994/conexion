<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="LoginWebService._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <div class="mb-3">
            <asp:Label for="exampleInputEmail1" class="form-label">USUARIO</asp:Label>
            <asp:TextBox CssClass="form-control" ID="userName" runat="server"></asp:TextBox>
        </div>
        <div class="mb-3">
            <asp:Label>CONTRASEÑA</asp:Label>
            <asp:TextBox type="password" CssClass="form-control" ID="passUser" runat="server"></asp:TextBox>
        </div>
        <asp:Button OnClick="login_Click" Text="Ingresar" CssClass="btn btn-primary" runat="server" />
    </form>
</asp:Content>
