<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ComprasEquipos.aspx.vb" Inherits="ComprasEquipos" MasterPageFile="~/Site.Master" %>

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
    <div id="divPagina">
                <div style="display:none;">
                    <asp:HiddenField ID="hidorden" Value="0" runat="server" />
                    <asp:HiddenField ID="hidordendetalle" Value="0" runat="server" />
                    <asp:HiddenField ID="hidproveedor" Value="0" runat="server" />
                    <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
                    <asp:HiddenField ID="hidsolicita" Value="0" runat="server" />
                </div>
                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="left" width="570px">
                    <thead>
                        <tr>
                            <th colspan="4" valign="top" align="center">
                                <asp:Label ID="Label3" runat="server" Text="ORDEN DE COMPRA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblFechaCompra" runat="server" Text="Fecha *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <input id="txtFechaCompra" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                            <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaCompra"
                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                            </obout:Calendar>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblNroCompra" runat="server" Text="Numero *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtNroCompra" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblProveedor" runat="server" Text="Prov. *"></asp:Label>
                        </td>
                        <td colspan="3" valign="top" align="left" style="width: 15%;">
                            <%--<asp:DropDownList ID="cboTercero" Width="100%" runat="server" AutoPostBack="true">
                        
                            </asp:DropDownList>--%>
                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtProveedor" ID="AutoCompleteExtender3"  
                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDProveedor">  
                            </asp:AutoCompleteExtender>  
                            <asp:TextBox ID="txtProveedor" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblSolicitante" runat="server" Text="Solicita *"></asp:Label>
                        </td>
                        <td colspan="3" valign="top" align="left" style="width: 15%;">
                            <%--<asp:DropDownList ID="cboSolicitante" Width="100%" runat="server" AutoPostBack="true">
                        
                            </asp:DropDownList>--%>
                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtTercero" ID="AutoCompleteExtender1"  
                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetID">  
                            </asp:AutoCompleteExtender>  
                            <asp:TextBox ID="txtTercero" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblAutoriza" runat="server" Text="Autoriza *"></asp:Label>
                        </td>
                        <td colspan="3" valign="top" align="left" style="width: 15%;">
                            <%--<asp:DropDownList ID="cboAutoriza" Width="100%" runat="server" AutoPostBack="true">
                        
                            </asp:DropDownList>--%>
                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtSolicita" ID="AutoCompleteExtender2"  
                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDSolicita">  
                            </asp:AutoCompleteExtender>  
                            <asp:TextBox ID="txtSolicita" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Label ID="Label8" ForeColor="Transparent" runat="server" Text="LABEL"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                        </td>
                        <td valign="top" colspan="3" align="left" style="width: 60%;">
                            <asp:TextBox ID="txtObservacion" runat="server" MaxLength="1000" TextMode="MultiLine" Width="99%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Label ID="lblTotalOrden" runat="server" ForeColor="Red" Font-Bold="true" Text="VALOR TOTAL ORDEN: $ 0.00"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Button ID="btnSalvarEncabezado" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevoEnc" runat="server" Text="Nuevo" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <div class="auto-style5">
                                <asp:GridView ID="gridOrden" runat="server" DataKeyNames="id_orco" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                    <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="fecha_orco" HeaderText="Fecha" />
                                        <asp:BoundField DataField="numero_orco" HeaderText="Numero" />
                                        <asp:BoundField DataField="nombre_prov" HeaderText="Proveedor" />
                                        <asp:BoundField DataField="nombre_soli" HeaderText="Solicitante" />
                                        <asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />
                                        <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="570px">
                    <thead>
                        <tr>
                            <th colspan="4" valign="top" align="center">
                                <asp:Label ID="Label1" runat="server" Text="DETALLE ORDEN DE COMPRA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="Label7" runat="server" Text="Tipo Iva-Ret *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:CheckBox ID="chkPorcentaje" runat="server" Text="%" Checked="true" AutoPostBack="true" />
                            <asp:CheckBox ID="chkValor" runat="server" Text="$" AutoPostBack="true" />
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblEquipo" runat="server" Text="Equipo *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:DropDownList ID="cboEquipo" Width="155px" runat="server" AutoPostBack="true">
                        
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblCantidad" runat="server" Text="Cantidad *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtCantidad" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblCosto" runat="server" Text="Costo Unitario *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtCosto" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblPorIva" runat="server" Text="% Iva *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtIva" runat="server" MaxLength="50" Text="0" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="lblRetencion" runat="server" Text="% Retención *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtRetencion" runat="server" MaxLength="50" Text="0" onkeypress="javascript:return solo_numeros(event)"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:Label ID="Label2" runat="server" Text="Costo Total *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <asp:TextBox ID="txtValor" runat="server" MaxLength="50" Width="120px" Text="0" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>&nbsp;
                            <asp:Button ID="btnCalcular" runat="server" Text="C" ToolTip="Calcular Total Orden Compra" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Label ID="Label6" ForeColor="Transparent" runat="server" Text="LABEL"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="Label5" runat="server" Text="Observacion"></asp:Label>
                        </td>
                        <td valign="top" colspan="3" align="left" style="width: 60%;">
                            <asp:TextBox ID="txtObservacionDetalle" runat="server" MaxLength="1000" TextMode="MultiLine" Width="95%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Label ID="Label4" ForeColor="Transparent" runat="server" Text="LABEL"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Button ID="btnSalvarDetalle" runat="server" Text="Agregar" />
                            <asp:Button ID="btnNuevoDet" runat="server" Text="Nuevo" />
                        </td>
                    </tr>
                    <tr>
                        <td  colspan="4" valign="top" align="center" style="width:100%;">
                            <div class="auto-style5">
                                <asp:GridView ID="gridDetalle" runat="server" DataKeyNames="id_deoc" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                    <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="nombre_EQUI" HeaderText="Equipo" />
                                        <asp:BoundField DataField="cantidad_deoc" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="costounitario_deoc" HeaderText="Costo" />
                                        <asp:BoundField DataField="iva_deoc" HeaderText="Iva" />
                                        <asp:BoundField DataField="ret_deoc" HeaderText="Rete" />
                                        <asp:BoundField DataField="valor_deoc" HeaderText="Total" />
                                        <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
