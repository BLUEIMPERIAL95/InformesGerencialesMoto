<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamCad.aspx.vb" Inherits="InfParamCad" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="INFORME CAD" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaInicio"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <input id="txtFechaFin" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaFin" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaFin"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cboEmpresa" Width="50%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblSucursal" runat="server" Text="Sucursal: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cboSucursal" Width="50%" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cboDocumento" Width="50%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="Egresos">Egresos</asp:ListItem>
                        <asp:ListItem Value="Facturas">Facturas</asp:ListItem>
                        <asp:ListItem Value="Manifiestos">Manifiestos</asp:ListItem>
                        <asp:ListItem Value="Notas">Notas</asp:ListItem>
                        <asp:ListItem Value="Recibos">Recibos</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                            <ProgressTemplate>
                                <div>
                                    <img src="images/cortinilla2.gif" alt="Loading" border="0" />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="0" border="0" align="center" width="880px">
            <tr>
                <td colspan="4" valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div style="width: 600px; height: 400px; overflow: scroll">
                            <asp:GridView ID="gridDocumentos" runat="server" AutoGenerateColumns="False" CssClass="mGrid" Width="100%" 
                                DataKeyNames="numero, Nombre" GridLines="Horizontal" ShowHeader="True" Font-Bold="true" Font-Size="Small">
                            <Columns>
                                <asp:BoundField HeaderText="Empresa" HeaderStyle-BackColor="White" DataField="Empresa" />
                                <asp:BoundField HeaderText="Sucursal" HeaderStyle-BackColor="White" DataField="sucursal" />
                                <asp:BoundField HeaderText="Número" HeaderStyle-BackColor="White" DataField="numero" />
                                <asp:BoundField HeaderText="Fecha" HeaderStyle-BackColor="White" DataField="fecha" />
                                <asp:BoundField HeaderText="Comprobante" HeaderStyle-BackColor="White" DataField="descripcion" />
                                <asp:BoundField HeaderText="Tercero" HeaderStyle-BackColor="White" DataField="Nombre" />
                                <asp:BoundField HeaderText="Valor" DataFormatString="{0:N}" HeaderStyle-BackColor="White" DataField="valor" />
                                <asp:BoundField HeaderText="Enviado" HeaderStyle-BackColor="White" DataField="Entregado" />
                                <asp:BoundField HeaderText="# Envio" HeaderStyle-BackColor="White" DataField="NumEnv" />
                                <asp:BoundField HeaderText="Recibido" HeaderStyle-BackColor="White" DataField="Recibido" />
                                <asp:BoundField HeaderText="# Recibido" HeaderStyle-BackColor="White" DataField="NumRec" />
                                <%--<asp:ButtonField CommandName="Guardar" ButtonType="Image" ImageUrl="~/images/ver-mas.png" />--%>
                            </Columns>
                            <%--<HeaderStyle CssClass="gridheader" />
                            <AlternatingRowStyle CssClass="gridfilasalt" />
                            <RowStyle CssClass="gridfilas" />--%>
                        </asp:GridView>
                        </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<cc1:Grid ID="gridAsesores" runat="server" PageSize="1000" PageSizeOptions="500"
                        AutoGenerateColumns="true" AllowAddingRecords="false" AllowColumnResizing="false" 
                        AllowGrouping="true" Width="500px" Height="300px" Serialize="true" AllowRecordSelection="false" FolderStyle="OboutGrid/style_5">
                        <TemplateSettings GroupHeaderTemplateId="GroupTemplate" />
                        <ScrollingSettings ScrollHeight="300px" ScrollWidth="900px" />
                        <PagingSettings Position="TopAndBottom" PageSizeSelectorPosition="TopAndBottom" ShowRecordsCount="true" />
                        <FilteringSettings InitialState="Visible" FilterPosition="Top"  />
                    </cc1:Grid>--%>
                </td>
                <td valign="top" align="center" style="width: 2%;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
