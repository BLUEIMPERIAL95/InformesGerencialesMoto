<%@ Page Language="VB" AutoEventWireup="false" CodeFile="BusquedaComprobanteCad.aspx.vb" Inherits="BusquedaComprobanteCad" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 82%;
        }
        txtOrden
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="display:none;">
        <asp:HiddenField ID="hidcad" Value="0" runat="server" />
        <asp:HiddenField ID="hidestado" Value="0" runat="server" />
        <asp:HiddenField ID="hiddetcad" Value="0" runat="server" />
        <asp:HiddenField ID="hidsucursal" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <thead>
                <tr>
                    <th colspan="6" valign="top" align="center">
                        <asp:Label ID="lblFacturaEncabezado" runat="server" Text="BUSQUEDA COMPROBANTE CAD" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                    <asp:Label ID="lblDatos" runat="server" Text="Filtros Busqueda"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="Label2" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboEmpresa" Width="90%" runat="server" AutoPostBack="true">
                        
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
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCadEnvio" runat="server" Text="Nro Comprobante *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtNumero" runat="server" MaxLength="100" Width="50%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label1" runat="server" Text="Tipo Comprobante *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:DropDownList ID="cboTipo" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="Egresos">Egresos</asp:ListItem>
                        <asp:ListItem Value="Facturas">Facturas</asp:ListItem>
                        <asp:ListItem Value="Manifiestos">Manifiestos</asp:ListItem>
                        <asp:ListItem Value="Notas">Notas</asp:ListItem>
                        <asp:ListItem Value="Recibos">Recibos</asp:ListItem>
                        <asp:ListItem Value="Mtu">Mtu</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td align="center" align="left" style="width: 100%;" colspan="4">
                    <asp:Label ID="lblRespuesta" runat="server" Text="" ForeColor="Red" Font-Size ="18" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Buscar" />
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
