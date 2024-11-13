<%@ Page Language="VB" AutoEventWireup="false" CodeFile="TestSaludMovimiento.aspx.vb" Inherits="TestSaludMovimiento" %>

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
    <%--<link href="Styles/LOGINStyle.css" rel="stylesheet" type="text/css" />--%>
    <link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.css" />
    <script src="https://code.jquery.com/jquery-3.2.1.js" type="text/javascript"></script>
    <%--<script type="text/javascript" language="javascript">
        $(document).ready(function () {
            //$('#open').on('click', function(){
            $('#popup').fadeIn('slow');
            $('.popup-overlay').fadeIn('slow');
            $('.popup-overlay').height($(window).height());
            return false;
            //});

            $('#close').on('click', function () {
                $('#popup').fadeOut('slow');
                $('.popup-overlay').fadeOut('slow');
                return false;
            });
        });
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div style="display:none;">
            <asp:HiddenField ID="hidPendiente" Value="0" runat="server" />
            <asp:HiddenField ID="hidTercero" Value="0" runat="server" />
        </div>
        <%--<div id="popup" style="display: none;">
            <div class="content-popup">
                <div class="close"><a href="#" id="close"><img src="images/close.png"/></a></div>--%>
                <div>
                    <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px">
                        <thead>
                            <tr>
                                <th colspan="2" valign="top" align="center">
                                    <asp:Label ID="Label2" runat="server" Text="TEST SALUD CONDUCTORES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblMovimiento" runat="server" Text="Movimiento *"></asp:Label>
                            </td>
                            <td valign="top" align="center" style="width: 20%;" Width="900%">
                                <asp:TextBox ID="txtMovimiento" runat="server" MaxLength="20" Width="100%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblPrimeraPregunta" runat="server" Text="1. ¿Presenta cuadro gripal leve o agudo? *"></asp:Label>
                            </td>
                            <td valign="top" align="center" style="width: 12%;">
                                <asp:Label ID="lblCual" runat="server" Text="Cual? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="center">
                                <asp:DropDownList ID="cboPrimera" runat="server" Width="40%" AutoPostBack="true">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top" align="center">
                                <asp:DropDownList ID="cboCual" runat="server" Width="40%" Enabled="false">
                                    <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="FIEBRE">FIEBRE</asp:ListItem>
                                    <asp:ListItem Value="TOS SECA">TOS SECA</asp:ListItem>
                                    <asp:ListItem Value="DIFICULTAD AL RESPIRAR">DIFICULTAD AL RESPIRAR</asp:ListItem>
                                    <asp:ListItem Value="VARIOS">VARIOS</asp:ListItem>
                                    <asp:ListItem Value="TODOS">TODOS</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblSegunda" runat="server" Text="2. ¿Dolor de garganta? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboSegunda" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblTercera" runat="server" Text="3. ¿Malestar general y dolor muscular que te limite las actividades de la vida diaria? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboTercera" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblCuarta" runat="server" Text="4. ¿Ha tenido contacto con casos sospechosos o confirmados de Covid-19? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboCuarta" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblQuinta" runat="server" Text="5. ¿Fiebre igual o mayor a 38 grados medida con termómetro? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboQuinta" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblSexta" runat="server" Text="6. ¿Tos seca y persistente de inicio reciente? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboSexta" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblSeptima" runat="server" Text="7. ¿Dificultad para respirar de inicio reciente? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboSeptima" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblOctava" runat="server" Text="8. ¿Pérdida del olfato y/o el gusto? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboOctava" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblNovena" runat="server" Text="9. ¿Vives con alguien en proceso de diagnóstico (le ordenaron prueba) o confirmado de tener COVID-19? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboNovena" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblDecima" runat="server" Text="10. ¿En los últimos 14 días has tenido contacto estrecho (por más de 15 minutos, a menos de 2 metros y sin usar elementos de protección personal) con alguien en proceso de diagnóstico (le ordenaron prueba) o confirmado de COVID-19? *"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:DropDownList ID="cboDecima" runat="server" Width="40%">
                                    <asp:ListItem Value="0">SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="NO" Selected>NO</asp:ListItem>
                                    <asp:ListItem Value="SI">SI</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" valign="top" align="center">
                                <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                            </td>
                        </tr>
                    </table>
    </form>
</body>
</html>
