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
Partial Class CadRecibidoListado
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscad As New cad

    Private Sub CadRecibidoListado_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2056, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            Llenar_grid(0, Session("id_usua"))
        End If
    End Sub

    Private Sub Llenar_grid(ByVal ind As Integer, ByVal par As String)
        Dim dtcad As New DataTable

        dtcad = cscad.seleccionar_cad_recibido_listado(ind, par)

        If dtcad.Rows.Count > 0 Then
            gridCadRecibido.DataSource = dtcad
            gridCadRecibido.DataBind()
        Else
            gridCadRecibido.DataSource = Nothing
            gridCadRecibido.DataBind()
        End If
    End Sub

    Private Sub imgNuevo_Click(sender As Object, e As ImageClickEventArgs) Handles imgNuevo.Click
        Response.Redirect("CadRecibidoDetalle.aspx?id=0")
    End Sub

    Private Sub imgBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles imgBuscar.Click
        If txtBuscar.Text <> "" Then
            If rdNumero.Checked = True Then
                Llenar_grid(1, txtBuscar.Text)
            End If

            If rdUsuario.Checked = True Then
                Llenar_grid(2, txtBuscar.Text)
            End If
        Else
            Llenar_grid(0, Session("id_usua"))
        End If
    End Sub

    Private Sub rdNumero_CheckedChanged(sender As Object, e As EventArgs) Handles rdNumero.CheckedChanged
        If rdNumero.Checked = True Then
            rdUsuario.Checked = False
        Else
            rdUsuario.Checked = True
        End If
    End Sub

    Private Sub rdUsuario_CheckedChanged(sender As Object, e As EventArgs) Handles rdUsuario.CheckedChanged
        If rdUsuario.Checked = True Then
            rdNumero.Checked = False
        Else
            rdNumero.Checked = True
        End If
    End Sub

    Private Sub gridCadEnvio_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCadRecibido.RowCommand
        Try
            If e.CommandName = "modificar" Then
                Dim idCad As Integer

                idCad = gridCadRecibido.DataKeys(e.CommandArgument).Values(0)

                If idCad > 0 Then
                    Response.Redirect("CadRecibidoDetalle.aspx?id=" & idCad & "")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Recibido no valido');", True)
                End If
            End If

            If e.CommandName = "eliminar" Then
                Dim idCad As Integer
                Dim strEstado As String

                idCad = gridCadRecibido.DataKeys(e.CommandArgument).Values(0)
                strEstado = gridCadRecibido.DataKeys(e.CommandArgument).Values(1)

                If idCad And strEstado <> "EMITIDO" > 0 Then
                    Dim strRespuesta As String

                    strRespuesta = cscad.eliminar_cad_recibido_por_id(idCad)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad recibido eliminado exitosamente...');", True)
                        Response.Redirect("CadRecibidoListado.aspx")
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad recibido no fue eliminado exitosamente...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Recibido Emitido. Imposible eliminar.');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
