Imports System.Data
Partial Class BusquedaComprobanteCad
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscade As New cad

    Private Sub BusquedaComprobanteCad_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(2058, Session("id_usua"))

            If Me.IsPostBack = False Then
                combos()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtprov, dtaut, dtempr, dtret As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo_cad", dtempr, cboEmpresa)
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtNumero.Text = "" Or cboTipo.SelectedValue = "0" Or cboEmpresa.SelectedValue = "0" Or cboAgencia.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim dtCad As New DataTable

                dtCad = cscade.seleccionar_cad_envio_detalle_por_documento(txtNumero.Text, cboAgencia.SelectedValue, cboTipo.SelectedValue)

                If dtCad.Rows.Count > 0 Then
                    lblRespuesta.Text = "El número de comprobante " & txtNumero.Text & " ha sido asignado al envío # " & dtCad.Rows(0)("numero_caen").ToString & "."
                Else
                    lblRespuesta.Text = "El número de comprobante " & txtNumero.Text & " no ha sido asignado a ningún envío."
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboEmpresa_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresa.TextChanged
        Dim dtage As New DataTable

        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
    End Sub
End Class
