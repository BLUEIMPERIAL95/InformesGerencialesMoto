<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Terceros.aspx.vb" Inherits="Terceros" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
        .auto-style5 {
            height: 7px;
        }
        .auto-style6 {
            width: 53%;
        }
        .auto-style7 {
            width: 55%;
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
                    <th colspan="7" valign="top" align="center" class="auto-style5">
                        <asp:Label ID="Label1" runat="server" Text="TERCEROS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
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
                        <asp:ListItem Value="Tarjeta Identidad">Tarjeta Identidad</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lbldocumento" runat="server" Text="Documento *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDocumento" runat="server" MaxLength="50" onkeypress="javascript:return solonumeros(event)" Width="130px" AutoPostBack="true"></asp:TextBox>
                    <asp:TextBox ID="txtDigito" runat="server" MaxLength="2" onkeypress="javascript:return solonumeros(event)" Enabled="false" Visible="false" Width="7%"></asp:TextBox>
                </td>
                <td valign="top" align="left" rowspan="7" class="auto-style7">
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
                     <asp:Label ID="lblTipoCampo" runat="server" Text="Tipo *"></asp:Label>
                 </td>
                 <td valign="top" align="left" style="width: 15%;">
                     <asp:DropDownList ID="cboTipoCampo" Width="120px" runat="server" AutoPostBack="true">
                          <asp:ListItem Value="Tercero" Selected ="True">TERCERO</asp:ListItem>
                          <asp:ListItem Value="Empleado">EMPLEADO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblactivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboActivo" Width="40%" runat="server">
                        <asp:ListItem Value="1" Selected="True">SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" class="auto-style7">
                    
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    
                </td>
            </tr>           
            <%--<tr>
                <td valign="top" align="center" style="width: 15%;">
                    <input id="txtFechaInicio" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaInicio"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <input id="txtFechaFin" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaFin" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaFin"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
            </tr>--%>
            <%--<tr>
                <td colspan="2" valign="top" align="center" style="width: 15%;">
                    <asp:Label ID="lblManifiesto" runat="server" Text="Tipo"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" align="center">
                    <asp:DropDownList ID="cboTipo" Width="40%" runat="server">
                        <asp:ListItem Value="-1" Selected>Seleccione</asp:ListItem>
                        <asp:ListItem Value="0">Completa</asp:ListItem>
                        <asp:ListItem Value="1">Cartera</asp:ListItem>
                        <asp:ListItem Value="2">Prejurídica</asp:ListItem>
                        <asp:ListItem Value="3">Jurídica</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>--%>
        <%--</table>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">--%>
            <thead>
                <tr>
                    <th colspan="7" valign="top" align="center">
                        <asp:Label ID="Label2" runat="server" Text="DATOS EMPLEADO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboEmpresa" Width="120px" runat="server" AutoPostBack="true" Enabled="false">
                        <asp:ListItem Value="1" Selected ="True">- SELECCIONE -</asp:ListItem>                        
                        <asp:ListItem Value="MOTOTRANSPORTAR">MOTOTRANSPORTAR</asp:ListItem>
                        <asp:ListItem Value="MOTOSEGURIDAD">MOTOSEGURIDAD</asp:ListItem>
                        <asp:ListItem Value="REFRILOGISTICA">REFRILOGISTICA</asp:ListItem>
                        <asp:ListItem Value="TRAMITAR">TRAMITAR</asp:ListItem>
                        <asp:ListItem Value="TRAMITAR LINEA">TRAMITAR LINEA</asp:ListItem>
                        <asp:ListItem Value="CIA CAPRI">CIA CAPRI</asp:ListItem>
                        <asp:ListItem Value="MOTOTRANSPORTAMOS">MOTOTRANSPORTAMOS</asp:ListItem>
                        <asp:ListItem Value="LABOR HEALTH">LABOR HEALTH</asp:ListItem>
                        <asp:ListItem Value="CEFATRANS SAS">CEFATRANS SAS</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblagencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboAgencia" Width="120px" runat="server" Enabled="false">
                        
                    </asp:DropDownList>
                </td>
            </tr>
           
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblArea" runat="server" Text="Area "></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboArea" Width="120px" runat="server" Enabled="false">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblCargo" runat="server" Text="Cargo "></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboCargo" Width="120px" runat="server" Enabled="false">
                        
                    </asp:DropDownList>
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
        <%--<table cellpadding="2" cellspacing="0" border="0" align="center" width="880px">
            <tr>
                <td valign="top" align="center">
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                        <ProgressTemplate>
                            <div>
                                <img src="images/cortinilla2.gif" alt="Loading" border="0" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </td>
            </tr>
        </table>--%>
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
                        <%--<asp:GridView ID="gridTerceros" runat="server" Width="100%" 
                            CellPadding="1" ForeColor="#333333" GridLines="None">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>--%>
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
                                <asp:BoundField DataField="tipo" HeaderText="Tipo" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
                            <%--<HeaderStyle CssClass="gridheader" />--%>
                            <%--<RowStyle CssClass="gridfilas" />--%>
                        </asp:GridView>
                    </div>
                </td>
                <%--<td valign="top" align="center" style="width: 100%;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div id="divmostrar" runat="server" style="width: 850px; height: 350px; overflow: scroll">
                        
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>--%>
                <%--<td valign="top" align="center" style="width: 2%;">
                    <table cellpadding="2" cellspacing="0" border="0" class="StyleTable1" align="center">
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton1" runat="server" Height="25px" ImageUrl="~/images/excel.ico" Width="25px" OnClick="ImageButton1_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 2%;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="25px" ImageUrl="~/images/pdf4.png" Width="25px" />
                            </td>
                        </tr>
                    </table>
                </td>--%>
            </tr>
            <%--<tr>
                <td valign="top" align="center" style="width: 100%; visibility: hidden;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <div id="divinforme" runat="server" style="width: 850px; height: 350px; overflow: scroll">
                        
                    </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
