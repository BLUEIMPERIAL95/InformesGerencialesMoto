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
Partial Class CuentaCobro
    Inherits System.Web.UI.Page
    Dim cscuco As New Cuentacob
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Shared da As SqlDataAdapter
    Shared dt As DataTable
    Shared con As SqlConnection = New SqlConnection("Server=192.168.9.250; Database=Mototransportar; User Id=sa; Password=Adm789**;")
    Dim idTercero As String

    Private Sub CuentaCobro_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2044, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
            LlenarGridDetalle()
            Calcular_Total()
        End If
    End Sub

    Private Sub LlenarGrid()
        Dim dtcuc As New DataTable

        dtcuc = cscuco.seleccionar_datos_cuentas_cobro_completo(Session("id_usua"))

        If dtcuc.Rows.Count > 0 Then
            gridCuentaCobro.DataSource = dtcuc
            gridCuentaCobro.DataBind()
        Else
            gridCuentaCobro.DataSource = Nothing
            gridCuentaCobro.DataBind()
        End If
    End Sub

    Sub combos()
        Dim dtprov, dtaut, dtempr, dtret As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo", dtempr, cboEmpresas)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtprov, cboTercero)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtaut, cboAutoriza)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_retenciones_cuenta_cobro_combo", dtret, cboTipo)
    End Sub

    Private Sub btnNuevoEnc_Click(sender As Object, e As EventArgs) Handles btnNuevoEnc.Click
        LimpiarEncabezado()
        LimpiarDetalle()

        gridDetalle.DataSource = Nothing
        gridDetalle.DataBind()
    End Sub

    Private Sub cboEmpresas_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresas.TextChanged
        Dim dtage As New DataTable

        txtNroCuenta.Text = ""

        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresas.SelectedValue)
    End Sub

    Private Sub cboAgencia_TextChanged(sender As Object, e As EventArgs) Handles cboAgencia.TextChanged
        Try
            Dim dtcuc, dtnum As New DataTable
            Dim idres As Integer

            dtcuc = cscuco.seleccionar_resolucion_por_agencia(cboAgencia.SelectedValue)

            If dtcuc.Rows.Count > 0 Then
                idres = dtcuc.Rows(0)("id_recc")
                hidresolucion.Value = idres
                dtnum = cscuco.seleccionar_proximo_numero_cuenta_cobro_por_resulucion(idres)

                If dtnum.Rows.Count > 0 Then
                    If dtnum.Rows(0)("faltantes") < 100 And dtnum.Rows(0)("faltantes") > 0 Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Unicamente le quedan " & dtnum.Rows(0)("faltantes") & " consecutivos con el actual para sobrepasar resolución...');", True)
                    Else
                        If dtnum.Rows(0)("faltantes") <= 0 Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Consecutivo resolución superado. Imposible continuar...');", True)
                            txtNroCuenta.Text = "0"
                            LimpiarEncabezado()
                            Exit Sub
                        End If
                    End If
                    txtNroCuenta.Text = dtnum.Rows(0)("proximo").ToString
                Else
                    txtNroCuenta.Text = "0"
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe resolución para agencia seleccionada...');", True)
                txtNroCuenta.Text = "0"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvarEncabezado_Click(sender As Object, e As EventArgs) Handles btnSalvarEncabezado.Click
        Try
            If cboEmpresas.SelectedValue = "0" Or cboAgencia.SelectedValue = "0" Or hidresolucion.Value = "0" Or hidresolucion.Value = "" Or txtFechaCuenta.Value = "" Or
                txtNroCuenta.Text = "" Or txtNroCuenta.Text = "0" Or hidtercero.Value = "0" Or hidsolicita.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Consecutivo o fecha de vigencia supera la resolución...');", True)
                Exit Sub
                Dim strRespuesta As String

                strRespuesta = cscuco.guardar_cuentas_cobro(hidcuentcobro.Value, cboEmpresas.SelectedValue, cboAgencia.SelectedValue, hidresolucion.Value, txtFechaCuenta.Value,
                                                            "NA", txtNroCuenta.Text, hidtercero.Value, hidsolicita.Value, Session("id_usua"), txtObservacion.Text)

                If strRespuesta = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cuenta de cobro fue salvada exitosamente...');", True)
                    LlenarGrid()
                    LimpiarEncabezado()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cuenta de cobro no fue salvada exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub LimpiarEncabezado()
        hidcuentcobro.Value = "0"
        hidresolucion.Value = "0"
        cboEmpresas.SelectedValue = "0"
        cboAgencia.SelectedValue = "0"
        txtFechaCuenta.Value = ""
        hidtercero.Value = "0"
        txtTercero.Text = ""
        txtNroCuenta.Text = ""
        hidsolicita.Value = "0"
        txtSolicita.Text = ""
        txtObservacion.Text = ""
        cboEmpresas.Enabled = True
        cboAgencia.Enabled = True
    End Sub

    Private Sub gridCuentaCobro_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridCuentaCobro.RowCommand
        Try
            Dim idcuenta As Integer
            Dim dtcuenta As New DataTable
            Dim strRespuesta As String
            If e.CommandName = "modificar" Then
                idcuenta = gridCuentaCobro.DataKeys(e.CommandArgument).Values(0)
                hidcuentcobro.Value = idcuenta

                dtcuenta = cscuco.seleccionar_proximo_numero_cuenta_cobro_por_id(hidcuentcobro.Value)

                If dtcuenta.Rows.Count > 0 Then
                    cboEmpresas.SelectedValue = dtcuenta.Rows(0)("id_emor").ToString
                    Dim dtage As New DataTable

                    csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresas.SelectedValue)
                    cboAgencia.SelectedValue = dtcuenta.Rows(0)("id_agcc").ToString
                    hidresolucion.Value = dtcuenta.Rows(0)("id_recc").ToString
                    txtFechaCuenta.Value = dtcuenta.Rows(0)("fecha_cuco").ToString
                    txtNroCuenta.Text = dtcuenta.Rows(0)("numero_cuco").ToString
                    hidtercero.Value = dtcuenta.Rows(0)("id_terc").ToString
                    txtTercero.Text = dtcuenta.Rows(0)("nom_ter").ToString
                    hidsolicita.Value = dtcuenta.Rows(0)("id_terc_autoriza").ToString
                    txtSolicita.Text = dtcuenta.Rows(0)("nom_sol").ToString
                    txtObservacion.Text = dtcuenta.Rows(0)("observacion_cuo").ToString

                    cboEmpresas.Enabled = False
                    cboAgencia.Enabled = False

                    LlenarGridDetalle()
                    Calcular_Total()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cuenta de cobro no válida');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idcuenta = gridCuentaCobro.DataKeys(e.CommandArgument).Values(0)
                    hidcuentcobro.Value = idcuenta

                    strRespuesta = cscuco.eliminar_cuentas_cobro_encabezado(hidcuentcobro.Value)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cuenta de cobro fue eliminada exitosamente...');", True)
                        LlenarGrid()
                        LimpiarEncabezado()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cuenta de cobro no fue eliminada exitosamente...');", True)
                    End If
                Else
                    If e.CommandName = "imprimir" Then
                        idcuenta = gridCuentaCobro.DataKeys(e.CommandArgument).Values(0)
                        hidcuentcobro.Value = idcuenta

                        Response.Redirect("FormatoCuentaCobro.aspx?idoc=" & hidcuentcobro.Value)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub LlenarGridDetalle()
        Dim dtcuc As New DataTable

        dtcuc = cscuco.seleccionar_cuenta_cobro_detalle_por_id_cuenta(hidcuentcobro.Value)

        If dtcuc.Rows.Count > 0 Then
            gridDetalle.DataSource = dtcuc
            gridDetalle.DataBind()
        Else
            gridDetalle.DataSource = Nothing
            gridDetalle.DataBind()
        End If
    End Sub

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        Try
            If hidcuentcobro.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No ha seleccionado ninguna cuenta de cobro...');", True)
            Else
                If txtConcepto.Text = "" Or txtCantidad.Text = "" Or txtCosto.Text = "" Or txtValor.Text = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
                Else
                    Dim strRespuesta As String

                    If txtValor.Text < CInt(hidbaseretencion.Value) Then
                        txtRetencion.Text = "0"
                    End If

                    strRespuesta = cscuco.guardar_detalle_cuentas_cobro(hidcuentcobrodetalle.Value, hidcuentcobro.Value, cboTipo.SelectedValue, txtConcepto.Text, txtCantidad.Text,
                                                                        txtCosto.Text, Replace(txtRetencion.Text, ".", ","), txtValor.Text, txtObservacionDetalle.Text)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Cuenta de cobro fue salvado exitosamente...');", True)
                        LlenarGridDetalle()
                        LimpiarDetalle()
                        Calcular_Total()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Cuenta de cobro no fue salvado exitosamente...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub LimpiarDetalle()
        hidcuentcobrodetalle.Value = "0"
        cboTipo.SelectedValue = "0"
        txtConcepto.Text = ""
        txtCantidad.Text = ""
        txtCosto.Text = ""
        txtRetencion.Text = "0"
        txtValor.Text = ""
        txtObservacionDetalle.Text = ""
    End Sub

    Private Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click
        If txtCantidad.Text = "" Or txtCantidad.Text = "0" Or txtCosto.Text = "" Or txtCosto.Text = "0" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe digitar cantidad y costo unitario...');", True)
        Else
            txtValor.Text = txtCantidad.Text * txtCosto.Text
        End If
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            Dim idcuenta As Integer
            Dim dtcuenta As New DataTable
            Dim strRespuesta As String
            If e.CommandName = "modificar" Then
                idcuenta = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                hidcuentcobrodetalle.Value = idcuenta

                dtcuenta = cscuco.seleccionar_cuenta_cobro_detalle_por_id_detalle(hidcuentcobrodetalle.Value)

                If dtcuenta.Rows.Count > 0 Then
                    cboTipo.SelectedValue = dtcuenta.Rows(0)("id_rtcc").ToString
                    txtConcepto.Text = dtcuenta.Rows(0)("concepto_ccde").ToString
                    txtCantidad.Text = dtcuenta.Rows(0)("cantidad_ccde").ToString
                    txtCosto.Text = dtcuenta.Rows(0)("valor_ccde").ToString
                    txtRetencion.Text = dtcuenta.Rows(0)("retencion_ccde").ToString
                    txtValor.Text = dtcuenta.Rows(0)("total_ccde").ToString
                    txtObservacionDetalle.Text = dtcuenta.Rows(0)("observacion_ccde").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle no válido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idcuenta = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                    hidcuentcobrodetalle.Value = idcuenta

                    strRespuesta = cscuco.eliminar_cuentas_cobro_detalle(hidcuentcobrodetalle.Value)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Cuenta de cobro fue eliminado exitosamente...');", True)
                        LlenarGridDetalle()
                        LimpiarDetalle()
                        Calcular_Total()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Cuenta de cobro no fue eliminado exitosamente...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Calcular_Total()
        Try
            Dim dtcuc As New DataTable
            Dim decTotal As Decimal
            decTotal = 0

            dtcuc = cscuco.seleccionar_cuenta_cobro_detalle_por_id_cuenta(hidcuentcobro.Value)

            For i As Integer = 0 To dtcuc.Rows.Count - 1
                decTotal = decTotal + (dtcuc.Rows(i)("total_ccde") - (dtcuc.Rows(i)("total_ccde") * (dtcuc.Rows(i)("retencion_ccde") / 100)))
            Next

            lblTotalCuenta.Text = "VALOR TOTAL CUENTA: " & String.Format("{0:c}", decTotal)
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevoDet_Click(sender As Object, e As EventArgs) Handles btnNuevoDet.Click
        LimpiarDetalle()
    End Sub

    Private Sub cboTipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipo.TextChanged
        Try
            Dim dtRet As New DataTable

            dtRet = cscuco.seleccionar_retencion_cuenta_cobro_por_id(cboTipo.SelectedValue)

            If dtRet.Rows.Count > 0 Then
                txtRetencion.Text = dtRet.Rows(0)("porcentaje_rtcc").ToString
                hidbaseretencion.Value = dtRet.Rows(0)("base_rtcc").ToString
            Else
                txtRetencion.Text = "0"
                hidbaseretencion.Value = "0"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles imgBuscar.Click
        Try
            Dim dtcuc As New DataTable

            If txtBuscar.Text = "" Then
                dtcuc = cscuco.seleccionar_datos_cuentas_cobro_busqueda(Session("id_usua"), 0, "")
            Else
                If rdNumero.Checked = True Then
                    dtcuc = cscuco.seleccionar_datos_cuentas_cobro_busqueda(Session("id_usua"), 1, txtBuscar.Text)
                Else
                    dtcuc = cscuco.seleccionar_datos_cuentas_cobro_busqueda(Session("id_usua"), 2, txtBuscar.Text)
                End If
            End If

            If dtcuc.Rows.Count > 0 Then
                gridCuentaCobro.DataSource = dtcuc
                gridCuentaCobro.DataBind()
            Else
                gridCuentaCobro.DataSource = Nothing
                gridCuentaCobro.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub rdNumero_CheckedChanged(sender As Object, e As EventArgs) Handles rdNumero.CheckedChanged
        If rdNumero.Checked = True Then
            rdTercero.Checked = False
        Else
            rdTercero.Checked = True
        End If
    End Sub

    Private Sub rdTercero_CheckedChanged(sender As Object, e As EventArgs) Handles rdTercero.CheckedChanged
        If rdTercero.Checked = True Then
            rdNumero.Checked = False
        Else
            rdNumero.Checked = True
        End If
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

    Private Sub txtTercero_TextChanged(sender As Object, e As EventArgs) Handles txtTercero.TextChanged

    End Sub

    Private Sub txtSolicita_TextChanged(sender As Object, e As EventArgs) Handles txtSolicita.TextChanged

    End Sub
End Class
