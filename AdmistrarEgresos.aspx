<%@ Page Language="VB" AutoEventWireup="false" CodeFile="AdmistrarEgresos.aspx.vb" Inherits="AdmistrarEgresos" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

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
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="headercertificados">
            <div class="title">
                
            </div>
            <div class="loginDisplay">
                <table cellpadding="2" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="center" style="width:80%;">
                            <b><font color="white" size="80px">MOTOTRANSPORTAR S.A.S</font></b>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="display:none;">
        <asp:HiddenField ID="hidcambiadero" Value="0" runat="server" />
        <asp:HiddenField ID="hidsucursal" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="lblFacturaEncabezado" runat="server" Text="ADMINISTRAR EGRESOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        &nbsp;&nbsp;
                        <%--<asp:ImageButton ID="imgPdf" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Cad Envío" />--%>
                        <%--&nbsp;&nbsp;
                        <asp:ImageButton ID="imgPdfCopia" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Copia" />
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="imgChequeCompleto" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Cheque Completo" />--%>
                    </th>
                </tr>
            </thead>
            <tr>
                <td colspan="6" valign="top" align="center" style="width: 10%;">
                    <asp:Label ID="lblDatos" runat="server" Text="Ingresar Datos"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="Label2" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboEmpresa" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="3" Selected="True">Mototransportamos</asp:ListItem>
                        <asp:ListItem Value="4">Refrilogistica</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblAgencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboAgencia" Width="90%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <%--<tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label4" runat="server" Text="Medio Pago *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:DropDownList ID="cboMedioPago" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="CAJA GENERAL">CAJA GENERAL</asp:ListItem>
                        <asp:ListItem Value="CHEQUE">CHEQUE</asp:ListItem>
                        <asp:ListItem Value="EFECTIVO">EFECTIVO</asp:ListItem>
                        <asp:ListItem Value="TRANSFERENCIA">TRANSFERENCIA</asp:ListItem>
                        <asp:ListItem Value="VALE CAMBIADERO">VALE CAMBIADERO</asp:ListItem>
                        <asp:ListItem Value="VALE CORRESPONSAL">VALE CORRESPONSAL</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label1" runat="server" Text="Porcentaje *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:DropDownList ID="cboPorcentaje" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="0">0</asp:ListItem>
                        <asp:ListItem Value="0.10">0.1</asp:ListItem>
                        <asp:ListItem Value="0.20">0.2</asp:ListItem>
                        <asp:ListItem Value="0.30">0.3</asp:ListItem>
                        <asp:ListItem Value="0.40">0.4</asp:ListItem>
                        <asp:ListItem Value="0.50">0.5</asp:ListItem>
                        <asp:ListItem Value="0.60">0.6</asp:ListItem>
                        <asp:ListItem Value="0.70">0.7</asp:ListItem>
                        <asp:ListItem Value="0.80">0.8</asp:ListItem>
                        <asp:ListItem Value="0.90">0.9</asp:ListItem>
                        <asp:ListItem Value="1.00">1</asp:ListItem>
                        <asp:ListItem Value="1.10">1.1</asp:ListItem>
                        <asp:ListItem Value="1.20">1.2</asp:ListItem>
                        <asp:ListItem Value="1.30">1.3</asp:ListItem>
                        <asp:ListItem Value="1.40">1.4</asp:ListItem>
                        <asp:ListItem Value="1.50">1.5</asp:ListItem>
                        <asp:ListItem Value="1.60">1.6</asp:ListItem>
                        <asp:ListItem Value="1.70">1.7</asp:ListItem>
                        <asp:ListItem Value="1.80">1.8</asp:ListItem>
                        <asp:ListItem Value="1.90">1.9</asp:ListItem>
                        <asp:ListItem Value="2.00">2</asp:ListItem>
                        <asp:ListItem Value="2.10">2.1</asp:ListItem>
                        <asp:ListItem Value="2.20">2.2</asp:ListItem>
                        <asp:ListItem Value="2.30">2.3</asp:ListItem>
                        <asp:ListItem Value="2.40">2.4</asp:ListItem>
                        <asp:ListItem Value="2.50">2.5</asp:ListItem>
                        <asp:ListItem Value="2.60">2.6</asp:ListItem>
                        <asp:ListItem Value="2.70">2.7</asp:ListItem>
                        <asp:ListItem Value="2.80">2.8</asp:ListItem>
                        <asp:ListItem Value="2.90">2.9</asp:ListItem>
                        <asp:ListItem Value="3.00">3</asp:ListItem>
                        <asp:ListItem Value="3.10">3.1</asp:ListItem>
                        <asp:ListItem Value="3.20">3.2</asp:ListItem>
                        <asp:ListItem Value="3.30">3.3</asp:ListItem>
                        <asp:ListItem Value="3.40">3.4</asp:ListItem>
                        <asp:ListItem Value="3.50">3.5</asp:ListItem>
                        <asp:ListItem Value="3.60">3.6</asp:ListItem>
                        <asp:ListItem Value="3.70">3.7</asp:ListItem>
                        <asp:ListItem Value="3.80">3.8</asp:ListItem>
                        <asp:ListItem Value="3.90">3.9</asp:ListItem>
                        <asp:ListItem Value="4.00">4</asp:ListItem>
                        <asp:ListItem Value="4.10">4.1</asp:ListItem>
                        <asp:ListItem Value="4.20">4.2</asp:ListItem>
                        <asp:ListItem Value="4.30">4.3</asp:ListItem>
                        <asp:ListItem Value="4.40">4.4</asp:ListItem>
                        <asp:ListItem Value="4.50">4.5</asp:ListItem>
                        <asp:ListItem Value="4.60">4.6</asp:ListItem>
                        <asp:ListItem Value="4.70">4.7</asp:ListItem>
                        <asp:ListItem Value="4.80">4.8</asp:ListItem>
                        <asp:ListItem Value="4.90">4.9</asp:ListItem>
                        <asp:ListItem Value="5.00">5</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblEgreso" runat="server" Text="Nro Egreso *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtNumero" runat="server" MaxLength="100" Width="50%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="B" Width="10%" />
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblValor" runat="server" Text="Valor *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtValor" runat="server" MaxLength="100" Width="50%" Enabled="false" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblReferencia" runat="server" Text="Fecha *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtReferencia" runat="server" MaxLength="100" Width="50%" Enabled="false"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label5" runat="server" Text="Tercero *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtTercero" runat="server" MaxLength="200" Width="50%" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblMovimiento" runat="server" Text="Movimiento "></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtMovimiento" runat="server" MaxLength="100" Width="50%" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <%--<tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblDescuento" runat="server" Text="Descuento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtDescuento" runat="server" MaxLength="100" Width="50%" Enabled="false" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label3" runat="server" Text="Total *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtTotal" runat="server" MaxLength="100" Width="50%" Enabled="false" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblReferencia" runat="server" Text="Referencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtReferencia" runat="server" MaxLength="100" Width="50%"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label5" runat="server" Text="Tercero *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtTercero" runat="server" MaxLength="200" Width="50%" Enabled="false"></asp:TextBox>
                </td>
            </tr>--%>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                    <asp:Button ID="btnInforme" runat="server" Text="Informe" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 95%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridCambiadero" runat="server" DataKeyNames="id_adca" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombre_emor" HeaderText="Empresa" />
                                <asp:BoundField DataField="nombre_agcc" HeaderText="Agencia" />
                                <asp:BoundField DataField="egreso_adca" HeaderText="Egreso" />
                                <asp:BoundField DataField="fecha_egreso" HeaderText="Fecha" />
                                <asp:BoundField DataField="tercero_adca" HeaderText="Tercero" />
                                <asp:BoundField DataField="movimiento_adca" HeaderText="Movimiento" />
                                <asp:BoundField DataField="valor_adca" HeaderText="Valor" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="usuario" HeaderText="Usuario" />
                                <%--<asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />--%>
                                <%--<asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
