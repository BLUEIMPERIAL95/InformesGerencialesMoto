<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Formato_Envio_Carta_Generador.aspx.vb" Inherits="Formato_Envio_Carta_Generador" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="principal" runat="server">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_saludo" runat="server" size="14px">Señores</font></td>
            </tr>
            <tr>
                <td align="left"><font id="lbl_Empresa" runat="server" size="14px"></font></td>
            </tr>
            <tr>
                <td align="left"><font id="lbl_ciudad" runat="server" size="14px"></font></td>
            </tr>
        </table>
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="right"><font id="lbl_certificacion" runat="server" size="14px">Asunto: certificación contrato</font></td>
            </tr>
        </table>
        <br />
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="justify"><font id="lbl_cuerpo1" runat="server" size="14px"></font></td>
            </tr>
        </table>
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_cuerpo_3" runat="server" size="14px">El servicio prestado lo calificamos de la siguiente manera:</font></td>
            </tr>
        </table>
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_cuerpo_4" runat="server" size="14px">Excelentes___ Buenos___ Regulares___ Malos___</font></td>
            </tr>
        </table>
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_cuerpo_5" runat="server" size="14px">Código UNSPSC 781018, 781019, 781316</font></td>
            </tr>
        </table>
        <br />
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_cuerpo_6" runat="server" size="14px">La anterior certificación se expide a solicitud del interesado en la fecha </font><font id="lbl_fecha_actual" runat="server" size="14px"></font></td>
            </tr>
        </table>
        <br />
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="lbl_pie" runat="server" size="14px">Cordialmente,</font></td>
            </tr>
        </table>
        <br />
        <br />
        <br />
        <br />
        <table width="80%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td align="left"><font id="Font1" runat="server" size="14px">______________________________________________</font></td>
            </tr>
        </table>
    </div>
    </form>
    
</body>
</html>
