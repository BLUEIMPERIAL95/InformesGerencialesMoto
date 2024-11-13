<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Reuniones.aspx.vb" Inherits="Reuniones" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 82%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidReunion" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="ADMINISTRACION DE REUNIONES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width:30%;">
                    <asp:Label ID="lblBuscar" runat="server" Text="Buscar por: "></asp:Label>
                    <asp:RadioButton ID="rdCodigo" runat="server" Text="Codigo" Checked="true" AutoPostBack="true" />
                    <asp:RadioButton ID="rdNombre" runat="server" Text="Nombre" AutoPostBack="true" />
                    <asp:RadioButton ID="rdFecha" runat="server" Text="Fecha" AutoPostBack="true" />
                </td>
                <td valign="top" align="left" style="width:60%;">
                    <asp:TextBox ID="txtBuscar" runat="server" MaxLength="100" Width="70%"></asp:TextBox>&nbsp;&nbsp;
                    <input id="txtFecha" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 15%;" type="text" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" Enabled="false" DatePickerMode="true" TextBoxId="MainContent_txtFecha"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" style="width:10%;">
                    <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="images/busqueda.png" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 100%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridReuniones" runat="server" DataKeyNames="id_reun" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="codigo_reun" HeaderText="Codigo" />
                                <asp:BoundField DataField="fecha_reun" HeaderText="Fecha" />
                                <asp:BoundField DataField="nombre_reun" HeaderText="Nombre" />
                                <asp:BoundField DataField="activo_reun" HeaderText="Activa" />
                                <asp:ButtonField CommandName="imprimir" ButtonType="Image" ImageUrl="images/pdf4.png" />
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

