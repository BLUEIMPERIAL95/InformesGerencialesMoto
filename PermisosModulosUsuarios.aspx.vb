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
Partial Class PermisosModulosUsuarios
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios

    Private Sub PermisosModulosUsuarios_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Private Sub combos()
        Dim dtmod, dtusu As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_modulos_combo", dtmod, cboModulo)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_usuarios_combo", dtusu, cboUsuario)
    End Sub

    Private Sub cboModulo_TextChanged(sender As Object, e As EventArgs) Handles cboModulo.TextChanged

    End Sub

    Private Sub cboUsuario_TextChanged(sender As Object, e As EventArgs) Handles cboUsuario.TextChanged
        cboModulo.SelectedValue = "0"
        hidusuario.Value = cboUsuario.SelectedValue

        Llenar_grid()
    End Sub

    Private Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
        If chkTodos.Checked = True Then
            cboModulo.SelectedValue = "0"
            cboModulo.Enabled = False
        Else
            cboModulo.SelectedValue = "0"
            cboModulo.Enabled = True
        End If
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboUsuario.SelectedValue <> "0" Then
                If cboModulo.SelectedValue <> "0" Or chkTodos.Checked = True Then
                    Dim strRes As String

                    strRes = csusua.guardar_permisos(cboUsuario.SelectedValue, cboModulo.SelectedValue, Session("id_usua"))

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Permiso actualizado con exito');", True)
                        Llenar_grid()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Permiso no actualizado');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Llenar_grid()
        Dim dtper As New DataTable

        dtper = csusua.seleccionar_permisos_por_usuario(cboUsuario.SelectedValue)

        If dtper.Rows.Count > 0 Then
            grdPermisos.DataSource = dtper
            grdPermisos.DataBind()
        Else
            grdPermisos.DataSource = dtper
            grdPermisos.DataBind()
        End If
    End Sub

    Private Sub grdPermisos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdPermisos.RowCommand
        Try
            Dim idpermiso As Integer
            Dim dtusu As New DataTable
            Dim strRes As String
            If e.CommandName = "eliminar" Then
                idpermiso = grdPermisos.DataKeys(e.CommandArgument).Values(0)

                strRes = csusua.eliminar_permisos(idpermiso)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Permiso eliminado');", True)
                    Llenar_grid()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Permiso No eliminado');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        cboUsuario.SelectedValue = "0"
        cboModulo.SelectedValue = "0"
        chkTodos.Checked = False
        grdPermisos.DataSource = Nothing
        grdPermisos.DataBind()
    End Sub
End Class
