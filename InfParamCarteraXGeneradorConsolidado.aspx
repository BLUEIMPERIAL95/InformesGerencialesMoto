<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamCarteraXGeneradorConsolidado.aspx.vb" Inherits="InfParamCarteraXGeneradorConsolidado" MasterPageFile="~/Site.Master" %>

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
                        <asp:Label ID="Label1" runat="server" Text="CARTERA X GENERADOR CONSOLIDADO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblfecInicio" runat="server" Text="Facturas Emitidas Hasta: "></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo: "></asp:Label>
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
                    <asp:DropDownList ID="cboTipo" Width="60%" runat="server">
                            <asp:ListItem Value="0" Selected>Completa</asp:ListItem>
                            <%--<asp:ListItem Value="1">Cartera Año Actual</asp:ListItem>
                            <asp:ListItem Value="2">Cartera (Año Actual - 1)</asp:ListItem>
                            <asp:ListItem Value="3">Cartera <= (Año Actual - 2)</asp:ListItem>
                            <asp:ListItem Value="4">Asesores (a 29 días)</asp:ListItem>
                            <asp:ListItem Value="5">Cartera (Entre 30 y 59 dias)</asp:ListItem>
                            <asp:ListItem Value="6">Prejuridica (Entre 60 y 79 dias)</asp:ListItem>
                            <asp:ListItem Value="7">Juridica (Mas de 80 dias)</asp:ListItem>--%>
                        </asp:DropDownList>
                </td>
            </tr>
            <tr style="visibility:hidden;">
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblGeneradores" runat="server" Text="Generadores: "></asp:Label>
                </td>
            </tr>
            <tr style="visibility:hidden;">
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cbogeneradores" runat="server" Width="400px"></asp:DropDownList>
                </td>
            </tr>
            <tr style="visibility:hidden;">
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblAsesores" runat="server" Text="Asesores: "></asp:Label>
                </td>
            </tr>
            <tr style="visibility:hidden;">
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cboasesores" runat="server" Width="400px"></asp:DropDownList>
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
        <p style="page-break-after:always;"></p>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <tr>
                <%--<td colspan="4" valign="top" align="center" style="width: 100%;">
                    <div style="width: 100%; height: 400px; overflow: scroll">
                        <asp:GridView ID="gridAsesores" runat="server" Width="500px" Height="300px" 
                            CellPadding="4" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                </td>--%>
                <td valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div id="divmostrar" runat="server" style="width: 850px; height: 280px; overflow: scroll">
                        
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
