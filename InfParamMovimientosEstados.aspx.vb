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

Partial Class InfParamMovimientosEstados
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csinformes As New Informes

    Private Sub InfParamMovimientosEstados_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3071, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            Dim dtter As New DataTable
            Dim strSQL As String

            'strSQL = "Select movimientos_transportes_consolidado.movimiento_id, "
            'strSQL += " movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            'strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            'strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            'strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            'strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            'strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion "
            'strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            'strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            'strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            'strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            ''strSQL += " And COALESCE(ventas_control.movimientos_transportes_id, 0) = 0 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            'strSQL += " And ((COALESCE(ventas_control.idel, 0) = 2) Or COALESCE(ventas_control.movimientos_transportes_id, 0) = 0) "
            'strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            strSQL = "Select movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) as 'Egreso' "
            strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            strSQL += " WHERE movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            strSQL += " And (COALESCE(movimientos_transportes_consolidado.factura_id, 0) = 0) "
            strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += " And COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) > 0 "
            strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            dtter = csinformes.ejecutar_query_bd(strSQL)

            'If dtSab.Rows.Count > 0 Then
            '    gridSabana.DataSource = dtSab
            '    gridSabana.DataBind()
            'Else
            '    gridSabana.DataSource = Nothing
            '    gridSabana.DataBind()
            'End If

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
                strHtml += "<td align='center' colspan='6'><b><font size='4'>Pendientes Por Facturar(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Despachador</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Fecha</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Anticipos</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Placa</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Cliente</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Factura</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Cumplido</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Causación</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Gastos</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Tercero</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Empresa</font></b></td>"
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
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Despachador</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Movimiento</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Anticipos</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Cliente</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Factura</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cumplido</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Causación</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Tercero</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
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
                'For i As Integer = 0 To dtter.Rows.Count - 1
                '    Dim dtven As New DataTable

                '    strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                '    dtven = csinformes.ejecutar_query_bd(strSQL)

                '    If dtven.Rows.Count = 0 Then
                '        strDespActual = dtter.Rows(i)("Despachador").ToString

                '        Exit For
                '    End If
                'Next

                For i As Integer = 0 To dtter.Rows.Count - 1
                    'Dim dtven As New DataTable

                    'strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                    'dtven = csinformes.ejecutar_query_bd(strSQL)

                    'If dtven.Rows.Count = 0 Then
                    If strDespActual = dtter.Rows(i)("Despachador").ToString Then
                        strHtml += "<tr>"

                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    Else
                        strHtml += "<tr bgcolor='#BDBDBD'>"
                        strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                        strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
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
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    End If
                    'End If
                Next

                'strHtml += "<tr>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "</tr>"

                'strHtmlmostrar += "<tr>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='9px'>TOTALES</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTALES</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
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
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL</font></b></td>"
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtmlCuadroFinal += "</tr>"

                strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>TOTAL</font></b></td>"
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
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=MovimientoEstados.xls")
            Response.ContentEncoding = Encoding.UTF8
            Dim tw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            divmostrar.RenderControl(hw)
            Response.Write(tw.ToString())
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Movimiento Estados desde: " & txtFechaInicio.Value & " hasta: " & txtFechaFin.Value

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

    Private Sub btnPrueba1_Click(sender As Object, e As EventArgs) Handles btnPrueba1.Click
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            Dim dtter As New DataTable
            Dim strSQL As String

            'strSQL = "Select movimientos_transportes_consolidado.movimiento_id, "
            'strSQL += " movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            'strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            'strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            'strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            'strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            'strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion "
            'strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            'strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            'strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            'strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            ''strSQL += " And COALESCE(ventas_control.movimientos_transportes_id, 0) = 0 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            'strSQL += " And ((COALESCE(ventas_control.idel, 0) = 2) Or COALESCE(ventas_control.movimientos_transportes_id, 0) = 0) "
            'strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            strSQL = "Select movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) as 'Egreso', "
            strSQL += " COALESCE((terceros_cxp_detalles.valor_total - terceros_cxp_detalles.abonos), 0) AS saldo_cxp "
            strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            strSQL += " Left Join terceros_cxp_detalles ON movimientos_transportes_consolidado.movimiento_id = terceros_cxp_detalles.movimientos_transportes_id "
            strSQL += " WHERE movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            strSQL += " And (COALESCE(movimientos_transportes_consolidado.factura_id, 0) > 0) "
            strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += " And COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) = 0 "
            strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            dtter = csinformes.ejecutar_query_bd(strSQL)

            'If dtSab.Rows.Count > 0 Then
            '    gridSabana.DataSource = dtSab
            '    gridSabana.DataBind()
            'Else
            '    gridSabana.DataSource = Nothing
            '    gridSabana.DataBind()
            'End If

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
                strHtml += "<td align='center' colspan='6'><b><font size='4'>Pendientes Por Facturar(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Despachador</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Fecha</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Anticipos</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Placa</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Cliente</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Factura</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Cumplido</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Causación</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Gastos</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Saldo_CXP</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Tercero</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Empresa</font></b></td>"
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
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Despachador</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Movimiento</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Anticipos</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Cliente</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Factura</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cumplido</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Causación</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Saldo_CXP</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Tercero</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
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
                'For i As Integer = 0 To dtter.Rows.Count - 1
                '    Dim dtven As New DataTable

                '    strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                '    dtven = csinformes.ejecutar_query_bd(strSQL)

                '    If dtven.Rows.Count = 0 Then
                '        strDespActual = dtter.Rows(i)("Despachador").ToString

                '        Exit For
                '    End If
                'Next

                For i As Integer = 0 To dtter.Rows.Count - 1
                    'Dim dtven As New DataTable

                    'strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                    'dtven = csinformes.ejecutar_query_bd(strSQL)

                    'If dtven.Rows.Count = 0 Then
                    If strDespActual = dtter.Rows(i)("Despachador").ToString Then
                        strHtml += "<tr>"

                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    Else
                        strHtml += "<tr bgcolor='#BDBDBD'>"
                        strHtml += "<td align='right' colspan='14'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                        strHtmlmostrar += "<td align='right' colspan='14'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
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
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    End If
                    'End If
                Next

                'strHtml += "<tr>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "</tr>"

                'strHtmlmostrar += "<tr>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='9px'>TOTALES</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTALES</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
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
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL</font></b></td>"
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtmlCuadroFinal += "</tr>"

                strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>TOTAL</font></b></td>"
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
    End Sub

    Private Sub btnPrueba2_Click(sender As Object, e As EventArgs) Handles btnPrueba2.Click
        If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
        Else
            Dim dtter As New DataTable
            Dim strSQL As String

            'strSQL = "Select movimientos_transportes_consolidado.movimiento_id, "
            'strSQL += " movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            'strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            'strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            'strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            'strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            'strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            'strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            'strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion "
            'strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            'strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            'strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            'strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            ''strSQL += " And COALESCE(ventas_control.movimientos_transportes_id, 0) = 0 "
            'strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            'strSQL += " And ((COALESCE(ventas_control.idel, 0) = 2) Or COALESCE(ventas_control.movimientos_transportes_id, 0) = 0) "
            'strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            strSQL = "Select movimientos_transportes_consolidado.sucorigina_nombre As Sucursal, "
            strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador, "
            strSQL += " movimientos_transportes_consolidado.movimiento_numero As Numero, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fecha AS Fecha, "
            strSQL += " movimientos_transportes_consolidado.movimiento_anticipos As Anticipos, "
            strSQL += " movimientos_transportes_consolidado.vehiculo_placa AS Placa, "
            strSQL += " movimientos_transportes_consolidado.generador_nombre As Cliente, "
            strSQL += " movimientos_transportes_consolidado.factura_numero AS Factura, "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa AS 'Flete Empresa', "
            strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero AS 'Flete Tercero', "
            strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos, "
            strSQL += " movimientos_transportes_consolidado.movimiento_cumplido AS Cumplido, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.causacion_numero, 0) AS Causacion, "
            strSQL += " COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) as 'Egreso', "
            strSQL += " COALESCE((terceros_cxp_detalles.valor_total - terceros_cxp_detalles.abonos), 0) AS saldo_cxp "
            strSQL += " From movimientos_transportes_consolidado "
            'strSQL += " Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
            strSQL += " Left Join movimientos_transportes ON movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes "
            strSQL += " Left Join generadores_despachadores ON movimientos_transportes.despachador_id = generadores_despachadores.idgeneradores_asesores "
            strSQL += " Left Join usuarios ON generadores_despachadores.usuarios_idusuarios = usuarios.idusuarios "
            'strSQL += " WHERE DateDiff(Now(), movimientos_transportes_consolidado.movimiento_fecha) > 8 "
            strSQL += " Left Join terceros_cxp_detalles ON movimientos_transportes_consolidado.movimiento_id = terceros_cxp_detalles.movimientos_transportes_id "
            strSQL += " WHERE movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' AND movimientos_transportes_consolidado.movimiento_estado <> 'BLOQUEADO' "
            strSQL += " And (COALESCE(movimientos_transportes_consolidado.factura_id, 0) = 0) "
            strSQL += " And movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += " And COALESCE(movimientos_transportes_consolidado.egreso_numero, 0) = 0 "
            strSQL += " ORDER BY movimientos_transportes_consolidado.despachador_nombre, movimientos_transportes_consolidado.generador_nombre, movimientos_transportes_consolidado.movimiento_numero "

            dtter = csinformes.ejecutar_query_bd(strSQL)

            'If dtSab.Rows.Count > 0 Then
            '    gridSabana.DataSource = dtSab
            '    gridSabana.DataBind()
            'Else
            '    gridSabana.DataSource = Nothing
            '    gridSabana.DataBind()
            'End If

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
                strHtml += "<td align='center' colspan='6'><b><font size='4'>Pendientes Por Facturar(Desde: " & txtFechaInicio.Value & " Hasta: " & txtFechaFin.Value & ")</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Despachador</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Fecha</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Anticipos</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Placa</font></b></td>"
                strHtml += "<td align='left' colspan='2'><b><font size='7px'>Cliente</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Factura</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Cumplido</font></b></td>"
                strHtml += "<td align='center'><b><font size='7px'>Causación</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Gastos</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Saldo CXP</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Tercero</font></b></td>"
                strHtml += "<td align='right'><b><font size='7px'>Fte Empresa</font></b></td>"
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
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Despachador</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Numero Movimiento</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Anticipos</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Cliente</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Factura</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cumplido</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Causación</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Saldo CXP</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Tercero</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
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
                'For i As Integer = 0 To dtter.Rows.Count - 1
                '    Dim dtven As New DataTable

                '    strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                '    dtven = csinformes.ejecutar_query_bd(strSQL)

                '    If dtven.Rows.Count = 0 Then
                '        strDespActual = dtter.Rows(i)("Despachador").ToString

                '        Exit For
                '    End If
                'Next

                For i As Integer = 0 To dtter.Rows.Count - 1
                    'Dim dtven As New DataTable

                    'strSQL = "SELECT ventas_control.idel FROM ventas_control WHERE ventas_control.movimientos_transportes_id = " & dtter.Rows(i)("movimiento_id") & " AND ventas_control.idel = 0 "

                    'dtven = csinformes.ejecutar_query_bd(strSQL)

                    'If dtven.Rows.Count = 0 Then
                    If strDespActual = dtter.Rows(i)("Despachador").ToString Then
                        strHtml += "<tr>"

                        strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    Else
                        strHtml += "<tr bgcolor='#BDBDBD'>"
                        strHtml += "<td align='right' colspan='14'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                        strHtmlmostrar += "<td align='right' colspan='14'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
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
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Anticipos") & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Cliente").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Cumplido").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Causacion").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Gastos") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("saldo_cxp") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Tercero") & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Flete Empresa") & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        'decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        'decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Flete Empresa")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Flete Empresa")
                    End If
                    'End If
                Next

                'strHtml += "<tr>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtml += "</tr>"

                'strHtmlmostrar += "<tr>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='center'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                'strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#BDBDBD'>"
                strHtml += "<td align='right' colspan='13'><b><font size='9px'>TOTALES</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlmostrar += "<td align='right' colspan='13'><b><font size='1px'>TOTALES</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                'strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
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
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL</font></b></td>"
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtmlCuadroFinal += "</tr>"

                strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>TOTAL</font></b></td>"
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
    End Sub
End Class
