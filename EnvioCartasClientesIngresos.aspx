<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EnvioCartasClientesIngresos.aspx.vb" Inherits="EnvioCartasClientesIngresos" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 900px;
            height: 306px;
            overflow: scroll;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="Envío Ingresos Clientes" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                    <asp:Label ID="lblGenerador" runat="server" Text="Generadores: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cbogeneradores" runat="server" Width="400px"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnPrueba" runat="server" Text="Generar" />
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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td colspan="4" valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div class="auto-style1">
                                <asp:GridView ID="gridDetalle" runat="server" DataKeyNames="generador_id, zona" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                                    <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="generador_documento" HeaderText="Documento" />
                                        <asp:BoundField DataField="generador_nombre" HeaderText="Generador" HtmlEncode="False" />
                                        <asp:BoundField DataField="Facturado" HeaderText="Facturado" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtcorreo" runat="server" Text='<%#Bind("correo")%>' />
                                            </ItemTemplate>
                                            <HeaderTemplate>Correo</HeaderTemplate>
                                        </asp:TemplateField>
                                        <asp:ButtonField CommandName="enviar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="top" align="center" style="width: 2%;">
                    <%--<asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" OnClick="ImageButton1_Click" />--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
