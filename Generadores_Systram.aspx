<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Generadores_Systram.aspx.vb" Inherits="Generadores_Systram" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style1 {
            width: 121%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000">
        
    </asp:ScriptManager>
    <div id="divPagina">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div style="display:none;">
            <asp:HiddenField ID="hidtercero" Value="0" runat="server" />
        </div>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="GENERADORES" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbltipodoc" runat="server" Text="T.Doc. *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboTipo" Width="120px" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1" Selected>Seleccione</asp:ListItem>
                        <asp:ListItem Value="Cedula de Ciudadania">Cedula de Ciudadania</asp:ListItem>
                        <asp:ListItem Value="Cedula de Extranjeria">Cedula de Extranjeria</asp:ListItem>
                        <asp:ListItem Value="Nit">Nit</asp:ListItem>
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
                    <asp:TextBox ID="txtNombre" runat="server" Width="450px" MaxLength="200" style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label3" runat="server" Text="Nombre 2"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtNombre2" runat="server" Width="450px" MaxLength="200" style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label4" runat="server" Text="Apelli 1 *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtApellido1" runat="server" Width="450px" MaxLength="200" style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="Label5" runat="server" Text="Apelli 2 *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtApellido2" runat="server" Width="450px" MaxLength="200" style="text-transform: uppercase"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbldireccion" runat="server" Text="Dirección *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDireccion" runat="server" Width="450px" MaxLength="200"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbltelefono" runat="server" Text="Teléfono *"></asp:Label>
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
                    <asp:Label ID="lblcorreo" runat="server" Text="E-mail *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtCorreo" runat="server" Width="450px" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblZona" runat="server" Text="Zona *"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboZonas" Width="450px" runat="server" AutoPostBack="true">
                        
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
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" border="0" align="center" width="880px">
            <tr>
                <td valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" />
                    <asp:Button ID="btnEnviarCorreo" runat="server" Text="Enviar Cartera" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                    <asp:Button ID="btPrueba" runat="server" Text="Prueba" />
                </td>
            </tr>
        </table>
        <%--<table cellpadding="2" cellspacing="0" border="0" align="center" width="880px">
            <tr>
                <td valign="top" align="center">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                        <ProgressTemplate>
                            <div>
                                <img src="images/cortinilla2.gif" alt="Loading" border="0" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>--%>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td valign="top" align="left" style="width:30%;">
                    <asp:Label ID="lblBuscar" runat="server" Text="Buscar por: "></asp:Label>
                    <asp:RadioButton ID="rdDocumento" runat="server" Text="Documento" Checked="true" AutoPostBack="true" />
                    <asp:RadioButton ID="rdNombre" runat="server" Text="Nombre" AutoPostBack="true" />
                </td>
                <td valign="top" align="left" style="width:50%;">
                    <asp:TextBox ID="txtBuscar" runat="server" MaxLength="100" Width="70%"></asp:TextBox>&nbsp;&nbsp;
                </td>
                <td valign="top" align="left" style="width:10%;">
                    <asp:ImageButton ID="imgBuscar" runat="server" ImageUrl="images/busqueda.png" />
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top" align="center" style="width:100%;">
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
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>