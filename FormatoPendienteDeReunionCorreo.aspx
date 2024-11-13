<%@ Page Language="VB" AutoEventWireup="false" CodeFile="FormatoPendienteDeReunionCorreo.aspx.vb" Inherits="FormatoPendienteDeReunionCorreo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Formato Impresion Pendiente Reunion</title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="principal" runat="server">
            <table width="100%" border="1">
              <tbody>
				<tr bgcolor="#BDBDBD">
                    <td align="center"><b><font size="17px">FORMATO DE IMPRESION PENDIENTE REUNIÓN</font></b></td>
			    </tr>
                </tbody>
            </table>
            <br />
            <table id="Table3" runat="server" width="100%" border="1">
                <tr bgcolor="#BDBDBD">
                    <td align="center" colspan="3"><b><font size="12px">INFORMACIÓN REUNIÓN</font></b></td>
                </tr>
                <tr>
                    <td align="left"><b><font size="12px">CODIGO: </font></b><font id="lbl_codreunion" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">NOMBRE: </font></b><font id="lbl_nomreunion" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">FECHA: </font></b><font id="lbl_fecreunion" runat="server" size="12px"></font></td>
                </tr>
            </table>
            <br />
            <table id="Table1" runat="server" width="100%" border="1">
                <tr bgcolor="#BDBDBD">
                    <td align="center" colspan="3"><b><font size="12px">INFORMACIÓN PENDIENTE</font></b></td>
                </tr>
                <tr>
                    <td align="left"><b><font size="12px">CODIGO: </font></b><font id="lbl_codpendiente" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">NOMBRE: </font></b><font id="lbl_nompendiente" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">FECHA: </font></b><font id="lbl_fecpendiente" runat="server" size="12px"></font></td>
                </tr>
                <tr>
                    <td align="left"><b><font size="12px">ESTADO: </font></b><font id="lbl_estpendiente" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">PRIORIDAD: </font></b><font id="lbl_pripendiente" runat="server" size="12px"></font></td>
                    <td align="left"><b><font size="12px">PLAZO: </font></b><font id="lbl_plapendiente" runat="server" size="12px"></font></td>
                </tr>
                <tr>
                    <td align="left" colspan="3"><b><font size="12px">DESCRIPCIÓN: </font></b><font id="lbl_despendiente" runat="server" size="12px"></font></td>
                </tr>
            </table>
            <br />
            <table id="tbl_responsables" runat="server" width="100%" border="1">
                <tr bgcolor="#BDBDBD">
                    <td align="center" colspan="3"><b><font size="12px">RESPONSABLES PENDIENTE</font></b></td>
                </tr>
                <tr>
                    <td align="left"><b><font size="12px">DOCUMENTO</font></b></td>
                    <td align="left"><b><font size="12px">NOMBRE</font></b></td>
                    <td align="center"><b><font size="12px">FECHA ASIGNACION</font></b></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
