<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PermisosModulosUsuarios.aspx.vb" Inherits="PermisosModulosUsuarios" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 121%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidusuario" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="PERMISOS USUARIOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblUsuario" runat="server" Text="Usuario *"></asp:Label>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblModulo" runat="server" Text="Modulo *"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboUsuario" Width="80%" runat="server" AutoPostBack="true">

                    </asp:DropDownList>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:DropDownList ID="cboModulo" Width="80%" runat="server" AutoPostBack="true">

                    </asp:DropDownList>
                    <asp:CheckBox ID="chkTodos" Text="Todos" runat="server" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" style="width:100%;">
                    <div style="width: 95%; height: 333px; overflow: scroll">
                        <asp:GridView ID="grdPermisos" runat="server" DataKeyNames="id_mous" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>

                                <asp:BoundField DataField="strnom1_usua" HeaderText="Usuario" />
                                <asp:BoundField DataField="nombre_modu" HeaderText="Modulo" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                            <%--<HeaderStyle CssClass="gridheader" />--%>
                            <%--<RowStyle CssClass="gridfilas" />--%>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

