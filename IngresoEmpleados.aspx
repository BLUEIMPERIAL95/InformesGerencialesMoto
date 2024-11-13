<%@ Page Language="VB" AutoEventWireup="false" CodeFile="IngresoEmpleados.aspx.vb" Inherits="IngresoEmpleados" MasterPageFile="~/Site.Master" %>

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
        <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="EMPLEADOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbltipodoc" runat="server" Text="T.Doc. *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboTipo" Width="120px" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1">Seleccione</asp:ListItem>
                        <asp:ListItem Value="Cedula de Ciudadania" Selected>Cedula de Ciudadania</asp:ListItem>
                        <asp:ListItem Value="Cedula de Extranjeria">Cedula de Extranjeria</asp:ListItem>
                        <asp:ListItem Value="Tarjeta de Identidad">Tarjeta de Identidad</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbldocumento" runat="server" Text="Documento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDocumento" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)" Width="130px" AutoPostBack="true"></asp:TextBox>
                    <asp:TextBox ID="txtDigito" runat="server" MaxLength="2" onkeypress="javascript:return solonumeros(event)" Enabled="false" Visible="false" Width="7%"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 25%;" rowspan="7">
                    <%--<asp:Image ID="imgHuella" runat="server" Height="108px" style="margin-left: 0px" Width="100px" />--%>
                    <%--<asp:ImageMap ID="imgHuella" runat="server"></asp:ImageMap>--%>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblnombre" runat="server" Text="Nombre *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtNombre" runat="server" Width="450px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbldireccion" runat="server" Text="Dirección"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDireccion" runat="server" Width="450px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbltelefono" runat="server" Text="Teléfono"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtTelefono" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblcelular" runat="server" Text="Celular"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCelular" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblcorreo" runat="server" Text="E-mail"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCorreo" runat="server" Width="450px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboEmpresa" Width="120px" runat="server">
                        <asp:ListItem Value="CIACAPRI">CIACAPRI</asp:ListItem>
                        <asp:ListItem Value="MOTOTRANSPORTAMOS" Selected>MOTOTRANSPORTAMOS</asp:ListItem>
                        <asp:ListItem Value="MOTOTRANSPORTAR">MOTOTRANSPORTAR</asp:ListItem>
                        <asp:ListItem Value="MOTOSEGURIDAD">MOTOSEGURIDAD</asp:ListItem>
                        <asp:ListItem Value="REFRILOGISTICA">REFRILOGISTICA</asp:ListItem>
                        <asp:ListItem Value="TRAMITAR">TRAMITAR</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblagencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboAgencia" Width="120px" runat="server">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblArea" runat="server" Text="Area "></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboArea" Width="120px" runat="server">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCargo" runat="server" Text="Cargo "></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboCargo" Width="120px" runat="server">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblactivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboActivo" Width="40%" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    
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
                        <asp:GridView ID="gridTerceros" runat="server" DataKeyNames="id" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="tipodoc" HeaderText="Tipo Documento" />
                                <asp:BoundField DataField="documento" HeaderText="Documento" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <%--<asp:BoundField DataField="direccion" HeaderText="Direccion" />
                                <asp:BoundField DataField="celular" HeaderText="Celular" />
                                <asp:BoundField DataField="correo" HeaderText="Correo" />--%>
                                <asp:BoundField DataField="activo" HeaderText="Activo" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
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
