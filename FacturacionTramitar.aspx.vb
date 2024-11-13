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

Partial Class FacturacionTramitar
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csvehi As New vehiculos
    Dim csfact As New facturas

    Private Sub FacturacionTramitar_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(31, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            LlenarGrid()
            combos()
        End If
    End Sub

    Sub LlenarGrid()
        Dim csfac As New facturas
        Dim dtfac As New DataTable

        dtfac = csfac.seleccionar_datos_facturas

        gridFacturas.DataSource = dtfac
        gridFacturas.DataBind()
    End Sub

    Sub combos()
        Dim dtpro, dtcli, dtveh, dtcob As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_provedores_tramitar_combo", dtpro, cboProveedor)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_terceros_combo", dtcli, cboCliente)
        csoper.LlenarDropDownList_Sql("nombre", "nombre", "sp_seleccionar_vehiculos_combo", dtveh, cboPlaca)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_coberturas_combo", dtcob, cboCobertura)
    End Sub

    Private Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click
        Try
            If cboProveedor.SelectedValue = "0" Or cboPlaca.SelectedValue = "0" Or cboCobertura.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim dtvlrtra As New DataTable

                dtvlrtra = csvehi.seleccionar_valores(cboPlaca.SelectedValue, cboProveedor.SelectedValue, cboCobertura.SelectedValue)

                If dtvlrtra.Rows.Count > 0 Then
                    txtValor.Text = dtvlrtra.Rows(0)("vlrprov_vatr").ToString
                    txtValProv.Text = dtvlrtra.Rows(0)("valor").ToString
                    txtComision.Text = dtvlrtra.Rows(0)("valor") - dtvlrtra.Rows(0)("vlrprov_vatr")
                Else
                    txtValor.Text = 0
                    txtValProv.Text = 0
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtCotizacion.Text = "" Or txtProforma.Text = "" Or txtFactura.Text = "" Or txtCertificado.Text = "" Or txtFecha.Value = "" Or txtFechaVence.Value = "" Or
               cboProveedor.SelectedValue = "0" Or cboCliente.SelectedValue = "0" Or cboPlaca.SelectedValue = "0" Or cboCobertura.SelectedValue = "0" Or
               txtValor.Text = 0 Or txtValProv.Text = 0 Then

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                If txtRecibo.Text = "" Then
                    txtRecibo.Text = 0
                End If

                strRes = csfact.guardar_facturas(txtCotizacion.Text, txtProforma.Text, txtFactura.Text, txtCertificado.Text, txtFecha.Value, txtFechaVence.Value,
                                                 cboProveedor.SelectedValue, cboCliente.SelectedValue, cboPlaca.SelectedValue, Session("id_usua"), cboCobertura.SelectedValue,
                                                 txtValor.Text, txtValProv.Text, txtRecibo.Text, cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Factura actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Factura no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub Limpiar()
        txtCotizacion.Text = ""
        txtProforma.Text = ""
        txtFactura.Text = ""
        txtCertificado.Text = ""
        txtFecha.Value = ""
        txtFechaVence.Value = ""
        cboProveedor.SelectedValue = "0"
        cboCliente.SelectedValue = "0"
        cboPlaca.SelectedValue = "0"
        cboCobertura.SelectedValue = "0"
        txtValor.Text = ""
        txtValProv.Text = ""
        txtRecibo.Text = ""
        cboActivo.SelectedValue = "1"
    End Sub

    Private Sub gridFacturas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridFacturas.RowCommand
        Try
            Dim idfactura As Integer
            Dim dtfac As New DataTable
            Dim strRes As String

            If e.CommandName = "modificar" Then
                idfactura = gridFacturas.DataKeys(e.CommandArgument).Values(0)
                hidfactura.Value = idfactura

                dtfac = csfact.seleccionar_datos_facturas_por_id(hidfactura.Value)

                If dtfac.Rows.Count > 0 Then
                    txtCotizacion.Text = dtfac.Rows(0)("cotizacion_fact").ToString
                    txtProforma.Text = dtfac.Rows(0)("proforma_fact").ToString
                    txtFactura.Text = dtfac.Rows(0)("numero_fact").ToString
                    txtCertificado.Text = dtfac.Rows(0)("certificado_fact").ToString
                    txtFecha.Value = dtfac.Rows(0)("fecha_fact").ToString
                    txtFechaVence.Value = dtfac.Rows(0)("fechaven_fact").ToString
                    cboProveedor.SelectedValue = dtfac.Rows(0)("id_terc_prov").ToString
                    cboCliente.SelectedValue = dtfac.Rows(0)("id_terc_clie").ToString
                    cboPlaca.SelectedValue = dtfac.Rows(0)("placa_vetr").ToString
                    cboCobertura.SelectedValue = dtfac.Rows(0)("id_cotr").ToString
                    txtValor.Text = dtfac.Rows(0)("valor_fact").ToString
                    txtValProv.Text = dtfac.Rows(0)("vlremp_fact").ToString
                    txtComision.Text = dtfac.Rows(0)("vlremp_fact") - dtfac.Rows(0)("valor_fact")
                    txtRecibo.Text = dtfac.Rows(0)("recibo_fact").ToString
                    cboActivo.SelectedValue = dtfac.Rows(0)("act_fact").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Factura no valida');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idfactura = gridFacturas.DataKeys(e.CommandArgument).Values(0)
                    hidfactura.Value = idfactura

                    strRes = csfact.eliminar_factura(hidfactura.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Factura Eliminada');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Factura No eliminada');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub CalendarFechaInicio_DateChanged(sender As Object, e As EventArgs) Handles CalendarFechaInicio.DateChanged
        txtFechaVence.Value = DateAdd("yyyy", 1, txtFecha.Value).ToString("yyyy-MM-dd")
    End Sub
End Class
