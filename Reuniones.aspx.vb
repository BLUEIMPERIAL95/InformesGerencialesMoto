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

Partial Class Reuniones
    Inherits System.Web.UI.Page
    Dim csreu As New reunion
    Dim csusua As New usuarios

    Private Sub Reuniones_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(33, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            Llenar_Grid()
        End If
    End Sub

    Private Sub rdCodigo_CheckedChanged(sender As Object, e As EventArgs) Handles rdCodigo.CheckedChanged
        If rdCodigo.Checked = True Then
            rdNombre.Checked = False
            rdFecha.Checked = False
            txtBuscar.Enabled = True
            CalendarFechaInicio.Enabled = False
            txtBuscar.Text = ""
            txtFecha.Value = ""
        End If
    End Sub

    Private Sub rdNombre_CheckedChanged(sender As Object, e As EventArgs) Handles rdNombre.CheckedChanged
        If rdNombre.Checked = True Then
            rdCodigo.Checked = False
            rdFecha.Checked = False
            txtBuscar.Enabled = True
            CalendarFechaInicio.Enabled = False
            txtBuscar.Text = ""
            txtFecha.Value = ""
        End If
    End Sub

    Private Sub rdFecha_CheckedChanged(sender As Object, e As EventArgs) Handles rdFecha.CheckedChanged
        If rdFecha.Checked = True Then
            rdCodigo.Checked = False
            rdNombre.Checked = False
            txtBuscar.Enabled = False
            CalendarFechaInicio.Enabled = True
            txtBuscar.Text = ""
            txtFecha.Value = ""
        End If
    End Sub

    Sub Llenar_Grid()
        Dim dtreu As New DataTable

        dtreu = csreu.seleccionar_reuniones_completo()

        gridReuniones.DataSource = dtreu
        gridReuniones.DataBind()
    End Sub

    Private Sub gridReuniones_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridReuniones.RowCommand
        Try
            Dim idreunion As Integer
            idreunion = gridReuniones.DataKeys(e.CommandArgument).Values(0)
            Dim strRes As String

            If e.CommandName = "imprimir" Then
                Response.Redirect("InfParamReuniones.aspx?idreu=" & idreunion)
            Else
                If e.CommandName = "modificar" Then
                    Response.Redirect("ReunionesDetalles.aspx?idreu=" & idreunion)
                Else
                    If e.CommandName = "eliminar" Then
                        hidReunion.Value = idreunion

                        strRes = csreu.eliminar_reunion_por_id_reunion(hidReunion.Value)

                        If strRes = "" Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion eliminada');", True)
                            Llenar_Grid()
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion no eliminada');", True)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
