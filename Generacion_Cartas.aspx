<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Generacion_Cartas.aspx.vb" Inherits="Generacion_Cartas" %>

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
                    <th colspan="2" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="GENERACION DOCUMENTOS EMPLEADOS DE LA ORGANIZACIÓN" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center">
                    <asp:Button ID="btnCartaLaboral" Width="300px" Height="50px" style="background-color: #831802; font-size: 25px; color:white;" runat="server" Text="Carta Laboral" />
                </td>
                <td valign="top" align="center">
                    <asp:Button ID="btnCartaRetiro" Width="300px" Height="50px" style="background-color: #0B12B4; font-size: 25px; color:white;" runat="server" Text="Carta Retiro" />
                </td>
            </tr>
            <%--<tr>
                <td valign="top" align="center">
                    <asp:Button ID="btnIngresosRetenciones" Width="300px" Height="50px" style="background-color: #036E70; font-size: 25px; color:white;" runat="server" Text="Ingresos y Retenciones" />
                </td>
                <td valign="top" align="center">
                    <asp:Button ID="btnReferenciaLaboral" Width="300px" Height="50px" style="background-color: #8F067C; font-size: 25px; color:white;" runat="server" Text="Referencia Laboral" />
                </td>
            </tr>--%>
        </table>
        <br />
        <table runat="server" id="tbl_construccion" visible="false" cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px">
            <tr>
                <td valign="top" align="center">
                    <img src="images/en-construccion-076.gif" alt="Loading" border="0" />
                </td>
            </tr>
        </table>
        <p style="page-break-after:always;"></p>
        <table cellpadding="2" style="visibility:hidden" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <tr>
                <td valign="top" align="center" style="width: 100%; visibility: hidden;">
                    <div id="divinforme" runat="server" style="width: 850px; height: 0px; overflow: scroll">
                        
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
