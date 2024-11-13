<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FacturacionTramitar.aspx.vb" Inherits="FacturacionTramitar" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 82%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidfactura" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="FACTURACION TRAMITAR" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCotizacion" runat="server" Text="Cotizacion *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCotizacion" runat="server" MaxLength="20" Width="100%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblProforma" runat="server" Text="Proforma *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtProforma" runat="server" MaxLength="20" Width="100%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblFactura" runat="server" Text="Factura *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtFactura" runat="server" MaxLength="20" Width="100%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCertificado" runat="server" Text="Certificado *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCertificado" runat="server" MaxLength="20" Width="100%" onkeypress="javascript:return solonumeros(event)" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" AutoPostBack="true" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFecha" AutoPostBack="true"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFechaVence" runat="server" Text="Fecha Vence *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFechaVence" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="Calendar1" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaVence"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblProveedor" runat="server" Text="Proveedor *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboProveedor" Width="490px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblCliente" runat="server" Text="Cliente *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboCliente" Width="490px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblPlaca" runat="server" Text="Placa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboPlaca" Width="160px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblCobertura" runat="server" Text="Cobertura *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboCobertura" Width="155px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblValProv" runat="server" Text="Valor Prov *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtValProv" runat="server" MaxLength="20" Width="100%" Enabled="false" Text="0"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblValor" runat="server" Text="Valor Emp *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtValor" runat="server" MaxLength="20" Width="100%" Enabled="false" Text="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblComision" runat="server" Text="Comision"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtComision" runat="server" MaxLength="20" Width="100%" Enabled="false" Text="0"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblRecibo" runat="server" Text="Recibo"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtRecibo" runat="server" MaxLength="20" Width="100%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblactivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboActivo" Width="40%" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:Button ID="btnCalcular" runat="server" Text="Calcular" />
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 95%; height: 280px; overflow: scroll">
                        <asp:GridView ID="gridFacturas" runat="server" DataKeyNames="id_fact" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="numero_fact" HeaderText="Numero" />
                                <asp:BoundField DataField="placa_vetr" HeaderText="Vehiculo" />
                                <asp:BoundField DataField="fecha_fact" HeaderText="Fecha" />
                                <asp:BoundField DataField="codigo_cotr" HeaderText="Cobertura" />
                                <asp:BoundField DataField="valor_fact" HeaderText="Valor" />
                                <asp:BoundField DataField="strnom1_usua" HeaderText="Usuario" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
