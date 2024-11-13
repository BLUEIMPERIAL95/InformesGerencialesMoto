Imports System.Data
Partial Class CadEnvioDetalle
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscade As New cad
    Dim csinformes As New Informes
    Dim dtcade As New DataTable
    Dim cscuco As New Cuentacob

    Private Sub CadEnvioDetalle_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(2055, Session("id_usua"))

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
                End If

                If hidcad.Value = 0 Then
                    Dim dtcad As New DataTable

                    dtcad = cscade.seleccionar_numero_proximo_cad_envio

                    If dtcad.Rows.Count > 0 Then
                        txtCadEnvio.Text = dtcad.Rows(0)("proximo").ToString
                    Else
                        txtCadEnvio.Text = 1
                    End If
                Else
                    Dim dtcad As New DataTable

                    dtcad = cscade.seleccionar_cad_envio_listado(2, hidcad.Value)

                    If dtcad.Rows.Count > 0 Then
                        lblFacturaEncabezado.Text = "Encabezado Envío Cad " & dtcad.Rows(0)("estado_caen").ToString
                        txtCadEnvio.Text = dtcad.Rows(0)("numero_caen").ToString
                        txtFecha.Value = dtcad.Rows(0)("fecha_caen").ToString
                        cboEmpresa.SelectedValue = dtcad.Rows(0)("id_emor").ToString
                        Dim dtage As New DataTable
                        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
                        cboAgencia.SelectedValue = dtcad.Rows(0)("id_agcc").ToString
                        Dim dtAgeIni As New DataTable

                        dtAgeIni = cscuco.seleccionar_agencias_cuenta_cobro_por_id(cboAgencia.SelectedValue)

                        If dtAgeIni.Rows.Count > 0 Then
                            hidsucursal.Value = dtAgeIni.Rows(0)("id_sucursal").ToString
                        Else
                            hidsucursal.Value = "0"
                        End If
                        cboDocumento.SelectedValue = dtcad.Rows(0)("tipodoc_caen").ToString
                        txtObservacion.Text = dtcad.Rows(0)("observacion_caen").ToString

                        Llenar_Grid_Detalle()
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtprov, dtaut, dtempr, dtret As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo_cad", dtempr, cboEmpresa)
    End Sub

    Private Sub cboEmpresa_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresa.TextChanged
        Dim dtage As New DataTable

        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo", dtage, cboAgencia, cboEmpresa.SelectedValue)
    End Sub

    Private Sub Llenar_Grid_Detalle()
        dtcade = cscade.seleccionar_cad_envio_detalle(hidcad.Value)

        If dtcade.Rows.Count > 0 Then
            gridDetalle.DataSource = dtcade
            gridDetalle.DataBind()
        Else
            gridDetalle.DataSource = dtcade
            gridDetalle.DataBind()
        End If
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtCadEnvio.Text = "" Or txtFecha.Value = "" Or cboEmpresa.SelectedValue = "0" Or cboAgencia.SelectedValue = "0" Or cboDocumento.SelectedValue = "0" Or hidestado.Value = "Emitido" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar o el envío ya fue emitido...');", True)
            Else
                Dim dtcae As New DataTable

                dtcae = cscade.guardar_cad_envio_encabezado(hidcad.Value, CInt(txtCadEnvio.Text), txtFecha.Value, CInt(cboEmpresa.SelectedValue), CInt(cboAgencia.SelectedValue),
                                                            cboDocumento.SelectedValue, txtObservacion.Text, Session("id_usua"))

                If dtcae.Rows.Count > 0 Then
                    Response.Redirect("CadEnvioDetalle.aspx?id=" & dtcae.Rows(0)("ide_caen") & "&est=exi")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            Dim strSQL As String
            strSQL = ""
            If lblFacturaEncabezado.Text.Contains("EMITIDO") Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Envío emitido. Imposible generar mas documentos...');", True)
            Else
                If cboEmpresa.SelectedValue = "3" Or cboEmpresa.SelectedValue = "4" Or cboEmpresa.SelectedValue = "8" Then
                    If cboDocumento.SelectedValue = "Egresos" Then
                        'strSQL += "Select sistema_sucursales.sucursal, "
                        'strSQL += "movimientos_contables.numero, "
                        'strSQL += "tipo_comprobantes.descripcion, "
                        'strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre,  "
                        'strSQL += "movimientos_contables.fecha, "
                        'strSQL += "terceros.documento, "
                        'strSQL += "Month(movimientos_contables.fecha) As Mes,  "
                        'strSQL += "movimientos_contables_detalles.tipo, "
                        'strSQL += "movimientos_contables_detalles.valor, "
                        'strSQL += "movimientos_contables_detalles.referencia, "
                        'strSQL += "movimientos_contables.egreso_referencia, "
                        'strSQL += "movimientos_contables.nombre_beneficiario "
                        'strSQL += "From terceros  "
                        'strSQL += "INNER Join(sistema_sucursales "
                        'strSQL += "INNER Join(tipo_comprobantes "
                        'strSQL += "INNER Join(movimientos_contables "
                        'strSQL += "INNER Join movimientos_contables_detalles "
                        'strSQL += "On movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad) "
                        'strSQL += "On tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo) "
                        'strSQL += "On sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales) "
                        'strSQL += "On terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        'strSQL += "WHERE tipo_comprobantes.descripcion Like '%EGRESO%' "
                        'strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        'strSQL += " And movimientos_contables_detalles.tipo = 2 "

                        strSQL += "Select DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "sistema_sucursales.sucursal, "
                        strSQL += "movimientos_contables.numero, "
                        strSQL += "tipo_comprobantes.descripcion, "
                        strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre, "
                        strSQL += "movimientos_contables.fecha, "
                        strSQL += "terceros.documento, "
                        strSQL += "movimientos_contables_detalles.valor "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%EGRESO%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY movimientos_contables.numero "
                    End If

                    If cboDocumento.SelectedValue = "Facturas" Then
                        'strSQL += "Select sistema_sucursales.sucursal, "
                        'strSQL += "movimientos_contables.numero, "
                        'strSQL += "tipo_comprobantes.descripcion, "
                        'strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre,  "
                        'strSQL += "movimientos_contables.fecha, "
                        'strSQL += "terceros.documento, "
                        'strSQL += "Month(movimientos_contables.fecha) As Mes,  "
                        'strSQL += "movimientos_contables_detalles.tipo, "
                        'strSQL += "movimientos_contables_detalles.valor, "
                        'strSQL += "movimientos_contables_detalles.referencia, "
                        'strSQL += "movimientos_contables.egreso_referencia, "
                        'strSQL += "movimientos_contables.nombre_beneficiario "
                        'strSQL += "From terceros  "
                        'strSQL += "INNER Join(sistema_sucursales "
                        'strSQL += "INNER Join(tipo_comprobantes "
                        'strSQL += "INNER Join(movimientos_contables "
                        'strSQL += "INNER Join movimientos_contables_detalles "
                        'strSQL += "On movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad) "
                        'strSQL += "On tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo) "
                        'strSQL += "On sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales) "
                        'strSQL += "On terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        'strSQL += "WHERE tipo_comprobantes.descripcion Like '%FACTURA%' "
                        'strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        'strSQL += " And movimientos_contables_detalles.tipo = 2 "

                        strSQL += "Select DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "sistema_sucursales.sucursal, "
                        strSQL += "movimientos_contables.numero, "
                        strSQL += "tipo_comprobantes.descripcion, "
                        strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre, "
                        strSQL += "movimientos_contables.fecha, "
                        strSQL += "terceros.documento, "
                        strSQL += "0 AS valor "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%FACTURACION ELECTRONICA%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 1 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY movimientos_contables.numero "
                    End If

                    If cboDocumento.SelectedValue = "Notas" Then
                        'strSQL += "Select sistema_sucursales.sucursal, "
                        'strSQL += "movimientos_contables.numero, "
                        'strSQL += "tipo_comprobantes.descripcion, "
                        'strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre,  "
                        'strSQL += "movimientos_contables.fecha, "
                        'strSQL += "terceros.documento, "
                        'strSQL += "Month(movimientos_contables.fecha) As Mes,  "
                        'strSQL += "movimientos_contables_detalles.tipo, "
                        'strSQL += "movimientos_contables_detalles.valor, "
                        'strSQL += "movimientos_contables_detalles.referencia, "
                        'strSQL += "movimientos_contables.egreso_referencia, "
                        'strSQL += "movimientos_contables.nombre_beneficiario "
                        'strSQL += "From terceros  "
                        'strSQL += "INNER Join(sistema_sucursales "
                        'strSQL += "INNER Join(tipo_comprobantes "
                        'strSQL += "INNER Join(movimientos_contables "
                        'strSQL += "INNER Join movimientos_contables_detalles "
                        'strSQL += "On movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad) "
                        'strSQL += "On tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo) "
                        'strSQL += "On sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales) "
                        'strSQL += "On terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        'strSQL += "WHERE tipo_comprobantes.descripcion Like '%NOTAS%' "
                        'strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        'strSQL += " And movimientos_contables_detalles.tipo = 2 "

                        strSQL += "Select DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "sistema_sucursales.sucursal, "
                        strSQL += "movimientos_contables.numero, "
                        strSQL += "tipo_comprobantes.descripcion, "
                        strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre, "
                        strSQL += "movimientos_contables.fecha, "
                        strSQL += "terceros.documento, "
                        strSQL += "0 AS valor "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%NOTAS%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY movimientos_contables.numero "
                    End If

                    If cboDocumento.SelectedValue = "Recibos" Then
                        'strSQL += "Select sistema_sucursales.sucursal, "
                        'strSQL += "movimientos_contables.numero, "
                        'strSQL += "tipo_comprobantes.descripcion, "
                        'strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre,  "
                        'strSQL += "movimientos_contables.fecha, "
                        'strSQL += "terceros.documento, "
                        'strSQL += "Month(movimientos_contables.fecha) As Mes,  "
                        'strSQL += "movimientos_contables_detalles.tipo, "
                        'strSQL += "movimientos_contables_detalles.valor, "
                        'strSQL += "movimientos_contables_detalles.referencia, "
                        'strSQL += "movimientos_contables.egreso_referencia, "
                        'strSQL += "movimientos_contables.nombre_beneficiario "
                        'strSQL += "From terceros  "
                        'strSQL += "INNER Join(sistema_sucursales "
                        'strSQL += "INNER Join(tipo_comprobantes "
                        'strSQL += "INNER Join(movimientos_contables "
                        'strSQL += "INNER Join movimientos_contables_detalles "
                        'strSQL += "On movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad) "
                        'strSQL += "On tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo) "
                        'strSQL += "On sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales) "
                        'strSQL += "On terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        'strSQL += "WHERE tipo_comprobantes.descripcion Like '%RECIBOS%' "
                        'strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        'strSQL += " And movimientos_contables_detalles.tipo = 2 "

                        strSQL += "Select DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "sistema_sucursales.sucursal, "
                        strSQL += "movimientos_contables.numero, "
                        strSQL += "tipo_comprobantes.descripcion, "
                        strSQL += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS Nombre, "
                        strSQL += "movimientos_contables.fecha, "
                        strSQL += "terceros.documento, "
                        strSQL += "0 AS valor "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%RECIBOS%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY movimientos_contables.numero "
                    End If

                    If cboDocumento.SelectedValue = "Manifiestos" Then
                        strSQL += "Select  DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "ss.sucursal, "
                        strSQL += "man.numero, "
                        strSQL += "'Manifiestos' AS descripcion, "
                        strSQL += "CONCAT(ori.zona, ' - ', des.zona) As Nombre, "
                        strSQL += "man.fecha, "
                        strSQL += "mov.numero As documento, "
                        strSQL += "0 AS valor "
                        strSQL += "From movimientos_transportes_manifiestos As man "
                        strSQL += "INNER Join sistema_sucursales as ss ON ss.idsucursales = man.sistema_sucursales_origina "
                        strSQL += "INNER Join movimientos_transportes as mov ON mov.idmovimientos_transportes = man.id_refmovimiento "
                        strSQL += "INNER Join zonas ori ON man.zonas_idzonas_origen = ori.idzonas "
                        strSQL += "INNER Join zonas des ON man.zonas_idzonas_destino = des.idzonas "
                        strSQL += "WHERE man.fecha BETWEEN '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND man.sistema_sucursales_origina = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY man.numero "
                    End If

                    If cboDocumento.SelectedValue = "Mtu" Then
                        strSQL += "Select  DISTINCT 'Mototransportamos' AS Empresa, "
                        strSQL += "ss.sucursal, "
                        strSQL += "man.numero, "
                        strSQL += "'Mtu' AS descripcion, "
                        strSQL += "CONCAT(ori.zona, ' - ', des.zona) As Nombre, "
                        strSQL += "man.fecha, "
                        strSQL += "mov.numero As documento, "
                        strSQL += "0 AS valor "
                        strSQL += "From movimientos_transportes_urbanos As man "
                        strSQL += "Left Join sistema_sucursales as ss ON ss.idsucursales = man.sucursal_id "
                        strSQL += "Left Join movimientos_transportes as mov ON mov.idmovimientos_transportes = man.movimiento_id "
                        strSQL += "Left Join zonas ori ON man.origen_id = ori.idzonas "
                        strSQL += "Left Join zonas des ON man.destino_id = des.idzonas "
                        strSQL += "WHERE man.fecha BETWEEN '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND man.sucursal_id = " & hidsucursal.Value & " "
                        strSQL += "ORDER BY man.numero "
                    End If

                    If cboEmpresa.SelectedValue = 3 Then
                        ConfigurationManager.AppSettings("bdsel") = "1"
                    Else
                        If cboEmpresa.SelectedValue = 8 Then
                            ConfigurationManager.AppSettings("bdsel") = "2"
                        Else
                            If cboEmpresa.SelectedValue = 4 Then
                                ConfigurationManager.AppSettings("bdsel") = "3"
                            End If
                        End If
                    End If
                    Dim dtdoc As New DataTable

                    dtdoc = csinformes.ejecutar_query_bd(strSQL)

                    For i As Integer = 0 To dtdoc.Rows.Count - 1
                        Dim dtbus As New DataTable

                        dtbus = cscade.seleccionar_cad_envio_detalle_por_documento(dtdoc.Rows(i)("numero").ToString, cboAgencia.SelectedValue, cboDocumento.SelectedValue)

                        If dtbus.Rows.Count > 0 Then
                            dtdoc.Rows(i).Delete()
                        End If
                    Next

                    If dtdoc.Rows.Count > 0 Then
                        gridDocumentos.DataSource = dtdoc
                        gridDocumentos.DataBind()
                    Else
                        gridDocumentos.DataSource = Nothing
                        gridDocumentos.DataBind()
                    End If
                Else
                    'ConfigurationManager.ConnectionStrings("systramconnectionstringSQL").ConnectionString = ConfigurationManager.ConnectionStrings("systramconnectionstringMotosegSQL").ConnectionString
                    If cboEmpresa.SelectedValue = "1" Or cboEmpresa.SelectedValue = "2" Or cboEmpresa.SelectedValue = "9" Or cboEmpresa.SelectedValue = "11" Or cboEmpresa.SelectedValue = "13" Or cboEmpresa.SelectedValue = "14" Then
                        If cboDocumento.SelectedValue = "Egresos" Then
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                            strSQL += "From tbl_movimientos_contables "
                            strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                            strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                            strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                            strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%EGRESO%' "
                            strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                            strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                            strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                        End If

                        If cboDocumento.SelectedValue = "Facturas" And cboEmpresa.SelectedValue = "2" Then
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                            strSQL += "From tbl_movimientos_contables "
                            strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                            strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                            strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                            strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%FACTURA%' "
                            strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                            strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                            strSQL += "And COALESCE(tbl_movimientos_contables_detalles.id_ceco, '') <> '' "
                            strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                        End If

                        If cboDocumento.SelectedValue = "Notas" And cboEmpresa.SelectedValue = "2" Then
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                            strSQL += "From tbl_movimientos_contables "
                            strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                            strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                            strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                            strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%NOTA%' "
                            strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                            strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                            strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                        End If

                        If cboDocumento.SelectedValue = "Recibos" Then
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                            strSQL += "From tbl_movimientos_contables "
                            strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                            strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                            strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                            strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%RECIBO%' "
                            strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                            strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                            strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                        End If
                    Else
                        If cboEmpresa.SelectedValue = "7" Then
                            If cboDocumento.SelectedValue = "Egresos" Then
                                strSQL += "Select DISTINCT 'CiaCapri' AS Empresa, "
                                strSQL += "'Principal' AS sucursal, "
                                strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                                strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                                strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                                strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                                strSQL += "tbl_terceros.documento_terc AS documento, "
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                                strSQL += "From tbl_movimientos_contables "
                                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                                strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%EGRESO%' "
                                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                                strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                            End If

                            If cboDocumento.SelectedValue = "Facturas" Then
                                strSQL += "Select DISTINCT 'CiaCapri' AS Empresa, "
                                strSQL += "'Principal' AS sucursal, "
                                strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                                strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                                strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                                strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                                strSQL += "tbl_terceros.documento_terc AS documento, "
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                                strSQL += "From tbl_movimientos_contables "
                                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                                strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%FACTURA%' "
                                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                                strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                                strSQL += "And COALESCE(tbl_movimientos_contables_detalles.id_ceco, '') <> '' "
                                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                            End If

                            If cboDocumento.SelectedValue = "Notas" Then
                                strSQL += "Select DISTINCT 'CiaCapri' AS Empresa, "
                                strSQL += "'Principal' AS sucursal, "
                                strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                                strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                                strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                                strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                                strSQL += "tbl_terceros.documento_terc AS documento, "
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                                strSQL += "From tbl_movimientos_contables "
                                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                                strSQL += "INNER Join tbl_terceros ON tbl_movimientos_contables_detalles.id_terc = tbl_terceros.id_terc "
                                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%NOTA%' "
                                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                                strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                            End If

                            If cboDocumento.SelectedValue = "Recibos" Then
                                strSQL += "Select DISTINCT 'CiaCapri' AS Empresa, "
                                strSQL += "'Principal' AS sucursal, "
                                strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                                strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                                strSQL += "COALESCE(tbl_tramitadores.nombre_tram, 'NA') AS Nombre, "
                                strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                                strSQL += "COALESCE(tbl_tramitadores.documento_tram, '0') AS documento, "
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor "
                                strSQL += "From tbl_movimientos_contables "
                                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                                strSQL += "LEFT Join tbl_tramitadores ON tbl_movimientos_contables_detalles.id_tram = tbl_tramitadores.id_tram "
                                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%RECIBO%' "
                                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                                strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                            End If
                        End If
                    End If

                    Dim dtdoc = csinformes.ejecutar_query_bd_sql(strSQL, cboEmpresa.SelectedValue)

                    For i As Integer = 0 To dtdoc.Rows.Count - 1
                        Dim dtbus As New DataTable

                        dtbus = cscade.seleccionar_cad_envio_detalle_por_documento(dtdoc.Rows(i)("numero").ToString, cboAgencia.SelectedValue, cboDocumento.SelectedValue)

                        If dtbus.Rows.Count > 0 Then
                            dtdoc.Rows(i).Delete()
                        End If
                    Next

                    If dtdoc.Rows.Count > 0 Then
                        gridDocumentos.DataSource = dtdoc
                        gridDocumentos.DataBind()
                    Else
                        gridDocumentos.DataSource = Nothing
                        gridDocumentos.DataBind()
                    End If
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvarDetalle_Click(sender As Object, e As EventArgs) Handles btnSalvarDetalle.Click
        Try
            Dim orow As GridViewRow
            Dim resultado As String
            resultado = ""
            For Each orow In gridDocumentos.Rows
                Dim text As TextBox = orow.FindControl("txtFolios")
                Dim chek As CheckBox = orow.FindControl("chelige")

                If chek.Checked = True Then
                    If Not IsNumeric(text.Text) Or text.Text = "" Or text.Text = "0" Then
                        resultado = resultado & "El valor de Folios debe ser numerico y mayor a cero."
                    End If
                End If
            Next

            If resultado = "" Then
                Dim num As Integer
                Dim folios As Integer
                Dim strRespuesta, strNombre As String
                strRespuesta = ""
                For Each orow In gridDocumentos.Rows
                    num = gridDocumentos.DataKeys(orow.RowIndex).Values(0)
                    strNombre = gridDocumentos.DataKeys(orow.RowIndex).Values(1)

                    Dim text As TextBox = orow.FindControl("txtFolios")
                    Dim chek As CheckBox = orow.FindControl("chelige")

                    If chek.Checked = True Then
                        folios = text.Text

                        strRespuesta = strRespuesta & cscade.guardar_cad_envio_detalle(hiddetcad.Value, hidcad.Value, num, strNombre, folios, " ")
                    End If
                Next

                If strRespuesta = "" Then
                    Response.Redirect("CadEnvioDetalle.aspx?id=" & hidcad.Value & "&est=exi")
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El valor de Folios debe ser numerico y mayor a cero.');", True)
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

                    strRespuesta = cscade.eliminar_cad_envio_detalle(iddet)

                    If strRespuesta = "" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Detalle fue eliminado exitosamente...');", True)
                        Llenar_Grid_Detalle()
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
                Response.Redirect("Formato_Cad_Envio.aspx?id=" & hidcad.Value)
            Else
                Dim strRes As String

                strRes = cscade.emitir_cad_envio(hidcad.Value)

                If strRes = "" Then
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad envío emitido exitosamente...');", True)
                    Response.Redirect("CadEnvioDetalle.aspx?id=" & hidcad.Value & "&est=exi")
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Cad envío no emitido exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboAgencia_TextChanged(sender As Object, e As EventArgs) Handles cboAgencia.TextChanged
        Dim dtAge As New DataTable

        dtAge = cscuco.seleccionar_agencias_cuenta_cobro_por_id(cboAgencia.SelectedValue)

        If dtAge.Rows.Count > 0 Then
            hidsucursal.Value = dtAge.Rows(0)("id_sucursal").ToString
        Else
            hidsucursal.Value = "0"
        End If
        'hidsucursal.Value = cboAgencia.SelectedValue
    End Sub
End Class
