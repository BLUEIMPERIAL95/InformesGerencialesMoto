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
Partial Class TestSaludMovimiento
    Inherits System.Web.UI.Page
    Dim csreun As New reunion

    Private Sub TestSaludMovimiento_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then

        End If
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtMovimiento.Text = "" Or cboPrimera.SelectedValue = "0" Or cboSegunda.SelectedValue = "0" Or cboTercera.SelectedValue = "0" _
                Or cboCuarta.SelectedValue = "0" Or cboQuinta.SelectedValue = "0" Or cboSexta.SelectedValue = "0" Or cboSeptima.SelectedValue = "0" _
                Or cboOctava.SelectedValue = "0" Or cboNovena.SelectedValue = "0" Or cboDecima.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
            Else
                Dim strRes As String

                strRes = csreun.guardar_test_salud(txtMovimiento.Text, cboPrimera.SelectedValue, cboCual.SelectedValue, cboSegunda.SelectedValue,
                                                   cboTercera.SelectedValue, cboCuarta.SelectedValue, cboQuinta.SelectedValue, cboSexta.SelectedValue,
                                                   cboSeptima.SelectedValue, cboOctava.SelectedValue, cboNovena.SelectedValue, cboDecima.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Test actualizado con exito...');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Test no actualizado con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub cboPrimera_TextChanged(sender As Object, e As EventArgs) Handles cboPrimera.TextChanged
        If cboPrimera.SelectedValue = "SI" Then
            cboCual.SelectedValue = "0"
            cboCual.Enabled = True
        Else
            cboCual.SelectedValue = "0"
            cboCual.Enabled = False
        End If
    End Sub

    Private Sub Limpiar()
        txtMovimiento.Text = ""
        cboPrimera.SelectedValue = "0"
        cboCual.SelectedValue = "0"
        cboSegunda.SelectedValue = "0"
        cboTercera.SelectedValue = "0"
        cboCuarta.SelectedValue = "0"
        cboQuinta.SelectedValue = "0"
        cboSexta.SelectedValue = "0"
        cboSeptima.SelectedValue = "0"
        cboOctava.SelectedValue = "0"
        cboNovena.SelectedValue = "0"
        cboDecima.SelectedValue = "0"
    End Sub
End Class
