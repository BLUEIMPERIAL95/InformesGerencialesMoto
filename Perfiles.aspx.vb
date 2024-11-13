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
Partial Class Perfiles
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Private Sub Perfiles_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(1, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect",
            '"alert('" & strRespuestaPer & "'); window.location='" +
            'Request.ApplicationPath + "Default.aspx';", True)
            Response.Redirect("Default.aspx")
        End If

        'If Session("documento") <> "98702336" And Session("documento") <> "71760116" Then
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
        '    Response.Redirect("Default.aspx")
        'End If

        If Me.IsPostBack = False Then
            'LlenarGrid()
        End If
    End Sub

    'Sub LlenarGrid()
    '    Dim csusu As New usuarios
    '    Dim dtper As New DataTable

    '    dtper = csusu.seleccionar_datos_perfiles_completo

    '    gridPerfiles.DataSource = dtper
    '    gridPerfiles.DataBind()
    'End Sub

    'Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
    '    Try
    '        If txtPerfil.Text = "" Or txtDescripcion.Text = "" Then
    '            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
    '        Else
    '            Dim strRes As String

    '            strRes = csusua.guardar_perfiles(hidperfil.Value, txtPerfil.Text, txtDescripcion.Text, cboActivo.SelectedValue)

    '            If strRes = "" Then
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Perfil actualizado con exito');", True)
    '                Limpiar()
    '            Else
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Perfil no actualizado');", True)
    '            End If

    '            LlenarGrid()
    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
    '    End Try
    'End Sub

    'Sub Limpiar()
    '    hidperfil.Value = 0
    '    txtPerfil.Text = ""
    '    txtDescripcion.Text = ""
    '    cboActivo.SelectedValue = "1"
    'End Sub

    'Private Sub gridPerfiles_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridPerfiles.RowCommand
    '    Try
    '        Dim idperfil As Integer
    '        Dim dtper As New DataTable
    '        Dim strRes As String

    '        If e.CommandName = "modificar" Then
    '            idperfil = gridPerfiles.DataKeys(e.CommandArgument).Values(0)
    '            hidperfil.Value = idperfil

    '            dtper = csusua.capturar_datos_perfiles_por_id(hidperfil.Value)

    '            If dtper.Rows.Count > 0 Then
    '                txtPerfil.Text = dtper.Rows(0)("nom_perf").ToString
    '                txtDescripcion.Text = dtper.Rows(0)("des_perf").ToString
    '                cboActivo.SelectedValue = dtper.Rows(0)("act_perf").ToString
    '            Else
    '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Perfil no valido');", True)
    '            End If
    '        Else
    '            If e.CommandName = "eliminar" Then
    '                idperfil = gridPerfiles.DataKeys(e.CommandArgument).Values(0)
    '                hidperfil.Value = idperfil

    '                strRes = csusua.eliminar_perfiles(hidperfil.Value)

    '                If strRes = "" Then
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Perfil eliminado');", True)
    '                    LlenarGrid()
    '                    Limpiar()
    '                Else
    '                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Perfil No eliminado');", True)
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
    '    End Try
    'End Sub

    'Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
    '    Limpiar()
    'End Sub
End Class
