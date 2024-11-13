<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RespuestaReunionPendienteTercero.aspx.vb" Inherits="RespuestaReunionPendienteTercero" %>

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
                    <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1000px">
                        <thead>
                            <tr>
                                <th colspan="6" valign="top" align="center">
                                    <asp:Label ID="Label2" runat="server" Text="RESPUESTAS PENDIENTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="Label3" runat="server" Text="Codigo *"></asp:Label>
                            </td>
                            <td valign="top" align="left" style="width: 20%;" Width="100%">
                                <asp:TextBox ID="txtCodigoPen" runat="server" MaxLength="20" Width="100%" Enabled="false"></asp:TextBox>
                            </td>
                            <td valign="top" align="left" style="width: 10%;">
                                <asp:Label ID="Label5" runat="server" Text="Prioridad *"></asp:Label>
                            </td>
                            <td valign="top" align="left" style="width: 23%;">
                                <asp:DropDownList ID="cboPrioridad" Width="100%" runat="server" Enabled="false">
                                    <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="BAJA">BAJA</asp:ListItem>
                                    <asp:ListItem Value="MEDIA">MEDIA</asp:ListItem>
                                    <asp:ListItem Value="ALTA">ALTA</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td valign="top" align="left" style="width: 10%;">
                                <asp:Label ID="Label6" runat="server" Text="Estado *"></asp:Label>
                            </td>
                            <td valign="top" align="left" style="width: 25%;">
                                <asp:DropDownList ID="cboEstado" Width="100%" runat="server">
                                    <asp:ListItem Value="0" Selected>SELECCIONE</asp:ListItem>
                                    <asp:ListItem Value="ASIGNADO">ASIGNADO</asp:ListItem>
                                    <asp:ListItem Value="PRECERRADO">PRECERRADO</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="Label8" runat="server" Text="Plazo *"></asp:Label>
                            </td>
                            <td valign="top" align="left" style="width: 14%;">
                                <input id="txtFechaFin" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                                <obout:Calendar ID="CalendarFechaFin" runat="server" DatePickerMode="true" TextBoxId="MainContent_Wizard1_txtFechaFin"
                                    ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                    MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                    CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                                </obout:Calendar>
                            </td>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="Label11" runat="server" Text="Nombre *"></asp:Label>
                            </td>
                            <td valign="top" align="left" colspan="3">
                                <asp:TextBox ID="txtNombrePen" runat="server" MaxLength="100" Width="99%" TextMode="MultiLine" Height="40px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="Label7" runat="server" Text="Descripción *"></asp:Label>
                            </td>
                            <td valign="top" align="left" colspan="5">
                                <asp:TextBox ID="txtDesPendiente" runat="server" MaxLength="1000" Width="99%" TextMode="MultiLine" Height="80px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="lblRespuestasAnteriores" runat="server" Text="Historico Respuestas"></asp:Label>
                            </td>
                            <td valign="top" align="left" colspan="5">
                                <asp:TextBox ID="txtRespuestasAnteriores" runat="server" MaxLength="10000" Width="99%" TextMode="MultiLine" Height="80px" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left" style="width: 12%;">
                                <asp:Label ID="Label1" runat="server" Text="Nueva Respuesta *"></asp:Label>
                            </td>
                            <td valign="top" align="left" colspan="5">
                                <asp:TextBox ID="txtRespuesta" runat="server" MaxLength="200" Width="99%" TextMode="MultiLine" Height="80px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" valign="top" align="center">
                                <asp:Button ID="btnSalvarRespuesta" runat="server" Text="Salvar Respuesta" Enabled="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            <%--</div>        
        </div>
        <div class="popup-overlay"></div>--%>
    </form>
</body>
</html>
