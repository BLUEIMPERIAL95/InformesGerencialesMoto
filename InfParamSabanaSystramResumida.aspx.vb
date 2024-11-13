Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.util
Imports System.Globalization
Imports System.Math
Partial Class InfParamSabanaSystramResumida
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim intContMov As Integer

    Private Sub InfParamSabanaSystramResumida_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2054, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
        Else
            Dim strSQL As String
            Dim dtsab As New DataTable

            strSQL = "Select DISTINCT "
            strSQL += "mtc.sucorigina_nombre As 'Sucursal Origina', "
            strSQL += "mtc.movimiento_estado as 'Estado Movimiento', "
            strSQL += "mtc.movimiento_numero as 'Movimiento Nro.', "
            strSQL += "DATE_FORMAT(mtc.movimiento_fecha,'%d/%m/%Y') as 'Fecha Movimiento', "
            strSQL += "If((SELECT sistema_empresa.control_seti FROM sistema_empresa) = 0, mtc.factura_numero, mtc.numero_factura_preliquidacion) As 'Factura Nro.', "
            strSQL += "DATE_FORMAT(ventas.fecha,'%Y/%m/%d') as 'Fecha Factura', "
            strSQL += "mtc.asesor_nombre as 'Asesor', "
            strSQL += "mtc.despachador_nombre as 'Despachador', "
            strSQL += "mtc.generador_nombre as 'Cliente', "
            strSQL += "mtc.movimiento_fl_empresa as 'Flete Empresa', "
            strSQL += "mtc.movimiento_fl_tercero as 'Flete Tercero', "
            strSQL += "(SELECT COALESCE(SUM(movimientos_anexos.valor_empresa), 0) FROM movimientos_anexos WHERE movimientos_anexos.movimientos_transportes_idmovimientos = mt.idmovimientos_transportes) as 'Total Conexos Empresa', "
            strSQL += "(SELECT COALESCE(SUM(movimientos_anexos.valor_tercero), 0) FROM movimientos_anexos WHERE movimientos_anexos.movimientos_transportes_idmovimientos = mt.idmovimientos_transportes) As 'Total Conexos Terceros', "
            strSQL += "COALESCE(mtc.movimiento_anexos, 0) As 'Total Gastos Anexos', "
            strSQL += "COALESCE(mtc.movimiento_anticipos, 0) As 'Total Anticipos', "
            strSQL += "mtc.movimiento_peso as 'Total Peso', "
            strSQL += "mtc.movimiento_operacion as 'Operacion', "
            strSQL += "mtc.vehiculo_placa as 'Vehiculo', "
            strSQL += "mtc.movimiento_origen as 'Origen', "
            strSQL += "mtc.movimiento_destino as 'Destino', "
            strSQL += "COALESCE(mtc.causacion_numero, 'N/A') as 'Causacion', "
            strSQL += "COALESCE(DATE_FORMAT(mtc.causacion_fecha,'%d/%m/%Y'), 'N/A') as 'F.Causacion', "
            strSQL += "COALESCE(mtc.egreso_numero, 'N/A') as 'Egreso', "
            strSQL += "COALESCE(DATE_FORMAT(mtc.egreso_fecha,'%d/%m/%Y'), 'N/A') as 'F.Egreso', "
            strSQL += "COALESCE(movimientos_transportes_manifiestos.distancia, 0) AS 'distancia 1', "
            strSQL += "COALESCE(movimientos_transportes_urbanos.distancia, 0) AS 'distancia 2' "
            strSQL += "From "
            strSQL += "movimientos_transportes_consolidado As mtc "
            strSQL += "Left Join vehiculos_carga ON vehiculos_carga.vehiculos_idvehiculos = mtc.vehiculo_id "
            strSQL += "INNER Join movimientos_transportes AS mt ON (mtc.movimiento_id = mt.idmovimientos_transportes) "
            strSQL += "Left Join movimientos_transportes_remesas ON (movimientos_transportes_remesas.movimientos_transportes_idmovimientos = mt.idmovimientos_transportes And movimientos_transportes_remesas.tipo_estados_idtipo = 21) "
            strSQL += "INNER Join usuarios AS us ON (mt.usuarios_idusuarios_ingresa = us.idusuarios) "
            strSQL += "INNER Join terceros AS ter ON (us.idterceros = ter.idterceros) "
            strSQL += "Left Join terceros_propietarios AS tp ON (tp.idterceros_propietarios = mtc.propietario_id) "
            strSQL += "Left Join terceros_conductores AS tercon ON (mtc.conductor_id = tercon.idterceros_conductores) "
            strSQL += "Left Join terceros AS ter1 ON (tercon.terceros_idterceros = ter1.idterceros) "
            strSQL += "Left Join vehiculos AS veh ON (mtc.vehiculo_id = veh.idvehiculos) "
            strSQL += "Left Join clases_vehiculares AS cv ON (veh.clases_vehiculares_idclases_vehiculares = cv.idclases_vehiculares) "
            strSQL += "INNER Join tipo_despachos AS td ON (mt.tipo_despachos_idtipo_despachos = td.idtipo_despachos) "
            strSQL += "Left Join movimientos_transportes_urbanos ON (movimientos_transportes_urbanos.movimiento_id = mtc.movimiento_id And movimientos_transportes_urbanos.estado_id = 21) "
            strSQL += "Left Join usuarios ON usuarios.idusuarios = mt.usuario_cumple "
            strSQL += "Left Join terceros ON terceros.idterceros = usuarios.idterceros "
            strSQL += "Left Join trailers ON trailers.idtrailers = mtc.trailer_id "
            strSQL += "Left Join ventas ON ventas.idventas = mtc.factura_id "
            strSQL += "Left Join movimientos_transportes_facturar ON movimientos_transportes_facturar.movimiento_id = mtc.movimiento_id "
            strSQL += "Left Join generadores ON generadores.idgeneradores = movimientos_transportes_facturar.generador_id "
            strSQL += "Left Join terceros tr ON tr.idterceros = generadores.terceros_idterceros "
            strSQL += "Left Join campanas ON mt.campanas_idcampanas = campanas.idcampanas "
            strSQL += "LEFT JOIN movimientos_transportes_manifiestos ON movimientos_transportes_manifiestos.id_refmovimiento = mt.idmovimientos_transportes "

            If cboFiltro.SelectedValue = "0" Then
                strSQL += "WHERE ventas.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            Else
                strSQL += "WHERE mtc.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            End If
            strSQL += "ORDER BY mtc.movimiento_numero "

            dtsab = csinformes.ejecutar_query_bd(strSQL)

            gridSabana.DataSource = dtsab
            gridSabana.DataBind()
        End If
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Dim name = "Sabana"

        Dim sb As New StringBuilder()
        Dim sw As New StringWriter(sb)
        Dim htw As New HtmlTextWriter(sw)

        Dim page As New Page()
        Dim form As New HtmlForm()

        gridSabana.EnableViewState = False

        ' Deshabilitar la validación de eventos, sólo asp.net 2 
        page.EnableEventValidation = False

        ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        page.DesignerInitialize()

        page.Controls.Add(form)
        form.Controls.Add(gridSabana)

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
