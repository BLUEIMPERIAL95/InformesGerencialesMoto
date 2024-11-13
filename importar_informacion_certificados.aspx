<%@ Page Language="VB" AutoEventWireup="false" CodeFile="importar_informacion_certificados.aspx.vb" Inherits="importar_informacion_certificados" MasterPageFile="~/Site.Master" %>

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
                        <asp:Label ID="Label1" runat="server" Text="IMPORTAR INFORMACION CERTIFICADOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblEmpresas" runat="server" Text="Empresas *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Periodo *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblBimestre" runat="server" Text="Bimestre "></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo Certificado *" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboEmpresas" Width="60%" runat="server">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboPeriodo" Width="60%" runat="server">
                        <asp:ListItem Value="0" Selected>- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="2020">2020</asp:ListItem>
                        <asp:ListItem Value="2021">2021</asp:ListItem>
                        <asp:ListItem Value="2022">2022</asp:ListItem>
                        <asp:ListItem Value="2023">2023</asp:ListItem>
                        <asp:ListItem Value="2024">2024</asp:ListItem>
                        <asp:ListItem Value="2025">2025</asp:ListItem>
                        <asp:ListItem Value="2026">2026</asp:ListItem>
                        <asp:ListItem Value="2027">2027</asp:ListItem>
                        <asp:ListItem Value="2028">2028</asp:ListItem>
                        <asp:ListItem Value="2029">2029</asp:ListItem>
                        <asp:ListItem Value="2030">2030</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboBimestre" Width="60%" runat="server">
                        <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="6">6</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboTipo" Width="60%" runat="server" Visible="false">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="3" align="center" style="width: 15%;">
                    <asp:Label ID="Label2" runat="server" Text="Archivo "></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="4" align="center" style="width: 15%;">
                    <%--<asp:FileUpload ID="FileExcel" runat="server" Width="310px" />--%>
                    <asp:FileUpload ID="FileExcel" runat="server" Width="310px"  />
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <asp:Button ID="btnGenerar" runat="server" Text="Generar" />
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                </td>
            </tr>
        </table>
        <br />
        <p style="page-break-after:always;"></p>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <tr>
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
                                <asp:Label ID="lblRegistros" runat="server" Text=""></asp:Label>
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
        </table>
        <br />
        <p style="page-break-after:always;"></p>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <tr>
                <td valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <div id="divmostrarlog" runat="server" style="width: 850px; height: 280px; overflow: scroll">
                        
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="top" align="center" style="width: 2%;">
                    <table cellpadding="2" cellspacing="0" border="0" class="StyleTable1" align="center">
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" />
                                <asp:Label ID="lblLogRegistros" runat="server" Text=""></asp:Label>
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
        </table>
    </div>
</asp:Content>
