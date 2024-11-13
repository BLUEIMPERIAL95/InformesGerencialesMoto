<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamRegistroEntrada.aspx.vb" Inherits="InfParamRegistroEntrada" MasterPageFile="~/Site.Master" %>

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
                    <th colspan="3" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="REGISTRO DE ENTRADA DIARIA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa"></asp:Label>
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
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboempresa" runat="server" Width="130px">
                        <asp:ListItem Value="0" Selected>TODAS</asp:ListItem>
                        <asp:ListItem Value="MOTOTRANSPORTAMOS">MOTOTRANSPORTAMOS</asp:ListItem>
                        <asp:ListItem Value="MOTOTRANSPORTAR">MOTOTRANSPORTAR</asp:ListItem>
                        <asp:ListItem Value="REFRILOGISTICA">REFRILOGISTICA</asp:ListItem>
                        <asp:ListItem Value="TRAMITAR">TRAMITAR</asp:ListItem>
                        <asp:ListItem Value="CIACAPRI">CIACAPRI</asp:ListItem>
                        <asp:ListItem Value="MOTOSEGURIDAD">MOTOSEGURIDAD</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center">
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
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" OnClick="ImageButton1_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="25px" ImageUrl="~/images/pdf4.png" Width="25px" Visible="false" />
                            </td>
                        </tr>
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
