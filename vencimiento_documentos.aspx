<%@ Page Language="VB" AutoEventWireup="false" CodeFile="vencimiento_documentos.aspx.vb" Inherits="vencimiento_documentos" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit"  TagPrefix="asp"%>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style2 {
            width: 14%;
        }
        .auto-style5 {
            width: 100%;
            overflow: scroll;
        }
        .auto-style6 {
            height: 369px;
        }
        .auto-style7 {
            width: 15%;
        }
    </style>
    <script type="text/javascript">
        function GetID(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidtercero').value = HdnKey;
        }

        function GetIDSolicita(source, eventArgs) {
            var HdnKey = eventArgs.get_value();
            document.getElementById('MainContent_hidsolicita').value = HdnKey;
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div id="divPagina">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div style="display:none;">
                    <asp:HiddenField ID="hidvencimiento" Value="0" runat="server" />
                </div>
                <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
                    <thead>
                        <tr>
                            <th colspan="4" valign="top" align="center">
                                <asp:Label ID="Label3" runat="server" Text="VENCIMIENTOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblDocumento" runat="server" Text="Documento *"></asp:Label>
                        </td>
                        <td colspan="3" valign="top" align="left">
                            <asp:TextBox ID="txtDocumento" runat="server" Width="98%" MaxLength="1000"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblFechaExpide" runat="server" Text="Fecha Expide *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <input id="txtFechaExpide" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                            <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaExpide"
                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                            </obout:Calendar>
                        </td>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblFechaVence" runat="server" Text="Fecha Vence *"></asp:Label>
                        </td>
                        <td valign="top" align="left" style="width: 15%;">
                            <input id="txtFechaVence" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                            <obout:Calendar ID="Calendar1" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaVence"
                                ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                                MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                                CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                            </obout:Calendar>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="lblCorreos" runat="server" Text="Correos *" ToolTip="Ingresar Correos válidos separados por el caracter ';'"></asp:Label>
                        </td>
                        <td valign="top" colspan="3" align="left">
                            <asp:TextBox ID="txtCorreos" runat="server" MaxLength="2000" TextMode="MultiLine" Width="98%" ToolTip="Ingresar Correos validos separados por el caracter ';'"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="lblCelulares" runat="server" Text="Celulares *" ToolTip="Ingresar Celulares válidos separados por el caracter ';'"></asp:Label>
                        </td>
                        <td valign="top" colspan="3" align="left">
                            <asp:TextBox ID="txtCelulares" runat="server" MaxLength="2000" TextMode="MultiLine" Width="98%" ToolTip="Ingresar Celulares válidos separados por el caracter ';'"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblDias" runat="server" Text="Aviso(Días) *"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:TextBox ID="txtDias" runat="server" Width="90%" MaxLength="3" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                        </td>
                        <td valign="top" align="left" class="auto-style2">
                            <asp:Label ID="lblActivo" runat="server" Text="Activo *"></asp:Label>
                        </td>
                        <td valign="top" align="left">
                            <asp:DropDownList ID="cboActivo" Width="99%" runat="server">
                                <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" align="left" style="width: 5%;">
                            <asp:Label ID="lblObservacion" runat="server" Text="Observación"></asp:Label>
                        </td>
                        <td valign="top" colspan="3" align="left">
                            <asp:TextBox ID="txtObservacion" runat="server" MaxLength="3000" TextMode="MultiLine" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" valign="top" align="center">
                            <div class="auto-style5">
                                <asp:GridView ID="griVencimientos" runat="server" DataKeyNames="id_vedo" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="90%" >
                                    <AlternatingRowStyle BackColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="nombre_vedo" HeaderText="Documento" />
                                        <asp:BoundField DataField="fexpide_vedo" HeaderText="F.Expide" />
                                        <asp:BoundField DataField="fvence_vedo" HeaderText="F.Vence" />
                                        <asp:BoundField DataField="diasaviso_vedo" HeaderText="Aviso" />
                                        <asp:BoundField DataField="activo_vedo" HeaderText="Activo" />
                                        <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                        <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
