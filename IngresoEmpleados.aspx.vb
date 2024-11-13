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
Partial Class IngresoEmpleados
    Inherits System.Web.UI.Page
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

        strRespuestaPer = csusua.validar_permiso_usuario(2037, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
        End If
    End Sub

    Sub combos()
        Dim dtare, dtcar As New DataTable

        csoper.LlenarDropDownList("sucursal", "sucursal", "sucursales_mostrar", dtSuc, cboAgencia)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_areas_combo", dtare, cboArea)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_cargos_combo", dtcar, cboCargo)
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If cboTipo.SelectedValue = "-1" Or txtDocumento.Text = "" Or txtNombre.Text = "" Or csoper.IsValidEmail(txtCorreo.Text) = False Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String
                If cboTipo.SelectedValue <> "Nit" Then
                    txtDigito.Text = 0
                End If
                strRes = csequipos.guardar_empleados(hidtercero.Value, cboTipo.SelectedValue, txtDocumento.Text, txtDigito.Text, txtNombre.Text, txtDireccion.Text,
                                                    txtTelefono.Text, txtCelular.Text, txtCorreo.Text, cboEmpresa.SelectedValue, cboAgencia.SelectedValue,
                                                    cboArea.SelectedValue, cboCargo.SelectedValue, cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado no actualizado');", True)
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

        dtter = cster.capturar_datos_empleados

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
        txtDireccion.Text = ""
        txtTelefono.Text = ""
        txtCelular.Text = ""
        txtCorreo.Text = ""
        cboEmpresa.SelectedValue = "MOTOTRANSPORTAMOS"
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

                dtter = csequipos.capturar_datos_empleados_por_id(hidtercero.Value)

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
                    txtDireccion.Text = dtter.Rows(0)("direccion").ToString
                    txtTelefono.Text = dtter.Rows(0)("telefono").ToString
                    txtCelular.Text = dtter.Rows(0)("celular").ToString
                    txtCorreo.Text = dtter.Rows(0)("correo").ToString
                    cboEmpresa.SelectedValue = dtter.Rows(0)("empresa").ToString
                    cboAgencia.SelectedValue = dtter.Rows(0)("agencia").ToString
                    cboArea.SelectedValue = dtter.Rows(0)("area").ToString
                    cboCargo.SelectedValue = dtter.Rows(0)("cargo").ToString
                    cboActivo.SelectedValue = dtter.Rows(0)("activo_empl").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idtercero = gridTerceros.DataKeys(e.CommandArgument).Values(0)
                    hidtercero.Value = idtercero

                    strRes = csequipos.eliminar_empleados(hidtercero.Value)

                    If strRes = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado eliminado');", True)
                        LlenarGrid()
                        Limpiar()
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado No eliminado');", True)
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
        Try
            Dim dtemp As New DataTable

            dtemp = csequipos.capturar_datos_terceros_por_documento(txtDocumento.Text)

            If dtemp.Rows.Count > 0 Then
                cboTipo.SelectedValue = dtemp.Rows(0)("tipodoc").ToString
                txtDocumento.Text = dtemp.Rows(0)("documento").ToString
                txtNombre.Text = dtemp.Rows(0)("nombre").ToString
                txtDireccion.Text = dtemp.Rows(0)("direccion").ToString
                txtTelefono.Text = dtemp.Rows(0)("telefono").ToString
                txtCelular.Text = dtemp.Rows(0)("celular").ToString
                txtCorreo.Text = dtemp.Rows(0)("correo").ToString
                cboEmpresa.SelectedValue = dtemp.Rows(0)("empresa").ToString
                cboAgencia.SelectedValue = dtemp.Rows(0)("agencia").ToString
                cboArea.SelectedValue = dtemp.Rows(0)("area").ToString
                cboCargo.SelectedValue = dtemp.Rows(0)("cargo").ToString
                cboActivo.SelectedValue = dtemp.Rows(0)("activo_empl").ToString
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Empleado Nuevo.');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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
End Class
