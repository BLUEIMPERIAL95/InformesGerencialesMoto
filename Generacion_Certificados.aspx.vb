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
Partial Class Generacion_Certificados
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csterc As New equipos

    Private Sub Generacion_Certificados_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("LoginCertificados.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Private Sub combos()
        Dim dtempr, dttipo As New System.Data.DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipo_certificados_combo", dttipo, cboTipo)
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            If cboTipo.SelectedValue = "0" Or cboPeriodo.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe seleccionar tipo certificado y periodo...');", True)
            Else
                Dim dtcer As New DataTable

                dtcer = csterc.seleccionar_datos_certificados_por_documento_tipo_periodo(cboTipo.SelectedValue, cboPeriodo.SelectedValue, Session("documento"))

                If dtcer.Rows.Count > 0 Then
                    gridCertificados.DataSource = dtcer
                    gridCertificados.DataBind()
                Else
                    gridCertificados.DataSource = Nothing
                    gridCertificados.DataBind()
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub gridCertificados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCertificados.RowCommand
        Try
            If e.CommandName = "imprimir" Then
                Dim idemor, bimestre As Integer

                idemor = gridCertificados.DataKeys(e.CommandArgument).Values(0)
                bimestre = gridCertificados.DataKeys(e.CommandArgument).Values(1)

                Response.Redirect("Formato_Certificado_Ica.aspx?emp=" & idemor & "&tip=" & cboTipo.SelectedValue & "&per=" & cboPeriodo.SelectedValue & "&doc=" & Session("documento") & "&bim=" & bimestre & "")
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub
End Class
