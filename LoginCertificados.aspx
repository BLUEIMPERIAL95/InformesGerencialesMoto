<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LoginCertificados.aspx.vb" Inherits="LoginCertificados" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <asp:Literal ID="PageTitleLabel" runat="server" Text="Login"></asp:Literal>
    </title>
    <asp:Literal ID="JSScriptsLiteral" runat="server"></asp:Literal>
    <link href="Styles/LOGINStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
</head>
<body id="login-page-body">
    <form id="thisForm" runat="server">
        <div>
            <table border="0" align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="SiteTypeLabel" CssClass="LabelMensajesWarning" runat="server" Text=""></asp:Label>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="middle" height="450px">
                        <div id="login">
                            <h1>&nbsp;</h1>
                            <h2>
                            <br /> 
                            <span style="color:chocolate;"><asp:Literal ID="Literal1" runat="server" Text="Introduzca en usuario su # de documento(Si es nit sin digito de verificación)."></asp:Literal></span></h2>
                            <span style="color:chocolate;"><asp:Literal ID="LoginText" runat="server" Text="En contraseña las letras cert y posterior su # de documento."></asp:Literal></span></h2>
                            <asp:TextBox ID="txtLogin" runat="server" Text="" MaxLength="20" Width="90px"></asp:TextBox>
                            <asp:TextBox ID="txtPass" TextMode="Password" Text="" runat="server" MaxLength="20" Width="90px"></asp:TextBox>
                            <asp:Button ID="OkButton" runat="server" Text="Ingresar" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <%--<div style="display: none;">
            <asp:TextBox ID="ChallengeTextBox" runat="server" Text="" MaxLength="300" />
            <asp:TextBox ID="DataTextBox" runat="server" Text="" MaxLength="1000" />
            <asp:Button ID="btnOculto" runat="server" Text="Button" 
                onclick="btnOculto_Click" />
        </div>--%>
    </form>
    <div id="footer">
        <div id="left" style="color: black;">
            <asp:Literal ID="VersionLiteral" runat="server" Text=""></asp:Literal>
        </div>
        <div id="right" style="color: black;">
            &copy; 2021 Mototransportar S.A.S.
        </div>
    </div>
</body>
</html>
