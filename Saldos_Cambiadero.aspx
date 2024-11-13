<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Saldos_Cambiadero.aspx.vb" Inherits="Saldos_Cambiadero" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 82%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidperfil" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="SALDOS CAMBIADERO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFecha" runat="server" Text="Fecha *"></asp:Label>
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
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboTipo" Width="80%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">Recarga</asp:ListItem>
                        <asp:ListItem Value="2">Saldo</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label2" runat="server" Text="Año *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboAño" Width="80%" runat="server">
                        <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="2023">2023</asp:ListItem>
                        <asp:ListItem Value="2024">2024</asp:ListItem>
                        <asp:ListItem Value="2025">2025</asp:ListItem>
                        <asp:ListItem Value="2026">2026</asp:ListItem>
                        <asp:ListItem Value="2027">2027</asp:ListItem>
                        <asp:ListItem Value="2028">2028</asp:ListItem>
                        <asp:ListItem Value="2029">2029</asp:ListItem>
                        <asp:ListItem Value="2030">2030</asp:ListItem>
                        <asp:ListItem Value="2031">2031</asp:ListItem>
                        <asp:ListItem Value="2032">2032</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label3" runat="server" Text="Mes *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboMes" Width="80%" runat="server">
                        <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">Enero</asp:ListItem>
                        <asp:ListItem Value="2">Febrero</asp:ListItem>
                        <asp:ListItem Value="3">Marzo</asp:ListItem>
                        <asp:ListItem Value="4">Abril</asp:ListItem>
                        <asp:ListItem Value="5">Mayo</asp:ListItem>
                        <asp:ListItem Value="6">Junio</asp:ListItem>
                        <asp:ListItem Value="7">Julio</asp:ListItem>
                        <asp:ListItem Value="8">Agosto</asp:ListItem>
                        <asp:ListItem Value="9">Septiembre</asp:ListItem>
                        <asp:ListItem Value="10">Octubre</asp:ListItem>
                        <asp:ListItem Value="11">Noviembre</asp:ListItem>
                        <asp:ListItem Value="12">Diciembre</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblSaldoInicial" runat="server" Text="S.Inicial *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtSaldoInicial" runat="server" MaxLength="200" Width="98%" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblSaldoFinal" runat="server" Text="S.Final *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtSaldoFinal" runat="server" MaxLength="200" Width="95%" onkeypress="javascript:return solonumeros(event)" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblDescripcion" runat="server" Text="Observacion *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;" colspan="3">
                    <asp:TextBox ID="txtObservacion" runat="server" MaxLength="3000" Width="98%" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 95%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridSaldos" runat="server" DataKeyNames="id_saca" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="fecha_saca" HeaderText="Fecha" />
                                <asp:BoundField DataField="tipo_saca" HeaderText="Tipo" />
                                <asp:BoundField DataField="año_saca" HeaderText="Año" />
                                <asp:BoundField DataField="mes_saca" HeaderText="Mes" />
                                <asp:BoundField DataField="saldoini_saca" HeaderText="S.Inicial" />
                                <%--<asp:BoundField DataField="saldofin_saca" HeaderText="S.Final" />--%>
                                <asp:BoundField DataField="strnom1_usua" HeaderText="Usuario" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

