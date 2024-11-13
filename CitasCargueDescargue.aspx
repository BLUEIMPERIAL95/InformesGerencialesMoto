<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CitasCargueDescargue.aspx.vb" Inherits="CitasCargueDescargue" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript">
        function fnDatos(strHora, intSede, intEspacio, intTipo, strClase1, strClase2, strFecha, intRese) {
            if (intEspacio != '0') {
                if (intRese == '0') {
                    var FechaActual;
                    FechaActual = new Date();
                    var dtmfecha = new Date(strFecha + ' ' + strHora);
                    var FechaActual = new Date(document.getElementById("MainContent_txtFechaActual").value + ' ' + FechaActual.getHours() + ':' + FechaActual.getMinutes());
                    //alert(dtmfecha + ' - ' + FechaActual);
                    if (dtmfecha >= FechaActual) {
                        //alert(strHora + ' - ' + intSede + ' - ' + intEspacio + ' - ' + intTipo + ' - ' + strFecha + ' - ' + strFecha + ' - ' + intRese);
                        $('body').RunAbriPopupRese(strHora, intSede, intEspacio, intTipo, strClase1, strClase2, strFecha, intRese);
                    }
                    else {
                        alert("No se pueden crear ni modificar reservas o bloqueos de días anteriores.");
                    }
                } else {
                    $('body').RunAbriPopupRese(strHora, intSede, intEspacio, intTipo, strClase1, strClase2, strFecha, intRese);
                }
            }
        }

        jQuery.fn.RunAbriPopupRese = function (strHora, intSede, intEspacio, intTipo, strClase1, strClase2, strFecha, intRese) {
            $('<div id="frmModalPopup" class="frmModalPopup" align="center"><iframe frameborder=0 src="CitasCargueDescargueIngresarDatos.aspx?Hora=' + strHora + '&Sede=' + intSede + '&Muelle=' + intEspacio + '&Tipo=' + intTipo + '&Clase1=' + strClase1 + '&Clase2=' + strClase2 + '&Fecha=' + strFecha + '&Reserva=' + intRese + '" width="800" height="400" scrolling="no"/></div>').dialog({ modal: true, width: 800, height: 400, close: function (event, ui) { var button; button = document.getElementById("MainContent_btnOculto"); button.click(); } });
            return false;
        };
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div>
    <table cellpadding="0" cellspacing="0"  border="0" width="600px" align="center" class="StyleTable1">
        <thead>
            <tr>
                <th colspan=4 valign="top" align="center">
                    <asp:Label ID="TableTitleLabel" runat="server" Text="Parámetros Citas Cargue y Descargue" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                </th>
            </tr>
        </thead>
        <tr>
            <td align=right width="50%"><b>&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                &nbsp;&nbsp;
                <br />
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>Sede:&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                <asp:DropDownList ID="cboSede" Width="90%" runat="server" AutoPostBack="true">
                        
                </asp:DropDownList>
                <font color="red">*</font>
                <br />
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>Muelle:&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                <asp:DropDownList ID="cboMuelle" Width="90%" runat="server" AutoPostBack="true">
                        
                </asp:DropDownList>
                <font color="red">*</font>
                <br />
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>Tipo:&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                <asp:DropDownList ID="cboTipo" Width="90%" runat="server" AutoPostBack="true">
                    <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                    <asp:ListItem Value="1">CARGUE</asp:ListItem>
                    <asp:ListItem Value="2">DESCARGUE</asp:ListItem>
                </asp:DropDownList>
                <font color="red">*</font>
                <br />
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>Clase:&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                <asp:DropDownList ID="cboClase1" Width="0%" runat="server" Visible="false">
                    <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                    <asp:ListItem Value="NACIONAL">NACIONAL</asp:ListItem>
                    <asp:ListItem Value="EXPORTACION">EXPORTACION</asp:ListItem>
                    <asp:ListItem Value="ULTIMA MILLA">ULTIMA MILLA</asp:ListItem>
                    <asp:ListItem Value="MEIIS">MEIIS</asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="cboClase2" Width="0%" runat="server" Visible="false">
                    <asp:ListItem Value="0" Selected="True">- SELECCIONE -</asp:ListItem>
                    <asp:ListItem Value="FAGRAVE">FAGRAVE</asp:ListItem>
                    <asp:ListItem Value="BUGA">BUGA</asp:ListItem>
                    <asp:ListItem Value="IMPORTACION">IMPORTACION</asp:ListItem>
                    <asp:ListItem Value="MEIIS">MEIIS</asp:ListItem>
                    <asp:ListItem Value="ACEGRASAS">ACEGRASAS</asp:ListItem>
                    <asp:ListItem Value="MAQUILAS">MAQUILAS</asp:ListItem>
                </asp:DropDownList>
                <font color="red">*</font>
                <br />
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>Fecha:&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%">
                <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFecha" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFecha"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy/MM/dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
            </td>
        </tr>
        <tr>
            <td align=right width="50%"><b>&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%" align="right">
                &nbsp;&nbsp;
                <br />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <br />
            </td>
        </tr>
        <tr>
            <td width="50%"><b>&nbsp;&nbsp;<br />
                </b></td>
            <td width="50%" align=right >
                <font color="red">*</font><font color="red" size="1px">&nbsp;Obligatorio</font>
                <br />
            </td>
        </tr>
    </table>
    <br />
    <table cellpadding="0" cellspacing="0"  border="0" width="800px" align="center" class="StyleTable1">
        <thead>
            <tr>
                <th valign="top" align="center">
                    <asp:Label ID="Label1" runat="server" Text="Detalle de Citas" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                </th>
            </tr>
        </thead>
        <tr>
            <td>
                &nbsp;&nbsp;
                <br />
            </td>
        </tr>
        <tr>
            <td align="center">
                <div id="tblConvencion" runat="server" >
                
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:table id="TablaFormas" runat="server" HorizontalAlign="Center" GridLines="Both" class="StyleTable1"></asp:table>
                <br />
            </td>
        </tr>
    </table>
    </div>
    <div id="divOcultos" style="display: none;">
        <asp:Button ID="btnOculto" runat="server" Text="Button" />
        <asp:TextBox ID="txtFechaActual" runat="server" MaxLength="20" />
    </div> 
</asp:Content>
