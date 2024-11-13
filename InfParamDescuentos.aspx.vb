Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text

Partial Class InfParamDescuentos
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios

    Private Sub InfParamDescuentos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(5, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnGenerar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerar.Click
        Dim strSQL As String
        Dim dtter As New DataTable

        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            strSQL = "select completar_ceros(movimientos_transportes.numero,8) as 'Numero Mvto',"
            strSQL += " movimientos_transportes.fecha_movimiento,"
            strSQL += " movimientos_transportes.flete_final_empresa as 'Fte Empresa',"
            strSQL += " movimientos_transportes.flete_final_tercero as 'Fte Tercero', "
            strSQL += " CONCAT(gen.nombre1,' ',gen.nombre2,' ',gen.apellido1,' ',gen.apellido2) as 'Generador',"
            strSQL += " gen.documento as 'Nit Generador',"
            strSQL += " vehiculos.placa as 'Placa',"
            strSQL += " CONCAT(cond.nombre1,' ',cond.nombre2,' ',cond.apellido1,' ',cond.apellido2) as 'Conductor',"
            strSQL += " cond.documento as 'Cedula Cond',"
            strSQL += " sistema_sucursales.sucursal as 'Sucursal', "
            strSQL += " tipo_despachos.descripcion as 'Tipo Operacion',"
            strSQL += " ROUND(((movimientos_transportes.flete_final_tercero*2.50)/100),0) as 'Seguro 2.5%',"
            strSQL += " movimientos_transportes_descuentos.valor as 'Fondo Descontado',"
            strSQL += " ROUND((((movimientos_transportes.flete_final_tercero*0.40)/100)*0.16),0) as 'Iva16%',"
            strSQL += " (select busq_zonas.zonastandar from busq_zonas where busq_zonas.idzona = movimientos_transportes_detalles.zonas_idzonas_origen) as 'Origen',"
            strSQL += " (select busq_zonas.zonastandar from busq_zonas where busq_zonas.idzona = movimientos_transportes_detalles.zonas_idzonas_destino) as 'Destino',"
            strSQL += " sum(movimientos_transportes_detalles.peso_final) as 'Peso KG',"
            strSQL += " completar_ceros(ventas.numero,8) as 'Factura', "
            strSQL += " ventas.fecha as 'Fecha'"
            strSQL += " from(movimientos_transportes)"
            strSQL += " left join generadores on generadores.idgeneradores = movimientos_transportes.generadores_idgeneradores"
            strSQL += " left join terceros gen on gen.idterceros = generadores.terceros_idterceros"
            strSQL += " left join generadores_asesores on generadores_asesores.idgeneradores_asesores = movimientos_transportes.generadores_asesores_idgeneradores"
            strSQL += " left join usuarios desp on desp.idusuarios = movimientos_transportes.usuarios_idusuarios_ingresa"
            strSQL += " left join usuarios on usuarios.idusuarios = generadores_asesores.usuarios_idusuarios"
            strSQL += " left join terceros ase on ase.idterceros = usuarios.idterceros"
            strSQL += " left join movimientos_transportes_actores on movimientos_transportes_actores.idmovimientos_transportes_actores = movimientos_transportes.movimientos_transportes_actores"
            strSQL += " left join vehiculos_carga on vehiculos_carga.idvehiculos_carga = movimientos_transportes_actores.vehiculos_carga_idvehiculos_carga"
            strSQL += " left join vehiculos on vehiculos.idvehiculos = vehiculos_carga.vehiculos_idvehiculos "
            strSQL += " left join terceros_conductores on  terceros_conductores.idterceros_conductores = movimientos_transportes_actores.terceros_conductores_idterceros_conductores"
            strSQL += " left join terceros cond on cond.idterceros = terceros_conductores.terceros_idterceros"
            strSQL += " left join sistema_sucursales on sistema_sucursales.idsucursales = movimientos_transportes.sistema_sucursales_origina"
            strSQL += " left join tipo_despachos on tipo_despachos.idtipo_despachos = movimientos_transportes.tipo_despachos_idtipo_despachos"
            strSQL += " left join movimientos_transportes_detalles on movimientos_transportes_detalles.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
            strSQL += " left join ventas_control on (ventas_control.movimientos_transportes_id = movimientos_transportes.idmovimientos_transportes and ventas_control.idel = 0)"
            strSQL += " left join ventas_detalles on (ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id and ventas_detalles.idel = 0)"
            strSQL += " left join ventas	on ventas.idventas = ventas_detalles.ventas_idventas"
            strSQL += " left join  movimientos_transportes_descuentos on movimientos_transportes_descuentos.movimientos_transportes_idmovimientos_transportes = movimientos_transportes.idmovimientos_transportes "
            strSQL += " left join tipo_descuentos_transportes on tipo_descuentos_transportes.idtipo_descuentos_transportes = movimientos_transportes_descuentos.tipo_descuentos_transportes_idtipo_descuentos_transportes "
            strSQL += " where movimientos_transportes.fecha_movimiento BETWEEN '"
            strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "'"
            strSQL += " and (tipo_descuentos_transportes.descripcion_interna like '%TRAYE%') "
            strSQL += " and movimientos_transportes.flete_final_tercero > 0 and movimientos_transportes.tipo_estados_idtipo_estados <> 9"
            strSQL += " and movimientos_transportes_descuentos.idel = 0"
            strSQL += " GROUP BY movimientos_transportes.idmovimientos_transportes"

            dtter = csinformes.ejecutar_query_bd(strSQL)

            gridDescuentos.DataSource = dtter
            gridDescuentos.DataBind()
        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Dim name = "Descuentos"

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Dim page As New Page()
        Dim form As New HtmlForm()

        gridDescuentos.EnableViewState = False

        ' Deshabilitar la validación de eventos, sólo asp.net 2 
        page.EnableEventValidation = False

        ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        page.DesignerInitialize()

        page.Controls.Add(form)
        form.Controls.Add(gridDescuentos)

        page.RenderControl(htw)

        Response.Clear()
        Response.Buffer = True

        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".xls")
        Response.Charset = "UTF-8"


        Response.ContentEncoding = Encoding.[Default]
        Response.Write(sb.ToString())
        Response.[End]()
    End Sub
End Class
