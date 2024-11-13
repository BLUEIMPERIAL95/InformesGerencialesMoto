<%@ Page Language="VB" AutoEventWireup="false" CodeFile="RegistroEntrada.aspx.vb" Inherits="RegistroEntrada" MasterPageFile="~/Site.Master" %>

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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div style="display:none;">
        <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="700px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="REGISTRO ENTRADA PARA VISITANTES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbltipodoc" runat="server" Text="T.Doc. *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:DropDownList ID="cboTipo" Width="97%" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                        <asp:ListItem Value="Cedula de Ciudadania" Selected>Cedula de Ciudadania</asp:ListItem>
                        <asp:ListItem Value="Cedula de Extranjeria">Cedula de Extranjeria</asp:ListItem>
                        <asp:ListItem Value="Tarjeta de Identidad">Tarjeta de Identidad</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbldocumento" runat="server" Text="Documento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:TextBox ID="txtDocumento" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)" Width="95%" AutoPostBack="true"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 25%;" rowspan="7">
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblnombre" runat="server" Text="Nombre *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtNombre" runat="server" Width="98%" MaxLength="200" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblTelefono" runat="server" Text="Teléfono *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="50" Text="" onkeypress="javascript:return solonumeros(event)" Width="95%" AutoPostBack="true"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCorreo" runat="server" Text="Correo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCorreo" runat="server" MaxLength="100" Text="" Width="95%" AutoPostBack="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblEps" runat="server" Text="Eps *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboEps" Width="97%" runat="server">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblArl" runat="server" Text="Arl *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboArl" Width="97%" runat="server">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblTemperatura" runat="server" Text="Temperatura"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtTemperatura" runat="server" MaxLength="50" Text="0" onkeypress="javascript:return solonumeros(event)" Width="95%" AutoPostBack="true"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblArea" runat="server" Text="Area *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboArea" Width="97%" runat="server">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:ImageButton ID="imgPdf" runat="server" Height="25px" ImageUrl="images/pdf4.png" Width="25px" />
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:CheckBox ID="chkpoliticas" runat="server" Text="Aceptar politicas de privacidad"/>
                </td>
            </tr>
            <tr>
                <td colspan="4" valign="top" align="center">
                    <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                        <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="center" style="width:100%;">
                    <div style="width: 95%; height: 280px; overflow: scroll">
                        <asp:GridView ID="gridRegistro" runat="server" DataKeyNames="" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="documento_TERC" HeaderText="Documento" />
                                <asp:BoundField DataField="nombre_TERC" HeaderText="Nombre" />
                                <asp:BoundField DataField="fechaent_REEN" HeaderText="Entrada" />
                                <asp:BoundField DataField="fechasal_REEN" HeaderText="Salida" />
                                <asp:BoundField DataField="empresa_TERC" HeaderText="Empresa" />
                                <asp:BoundField DataField="temperatura_REEN" HeaderText="Temperatura" />
                            </Columns>
                            <%--<HeaderStyle CssClass="gridheader" />--%>
                            <%--<RowStyle CssClass="gridfilas" />--%>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
