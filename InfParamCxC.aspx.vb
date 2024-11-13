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
Partial Class InfParamCxC
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(18, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                'Dim dtcose = csoper.capturar_contrasena_segura()
                'Dim awsUsuario = dtcose.Rows.Item(0).Item("aws_usuario_correo").ToString()
                'Dim awsClave = dtcose.Rows.Item(0).Item("aws_clave_correo").ToString()

                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""
                Dim intAño As Integer
                Dim strTipo As String

                'If cboTipo.SelectedValue = 1 Then
                '    intAño = Year(Now)
                '    strTipo = "Cartera Desde: " & intAño
                'Else
                '    If cboTipo.SelectedValue = 2 Then
                '        intAño = Year(Now) - 1
                '        strTipo = "Prejurídica Desde: " & intAño
                '    Else
                '        If cboTipo.SelectedValue = 3 Then
                '            intAño = Year(Now) - 2
                '            strTipo = "Jurídica Desde: " & intAño
                '        Else
                '            strTipo = "Completa"
                '        End If
                '    End If
                'End If

                strSQL = " Select sistema_sucursales.sucursal, 'PRINCIPAL' AS Sucursal, "
                strSQL += " CONCAT(terceros.nombre1,' ',terceros.nombre2,' ',terceros.apellido1,' ',terceros.apellido2) as Tercero,"
                strSQL += " DATE_FORMAT(propietarios_cxc.fecha, '%d/%m/%Y') as Fecha,"
                strSQL += " propietarios_cxc.observacion as 'Observacion',"
                strSQL += " COALESCE(SUM(propietarios_cxc_detalles.valor), 0) As Total,"
                strSQL += " COALESCE(SUM(propietarios_cxc_detalles.valor_abono), 0) As Abonos,"
                strSQL += " COALESCE(SUM(propietarios_cxc_detalles.valor), 0) - COALESCE(SUM(propietarios_cxc_detalles.valor_abono), 0) As Saldo,"
                strSQL += " COALESCE(movimientos_transportes.placa, '') as 'Placa',"
                strSQL += " COALESCE(movimientos_transportes.numero, '') as 'Mvto Transporte',"
                strSQL += " COALESCE(movimientos_contables.numero, '') as 'Egreso',"
                strSQL += " Case WHEN COALESCE(CONCAT(ela.nombre1,' ',ela.nombre2,' ',ela.apellido1,' ',ela.apellido2), '') <> '' THEN CONCAT(ela.nombre1,' ',ela.nombre2,' ',ela.apellido1,' ',ela.apellido2) ELSE"
                strSQL += " COALESCE(CONCAT(el1.nombre1,' ',el1.nombre2,' ',el1.apellido1,' ',el1.apellido2), '') END as 'Despachador'"
                strSQL += " From propietarios_cxc"
                strSQL += " Left Join terceros ON terceros.idterceros = propietarios_cxc.terceros_propietarios_idterceros"
                strSQL += " Left Join propietarios_cxc_detalles ON propietarios_cxc_detalles.propietarios_cxc_idcartera = propietarios_cxc.idcartera_propietarios"
                strSQL += " Left Join movimientos_transportes on movimientos_transportes.idmovimientos_transportes = propietarios_cxc.idtransporte"
                strSQL += " Left Join movimientos_contables on movimientos_contables.idcontabilidad = propietarios_cxc.idcontabilidad"
                strSQL += " Left Join usuarios desp on desp.idusuarios = movimientos_transportes.usuarios_idusuarios_ingresa"
                strSQL += " Left Join terceros ela on ela.idterceros = desp.idterceros"
                strSQL += " Left Join usuarios des1 on des1.idusuarios = propietarios_cxc.despachador_id"
                strSQL += " Left Join terceros el1 on el1.idterceros = des1.idterceros"
                strSQL += " Left Join sistema_sucursales ON movimientos_contables.sistema_sucursales_idsucursales = sistema_sucursales.idsucursales "
                strSQL += " GROUP BY Tercero, propietarios_cxc.fecha, movimientos_transportes.placa, movimientos_contables.numero, propietarios_cxc.estado, Despachador"
                strSQL += " HAVING propietarios_cxc.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "'"
                strSQL += " And Format(COALESCE(SUM(propietarios_cxc_detalles.valor), 0) - COALESCE(SUM(propietarios_cxc_detalles.valor_abono), 0), 2) > 0 "
                strSQL += " AND propietarios_cxc.estado <> 3 "
                strSQL += " ORDER BY Despachador"

                dtter = csinformes.ejecutar_query_bd(strSQL)

                'gridAsesores.DataSource = dtter
                'gridAsesores.DataBind()

                If dtter.Rows.Count > 0 Then
                    Dim pathimgCabeza1 As String
                    Dim urlFotoCabeza1 As String = ""
                    If ConfigurationManager.AppSettings("bdsel").ToString = 1 Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo.jpg")
                    Else
                        If ConfigurationManager.AppSettings("bdsel").ToString = 2 Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                        Else
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logorefri.jpg")
                        End If
                    End If

                    If File.Exists(pathimgCabeza1) Then
                        urlFotoCabeza1 = pathimgCabeza1
                    Else
                        urlFotoCabeza1 = Path.Combine(Server.MapPath("images"), "nophoto.jpg")
                    End If

                    Dim strHtml, strHtmlmostrar, strHtmlCuadroFinal, strHtmlCuadroFinalMostrar As String
                    strHtml = ""
                    strHtmlmostrar = ""
                    strHtmlCuadroFinal = ""
                    strHtmlCuadroFinalMostrar = ""

                    strHtmlCuadroFinal += "<br /><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtmlCuadroFinalMostrar += "<br /><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"

                    strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtml += "<td align='center' colspan='6'><b><font size='4'>Cuentas por Cobrar(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='7px'>Nombre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Fecha</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='7px'>Detalle</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Placa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Egreso</font></b></td>"
                    strHtml += "<td align='center' colspan='2'><b><font size='7px'>Despachador</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Total</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Abono</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Saldo</font></b></td>"
                    strHtml += "</tr>"

                    'strHtmlmostrar = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    'strHtmlmostrar += "<tr>"
                    'strHtmlmostrar += "<td align='left'></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                    'strHtmlmostrar += "</tr>"
                    'strHtmlmostrar += "<tr>"
                    'strHtmlmostrar += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    'strHtmlmostrar += "<td align='center' colspan='6'><b><font size='4'>MANIFIESTOS(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                    'strHtmlmostrar += "</tr>"
                    'strHtmlmostrar += "</table>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Sucursal</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Nombre</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Detalle</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Movimiento</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Egreso</font></b></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><b><font size='1px'>Despachador</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Total</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Abono</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Saldo</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    Dim decTotal1, decTotal2, decTotal3, decTotalDespachador As Decimal
                    Dim strDespActual As String
                    'decTotalDespachador = 0
                    'decTotalSucursal = 0
                    'decTotalAnticipos = 0
                    'strSucActual = dtter.Rows(0)("Sucursal").ToString
                    'strDespActual = dtter.Rows(0)("Despachador").ToString
                    decTotal1 = 0
                    decTotal2 = 0
                    decTotal3 = 0
                    decTotalDespachador = 0
                    strDespActual = dtter.Rows(0)("Despachador").ToString
                    For i As Integer = 0 To dtter.Rows.Count - 1
                        If strDespActual = dtter.Rows(i)("Despachador").ToString Then
                            strHtml += "<tr>"

                            strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Total") & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Abonos") & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Saldo") & "</font></td>"

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Total") & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Abonos") & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Saldo") & "</font></td>"

                            strHtmlmostrar += "</tr>"

                            decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                            decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                            decTotal3 = decTotal3 + dtter.Rows(i)("Saldo")
                            decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Saldo")
                        Else
                            strHtml += "<tr bgcolor='#BDBDBD'>"
                            strHtml += "<td align='right' colspan='12'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                            strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                            strHtmlmostrar += "<td align='right' colspan='12'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                            strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                            strHtmlmostrar += "</tr>"

                            strHtmlCuadroFinal += "<tr>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & strDespActual & ": </font></b></td>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                            strHtmlCuadroFinal += "</tr>"

                            strHtmlCuadroFinalMostrar += "<tr>"
                            strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & strDespActual & ": </font></b></td>"
                            strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                            strHtmlCuadroFinalMostrar += "</tr>"

                            decTotalDespachador = 0
                            strDespActual = dtter.Rows(i)("Despachador").ToString

                            strHtml += "<tr>"

                            strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                            'strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Total") & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Abonos") & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Saldo") & "</font></td>"

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                            'strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Total") & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Abonos") & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Saldo") & "</font></td>"

                            strHtmlmostrar += "</tr>"

                            decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                            decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                            decTotal3 = decTotal3 + dtter.Rows(i)("Saldo")
                            decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Saldo")
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='right' colspan='12'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlmostrar += "<td align='right' colspan='12'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                    strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='right' colspan='8'><b><font size='9px'>TOTALES</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlmostrar += "<td align='right' colspan='8'><b><font size='1px'>TOTALES</font></b></td>"
                    strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtmlCuadroFinal += "<tr>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & strDespActual & ": </font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinalMostrar += "<tr>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & strDespActual & ": </font></b></td>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtmlCuadroFinalMostrar += "</tr>"

                    strHtmlCuadroFinal += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL CXC</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>TOTAL CXC</font></b></td>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtmlCuadroFinalMostrar += "</tr>"

                    strHtml += "</table>"
                    strHtmlmostrar += "</table>"

                    strHtmlCuadroFinal += "</table>"
                    strHtmlCuadroFinalMostrar += "</table>"

                    strHtml += strHtmlCuadroFinal
                    strHtmlmostrar += strHtmlCuadroFinalMostrar

                    divmostrar.InnerHtml = strHtmlmostrar
                    divinforme.InnerHtml = strHtml
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=ExcelFile.xls")
            Response.ContentEncoding = Encoding.UTF8
            Dim tw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            divmostrar.RenderControl(hw)
            Response.Write(tw.ToString())
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try

        'Dim name = "Asesores"

        'Dim sb As New StringBuilder()
        'Dim sw As New StringWriter(sb)
        'Dim htw As New HtmlTextWriter(sw)

        'Dim page As New Page()
        'Dim form As New HtmlForm()

        'gridAsesores.EnableViewState = False

        '' Deshabilitar la validación de eventos, sólo asp.net 2 
        'page.EnableEventValidation = False

        '' Realiza las inicializaciones de la instancia de la clase Page que requieran los diseñadores RAD. 
        'page.DesignerInitialize()

        'page.Controls.Add(form)
        'form.Controls.Add(gridAsesores)

        'page.RenderControl(htw)

        'Response.Clear()
        'Response.Buffer = True

        'Response.ContentType = "application/pdf"
        'Response.AddHeader("Content-Disposition", "attachment;filename=" & name & ".pdf")
        'Response.Charset = "UTF-8"


        'Response.ContentEncoding = Encoding.[Default]
        'Response.Write(sb.ToString())
        'Response.[End]()
    End Sub

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Cuentas por Cobrar desde: " & txtFechaInicio.Value & " hasta: " & txtFechaFin.Value

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim stringWriter As StringWriter = New StringWriter()
            Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
            divinforme.RenderControl(htmlTextWriter)
            Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
            Dim Doc As Document = New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            Dim htmlparser As HTMLWorker = New HTMLWorker(Doc)
            PdfWriter.GetInstance(Doc, Response.OutputStream)
            Doc.Open()
            htmlparser.Parse(stringReader)
            Doc.Close()
            Response.Write(Doc)
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
