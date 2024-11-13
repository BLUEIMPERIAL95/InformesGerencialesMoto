<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CadEnvioDetalle.aspx.vb" Inherits="CadEnvioDetalle" MasterPageFile="~/Site.Master" %>

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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1150px">
            <thead>
                <tr>
                    <th colspan="6" valign="top" align="center">
                        <asp:Label ID="lblFacturaEncabezado" runat="server" Text="ENCABEZADO ENVÍO CAD" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="imgPdf" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Cad Envío" />
                        <%--&nbsp;&nbsp;
                        <asp:ImageButton ID="imgPdfCopia" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Copia" />
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="imgChequeCompleto" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Cheque Completo" />--%>
                    </th>
                </tr>
            </thead>
            <tr>
                <td colspan="6" valign="top" align="center" style="width: 10%;">
                    <asp:Label ID="lblDatos" runat="server" Text="Datos Varios"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblCadEnvio" runat="server" Text="Número *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:TextBox ID="txtCadEnvio" runat="server" MaxLength="100" Width="50%" Enabled="false" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 70%" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFecha" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFecha"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="Label1" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboEmpresa" Width="90%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblAgencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboAgencia" Width="90%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 8%;">
                    <asp:Label ID="lblDocumento" runat="server" Text="Documento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:DropDownList ID="cboDocumento" Width="90%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                            <asp:ListItem Value="Egresos">Egresos</asp:ListItem>
                            <asp:ListItem Value="Facturas">Facturas</asp:ListItem>
                            <asp:ListItem Value="Notas">Notas</asp:ListItem>
                            <asp:ListItem Value="Recibos">Recibos</asp:ListItem>
                            <asp:ListItem Value="Manifiestos">Manifiestos</asp:ListItem>
                            <asp:ListItem Value="Mtu">Mtu</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="6" valign="top" align="center" style="width: 10%;">
                    <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="6" valign="top" align="left" style="width: 95%;">
                    <asp:TextBox ID="txtObservacion" runat="server" MaxLength="1000" Width="99%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="6" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" style="width: 100%;">
                    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0">
                        <WizardSteps>
                            <asp:WizardStep ID="wzAsignar" runat="server" Title="Asignar documentos">
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1050px">
                                    <thead>
                                        <tr>
                                            <th colspan="5" valign="top" align="center">
                                                <asp:Label ID="Label2" runat="server" Text="ASIGNACION DOCUMENTOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td valign="top" align="center" style="width: 15%;">
                                            <asp:Label ID="lblfecInicio" runat="server" Text="Fecha Inicio"></asp:Label>
                                        </td>
                                        <td valign="top" align="center" style="width: 15%;">
                                            <asp:Label ID="lblfecFin" runat="server" Text="Fecha Fin"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="center" style="width: 15%;">
                                            <input id="txtFechaInicio" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                                            <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_Wizard1_txtFechaInicio"
                                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                                            </obout:Calendar>
                                        </td>
                                        <td valign="top" align="center" style="width: 15%;">
                                            <input id="txtFechaFin" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                                            <obout:Calendar ID="CalendarFechaFin" runat="server" DatePickerMode="true" TextBoxId="MainContent_Wizard1_txtFechaFin"
                                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                                            </obout:Calendar>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td colspan="2" valign="top" align="center" style="width: 15%;">
                                            <asp:Label ID="lblManifiesto" runat="server" Text="Tipo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" valign="top" align="center">
                                            <asp:DropDownList ID="cboTipo" Width="40%" runat="server">
                                                <asp:ListItem Value="-1" Selected>Seleccione</asp:ListItem>
                                                <asp:ListItem Value="0">Completa</asp:ListItem>
                                                <asp:ListItem Value="1">Cartera</asp:ListItem>
                                                <asp:ListItem Value="2">Prejurídica</asp:ListItem>
                                                <asp:ListItem Value="3">Jurídica</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="2" valign="top" align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnGenerar" runat="server" Text="Generar" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" border="0" align="center" width="880px">
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                                <ProgressTemplate>
                                                    <div>
                                                        <img src="images/cortinilla2.gif" alt="Loading" border="0" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1050px" runat="server" id="Table1">
                                    <tr>
                                        <td valign="top" align="center" class="auto-style1">
                                            <div style="width: 95%; height: 333px; overflow: scroll">
                                                <asp:GridView ID="gridDocumentos" runat="server" AutoGenerateColumns="False" CssClass="mGrid" Width="100%" 
                                                        DataKeyNames="numero, Nombre" GridLines="Horizontal" ShowHeader="True" Font-Bold="true" Font-Size="Small">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="" HeaderStyle-BackColor="White">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chelige" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Número" HeaderStyle-BackColor="White" DataField="numero" />
                                                        <asp:BoundField HeaderText="Fecha" HeaderStyle-BackColor="White" DataField="fecha" />
                                                        <asp:BoundField HeaderText="Sucursal" HeaderStyle-BackColor="White" DataField="sucursal" />
                                                        <asp:BoundField HeaderText="Comprobante" HeaderStyle-BackColor="White" DataField="descripcion" />
                                                        <asp:BoundField HeaderText="Tercero" HeaderStyle-BackColor="White" DataField="Nombre" />
                                                        <asp:BoundField HeaderText="Valor" DataFormatString="{0:N}" HeaderStyle-BackColor="White" DataField="valor" />
                                                        <asp:TemplateField HeaderText="Folios" HeaderStyle-BackColor="White" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtFolios" Text="0" runat="server" ItemStyle-HorizontalAlign="Right">
                                                                </asp:TextBox>
                                                                &nbsp;
                                                                <asp:ImageButton ID="imgvalidar" ImageUrl="~/presentacion/imagenes/validar.png" Visible="false" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:ButtonField CommandName="Guardar" ButtonType="Image" ImageUrl="~/images/ver-mas.png" />--%>
                                                    </Columns>
                                                    <%--<HeaderStyle CssClass="gridheader" />
                                                    <AlternatingRowStyle CssClass="gridfilasalt" />
                                                    <RowStyle CssClass="gridfilas" />--%>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="center">                                           
                                            <asp:Button ID="btnSalvarDetalle" runat="server" Text="Salvar" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="wzDetalles" runat="server" Title="Detalles">
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1050px">
                                    <thead>
                                        <tr>
                                            <th colspan="6" valign="top" align="center">
                                                <asp:Label ID="Label6" runat="server" Text="DETALLES ENVÍO CAD" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1050px" runat="server" id="tbl_detalle">
                                    <tr>
                                        <td valign="top" align="center" class="auto-style1">
                                            <div style="width: 95%; height: 333px; overflow: scroll">
                                                <asp:GridView ID="gridDetalle" runat="server" DataKeyNames="id_cade" AutoGenerateColumns="False" CssClass="mGrid" CellPadding="4" GridLines="None" Width="100%" >
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="numcom_cade" HeaderText="Número" />
                                                        <asp:BoundField DataField="tercero_cade" HeaderText="Tercero" />
                                                        <asp:BoundField DataField="folios_cade" HeaderText="# Folios" />
                                                        <asp:ButtonField CommandName="eliminar" HeaderText="Eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                        </WizardSteps>
                    </asp:Wizard>
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
