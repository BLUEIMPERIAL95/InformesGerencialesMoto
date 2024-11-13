<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CadEnvioListado.aspx.vb" Inherits="CadEnvioListado" MasterPageFile="~/Site.Master" %>

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
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1100px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="CAD ENVIO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width:10%;">
                    <asp:ImageButton ID="imgNuevo" runat="server" ImageUrl="images/new_page.png" />
                </td>
                <td valign="top" align="left" style="width:30%;">
                    <asp:Label ID="lblBuscar" runat="server" Text="Buscar por: "></asp:Label>
                    <asp:RadioButton ID="rdNumero" runat="server" Text="Numero" Checked="true" AutoPostBack="true" />
                    <asp:RadioButton ID="rdUsuario" runat="server" Text="Usuario" AutoPostBack="true" />
                </td>
                <td valign="top" align="left" style="width:50%;">
                    <asp:TextBox ID="txtBuscar" runat="server" MaxLength="100" Width="70%"></asp:TextBox>&nbsp;&nbsp;
                </td>
                <td valign="top" align="left" style="width:10%;">
                    <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="images/busqueda.png" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="1100px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 100%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridCadEnvio" runat="server" DataKeyNames="id_caen, estado_caen" AutoGenerateColumns="False" CssClass="mGrid" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="numero_caen" HeaderText="Numero" />
                                <asp:BoundField DataField="fecha_caen" HeaderText="Fecha" />
                                <asp:BoundField DataField="nombre_emor" HeaderText="Empresa" />
                                <asp:BoundField DataField="nombre_agcc" HeaderText="Agencia" />
                                <asp:BoundField DataField="estado_caen" HeaderText="Estado" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/deleted.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
