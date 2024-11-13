<%@ Page Language="VB" AutoEventWireup="false" CodeFile="MaestroEquipos.aspx.vb" Inherits="MaestroEquipos" MasterPageFile="~/Site.Master" %>

<%@ Register Assembly="obout_Interface" Namespace="Obout.Interface" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Grid_NET" Namespace="Obout.Grid" TagPrefix="cc1" %>
<%@ Register Assembly="obout_Calendar2_Net" Namespace="OboutInc.Calendar2" TagPrefix="obout" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    </asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3000"></asp:ScriptManager>
    <div style="display:none;">
        <asp:HiddenField ID="hidequipo" Value="0" runat="server" />
        <asp:HiddenField ID="hidimagen" Value="0" runat="server" />
        <asp:HiddenField ID="hidcodigo" Value="0" runat="server" />
    </div>
    <div id="divPagina">
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label1" runat="server" Text="ARTICULOS" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblTipoEquipo" runat="server" Text="Tipo Art. *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboTipoEquipo" Width="155px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblEmpresa" runat="server" Text="Empresa *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboEmpresa" Width="155px" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1" Selected>Seleccione</asp:ListItem>
                        <asp:ListItem Value="CIA">CiaCapri</asp:ListItem>
                        <asp:ListItem Value="TAM">Mototransportamos</asp:ListItem>
                        <asp:ListItem Value="TAR">Mototransportar</asp:ListItem>
                        <asp:ListItem Value="SEG">Motoseguridad</asp:ListItem>
                        <asp:ListItem Value="REF">Refrilogistica</asp:ListItem>
                        <asp:ListItem Value="TRA">Tramitar</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td rowspan="3" style="width: 35%;">
                    <asp:Image ID="imgEquipo" runat="server" Width="150px" Height="100px" Visible ="false" />
                </td>
            </tr>
            <tr><td valign="top" align="left" style="width: 5%;">
                    <asp:Label ID="lblnombre" runat="server" Text="Nombre *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 30%;">
                    <asp:TextBox ID="txtNombre" runat="server" Width="95%" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 2%;">
                    <asp:Label ID="lblCodigo" runat="server" Text="Codigo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 25%;">
                    <asp:TextBox ID="txtCodigo" runat="server" Width="150px" MaxLength="20" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr><td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblactivo" runat="server" Text="Activo *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboActivo" Width="155px" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblAgencia" runat="server" Text="Agencia *"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboAgencia" Width="155px" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1" Selected>Seleccione</asp:ListItem>
                        <asp:ListItem Value="Capricentro">Capricentro</asp:ListItem>
                        <asp:ListItem Value="Caldas">Caldas</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    
                </td>
                <td valign="top" colspan="3" align="left" style="width: 15%;">
                    
                </td>
                <td valign="top" align="center" style="width: 15%;">
                    <asp:ImageButton ID="imgAnterior" runat="server" ImageUrl="images/anterior.png" ToolTip="Imagen Anterior" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="imgSiguiente" runat="server" ImageUrl="images/siguiente.png" ToolTip="Imagen Siguiente" />
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 5%;">
                    <asp:Label ID="lblObservacion" runat="server" Text="Observacion"></asp:Label>
                </td>
                <td valign="top" colspan="3" align="left" style="width: 60%;">
                    <asp:TextBox ID="txtObservacion" runat="server" MaxLength="500" TextMode="MultiLine" Width="89%"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 35%;">
                    <asp:FileUpload ID="fluImagen" runat="server" Width="74px" />
                    <asp:Button ID="btnSalvarImagen" runat="server" Text="G" />
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px" runat="server" id="tbl_ficha_tecnica" visible="false">
            <thead>
                <tr>
                    <th colspan="4" valign="top" align="center">
                        <asp:Label ID="Label2" runat="server" Text="FICHA TECNICA EQUIPOS DE COMPUTO" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblserie" runat="server" Text="Serie"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtSerie" runat="server" MaxLength="50"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblmarca" runat="server" Text="Marca"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtMarca" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblModelo" runat="server" Text="Modelo"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtModelo" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblSisOperativo" runat="server" Text="S.Operativo"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtSisOperativo" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblSerial" runat="server" Text="Serial"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtSerial" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblPantalla" runat="server" Text="Pantalla"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtPantalla" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblDisco" runat="server" Text="Disco"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDisco" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblBoard" runat="server" Text="Board"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtBoard" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblProcesador" runat="server" Text="Procesador"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtProcesador" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblRam" runat="server" Text="Ram"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtRam" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblMouse" runat="server" Text="Mouse"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtMouse" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblTeclado" runat="server" Text="Teclado"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtTeclado" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblDirMac" runat="server" Text="Dir.MAC"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDirMac" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblDirWifi" runat="server" Text="Dir.WIFI"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtDirWifi" runat="server" MaxLength="100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblColor" runat="server" Text="Color"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtColor" runat="server" MaxLength="100"></asp:TextBox>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblUnidad" runat="server" Text="Unidad"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboUnidad" Width="40%" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" style="width: 15%;">
                    
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblSd" runat="server" Text="Sd"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboSD" Width="40%" runat="server">
                        <asp:ListItem Value="1" Selected>SI</asp:ListItem>
                        <asp:ListItem Value="0">NO</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <br />
        <table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="900px">
            <tr>
                <td colspan="4" valign="top" align="center">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </td>
            </tr>
        </table>
        <%--<table cellpadding="2" cellspacing="0" class="StyleTable1" align="center" width="600px">
            <thead>
                <tr>
                    <th colspan="5" valign="top" align="center">
                        <asp:Label ID="Label3" runat="server" Text="DATOS COMPRA" CssClass="LabelWithBackgroundStyle1"></asp:Label>
                    </th>
                </tr>
            </thead>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFechaCompra" runat="server" Text="Fecha Comp."></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFechaCompra" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="CalendarFechaInicio" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaCompra"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:Label ID="lblNroCompra" runat="server" Text="Nro Compra"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <asp:TextBox ID="txtNroCompra" runat="server" MaxLength="50"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblTercero" runat="server" Text="Tercero"></asp:Label>
                </td>
                <td colspan="3" valign="top" align="left" style="width: 15%;">
                    <asp:DropDownList ID="cboTercero" Width="490px" runat="server" AutoPostBack="true">
                        
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFechaGar1" runat="server" Text="Gar.Desde"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFechaGar1" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="Calendar1" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaGar1"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
                <td valign="top" align="left" class="auto-style2">
                    <asp:Label ID="lblFechaGar2" runat="server" Text="Gar.Hasta"></asp:Label>
                </td>
                <td valign="top" align="left" style="width: 15%;">
                    <input id="txtFechaGar2" runat="server" onclick="javascript: fndisplayCalendar(event);" style="width: 75px" type="text" readonly="readonly" />
                    <obout:Calendar ID="Calendar2" runat="server" DatePickerMode="true" TextBoxId="MainContent_txtFechaGar2"
                        ShowYearSelector="true" ShowMonthSelector="true" YearSelectorType="DropDownList"
                        MonthSelectorType="DropDownList" DateFormat="yyyy-MM-dd" DatePickerImagePath="images/date_picker1.gif"
                        CultureName="es-CO" TitleText="" DateMin="1960/01/01" Columns="1" TextArrowLeft="<<" TextArrowRight=">>">
                    </obout:Calendar>
                </td>
            </tr>
        </table>--%>
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
                <td valign="top" align="center" style="width:100%;">
                    <div style="width: 95%; height: 150px; overflow: scroll">
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
                        <asp:GridView ID="gridEquipos" runat="server" DataKeyNames="id_EQUI" AutoGenerateColumns="False" CellPadding="4" GridLines="None" Width="100%" >
                            <AlternatingRowStyle BackColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:BoundField DataField="codigo_EQUI" HeaderText="Código" />
                                <asp:BoundField DataField="nombre_EQUI" HeaderText="Equipo" />
                                <asp:BoundField DataField="empresa_EQUI" HeaderText="Empresa" />
                                <asp:BoundField DataField="agencia_EQUI" HeaderText="Agencia" />
                                <asp:BoundField DataField="nombre_tieq" HeaderText="Tipo" />
                                <asp:BoundField DataField="activo" HeaderText="Activo" />
                                <asp:ButtonField CommandName="modificar" ButtonType="Image" ImageUrl="images/ver-mas.png" />
                                <asp:ButtonField CommandName="eliminar" ButtonType="Image" ImageUrl="images/Admin_Delete.png" />
                            </Columns>
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
    </div>
</asp:Content>
