Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Partial Class InfParamImpuestosPropietarios
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

        strRespuestaPer = csusua.validar_permiso_usuario(4076, Session("id_usua"))

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
            strSQL = "Select CASE WHEN COALESCE(mtcon.manifiesto_numero, 0) = 0 THEN mtcon.urbano_fecha ELSE mtcon.manifiesto_fecha END AS Fecha, "
            strSQL += "mtcon.movimiento_numero, "
            strSQL += "CASE WHEN COALESCE(mtcon.manifiesto_numero, 0) = 0 THEN mtcon.urbano_numero ELSE mtcon.manifiesto_numero END AS 'MAN-MTU', "
            strSQL += "ter.documento as 'propietario_documento', "
            strSQL += "CONCAT(ter.nombre1, ' ', ter.nombre2, ' ', ter.apellido1, ' ', ter.apellido2) As 'propietario_nombre', "
            strSQL += "cuentas_contables.codigo AS CodCuenta, "
            strSQL += "cuentas_contables.descripcion As DesCuenta, "
            strSQL += "ROUND(sum(COALESCE(mt.flete_final_tercero, 0))+sum(COALESCE((Select SUM(valor_tercero) FROM movimientos_anexos WHERE movimientos_anexos.fecha BETWEEN '" & txtFechaInicio.Value & "' and '" & txtFechaFin.Value & "' AND COALESCE(movimientos_anexos.idcancelado, 0) > 0 AND movimientos_anexos.idel = 0 AND movimientos_anexos.movimientos_transportes_idmovimientos = mt.idmovimientos_transportes),0)), 2) as 'flete', "
            strSQL += "ROUND((sum(COALESCE(mt.flete_final_tercero,0))+sum(COALESCE((SELECT SUM(valor_tercero) FROM movimientos_anexos WHERE movimientos_anexos.fecha BETWEEN '" & txtFechaInicio.Value & "' and '" & txtFechaFin.Value & "' AND COALESCE(movimientos_anexos.idcancelado, 0) > 0 AND movimientos_anexos.idel = 0 AND movimientos_anexos.movimientos_transportes_idmovimientos = mt.idmovimientos_transportes),0))) * (tipo_descuentos_transportes.valor_porcentaje / 100), 2) as 'ReteIca', "
            strSQL += "ss.descripcion as 'sucorigina_nombre', "
            strSQL += "tp.descripcion as 'movimiento_operacion' "
            strSQL += "From movimientos_transportes mt "
            strSQL += " inner Join movimientos_transportes_causaciones_consolidado mtc on mtc.movimiento_id = mt.idmovimientos_transportes "
            strSQL += " inner Join generadores g on g.idgeneradores = mt.generadores_idgeneradores "
            strSQL += " inner Join terceros tgen on tgen.idterceros = g.terceros_idterceros "
            strSQL += " inner Join movimientos_transportes_consolidado mtcon on mtcon.movimiento_id = mt.idmovimientos_transportes "
            strSQL += " inner Join terceros_propietarios on terceros_propietarios.idterceros_propietarios = mtcon.propietario_id "
            strSQL += " inner Join terceros terpro on terpro.idterceros = terceros_propietarios.terceros_idterceros "
            strSQL += " Left Join movimientos_transportes_descuentos ON movimientos_transportes_descuentos.movimientos_transportes_idmovimientos_transportes = mt.idmovimientos_transportes "
            strSQL += " Left Join tipo_descuentos_transportes ON tipo_descuentos_transportes.idtipo_descuentos_transportes = movimientos_transportes_descuentos.tipo_descuentos_transportes_idtipo_descuentos_transportes "
            strSQL += " inner Join terceros ter on ter.documento = terpro.documento "
            strSQL += " Left Join tipo_despachos tp on tp.idtipo_despachos = mt.tipo_despachos_idtipo_despachos "
            strSQL += " Left Join sistema_sucursales ss on ss.idsucursales = mt.sistema_sucursales_origina "
            strSQL += " inner Join movimientos_contables mc ON mc.idcontabilidad = mtc.contable_id "
            strSQL += " Left Join cuentas_contables ON tipo_descuentos_transportes.cuentas_contables_id = cuentas_contables.idplan_cuentas "
            strSQL += " where mt.tipo_estados_idtipo_estados = 30 And mtcon.manifiesto_fecha between '" & txtFechaInicio.Value & "' and '" & txtFechaFin.Value & "' and mc.idel = 0 "
            strSQL += " And mtc.causacion_estado = 'GENERADO' "
            strSQL += " And tipo_descuentos_transportes.tipo_descuento = " & cboTipos.SelectedValue & " And movimientos_transportes_descuentos.idel = 0 "
            strSQL += " And mt.tipo_operacion = 0 "
            strSQL += " Group BY mtcon.movimiento_numero "
            strSQL += " order by mt.fecha_movimiento "

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

        Dim name = "ImpuestosPropietarios"

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
