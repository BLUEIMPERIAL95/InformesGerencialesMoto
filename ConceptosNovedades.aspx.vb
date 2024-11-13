Imports System.Data
Partial Class ConceptosNovedades
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csnove As New Novedades

    Private Sub ConceptosNovedades_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("../login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(4075, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            LlenarGrid()
        End If
    End Sub

    Private Sub LlenarGrid()
        Try
            Dim dtconc As New DataTable

            dtconc = csnove.seleccionar_conceptos_novedades_completo()

            gridConceptosFac.DataSource = dtconc
            gridConceptosFac.DataBind()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtNombre.Text = "" Or cboActivo.SelectedValue = "-1" Or txtReferencia.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRespuesta As String

                strRespuesta = csnove.guardar_conceptos_novedades(hidconcepto.Value, txtNombre.Text, txtReferencia.Text,
                                                                  cboActivo.SelectedValue)

                If strRespuesta = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Concepto almacenado con éxito.');", True)
                    LlenarGrid()
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Concepto no almacenado con éxito.');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Limpiar()
        hidconcepto.Value = "0"
        txtNombre.Text = ""
        txtReferencia.Text = ""
        cboActivo.SelectedValue = "-1"
    End Sub

    Private Sub gridConceptosFac_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridConceptosFac.RowCommand
        Try
            Dim idconc As Integer
            Dim dtcon As New DataTable
            Dim strRes As String

            If e.CommandName = "modificar" Then
                idconc = gridConceptosFac.DataKeys(e.CommandArgument).Values(0)
                hidconcepto.Value = idconc

                dtcon = csnove.seleccionar_conceptos_novedades_por_id(hidconcepto.Value)

                If dtcon.Rows.Count > 0 Then
                    hidconcepto.Value = dtcon.Rows(0)("id_cono").ToString
                    txtNombre.Text = dtcon.Rows(0)("nombre_cono").ToString
                    txtReferencia.Text = dtcon.Rows(0)("referencia_cono").ToString
                    cboActivo.SelectedValue = dtcon.Rows(0)("activo").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Concepto no válido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idconc = gridConceptosFac.DataKeys(e.CommandArgument).Values(0)
                    hidconcepto.Value = idconc

                    strRes = csnove.eliminar_conceptos_novedades_por_id(hidconcepto.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Concepto eliminado con éxito.');", True)
                        Limpiar()
                        LlenarGrid()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Concepto no eliminado con éxito.');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
