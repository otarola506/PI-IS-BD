﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusquedaTopico.aspx.cs" Inherits="Iteracion_1.BusquedaTopico" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" method="post">
        <div>
        </div>
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="SqlDataSource1" DataTextField="nombre" DataValueField="nombre" style="z-index: 1; left: 299px; top: 44px; position: absolute">
        </asp:DropDownList>
        <asp:Button ID="Btn_ConsultaXTopico" runat="server" OnClick="Button1_Click" style="z-index: 1; left: 425px; top: 42px; position: absolute" Text="Consulta categoria" />
        <asp:GridView ID="tabla" runat="server" style="top: 160px; left: 33px; position: absolute; height: 152px; width: 232px">
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:grupo2Conn %>" SelectCommand="SELECT [nombre] FROM [Categoria] ORDER BY [nombre]"></asp:SqlDataSource>
        <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged" style="z-index: 1; left: 28px; top: 97px; position: absolute"></asp:TextBox>
        <asp:Button ID="BusquedaXTitulo" runat="server" OnClick="BusquedaXTitulo_Click" style="z-index: 1; left: 249px; top: 92px; position: absolute" Text="Busqueda por titulo" />
    </form>
</body>
</html>
