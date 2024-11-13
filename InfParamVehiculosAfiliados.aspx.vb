Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text

Partial Class InfParamVehiculosAfiliados
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamVehiculosAfiliados_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3072, Session("id_usua"))

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
            strSQL = "Select vehiculos.placa, "
            strSQL += "vehiculos_afiliados.fecha_afiliacion, "
            strSQL += "vehiculos_afiliados.fecha_final_afiliacion, "
            strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.nombre2, ' ', terceros.nombre2) AS Propietario, "
            strSQL += "telefonos.telefono "
            strSQL += "From vehiculos_afiliados "
            strSQL += "LEFT JOIN vehiculos ON vehiculos_afiliados.vehiculos_idvehiculos = vehiculos.idvehiculos "
            strSQL += "LEFT JOIN vehiculos_tenencias ON vehiculos.idvehiculos = vehiculos_tenencias.vehiculos_idvehiculos "
            strSQL += "LEFT JOIN terceros_propietarios ON vehiculos_tenencias.terceros_propietarios_idterceros_propietarios = terceros_propietarios.idterceros_propietarios "
            strSQL += "LEFT JOIN terceros ON terceros_propietarios.terceros_idterceros = terceros.idterceros "
            strSQL += "LEFT JOIN telefonos ON terceros.idterceros = telefonos.terceros_idterceros "
            strSQL += "WHERE vehiculos_afiliados.fecha_final_afiliacion BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += "And vehiculos_tenencias.tipo_tenencia = 1 And vehiculos_tenencias.idel = 0 "
            strSQL += "ORDER BY vehiculos_afiliados.fecha_final_afiliacion DESC "

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
