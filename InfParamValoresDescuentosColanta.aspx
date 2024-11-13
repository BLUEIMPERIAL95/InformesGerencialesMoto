<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamValoresDescuentosColanta.aspx.vb" Inherits="InfParamValoresDescuentosColanta" MasterPageFile="~/Site.Master" %>

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
                        <asp:Label ID="Label1" runat="server" Text="INFORME VALORES DESCUENTOS COLANTA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblMes" runat="server" Text="Mes *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblAño" runat="server" Text="Año *"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 25%;">
                    <asp:DropDownList ID="cboMes" Width="40%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">1 - Enero</asp:ListItem>
                        <asp:ListItem Value="2">2 - Febrero</asp:ListItem>
                        <asp:ListItem Value="3">3 - Marzo</asp:ListItem>
                        <asp:ListItem Value="4">4 - Abril</asp:ListItem>
                        <asp:ListItem Value="5">5 - Mayo</asp:ListItem>
                        <asp:ListItem Value="6">6 - Junio</asp:ListItem>
                        <asp:ListItem Value="7">7 - Julio</asp:ListItem>
                        <asp:ListItem Value="8">8 - Agosto</asp:ListItem>
                        <asp:ListItem Value="9">9 - Septiembre</asp:ListItem>
                        <asp:ListItem Value="10">10 - Octubre</asp:ListItem>
                        <asp:ListItem Value="11">11 - Noviembre</asp:ListItem>
                        <asp:ListItem Value="12">12 - Diciembre</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="center" style="width: 25%;">
                    <asp:DropDownList ID="cboAño" Width="40%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="0" Selected ="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="2022">2022</asp:ListItem>
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
                        <asp:ListItem Value="2033">2033</asp:ListItem>
                        <asp:ListItem Value="2034">2034</asp:ListItem>
                        <asp:ListItem Value="2035">2035</asp:ListItem>
                        <asp:ListItem Value="2036">2036</asp:ListItem>
                        <asp:ListItem Value="2037">2037</asp:ListItem>
                        <asp:ListItem Value="2038">2038</asp:ListItem>
                        <asp:ListItem Value="2039">2039</asp:ListItem>
                        <asp:ListItem Value="2040">2040</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr style="visibility:hidden;">
                <td colspan="2" valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblManifiesto" runat="server" Text="Tipo"></asp:Label>
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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
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
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" Visible="false" runat="server" Height="25px" ImageUrl="~/images/pdf4.png" Width="25px" />
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
