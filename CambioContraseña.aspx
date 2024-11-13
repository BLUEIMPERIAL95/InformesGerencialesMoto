<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CambioContraseña.aspx.vb" Inherits="CambioContraseña" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/SAGERStyle.css" rel="stylesheet" type="text/css" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
    <%--<link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="500px" runat="server" id="tbl_datos">
                <thead>
                    <tr>
                        <th colspan="2" valign="top" align="center">
                            <asp:Label ID="Label1" runat="server" Text="ACTUALIZAR CONTRASEÑA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 25%;">
                        <asp:Label ID="Label8" runat="server" Text="." ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 25%;">
                        <asp:Label ID="Label3" runat="server" Text="Digitar una contraseña válida de 8 caracteres. Con mínimo una letra mayúscula, minúscula, un número y un caracter especial."></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 25%;">
                        <asp:Label ID="Label5" runat="server" Text="." ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 25%;">
                        <asp:Label ID="Label6" runat="server" Text="." ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 25%;">
                        <asp:Label ID="Label7" runat="server" Text="." ForeColor="White"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 25%;">
                        <asp:Label ID="lblContraseña" runat="server" Text="Digite Nueva Contraseña *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 25%;">
                        <asp:TextBox ID="txtContraseña" runat="server" MaxLength="8" Width="80%" TextMode="Password" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 25%;">
                        <asp:Label ID="Label2" runat="server" Text="Confirmar Nueva Contraseña  *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 25%;">
                        <asp:TextBox ID="txtContraseña2" runat="server" MaxLength="8" Width="80%" TextMode="Password" autocomplete="off"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top" align="center" style="width: 15%;">
                        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
