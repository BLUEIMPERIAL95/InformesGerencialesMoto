Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text

Partial Class InfParamCumplimientoPresupuestoMes
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamCumplimientoPresupuestoMes_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3070, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then

        End If
    End Sub

    Private Sub btnAgencia_Click(sender As Object, e As EventArgs) Handles btnAgencia.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim strSQL As String
                Dim dtage As New DataTable

                strSQL = "Select Case WHEN movimientos_transportes_consolidado.sucorigina_id = 10 Or "
                strSQL += "movimientos_transportes_consolidado.sucorigina_id = 13 Or "
                strSQL += "movimientos_transportes_consolidado.sucorigina_id = 15 Or "
                strSQL += "movimientos_transportes_consolidado.sucorigina_id = 20 Then 'OTRAS' " 
                strSQL += "Else  movimientos_transportes_consolidado.sucorigina_nombre End As Agencia, " 
                strSQL += "COALESCE(sistema_sucursales.presupuesto, 0) As Presupuesto, " 
                strSQL += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As 'Hizo', " 
                strSQL += "ROUND(((COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0)) / COALESCE(sistema_sucursales.presupuesto, 0) * 100), 0) As '% Cump' "
                strSQL += "From movimientos_transportes_consolidado "
                strSQL += "Left Join sistema_sucursales ON movimientos_transportes_consolidado.sucorigina_id = sistema_sucursales.idsucursales "
                strSQL += "Where factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQL += "And COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                strSQL += "Group BY Agencia "
                strSQL += "ORDER BY Hizo DESC "

                dtage = csinformes.ejecutar_query_bd(strSQL)

                gridAsesores.DataSource = dtage
                gridAsesores.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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

        Dim name = "Cumplimiento"

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

    Private Sub btnAsesor_Click(sender As Object, e As EventArgs) Handles btnAsesor.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim strSQL As String
                Dim dtage As New DataTable

                strSQL = "Select movimientos_transportes_consolidado.despachador_documento, "
                strSQL += "movimientos_transportes_consolidado.asesor_documento, "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.asesor_documento = '73118529' OR "  
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '85463423' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '22187687' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '32116503' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '31011305' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '8356841' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '42779239' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '1075259099' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '43612748' OR " 
                strSQL += "movimientos_transportes_consolidado.asesor_documento = '8163999' THEN 'OTROS' "
                strSQL += "ELSE movimientos_transportes_consolidado.asesor_nombre END AS Asesor, "
                strSQL += "COALESCE(usuarios.presupuesto_asesor, 0) As Presupuesto, "
                strSQL += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As 'Hizo', "
                strSQL += "ROUND(((COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0)) / COALESCE(usuarios.presupuesto_asesor, 0) * 100), 0) As '% Cump' "
                strSQL += "From movimientos_transportes_consolidado "
                strSQL += "Left Join generadores_asesores ON movimientos_transportes_consolidado.asesor_id = generadores_asesores.idgeneradores_asesores "
                strSQL += "Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += "Left Join terceros ON terceros.idterceros = usuarios.idterceros "
                strSQL += "Where factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQL += "And COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                strSQL += "Group BY Asesor "
                strSQL += "ORDER BY Hizo DESC "

                dtage = csinformes.ejecutar_query_bd(strSQL)

                gridAsesores.DataSource = dtage
                gridAsesores.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnDespachador_Click(sender As Object, e As EventArgs) Handles btnDespachador.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                Dim strSQL As String
                Dim dtage As New DataTable

                strSQL = "Select CASE WHEN movimientos_transportes_consolidado.despachador_documento = '72161511' OR "  
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '72282315' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '87698001' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '84456597' OR "
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '8769800' THEN 'BARRANQUILLA' "
                strSQL += "ELSE "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '43514362' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '52055247' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '93235822' THEN 'CALI' "
                strSQL += "ELSE "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '85463423' THEN 'SANTA MARTA' "
                strSQL += "ELSE "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '79856531' THEN 'BUENAVENTURA' " 
                strSQL += "ELSE "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '73118529' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '22810491' THEN 'CARTAGENA' " 
                strSQL += "ELSE " 
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '55152811' OR "  
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '20865535' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '1075259099' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '41658574' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '1001133202' OR " 
                strSQL += "movimientos_transportes_consolidado.despachador_documento = '37931977' THEN 'VARIOS' "
                strSQL += "ELSE "
                strSQL += "CASE WHEN movimientos_transportes_consolidado.despachador_documento = '98561058' THEN 'GUILLERMO OCAMPO OROZCO' "
                strSQL += "ELSE "
                strSQL += "movimientos_transportes_consolidado.despachador_nombre END END END END END END END AS Despachador, "
                strSQL += "COALESCE(usuarios.presupuesto_despachador, 0) As Presupuesto, "
                strSQL += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0) As 'Hizo', "
                strSQL += "ROUND(((COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_fl_tercero, 0) + "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0) - COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero, 0) - "
                strSQL += "COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0)), 0)) / COALESCE(usuarios.presupuesto_despachador, 0) * 100), 0) As '% Cump' "
                strSQL += "From movimientos_transportes_consolidado "
                strSQL += "Left Join generadores_despachadores ON movimientos_transportes_consolidado.asesor_id = generadores_despachadores.idgeneradores_asesores "
                strSQL += "Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += "Left Join terceros ON terceros.idterceros = usuarios.idterceros "
                strSQL += "Where factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQL += "And COALESCE(movimientos_transportes_consolidado.factura_id, 0) <> 0 "
                strSQL += "Group BY Despachador "
                strSQL += "ORDER BY Hizo DESC "

                dtage = csinformes.ejecutar_query_bd(strSQL)

                gridAsesores.DataSource = dtage
                gridAsesores.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
