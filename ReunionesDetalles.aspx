<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReunionesDetalles.aspx.vb" Inherits="ReunionesDetalles" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="asp"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script type="text/javascript">
        function GetIDExpositor(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidExpositor').value = HdnKey;
        }

        function GetIDSolicita(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidSolicita').value = HdnKey;
        }

        function GetIDResponsable(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidResponsable').value = HdnKey;
        }

        function GetIDParticipante(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidParticipante').value = HdnKey;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div id="divPagina">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div style="display:none;">
            <asp:HiddenField ID="hidReunion" Value="0" runat="server" />
            <asp:HiddenField ID="hidPendiente" Value="0" runat="server" />
            <asp:HiddenField ID="hidPendienteTercero" Value="0" runat="server" />
            <asp:HiddenField ID="hidExpositor" Value="0" runat="server" />
            <asp:HiddenField ID="hidSolicita" Value="0" runat="server" />
            <asp:HiddenField ID="hidResponsable" Value="0" runat="server" />
            <asp:HiddenField ID="hidParticipante" Value="0" runat="server" />
        </div>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1100px">
            <thead>
                <tr>
                    <th colspan="6" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="REUNION" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 12%;">
                    <asp:Label ID="lblTipoReunion" runat="server" Text="Tipo Reunión *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;" Width="100%">
                    <asp:DropDownList ID="cboTipoReunion" Width="100%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 12%;">
                    <asp:Label ID="lblCodigo" runat="server" Text="Codigo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;" Width="100%">
                    <asp:TextBox ID="txtCodigo" runat="server" MaxLength="20" Width="100%" Enabled="false"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 12%;">
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 14%;">
                    <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFecha" AutoPostBack="true"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left">
                    <asp:Label ID="lblLugar" runat="server" Text="Lugar"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:TextBox ID="txtLugar" runat="server" MaxLength="100" Width="95%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre *" Width="100%"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" Width="95%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="lblActivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:DropDownList ID="cboActivo" Width="100%" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left">
                    <asp:Label ID="lblObjetivo" runat="server" Text="Objetivo"></asp:Label>
                </td>
                <td valign="top" align="left">
                    <asp:TextBox ID="txtObjetivo" runat="server" MaxLength="200" Width="95%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                </td>
                <td valign="top" align="left">
                    <asp:Label ID="lblExpositor" runat="server" Text="Expositor *" Width="100%"></asp:Label>
                </td>
                <td valign="top" align="left" colspan="3">
                    <%--<asp:DropDownList ID="cboExpositor" Width="100%" runat="server">
                        
                    </asp:DropDownList>--%>
                    <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                        EnableCaching="false" CompletionSetCount="10" TargetControlID="txtExpositor" ID="AutoCompleteExtender1"  
                        runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDExpositor">  
                    </asp:AutoCompleteExtender>  
                    <asp:TextBox ID="txtExpositor" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Descripcion *"></asp:Label>
                </td>
                <td valign="top" align="left" colspan="5">
                    <asp:TextBox ID="txtDescripcion" runat="server" MaxLength="8000" Width="99%" TextMode="MultiLine" Height="80px"></asp:TextBox>
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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" style="width: 100%;">
                    <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="1">
                        <WizardSteps>
                            <asp:WizardStep ID="WizardStep1" runat="server" Title="Participantes" >
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
                                    <thead>
                                        <tr>
                                            <th colspan="2" valign="top" align="center">
                                                <asp:Label ID="Label4" runat="server" Text="PARTICIPANTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td valign="top" align="center" style="width: 20%;">
                                            <asp:Label ID="lblParticipante" runat="server" Text="Participante *"></asp:Label>
                                        </td>
                                        <td valign="top" align="center" style="width: 80%;">
                                            <%--<asp:DropDownList ID="cboParticipantes" Width="50%" runat="server">
                        
                                            </asp:DropDownList>--%>
                                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtParticipante" ID="AutoCompleteExtender4"  
                                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDParticipante">  
                                            </asp:AutoCompleteExtender>  
                                            <asp:TextBox ID="txtParticipante" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" valign="top" align="center">
                                            <asp:Button ID="btnAsignarParticipante" runat="server" Text="Asignar" ToolTip="Asignar Participante a Reunión anterior" />
                                            <asp:Button ID="btnCapturarParticipantes" runat="server" Text="Capturar" ToolTip="Capturar participantes reunión anterior" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td colspan="2" valign="top" align="center" class="auto-style1">
                                            <div style="width: 95%; height: 250px; overflow: scroll">
                                                <asp:GridView ID="gridParticipantes" runat="server" DataKeyNames="id_repa" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="documento_TERC" HeaderText="Documento" />
                                                        <asp:BoundField DataField="nombre_TERC" HeaderText="Participante" />
                                                        <asp:BoundField DataField="fechahora_repa" HeaderText="Fecha Ingreso" />
                                                        <asp:BoundField DataField="fehosal_repa" HeaderText="Fecha Salida" />
                                                        <%--<asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />--%>
                                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep2" runat="server" Title="Pendientes">
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1000px">
                                    <thead>
                                        <tr>
                                            <th colspan="6" valign="top" align="center">
                                                <asp:Label ID="Label2" runat="server" Text="PENDIENTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td valign="top" align="left" style="width: 12%;">
                                            <asp:Label ID="Label3" runat="server" Text="Codigo *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 20%;" Width="100%">
                                            <asp:TextBox ID="txtCodigoPen" runat="server" MaxLength="20" Width="100%" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td valign="top" align="left" style="width: 10%;">
                                            <asp:Label ID="Label5" runat="server" Text="Prioridad *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 23%;">
                                            <asp:DropDownList ID="cboPrioridad" Width="100%" runat="server">
                                                <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                                                <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                                                <asp:ListItem Value="MEDIA">MEDIA</asp:ListItem>
                                                <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td valign="top" align="left" style="width: 10%;">
                                            <asp:Label ID="Label6" runat="server" Text="Estado *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 25%;">
                                            <asp:DropDownList ID="cboEstado" Width="100%" runat="server">
                                                <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                                                <asp:ListItem Value="PENDIENTE">PENDIENTE</asp:ListItem>
                                                <asp:ListItem Value="ASIGNADO">ASIGNADO</asp:ListItem>
                                                <asp:ListItem Value="PRECERRADO">PRECERRADO</asp:ListItem>
                                                <asp:ListItem Value="CERRADO">CERRADO</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" style="width: 12%;">
                                            <asp:Label ID="Label8" runat="server" Text="Plazo *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 14%;">
                                            <input id="txtFechaFin" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                                            <obout:Calendar ID="CalendarFechaFin" runat="server" DatePickerMode="true" TextBoxId="MainContent_Wizard1_txtFechaFin"
                                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                                            </obout:Calendar>
                                        </td>
                                        <td valign="top" align="left" style="width: 12%;">
                                            <asp:Label ID="Label11" runat="server" Text="Nombre *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left">
                                            <asp:TextBox ID="txtNombrePen" runat="server" MaxLength="100" Width="99%" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                        </td>
                                        <td valign="top" align="left" style="width: 12%;">
                                            <asp:Label ID="Label9" runat="server" Text="Solicita *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 25%;">
                                            <%--<asp:DropDownList ID="cboSolicita" Width="100%" runat="server">
                                                
                                            </asp:DropDownList>--%>
                                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtSolicita" ID="AutoCompleteExtender2"  
                                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDSolicita">  
                                            </asp:AutoCompleteExtender>  
                                            <asp:TextBox ID="txtSolicita" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" align="left" style="width: 12%;">
                                            <asp:Label ID="Label7" runat="server" Text="Descripción *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" colspan="5">
                                            <asp:TextBox ID="txtDesPendiente" runat="server" MaxLength="8000" Width="99%" TextMode="MultiLine" Height="80px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" valign="top" align="center">
                                            <asp:Button ID="btnSalvarPendiente" runat="server" Text="Salvar Pendiente" />
                                            <asp:Button ID="btnNuevoPendiente" runat="server" Text="Nuevo Pendiente" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1000px">
                                    <%--<thead>
                                        <tr>
                                            <th valign="top" align="center">
                                                <asp:Label ID="Label9" runat="server" Text="ADMINISTRAR PENDIENTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>--%>
                                     <tr>
                                        <td valign="top" align="center" class="auto-style1">
                                            <div style="width: 95%; height: 180px; overflow: scroll">
                                                <asp:GridView ID="grdPendientes" runat="server" DataKeyNames="id_peru, codigo_peru" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="codigo_peru" HeaderText="Codigo" />
                                                        <asp:BoundField DataField="fecha_peru" HeaderText="Fecha" />
                                                        <asp:BoundField DataField="nombre_peru" HeaderText="Nombre" />
                                                        <asp:BoundField DataField="prioridad_peru" HeaderText="Prioridad" />
                                                        <asp:BoundField DataField="estado_peru" HeaderText="Estado" />
                                                        <asp:BoundField DataField="plazo_peru" HeaderText="Plazo" />
                                                        <%--<asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />--%>
                                                        <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="WizardStep3" runat="server" Title="Asignacion">
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
                                    <thead>
                                        <tr>
                                            <th colspan="6" valign="top" align="center">
                                                <asp:Label ID="lblAsignacionPendiente" runat="server" Text="ASIGNACION PENDIENTE" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tr>
                                        <td valign="top" align="left" style="width: 20%;">
                                            <asp:Label ID="Label10" runat="server" Text="Responsable *"></asp:Label>
                                        </td>
                                        <td valign="top" align="left" style="width: 80%;">
                                            <%--<asp:DropDownList ID="cboResponsable" Width="90%" runat="server">
                        
                                            </asp:DropDownList>--%>
                                            <asp:AutoCompleteExtender ServiceMethod="GetSearch" MinimumPrefixLength="1" CompletionInterval="10"  
                                                EnableCaching="false" CompletionSetCount="10" TargetControlID="txtResponsable" ID="AutoCompleteExtender3"  
                                                runat="server" FirstRowSelected="false" OnClientItemSelected="GetIDResponsable">  
                                            </asp:AutoCompleteExtender>  
                                            <asp:TextBox ID="txtResponsable" runat="server" Width="98%" AutoPostBack="true"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" valign="top" align="center">
                                            <asp:Button ID="btnAsignar" runat="server" Text="Asignar" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1000px">
                                    <%--<thead>
                                        <tr>
                                            <th valign="top" align="center">
                                                <asp:Label ID="Label9" runat="server" Text="ADMINISTRAR PENDIENTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                            </th>
                                        </tr>
                                    </thead>--%>
                                     <tr>
                                        <td valign="top" align="center" class="auto-style1">
                                            <div style="width: 95%; height: 180px; overflow: scroll">
                                                <asp:GridView ID="grdPendientesTerceros" runat="server" DataKeyNames="id_pete, id_peru, codigo_peru, id_TERC" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <Columns>
                                                        <asp:BoundField DataField="codigo_peru" HeaderText="Codigo" />
                                                        <asp:BoundField DataField="fecha_asig" HeaderText="Fecha" />
                                                        <asp:BoundField DataField="estado_pete" HeaderText="Estado" />
                                                        <asp:BoundField DataField="documento_TERC" HeaderText="Documento" />
                                                        <asp:BoundField DataField="nombre_TERC" HeaderText="Responsable" />
                                                        <asp:ButtonField CommandName="ModificarEstado" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                                        <asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />
                                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
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
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

