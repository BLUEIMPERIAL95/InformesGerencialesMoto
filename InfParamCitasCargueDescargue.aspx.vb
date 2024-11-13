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
Partial Class InfParamCitasCargueDescargue
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim cscitas As New citas

    Private Sub InfParamCitasCargueDescargue_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(2059, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim dtcit As New DataTable

                dtcit = cscitas.seleccionar_citas_cargue_descargue_por_id_informe(Replace(txtFechaInicio.Value, "-", "/") & " 00:00:00", Replace(txtFechaFin.Value, "-", "/") & " 00:00:00")

                If dtcit.Rows.Count > 0 Then
                    gridCitas.DataSource = dtcit
                    gridCitas.DataBind()
                Else
                    gridCitas.DataSource = Nothing
                    gridCitas.DataBind()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Dim name = "CitasCargueDescargue"

            Dim sb As New StringBuilder()
            Dim sw As New StringWriter(sb)
            Dim htw As New HtmlTextWriter(sw)

            Dim page As New Page()
            Dim form As New HtmlForm()

            gridCitas.EnableViewState = False

            ' Deshabilitar la validación de eventos, sólo asp.net 2 
            page.EnableEventValidation = False

            ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
            page.DesignerInitialize()

            page.Controls.Add(form)
            form.Controls.Add(gridCitas)

            page.RenderControl(htw)

            Response.Clear()
            Response.Buffer = True

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".xls")
            Response.Charset = "UTF-8"

            Response.ContentEncoding = Encoding.[Default]
            Response.Write(sb.ToString())
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
