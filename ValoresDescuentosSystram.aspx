<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ValoresDescuentosSystram.aspx.vb" Inherits="ValoresDescuentosSystram" MasterPageFile="~/Site.Master" %>

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

    <script type="text/javascript">
        function fnDatos() {
            $('body').RunAbriPopupRese();
        }

        jQuery.fn.RunAbriPopupRese = function (strHora, intSede, intEspacio, intTipo, strClase1, strClase2, strFecha, intRese) {
            $('<div id="frmModalPopup" class="frmModalPopup" align="center"><iframe frameborder=0 src="ImportacionValoresDescuentosSystram.aspx" width="800" height="400" scrolling="no"/></div>').dialog({ modal: true, width: 800, height: 400, close: function (event, ui) { var button; button = document.getElementById("MainContent_btnOculto"); button.click(); } });
            return false;
        };
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="display:none;">
        <asp:HiddenField ID="hidDescuento" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <thead>
                <tr>
                    <th colspan="6" valign="top" align="center">
                        <asp:Label ID="lblFacturaEncabezado" runat="server" Text="VALORES DESCUENTOS SYSTRAM AÑO-MES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                    <asp:Label ID="lblPlaca" runat="server" Text="Placa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:TextBox ID="txtPlaca" runat="server" MaxLength="6" Width="50%" style="text-transform:uppercase;"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 10%;">
                    <asp:Label ID="lblDocumento" runat="server" Text="Documento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 23%;">
                    <asp:TextBox ID="txtDocumento" runat="server" MaxLength="20" Width="50%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 10%;">
                    <asp:Label ID="lblDescuento" runat="server" Text="Descuento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 24%;">
                    <asp:DropDownList ID="cboDescuento" Width="90%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblMes" runat="server" Text="Mes *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboMes" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">1 - Enero</asp:ListItem>
                        <asp:ListItem Value="2">2 - Febrero</asp:ListItem>
                        <asp:ListItem Value="3">3 - Marzo</asp:ListItem>
                        <asp:ListItem Value="4">4 - Abril</asp:ListItem>
                        <asp:ListItem Value="5">5 - Mayo</asp:ListItem>
                        <asp:ListItem Value="6">6 - Junio</asp:ListItem>
                        <asp:ListItem Value="7">7 - Julio</asp:ListItem>
                        <asp:ListItem Value="8">8 - Agosto</asp:ListItem>
                        <asp:ListItem Value="9">9 - Septiembre</asp:ListItem>
                        <asp:ListItem Value="10">10 - Octubre</asp:ListItem>
                        <asp:ListItem Value="11">11 - Noviembre</asp:ListItem>
                        <asp:ListItem Value="12">12 - Diciembre</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblAño" runat="server" Text="Año *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboAño" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="2022">2022</asp:ListItem>
                        <asp:ListItem Value="2023">2023</asp:ListItem>
                        <asp:ListItem Value="2024">2024</asp:ListItem>
                        <asp:ListItem Value="2025">2025</asp:ListItem>
                        <asp:ListItem Value="2026">2026</asp:ListItem>
                        <asp:ListItem Value="2027">2027</asp:ListItem>
                        <asp:ListItem Value="2028">2028</asp:ListItem>
                        <asp:ListItem Value="2029">2029</asp:ListItem>
                        <asp:ListItem Value="2030">2030</asp:ListItem>
                        <asp:ListItem Value="2031">2031</asp:ListItem>
                        <asp:ListItem Value="2032">2032</asp:ListItem>
                        <asp:ListItem Value="2033">2033</asp:ListItem>
                        <asp:ListItem Value="2034">2034</asp:ListItem>
                        <asp:ListItem Value="2035">2035</asp:ListItem>
                        <asp:ListItem Value="2036">2036</asp:ListItem>
                        <asp:ListItem Value="2037">2037</asp:ListItem>
                        <asp:ListItem Value="2038">2038</asp:ListItem>
                        <asp:ListItem Value="2039">2039</asp:ListItem>
                        <asp:ListItem Value="2040">2040</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 10%;">
                    <asp:Label ID="lblValor" runat="server" Text="Valor *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 23%;">
                    <asp:TextBox ID="txtValor" runat="server" MaxLength="20" Width="50%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="6" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                    <asp:Button ID="btnRecargar" runat="server" Text="Recargar" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 95%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridDescuentosSystram" runat="server" DataKeyNames="id_vads" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="placa_vads" HeaderText="Placa" />
                                <asp:BoundField DataField="documento_vads" HeaderText="Documento" />
                                <asp:BoundField DataField="nombre_desy" HeaderText="Descuento" />
                                <asp:BoundField DataField="mes_vads" HeaderText="Mes" />
                                <asp:BoundField DataField="año_vads" HeaderText="Año" />
                                <asp:BoundField DataField="valor_desy" HeaderText="Valor" DataFormatString="{0:C}" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divOcultos" style="display: none;">
        <asp:Button ID="btnOculto" runat="server" Text="Button" />
        <asp:TextBox ID="txtFechaActual" runat="server" MaxLength="20" />
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
