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
Imports DPFP
Imports DPFP.Capture
Imports System.Drawing
Imports System.Data.SqlClient
Partial Class novedades_nomina_detalle
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csviat As New Novedades
    Shared da As SqlDataAdapter
    Shared dt As DataTable
    Shared con As SqlConnection = New SqlConnection("Server=192.168.9.250; Database=Mototransportar; User Id=sa; Password=Adm789**;")
    Dim idTercero As String

    Private Sub viaticosdetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(4075, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If

            If Me.IsPostBack = False Then
                combos()

                If Request.QueryString("est") IsNot Nothing Then
                    If Request.QueryString("est") = "exi" Then
                        If (Day(Request.QueryString("fec")) > 10 And Day(Request.QueryString("fec")) <= 15) Or (Day(Request.QueryString("fec")) > 25 And Day(Request.QueryString("fec")) <= 31) Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Señor usuario, recuerde que las novedades se reciben hasta el 10 para la primera quincena y hasta el 25 para la segunda quincena. Las novedades montadas posterior a estas fechas serán gestionadas para la quincena posterior.');", True)
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Actualizado Exitosamente...');", True)
                        End If

                        If Request.QueryString("id") <> Nothing Then
                            hidviatico.Value = Request.QueryString("id")
                        End If

                        Dim dtcad As New DataTable

                        dtcad = csviat.seleccionar_novedades(3, hidviatico.Value)

                        If dtcad.Rows.Count > 0 Then
                            lblFacturaEncabezado.Text = "Encabezado Novedad Nomina " & dtcad.Rows(0)("estado_nono").ToString
                            txtNumero.Text = dtcad.Rows(0)("numero_nono").ToString
                            txtFecha.Value = dtcad.Rows(0)("fecha_nono").ToString
                            hidproveedor.Value = dtcad.Rows(0)("id_terc").ToString
                            txtProveedor.Text = dtcad.Rows(0)("nombre_terc").ToString
                            txtObservacion.Text = dtcad.Rows(0)("observacion_nono").ToString

                            Llenar_Grid_Detalle()
                        End If
                    End If
                End If

                If Request.QueryString("id") <> Nothing Then
                    hidviatico.Value = Request.QueryString("id")
                End If

                If hidviatico.Value = 0 Then
                    Dim dtvia As New DataTable

                    dtvia = csviat.seleccionar_numero_proximo_novedades_nomina

                    If dtvia.Rows.Count > 0 Then
                        txtNumero.Text = dtvia.Rows(0)("proximo").ToString
                    Else
                        txtNumero.Text = 1
                    End If
                Else
                    Dim dtcad As New DataTable

                    dtcad = csviat.seleccionar_novedades(3, hidviatico.Value)

                    If dtcad.Rows.Count > 0 Then
                        lblFacturaEncabezado.Text = "Encabezado Novedad Nomina " & dtcad.Rows(0)("estado_nono").ToString
                        txtNumero.Text = dtcad.Rows(0)("numero_nono").ToString
                        txtFecha.Value = dtcad.Rows(0)("fecha_nono").ToString
                        hidproveedor.Value = dtcad.Rows(0)("id_terc").ToString
                        txtProveedor.Text = dtcad.Rows(0)("nombre_terc").ToString
                        txtObservacion.Text = dtcad.Rows(0)("observacion_nono").ToString

                        Llenar_Grid_Detalle()
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub combos()
        Dim dtegre As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_conceptos_novedades_combo", dtegre, cboTipo)
    End Sub

    Private Sub btnSalvarEncabezado_Click(sender As Object, e As EventArgs) Handles btnSalvarEncabezado.Click
        Try
            If txtNumero.Text = "" Or txtFecha.Value = "" Or hidestado.Value = "EMITIDO" Or txtFecha.Value = "" Or hidproveedor.Value = "" Or hidproveedor.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar o la novedad de nomina ya fue emitido...');", True)
            Else
                Dim dtcae As New DataTable

                dtcae = csviat.guardar_novedad_nomina_encabezado(hidviatico.Value, CInt(txtNumero.Text), txtFecha.Value, hidproveedor.Value, txtObservacion.Text, Session("id_usua"))

                If dtcae.Rows.Count > 0 Then
                    Response.Redirect("novedades_nomina_detalle.aspx?id=" & dtcae.Rows(0)("ide_viat") & "&est=exi&fec=" & txtFecha.Value & "")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Llenar_Grid_Detalle()
        Dim dtcade As New DataTable

        dtcade = csviat.seleccionar_novedades_nomina_detalle(hidviatico.Value)

        If dtcade.Rows.Count > 0 Then
            gridDetalle.DataSource = dtcade
            gridDetalle.DataBind()
        Else
            gridDetalle.DataSource = dtcade
            gridDetalle.DataBind()
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <System.Web.Services.WebMethod>
    Public Shared Function GetSearch(ByVal prefixText As String) As List(Of String)
        Dim Result As DataTable = New DataTable()
        Dim str As String = "select tbl_terceros.id_TERC, LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') FROM tbl_terceros WHERE tbl_terceros.tipo = 'Empleado' AND LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') LIKE '%" & prefixText & "%' ORDER BY LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') "
        da = New SqlDataAdapter(str, con)
        dt = New DataTable()
        da.Fill(dt)
        Dim Output As List(Of String) = New List(Of String)()

        For i As Integer = 0 To dt.Rows.Count - 1
            Output.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows(i)(1).ToString(), dt.Rows(i)(0).ToString()))
        Next

        Return Output
    End Function

    <System.Web.Script.Services.ScriptMethod()>
    <System.Web.Services.WebMethod>
    Public Shared Function GetSearch1(ByVal prefixText As String) As List(Of String)
        Dim Result As DataTable = New DataTable()
        Dim str As String = "select tbl_terceros.id_TERC, LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') FROM tbl_terceros WHERE tbl_terceros.tipo = 'Empleado' AND LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') LIKE '%" & prefixText & "%' ORDER BY LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') "
        da = New SqlDataAdapter(str, con)
        dt = New DataTable()
        da.Fill(dt)
        Dim Output As List(Of String) = New List(Of String)()

        For i As Integer = 0 To dt.Rows.Count - 1
            Output.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows(i)(1).ToString(), dt.Rows(i)(0).ToString()))
        Next

        Return Output
    End Function

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        Dim intSalvar As Integer
        intSalvar = 0

        If hidtiponovedad.Value = "1" Then
            If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
            Else
                intSalvar = 1
            End If
        End If

        If hidtiponovedad.Value = "2" Then
            If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or txtValor.Text = "" Or txtValor.Text = "0" Or cboCuotas.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
            Else
                intSalvar = 1
            End If
        End If

        If hidtiponovedad.Value = "3" Then
            If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtFechaInicio.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
            Else
                intSalvar = 1
            End If
        End If

        If hidtiponovedad.Value = "4" Then
            If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtFechaInicio.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
            Else
                intSalvar = 1
            End If
        End If

        If hidtiponovedad.Value = "5" Then
            If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
            Else
                intSalvar = 1
            End If
        End If

        If intSalvar = 1 Then
            Dim dtcae As New DataTable

            dtcae = csviat.guardar_novedad_nomina_detalle(hiddetalle.Value, hidviatico.Value, hidtercero.Value, cboTipo.SelectedValue, txtReferencia.Text, txtFechaInicio.Value, txtFechaFin.Value, txtCantidad.Text, txtValor.Text, cboCuotas.SelectedValue, txtDescripcion.Text)

            If dtcae.Rows.Count > 0 Then
                If dtcae.Rows(0)("ide_nono").ToString = "" Then
                    Llenar_Grid_Detalle()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad emitida o falta información por digitar...');", True)
        End If
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            Dim iddet As Integer
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "eliminar" Then
                If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad EMITIDO. Imposible eliminar detalle...');", True)
                Else
                    iddet = gridDetalle.DataKeys(e.CommandArgument).Values(0)

                    strRespuesta = csviat.eliminar_novedad_nomina_detalle(iddet)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle fue eliminado exitosamente...');", True)
                        Llenar_Grid_Detalle()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle no fue eliminado exitosamente...');", True)
                    End If
                End If
            Else
                If e.CommandName = "imagen" Then
                    'If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                    '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Novedad EMITIDO. Imposible subir imagenes...');", True)
                    'Else
                    hidordendetalle.Value = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "AbriCargueImagen('" + hidordendetalle.Value + "');", True)
                    'End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    'Private Sub calcular_totales()
    '    Dim dtviat As New DataTable

    '    dtviat = csviat.seleccionar_viaticos("3", hidviatico.Value)

    '    If dtviat.Rows.Count > 0 Then
    '        txtEntradas.Text = dtviat.Rows(0)("valor_entrada")
    '        txtSalidas.Text = dtviat.Rows(0)("valor_salida")
    '        txtSaldo.Text = dtviat.Rows(0)("valor_entrada") - dtviat.Rows(0)("valor_salida")
    '    Else
    '        txtEntradas.Text = 0
    '        txtSalidas.Text = 0
    '        txtSaldo.Text = 0
    '    End If
    'End Sub

    Private Sub imgPdf_Click(sender As Object, e As ImageClickEventArgs) Handles imgPdf.Click
        Try
            If lblFacturaEncabezado.Text.Contains("SIN EMITIR") Then
                Dim strRes As String

                strRes = csviat.emitir_novedad_nomina(hidviatico.Value)

                If strRes = "" Then
                    'Response.Redirect("novedades_nomina_detalle.aspx?id=" & hidviatico.Value & "&est=exi")
                    Response.Redirect("novedades_nomina_detalle.aspx?id=" & hidviatico.Value & "&est=exi&fec=" & txtFecha.Value & "")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            Else
                Response.Redirect("Formato_Novedades_Nomina.aspx?id=" & hidviatico.Value)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboTipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipo.TextChanged
        CalendarFechaIni.Enabled = False
        CalendarFechaFin.Enabled = False
        txtCantidad.Text = "0"
        txtValor.Text = "0"
        txtCantidad.Enabled = False
        txtValor.Enabled = False
        cboCuotas.SelectedValue = "0"
        cboCuotas.Enabled = False

        Dim dtCon As New DataTable

        dtCon = csviat.seleccionar_conceptos_novedades_por_id(cboTipo.SelectedValue)

        Dim tipConcepto As Integer

        If dtCon.Rows.Count > 0 Then
            If dtCon.Rows(0)("descripcion_cono").ToString <> "N/A" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('" & dtCon.Rows(0)("descripcion_cono").ToString & "');", True)
            End If

            tipConcepto = dtCon.Rows(0)("tipo_cono").ToString

            hidtiponovedad.Value = tipConcepto

            If tipConcepto = 1 Then
                CalendarFechaIni.Enabled = True
                CalendarFechaFin.Enabled = True
            Else
                If tipConcepto = 2 Then
                    CalendarFechaIni.Enabled = True
                    CalendarFechaFin.Enabled = True
                    txtValor.Enabled = True
                    cboCuotas.Enabled = True
                Else
                    If tipConcepto = 3 Then
                        CalendarFechaIni.Enabled = True
                    Else
                        If tipConcepto = 4 Then
                            CalendarFechaIni.Enabled = True
                        Else
                            If tipConcepto = 5 Then
                                CalendarFechaFin.Enabled = True
                            Else
                                If tipConcepto = 6 Then

                                End If
                            End If
                        End If
                    End If
                End If
            End If
        Else
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tipo concepto inválido...');", True)
        End If
    End Sub
End Class
