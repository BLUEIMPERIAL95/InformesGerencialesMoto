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
Imports DPFP
Imports DPFP.Capture
Imports System.Drawing
Imports System.Data.SqlClient
Partial Class vencimiento_documentos
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csterc As New equipos
    Dim csoper As New Operaciones

    Private Sub vencimiento_documentos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2051, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            Llenar_Grid()
        End If
    End Sub

    Private Sub Llenar_Grid()
        Dim dtvedo As New DataTable

        dtvedo = csterc.seleccionar_vencimiento_documentos_completo()

        If dtvedo.Rows.Count > 0 Then
            griVencimientos.DataSource = dtvedo
            griVencimientos.DataBind()
        Else
            griVencimientos.DataSource = Nothing
            griVencimientos.DataBind()
        End If
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtDocumento.Text = "" Or txtFechaExpide.Value = "" Or txtFechaVence.Value = "" Or txtCorreos.Text = "" Or txtCelulares.Text = "" Or txtDias.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Información Inválida. Favor revisar...');", True)
            Else
                Dim veccorreos As String() = txtCorreos.Text.Split(";")

                If veccorreos.Length > 0 Then
                    For i As Integer = 0 To veccorreos.Length - 1
                        If csoper.IsValidEmail(veccorreos(i)) = False Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Correo Inválido. " & veccorreos(i) & ". Favor revisar...');", True)
                            Exit Sub
                        End If
                    Next
                End If

                Dim strRes As String

                strRes = csterc.guardar_vencimiento_documentos(hidvencimiento.Value, txtDocumento.Text, txtFechaExpide.Value, txtFechaVence.Value,
                                                               txtDias.Text, txtCorreos.Text, txtCelulares.Text, txtObservacion.Text, Session("id_usua"),
                                                               cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento almacenado con éxito...');", True)
                    Llenar_Grid()
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento no almacenado con éxito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Limpiar()
        hidvencimiento.Value = "0"
        txtDocumento.Text = ""
        txtFechaExpide.Value = ""
        txtFechaVence.Value = ""
        txtDias.Text = ""
        txtCorreos.Text = ""
        txtCelulares.Text = ""
        txtObservacion.Text = ""
        cboActivo.SelectedValue = "1"
    End Sub

    Private Sub griVencimientos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles griVencimientos.RowCommand
        Try
            If e.CommandName = "modificar" Then
                Dim idven As Integer
                Dim dtven As New DataTable

                idven = griVencimientos.DataKeys(e.CommandArgument).Values(0)
                hidvencimiento.Value = idven

                dtven = csterc.seleccionar_vencimiento_documentos_por_id(hidvencimiento.Value)

                If dtven.Rows.Count > 0 Then
                    hidvencimiento.Value = dtven.Rows(0)("id_vedo").ToString
                    txtDocumento.Text = dtven.Rows(0)("nombre_vedo").ToString
                    txtFechaExpide.Value = dtven.Rows(0)("fexpide_vedo").ToString
                    txtFechaVence.Value = dtven.Rows(0)("fvence_vedo").ToString
                    txtDias.Text = dtven.Rows(0)("diasaviso_vedo").ToString
                    txtCorreos.Text = dtven.Rows(0)("correos_vedo").ToString
                    txtCelulares.Text = dtven.Rows(0)("celulares_vedo").ToString
                    txtObservacion.Text = dtven.Rows(0)("observacion_vedo").ToString
                    cboActivo.SelectedValue = dtven.Rows(0)("activo_vedo").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Vencimiento no válido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    Dim idven As Integer
                    Dim strRespuesta As String
                    strRespuesta = ""

                    idven = griVencimientos.DataKeys(e.CommandArgument).Values(0)
                    hidvencimiento.Value = idven

                    strRespuesta = csterc.eliminar_vencimiento_documentos_por_id(hidvencimiento.Value)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento eliminado con éxito...');", True)
                        Llenar_Grid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento no eliminado con éxito...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
