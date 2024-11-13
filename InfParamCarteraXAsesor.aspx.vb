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
Partial Class InfParamCarteraXAsesor
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones

    Private Sub InfParamCarteraXAsesor_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(24, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtase, dtgen As New DataTable

        csoper.LlenarDropDownList("nombrecompleto", "nit", "generadores_mostrar_todos", dtgen, cbogeneradores)
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

            If cboTipo.SelectedValue = 1 Then
                intAño = Year(Now)
                strTipo = "Cartera Año: " & intAño
            Else
                If cboTipo.SelectedValue = 2 Then
                    intAño = Year(Now) - 1
                    strTipo = "Cartera Año: " & intAño
                Else
                    If cboTipo.SelectedValue = 3 Then
                        intAño = Year(Now) - 2
                        strTipo = "Cartera Año: " & intAño
                    Else
                        If cboTipo.SelectedValue = 4 Then
                            intAño = Year(Now)
                            strTipo = "Cartera (a 29 días) Año: " & intAño
                        Else
                            If cboTipo.SelectedValue = 5 Then
                                intAño = Year(Now)
                                strTipo = "Cartera (Entre 30 y 59 dias) Año: " & intAño
                            Else
                                If cboTipo.SelectedValue = 6 Then
                                    intAño = Year(Now)
                                    strTipo = "Cartera (Entre 60 y 79 dias) Año: " & intAño
                                Else
                                    If cboTipo.SelectedValue = 7 Then
                                        intAño = Year(Now)
                                        strTipo = "Cartera (Mas de 80 dias) Año: " & intAño
                                    Else
                                        If cboTipo.SelectedValue = 8 Then
                                            intAño = Year(Now)
                                            strTipo = "Cartera Completa "
                                        Else
                                            If cboTipo.SelectedValue = 9 Then
                                                strTipo = "Cartera Juridica"
                                            Else
                                                strTipo = "Cartera Años: " & Year(Now) - 1 & " y " & Year(Now)
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            strSQL = "Select "
            strSQL += " vc.generador_id,"
            strSQL += " vc.venta_id,"
            strSQL += " vc.generador_nombre as Generador,"
            strSQL += " vc.generador_documento as Documento,"
            strSQL += " vc.venta_plazo As Plazo,"
            strSQL += " vc.generador_telefonos as Telefonos,"
            strSQL += " vc.generador_direccion As Direccion,"
            strSQL += " zo.zona as zona,"
            strSQL += " COALESCE(vc.asesor_id, 0) As idas,"
            If Session("empresa") = "Refrilogistica" Then
                strSQL += " COALESCE(vc.asesor_nombre, COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " FROM generadores_asesores "
                strSQL += " LEFT JOIN usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios"
                strSQL += " LEFT JOIN terceros ON usuarios.idterceros = terceros.idterceros"
                strSQL += " WHERE terceros.documento = vc.generador_documento AND generadores_asesores.idel = 0 AND usuarios.idel = 0 LIMIT 1), 'JULIANA  GIRALDO CASTILLO')) as Asesor,"
            Else
                strSQL += " COALESCE(vc.asesor_nombre, COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
                strSQL += " FROM generadores_asesores "
                strSQL += " LEFT JOIN usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios"
                strSQL += " LEFT JOIN terceros ON usuarios.idterceros = terceros.idterceros"
                strSQL += " WHERE terceros.documento = vc.generador_documento AND generadores_asesores.idel = 0 AND usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')) as Asesor,"
            End If
            strSQL += " vc.sucursal_origina_descripcion As Sucursal,"
            strSQL += " vc.venta_numero as NroFac,"
            strSQL += " DATE_FORMAT(vc.venta_fecha,'%Y-%m-%d') as Fecha,"
            strSQL += " DATE_FORMAT(vc.venta_vence,'%Y-%m-%d') as Vence,"
            strSQL += " vc.venta_total As Total,"
            strSQL += " vc.venta_abonos as Abono,"
            strSQL += " (vc.venta_total - vc.venta_abonos) As Saldo,"
            strSQL += " (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence)) as DiasVence,"
            strSQL += " (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo as DiasFac,"
            strSQL += " COALESCE(gena.usuarios_idusuarios, 1) As us_asesor,"
            If Session("empresa") = "Refrilogistica" Then
                strSQL += " COALESCE((SELECT CONCAT(te.nombre1, ' ', te.nombre2, ' ', te.apellido1, ' ', te.apellido2) FROM generadores_asesores "
                strSQL += " Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios"
                strSQL += " Left Join terceros te ON usuarios.idterceros = te.idterceros"
                strSQL += " WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
                strSQL += " ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 'JULIANA  GIRALDO CASTILLO') AS ase_ofi"
            Else
                strSQL += " COALESCE((SELECT CONCAT(te.nombre1, ' ', te.nombre2, ' ', te.apellido1, ' ', te.apellido2) FROM generadores_asesores "
                strSQL += " Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios"
                strSQL += " Left Join terceros te ON usuarios.idterceros = te.idterceros"
                strSQL += " WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
                strSQL += " ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 'TERCERO ESPECIAL') AS ase_ofi"
            End If
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

            If cboTipo.SelectedValue = 1 Then
                strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) And YEAR(vc.venta_fecha) = YEAR(NOW())"
            Else
                If cboTipo.SelectedValue = 2 Then
                    strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) And YEAR(vc.venta_fecha) = (YEAR(NOW()) - 1)"
                Else
                    If cboTipo.SelectedValue = 3 Then
                        strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) And YEAR(vc.venta_fecha) = (YEAR(NOW()) - 2)"
                    Else
                        If cboTipo.SelectedValue = 4 Then
                            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo < 30 And YEAR(vc.venta_fecha) = YEAR(NOW())"
                        Else
                            If cboTipo.SelectedValue = 5 Then
                                strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo BETWEEN 30 AND 59 And YEAR(vc.venta_fecha) = YEAR(NOW())"
                            Else
                                If cboTipo.SelectedValue = 6 Then
                                    strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo BETWEEN 60 AND 79 And YEAR(vc.venta_fecha) = YEAR(NOW())"
                                Else
                                    If cboTipo.SelectedValue = 7 Then
                                        strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) AND (SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo >= 80 And YEAR(vc.venta_fecha) = YEAR(NOW())"
                                    Else
                                        If cboTipo.SelectedValue = 8 Then
                                            strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY)))"
                                        Else
                                            If cboTipo.SelectedValue = 9 Then
                                                strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) And YEAR(vc.venta_fecha) <= (YEAR(NOW()) - 2)"
                                            Else
                                                strSQL += " AND vc.venta_vence<=(SELECT( ADDDATE('" & txtFechaInicio.Value & "',INTERVAL vc.venta_plazo DAY))) And (YEAR(vc.venta_fecha) = (YEAR(NOW()) - 1) Or YEAR(vc.venta_fecha) = YEAR(NOW()))"
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            If cboasesores.SelectedValue > 0 Then
                strSQL += " AND COALESCE((SELECT generadores_asesores.usuarios_idusuarios FROM generadores_asesores "
                strSQL += " Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
                strSQL += " WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
                strSQL += " ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 0) = " & cboasesores.SelectedValue & ""
            End If

            If cbogeneradores.SelectedValue > 0 Then
                strSQL += " AND vc.generador_documento = " & cbogeneradores.SelectedValue & ""
            End If

            strSQL += " ORDER BY ase_ofi,Generador,(SELECT DATEDIFF(DATE(NOW()),vc.venta_vence))+vc.venta_plazo DESC"

            dtter = csinformes.ejecutar_query_bd(strSQL)

                If dtter.Rows.Count > 0 Then
                    Dim pathimgCabeza1 As String
                    Dim urlFotoCabeza1 As String = ""
                    Dim strAsesorActual, strGeneradorActual As String
                    Dim decTotalFacAse, decTotalFacGen, decTotalAboAse, decTotalAboGen, decTotalSalAse, decTotalSalGen, decTotalFac, decTotalAbo, decTotalSal As Decimal
                    decTotalFacAse = 0
                    decTotalAboAse = 0
                    decTotalSalAse = 0
                    decTotalFacGen = 0
                    decTotalAboGen = 0
                    decTotalSalGen = 0
                    decTotalFac = 0
                    decTotalAbo = 0
                    decTotalSal = 0

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

                        strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='9'><b><font size='15px'>Cartera Por Asesor: " & dtter.Rows(0)("ase_ofi").ToString & "</font></b></td>"
                        strHtml += "</tr>"
                        strHtml += "<tr>"
                strHtml += "<td align='left' colspan='9'><b><font size='14px'>" & dtter.Rows(0)("Generador").ToString & " - " & dtter.Rows(0)("Documento").ToString & " - " & dtter.Rows(0)("Telefonos").ToString & " - " & dtter.Rows(0)("zona").ToString & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='8px'></font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Factura</font></b></td>"
                        strHtml += "<td align='left'><b><font size='12px'>Sucursal</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Fecha</font></b></td>"
                        strHtml += "<td align='center'><b><font size='12px'>Vence</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>Vr.Factura</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>Vr.Abono</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>Vr.Saldo</font></b></td>"
                        strHtml += "<td align='right'><b><font size='12px'>Días</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                        strHtmlmostrar += "<tr>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Asesor</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Generador</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Factura</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>Sucursal</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>Vence</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>Vr.Factura</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>Vr.Abono</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>Vr.Saldo</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>Días</font></b></td>"
                        strHtmlmostrar += "</tr>"

                        strAsesorActual = dtter.Rows(0)("ase_ofi").ToString
                        strGeneradorActual = dtter.Rows(0)("Generador").ToString
                        For i As Integer = 0 To dtter.Rows.Count - 1
                            If i Mod 2 = 0 Then
                                color = "#FFFFFF"
                            Else
                                color = "#EFF3FB"
                            End If

                            If strAsesorActual = dtter.Rows(i)("ase_ofi").ToString Then
                                If strGeneradorActual = dtter.Rows(i)("Generador").ToString Then
                                    strHtml += "<tr>"

                                    strHtml += "<td align='left'><font size='8px'>" & dtter.Rows(i)("Asesor").ToString.Substring(0, 7) & "</font></td>"
                                    strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("NroFac").ToString & "</font></td>"
                                    strHtml += "<td align='left'><font size='12px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                                    strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                                    strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Vence").ToString & "</font></td>"
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Abono")) & "</font></td>"
                                    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                    strHtml += "<td align='right'><font size='12px'>" & dtter.Rows(i)("DiasFac").ToString & "</font></td>"

                                    strHtml += "</tr>"

                                    decTotalFacGen = decTotalFacGen + dtter.Rows(i)("Total")
                                    decTotalAboGen = decTotalAboGen + dtter.Rows(i)("Abono")
                                    decTotalSalGen = decTotalSalGen + dtter.Rows(i)("Saldo")

                                    decTotalFacAse = decTotalFacAse + dtter.Rows(i)("Total")
                                    decTotalAboAse = decTotalAboAse + dtter.Rows(i)("Abono")
                                    decTotalSalAse = decTotalSalAse + dtter.Rows(i)("Saldo")
                                Else
                                    strHtml += "<tr>"
                                    strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL GENERADOR " & strGeneradorActual & ": </font></b></td>"
                                    strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacGen) & "</font></b></td>"
                                    strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboGen) & "</font></b></td>"
                                    strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalGen) & "</font></b></td>"
                                    strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                                    strHtml += "</tr>"

                                    decTotalFacGen = 0
                                    decTotalAboGen = 0
                                    decTotalSalGen = 0

                                    strGeneradorActual = dtter.Rows(i)("Generador").ToString

                                    strHtml += "<tr>"
                            strHtml += "<td align='left' colspan='9'><b><font size='12px'>" & dtter.Rows(i)("Generador").ToString & " - " & dtter.Rows(i)("Documento").ToString & " - " & dtter.Rows(i)("Telefonos").ToString & " - " & dtter.Rows(i)("zona").ToString & "</font></b></td>"
                            strHtml += "</tr>"

                                    If strAsesorActual = dtter.Rows(i)("ase_ofi").ToString Then
                                        strHtml += "<tr>"

                                        strHtml += "<td align='left'><font size='8px'>" & dtter.Rows(i)("Asesor").ToString.Substring(0, 7) & "</font></td>"
                                        strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("NroFac").ToString & "</font></td>"
                                        strHtml += "<td align='left'><font size='12px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                                        strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                                        strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Vence").ToString & "</font></td>"
                                        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                                        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Abono")) & "</font></td>"
                                        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                        strHtml += "<td align='right'><font size='12px'>" & dtter.Rows(i)("DiasFac").ToString & "</font></td>"

                                        strHtml += "</tr>"

                                        decTotalFacGen = decTotalFacGen + dtter.Rows(i)("Total")
                                        decTotalAboGen = decTotalAboGen + dtter.Rows(i)("Abono")
                                        decTotalSalGen = decTotalSalGen + dtter.Rows(i)("Saldo")

                                        decTotalFacAse = decTotalFacAse + dtter.Rows(i)("Total")
                                        decTotalAboAse = decTotalAboAse + dtter.Rows(i)("Abono")
                                        decTotalSalAse = decTotalSalAse + dtter.Rows(i)("Saldo")
                                    Else
                                        strHtml += "<tr>"
                                        strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL ASESOR " & strAsesorActual & ": </font></b></td>"
                                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacAse) & "</font></b></td>"
                                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboAse) & "</font></b></td>"
                                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalAse) & "</font></b></td>"
                                        strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                                        strHtml += "</tr>"

                                        decTotalFacGen = 0
                                        decTotalAboGen = 0
                                        decTotalSalGen = 0

                                        decTotalFacAse = 0
                                        decTotalAboAse = 0
                                        decTotalSalAse = 0

                                        strHtml += "<tr>"
                                        strHtml += "<td align='center' colspan='9'><b><font size='17px'>Cartera Por Asesor: " & dtter.Rows(i)("ase_ofi").ToString & "</font></b></td>"
                                        strHtml += "</tr>"
                                    End If
                                End If
                            Else
                                strHtml += "<tr>"
                                strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL GENERADOR " & strGeneradorActual & ": </font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacGen) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboGen) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalGen) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                                strHtml += "</tr>"

                                decTotalFacGen = 0
                                decTotalAboGen = 0
                                decTotalSalGen = 0

                                strGeneradorActual = dtter.Rows(i)("Generador").ToString

                                strHtml += "<tr>"
                                strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL ASESOR " & strAsesorActual & ": </font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacAse) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboAse) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalAse) & "</font></b></td>"
                                strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                                strHtml += "</tr>"

                                decTotalFacAse = 0
                                decTotalAboAse = 0
                                decTotalSalAse = 0

                                strAsesorActual = dtter.Rows(i)("ase_ofi").ToString

                                strHtml += "<tr>"
                                strHtml += "<td align='center' colspan='9'><b><font size='15px'>Cartera Por Asesor: " & dtter.Rows(i)("ase_ofi").ToString & "</font></b></td>"
                                strHtml += "</tr>"

                                strHtml += "<tr>"
                        strHtml += "<td align='left' colspan='9'><b><font size='14px'>" & dtter.Rows(i)("Generador").ToString & " - " & dtter.Rows(i)("Documento").ToString & " - " & dtter.Rows(i)("Telefonos").ToString & " - " & dtter.Rows(i)("zona").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                                strHtml += "<tr>"

                                strHtml += "<td align='left'><font size='8px'>" & dtter.Rows(i)("Asesor").ToString.Substring(0, 7) & "</font></td>"
                                strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("NroFac").ToString & "</font></td>"
                                strHtml += "<td align='left'><font size='12px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='12px'>" & dtter.Rows(i)("Vence").ToString & "</font></td>"
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Abono")) & "</font></td>"
                                strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                                strHtml += "<td align='right'><font size='12px'>" & dtter.Rows(i)("DiasFac").ToString & "</font></td>"

                                strHtml += "</tr>"

                                decTotalFacGen = decTotalFacGen + dtter.Rows(i)("Total")
                                decTotalAboGen = decTotalAboGen + dtter.Rows(i)("Abono")
                                decTotalSalGen = decTotalSalGen + dtter.Rows(i)("Saldo")

                                decTotalFacAse = decTotalFacAse + dtter.Rows(i)("Total")
                                decTotalAboAse = decTotalAboAse + dtter.Rows(i)("Abono")
                                decTotalSalAse = decTotalSalAse + dtter.Rows(i)("Saldo")
                            End If

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Asesor").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("NroFac").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Vence").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Abono")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("DiasFac").ToString & "</font></td>"

                            strHtmlmostrar += "</tr>"

                            decTotalFac = decTotalFac + dtter.Rows(i)("Total")
                            decTotalAbo = decTotalAbo + dtter.Rows(i)("Abono")
                            decTotalSal = decTotalSal + dtter.Rows(i)("Saldo")
                        Next

                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL GENERADOR " & strGeneradorActual & ": </font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacGen) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboGen) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalGen) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                        strHtml += "</tr>"

                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='5'><b><font size='11px'>TOTAL ASESOR " & strAsesorActual & ": </font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalFacAse) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalAboAse) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'>" & String.Format("{0:c}", decTotalSalAse) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='10px'></font></b></td>"
                        strHtml += "</tr>"

                        strHtml += "<tr>"
                        strHtml += "<td align='center' colspan='2'><b><font size='12px'>TOTAL CARTERA : </font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='11px'>" & String.Format("{0:c}", decTotalFac) & "</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='11px'>" & String.Format("{0:c}", decTotalAbo) & "</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='11px'>" & String.Format("{0:c}", decTotalSal) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='11px'></font></b></td>"
                        strHtml += "</tr>"

                        strHtml += "</table>"

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
            Dim Doc As Document = New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            Dim htmlparser As HTMLWorker = New HTMLWorker(Doc)
            PdfWriter.GetInstance(Doc, Response.OutputStream)
            'Dim wri As PdfWriter = PdfWriter.GetInstance(Doc, Response.OutputStream)
            'wri.PageEvent = New PageEventHelper
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
