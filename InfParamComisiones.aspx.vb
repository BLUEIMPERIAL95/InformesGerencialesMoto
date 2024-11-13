Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text

Partial Class InfParamComisiones
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(4, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtase As New DataTable

        'csoper.LlenarDropDownList("Nombre", "id", "cargarasesores", dtase, cboasesores)
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Dim strSQL As String
        Dim dtter As New DataTable

        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            'strSQL = "select completar_ceros(movimientos_transportes.numero,8) as 'Numero Mvto', "
            'strSQL += "movimientos_transportes.fecha_movimiento as 'Fecha Mvto',"
            'strSQL += "movimientos_transportes.flete_final_empresa as 'Fte Empresa',"
            'strSQL += "movimientos_transportes.flete_final_tercero as 'Fte Tercero', "
            'strSQL += "movimientos_transportes.total_anexo_empresa as 'Anexo Empresa',"
            'strSQL += "movimientos_transportes.total_anexo_tercero as 'Anexo Tercero',"
            'strSQL += "COALESCE((SELECT movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)),0) as 'Total Anexo',"
            'strSQL += "round((movimientos_transportes.flete_final_empresa-(movimientos_Transportes.flete_final_tercero+COALESCE((SELECT movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)),0))),0) as 'Intermediacion',"
            'strSQL += "round((((1-(movimientos_transportes.flete_final_tercero+COALESCE((SELECT movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)),0))/movimientos_transportes.flete_final_empresa))*100),2) as '% Intermediacion',"
            'strSQL += "movimientos_transportes_consolidado.generador_nombre as 'Generador',"
            'strSQL += "movimientos_transportes_consolidado.generador_documento as 'Nit Generador',"
            'strSQL += "movimientos_transportes_consolidado.asesor_nombre as 'Asesor Comercial',"
            'strSQL += "movimientos_transportes_consolidado.despachador_nombre as 'Despachador',"
            'strSQL += "movimientos_transportes_consolidado.vehiculo_placa as 'Placa',"
            'strSQL += "movimientos_transportes_consolidado.conductor_nombre as 'Conductor',"
            'strSQL += "movimientos_transportes_consolidado.conductor_documento as 'Cedula Cond',"
            'strSQL += "movimientos_transportes_consolidado.movimiento_modalidad as 'Modalidad Operacion',"
            'strSQL += "movimientos_transportes_consolidado.sucorigina_nombre as 'Sucursal',"
            'strSQL += "movimientos_transportes_consolidado.movimiento_operacion as 'Tipo Operacion',"
            'strSQL += "movimientos_transportes_consolidado.movimiento_origen as 'Origen',"
            'strSQL += "movimientos_transportes_consolidado.movimiento_destino as 'Destino',"
            'strSQL += "movimientos_transportes_detalles.peso_final as 'Peso KG',"
            'strSQL += "sum(movimientos_transportes_detalles.unidades_final) as 'Unidades',"
            'strSQL += "movimientos_transportes_detalles.valor_declarado as 'Vlr Declarado',"
            'strSQL += "movimientos_transportes.total_anticipos as 'Total Anticipo',"
            'strSQL += "completar_ceros(ventas.numero,8) as 'Factura', "
            'strSQL += "ventas.fecha as 'Fecha Factura',"
            'strSQL += "DATEDIFF(ventas.fecha,movimientos_transportes.fecha_elaboracion) as 'Dias Factura-Mvto',"
            'strSQL += "tipo_estados.descripcion as 'Estado Mvto Transporte',"
            'strSQL += "movimientos_contables.numero as 'Recibo',"
            'strSQL += "ventas_recaudos.fecha_elaboracion as 'Fecha Elaborado',"
            'strSQL += "ventas_recaudos.fecha_recaudo as 'Fecha Recaudo',"
            'strSQL += "DATEDIFF(ventas_recaudos.fecha_recaudo,ventas.fecha) as 'Dias RC-Factura',"
            'strSQL += "DATEDIFF(ventas_recaudos.fecha_recaudo,movimientos_transportes.fecha_elaboracion) as 'Dias RC-MvtoTTe',"
            'strSQL += "ventas_recaudos_detalle.valor as 'Valor Pagado',"
            'strSQL += "ventas_recaudos.total as 'Valor Total Recibo',"
            'strSQL += "ventas_recaudos_detalle.descripcion as 'Detalle RC',"
            'strSQL += "ventas_recaudos.observacion as 'Observacion RC'"
            'strSQL += "from movimientos_transportes"
            'strSQL += " inner join movimientos_transportes_consolidado on movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes"
            'strSQL += " inner join movimientos_transportes_actores on movimientos_transportes_actores.idmovimientos_transportes_actores = movimientos_transportes.movimientos_transportes_actores"
            'strSQL += " inner join tipo_estados on tipo_estados.idtipo_estados = movimientos_transportes.tipo_estados_idtipo_estados"
            'strSQL += " inner join movimientos_transportes_detalles on movimientos_transportes_detalles.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
            'strSQL += " left join movimientos_gastos_anexos on movimientos_gastos_anexos.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
            'strSQL += " left join ventas_control on (ventas_control.movimientos_transportes_id = movimientos_transportes.idmovimientos_transportes and ventas_control.idel = 0)"
            'strSQL += " left join ventas_detalles on (ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id and ventas_detalles.idel = 0)"
            'strSQL += " left join ventas on ventas.idventas = ventas_detalles.ventas_idventas"
            'strSQL += " Left Join ventas_consolidado on ventas.idventas = ventas_consolidado.venta_id"
            'strSQL += " left join ventas_recaudos_detalle on ventas_recaudos_detalle.ventas_idventas = ventas.idventas"
            'strSQL += " left join ventas_recaudos on ventas_recaudos.idventas_recaudos = ventas_recaudos_detalle.ventas_recaudos_id"
            'strSQL += " left join movimientos_contables on movimientos_contables.idcontabilidad = ventas_recaudos.movimientos_contables_id"
            'strSQL += " where movimientos_transportes.fecha_movimiento BETWEEN '"
            'strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "'"
            'strSQL += " and movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
            ''If cboasesores.SelectedValue > 0 Then
            ''    strSQL += " And movimientos_transportes_consolidado.asesor_id = " & cboasesores.SelectedValue & ""
            ''End If
            'strSQL += " And ventas_consolidado.venta_abonos = ventas_consolidado.venta_total"
            'strSQL += " GROUP BY movimientos_transportes.idmovimientos_transportes"

            strSQL = "Select completar_ceros(movimientos_transportes.numero, 8) As 'Numero Mvto', "
            strSQL += "movimientos_transportes.fecha_movimiento As 'Fecha Mvto', "
            strSQL += "movimientos_transportes.flete_final_empresa As 'Fte Empresa', "
            strSQL += "movimientos_transportes.flete_final_tercero As 'Fte Tercero', "
            strSQL += "movimientos_transportes_consolidado.movimiento_cnx_empresa As 'Anexo Empresa', "
            strSQL += "movimientos_transportes_consolidado.movimiento_cnx_tercero As 'Anexo Tercero', "
            strSQL += "COALESCE((SELECT movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)),0) as 'Total Anexo', "
            strSQL += "round((movimientos_transportes.flete_final_empresa - (movimientos_transportes.flete_final_tercero + COALESCE((SELECT movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)), 0))),0) As 'Intermediacion', "
            strSQL += "round((((1 - (movimientos_transportes.flete_final_tercero + COALESCE((Select movimiento_transporte_funcion_gastosan(movimientos_transportes.idmovimientos_transportes)), 0)) / movimientos_transportes.flete_final_empresa))*100),2) As '% Intermediacion', "
            strSQL += " movimientos_transportes_consolidado.generador_nombre as 'Generador', "
            strSQL += "movimientos_transportes_consolidado.generador_documento As 'Nit Generador', "
            strSQL += "movimientos_transportes_consolidado.asesor_nombre as 'Asesor Comercial', "
            strSQL += "movimientos_transportes_consolidado.despachador_nombre As 'Despachador', "
            strSQL += "movimientos_transportes_consolidado.vehiculo_placa as 'Placa', "
            strSQL += "movimientos_transportes_consolidado.conductor_nombre As 'Conductor', "
            strSQL += "movimientos_transportes_consolidado.conductor_documento as 'Cedula Cond', "
            strSQL += "movimientos_transportes_consolidado.movimiento_modalidad as 'Modalidad Operacion', "
            strSQL += "movimientos_transportes_consolidado.sucorigina_nombre as 'Sucursal', "
            strSQL += "movimientos_transportes_consolidado.movimiento_operacion as 'Tipo Operacion', "
            strSQL += "movimientos_transportes_consolidado.movimiento_origen as 'Origen', "
            strSQL += "movimientos_transportes_consolidado.movimiento_destino As 'Destino', "
            strSQL += "movimientos_transportes_detalles.peso_final as 'Peso KG', "
            strSQL += "sum(movimientos_transportes_detalles.unidades_final) As 'Unidades', "
            strSQL += "movimientos_transportes_detalles.valor_declarado as 'Vlr Declarado', "
            strSQL += "movimientos_transportes.total_anticipos As 'Total Anticipo', "
            strSQL += "completar_ceros(ventas.numero, 8) As 'Factura',  "
            strSQL += "ventas.fecha as 'Fecha Factura', "
            strSQL += "DateDiff(ventas.fecha, movimientos_transportes.fecha_elaboracion) As 'Dias Factura-Mvto', "
            strSQL += "tipo_estados.descripcion as 'Estado Mvto Transporte', "
            strSQL += "movimientos_contables.numero As 'Recibo', "
            strSQL += "ventas_recaudos.fecha_elaboracion as 'Fecha Elaborado', "
            strSQL += "ventas_recaudos.fecha_recaudo As 'Fecha Recaudo', "
            strSQL += "DateDiff(ventas_recaudos.fecha_recaudo, ventas.fecha) As 'Dias RC-Factura', "
            strSQL += "DateDiff(ventas_recaudos.fecha_recaudo, movimientos_transportes.fecha_elaboracion) As 'Dias RC-MvtoTTe', "
            strSQL += "ventas_recaudos_detalle.valor as 'Valor Pagado', "
            strSQL += "ventas_recaudos.total As 'Valor Total Recibo', "
            strSQL += "ventas_recaudos_detalle.descripcion as 'Detalle RC', "
            strSQL += "ventas_recaudos.observacion as 'Observacion RC' "
            strSQL += "From movimientos_transportes "
            strSQL += "inner Join movimientos_transportes_consolidado on movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            strSQL += "inner Join movimientos_transportes_actores on movimientos_transportes_actores.idmovimientos_transportes_actores = movimientos_transportes.movimientos_transportes_actores "
            strSQL += "inner Join tipo_estados on tipo_estados.idtipo_estados = movimientos_transportes.tipo_estados_idtipo_estados "
            strSQL += "inner Join movimientos_transportes_detalles on movimientos_transportes_detalles.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes "
            strSQL += "Left Join movimientos_gastos_anexos on movimientos_gastos_anexos.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes "
            strSQL += "Left Join ventas_control on (ventas_control.movimientos_transportes_id = movimientos_transportes.idmovimientos_transportes And ventas_control.idel = 0) "
            strSQL += "Left Join ventas_detalles on (ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id And ventas_detalles.idel = 0) "
            strSQL += "Left Join ventas on ventas.idventas = ventas_detalles.ventas_idventas "
            strSQL += "Left Join ventas_recaudos_detalle on ventas_recaudos_detalle.ventas_idventas = ventas.idventas "
            strSQL += "Left Join ventas_recaudos on ventas_recaudos.idventas_recaudos = ventas_recaudos_detalle.ventas_recaudos_id "
            strSQL += "Left Join movimientos_contables on movimientos_contables.idcontabilidad = ventas_recaudos.movimientos_contables_id "
            strSQL += "where ventas_recaudos.fecha_recaudo BETWEEN '"
            strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "' "
            strSQL += "And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
            strSQL += "And ventas_recaudos_detalle.descripcion Like '%CANCELA%' AND ventas_recaudos_detalle.descripcion NOT LIKE '%ANULA%' "
            strSQL += "Group BY movimientos_transportes.idmovimientos_transportes "

            dtter = csinformes.ejecutar_query_bd(strSQL)

            gridAsesores.DataSource = dtter
            gridAsesores.DataBind()
        End If
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        'Response.ContentType = "application/x-msexcel"
        'Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xls")
        'Response.ContentEncoding = Encoding.UTF8
        'Dim tw As StringWriter = New StringWriter()
        'Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
        'gridAsesores.RenderControl(hw)
        'Response.Write(tw.ToString())
        'Response.[End]()

        Dim name = "Asesores"

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Dim page As New Page()
        Dim form As New HtmlForm()

        gridAsesores.EnableViewState = False

        ' Deshabilitar la validación de eventos, sólo asp.net 2 
        page.EnableEventValidation = False

        ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        page.DesignerInitialize()

        page.Controls.Add(form)
        form.Controls.Add(gridAsesores)

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
