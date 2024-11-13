<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CuentaCobro.aspx.vb" Inherits="CuentaCobro" MasterPageFile="~/Site.Master" %>

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
            width: 100%;
            height: 405px;
            overflow: scroll;
        }
        .auto-style6 {
            height: 369px;
        }
        .auto-style7 {
            width: 15%;
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
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div id="divPagina">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div style="display:none;">
            <asp:HiddenField ID="hidcuentcobro" Value="0" runat="server" />
            <asp:HiddenField ID="hidcuentcobrodetalle" Value="0" runat="server" />
            <asp:HiddenField ID="hidresolucion" Value="0" runat="server" />
            <asp:HiddenField ID="hidbaseretencion" Value="0" runat="server" />
            <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
            <asp:HiddenField ID="hidsolicita" Value="0" runat="server" />
        </div>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="left" width="570px" style="height: 450px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label3" runat="server" Text="DOCUMENTO DE COBRO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList ID="cboEmpresas" Width="100%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblAgencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList ID="cboAgencia" Width="100%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFechaCuenta" runat="server" Text="Fecha *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFechaCuenta" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaCuenta"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblNroCuenta" runat="server" Text="Numero *"></asp:Label>
                </td>
                <td valign="top" align="left" class="auto-style7">
                    <asp:TextBox ID="txtNroCuenta" runat="server" MaxLength="50" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblTercero" runat="server" Text="Proveedor *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left">
                    <%--<asp:DropDownList ID="cboTercero" Width="100%" runat="server">
                        
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
                    <asp:Label ID="lblAutoriza" runat="server" Text="Solicita *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left">
                    <%--<asp:DropDownList ID="cboAutoriza" Width="100%" runat="server">
                        
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
                    <asp:Label ID="Label4" ForeColor="Transparent" runat="server" Text="LABEL"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 5%;">
                    <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                </td>
                <td valign="top" colspan="3" align="left">
                    <asp:TextBox ID="txtObservacion" runat="server" MaxLength="1000" TextMode="MultiLine" Width="99%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <asp:Label ID="lblTotalCuenta" runat="server" ForeColor="Red" Font-Bold="true" Text="VALOR TOTAL CUENTA: $ 0.00"></asp:Label>
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
                    <asp:Label ID="lblBuscar" runat="server" Text="Buscar por: "></asp:Label>
                    <asp:RadioButton ID="rdNumero" runat="server" Text="Número" Checked="true" AutoPostBack="true" />
                    <asp:RadioButton ID="rdTercero" runat="server" Text="Tercero" AutoPostBack="true" />&nbsp;&nbsp;
                    <asp:TextBox ID="txtBuscar" runat="server" MaxLength="50" Width="30%"></asp:TextBox>&nbsp;&nbsp;
                    <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="images/busqueda.png" />
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center" class="auto-style6">
                    <div class="auto-style5">
                        <asp:GridView ID="gridCuentaCobro" runat="server" DataKeyNames="id_cuco" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="90%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombre_agcc" HeaderText="Agencia" />
                                <asp:BoundField DataField="fecha_cuco" HeaderText="Fecha" />
                                <asp:BoundField DataField="numero_cuco" HeaderText="Numero" />
                                <asp:BoundField DataField="nombre_terc" HeaderText="Tercero" />
                                <asp:BoundField DataField="nombre_terc_aut" HeaderText="Autoriza" />
                                <asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="570px" style="height: 400px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="DETALLE DOCUMENTO DE COBRO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo *"></asp:Label>
                </td>
                <td valign="top" colspan="3" align="left" class="auto-style7">
                    <asp:DropDownList ID="cboTipo" runat="server" Width="100%" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 5%;">
                    <asp:Label ID="lblConcepto" runat="server" Text="Concepto *"></asp:Label>
                </td>
                <td valign="top" colspan="3" align="left" style="width: 60%;">
                    <asp:TextBox ID="txtConcepto" runat="server" MaxLength="1000" TextMode="MultiLine" Width="95%"></asp:TextBox>
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
                    <asp:Label ID="Label2" runat="server" Text="Costo Total *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtValor" runat="server" Width="110px" MaxLength="50" Text="0" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>&nbsp;
                    <asp:Button ID="btnCalcular" runat="server" Text="C" ToolTip="Calcular Total Cuenta Cobro" />
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblRetencion" runat="server" Text="Retencion *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtRetencion" runat="server" Text="0" Enabled="false" MaxLength="50" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
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
                    <asp:Label ID="Label6" ForeColor="Transparent" runat="server" Text="LABEL"></asp:Label>
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
                        <asp:GridView ID="gridDetalle" runat="server" DataKeyNames="id_ccde" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="90%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="concepto_ccde" HeaderText="Concepto" />
                                <asp:BoundField DataField="cantidad_ccde" HeaderText="Cantidad" />
                                <asp:BoundField DataField="valor_ccde" HeaderText="Costo" />
                                <asp:BoundField DataField="total_ccde" HeaderText="Total" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
