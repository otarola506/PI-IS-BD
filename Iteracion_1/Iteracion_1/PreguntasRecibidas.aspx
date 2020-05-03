﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreguntasRecibidas.aspx.cs" Inherits="PreguntasWebForms.PreguntasRecibidas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <style>
        .roundedCorner{
            font-size:11pt;
            margin-left:auto;
            margin-right:auto;
            margin-top:1px;
            margin-bottom:1px;
            padding:3px;
            border-top:1px solid;
            border-left:1px solid;
            border-right:1px solid;
            border-bottom:1px solid;
            -moz-border-radius: 8px;
            -webkit-border-radius: 8px;
        }

        .background {
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .popup{
            background-color: aqua;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 400px;
            height: 300px;
        }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Label ID="Label1" runat="server" Text="Preguntas"></asp:Label>
        <asp:HiddenField ID="hfPregID" runat="server" />
        <p>
            <asp:GridView ID="gvPreguntas" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="pregIdPK" HeaderText="PregID" />
                    <asp:BoundField DataField="pregunta" HeaderText="Preguntas" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="FrecButton" runat="server" CommandArgument = '<%# Eval("pregIdPK") %>' OnClick="FButton_OnClick" Text="Agregar a Frecuentes" />
                            <asp:Button ID="EditButton" runat="server" CommandArgument = '<%# Eval("pregIdPK") %>' OnClick="EButton_OnClick" Text="Editar" />
                            <asp:Button ID="DescButton" runat="server" CommandArgument = '<%# Eval("pregIdPK") %>' OnClick="DButton_OnClick" Text="Descartar" />
                        </ItemTemplate>                       
                    </asp:TemplateField>                                
                </Columns>
            </asp:GridView>
        </p>
    </form>
</body>
</html>
