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

Partial Class MaestroUsuarios
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios

    Private Sub MaestroUsuarios_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Session("documento") <> "98702336" And Session("documento") <> "71760116" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
        End If
    End Sub

    Sub combos()
        Dim dtper As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_perfiles_combo", dtper, cboPerfil)
    End Sub

    Sub LlenarGrid()
        Dim csusu As New usuarios
        Dim dtusu As New DataTable

        dtusu = csusu.seleccionar_datos_usuarios_completo

        gridUsuarios.DataSource = dtusu
        gridUsuarios.DataBind()
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboPerfil.SelectedValue = "0" Or txtDocumento.Text = "" Or txtNombre.Text = "" Or txtUsuario.Text = "" Or txtContraseña.Text = "" Or csoper.IsValidEmail(txtCorreo.Text) = False Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csusua.guardar_usuarios(hidusuario.Value, cboPerfil.SelectedValue, txtDocumento.Text, txtNombre.Text, txtTelefono.Text, txtCelular.Text,
                                                 txtDireccion.Text, txtCorreo.Text, txtFechaNace.Value, txtUsuario.Text, txtContraseña.Text, cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub Limpiar()
        hidusuario.Value = 0
        txtDocumento.Text = ""
        txtNombre.Text = ""
        txtTelefono.Text = ""
        txtCelular.Text = ""
        txtDireccion.Text = ""
        txtCorreo.Text = ""
        txtUsuario.Text = ""
        txtContraseña.Text = ""
        txtFechaNace.Value = ""
        cboActivo.SelectedValue = "1"
    End Sub

    Private Sub txtDocumento_TextChanged(sender As Object, e As EventArgs) Handles txtDocumento.TextChanged
        txtUsuario.Text = txtDocumento.Text
    End Sub

    Private Sub gridUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridUsuarios.RowCommand
        Try
            Dim idusuario As Integer
            Dim dtusu As New DataTable
            Dim strRes As String
            If e.CommandName = "modificar" Then
                idusuario = gridUsuarios.DataKeys(e.CommandArgument).Values(0)
                hidusuario.Value = idusuario

                dtusu = csusua.capturar_datos_usuarios_por_id(hidusuario.Value)

                If dtusu.Rows.Count > 0 Then
                    cboPerfil.SelectedValue = dtusu.Rows(0)("id_perf").ToString
                    txtDocumento.Text = dtusu.Rows(0)("strdoc_usua").ToString
                    txtNombre.Text = dtusu.Rows(0)("strnom1_usua").ToString
                    txtTelefono.Text = dtusu.Rows(0)("strtel_usua").ToString
                    txtCelular.Text = dtusu.Rows(0)("strcel_usua").ToString
                    txtDireccion.Text = dtusu.Rows(0)("strdir_usua").ToString
                    txtCorreo.Text = dtusu.Rows(0)("strcor_usua").ToString
                    txtFechaNace.Value = dtusu.Rows(0)("datfecnac_usua").ToString
                    txtUsuario.Text = dtusu.Rows(0)("strusu_usua").ToString
                    txtContraseña.Text = dtusu.Rows(0)("strcon_usua").ToString
                    cboActivo.SelectedValue = dtusu.Rows(0)("act_usua")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario no valido');", True)
                End If
            Else
                idusuario = gridUsuarios.DataKeys(e.CommandArgument).Values(0)
                hidusuario.Value = idusuario

                strRes = csusua.eliminar_usuarios(hidusuario.Value)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario eliminado');", True)
                    LlenarGrid()
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Usuario No eliminado');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub
End Class
