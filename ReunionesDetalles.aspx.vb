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
Imports System.Data.SqlClient
Partial Class ReunionesDetalles
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csreun As New reunion
    Dim csoper As New Operaciones
    Shared da As SqlDataAdapter
    Shared dt As DataTable
    Shared con As SqlConnection = New SqlConnection("Server=192.168.9.250; Database=Mototransportar; User Id=sa; Password=Adm789**;")
    Dim idTercero As String

    Private Sub ReunionesDetalles_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(33, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        'If strRespuestaPer <> "" Then
        '    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
        '    'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
        '    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect",
        '    "alert('" & strRespuestaPer & "'); window.location='" +
        '    Request.ApplicationPath + "/Default.aspx';", True)
        '    'Response.Redirect("Default.aspx")
        'End If

        If Me.IsPostBack = False Then
            combos()

            Dim dtreu, dtcod As New DataTable

            If Request.QueryString("idreu") IsNot Nothing Then
                hidReunion.Value = Request.QueryString("idreu")
            End If

            If hidReunion.Value > 1 Then
                Cargar_Reunion_Completa(hidReunion.Value)
            End If
        End If
    End Sub

    Sub combos()
        Dim dtper As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipo_reuniones_combo", dtper, cboTipoReunion)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtper, cboExpositor)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtper, cboResponsable)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtper, cboParticipantes)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtper, cboSolicita)
    End Sub

    Sub LlenarGridParticipantes()
        Dim dtpar As New DataTable

        dtpar = csreun.seleccionar_participantes_reunion(hidReunion.Value)

        gridParticipantes.DataSource = dtpar
        gridParticipantes.DataBind()
    End Sub

    Sub LlenarGridPendientes()
        Dim dtpen As New DataTable

        dtpen = csreun.capturar_datos_pendientes_reuniones_por_id_reunion(hidReunion.Value)

        grdPendientes.DataSource = dtpen
        grdPendientes.DataBind()

        Dim fila As GridViewRow

        Dim strResTer As String
        Dim idpen As Integer
        For Each fila In grdPendientes.Rows
            idpen = grdPendientes.DataKeys(fila.RowIndex).Value
            Dim dtres As New DataTable
            strResTer = ""
            Dim btmodificar As ImageButton = TryCast(grdPendientes.Rows(fila.RowIndex).Cells(6).Controls(0), ImageButton)

            dtres = csreun.seleccionar_respuestas_pendiente_reunion_terceros_por_id_pendiente(idpen)

            If dtres.Rows.Count > 0 Then
                For i As Integer = 0 To dtres.Rows.Count - 1
                    strResTer = strResTer & " " & dtres.Rows(i)("respuesta").ToString
                Next
                btmodificar.ToolTip = strResTer
            Else
                btmodificar.ToolTip = "Sin Respuesta"
            End If
        Next
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtCodigo.Text = "" Or txtFecha.Value = "" Or txtNombre.Text = "" Or txtDescripcion.Text = "" Or cboTipoReunion.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim dtres As New DataTable
                Dim intAct As Integer

                'If cboActivo.Text = "SI" Then
                intAct = cboActivo.SelectedValue
                'Else
                '    intAct = 0
                'End If

                dtres = csreun.guardar_reuniones(txtCodigo.Text, txtFecha.Value, txtNombre.Text, txtDescripcion.Text, cboTipoReunion.SelectedValue,
                                                 hidExpositor.Value, txtObjetivo.Text, "00:00:00", txtLugar.Text, intAct)


                If dtres.Rows.Count > 0 Then
                    If dtres.Rows(0)("id") > 1 Then
                        hidReunion.Value = dtres.Rows(0)("id").ToString
                        Cargar_Reunion_Completa(hidReunion.Value)
                    Else
                        Cargar_Reunion_Completa(hidReunion.Value)
                    End If
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunión actualizada con exito...');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunión no actualizada con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub Cargar_Reunion_Completa(ByVal id As Integer)
        Try
            If id <> "0" Then
                Dim dtreu As New DataTable

                cboTipoReunion.Enabled = False
                dtreu = csreun.capturar_datos_reuniones_por_id(hidReunion.Value)

                If dtreu.Rows.Count > 0 Then
                    cboTipoReunion.SelectedValue = dtreu.Rows(0)("abrevia_tire").ToString
                    txtCodigo.Text = dtreu.Rows(0)("codigo_reun").ToString
                    txtFecha.Value = dtreu.Rows(0)("fecha_reun").ToString
                    txtNombre.Text = dtreu.Rows(0)("nombre_reun").ToString
                    txtDescripcion.Text = dtreu.Rows(0)("descripcion_reun").ToString
                    hidExpositor.Value = dtreu.Rows(0)("id_expo").ToString
                    txtExpositor.Text = dtreu.Rows(0)("nombre_terc").ToString
                    txtObjetivo.Text = dtreu.Rows(0)("objetivo_reun").ToString
                    txtLugar.Text = dtreu.Rows(0)("lugar_reun").ToString
                    cboActivo.SelectedValue = dtreu.Rows(0)("activo_reun").ToString

                    LlenarGridParticipantes()

                    LlenarGridPendientes()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion Invalida...');", True)
                End If

                Sugerir_Codigo_Pendiente()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub btnSalvarPendiente_Click(sender As Object, e As EventArgs) Handles btnSalvarPendiente.Click
        Try
            If txtCodigoPen.Text = "" Or cboPrioridad.SelectedValue = "0" Or cboEstado.SelectedValue = "0" Or txtFechaFin.Value = "" Or txtNombre.Text = "" Or txtDesPendiente.Text = "" Or hidSolicita.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csreun.guardar_pendientes_reuniones(hidReunion.Value, txtCodigoPen.Text, txtNombrePen.Text, txtDesPendiente.Text, cboEstado.SelectedValue, cboPrioridad.SelectedValue,
                                                 txtFechaFin.Value, hidSolicita.Value, 1)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente actualizado con exito...');", True)
                    Limpiar_Pendiente()
                    LlenarGridPendientes()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente no actualizado con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Sub Sugerir_Codigo_Pendiente()
        Dim dtcod As New DataTable

        dtcod = csreun.seleccionar_proximo_codigo_pendientes_reuniones(hidReunion.Value)

        If dtcod.Rows.Count > 0 Then
            txtCodigoPen.Text = dtcod.Rows(0)("cod_pen").ToString
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Codigo Invalido...');", True)
        End If
    End Sub

    Sub Limpiar_Pendiente()
        txtCodigoPen.Text = ""
        cboEstado.SelectedValue = "0"
        cboPrioridad.SelectedValue = "0"
        txtDesPendiente.Text = ""
        txtFechaFin.Value = ""
        hidPendiente.Value = ""
        hidSolicita.Value = "0"
        Sugerir_Codigo_Pendiente()
    End Sub

    Private Sub btnNuevoPendiente_Click(sender As Object, e As EventArgs) Handles btnNuevoPendiente.Click
        Limpiar_Pendiente()
    End Sub

    Private Sub grdPendientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdPendientes.RowCommand
        Try
            Dim idpendiente As Integer
            Dim dtpen As New DataTable
            Dim strRes, strCodPendiente As String
            strCodPendiente = ""

            If e.CommandName = "modificar" Then
                idpendiente = grdPendientes.DataKeys(e.CommandArgument).Values(0)
                hidPendiente.Value = idpendiente

                dtpen = csreun.capturar_datos_pendiente_reunion_por_id_pendiente(hidPendiente.Value)

                If dtpen.Rows.Count > 0 Then
                    txtCodigoPen.Text = dtpen.Rows(0)("codigo_peru").ToString
                    cboPrioridad.SelectedValue = dtpen.Rows(0)("prioridad_peru").ToString
                    cboEstado.SelectedValue = dtpen.Rows(0)("estado_peru").ToString
                    If cboEstado.SelectedValue = "CERRADO" Then
                        btnAsignar.Enabled = False
                    Else
                        btnAsignar.Enabled = True
                    End If
                    txtNombrePen.Text = dtpen.Rows(0)("nombre_peru").ToString
                    txtDesPendiente.Text = dtpen.Rows(0)("descripcion_peru").ToString
                    txtFechaFin.Value = dtpen.Rows(0)("plazo_peru").ToString
                    hidSolicita.Value = dtpen.Rows(0)("id_terc").ToString
                    txtSolicita.Text = dtpen.Rows(0)("nombre_terc").ToString
                    lblAsignacionPendiente.Text = "ASIGNACION PENDIENTE " & txtNombrePen.Text

                    LlenarGridPendientesTerceros()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idpendiente = grdPendientes.DataKeys(e.CommandArgument).Values(0)
                    hidPendiente.Value = idpendiente

                    strRes = csreun.eliminar_pendiente_reunion_por_id_pendiente(hidPendiente.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion Pendiente eliminado');", True)
                        LlenarGridPendientes()
                        Limpiar_Pendiente()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion Pendiente no eliminado');", True)
                    End If
                Else
                    If e.CommandName = "imprimir" Then
                        idpendiente = grdPendientes.DataKeys(e.CommandArgument).Values(0)
                        strCodPendiente = grdPendientes.DataKeys(e.CommandArgument).Values(1)
                        hidPendiente.Value = idpendiente

                        Response.Redirect("FormatoPendienteDeReunionCorreo.aspx?idpen=" & hidPendiente.Value & "&codpen=" & strCodPendiente & "&idreu=" & hidReunion.Value & "")
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnAsignar_Click(sender As Object, e As EventArgs) Handles btnAsignar.Click
        Try
            If hidResponsable.Value = "0" Or hidPendiente.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csreun.guardar_pendientes_reuniones_terceros(hidPendiente.Value, hidResponsable.Value)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente Tercero actualizado con exito...');", True)
                    Limpiar_Pendiente_Tercero()
                    LlenarGridPendientesTerceros()
                    Cargar_Pendiente(hidPendiente.Value)
                    'Response.Redirect("FormatoPendienteDeReunionCorreo.aspx?idpen=" & hidPendiente.Value & "&codpen=" & txtCodigoPen.Text & "&idreu=" & hidReunion.Value & "&idter=" & cboResponsable.SelectedValue & "")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente Tercero no actualizado con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Sub Limpiar_Pendiente_Tercero()
        hidResponsable.Value = "0"
        hidPendienteTercero.Value = ""
    End Sub

    Sub LlenarGridPendientesTerceros()
        Dim dtpen As New DataTable

        dtpen = csreun.capturar_datos_pendiente_reunion_terceros_por_id_pendiente(hidPendiente.Value)

        grdPendientesTerceros.DataSource = dtpen
        grdPendientesTerceros.DataBind()
    End Sub

    Private Sub grdPendientesTerceros_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles grdPendientesTerceros.RowCommand
        Try
            Dim idpendiente, idtercero As Integer
            Dim dtpen As New DataTable
            Dim strRes, strCodPendiente As String

            If e.CommandName = "eliminar" Then
                idpendiente = grdPendientesTerceros.DataKeys(e.CommandArgument).Values(0)
                hidPendienteTercero.Value = idpendiente

                strRes = csreun.eliminar_pendiente_reunion_terceros_por_id_pendiente_tercero(hidPendienteTercero.Value)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion Pendiente Tercero eliminado');", True)
                    LlenarGridPendientesTerceros()
                    Limpiar_Pendiente_Tercero()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Reunion Pendiente Tercero no eliminado');", True)
                End If
            Else
                If e.CommandName = "imprimir" Then
                    idpendiente = grdPendientesTerceros.DataKeys(e.CommandArgument).Values(1)
                    strCodPendiente = grdPendientesTerceros.DataKeys(e.CommandArgument).Values(2)
                    idtercero = grdPendientesTerceros.DataKeys(e.CommandArgument).Values(3)
                    hidPendiente.Value = idpendiente

                    Response.Redirect("FormatoPendienteDeReunionCorreo.aspx?idpen=" & hidPendiente.Value & "&codpen=" & strCodPendiente & "&idreu=" & hidReunion.Value & "&idter=" & idtercero & "")
                Else
                    If e.CommandName = "ModificarEstado" Then
                        idpendiente = grdPendientesTerceros.DataKeys(e.CommandArgument).Values(0)
                        hidPendienteTercero.Value = idpendiente

                        strRes = csreun.modificar_pendiente_reunion_terceros_por_id_pendiente_tercero(hidPendienteTercero.Value)

                        If strRes = "" Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Estado Pendiente Reunion Tercero cerrado');", True)
                            LlenarGridPendientesTerceros()
                            Limpiar_Pendiente_Tercero()
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Estado Pendiente Reunion Tercero no cerrado');", True)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboTipoReunion_TextChanged(sender As Object, e As EventArgs) Handles cboTipoReunion.TextChanged
        txtCodigo.Text = cboTipoReunion.SelectedValue & "-" & Now.ToString("yyyyMMddhhmmss")
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click

    End Sub

    'Private Sub cboParticipantes_TextChanged(sender As Object, e As EventArgs) Handles cboParticipantes.TextChanged

    'End Sub

    Private Sub btnAsignarParticipante_Click(sender As Object, e As EventArgs) Handles btnAsignarParticipante.Click
        Try
            If hidParticipante.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe seleccionar participante...');", True)
            Else
                Dim strRes As String

                strRes = csreun.guardar_reuniones_participantes(hidReunion.Value, hidParticipante.Value, 1)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Participante asignado con exito...');", True)
                    hidParticipante.Value = "0"
                    LlenarGridParticipantes()
                    'Response.Redirect("FormatoPendienteDeReunionCorreo.aspx?idpen=" & hidPendiente.Value & "&codpen=" & txtCodigoPen.Text & "&idreu=" & hidReunion.Value & "&idter=" & cboResponsable.SelectedValue & "")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Participante no asignado con exito...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Cargar_Pendiente(ByVal idpen As Integer)
        Dim dtpen As New DataTable

        dtpen = csreun.capturar_datos_pendiente_reunion_por_id_pendiente(hidPendiente.Value)

        If dtpen.Rows.Count > 0 Then
            txtCodigoPen.Text = dtpen.Rows(0)("codigo_peru").ToString
            cboPrioridad.SelectedValue = dtpen.Rows(0)("prioridad_peru").ToString
            cboEstado.SelectedValue = dtpen.Rows(0)("estado_peru").ToString
            If cboEstado.SelectedValue = "CERRADO" Then
                btnAsignar.Enabled = False
            Else
                btnAsignar.Enabled = True
            End If
            txtNombrePen.Text = dtpen.Rows(0)("nombre_peru").ToString
            txtDesPendiente.Text = dtpen.Rows(0)("descripcion_peru").ToString
            txtFechaFin.Value = dtpen.Rows(0)("plazo_peru").ToString

            lblAsignacionPendiente.Text = "ASIGNACION PENDIENTE " & txtNombrePen.Text

            LlenarGridPendientesTerceros()
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Pendiente no valido');", True)
        End If
    End Sub

    Private Sub btnCapturarParticipantes_Click(sender As Object, e As EventArgs) Handles btnCapturarParticipantes.Click
        Try
            Dim strRes As String

            strRes = csreun.capturar_participantes_reunion_anterior(hidReunion.Value)

            If strRes = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con exito...');", True)
                LlenarGridParticipantes()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción no realizada con exito...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub gridParticipantes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridParticipantes.RowCommand
        Try
            Dim idparticipante As Integer
            Dim strRes As String

            If e.CommandName = "eliminar" Then
                idparticipante = gridParticipantes.DataKeys(e.CommandArgument).Values(0)

                strRes = csreun.eliminar_participante_reunion(idparticipante)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Participante eliminado');", True)
                    LlenarGridParticipantes()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Participante no eliminado');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub txtDescripcion_TextChanged(sender As Object, e As EventArgs) Handles txtDescripcion.TextChanged

    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <System.Web.Services.WebMethod>
    Public Shared Function GetSearch(ByVal prefixText As String) As List(Of String)
        Dim Result As DataTable = New DataTable()
        Dim str As String = "select tbl_terceros.id_TERC, LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') FROM tbl_terceros WHERE LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') LIKE '%" & prefixText & "%' ORDER BY LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') "
        da = New SqlDataAdapter(str, con)
        dt = New DataTable()
        da.Fill(dt)
        Dim Output As List(Of String) = New List(Of String)()

        For i As Integer = 0 To dt.Rows.Count - 1
            Output.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows(i)(1).ToString(), dt.Rows(i)(0).ToString()))
        Next

        Return Output
    End Function
End Class
