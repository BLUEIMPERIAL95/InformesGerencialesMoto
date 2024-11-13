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

Partial Class Saldos_Cambiadero
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscamb As New cambiadero

    Private Sub Saldos_Cambiadero_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3073, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            LlenarGrid()
        End If
    End Sub

    Sub LlenarGrid()
        Dim dtcam As New DataTable

        dtcam = cscamb.seleccionar_saldos_cambiadero_listado_por_usuario(Session("id_usua"))

        gridSaldos.DataSource = dtcam
        gridSaldos.DataBind()
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtFecha.Value = "" Or cboTipo.SelectedValue = "0" Or cboAño.SelectedValue = "0" Or cboMes.SelectedValue = "0" Or txtSaldoInicial.Text = "" Or txtSaldoInicial.Text = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim dtsal As New DataTable

                dtsal = cscamb.guardar_saldos_cambiadero(0, txtFecha.Value, cboTipo.SelectedValue, cboAño.SelectedValue, cboMes.SelectedValue, txtSaldoInicial.Text, txtObservacion.Text, Session("id_usua"))

                If dtsal.Rows.Count > 0 Then
                    If dtsal.Rows(0)("respuesta").ToString = "OK" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Saldo almacenado...');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Saldo no almacenado...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Saldo no almacenado...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Limpiar()
        txtFecha.Value = ""
        cboTipo.SelectedValue = "0"
        cboAño.SelectedValue = "0"
        cboMes.SelectedValue = "0"
        txtSaldoInicial.Text = ""
        lblSaldoInicial.Text = "S.Inicial"
        txtSaldoFinal.Text = ""
        txtObservacion.Text = ""
        btnSalvar.Text = "Salvar"
    End Sub

    Private Sub gridSaldos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridSaldos.RowCommand
        Try
            Dim idcam As Integer
            Dim dtcam As New DataTable
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "modificar" Then
                idcam = gridSaldos.DataKeys(e.CommandArgument).Values(0)

                dtcam = cscamb.seleccionar_saldos_cambiadero_por_id(idcam)

                If dtcam.Rows.Count > 0 Then
                    cboTipo.SelectedValue = dtcam.Rows(0)("tipo_saca").ToString
                    txtFecha.Value = dtcam.Rows(0)("fecha_saca").ToString
                    cboAño.SelectedValue = dtcam.Rows(0)("año_saca").ToString
                    cboMes.SelectedValue = dtcam.Rows(0)("mes_saca").ToString
                    txtSaldoInicial.Text = dtcam.Rows(0)("saldoini_saca").ToString
                    txtObservacion.Text = dtcam.Rows(0)("observacion_saca").ToString
                    If cboTipo.SelectedValue = "1" Then
                        btnSalvar.Text = "Salvar Recarga"
                        lblSaldoInicial.Text = "Recarga"
                    Else
                        btnSalvar.Text = "Salvar Saldo"
                        lblSaldoInicial.Text = "S.Inicial"
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro inválido...');", True)
                End If
            End If

            If e.CommandName = "eliminar" Then
                idcam = gridSaldos.DataKeys(e.CommandArgument).Values(0)

                strRespuesta = cscamb.eliminar_saldos_cambiadero_por_id(idcam)

                If strRespuesta = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('saldo eliminado...');", True)
                    LlenarGrid()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('saldo no eliminado...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboTipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipo.TextChanged
        If cboTipo.SelectedValue = "1" Then
            btnSalvar.Text = "Salvar Recarga"
            lblSaldoInicial.Text = "Recarga"
        Else
            btnSalvar.Text = "Salvar Saldo"
            lblSaldoInicial.Text = "S.Inicial"
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub
End Class
