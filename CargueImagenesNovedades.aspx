<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CargueImagenesNovedades.aspx.vb" Inherits="CargueImagenesNovedades" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="Styles/SAGERStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="display:none;">
            <asp:HiddenField ID="hidDocumento" Value="0" runat="server" />
            <asp:HiddenField ID="hidImagen" Value="0" runat="server" />
        </div>
        <div>
            <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="500px" runat="server" id="tbl_datos">
                <thead>
                    <tr>
                        <th colspan="4" valign="top" align="center">
                            <asp:Label ID="Label1" runat="server" Text="CARGUE IMAGENES SOLO EXTENSION JPEG" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td style="width: 35%;" align="center">
                        <asp:Image ID="imgEquipo" runat="server" Width="150px" Height="100px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center" style="width: 35%;">
                        <asp:FileUpload ID="fluImagen" runat="server" Width="74px" OnClick="btnCargarArchivo_Click" />
                        <%--<asp:Button ID="btnSalvarImagen" runat="server" Text="G" />--%>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center" style="width: 15%;">
                        <asp:ImageButton ID="imgAnterior" runat="server" ImageUrl="images/anterior.png" ToolTip="Imagen Anterior" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:ImageButton ID="imgSiguiente" runat="server" ImageUrl="images/siguiente.png" ToolTip="Imagen Siguiente" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top" align="center">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
