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
Partial Class viaticosdetalle
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csviat As New cviaticos
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

            strRespuestaPer = csusua.validar_permiso_usuario(4074, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If

            If Me.IsPostBack = False Then
                If Request.QueryString("est") IsNot Nothing Then
                    If Request.QueryString("est") = "exi" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Actualizado Exitosamente...');", True)
                    End If
                End If

                If Request.QueryString("id") <> Nothing Then
                    hidviatico.Value = Request.QueryString("id")
                End If

                If hidviatico.Value = 0 Then
                    Dim dtvia As New DataTable

                    dtvia = csviat.seleccionar_numero_proximo_viatico

                    If dtvia.Rows.Count > 0 Then
                        txtNumero.Text = dtvia.Rows(0)("proximo").ToString
                    Else
                        txtNumero.Text = 1
                    End If
                Else
                    Dim dtcad As New DataTable

                    dtcad = csviat.seleccionar_viaticos(3, hidviatico.Value)

                    If dtcad.Rows.Count > 0 Then
                        lblFacturaEncabezado.Text = "Encabezado Viatico " & dtcad.Rows(0)("estado_viat").ToString
                        txtNumero.Text = dtcad.Rows(0)("numero_viat").ToString
                        txtFecha.Value = dtcad.Rows(0)("fecha_viat").ToString
                        hidproveedor.Value = dtcad.Rows(0)("id_terc").ToString
                        txtProveedor.Text = dtcad.Rows(0)("nombre_terc").ToString
                        txtObservacion.Text = dtcad.Rows(0)("observacion_viat").ToString
                        txtEntradas.Text = dtcad.Rows(0)("valor_entrada")
                        txtSalidas.Text = dtcad.Rows(0)("valor_salida")
                        txtSaldo.Text = dtcad.Rows(0)("valor_entrada") - dtcad.Rows(0)("valor_salida")

                        Llenar_Grid_Detalle()
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvarEncabezado_Click(sender As Object, e As EventArgs) Handles btnSalvarEncabezado.Click
        Try
            If txtNumero.Text = "" Or txtFecha.Value = "" Or hidestado.Value = "EMITIDO" Or txtFecha.Value = "" Or hidproveedor.Value = "" Or hidproveedor.Value = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar o el viatico ya fue emitido...');", True)
            Else
                Dim dtcae As New DataTable

                dtcae = csviat.guardar_viatico_encabezado(hidviatico.Value, CInt(txtNumero.Text), txtFecha.Value, hidproveedor.Value, txtObservacion.Text, Session("id_usua"))

                If dtcae.Rows.Count > 0 Then
                    Response.Redirect("viaticosdetalle.aspx?id=" & dtcae.Rows(0)("ide_viat") & "&est=exi")
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

        dtcade = csviat.seleccionar_viaticos_detalle(hidviatico.Value)

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

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        If hidestado.Value = "EMITIDO" Or hidtercero.Value = "" Or hidtercero.Value = "0" Or cboTipo.SelectedValue = "0" Or txtCantidad.Text = "" Or txtValor.Text = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar o el viatico ya fue emitido...');", True)
        Else
            Dim dtcae As New DataTable

            dtcae = csviat.guardar_viatico_detalle(hiddetalle.Value, hidviatico.Value, hidtercero.Value, cboTipo.SelectedValue, txtReferencia.Text, txtCantidad.Text, txtValor.Text, txtDescripcion.Text)

            If dtcae.Rows.Count > 0 Then
                If dtcae.Rows(0)("ide_viat").ToString = "" Then
                    Llenar_Grid_Detalle()
                    calcular_totales()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
            End If
        End If
    End Sub

    Private Sub gridDetalle_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDetalle.RowCommand
        Try
            Dim iddet As Integer
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "eliminar" Then
                If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Viatico EMITIDO. Imposible eliminar detalle...');", True)
                Else
                    iddet = gridDetalle.DataKeys(e.CommandArgument).Values(0)

                    strRespuesta = csviat.eliminar_viatico_detalle(iddet)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle fue eliminado exitosamente...');", True)
                        Llenar_Grid_Detalle()
                        calcular_totales()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle no fue eliminado exitosamente...');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub calcular_totales()
        Dim dtviat As New DataTable

        dtviat = csviat.seleccionar_viaticos("3", hidviatico.Value)

        If dtviat.Rows.Count > 0 Then
            txtEntradas.Text = dtviat.Rows(0)("valor_entrada")
            txtSalidas.Text = dtviat.Rows(0)("valor_salida")
            txtSaldo.Text = dtviat.Rows(0)("valor_entrada") - dtviat.Rows(0)("valor_salida")
        Else
            txtEntradas.Text = 0
            txtSalidas.Text = 0
            txtSaldo.Text = 0
        End If
    End Sub

    Private Sub imgPdf_Click(sender As Object, e As ImageClickEventArgs) Handles imgPdf.Click
        Try
            If lblFacturaEncabezado.Text.Contains("SIN EMITIR") Then
                Dim strRes As String

                strRes = csviat.emitir_viatico(hidviatico.Value)

                If strRes = "" Then
                    Response.Redirect("viaticosdetalle.aspx?id=" & hidviatico.Value & "&est=exi")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            Else
                Response.Redirect("Formato_Viaticos.aspx?id=" & hidviatico.Value)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
