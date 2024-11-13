<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamCumplimientoPresupuestoMes.aspx.vb" Inherits="InfParamCumplimientoPresupuestoMes" MasterPageFile="~/Site.Master" %>

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
                        <asp:Label ID="Label1" runat="server" Text="Informe Cumplimiento Presupuesto Mes" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
            <%--<tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:Label ID="lblAsesores" runat="server" Text="Asesores: "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center" style="width: 100%;">
                    <asp:DropDownList ID="cboasesores" runat="server" Width="400px"></asp:DropDownList>
                </td>
            </tr>--%>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnAgencia" runat="server" Text="Agencia" />
                            <asp:Button ID="btnAsesor" runat="server" Text="Asesor" />
                            <asp:Button ID="btnDespachador" runat="server" Text="Despachador" />
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
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" OnClick="ImageButton1_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
