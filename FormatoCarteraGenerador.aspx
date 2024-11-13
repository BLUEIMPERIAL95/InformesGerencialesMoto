<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormatoCarteraGenerador.aspx.vb" Inherits="FormatoCarteraGenerador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <%--<div style="display:none;">
            <asp:HiddenField ID="hidcorreo" Value="0" runat="server" />
        </div>--%>
        <div id="principal" runat="server">
            <table id="Table1" runat="server" width="80%" border="0">
                <tr>
                    <td align="justify"><b><font bol size="12px">Cordial Saludo</font></b></td>
                </tr>
                <tr>
                    <td align="justify"><b><font size="12px">Esta es la cartera pendiente por cancelar luego de aplicar su último pago. Agradezco la revisión del mismo, y en caso de presentar diferencias, <u>por favor informarlas al correo cartera@mototransportar.com.co. </u></font></b></td>
                </tr>
                <tr>
                    <td align="justify"><b><font size="12px">Igualmente a este correo enviar los soportes de pago informando los números de facturas canceladas o al <u>whatsapp 3103723816. Este es un correo automático, por favor no responder.</u> </font></b></td>
                </tr>
            </table>
            <br />
            <table id="tbl_encabezado" width="100%" border="0" runat="server">
              <tbody>
				<tr>
                    <td align="center"><b><font size="17px">FORMATO CARTERA X ASESOR HASTA FECHA </font><font id="lbl_fecha" runat="server" size="12px"></font><font id="lbl_correo" runat="server" size="12px" style></font></b></td>
			    </tr>
                <tr>
                    <td align="center"><b><font size="17px">Cartera X Asesor: </font><font id="lbl_asesor" runat="server" size="12px"></font></b></td>
			    </tr>
                <tr>
                    <td align="center"><b><font id="lbl_nombre_cliente" runat="server" size="12px"></font><font id="lbl_nit" runat="server" size="12px"></font><font id="lbl_telefono" runat="server" size="12px"></font></b></td>
			    </tr>
                </tbody>
            </table>
            <br />
            <table id="tbl_detalle" runat="server" width="100%" border="0">
                <tr>
                    <td align="left"><b><font size="12px"></font></b></td>
                    <td align="center"><b><font size="12px">Factura</font></b></td>
                    <td align="left"><b><font size="12px">Sucursal</font></b></td>
                    <td align="center"><b><font size="12px">Fecha</font></b></td>
                    <td align="center"><b><font size="12px">Vence</font></b></td>
                    <td align="right"><b><font size="12px">Vr.Factura</font></b></td>
                    <td align="right"><b><font size="12px">Vr.Abono</font></b></td>
                    <td align="right"><b><font size="12px">Vr.Saldo</font></b></td>
                    <td align="right"><b><font size="12px">Días</font></b></td>
                </tr>
            </table>
            <br />
            <table id="tbl_totales" runat="server" width="100%" border="1">
                <tr bgcolor="#BDBDBD">
                    <td align="center" colspan="3"><b><font size="12px">TOTALES</font></b></td>
                </tr>
                <tr>
                    <td align="left"><b><font size="12px">TOTAL FACTURA </font></b><font id="lbl_total_factura" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">TOTAL ABONO </font></b><font id="lbl_total_abono" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">TOTAL SALDO </font></b><font id="lbl_total_saldo" runat="server" size="12px"></font></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
