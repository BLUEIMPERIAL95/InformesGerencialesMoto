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
Imports System.Math

Partial Class InfParamCarteraXAsesorConsolidado
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamCarteraXAsesorConsolidado_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtase, dtgen As New DataTable

        csoper.LlenarDropDownList("nombrecompleto", "idgeneradores", "generadores_mostrar_todos", dtgen, cbogeneradores)
        csoper.LlenarDropDownList("Nombre", "id", "cargarasesores", dtase, cboasesores)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        'Try
        Dim strSQL, strTipo, color As String
        Dim dtter As New DataTable
        color = "FFFFFF"

        If txtFechaInicio.Value = "" Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fecha Inválida...');", True)
        Else
            divmostrar.InnerHtml = ""
            divinforme.InnerHtml = ""
            Dim intAño As Integer

            'If cboTipo.SelectedValue = 1 Then
            '    intAño = Year(Now)
            '    strTipo = "Cartera Año: " & intAño
            'Else
            '    If cboTipo.SelectedValue = 2 Then
            '        intAño = Year(Now) - 1
            '        strTipo = "Cartera Año: " & intAño
            '    Else
            '        If cboTipo.SelectedValue = 3 Then
            '            intAño = Year(Now) - 2
            '            strTipo = "Cartera Menor o igual Año: " & intAño
            '        Else
            '            If cboTipo.SelectedValue = 4 Then
            '                strTipo = "Asesores (a 29 días)"
            '            Else
            '                If cboTipo.SelectedValue = 5 Then
            '                    strTipo = "Cartera (Entre 30 y 59 dias)"
            '                Else
            '                    If cboTipo.SelectedValue = 6 Then
            '                        strTipo = "Prejuridica (Entre 60 y 79 dias)"
            '                    Else
            '                        If cboTipo.SelectedValue = 7 Then
            '                            strTipo = "Juridica (Mas de 80 dias)"
            '                        Else
            '                            strTipo = "Completa"
            '                        End If
            '                    End If
            '                End If
            '            End If
            '        End If
            '    End If
            'End If

            strTipo = "Completa"

            'strSQL = " Select COALESCE(vc.asesor_id, 0) As idas, "
            If Session("empresa") = "Refrilogistica" Then
                strSQL = " Select COALESCE(vc.asesor_nombre, "
                strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
                strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'JULIANA  GIRALDO CASTILLO')) as Asesor, "
            Else
                strSQL = " Select COALESCE(vc.asesor_nombre, "
                strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
                strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')) as Asesor, "
            End If
            'strSQL += " vc.generador_id, "
            strSQL += " vc.generador_nombre As Generador, "
            strSQL += " - -(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo AS DiasFac, "
            strSQL += " Case WHEN ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) < 30 And YEAR(vc.venta_fecha) = YEAR(NOW()) THEN '1' ELSE "
            strSQL += " Case WHEN ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) >= 30 And ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) < 45 And YEAR(vc.venta_fecha) = YEAR(NOW()) THEN '2' ELSE "
            strSQL += " Case WHEN ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) >= 45 And ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) < 70 And YEAR(vc.venta_fecha) = YEAR(NOW()) THEN '3' ELSE "
            strSQL += " Case WHEN ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) >= 70 And YEAR(vc.venta_fecha) = YEAR(NOW()) THEN '4' ELSE "
            strSQL += " Case WHEN YEAR(vc.venta_fecha) = (YEAR(NOW()) - 1) THEN '5' ELSE "
            strSQL += " Case WHEN ((SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo) >= 70 And YEAR(vc.venta_fecha) <= (YEAR(NOW()) - 2) THEN '6' END END END END END END as DesCart, "
            strSQL += " SUM((vc.venta_total - vc.venta_abonos)) As Saldo "
            strSQL += " From ventas_consolidado vc "
            strSQL += " INNER Join terceros ter ON (vc.generador_terceroid=ter.idterceros)"
            strSQL += " INNER Join zonas zo on(ter.zonas_idzonas=zo.idzonas)"
            strSQL += " INNER Join generadores gen ON(vc.generador_id=gen.idgeneradores And gen.idel=0)"
            strSQL += " Left Join generadores_asesores gena ON(vc.asesor_id=gena.idgeneradores_asesores)"
            strSQL += " Left Join usuarios us ON(us.idusuarios=gena.usuarios_idusuarios)"
            strSQL += " WHERE vc.venta_id = (SELECT vd.ventas_idventas FROM ventas_detalles vd WHERE vc.venta_id=vd.ventas_idventas And vd.idel=0 LIMIT 1) "
            strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)Is NULL)"
            'strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "')) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "')Is NULL)"
            strSQL += " And vc.venta_abonos < vc.venta_total And (vc.venta_total - vc.venta_abonos) > 10"

            'If cboTipo.SelectedValue = 1 Then
            '    strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND YEAR(vc.venta_vence) = '" & intAño & "'"
            'Else
            '    If cboTipo.SelectedValue = 2 Then
            '        strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND YEAR(vc.venta_vence) = '" & intAño & "'"
            '    Else
            '        If cboTipo.SelectedValue = 3 Then
            '            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND YEAR(vc.venta_vence) <= '" & intAño & "'"
            '        Else
            '            If cboTipo.SelectedValue = 4 Then
            '                strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo < 30"
            '            Else
            '                If cboTipo.SelectedValue = 5 Then
            '                    strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY)))AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo BETWEEN 30 AND 59"
            '                Else
            '                    If cboTipo.SelectedValue = 6 Then
            '                        strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo BETWEEN 60 AND 79"
            '                    Else
            '                        If cboTipo.SelectedValue = 7 Then
            '                            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo >= 80"
            '                        Else
            '                            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY)))"
            '                        End If
            '                    End If
            '                End If
            '            End If
            '        End If
            '    End If
            'End If

            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY)))"

            If cboasesores.SelectedValue > 0 Then
                strSQL += " AND COALESCE((SELECT generadores_asesores.usuarios_idusuarios FROM generadores_asesores "
                strSQL += " Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += " WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
                strSQL += " ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 0) = " & cboasesores.SelectedValue & ""
            End If

            If cbogeneradores.SelectedValue > 0 Then
                strSQL += " AND vc.generador_id = " & cbogeneradores.SelectedValue & ""
            End If

            'strSQL += " Group BY vc.generador_documento "
            'strSQL += " ORDER BY Saldo DESC"

            If Session("empresa") = "Refrilogistica" Then
                strSQL += " Group BY vc.generador_nombre, COALESCE(vc.asesor_nombre, "
                strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
                strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'JULIANA  GIRALDO CASTILLO')), DesCart "
                strSQL += " ORDER BY Asesor,vc.generador_nombre,DesCart"
            Else
                strSQL += " Group BY vc.generador_nombre, COALESCE(vc.asesor_nombre, "
                strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
                strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')), DesCart "
                strSQL += " ORDER BY Asesor,vc.generador_nombre,DesCart"
            End If

            dtter = csinformes.ejecutar_query_bd(strSQL)

            If dtter.Rows.Count > 0 Then
                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                Dim strAsesorActual, strGeneradorActual As String
                Dim contTD As Integer
                Dim decTotalCliente, decTotal1, decTotal2, decTotal3, decTotal4, decTotal5, decTotal6, decTotal7, decTotTotal1, decTotTotal2, decTotTotal3, decTotTotal4, decTotTotal5, decTotTotal6, decTotTotal7 As Decimal
                decTotalCliente = 0
                decTotal1 = 0
                decTotal2 = 0
                decTotal3 = 0
                decTotal4 = 0
                decTotal5 = 0
                decTotal6 = 0
                decTotal7 = 0
                decTotTotal1 = 0
                decTotTotal2 = 0
                decTotTotal3 = 0
                decTotTotal4 = 0
                decTotTotal5 = 0
                decTotTotal6 = 0
                decTotTotal7 = 0

                contTD = 1

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

                Dim strHtml, strHtmlmostrar As String
                strHtml = ""
                strHtmlmostrar = ""

                strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left'></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='80' width='180'></td>"
                strHtml += "<td align='center' colspan='5'><b><font size='17px'>CARTERA X ASESOR(Hasta Fecha: " & txtFechaInicio.Value & " Tipo: " & strTipo & ")</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' colspan='3'><b><font size='13px'>" & dtter.Rows(0)("Asesor").ToString & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera < 30 dias</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera entre 30 y 45</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera entre 46 y 69</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera 70 dias o mas(" & Year(Now()) & ")</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera (" & Year(Now()) - 1 & ")</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera Juridica</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Total de saldo</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' colspan='3'><b><font size='12px'>" & dtter.Rows(0)("Generador").ToString & "</font></b></td>"

                strHtmlmostrar = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='4'></font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left'></td>"
                strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>CARTERA X ASESOR(Hasta Fecha: " & txtFechaInicio.Value & " Tipo: " & strTipo & ")</font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "</table>"
                strHtmlmostrar += "<br />"
                strHtmlmostrar += "<br />"
                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera < 30 dias</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera entre 30 y 45</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera entre 46 y 69</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera 70 dias o mas(" & Year(Now()) & ")</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera (" & Year(Now()) - 1 & ")</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera Juridica</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Total de saldo</font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>" & dtter.Rows(0)("Generador").ToString & "</font></b></td>"

                strAsesorActual = dtter.Rows(0)("Asesor").ToString
                strGeneradorActual = dtter.Rows(0)("Generador").ToString
                For i As Integer = 0 To dtter.Rows.Count - 1
                    If i Mod 2 = 0 Then
                        color = "#FFFFFF"
                    Else
                        color = "#EFF3FB"
                    End If

                    If strAsesorActual = dtter.Rows(i)("Asesor").ToString Then
                        If strGeneradorActual = dtter.Rows(i)("Generador").ToString Then
                            'strHtml += "<tr>"

                            If dtter.Rows(i)("DesCart") > contTD Then
                                For j As Integer = contTD + 1 To dtter.Rows(i)("DesCart")
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                Next
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                contTD = dtter.Rows(i)("DesCart")
                            Else
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            End If
                            decTotalCliente = decTotalCliente + dtter.Rows(i)("Saldo")
                            'decTotal7 = decTotal7 + decTotalCliente
                            contTD = contTD + 1
                        Else
                            If contTD <= 6 Then
                                For j As Integer = contTD To 6
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                Next
                            End If
                            decTotal7 = decTotal7 + decTotalCliente
                            strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                            strHtml += "</tr>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                            strHtmlmostrar += "</tr>"
                            contTD = 1
                            decTotalCliente = 0
                            strHtml += "<tr>"
                            strHtml += "<td align='left' colspan='3'><b><font size='12px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                            If dtter.Rows(i)("DesCart") > contTD Then
                                For j As Integer = contTD + 1 To dtter.Rows(i)("DesCart")
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                Next
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                contTD = dtter.Rows(i)("DesCart")
                            Else
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            End If
                            contTD = contTD + 1
                            decTotalCliente = decTotalCliente + dtter.Rows(i)("Saldo")
                            'decTotal7 = decTotal7 + decTotalCliente
                            strGeneradorActual = dtter.Rows(i)("Generador").ToString
                        End If
                    Else
                        If contTD <= 6 Then
                            For j As Integer = contTD To 6
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            Next
                        End If
                        decTotal7 = decTotal7 + decTotalCliente
                        decTotTotal7 = decTotTotal7 + decTotal7
                        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                        strHtml += "</tr>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                        strHtmlmostrar += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='3'><b><font size='13px'>TOTALES</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal6) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal7) & "</font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='3'><b><font size='13px'>PORCENTAJES</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal1 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal2 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal3 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal4 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal5 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal6 / decTotal7, 2)) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round((decTotal1 / decTotal7) + (decTotal2 / decTotal7) + (decTotal3 / decTotal7) + (decTotal4 / decTotal7) + (decTotal5 / decTotal7) + (decTotal6 / decTotal7), 2)) & "</font></b></td>"
                        strHtml += "</tr>"
                        contTD = 1
                        decTotalCliente = 0
                        decTotal1 = 0
                        decTotal2 = 0
                        decTotal3 = 0
                        decTotal4 = 0
                        decTotal5 = 0
                        decTotal6 = 0
                        decTotal7 = 0
                        strHtml += "</table>"
                        strHtml += "<br />"
                        strHtml += "<br />"
                        strHtml += "<br />"
                        strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='left' colspan='3'><b><font size='13px'>" & dtter.Rows(i)("Asesor").ToString & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera < 30 dias</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera entre 30 y 45</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera entre 46 y 69</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera 70 dias o mas(" & Year(Now()) & ")</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera (" & Year(Now()) - 1 & ")</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Cartera Juridica</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Total de saldo</font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                        strHtml += "<td align='left' colspan='3'><b><font size='12px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                        'strHtmlmostrar += "</tr>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                        If dtter.Rows(i)("DesCart") > contTD Then
                            For j As Integer = contTD + 1 To dtter.Rows(i)("DesCart")
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                            Next
                            strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            contTD = dtter.Rows(i)("DesCart")
                        Else
                            strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                        End If
                        contTD = contTD + 1
                        decTotalCliente = decTotalCliente + dtter.Rows(i)("Saldo")
                        'decTotal7 = decTotal7 + decTotalCliente
                        strAsesorActual = dtter.Rows(i)("Asesor").ToString
                    End If

                    If dtter.Rows(i)("DesCart") = 1 Then
                        decTotal1 = decTotal1 + dtter.Rows(i)("Saldo")
                        decTotTotal1 = decTotTotal1 + dtter.Rows(i)("Saldo")
                    Else
                        If dtter.Rows(i)("DesCart") = 2 Then
                            decTotal2 = decTotal2 + dtter.Rows(i)("Saldo")
                            decTotTotal2 = decTotTotal2 + dtter.Rows(i)("Saldo")
                        Else
                            If dtter.Rows(i)("DesCart") = 3 Then
                                decTotal3 = decTotal3 + dtter.Rows(i)("Saldo")
                                decTotTotal3 = decTotTotal3 + dtter.Rows(i)("Saldo")
                            Else
                                If dtter.Rows(i)("DesCart") = 4 Then
                                    decTotal4 = decTotal4 + dtter.Rows(i)("Saldo")
                                    decTotTotal4 = decTotTotal4 + dtter.Rows(i)("Saldo")
                                Else
                                    If dtter.Rows(i)("DesCart") = 5 Then
                                        decTotal5 = decTotal5 + dtter.Rows(i)("Saldo")
                                        decTotTotal5 = decTotTotal5 + dtter.Rows(i)("Saldo")
                                    Else
                                        If dtter.Rows(i)("DesCart") = 6 Then
                                            decTotal6 = decTotal6 + dtter.Rows(i)("Saldo")
                                            decTotTotal6 = decTotTotal6 + dtter.Rows(i)("Saldo")
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                If contTD <= 6 Then
                    For j As Integer = contTD To 6
                        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    Next
                End If
                decTotal7 = decTotal7 + decTotalCliente
                decTotTotal7 = decTotTotal7 + decTotal7
                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtml += "<td align='center' colspan='3'><b><font size='13px'>TOTALES</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal6) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal7) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' colspan='3'><b><font size='13px'>PORCENTAJES</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal1 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal2 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal3 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal4 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal5 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal6 / decTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round((decTotal1 / decTotal7) + (decTotal2 / decTotal7) + (decTotal3 / decTotal7) + (decTotal4 / decTotal7) + (decTotal5 / decTotal7) + (decTotal6 / decTotal7), 2)) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' colspan='3'><b><font size='13px'>TOTALES CARTERA</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal1) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal2) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal3) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal4) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal5) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal6) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotTotal7) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center' colspan='3'><b><font size='13px'>PORCENTAJES CARTERA</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal1 / decTotTotal7, 2)) & "</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal2 / decTotTotal7, 2)) & "</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal3 / decTotTotal7, 2)) & "</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal4 / decTotTotal7, 2)) & "</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal5 / decTotTotal7, 2)) & "</font></b></td>"
                'strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotTotal6 / decTotTotal7, 2)) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal1 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal2 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal3 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal4 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal5 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", decTotTotal6 / decTotTotal7) & "</font></b></td>"
                strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round((decTotTotal1 / decTotTotal7) + (decTotTotal2 / decTotTotal7) + (decTotTotal3 / decTotTotal7) + (decTotTotal4 / decTotTotal7) + (decTotTotal5 / decTotTotal7) + (decTotTotal6 / decTotTotal7), 2)) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                'strHtmlmostrar += "</tr>"
                'strHtmlmostrar += "<tr>"
                strHtmlmostrar += "</table>"
                strHtmlmostrar += "<br />"
                strHtmlmostrar += "<br />"
                strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center' colspan='3'><b><font size='1px'>TOTALES CARTERA</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal1) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal2) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal3) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal4) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal5) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal6) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='center' colspan='3'><b><font size='1px'>PORCENTAJES CARTERA</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal1 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal2 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal3 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal4 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal5 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", decTotTotal6 / decTotTotal7) & "</font></b></td>"
                strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:P2}", Round((decTotTotal1 / decTotTotal7) + (decTotTotal2 / decTotTotal7) + (decTotTotal3 / decTotTotal7) + (decTotTotal4 / decTotTotal7) + (decTotTotal5 / decTotTotal7) + (decTotTotal6 / decTotTotal7), 2)) & "</font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "</table>"

                divmostrar.InnerHtml = strHtmlmostrar
                divinforme.InnerHtml = strHtml
            End If
        End If
        'Catch ex As Exception

        'End Try
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

    Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Informe Cartera Hasta: " & txtFechaInicio.Value

            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim stringWriter As StringWriter = New StringWriter()
            Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
            divinforme.RenderControl(htmlTextWriter)
            Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
            Dim Doc As Document = New Document(PageSize.A3.Rotate, 5.0F, 5.0F, 5.0F, 0.0F)
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
