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
Partial Class RespuestaReunionPendienteTercero
    Inherits System.Web.UI.Page
    Dim csreun As New reunion

    Private Sub RespuestaReunionPendienteTercero_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then
            Dim dtpen As New DataTable

            hidPendiente.Value = Request.QueryString("idpen")
            hidTercero.Value = Request.QueryString("idter")

            dtpen = csreun.capturar_datos_pendiente_reunion_por_id_pendiente(hidPendiente.Value)

            If dtpen.Rows.Count > 0 Then
                If dtpen.Rows(0)("estado_peru").ToString = "ASIGNADO" Or dtpen.Rows(0)("estado_peru").ToString = "PRECERRADO" Then
                    cboEstado.SelectedValue = dtpen.Rows(0)("estado_peru").ToString

                    txtCodigoPen.Text = dtpen.Rows(0)("codigo_peru").ToString
                    cboPrioridad.SelectedValue = dtpen.Rows(0)("prioridad_peru").ToString
                    txtNombrePen.Text = dtpen.Rows(0)("nombre_peru").ToString
                    txtDesPendiente.Text = dtpen.Rows(0)("descripcion_peru").ToString
                    txtFechaFin.Value = dtpen.Rows(0)("plazo_peru").ToString
                    txtRespuestasAnteriores.Text = dtpen.Rows(0)("respuestas").ToString

                    btnSalvarRespuesta.Enabled = True
                Else
                    hidPendiente.Value = ""
                    hidTercero.Value = ""
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente no valido');", True)
            End If
        End If
    End Sub

    Private Sub btnSalvarRespuesta_Click(sender As Object, e As EventArgs) Handles btnSalvarRespuesta.Click
        Try
            If cboEstado.SelectedValue = "0" Or txtRespuesta.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csreun.guardar_respuesta_pendientes_correos(hidPendiente.Value, hidTercero.Value, cboEstado.SelectedValue, txtRespuesta.Text)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Respuesta actualizada con exito...');", True)
                    Limpiar()
                    Dim dtpen As New DataTable

                    hidPendiente.Value = Request.QueryString("idpen")
                    hidTercero.Value = Request.QueryString("idter")

                    dtpen = csreun.capturar_datos_pendiente_reunion_por_id_pendiente(hidPendiente.Value)

                    If dtpen.Rows.Count > 0 Then
                        If dtpen.Rows(0)("estado_peru").ToString = "ASIGNADO" Or dtpen.Rows(0)("estado_peru").ToString = "PRECERRADO" Then
                            cboEstado.SelectedValue = dtpen.Rows(0)("estado_peru").ToString

                            txtCodigoPen.Text = dtpen.Rows(0)("codigo_peru").ToString
                            cboPrioridad.SelectedValue = dtpen.Rows(0)("prioridad_peru").ToString
                            txtNombrePen.Text = dtpen.Rows(0)("nombre_peru").ToString
                            txtDesPendiente.Text = dtpen.Rows(0)("descripcion_peru").ToString
                            txtFechaFin.Value = dtpen.Rows(0)("plazo_peru").ToString
                            txtRespuestasAnteriores.Text = dtpen.Rows(0)("respuestas").ToString

                            btnSalvarRespuesta.Enabled = True
                        Else
                            hidPendiente.Value = ""
                            hidTercero.Value = ""
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente no valido');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Respuesta no actualizada con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub Limpiar()
        txtCodigoPen.Text = ""
        cboPrioridad.SelectedValue = 0
        cboEstado.SelectedValue = 0
        txtNombrePen.Text = ""
        txtDesPendiente.Text = ""
        txtFechaFin.Value = ""
        txtRespuesta.Text = ""
        hidPendiente.Value = ""
        hidTercero.Value = ""
        btnSalvarRespuesta.Enabled = False
        'Response.Redirect("")
    End Sub
End Class
