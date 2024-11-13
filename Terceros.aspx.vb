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
Partial Class Terceros
    Inherits System.Web.UI.Page
    Implements DPFP.Capture.EventHandler
    Dim csequipos As New equipos
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim dtSuc As New DataTable
    Private Captura As DPFP.Capture.Capture

    Private Sub Terceros_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(27, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        'If Session("documento") <> "98702336" And Session("documento") <> "71760116" And Session("documento") <> "1066750169" And Session("documento") <> "39177276" Then
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
        '    Response.Redirect("Default.aspx")
        'End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
            'Inicializar()
            'iniciarCaptura()
        End If
    End Sub

    Sub combos()
        Dim dtare, dtcar, dtzon As New DataTable

        csoper.LlenarDropDownList("sucursal", "sucursal", "sucursales_mostrar", dtSuc, cboAgencia)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_areas_combo", dtare, cboArea)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_cargos_combo", dtcar, cboCargo)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_zona_municipio_dpto_combo", dtzon, cboZonas)
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboTipo.SelectedValue = "-1" Or txtDocumento.Text = "" Or txtNombre.Text = "" _
                Or txtDireccion.Text = "" Or txtTelefono.Text = "" Or csoper.IsValidEmail(txtCorreo.Text) = False Or cboZonas.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                If cboTipo.SelectedValue <> "Nit" Then
                    If txtApellido1.Text = "" Or txtApellido2.Text = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
                        Exit Sub
                    End If
                End If
                Dim strRes As String
                If cboTipo.SelectedValue <> "Nit" Then
                    txtDigito.Text = 0
                End If
                strRes = csequipos.guardar_terceros(hidtercero.Value, cboTipo.SelectedValue, txtDocumento.Text, txtDigito.Text, txtNombre.Text, txtNombre2.Text, txtApellido1.Text,
                                                    txtApellido2.Text, txtDireccion.Text, txtTelefono.Text, txtCelular.Text, txtCorreo.Text, cboZonas.SelectedValue,
                                                    cboEmpresa.SelectedValue, cboAgencia.SelectedValue, cboArea.SelectedValue, cboCargo.SelectedValue, cboActivo.SelectedValue, cboTipoCampo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub LlenarGrid()
        Dim cster As New equipos
        Dim dtter As New DataTable

        dtter = cster.capturar_datos_terceros

        gridTerceros.DataSource = dtter
        gridTerceros.DataBind()
    End Sub

    Sub LlenarGridBusqueda(ByVal ind As Integer, ByVal par As String)
        Dim cster As New equipos
        Dim dtter As New DataTable

        dtter = cster.capturar_datos_terceros_busqueda(ind, par)

        gridTerceros.DataSource = dtter
        gridTerceros.DataBind()
    End Sub

    Sub Limpiar()
        hidtercero.Value = 0
        cboTipo.SelectedValue = -1
        txtDocumento.Text = ""
        txtDigito.Text = ""
        lbldocumento.Text = "Documento *"
        txtDigito.Visible = False
        txtNombre.Text = ""
        txtNombre2.Text = ""
        txtApellido1.Text = ""
        txtApellido2.Text = ""
        txtDireccion.Text = ""
        txtTelefono.Text = ""
        txtCelular.Text = ""
        txtCorreo.Text = ""
        cboZonas.SelectedValue = "0"
        cboEmpresa.SelectedValue = 1
        cboAgencia.SelectedValue = "0"
        cboArea.SelectedValue = "0"
        cboCargo.SelectedValue = "0"
        cboActivo.SelectedValue = "1"
    End Sub

    Private Sub gridTerceros_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridTerceros.RowCommand
        Try
            Dim idtercero As Integer
            Dim dtter As New DataTable
            Dim strRes As String
            If e.CommandName = "modificar" Then
                idtercero = gridTerceros.DataKeys(e.CommandArgument).Values(0)
                hidtercero.Value = idtercero

                dtter = csequipos.capturar_datos_terceros_por_id(hidtercero.Value)

                If dtter.Rows.Count > 0 Then
                    cboTipo.SelectedValue = dtter.Rows(0)("tipodoc").ToString
                    txtDocumento.Text = dtter.Rows(0)("documento").ToString
                    If dtter.Rows(0)("tipodoc").ToString = "Nit" Then
                        lbldocumento.Text = "Nit *"
                        txtDigito.Visible = True
                        txtDigito.Text = csoper.calcular_digito(txtDocumento.Text)
                    Else
                        lbldocumento.Text = "Documento *"
                        txtDigito.Visible = False
                    End If
                    txtNombre.Text = dtter.Rows(0)("nombre").ToString
                    txtNombre2.Text = dtter.Rows(0)("nombre2").ToString
                    txtApellido1.Text = dtter.Rows(0)("apellido1").ToString
                    txtApellido2.Text = dtter.Rows(0)("apellido2").ToString
                    txtDireccion.Text = dtter.Rows(0)("direccion").ToString
                    txtTelefono.Text = dtter.Rows(0)("telefono").ToString
                    txtCelular.Text = dtter.Rows(0)("celular").ToString
                    txtCorreo.Text = dtter.Rows(0)("correo").ToString
                    cboZonas.SelectedValue = dtter.Rows(0)("zona").ToString
                    cboEmpresa.SelectedValue = dtter.Rows(0)("empresa").ToString
                    cboAgencia.SelectedValue = dtter.Rows(0)("agencia").ToString
                    cboArea.SelectedValue = dtter.Rows(0)("area").ToString
                    cboCargo.SelectedValue = dtter.Rows(0)("cargo").ToString
                    cboActivo.SelectedValue = dtter.Rows(0)("activo_TERC").ToString
                    cboTipoCampo.SelectedValue = dtter.Rows(0)("tipo").ToString
                Else


                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idtercero = gridTerceros.DataKeys(e.CommandArgument).Values(0)
                    hidtercero.Value = idtercero

                    strRes = csequipos.eliminar_terceros(hidtercero.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero eliminado');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Tercero No eliminado');", True)
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboTipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipo.TextChanged
        If cboTipo.SelectedValue = "Nit" Then
            lbldocumento.Text = "Nit *"
            txtDigito.Visible = True
        Else
            lbldocumento.Text = "Documento *"
            txtDigito.Visible = False
        End If
    End Sub

    Private Sub txtDocumento_TextChanged(sender As Object, e As EventArgs) Handles txtDocumento.TextChanged
        If cboTipo.SelectedValue = "Nit" Then
            txtDigito.Text = csoper.calcular_digito(txtDocumento.Text)
        End If
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        LlenarGrid()
    End Sub

    Protected Overridable Sub Inicializar()
        Try
            Captura = New Capture
            If Not Captura Is Nothing Then
                Captura.EventHandler = Me
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No se pudo inicializar la captura...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No se pudo inicializar la captura..." & ex.Message & "');", True)
        End Try
    End Sub

    Protected Sub iniciarCaptura()
        'If Not Captura Is Nothing Then
        '    Try
        '        Captura.StartCapture()
        '    Catch ex As Exception
        '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No se pudo iniciar la captura..." & ex.Message & "');", True)
        '    End Try
        'End If
    End Sub

    Protected Sub pararCaptura()
        If Not Captura Is Nothing Then
            Try
                Captura.StopCapture()
            Catch ex As Exception
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No se pudo parar la captura...');", True)
            End Try
        End If
    End Sub

    Public Sub OnComplete(Capture As Object, ReaderSerialNumber As String, Sample As Sample) Implements DPFP.Capture.EventHandler.OnComplete

    End Sub

    Public Sub OnFingerGone(Capture As Object, ReaderSerialNumber As String) Implements DPFP.Capture.EventHandler.OnFingerGone

    End Sub

    Public Sub OnFingerTouch(Capture As Object, ReaderSerialNumber As String) Implements DPFP.Capture.EventHandler.OnFingerTouch
        'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Toco huellero...');", True)
    End Sub

    Public Sub OnReaderConnect(Capture As Object, ReaderSerialNumber As String) Implements DPFP.Capture.EventHandler.OnReaderConnect

    End Sub

    Public Sub OnReaderDisconnect(Capture As Object, ReaderSerialNumber As String) Implements DPFP.Capture.EventHandler.OnReaderDisconnect

    End Sub

    Public Sub OnSampleQuality(Capture As Object, ReaderSerialNumber As String, CaptureFeedback As CaptureFeedback) Implements DPFP.Capture.EventHandler.OnSampleQuality

    End Sub

    Protected Function ConvertirSampleMapaDeBits(ByVal Sample As DPFP.Sample) As Bitmap
        'Dim Convertidor As New DPFP.Capture.SampleConversion
        'Dim mapaBits As Bitmap = Nothing
        'Return mapaBits
    End Function

    Private Sub ponerImagen(ByVal bmp)
        'imgHuella.
    End Sub

    Private Sub imgBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles imgBuscar.Click
        Try
            If txtBuscar.Text <> "" Then
                If rdDocumento.Checked = True Then
                    LlenarGridBusqueda(1, txtBuscar.Text)
                End If

                If rdNombre.Checked = True Then
                    LlenarGridBusqueda(2, txtBuscar.Text)
                End If
            Else
                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub rdDocumento_CheckedChanged(sender As Object, e As EventArgs) Handles rdDocumento.CheckedChanged
        If rdDocumento.Checked = True Then
            rdNombre.Checked = False
        Else
            rdNombre.Checked = True
        End If
    End Sub

    Private Sub rdNombre_CheckedChanged(sender As Object, e As EventArgs) Handles rdNombre.CheckedChanged
        If rdNombre.Checked = True Then
            rdDocumento.Checked = False
        Else
            rdDocumento.Checked = True
        End If
    End Sub

    '<System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    'Public Function GetCompletionList() As List(Of String)
    '    Dim cster As New equipos
    '    Dim dtzon As New DataTable

    '    Dim countryNames As List(Of String) = New List(Of String)()
    '    dtzon = cster.seleccionar_zonas_automcompletar(txtZonas.Text)

    '    If dtzon.Rows.Count Then
    '        For i As Integer = 0 To dtzon.Rows.Count - 1
    '            countryNames.Add(dtzon.Rows(i)("Zona").ToString)
    '        Next
    '    End If

    '    Return countryNames
    'End Function

    Protected Sub cboTipoCampo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTipoCampo.SelectedIndexChanged
        If cboTipoCampo.SelectedValue = "Empleado" Then
            cboEmpresa.Enabled = True
            cboAgencia.Enabled = True
            cboArea.Enabled = True
            cboCargo.Enabled = True
        Else
            cboEmpresa.Enabled = False
            cboAgencia.Enabled = False
            cboArea.Enabled = False
            cboCargo.Enabled = False
        End If

    End Sub


End Class
