<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CitasCargueDescargueIngresarDatos.aspx.vb" Inherits="CitasCargueDescargueIngresarDatos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/SAGERStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px" runat="server" id="tbl_datos">
                <thead>
                    <tr>
                        <th colspan="4" valign="top" align="center">
                            <asp:Label ID="Label1" runat="server" Text="DATOS CITA CARGUE DESCARGUE" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblDocumento" runat="server" Text="Documento *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtDocumento" runat="server" MaxLength="20" Width="90%"></asp:TextBox>
                    </td>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblNombre" runat="server" Text="Nombre *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="Label6" runat="server" Text="Empresa *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtEmpresa" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                    </td>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="Label2" runat="server" Text="Placa *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtPlaca" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="Label7" runat="server" Text="Tipo Veh. *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtTipoVehiculo" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                    </td>
                   <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblHora" runat="server" Text="Hora *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:DropDownList ID="cboHora" Width="90%" runat="server" Enabled="false">
                        
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblDuracion" runat="server" Text="Duracion *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:DropDownList ID="cboDuracion" Width="90%" runat="server">
                        
                        </asp:DropDownList>
                    </td>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblDocTrans" runat="server" Text="Doc Trans *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtDocTrans" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblPeso" runat="server" Text="Peso(Ton) *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtpeso" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
                    </td>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="lblCantidad" runat="server" Text="Cant CJ/BD/BL *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:TextBox ID="txtCantidad" runat="server" MaxLength="100" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top" align="center" style="width: 10%;">
                        <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top" align="left" style="width: 95%;">
                        <asp:TextBox ID="txtObservacion" runat="server" MaxLength="3000" Width="99%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top" align="center">
                        <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px" visible="false" runat="server" id="tbl_cumplir_cancelar">
                <thead>
                    <tr>
                        <th colspan="4" valign="top" align="center">
                            <asp:Label ID="Label3" runat="server" Text="CUMPLIMIENTO O CANCELACIÓN DE CITA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="Label5" runat="server" Text="Tipo *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:DropDownList ID="cboTipoTarea" Width="90%" runat="server" AutoPostBack="true">
                            <asp:ListItem Value="0" Selected>- SELECCIONE -</asp:ListItem>
                            <asp:ListItem Value="1">CUMPLIR</asp:ListItem>
                            <asp:ListItem Value="2">CANCELAR</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td valign="top" align="left" style="width: 15;">
                        <asp:Label ID="Label4" runat="server" Text="Duracion Real *"></asp:Label>
                    </td>
                    <td valign="top" align="left" style="width: 35%;">
                        <asp:DropDownList ID="cboDuracionReal" Width="90%" runat="server">
                        
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" valign="top" align="center">
                        <asp:Button ID="btnCumplir" runat="server" Text="Salvar" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
