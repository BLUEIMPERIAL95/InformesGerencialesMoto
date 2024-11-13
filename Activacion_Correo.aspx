<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Activacion_Correo.aspx.vb" Inherits="Activacion_Correo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@9" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0/jquery.min.js" type="text/javascript"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/jquery-modal/0.9.1/jquery.modal.min.css" />
</head>
<body>
    <form id="form1" runat="server">
        <%--<div style="display:none;">
            <asp:HiddenField ID="hidcorreo" Value="0" runat="server" />
        </div>--%>
        <div id="divPendientes" runat="server">
            
        </div>
    </form>
</body>
</html>