<%@ Page Language="VB" AutoEventWireup="false" CodeFile="viaticosdetalle.aspx.vb" Inherits="viaticosdetalle" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="asp"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style2 {
            width: 14%;
        }
        .auto-style5 {
            width: 95%;
            height: 405px;
            overflow: scroll;
        }
    </style>
    <script type="text/javascript">
        function GetID(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidtercero').value = HdnKey;
        }

        function GetIDSolicita(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidsolicita').value = HdnKey;
        }

        function GetIDProveedor(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidproveedor').value = HdnKey;
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="display:none;">
                <asp:HiddenField ID="hidviatico" Value="0" runat="server" />
                <asp:HiddenField ID="hidestado" Value="0" runat="server" />
                <asp:HiddenField ID="hiddetalle" Value="0" runat="server" />
            </div>
            <div id="divPagina">
                <div style="display:none;">
                    <asp:HiddenField ID="hidorden" Value="0" runat="server" />
                    <asp:HiddenField ID="hidordendetalle" Value="0" runat="server" />
                    <asp:HiddenField ID="hidproveedor" Value="0" runat="server" />
                    <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
                    <asp:HiddenField ID="hidsolicita" Value="0" runat="server" />
                </div>
                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="100%">
                    <thead>
                        <tr>
                    <th colspan="6" valign="top" align="center">
                        <asp:Label ID="lblFacturaEncabezado" runat="server" Text="ENCABEZADO VIATICO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="imgPdf" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Viatico" />
                        <%--&nbsp;&nbsp;
                        <asp:ImageButton ID="imgPdfCopia" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Copia" />
                        &nbsp;&nbsp;
                        <asp:ImageButton ID="imgChequeCompleto" runat="server" Height="20px" ImageUrl="~/images/pdf4.png" Width="20px" ToolTip="Emitir Egreso Cheque Completo" />--%>
                    </th>
                </tr>
                    </thead>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblFechaCompra" runat="server" Text="Fecha *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                            <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFecha"
                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                            </obout:Calendar>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblNro" runat="server" Text="Numero *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtNumero" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblProveedor" runat="server" Text="Empleado *"></asp:Label>
                        </td>
                        <td colspan="5" valign="top" align="left" style="width: 15%;">
                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtProveedor" ID="AutoCompleteExtender3"  
                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDProveedor">  
                            </asp:AutoCompleteExtender>  
                            <asp:TextBox ID="txtProveedor" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblEntradas" runat="server" Text="Total Entradas *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtEntradas" runat="server" MaxLength="50" Enabled="false" Text="0"></asp:TextBox>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblSalidas" runat="server" Text="Total Salidas *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtSalidas" runat="server" MaxLength="50" Enabled="false" Text="0"></asp:TextBox>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="Saldo" runat="server" Text="Saldo *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;" colspan="2">
                            <asp:TextBox ID="txtSaldo" runat="server" MaxLength="50" Enabled="false" Text="0"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                        </td>
                        <td valign="top" colspan="5" align="left" style="width: 60%;">
                            <asp:TextBox ID="txtObservacion" runat="server" MaxLength="2000" TextMode="MultiLine" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" valign="top" align="center">
                            <asp:Button ID="btnSalvarEncabezado" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevoEnc" runat="server" Text="Nuevo" />
                        </td>
                    </tr>
                </table>
                <br />
                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
                <tr>
                    <td valign="top" align="center" style="width: 100%;">
                        <asp:Wizard ID="wizEgresos" runat="server" ActiveStepIndex="0">
                            <WizardSteps>
                                <asp:WizardStep ID="wzDetalles" runat="server" Title="Detalles">
                                    <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="100%">
                                        <thead>
                                            <tr>
                                                <th colspan="6" valign="top" align="center">
                                                    <asp:Label ID="Label6" runat="server" Text="VIATICOS DETALLE" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tr>
                                            <td valign="top" align="left" class="auto-style2">
                                                <asp:Label ID="lblTercero" runat="server" Text="Tercero *"></asp:Label>
                                            </td>
                                            <td colspan="3" valign="top" align="left" style="width: 15%;">
                                                <asp:AutoCompleteExtender ServiceMethod="GetSearch1" MinimumPrefixLength="1" CompletionInterval="10"  
                                                    EnableCaching="false" CompletionSetCount="10" TargetControlID="txtTercero" ID="AutoCompleteExtender1"  
                                                    runat="server" FirstRowSelected="false" OnClientItemSelected="GetID">  
                                                </asp:AutoCompleteExtender>  
                                                <asp:TextBox ID="txtTercero" runat="server" Width="96%" AutoPostBack="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" style="width: 8%;">
                                                <asp:Label ID="lblTipo" runat="server" Text="Tipo *"></asp:Label>
                                            </td>
                                            <td valign="top" align="left" style="width: 17%;">
                                                <asp:DropDownList ID="cboTipo" Width="65%" runat="server">
                                                    <asp:ListItem Value="0" Selected>- SELECCIONE -</asp:ListItem>
                                                    <asp:ListItem Value="ENTRADA">ENTRADA</asp:ListItem>
                                                    <asp:ListItem Value="SALIDA">SALIDA</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td valign="top" align="left" style="width: 9%;">
                                                <asp:Label ID="lblReferencia" runat="server" Text="Referencia"></asp:Label>
                                            </td>
                                            <td valign="top" align="left" style="width: 25%;">
                                                <asp:TextBox ID="txtReferencia" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left" style="width: 9%;">
                                                <asp:Label ID="lblCantidad" runat="server" Text="Cantidad *"></asp:Label>
                                            </td>
                                            <td valign="top" align="left" style="width: 25%;">
                                                <asp:TextBox ID="txtCantidad" runat="server" MaxLength="100" Width="90%" onkeypress="javascript:return solonumeros(event)" Text="0"></asp:TextBox>
                                            </td>
                                            <td valign="top" align="left" style="width: 9%;">
                                                <asp:Label ID="lblValor" runat="server" Text="Valor *"></asp:Label>
                                            </td>
                                            <td valign="top" align="left" style="width: 25%;">
                                                <asp:TextBox ID="txtValor" runat="server" MaxLength="100" Width="90%" onkeypress="javascript:return solonumeros(event)" Text="0"></asp:TextBox>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td valign="top" align="left" style="width: 5%;">
                                                <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                                            </td>
                                            <td valign="top" colspan="3" align="left" style="width: 60%;">
                                                <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="1000" TextMode="MultiLine" Width="99%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6" valign="top" align="center">
                                                <asp:Button ID="btnSalvarDetalle" runat="server" Text="Salvar" />
                                                <asp:Button ID="btnNuevoDetalle" runat="server" Text="Nuevo" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                    <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1050px" runat="server" id="tbl_detalle">
                                        <tr>
                                            <td valign="top" align="center" class="auto-style1">
                                                <div style="width: 95%; height: 333px; overflow: scroll">
                                                    <asp:GridView ID="gridDetalle" runat="server" DataKeyNames="id_vide" AutoGenerateColumns="False" CssClass="mGrid" CellPadding="4" GridLines="None" Width="100%" >
                                                        <AlternatingRowStyle BackColor="White" />
                                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                        <Columns>
                                                            <asp:BoundField DataField="nombre_terc" HeaderText="Tercero" />
                                                            <asp:BoundField DataField="tipo_vide" HeaderText="Tipo" />
                                                            <asp:BoundField DataField="referencia_vide" HeaderText="Referencia" />
                                                            <asp:BoundField DataField="cant_vide" HeaderText="Cantidad" />
                                                            <asp:BoundField DataField="valor_vide" HeaderText="Valor" />
                                                            <asp:BoundField DataField="total_vide" HeaderText="Total" />
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
