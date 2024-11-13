<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Generacion_Certificados.aspx.vb" Inherits="Generacion_Certificados" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/SAGERStyle.css" rel="stylesheet" type="text/css" />
    <link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="headercertificados">
            <div class="title">
                
            </div>
            <div class="loginDisplay">
                <table cellpadding="2" cellspacing="0" style="width: 100%">
                    <tr>
                        <td align="center" style="width:80%;">
                            <b><font color="white" size="80px">MOTOTRANSPORTAR S.A.S</font></b>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divPagina">
        <br />
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px">
            <thead>
                <tr>
                    <th colspan="3" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="GENERACION CERTIFICADOS EMPRESAS DE LA ORGANIZACIÓN" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td colspan="3" valign="top" align="center" style="width: 15%;">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="Label2" runat="server" ForeColor="Red" Font-Bold="true" Text="SELECCIONAR TIPO DE CERTIFICADO Y PERIODO"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center" style="width: 15%;">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblTipo" runat="server" Text="Tipo Certificado *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblPeriodo" runat="server" Text="Periodo *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="Label3" runat="server" Text="Bimestre *" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboTipo" Width="60%" runat="server">
                        
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
                    <asp:DropDownList ID="cboBimestre" Width="60%" runat="server" Visible="false">
                        <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="6">6</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center" style="width: 15%;">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center">
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar Certificados" />
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center" style="width: 15%;">
                    &nbsp;&nbsp;&nbsp;&nbsp;
                </td>
            </tr>
        </table>
        <br />
        <p style="page-break-after:always;"></p>
        <table cellpadding="0" cellspacing="0" class="StyleTable1" align="center" width="700px">
            <tr>
                <td valign="top" align="center" style="width: 100%;">
                    <asp:GridView ID="gridCertificados" runat="server" DataKeyNames="id_emor, bimestre_dace" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                        <AlternatingRowStyle BackColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundField DataField="nombre_tice" HeaderText="Tipo Certificado" />
                            <asp:BoundField DataField="nombre_emor" HeaderText="Empresa" />
                            <asp:BoundField DataField="periodo_dace" HeaderText="Periodo" />
                            <asp:BoundField DataField="bimestre_dace" HeaderText="Bimestre" />
                            <asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />
                        </Columns>
                        <%--<HeaderStyle CssClass="gridheader" />--%>
                        <%--<RowStyle CssClass="gridfilas" />--%>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
