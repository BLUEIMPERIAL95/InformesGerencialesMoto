<%@ Page Language="VB" AutoEventWireup="false" CodeFile="InfParamPendientesTipoReunionResumido.aspx.vb" Inherits="InfParamPendientesTipoReunionResumido" MasterPageFile="~/Site.Master" %>

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
                    <th valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="INFORME PENDIENTES POR TIPO DE REUNION" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblReunion" runat="server" Text="Tipo Reunión *"></asp:Label>
                </td>
                <%--<td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                </td>--%>
            </tr>
            <tr>
                <td valign="top" align="center">
                    <asp:DropDownList ID="cboreuniones" runat="server" Width="40%"></asp:DropDownList>
                </td>
                <%--<td valign="top" align="center">
                    <asp:DropDownList ID="cboEstados" runat="server" Width="40%">
                        <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                        <asp:ListItem Value="PENDIENTE">PENDIENTE</asp:ListItem>
                        <asp:ListItem Value="ASIGNADO">ASIGNADO</asp:ListItem>
                        <asp:ListItem Value="PRECERRADO">PRECERRADO</asp:ListItem>
                        <asp:ListItem Value="CERRADO">CERRADO</asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
            </tr>
            <tr>
                <td valign="top" align="center">
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
                        <%--<tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" OnClick="ImageButton1_Click" />
                            </td>
                        </tr>--%>
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="25px" ImageUrl="~/images/pdf4.png" Width="25px" />
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
