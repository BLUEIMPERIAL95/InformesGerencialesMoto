<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ImportacionValoresDescuentosSystram.aspx.vb" Inherits="ImportacionValoresDescuentosSystram" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/SAGERStyle.css" rel="stylesheet" type="text/css" />
    <%--<link href="Styles/smoothness/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="Scripts/FuncionesVarias.js" type="text/javascript"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px" runat="server" id="tbl_datos">
                <thead>
                    <tr>
                        <th colspan="4" valign="top" align="center">
                            <asp:Label ID="Label1" runat="server" Text="IMPORTACION VALORES DESCUENTOS SYSTRAM" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td valign="top" align="center" style="width: 15%;">
                        <%--<asp:FileUpload ID="FileExcel" runat="server" Width="310px" />--%>
                        <asp:FileUpload ID="FileExcel" runat="server" Width="310px"  />
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center" style="width: 15%;">
                        <asp:Button ID="btnImportar" runat="server" Text="Importar" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
