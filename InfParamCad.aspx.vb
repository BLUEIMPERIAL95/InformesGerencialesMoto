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
Imports System.Globalization
Imports System.Math
Partial Class InfParamCad
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim cscade As New cad

    Private Sub InfParamCad_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(2060, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If

            If Me.IsPostBack = False Then
                combos()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtprov, dtaut, dtempr, dtret As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo_cad", dtempr, cboEmpresa)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Or cboEmpresa.SelectedValue = "0" Or cboDocumento.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros inválidos...');", True)
            Else
                Dim strSQL As String
                strSQL = ""
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
                        strSQL += "movimientos_contables_detalles.valor, "
                        strSQL += "'' AS Entregado, "
                        strSQL += "0 AS NumEnv, "
                        strSQL += "'' AS Recibido, "
                        strSQL += "0 AS NumRec "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%EGRESO%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & cboSucursal.SelectedValue & " "
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
                        strSQL += "0 AS valor, "
                        strSQL += "'' AS Entregado, "
                        strSQL += "0 AS NumEnv, "
                        strSQL += "'' AS Recibido, "
                        strSQL += "0 AS NumRec "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%FACTURACION ELECTRONICA%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 1 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & cboSucursal.SelectedValue & " "
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
                        strSQL += "0 AS valor, "
                        strSQL += "'' AS Entregado, "
                        strSQL += "0 AS NumEnv, "
                        strSQL += "'' AS Recibido, "
                        strSQL += "0 AS NumRec "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%NOTAS%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & cboSucursal.SelectedValue & " "
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
                        strSQL += "0 AS valor, "
                        strSQL += "'' AS Entregado, "
                        strSQL += "0 AS NumEnv, "
                        strSQL += "'' AS Recibido, "
                        strSQL += "0 AS NumRec "
                        strSQL += "From movimientos_contables "
                        strSQL += "INNER Join sistema_sucursales ON sistema_sucursales.idsucursales = movimientos_contables.sistema_sucursales_idsucursales "
                        strSQL += "INNER Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strSQL += "INNER Join terceros ON terceros.idterceros = movimientos_contables_detalles.terceros_idterceros "
                        strSQL += "INNER Join tipo_comprobantes ON tipo_comprobantes.idtipo_comprobantes = movimientos_contables.tipo_comprobantes_idtipo "
                        strSQL += "WHERE tipo_comprobantes.descripcion Like '%RECIBOS%' "
                        strSQL += "And movimientos_contables_detalles.tipo = 2 "
                        strSQL += "And movimientos_contables.fecha Between '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND movimientos_contables.sistema_sucursales_idsucursales = " & cboSucursal.SelectedValue & " "
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
                        strSQL += "0 AS valor, "
                        strSQL += "'' AS Entregado, "
                        strSQL += "0 AS NumEnv, "
                        strSQL += "'' AS Recibido, "
                        strSQL += "0 AS NumRec "
                        strSQL += "From movimientos_transportes_manifiestos As man "
                        strSQL += "INNER Join sistema_sucursales as ss ON ss.idsucursales = man.sistema_sucursales_origina "
                        strSQL += "INNER Join movimientos_transportes as mov ON mov.idmovimientos_transportes = man.id_refmovimiento "
                        strSQL += "INNER Join zonas ori ON man.zonas_idzonas_origen = ori.idzonas "
                        strSQL += "INNER Join zonas des ON man.zonas_idzonas_destino = des.idzonas "
                        strSQL += "WHERE man.fecha BETWEEN '" & txtFechaInicio.Value & "' And '" & txtFechaFin.Value & "' "
                        strSQL += "AND man.sistema_sucursales_origina = " & cboSucursal.SelectedValue & " "
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

                    'For i As Integer = 0 To dtdoc.Rows.Count - 1
                    '    Dim dtbus As New DataTable

                    '    dtbus = cscade.seleccionar_cad_envio_detalle_por_documento(dtdoc.Rows(i)("numero").ToString, cboDocumento.SelectedValue)

                    '    If dtbus.Rows.Count > 0 Then
                    '        dtdoc.Rows(i).Delete()
                    '    End If
                    'Next

                    If dtdoc.Rows.Count > 0 Then
                        gridDocumentos.DataSource = dtdoc
                        gridDocumentos.DataBind()
                    Else
                        gridDocumentos.DataSource = Nothing
                        gridDocumentos.DataBind()
                    End If

                    Dim num As Integer
                    For Each orow In gridDocumentos.Rows
                        num = gridDocumentos.DataKeys(orow.RowIndex).Values(0)

                        Dim dtbus, dtbus1 As New DataTable

                        dtbus = cscade.seleccionar_cad_envio_detalle_por_documento_informe(num, cboSucursal.SelectedValue, cboDocumento.SelectedValue)
                        dtbus1 = cscade.seleccionar_cad_recibido_detalle_por_documento_informe(num, cboSucursal.SelectedValue, cboDocumento.SelectedValue)

                        If dtbus.Rows.Count > 0 Then
                            gridDocumentos.Rows(orow.RowIndex).Cells(7).Text = "SI"
                            gridDocumentos.Rows(orow.RowIndex).Cells(8).Text = dtbus.Rows(0)("numero_caen").ToString
                        Else
                            gridDocumentos.Rows(orow.RowIndex).Cells(7).Text = "NO"
                            gridDocumentos.Rows(orow.RowIndex).Cells(8).Text = ""
                        End If

                        If dtbus1.Rows.Count > 0 Then
                            gridDocumentos.Rows(orow.RowIndex).Cells(9).Text = "SI"
                            gridDocumentos.Rows(orow.RowIndex).Cells(10).Text = dtbus1.Rows(0)("numero_care").ToString
                        Else
                            gridDocumentos.Rows(orow.RowIndex).Cells(9).Text = "NO"
                            gridDocumentos.Rows(orow.RowIndex).Cells(10).Text = ""
                        End If
                    Next
                Else
                    'ConfigurationManager.ConnectionStrings("systramconnectionstringSQL").ConnectionString = ConfigurationManager.ConnectionStrings("systramconnectionstringMotosegSQL").ConnectionString
                    If cboEmpresa.SelectedValue = "2" Then
                        If cboDocumento.SelectedValue = "Egresos" Then
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                            strSQL += "'' AS Entregado, "
                            strSQL += "0 AS NumEnv, "
                            strSQL += "'' AS Recibido, "
                            strSQL += "0 AS NumRec "
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
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                            strSQL += "'' AS Entregado, "
                            strSQL += "0 AS NumEnv, "
                            strSQL += "'' AS Recibido, "
                            strSQL += "0 AS NumRec "
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
                            strSQL += "Select DISTINCT 'Motoseguridad' AS Empresa, "
                            strSQL += "'Principal' AS sucursal, "
                            strSQL += "tbl_movimientos_contables.numero_moco AS numero, "
                            strSQL += "tbl_comprobantes.nombre_comp As descripcion, "
                            strSQL += "tbl_terceros.nombre_terc AS Nombre, "
                            strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                            strSQL += "tbl_terceros.documento_terc AS documento, "
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                            strSQL += "'' AS Entregado, "
                            strSQL += "0 AS NumEnv, "
                            strSQL += "'' AS Recibido, "
                            strSQL += "0 AS NumRec "
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
                            strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                            strSQL += "'' AS Entregado, "
                            strSQL += "0 AS NumEnv, "
                            strSQL += "'' AS Recibido, "
                            strSQL += "0 AS NumRec "
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
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                                strSQL += "'' AS Entregado, "
                                strSQL += "0 AS NumEnv, "
                                strSQL += "'' AS Recibido, "
                                strSQL += "0 AS NumRec "
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
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                                strSQL += "'' AS Entregado, "
                                strSQL += "0 AS NumEnv, "
                                strSQL += "'' AS Recibido, "
                                strSQL += "0 AS NumRec "
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
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                                strSQL += "'' AS Entregado, "
                                strSQL += "0 AS NumEnv, "
                                strSQL += "'' AS Recibido, "
                                strSQL += "0 AS NumRec "
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
                                strSQL += "tbl_tramitadores.nombre_tram AS Nombre, "
                                strSQL += "tbl_movimientos_contables.fecha_moco As fecha, "
                                strSQL += "tbl_tramitadores.documento_tram AS documento, "
                                strSQL += "tbl_movimientos_contables_detalles.valor_mcde AS valor, "
                                strSQL += "'' AS Entregado, "
                                strSQL += "0 AS NumEnv, "
                                strSQL += "'' AS Recibido, "
                                strSQL += "0 AS NumRec "
                                strSQL += "From tbl_movimientos_contables "
                                strSQL += "INNER Join tbl_movimientos_contables_detalles ON tbl_movimientos_contables.id_moco = tbl_movimientos_contables_detalles.id_moco "
                                strSQL += "INNER Join tbl_tramitadores ON tbl_movimientos_contables_detalles.id_tram = tbl_tramitadores.id_tram "
                                strSQL += "INNER Join tbl_comprobantes ON tbl_movimientos_contables.id_comp = tbl_comprobantes.id_comp "
                                strSQL += "WHERE tbl_comprobantes.nombre_comp Like '%RECIBO%' "
                                strSQL += "And tbl_movimientos_contables_detalles.tipo_mcde = 2 "
                                strSQL += "And tbl_movimientos_contables.fecha_moco BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                                strSQL += "ORDER BY tbl_movimientos_contables.numero_moco "
                            End If
                        End If
                    End If

                    Dim dtdoc = csinformes.ejecutar_query_bd_sql(strSQL, cboEmpresa.SelectedValue)

                    If dtdoc.Rows.Count > 0 Then
                        gridDocumentos.DataSource = dtdoc
                        gridDocumentos.DataBind()
                    Else
                        gridDocumentos.DataSource = Nothing
                        gridDocumentos.DataBind()
                    End If

                    Dim num As Integer
                    For Each orow In gridDocumentos.Rows
                        num = gridDocumentos.DataKeys(orow.RowIndex).Values(0)

                        Dim dtbus, dtbus1 As New DataTable

                        dtbus = cscade.seleccionar_cad_envio_detalle_por_documento_informe(num, cboSucursal.SelectedValue, cboDocumento.SelectedValue)
                        dtbus1 = cscade.seleccionar_cad_recibido_detalle_por_documento_informe(num, cboSucursal.SelectedValue, cboDocumento.SelectedValue)

                        If dtbus.Rows.Count > 0 Then
                            gridDocumentos.Rows(orow.RowIndex).Cells(7).Text = "SI"
                            gridDocumentos.Rows(orow.RowIndex).Cells(8).Text = dtbus.Rows(0)("numero_caen").ToString
                        Else
                            gridDocumentos.Rows(orow.RowIndex).Cells(7).Text = "NO"
                            gridDocumentos.Rows(orow.RowIndex).Cells(8).Text = ""
                        End If

                        If dtbus1.Rows.Count > 0 Then
                            gridDocumentos.Rows(orow.RowIndex).Cells(9).Text = "SI"
                            gridDocumentos.Rows(orow.RowIndex).Cells(10).Text = dtbus1.Rows(0)("numero_care").ToString
                        Else
                            gridDocumentos.Rows(orow.RowIndex).Cells(9).Text = "NO"
                            gridDocumentos.Rows(orow.RowIndex).Cells(10).Text = ""
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub gridDocumentos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gridDocumentos.RowDataBound
        Try
            Dim num As Integer

            If e.Row.RowType = DataControlRowType.DataRow Then
                num = gridDocumentos.DataKeys(e.Row.RowIndex).Value

                Dim dtbus1 As New DataTable
                dtbus1 = cscade.seleccionar_cad_recibido_detalle_por_documento_informe(num, cboSucursal.SelectedValue, cboDocumento.SelectedValue)

                If dtbus1.Rows.Count = 0 Then
                    e.Row.BackColor = Drawing.Color.Yellow
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Dim name = "InformeCad"

            Dim sb As New StringBuilder()
            Dim sw As New StringWriter(sb)
            Dim htw As New HtmlTextWriter(sw)

            Dim page As New Page()
            Dim form As New HtmlForm()

            gridDocumentos.EnableViewState = False

            ' Deshabilitar la validación de eventos, sólo asp.net 2 
            page.EnableEventValidation = False

            ' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
            page.DesignerInitialize()

            page.Controls.Add(form)
            form.Controls.Add(gridDocumentos)

            page.RenderControl(htw)

            Response.Clear()
            Response.Buffer = True

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".xls")
            Response.Charset = "UTF-8"

            Response.ContentEncoding = Encoding.[Default]
            Response.Write(sb.ToString())
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub cboEmpresa_TextChanged(sender As Object, e As EventArgs) Handles cboEmpresa.TextChanged
        Dim dtage As New DataTable

        csoper.LlenarDropDownList_Sql_Parametro("nombre", "id", "sp_seleccionar_agencias_cuenta_cobro_combo_sucursal", dtage, cboSucursal, cboEmpresa.SelectedValue)
    End Sub
End Class
