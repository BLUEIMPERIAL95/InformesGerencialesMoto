<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ConceptosNovedades.aspx.vb" Inherits="ConceptosNovedades"  MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 82%;
        }
        .auto-style2 {
            height: 19px;
        }
        .auto-style3 {
            width: 10%;
            height: 23px;
        }
        .auto-style4 {
            height: 23px;
            width: 391px;
        }
        .auto-style13 {
            width: 10%;
        }
        .auto-style14 {
            width: 391px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidconcepto" Value="0" runat="server" />
        <asp:HiddenField ID="hidManejaCC" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center" class="auto-style2">
                        <asp:Label ID="lblConceptos" runat="server" Text="CONCEPTOS NOVEDADES NÓMINA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            
            <tr>                
                <td valign="top" align="left" style="width:10%">
                    <asp:Label ID="lblNombre" runat="server" Text="Nombre *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width:40%">
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                </td>  
                <td valign="top" align="left" style="width:10%">
                    <asp:Label ID="lblReferencia" runat="server" Text="Referencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width:40%">
                    <asp:TextBox ID="txtReferencia" runat="server" MaxLength="200" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width:10%">
                    <asp:Label ID="lblActivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width:40%">
                    <asp:DropDownList ID="cboActivo" Width="90%" runat="server">
                        <asp:ListItem Value="-1" Selected>- SELECCIONE -</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                        <asp:ListItem Value="1">SI</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center" class="auto-style14">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="800px">
            <tr>
                <td valign="top" align="center" class="auto-style1">
                    <div style="width: 95%; height: 333px; overflow: scroll">
                        <asp:GridView ID="gridConceptosFac" runat="server" DataKeyNames="id_cono" AutoGenerateColumns="False" CssClass="mGrid" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="nombre_cono" HeaderText="Nombre" />
                                <asp:BoundField DataField="referencia_cono" HeaderText="Referencia" />
                                <asp:BoundField DataField="activo_cono" HeaderText="Activo" />
                                <asp:ButtonField CommandName="modificar" HeaderText="Modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" HeaderText="Eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
