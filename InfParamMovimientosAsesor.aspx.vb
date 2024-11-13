Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Partial Class InfParamMovimientosAsesor
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

        strRespuestaPer = csusua.validar_permiso_usuario(6, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtase As New DataTable

        csoper.LlenarDropDownList("nombrecompleto", "idterceros", "terceros_mostrar_terceros_combo", dtase, cboasesores)
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Dim strSQL As String
        Dim dtter As New DataTable

        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            strSQL = "Select movimientos_transportes_consolidado.movimiento_numero As Numero,"
            strSQL += " movimientos_transportes_consolidado.generador_nombre AS Generador,"
            strSQL += " DATE_FORMAT(movimientos_transportes_consolidado.movimiento_fecha,'%Y-%m-%d') as Fecha,"
            strSQL += " movimientos_transportes_consolidado.movimiento_operacion As Despacho,"
            strSQL += " movimientos_transportes_consolidado.sucorigina_nombre As Sucursal,"
            strSQL += "(movimientos_transportes_consolidado.movimiento_fl_empresa + "
            strSQL += " COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) As VlrEmpresa, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero + "
            strSQL += " COALESCE(movimientos_transportes_consolidado.movimiento_cnx_tercero) As VlrTercero,"
            strSQL += " COALESCE(movimientos_transportes_consolidado.movimiento_anexos, 0) AS GastoAnexo,"
            strSQL += " COALESCE(movimientos_transportes_consolidado.movimiento_peso, 0) As Peso,"
            strSQL += " movimientos_transportes_consolidado.despachador_nombre as Despachador,"
            strSQL += " movimientos_transportes_consolidado.asesor_nombre as Asesor"
            strSQL += " From movimientos_transportes_consolidado"
            strSQL += " Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            If cboasesores.SelectedValue > 0 Then
                strSQL += " And movimientos_transportes_consolidado.asesor_nombre LIKE '%" & cboasesores.Items(cboasesores.SelectedIndex).Text & "%' "
            End If
            strSQL += " ORDER BY movimientos_transportes_consolidado.movimiento_numero"

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
