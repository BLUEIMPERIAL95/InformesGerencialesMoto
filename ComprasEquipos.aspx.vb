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

Partial Class ComprasEquipos
    Inherits System.Web.UI.Page
    Dim csorde As New OrdenesCompra
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Shared da As SqlDataAdapter
    Shared dt As DataTable
    Shared con As SqlConnection = New SqlConnection("Server=192.168.9.250; Database=Mototransportar; User Id=sa; Password=Adm789**;")
    Dim idTercero As String

    Private Sub ComprasEquipos_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(29, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
            ultima_orden()
        End If
    End Sub

    Sub combos()
        Dim dtprov, dtsol, dtaut, dtequi As New DataTable

        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtprov, cboTercero)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtsol, cboSolicitante)
        'csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtaut, cboAutoriza)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_equipos_combo", dtequi, cboEquipo)
    End Sub

    Sub LlenarGrid()
        Dim dtcomp As New DataTable

        dtcomp = csorde.seleccionar_datos_ordenes_compra_completo()

        gridOrden.DataSource = dtcomp
        gridOrden.DataBind()
    End Sub

    Private Sub btnSalvarEncabezado_Click(sender As Object, e As EventArgs) Handles btnSalvarEncabezado.Click
        Try
            If txtFechaCompra.Value = "" Or txtNroCompra.Text = "" Or hidproveedor.Value = "0" Or hidtercero.Value = "0" Or hidsolicita.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csorde.guardar_ordenes_compra(hidorden.Value, txtFechaCompra.Value, txtNroCompra.Text, hidproveedor.Value, hidtercero.Value,
                                                       hidsolicita.Value, txtObservacion.Text, 1)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden Compra actualizada con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden Compra no actualizada');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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

    Private Sub Limpiar()
        hidorden.Value = "0"
        txtFechaCompra.Value = ""
        txtNroCompra.Text = ""
        hidproveedor.Value = "0"
        txtProveedor.Text = ""
        hidtercero.Value = "0"
        txtTercero.Text = ""
        hidsolicita.Value = "0"
        txtSolicita.Text = ""
        txtObservacion.Text = ""
        lblTotalOrden.Text = "VALOR TOTAL ORDEN: $ 0.00"

        ultima_orden()
    End Sub

    Private Sub ultima_orden()
        Dim dtorden As New DataTable

        dtorden = csorde.seleccionar_proxima_orden_compra

        If dtorden.Rows.Count > 0 Then
            txtNroCompra.Text = dtorden.Rows(0)("proximo").ToString
        Else
            txtNroCompra.Text = ""
        End If
    End Sub

    Private Sub gridOrden_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridOrden.RowCommand
        Try
            Dim idorden As Integer
            Dim dtorden As New DataTable
            If e.CommandName = "modificar" Then
                idorden = gridOrden.DataKeys(e.CommandArgument).Values(0)
                hidorden.Value = idorden

                dtorden = csorde.seleccionar_orden_compra_por_id(hidorden.Value)

                If dtorden.Rows.Count > 0 Then
                    txtFechaCompra.Value = dtorden.Rows(0)("fecha_orco").ToString
                    txtNroCompra.Text = dtorden.Rows(0)("numero_orco").ToString
                    hidproveedor.Value = dtorden.Rows(0)("id_terc_prov").ToString
                    txtProveedor.Text = dtorden.Rows(0)("nom_pro").ToString
                    hidtercero.Value = dtorden.Rows(0)("id_terc_soli").ToString
                    txtTercero.Text = dtorden.Rows(0)("nom_sol").ToString
                    hidsolicita.Value = dtorden.Rows(0)("id_terc_auto").ToString
                    txtSolicita.Text = dtorden.Rows(0)("nom_aut").ToString
                    txtObservacion.Text = dtorden.Rows(0)("observacion_orco").ToString

                    LlenarGridDetalle()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden de Compra no válida');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    Dim strRes As String

                    idorden = gridOrden.DataKeys(e.CommandArgument).Values(0)
                    hidorden.Value = idorden

                    strRes = csorde.eliminar_ordenes_compra(hidorden.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden Compra eliminada');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Orden Compra No eliminada');", True)
                    End If
                Else
                    If e.CommandName = "imprimir" Then
                        idorden = gridOrden.DataKeys(e.CommandArgument).Values(0)
                        hidorden.Value = idorden

                        Response.Redirect("FormatoOrdenCompra.aspx?idoc=" & hidorden.Value)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevoEnc_Click(sender As Object, e As EventArgs) Handles btnNuevoEnc.Click
        Limpiar()
        LimpiarDetalle()

        gridDetalle.DataSource = Nothing
        gridDetalle.DataBind()
    End Sub

    Sub LlenarGridDetalle()
        Dim dtcomp As New DataTable

        dtcomp = csorde.seleccionar_detalle_ordenes_compra_por_id_orden(hidorden.Value)

        gridDetalle.DataSource = dtcomp
        gridDetalle.DataBind()

        calcular_total()
    End Sub

    Private Sub btnNuevoDet_Click(sender As Object, e As EventArgs) Handles btnNuevoDet.Click
        LimpiarDetalle()
    End Sub

    Private Sub LimpiarDetalle()
        hidordendetalle.Value = "0"
        cboEquipo.SelectedValue = "0"
        txtCantidad.Text = ""
        txtCosto.Text = ""
        txtIva.Text = "0"
        txtRetencion.Text = "0"
        txtValor.Text = "0"
        txtObservacionDetalle.Text = ""
    End Sub

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        Try
            If hidorden.Value = "0" Or cboEquipo.SelectedValue = "0" Or txtCantidad.Text = "0" Or txtCantidad.Text = "" Or txtCosto.Text = "0" Or txtCosto.Text = "" _
                Or txtIva.Text = "" Or txtRetencion.Text = "" Or txtValor.Text = "0" Or txtValor.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csorde.guardar_detalle_ordenes_compra(hidordendetalle.Value, hidorden.Value, cboEquipo.SelectedValue, txtCantidad.Text, REPLACE(txtCosto.Text, ".", ","), txtIva.Text,
                                                               txtRetencion.Text, txtValor.Text, txtObservacionDetalle.Text, 1)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Orden Compra actualizada con exito');", True)
                    LimpiarDetalle()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Orden Compra no actualizada');", True)
                End If

                LlenarGridDetalle()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub calcular_total()
        Try
            Dim dtOrd As New DataTable

            dtOrd = csorde.seleccionar_valor_total_por_id_orden(hidorden.Value)

            If dtOrd.Rows.Count > 0 Then
                lblTotalOrden.Text = "VALOR TOTAL ORDEN: " & String.Format("{0:c}", dtOrd.Rows(0)("total"))
            Else
                lblTotalOrden.Text = "VALOR TOTAL ORDEN: $ 0.00"
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            Dim idorden As Integer
            Dim dtorden As New DataTable
            If e.CommandName = "modificar" Then
                idorden = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                hidordendetalle.Value = idorden

                dtorden = csorde.seleccionar_detalle_orden_por_id_detalle(hidordendetalle.Value)

                If dtorden.Rows.Count > 0 Then
                    cboEquipo.SelectedValue = dtorden.Rows(0)("id_equi").ToString
                    txtCantidad.Text = dtorden.Rows(0)("cantidad_deoc").ToString
                    txtCosto.Text = dtorden.Rows(0)("costounitario_deoc").ToString
                    txtIva.Text = dtorden.Rows(0)("iva_deoc").ToString
                    txtRetencion.Text = dtorden.Rows(0)("ret_deoc").ToString
                    txtValor.Text = dtorden.Rows(0)("valor_deoc").ToString
                    txtObservacionDetalle.Text = dtorden.Rows(0)("observacion_deoc").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Orden de Compra no válida');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    Dim strRes As String

                    idorden = gridDetalle.DataKeys(e.CommandArgument).Values(0)
                    hidordendetalle.Value = idorden

                    strRes = csorde.eliminar_detalle_ordenes_compra(hidordendetalle.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Orden Compra eliminado');", True)
                        LlenarGridDetalle()
                        LimpiarDetalle()
                        calcular_total()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle Orden Compra No eliminado');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click
        If chkPorcentaje.Checked = True Then
            If txtCantidad.Text = "" Or txtCosto.Text = "" Or txtIva.Text = "" Or txtRetencion.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                txtValor.Text = Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) +
                                (Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) * (Replace(txtIva.Text, ".", ",") / 100)) -
                                (Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) * (Replace(txtRetencion.Text, ".", ",") / 100))

                txtIva.Text = (Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) * (Replace(txtIva.Text, ".", ",") / 100))
                txtRetencion.Text = (Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) * (Replace(txtRetencion.Text, ".", ",") / 100))
            End If
        Else
            If txtCantidad.Text = "" Or txtCosto.Text = "" Or txtIva.Text = "" Or txtRetencion.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                txtValor.Text = Math.Round((Replace(txtCantidad.Text, ".", ",") * Replace(txtCosto.Text, ".", ",")), 2) + Replace(txtIva.Text, ".", ",") - Replace(txtRetencion.Text, ".", ",")
            End If
        End If
    End Sub

    Private Sub chkPorcentaje_CheckedChanged(sender As Object, e As EventArgs) Handles chkPorcentaje.CheckedChanged
        If chkPorcentaje.Checked = True Then
            chkValor.Checked = False
            lblPorIva.Text = "% Iva"
            lblRetencion.Text = "% Retencion"
        Else
            chkValor.Checked = True
            lblPorIva.Text = "$ Iva"
            lblRetencion.Text = "$ Retencion"
        End If

        LimpiarDetalle()
    End Sub

    Private Sub chkValor_CheckedChanged(sender As Object, e As EventArgs) Handles chkValor.CheckedChanged
        If chkValor.Checked = True Then
            chkPorcentaje.Checked = False
            lblPorIva.Text = "$ Iva"
            lblRetencion.Text = "$ Retencion"
        Else
            chkPorcentaje.Checked = True
            lblPorIva.Text = "% Iva"
            lblRetencion.Text = "% Retencion"
        End If

        LimpiarDetalle()
    End Sub
End Class
