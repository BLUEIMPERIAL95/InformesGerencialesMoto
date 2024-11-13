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
Partial Class InfParamCostoFinancieroCliente
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamCostoFinancieroCliente_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2061, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtCos As New DataTable

            If (txtFechaInicio.Value = "" And txtFechaFin.Value = "") And cboTipo.SelectedValue = -1 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""

                strSQL = " Select mtc.generador_nombre As 'Cliente',"
                strSQL += " SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa) As 'Flete_Empresa',"
                strSQL += " SUM(mtc.movimiento_fl_tercero + mtc.movimiento_cnx_tercero) As 'Flete_Tercero',"
                strSQL += " SUM(COALESCE(mtc.movimiento_anexos, 0)) As 'Total_Gastos_Anexos',"
                strSQL += " SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa - mtc.movimiento_fl_tercero - mtc.movimiento_cnx_tercero - mtc.movimiento_anexos) As 'Ingreso',"
                strSQL += " ROUND(SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa - mtc.movimiento_fl_tercero - mtc.movimiento_cnx_tercero - mtc.movimiento_anexos) /"
                strSQL += " SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa), 2) * 100 As 'Part',"
                strSQL += " ROUND(7 * 30 *"
                strSQL += " COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) /"
                strSQL += " (SELECT SUM(vcc.venta_total)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where Year(vcc.venta_fecha) = Year(SYSDATE())"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) AS DiasCartera,"
                strSQL += " ROUND(((7 * 30 *"
                strSQL += " COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) /"
                strSQL += " (SELECT SUM(vcc.venta_total)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where Year(vcc.venta_fecha) = Year(SYSDATE())"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA')) / 30), 2) AS 'PartCos',"
                strSQL += " ROUND(((7 * 30 *"
                strSQL += " COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) /"
                strSQL += " (SELECT SUM(vcc.venta_total)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where Year(vcc.venta_fecha) = Year(SYSDATE())"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA')) / 30) / 100 *"
                strSQL += " (COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0)), 0) AS 'Costo',"
                strSQL += " SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa - mtc.movimiento_fl_tercero - mtc.movimiento_cnx_tercero - mtc.movimiento_anexos) - "
                strSQL += " ROUND(((7 * 30 *"
                strSQL += " COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) /"
                strSQL += " (SELECT SUM(vcc.venta_total)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where Year(vcc.venta_fecha) = Year(SYSDATE())"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA')) / 30) / 100 *"
                strSQL += " (COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0)), 0) AS IngReal,"
                strSQL += " ROUND((SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa - mtc.movimiento_fl_tercero - mtc.movimiento_cnx_tercero - mtc.movimiento_anexos) -"
                strSQL += " ROUND(((7 * 30 *"
                strSQL += " COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY)))"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0) /"
                strSQL += " (SELECT SUM(vcc.venta_total)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where Year(vcc.venta_fecha) = Year(SYSDATE())"
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA')) / 30) / 100 *"
                strSQL += " (COALESCE((SELECT SUM(vcc.venta_total - vcc.venta_abonos)"
                strSQL += " From ventas_consolidado vcc"
                strSQL += " Where vcc.venta_id = (Select vd.ventas_idventas From ventas_detalles vd Where vcc.venta_id = vd.ventas_idventas And vd.idel = 0 LIMIT 1)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vcc.venta_id)Is NULL)"
                strSQL += " And ((vcc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "'))"
                strSQL += " Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vcc.venta_id And vr.fecha_recaudo <= '" & txtFechaFin.Value & "')Is NULL)"
                strSQL += " And vcc.venta_abonos < vcc.venta_total And (vcc.venta_total - vcc.venta_abonos) > 10"
                strSQL += " And vcc.venta_vence<=(SELECT( ADDDATE(SYSDATE(),INTERVAL vcc.venta_plazo DAY))) "
                strSQL += " And vcc.generador_id = ven.generadores_idgeneradores AND vcc.venta_estado <> 'ANULADA'), 0)), 0)) / "
                strSQL += " SUM(mtc.movimiento_fl_empresa + mtc.movimiento_cnx_empresa), 2) * 100 As 'Inter_Real'"
                strSQL += " From movimientos_transportes_consolidado As mtc"
                strSQL += " Left Join ventas ven ON ven.idventas = mtc.factura_id"
                strSQL += " WHERE mtc.factura_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' AND mtc.generador_id = 129 "
                strSQL += " Group BY mtc.generador_nombre"
                strSQL += " ORDER BY IngReal DESC"

                dtCos = csinformes.ejecutar_query_bd(strSQL)

                If dtCos.Rows.Count > 0 Then
                    Dim totalEmp, totalTer, totalGas, totalIng, totalInt, totalDia, totalParCos, totalCos, totalIngRea, totalIntRea, cont, contotros As Int64
                    Dim otrosEmp, otrosTer, otrosGas, otrosIng, otrosInt, otrosDia, otrosParCos, otrosCos, otrosIngRea, otrosIntRea As Int64
                    Dim promInt, promDia, promCos, promIntRea As Integer
                    Dim promIntRea2 As Decimal

                    otrosEmp = 0
                    otrosTer = 0
                    otrosGas = 0
                    otrosIng = 0
                    otrosInt = 0
                    otrosDia = 0
                    otrosParCos = 0
                    otrosCos = 0
                    otrosIngRea = 0
                    otrosIntRea = 0
                    totalEmp = 0
                    totalTer = 0
                    totalGas = 0
                    totalIng = 0
                    totalInt = 0
                    totalDia = 0
                    totalParCos = 0
                    totalCos = 0
                    totalIngRea = 0
                    totalIntRea = 0
                    promInt = 0
                    promDia = 0
                    promCos = 0
                    promIntRea = 0
                    promIntRea2 = 0
                    cont = 1
                    contotros = 1
                    For i As Integer = 0 To dtCos.Rows.Count - 1
                        totalEmp = totalEmp + dtCos.Rows(i)("Flete_Empresa")
                        totalTer = totalTer + dtCos.Rows(i)("Flete_Tercero")
                        totalGas = totalGas + dtCos.Rows(i)("Total_Gastos_Anexos")
                        totalIng = totalIng + dtCos.Rows(i)("Ingreso")
                        totalInt = totalInt + dtCos.Rows(i)("Part")
                        totalDia = totalDia + dtCos.Rows(i)("DiasCartera")
                        totalParCos = totalParCos + dtCos.Rows(i)("PartCos")
                        totalCos = totalCos + dtCos.Rows(i)("Costo")
                        If dtCos.Rows(i)("IngReal") > 0 Then
                            totalIngRea = totalIngRea + dtCos.Rows(i)("IngReal")
                        End If

                        If dtCos.Rows(i)("Inter_Real") > 0 Then
                            totalIntRea = totalIntRea + dtCos.Rows(i)("Inter_Real")
                        End If

                        cont = cont + 1
                    Next

                    promInt = Math.Round(totalInt / cont, 2)
                    promDia = Math.Round(totalDia / cont, 2)
                    promCos = Math.Round(totalParCos / cont, 2)
                    promIntRea = Math.Round(totalIntRea / cont, 2)

                    Dim strHtml As String
                    strHtml = ""

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr bgcolor='#F9D2C6'>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Tot Empresa</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>TotTercero</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Tot G.Anexos</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Tot Ingreso</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Prom % Inter</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Prom Dias Cartera</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Prom % Costo F.</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Costo F.</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Ing. Real</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Prom % Inter Real</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Prom % Part Real</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='3px'>TOTALES</font></b></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalEmp) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalTer) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalGas) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalIng) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & promInt & "%</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & promDia & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & promCos & "%</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalCos) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", totalIngRea) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & promIntRea & "%</font></td>"
                    strHtml += "<td align='right'><font size='1px'>100%</font></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='left'><b><font size='1px'>Cliente</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>F.Empresa</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>F.Tercero</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>G.Anexos</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Ingreso</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>% Inter</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Dias Cartera</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>% Costo F.</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Costo F.</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Ing. Real</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>% Inter Real</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>% Part Real</font></b></td>"
                    strHtml += "</tr>"

                    For i As Integer = 0 To dtCos.Rows.Count - 1
                        If dtCos.Rows(i)("IngReal") > 0 Then
                            promIntRea2 = Math.Round(((dtCos.Rows(i)("IngReal") / totalIngRea) * 100), 2)
                        Else
                            promIntRea2 = 0
                        End If

                        If promIntRea2 > 1 Then
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCos.Rows(i)("Cliente").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("Flete_Empresa")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("Flete_Tercero")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("Total_Gastos_Anexos")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("Ingreso")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & dtCos.Rows(i)("Part") & "%</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & dtCos.Rows(i)("DiasCartera") & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & dtCos.Rows(i)("PartCos") & "%</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("Costo")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCos.Rows(i)("IngReal")) & "</font></td>"
                            If dtCos.Rows(i)("IngReal") >= 0 Then
                                strHtml += "<td align='right'><font size='1px'>" & dtCos.Rows(i)("Inter_Real") & "%</font></td>"
                                strHtml += "<td align='right'><font size='1px'>" & promIntRea2 & "%</font></td>"
                            Else
                                strHtml += "<td align='right'><font size='1px'>0%</font></td>"
                                strHtml += "<td align='right'><font size='1px'>0%</font></td>"
                            End If
                            strHtml += "</tr>"
                        Else
                            otrosEmp = otrosEmp + dtCos.Rows(i)("Flete_Empresa")
                            otrosTer = otrosTer + dtCos.Rows(i)("Flete_Tercero")
                            otrosGas = otrosGas + dtCos.Rows(i)("Total_Gastos_Anexos")
                            otrosIng = otrosIng + dtCos.Rows(i)("Ingreso")
                            otrosInt = otrosInt + dtCos.Rows(i)("Part")
                            otrosDia = otrosDia + dtCos.Rows(i)("DiasCartera")
                            otrosParCos = otrosParCos + dtCos.Rows(i)("PartCos")
                            otrosCos = otrosCos + dtCos.Rows(i)("Costo")

                            If dtCos.Rows(i)("IngReal") > 0 Then
                                otrosIngRea = otrosIngRea + dtCos.Rows(i)("IngReal")
                            End If

                            If dtCos.Rows(i)("Inter_Real") > 0 Then
                                otrosIntRea = otrosIntRea + dtCos.Rows(i)("Inter_Real")
                            End If

                            contotros = contotros + 1
                        End If
                    Next

                    promIntRea2 = Math.Round(((otrosIngRea / totalIngRea) * 100), 2)

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><font size='1px'>OTROS</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosEmp) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosTer) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosGas) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosIng) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & Math.Round(otrosInt / contotros, 0) & "%</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & Math.Round(otrosDia / contotros, 0) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & Math.Round(otrosParCos / contotros, 0) & "%</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosCos) & "</font></td>"
                    If otrosIngRea >= 0 Then
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", otrosIngRea) & "</font></td>"
                    Else
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    End If

                    If otrosIntRea >= 0 Then
                        strHtml += "<td align='right'><font size='1px'>" & Math.Round(otrosIntRea / contotros, 0) & "%</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & promIntRea2 & "%</font></td>"
                    Else
                        strHtml += "<td align='right'><font size='1px'>0%</font></td>"
                        strHtml += "<td align='right'><font size='1px'>0%</font></td>"
                    End If
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
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
    End Sub
End Class
