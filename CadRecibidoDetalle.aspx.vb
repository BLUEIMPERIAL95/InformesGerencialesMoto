Imports System.Data
Partial Class CadRecibidoDetalle
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscade As New cad
    Dim csinformes As New Informes
    Dim dtcade As New DataTable

    Private Sub CadRecibidoDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
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
                combos()

                If Request.QueryString("est") IsNot Nothing Then
                    If Request.QueryString("est") = "exi" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Actualizado Exitosamente...');", True)
                    End If
                End If

                If Request.QueryString("id") <> Nothing Then
                    hidcad.Value = Request.QueryString("id")

                    If CInt(Request.QueryString("id")) = CInt(0) Then
                        cboEnvio.Enabled = True
                    Else
                        cboEnvio.Enabled = False
                    End If
                End If

                If hidcad.Value = 0 Then
                    Dim dtcad As New DataTable

                    dtcad = cscade.seleccionar_numero_proximo_cad_recibido

                    If dtcad.Rows.Count > 0 Then
                        txtCadRecibido.Text = dtcad.Rows(0)("proximo").ToString
                    Else
                        txtCadRecibido.Text = 1
                    End If
                Else
                    Dim dtcad As New DataTable

                    dtcad = cscade.seleccionar_cad_recibido_listado(2, hidcad.Value)

                    If dtcad.Rows.Count > 0 Then
                        lblFacturaEncabezado.Text = "Encabezado Recibido Cad " & dtcad.Rows(0)("estado_care").ToString
                        hidcadenvio.Value = dtcad.Rows(0)("id_caen").ToString
                        cboEnvio.SelectedValue = dtcad.Rows(0)("id_caen").ToString
                        txtCadEnvio.Text = dtcad.Rows(0)("numero_caen").ToString
                        txtCadRecibido.Text = dtcad.Rows(0)("numero_care").ToString
                        txtFecha.Value = dtcad.Rows(0)("fecha_care").ToString
                        cboEmpresa.SelectedValue = dtcad.Rows(0)("id_emor").ToString
                        Dim dtage As New DataTable
                        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
                        cboAgencia.SelectedValue = dtcad.Rows(0)("id_agcc").ToString
                        cboDocumento.SelectedValue = dtcad.Rows(0)("tipodoc_care").ToString
                        txtObservacion.Text = dtcad.Rows(0)("observacion_care").ToString

                        Llenar_Grid_Detalle()

                        Llenar_Grid_Detalle_Detalle()
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtempr, dtcad As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo_cad", dtempr, cboEmpresa)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_cad_envios_combo", dtcad, cboEnvio)
    End Sub

    Private Sub cboEnvio_TextChanged(sender As Object, e As EventArgs) Handles cboEnvio.TextChanged
        Try
            Dim dtcad As New DataTable

            dtcad = cscade.seleccionar_cad_envio_listado(2, cboEnvio.SelectedValue)

            If dtcad.Rows.Count > 0 Then
                lblFacturaEncabezado.Text = "Encabezado Envío Cad " & dtcad.Rows(0)("estado_caen").ToString
                txtCadEnvio.Text = dtcad.Rows(0)("numero_caen").ToString
                txtFecha.Value = dtcad.Rows(0)("fecha_caen").ToString
                cboEmpresa.SelectedValue = dtcad.Rows(0)("id_emor").ToString
                Dim dtage As New DataTable
                csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
                cboAgencia.SelectedValue = dtcad.Rows(0)("id_agcc").ToString
                cboDocumento.SelectedValue = dtcad.Rows(0)("tipodoc_caen").ToString
                txtObservacion.Text = dtcad.Rows(0)("observacion_caen").ToString
                hidcadenvio.Value = cboEnvio.SelectedValue

                Llenar_Grid_Detalle()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Llenar_Grid_Detalle()
        dtcade = cscade.seleccionar_cad_recibido_seleccion(hidcadenvio.Value)

        For i As Integer = 0 To dtcade.Rows.Count - 1
            Dim dtbus As New DataTable

            dtbus = cscade.seleccionar_cad_recibido_detalle_por_documento(dtcade.Rows(i)("numero").ToString, cboAgencia.SelectedValue, cboDocumento.SelectedValue)

            If dtbus.Rows.Count > 0 Then
                dtcade.Rows(i).Delete()
            End If
        Next

        If dtcade.Rows.Count > 0 Then
            gridDocumentos.DataSource = dtcade
            gridDocumentos.DataBind()

            lblDocumentosRecibidos.Text = "ASIGNACION DOCUMENTOS # " & dtcade.Rows.Count.ToString
        Else
            gridDocumentos.DataSource = dtcade
            gridDocumentos.DataBind()
        End If
    End Sub

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        Try
            Try
                If txtCadEnvio.Text = "" Or txtCadRecibido.Text = "" Or txtFecha.Value = "" Or cboEmpresa.SelectedValue = "0" Or cboAgencia.SelectedValue = "0" Or cboDocumento.SelectedValue = "0" Or hidestado.Value = "Emitido" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar o el envío ya fue emitido...');", True)
                Else
                    Dim dtcae As New DataTable

                    If cboEnvio.SelectedValue = "0" Then
                        dtcae = cscade.guardar_cad_recibido_encabezado(hidcad.Value, hidcadenvio.Value, CInt(txtCadRecibido.Text), txtFecha.Value, CInt(cboEmpresa.SelectedValue), CInt(cboAgencia.SelectedValue),
                                                                cboDocumento.SelectedValue, txtObservacion.Text, Session("id_usua"))
                    Else
                        dtcae = cscade.guardar_cad_recibido_encabezado(hidcad.Value, cboEnvio.SelectedValue, CInt(txtCadRecibido.Text), txtFecha.Value, CInt(cboEmpresa.SelectedValue), CInt(cboAgencia.SelectedValue),
                                                                cboDocumento.SelectedValue, txtObservacion.Text, Session("id_usua"))
                    End If

                    If dtcae.Rows.Count > 0 Then
                        Dim num As Integer
                        Dim folios As Integer
                        Dim strRespuesta, strNombre As String
                        strRespuesta = ""
                        For Each orow In gridDocumentos.Rows
                            num = gridDocumentos.DataKeys(orow.RowIndex).Values(0)
                            strNombre = gridDocumentos.DataKeys(orow.RowIndex).Values(1)
                            folios = gridDocumentos.DataKeys(orow.RowIndex).Values(2)

                            Dim chek As CheckBox = orow.FindControl("chelige")

                            If chek.Checked = True Then

                                strRespuesta = strRespuesta & cscade.guardar_cad_recibido_detalle(hiddetcad.Value, dtcae.Rows(0)("id_care"), num, strNombre, folios, " ")
                            Else
                                strRespuesta = cscade.eliminar_cad_envio_detalle_por_comprobante(cboEnvio.SelectedValue, num)
                            End If
                        Next

                        Response.Redirect("CadRecibidoDetalle.aspx?id=" & dtcae.Rows(0)("id_care") & "&est=exi")
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                    End If
                End If
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
            End Try
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Llenar_Grid_Detalle_Detalle()
        Try
            dtcade = cscade.seleccionar_cad_recibido_detalle(hidcad.Value)

            If dtcade.Rows.Count > 0 Then
                gridDetalle.DataSource = dtcade
                gridDetalle.DataBind()

                lblDetalleDocumentos.Text = "DETALLES ENVÍO CAD # " & dtcade.Rows.Count.ToString
            Else
                gridDetalle.DataSource = dtcade
                gridDetalle.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            Dim iddet As Integer
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "eliminar" Then
                If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad Envío EMITIDO. Imposible eliminar detalle...');", True)
                Else
                    iddet = gridDetalle.DataKeys(e.CommandArgument).Values(0)

                    strRespuesta = cscade.eliminar_cad_recibido_detalle(iddet)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle fue eliminado exitosamente...');", True)
                        Llenar_Grid_Detalle()
                        Llenar_Grid_Detalle_Detalle()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle no fue eliminado exitosamente...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgPdf_Click(sender As Object, e As ImageClickEventArgs) Handles imgPdf.Click
        Try
            If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                Response.Redirect("Formato_Cad_Recibido.aspx?id=" & hidcad.Value)
            Else
                Dim strRes As String

                strRes = cscade.emitir_cad_recibido(hidcad.Value)

                If strRes = "" Then
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad envío emitido exitosamente...');", True)
                    Response.Redirect("CadRecibidoDetalle.aspx?id=" & hidcad.Value & "&est=exi")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad envío no emitido exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub chkTodos_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodos.CheckedChanged
        For Each orow In gridDocumentos.Rows
            Dim chek As CheckBox = orow.FindControl("chelige")

            If chek.Checked = True Then
                chek.Checked = False
            Else
                chek.Checked = True
            End If
        Next
    End Sub
End Class
