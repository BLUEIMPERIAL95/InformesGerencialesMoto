<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InformesControlInterno.aspx.vb" Inherits="InformesControlInterno" MasterPageFile="~/Site.Master" %>

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
                    <th colspan="2" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="INFORMES VARIOS CONTROL INTERNO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" border="0" align="center" width="600px">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button ID="btnMovimientos" style="background-color: #F8CFCF;" width="150px" runat="server" Text="Movimientos" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnOperacion" style="background-color: #E0E0F7;" width="150px" runat="server" Text="Operación" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnAnulados" style="background-color: #F9DFC2;" width="150px" runat="server" Text="Anulados" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button ID="btnGastos" style="background-color: #FBAAA3;" width="150px" runat="server" Text="Gastos" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnSinFacturar" style="background-color: #9398F9;" width="150px" runat="server" Text="Sin Facturar" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnProveedores" style="background-color: #F9C284;" width="150px" runat="server" Text="Proveedores" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button ID="btnFactNC" style="background-color: #F95858;" width="150px" runat="server" Text="Fact y NC" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnRecaudo" style="background-color: #6068F8;" width="150px" runat="server" Text="Recaudo" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnCartera" style="background-color: #F9AB54;" width="150px" runat="server" Text="Cartera" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button ID="btnAnticipos" style="background-color: #F73827;" width="150px" runat="server" Text="Anticipos" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnConsolidado" style="background-color: #1A25F9;" width="150px" runat="server" Text="Consolidado" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnDetCartera" style="background-color: #FD8B0B;" width="150px" runat="server" Text="Det.Cartera" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <asp:Button ID="btnDetFacturas" style="background-color: #9F030C;" width="150px" runat="server" Text="Detalle Facturas" />
                    </td>
                    <td valign="top" align="center">
                        <asp:Button ID="btnCXC" style="background-color: #656DC1;" width="150px" runat="server" Text="Cuentas X Cobrar" />
                    </td>
                </tr>
                </ContentTemplate>
            </asp:UpdatePanel>
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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <tr>               
                <td valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div id="divmostrar" runat="server" style="width: 850px; height: 350px; overflow: scroll">
                        
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="top" align="center" style="width: 2%;">
                    <table cellpadding="2" cellspacing="0" border="0" class="StyleTable1" align="center">
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" />
                            </td>
                        </tr>
                        <%--<tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="25px" ImageUrl="~/images/pdf4.png" Width="25px" />
                            </td>
                        </tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 100%; visibility: hidden;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div id="divinforme" runat="server" style="width: 850px; height: 0px; overflow: scroll">
                        
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
