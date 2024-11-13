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
Partial Class Generadores_Systram
    Inherits System.Web.UI.Page
    Dim csequipos As New equipos
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim dtSuc As New DataTable
    Dim csInf As New Informes
    Private Sub Generadores_Systram_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2045, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
            LlenarGrid()
        End If
    End Sub

    Sub combos()
        Dim dtare, dtcar, dtzon As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_zona_municipio_dpto_combo", dtzon, cboZonas)
    End Sub

    Sub LlenarGrid()
        Dim cster As New equipos
        Dim dtter As New DataTable

        Dim intTipoEmp As Integer
        If Session("codEmpr") = 1 Then
            intTipoEmp = 0
        Else
            If Session("codEmpr") = 3 Then
                intTipoEmp = 1
            End If
        End If

        dtter = cster.seleccionar_generadores_sytram_completo(intTipoEmp)

        gridTerceros.DataSource = dtter
        gridTerceros.DataBind()
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

                Dim intTipoEmp As Integer
                If Session("codEmpr") = 1 Then
                    intTipoEmp = 0
                Else
                    If Session("codEmpr") = 3 Then
                        intTipoEmp = 1
                    End If
                End If

                strRes = csequipos.guardar_generadores_systram(hidtercero.Value, cboTipo.SelectedValue, txtDocumento.Text, txtDigito.Text, txtNombre.Text, txtNombre2.Text, txtApellido1.Text,
                                                    txtApellido2.Text, txtDireccion.Text, txtTelefono.Text, txtCelular.Text, txtCorreo.Text, cboZonas.SelectedValue, intTipoEmp, cboActivo.SelectedValue)

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Generador actualizado con exito');", True)
                    Limpiar()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Generador no actualizado');", True)
                End If

                LlenarGrid()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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

                dtter = csequipos.seleccionar_generadores_systram_por_id(hidtercero.Value)

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
                    cboActivo.SelectedValue = dtter.Rows(0)("activo_GENE").ToString
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Generador no valido');", True)
                End If
            Else
                If e.CommandName = "eliminar" Then
                    idtercero = gridTerceros.DataKeys(e.CommandArgument).Values(0)
                    hidtercero.Value = idtercero

                    strRes = csequipos.eliminar_generadores_systram(hidtercero.Value)

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

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            strSQL = "SELECT CASE WHEN terceros.tipo_documentos_idtipo_documentos = 1 THEN 'Cedula de Ciudadania' ELSE 'Nit' END AS tipo_documento, "
            strSQL += "terceros.documento, "
            strSQL += "terceros.nombre1, "
            strSQL += "terceros.nombre2, "
            strSQL += "terceros.apellido1, "
            strSQL += "terceros.apellido2, "
            strSQL += "terceros.direccion, "
            strSQL += "COALESCE((SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros = terceros.idterceros "
            strSQL += "AND telefonos.tipo_telefonos_idtipo_telefonos = 1 ORDER BY telefonos.idtelefonos DESC LIMIT 1), 0) AS telefono, "
            strSQL += "COALESCE((SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros = terceros.idterceros "
            strSQL += "AND telefonos.tipo_telefonos_idtipo_telefonos = 2 ORDER BY telefonos.idtelefonos DESC LIMIT 1), 0) AS celular, "
            strSQL += "generadores.correo "
            strSQL += "FROM generadores "
            strSQL += "LEFT JOIN terceros ON generadores.terceros_idterceros = terceros.idterceros "
            strSQL += "WHERE generadores.idel = 0 "
            strSQL += "ORDER BY terceros.idterceros DESC "

            dtter = csInf.ejecutar_query_bd(strSQL)

            If dtter.Rows.Count > 0 Then
                Dim strTipoDoc, strDocumento, strNombre1, strNombre2, strApellido1, strApellido2, strDireccion, strTelefono, strCelular, strCorreo, strRes As String
                Dim intDigVer As Integer
                For i As Integer = 0 To dtter.Rows.Count - 1
                    strTipoDoc = dtter.Rows(i)("tipo_documento").ToString
                    strDocumento = dtter.Rows(i)("documento").ToString
                    strNombre1 = dtter.Rows(i)("nombre1").ToString
                    strNombre2 = dtter.Rows(i)("nombre2").ToString
                    strApellido1 = dtter.Rows(i)("apellido1").ToString
                    strApellido2 = dtter.Rows(i)("apellido2").ToString
                    strDireccion = dtter.Rows(i)("direccion").ToString
                    strTelefono = dtter.Rows(i)("telefono").ToString
                    strCelular = dtter.Rows(i)("celular").ToString
                    strCorreo = dtter.Rows(i)("correo").ToString

                    If strTipoDoc = "Nit" Then
                        intDigVer = csoper.calcular_digito(strDocumento)
                    Else
                        intDigVer = 0
                    End If

                    Dim intTipoEmp As Integer
                    If Session("codEmpr") = 1 Then
                        intTipoEmp = 0
                    Else
                        If Session("codEmpr") = 3 Then
                            intTipoEmp = 1
                        End If
                    End If

                    strRes = csequipos.guardar_generadores_systram(0, strTipoDoc, strDocumento, intDigVer, strNombre1, strNombre2,
                                                                   strApellido1, strApellido2, strDireccion, strTelefono, strCelular, strCorreo, 1, intTipoEmp, 1)
                Next

                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con éxito...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
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

    Sub LlenarGridBusqueda(ByVal ind As Integer, ByVal par As String)
        Dim cster As New equipos
        Dim dtter As New DataTable

        Dim intTipoEmp As Integer
        If Session("codEmpr") = 1 Then
            intTipoEmp = 0
        Else
            If Session("codEmpr") = 3 Then
                intTipoEmp = 1
            End If
        End If

        dtter = cster.capturar_datos_generadores_systram_busqueda(ind, par, intTipoEmp)

        gridTerceros.DataSource = dtter
        gridTerceros.DataBind()
    End Sub

    Private Sub btnEnviarCorreo_Click(sender As Object, e As EventArgs) Handles btnEnviarCorreo.Click
        Try
            If hidtercero.Value = "0" Or hidtercero.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe cargar el generador...');", True)
            Else
                Dim strSQL As String
                Dim dtTer As DataTable

                strSQL = "Select vc.generador_id, vc.venta_id, vc.generador_nombre As Generador, "
                strSQL += "vc.generador_documento as Documento, vc.venta_plazo As Plazo, vc.generador_telefonos As Telefonos, "
                strSQL += "vc.generador_direccion As Direccion, zo.zona As zona, COALESCE(vc.asesor_id, 0) As idas, "
                strSQL += "COALESCE(vc.asesor_nombre, COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += "From generadores_asesores "
                strSQL += "Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += "Left Join terceros ON usuarios.idterceros = terceros.idterceros "
                strSQL += "WHERE terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 "
                strSQL += "And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')) as Asesor, "
                strSQL += "vc.sucursal_origina_descripcion As Sucursal, vc.venta_numero As NroFac, "
                strSQL += "DATE_FORMAT(vc.venta_fecha,'%Y-%m-%d') as Fecha, DATE_FORMAT(vc.venta_vence,'%Y-%m-%d') as Vence, "
                strSQL += "vc.venta_total As Total, vc.venta_abonos As Abono, (vc.venta_total - vc.venta_abonos) As Saldo, "
                strSQL += "(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) as DiasVence, "
                strSQL += "(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo as DiasFac, "
                strSQL += "COALESCE(gena.usuarios_idusuarios, 1) As us_asesor, "
                strSQL += "COALESCE((SELECT CONCAT(te.nombre1, ' ', te.nombre2, ' ', te.apellido1, ' ', te.apellido2) "
                strSQL += "From generadores_asesores "
                strSQL += "Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += "Left Join terceros te ON usuarios.idterceros = te.idterceros "
                strSQL += "WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
                strSQL += "ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 'TERCERO ESPECIAL') AS ase_ofi "
                strSQL += "From ventas_consolidado vc "
                strSQL += "INNER Join terceros ter ON (vc.generador_terceroid=ter.idterceros) "
                strSQL += "INNER Join zonas zo on(ter.zonas_idzonas=zo.idzonas) "
                strSQL += "INNER Join generadores gen ON(vc.generador_id=gen.idgeneradores And gen.idel=0) "
                strSQL += "Left Join generadores_asesores gena ON(vc.asesor_id=gena.idgeneradores_asesores) "
                strSQL += "Left Join usuarios us ON(us.idusuarios=gena.usuarios_idusuarios) "
                strSQL += "WHERE vc.venta_id = (SELECT vd.ventas_idventas FROM ventas_detalles vd "
                strSQL += "WHERE vc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1) "
                strSQL += "And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd "
                strSQL += "WHERE vrd.ventas_idventas = vc.venta_id)) Or (SELECT SUM(vrd.valor) "
                strSQL += "From ventas_recaudos_detalle vrd Where vrd.ventas_idventas = vc.venta_id)Is NULL) "
                strSQL += "And vc.venta_abonos < vc.venta_total And (vc.venta_total - vc.venta_abonos) > 10 "
                strSQL += "And vc.venta_vence<=(SELECT( ADDDATE('" & DateTime.Now.ToString("yyyy-MM-dd") & "',INTERVAL vc.venta_plazo DAY))) "
                strSQL += "And vc.generador_documento = '" & txtDocumento.Text & "' "
                strSQL += "ORDER BY ase_ofi, Generador, (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo DESC "

                dtTer = csInf.ejecutar_query_bd(strSQL)

                If dtTer.Rows.Count > 0 Then
                    EnviarCorreo()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cliente sin cartera. Imposible enviar...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub EnviarCorreo()
        Response.Redirect("FormatoCarteraGenerador.aspx?nit=" & txtDocumento.Text & "&cor=" & txtCorreo.Text & "")
    End Sub

    Private Sub btPrueba_Click(sender As Object, e As EventArgs) Handles btPrueba.Click
        Response.Redirect("FormatoPendientesReunion.aspx")
    End Sub
End Class
