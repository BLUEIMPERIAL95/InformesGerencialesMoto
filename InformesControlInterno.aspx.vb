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

Partial Class InformesControlInterno
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim intContMov As Integer

    Private Sub InformesControlInterno_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2050, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnMovimientos_Click(sender As Object, e As EventArgs) Handles btnMovimientos.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim dtCons, dtCont, dtContAnt, dtContAnticipo, dtContSinAnticipo, dtOper, dtOperAnt As New DataTable
                Dim strCon, strCant, strCantAnt, strCantAnticipo, strCantSinFacturados, strCantSinLiq, strHtml, strNomMesAct, strNomMesAnt, strSucuMov As String
                Dim fechamesact, fechamesant, fechamesantfin As DateTime
                strCon = ""
                strCant = ""
                strCantAnt = ""
                strCantAnticipo = ""
                strCantSinFacturados = ""
                strCantSinLiq = ""
                strHtml = ""
                strSucuMov = ""

                divmostrar.InnerHtml = ""

                strCon += "Select MIN(movimientos_transportes_consolidado.movimiento_numero) As min_numero, "
                strCon += "MAX(movimientos_transportes_consolidado.movimiento_numero) As max_numero "
                strCon += "From movimientos_transportes_consolidado "
                strCon += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strCon += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "

                dtCons = csinformes.ejecutar_query_bd(strCon)

                If dtCons.Rows.Count > 0 Then
                    fechamesant = DateAdd(DateInterval.Month, -1, CDate(txtFechaInicio.Value))
                    fechamesantfin = DateAdd(DateInterval.Month, -1, CDate(txtFechaFin.Value))
                    fechamesact = CDate(txtFechaInicio.Value)
                    Dim fechaact, fechaant As New DateTime
                    fechaant = Convert.ToDateTime(fechamesant)
                    fechaact = Convert.ToDateTime(fechamesact)
                    strNomMesAnt = MonthName(fechaant.Month)
                    strNomMesAct = MonthName(fechaact.Month)

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='center'><b><font size='1px'>MOVIMIENTOS</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='center'><b><font size='1px'>DESCRIPCION</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>CANTIDAD</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>%</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>" & strNomMesAnt.ToUpper & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>VARIACION</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='center'><b><font size='1px'>MOVIMIENTOS " & strNomMesAct.ToUpper & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>INI " & dtCons.Rows(0)("min_numero").ToString & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>FIN " & dtCons.Rows(0)("max_numero").ToString & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtml += "</tr>"

                    strCantAnt += "SELECT COUNT(movimientos_transportes.idmovimientos_transportes) As nummov "
                    strCantAnt += "From movimientos_transportes "
                    strCantAnt += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCantAnt += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & fechamesant.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' AND '" & fechamesantfin.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' "
                    'strCantAnt += " And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
                    strCantAnt += "ORDER BY movimientos_transportes.numero "

                    dtContAnt = csinformes.ejecutar_query_bd(strCantAnt)

                    Dim intContMovAnt As Integer
                    If dtContAnt.Rows.Count > 0 Then
                        intContMovAnt = dtContAnt.Rows(0)("nummov")
                    Else
                        intContMovAnt = 0
                    End If

                    strCant += "SELECT COUNT(movimientos_transportes.idmovimientos_transportes) As nummov "
                    strCant += "From movimientos_transportes "
                    strCant += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCant += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    'strCant += " And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
                    strCant += "ORDER BY movimientos_transportes.numero "

                    dtCont = csinformes.ejecutar_query_bd(strCant)

                    If dtCont.Rows.Count > 0 Then
                        intContMov = dtCont.Rows(0)("nummov")
                    Else
                        intContMov = 0
                    End If

                    strCant = ""
                    strCant += "SELECT COUNT(movimientos_transportes.idmovimientos_transportes) As nummov "
                    strCant += "From movimientos_transportes "
                    strCant += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCant += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCant += " And movimientos_transportes.tipo_estados_idtipo_estados = 9 "
                    strCant += "ORDER BY movimientos_transportes.numero "

                    dtCont = csinformes.ejecutar_query_bd(strCant)

                    Dim intContAnu As Integer
                    If dtCont.Rows.Count > 0 Then
                        intContAnu = dtCont.Rows(0)("nummov")
                    Else
                        intContAnu = 0
                    End If

                    strCantAnt = ""
                    strCantAnt += "SELECT COUNT(movimientos_transportes.idmovimientos_transportes) As nummov "
                    strCantAnt += "From movimientos_transportes "
                    strCantAnt += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCantAnt += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & fechamesant.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' AND '" & fechamesantfin.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' "
                    strCantAnt += " And movimientos_transportes.tipo_estados_idtipo_estados = 9 "
                    strCantAnt += "ORDER BY movimientos_transportes.numero "

                    dtContAnt = csinformes.ejecutar_query_bd(strCantAnt)

                    Dim intContMovAnu As Integer
                    If dtContAnt.Rows.Count > 0 Then
                        intContMovAnu = dtContAnt.Rows(0)("nummov")
                    Else
                        intContMovAnu = 0
                    End If

                    Dim intContNac, intContUrb, intContInm, intContEsp, intContItr, intContBar, intContInt,
                        intContNacAnt, intContUrbAnt, intContInmAnt, intContEspAnt, intContItrAnt, intContBarAnt, intContIntAnt As Integer
                    intContNac = 0
                    intContUrb = 0
                    intContInm = 0
                    intContEsp = 0
                    intContItr = 0
                    intContBar = 0
                    intContInt = 0

                    intContNacAnt = 0
                    intContUrbAnt = 0
                    intContInmAnt = 0
                    intContEspAnt = 0
                    intContItrAnt = 0
                    intContBarAnt = 0
                    intContIntAnt = 0

                    strCantAnt = ""
                    strCantAnt += "SELECT tipo_despachos.descripcion "
                    strCantAnt += "From movimientos_transportes "
                    strCantAnt += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCantAnt += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & fechamesant.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' AND '" & fechamesantfin.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) & "' "
                    strCantAnt += " And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
                    strCantAnt += "ORDER BY movimientos_transportes.numero "

                    dtOperAnt = csinformes.ejecutar_query_bd(strCantAnt)

                    Dim strDescOperAnt As String
                    strDescOperAnt = ""
                    If dtOperAnt.Rows.Count > 0 Then
                        For i As Integer = 0 To dtOperAnt.Rows.Count - 1
                            strDescOperAnt = dtOperAnt.Rows(i)("descripcion").ToString

                            If strDescOperAnt.Contains("NACIONAL") Then
                                intContNacAnt = intContNacAnt + 1
                            End If

                            If strDescOperAnt.Contains("URBANO") Then
                                intContUrbAnt = intContUrbAnt + 1
                            End If

                            If strDescOperAnt.Contains("MUNICIPAL") Then
                                intContInmAnt = intContInmAnt + 1
                            End If

                            If strDescOperAnt.Contains("ESPECIAL") Then
                                intContEspAnt = intContEspAnt + 1
                            End If

                            If strDescOperAnt.Contains("ITR") Then
                                intContItrAnt = intContItrAnt + 1
                            End If

                            If strDescOperAnt.Contains("INTERNAC") Then
                                intContIntAnt = intContIntAnt + 1
                            End If
                        Next
                    End If

                    strCant = ""
                    strCant += "SELECT tipo_despachos.descripcion "
                    strCant += "From movimientos_transportes "
                    strCant += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                    strCant += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCant += " And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
                    strCant += "ORDER BY movimientos_transportes.numero "

                    dtOper = csinformes.ejecutar_query_bd(strCant)

                    Dim strDescOper As String
                    strDescOper = ""
                    If dtOper.Rows.Count > 0 Then
                        For i As Integer = 0 To dtOper.Rows.Count - 1
                            strDescOper = dtOper.Rows(i)("descripcion").ToString

                            If strDescOper.Contains("NACIONAL") Then
                                intContNac = intContNac + 1
                            End If

                            If strDescOper.Contains("URBANO") Then
                                intContUrb = intContUrb + 1
                            End If

                            If strDescOper.Contains("MUNICIPAL") Then
                                intContInm = intContInm + 1
                            End If

                            If strDescOper.Contains("ESPECIAL") Then
                                intContEsp = intContEsp + 1
                            End If

                            If strDescOper.Contains("ITR") Then
                                intContItr = intContItr + 1
                            End If

                            If strDescOper.Contains("INTERNAC") Then
                                intContInt = intContInt + 1
                            End If
                        Next
                    End If

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>Nacionales</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContNac & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContNac / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContNacAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContNac - intContNacAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>Urbanos</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContUrb & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContUrb / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContUrbAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContUrb - intContUrbAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>Intermunicipales</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContInm & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContInm / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContInmAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContInm - intContInmAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>Anulados</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContAnu & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContAnu / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMovAnu & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContAnu - intContMovAnu & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>S.Especiales</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContEsp & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContEsp / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContEspAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContEsp - intContEspAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>ITR</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContItr & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContItr / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContItrAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContItr - intContItrAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>Internacionales</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContInt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round((intContInt / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContIntAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContInt - intContIntAnt & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='right'><b><font size='1px'>TOTALES</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMov & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>100%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMovAnt & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMov - intContMovAnt & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"

                    Dim dtContMovAnticipos As New DataTable

                    strCantAnticipo += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) AS nummov "
                    strCantAnticipo += "From movimientos_transportes_consolidado "
                    strCantAnticipo += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCantAnticipo += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                    strCantAnticipo += "AND COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0) > 0"

                    dtContMovAnticipos = csinformes.ejecutar_query_bd(strCantAnticipo)
                    Dim intContMovAnticipos As Integer
                    intContMovAnticipos = 0

                    If dtContMovAnticipos.Rows.Count > 0 Then
                        intContMovAnticipos = dtContMovAnticipos.Rows(0)("nummov")
                    Else
                        dtContAnt.Rows(0)("nummov") = 0
                    End If

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'>SIN ANTICIPO</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMov - intContMovAnticipos & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round(((intContMov - intContMovAnticipos) / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'>CON ANTICIPO</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMovAnticipos & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round(((intContMovAnticipos) / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<br />"

                    Dim dtContSinFact As New DataTable

                    strCantSinFacturados += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSinFac "
                    strCantSinFacturados += "From movimientos_transportes_consolidado "
                    strCantSinFacturados += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                    strCantSinFacturados += "Left Join ventas_detalles ON ventas_control.ventas_detalles_id = ventas_detalles.idventas_detalles "
                    strCantSinFacturados += "Left Join ventas ON ventas_detalles.ventas_idventas = ventas.idventas "
                    strCantSinFacturados += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCantSinFacturados += "And COALESCE(ventas_control.movimientos_transportes_id, 0) <> 0 "
                    strCantSinFacturados += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                    strCantSinFacturados += "And ventas.tipo_estados_idtipo <> 20 "

                    dtContSinFact = csinformes.ejecutar_query_bd(strCantSinFacturados)
                    Dim intContSinFact As Integer

                    If dtContSinFact.Rows.Count > 0 Then
                        intContSinFact = dtContSinFact.Rows(0)("NumSinFac")
                    Else
                        intContSinFact = 0
                    End If

                    If intContSinFact > intContMov Then
                        intContSinFact = intContMov
                    End If

                    Dim dtCantSinLiq As New DataTable

                    strCantSinLiq += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSinLiq, "
                    strCantSinLiq += "SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0)) As ValAntSinLiq, "
                    strCantSinLiq += "SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) As ValFleSinLiq "
                    strCantSinLiq += "From movimientos_transportes_consolidado "
                    strCantSinLiq += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCantSinLiq += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                    strCantSinLiq += "And movimientos_transportes_consolidado.movimiento_estado <> 'CAUSADO' "

                    dtCantSinLiq = csinformes.ejecutar_query_bd(strCantSinLiq)
                    Dim intContSinLiqu, decValAntSinLiq, decValFleSinLiq As Double

                    If dtCantSinLiq.Rows.Count > 0 Then
                        intContSinLiqu = dtCantSinLiq.Rows(0)("NumSinLiq")
                        decValAntSinLiq = dtCantSinLiq.Rows(0)("ValAntSinLiq")
                        decValFleSinLiq = dtCantSinLiq.Rows(0)("ValFleSinLiq")
                    Else
                        intContSinLiqu = 0
                        decValAntSinLiq = 0
                        decValFleSinLiq = 0
                    End If

                    strCantSinFacturados = ""
                    strCantSinFacturados += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSinFac, "
                    strCantSinFacturados += "SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0)) As ValAntSinFac, "
                    strCantSinFacturados += "SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0) + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) As ValFleSinFac "
                    strCantSinFacturados += "From movimientos_transportes_consolidado "
                    strCantSinFacturados += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                    strCantSinFacturados += "Left Join ventas_detalles ON ventas_control.ventas_detalles_id = ventas_detalles.idventas_detalles "
                    strCantSinFacturados += "Left Join ventas ON ventas_detalles.ventas_idventas = ventas.idventas "
                    strCantSinFacturados += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strCantSinFacturados += "And COALESCE(ventas_control.movimientos_transportes_id, 0) = 0 "
                    strCantSinFacturados += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' And movimientos_transportes_consolidado.movimiento_estado <> 'CAUSADO' "
                    'strCantSinFacturados += "And ventas.tipo_estados_idtipo <> 20 "

                    dtContSinFact = csinformes.ejecutar_query_bd(strCantSinFacturados)
                    Dim vlrContSinFact, decValAntSinFact, decValFleSinFact As Double

                    If dtContSinFact.Rows.Count > 0 Then
                        vlrContSinFact = dtContSinFact.Rows(0)("NumSinFac")
                        decValAntSinFact = dtContSinFact.Rows(0)("ValAntSinFac")
                        decValFleSinFact = dtContSinFact.Rows(0)("ValFleSinFac")
                    Else
                        vlrContSinFact = 0
                        decValAntSinFact = 0
                        decValFleSinFact = 0
                    End If

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>ANTICIPO</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>FLETE</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'>SIN FACTURAR</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContMov - intContSinFact & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round(((intContMov - intContSinFact) / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decValAntSinLiq / 1000) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decValFleSinLiq / 1000) & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'>SIN LIQUIDAR</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & intContSinLiqu & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & Math.Round(((intContSinLiqu) / intContMov) * 100, 2) & "%</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decValAntSinFact / 1000) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decValFleSinFact / 1000) & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<br />"

                    strSucuMov = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSuc, "
                    strSucuMov += "movimientos_transportes_consolidado.sucorigina_nombre AS NomSuc "
                    strSucuMov += "From movimientos_transportes_consolidado "
                    strSucuMov += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    'strSucuMov += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                    strSucuMov += "Group BY movimientos_transportes_consolidado.sucorigina_nombre "
                    strSucuMov += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                    Dim dtSuc As New DataTable

                    dtSuc = csinformes.ejecutar_query_bd(strSucuMov)

                    If dtSuc.Rows.Count > 0 Then
                        strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='30%'>"
                        strHtml += "<tr bgcolor='#F8CFCF'>"
                        strHtml += "<td align='left'><b><font size='1px'>SUCURSAL</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>CANTIDAD</font></b></td>"
                        strHtml += "</tr>"

                        For i As Integer = 0 To dtSuc.Rows.Count - 1
                            strHtml += "<tr>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtSuc.Rows(i)("NomSuc") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtSuc.Rows(i)("NumSuc") & "</font></b></td>"
                            strHtml += "</tr>"
                        Next
                    End If

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & intContMov & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='30%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='left'><b><font size='1px'>MES</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>VARIACION</font></b></td>"
                    strHtml += "</tr>"

                    Dim decAnterior As Integer

                    For i As Integer = 1 To Month(Now)
                        strSucuMov = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSuc "
                        strSucuMov += "From movimientos_transportes_consolidado "
                        strSucuMov += "Where MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & i & " "
                        strSucuMov += "And YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(Now) & " "

                        dtSuc = csinformes.ejecutar_query_bd(strSucuMov)

                        If dtSuc.Rows.Count > 0 Then
                            If i = 1 Then
                                strHtml += "<tr>"
                                strHtml += "<td align='left'><b><font size='1px'>" & i & "</font></b></td>"
                                strHtml += "<td align='left'><b><font size='1px'>" & dtSuc.Rows(0)("NumSuc") & "</font></b></td>"
                                strHtml += "<td align='left'><b><font size='1px'>0</font></b></td>"
                                strHtml += "</tr>"
                                decAnterior = dtSuc.Rows(0)("NumSuc")
                            Else
                                strHtml += "<tr>"
                                strHtml += "<td align='left'><b><font size='1px'>" & i & "</font></b></td>"
                                strHtml += "<td align='left'><b><font size='1px'>" & dtSuc.Rows(0)("NumSuc") & "</font></b></td>"
                                strHtml += "<td align='left'><b><font size='1px'>" & dtSuc.Rows(0)("NumSuc") - decAnterior & "</font></b></td>"
                                strHtml += "</tr>"
                                decAnterior = dtSuc.Rows(0)("NumSuc")
                            End If
                        End If
                    Next

                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>TOTAL: </font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & intContMov & "</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"

                    strHtml += "<Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                    strHtml += "<tr bgcolor='#F8CFCF'>"
                    strHtml += "<td align='center'><b><font size='1px'>FIN MOVIMIENTOS</font></b></td>"
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
            Response.AddHeader("Content-Disposition", "attachment; filename=Movimientos.xls")
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

    Private Sub btnOperacion_Click(sender As Object, e As EventArgs) Handles btnOperacion.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strAcumDes, strHtml As String
                Dim dtAcumDes As New DataTable
                Dim ContMovGen As Integer
                strAcumDes = ""
                strHtml = ""

                divmostrar.InnerHtml = ""

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#E0E0F7'>"
                strHtml += "<td align='center'><b><font size='1px'>OPERACION</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strAcumDes = "Select COALESCE(movimientos_transportes_consolidado.despachador_nombre, 'NA') AS Despachador, "
                strAcumDes += "COUNT(movimientos_transportes_consolidado.movimiento_id) AS NumMov "
                strAcumDes += "From movimientos_transportes_consolidado "
                strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strAcumDes += "And (movimientos_transportes_consolidado.movimiento_operacion Like '%NACIONAL%' Or movimientos_transportes_consolidado.movimiento_operacion Like '%INTERMUN%' Or movimientos_transportes_consolidado.movimiento_operacion Like '%URBANO%') "
                strAcumDes += "Group BY movimientos_transportes_consolidado.despachador_nombre "
                strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAcumDes = csinformes.ejecutar_query_bd(strAcumDes)

                If dtAcumDes.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='center' colspan='3'><b><font size='1px'>ACUMULADO</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left'><b><font size='1px'>POS</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>DESPACHADOR</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "</tr>"

                    ContMovGen = 0
                    For i As Integer = 0 To dtAcumDes.Rows.Count - 1
                        If i < 5 Then
                            strHtml += "<tr bgcolor='#B1D1A0'>"
                        Else
                            If i < 10 Then
                                strHtml += "<tr bgcolor='#E3CA9E'>"
                            Else
                                strHtml += "<tr>"
                            End If
                        End If
                        strHtml += "<td align='left'><b><font size='1px'>" & i + 1 & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("Despachador").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NumMov").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                        ContMovGen = ContMovGen + dtAcumDes.Rows(i)("NumMov")
                    Next

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>TOTAL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & ContMovGen & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strAcumDes = "Select movimientos_transportes_consolidado.despachador_nombre AS Despachador,  "
                strAcumDes += "COUNT(movimientos_transportes_consolidado.movimiento_id) AS NumMov "
                strAcumDes += "From movimientos_transportes_consolidado "
                strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strAcumDes += "And movimientos_transportes_consolidado.movimiento_operacion Like '%NACIONAL%' "
                strAcumDes += "Group BY movimientos_transportes_consolidado.despachador_nombre "
                strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAcumDes = csinformes.ejecutar_query_bd(strAcumDes)

                If dtAcumDes.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='center' colspan='3'><b><font size='1px'>NACIONALES</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left'><b><font size='1px'>POS</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>DESPACHADOR</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContMov As Integer
                    ContMov = 0
                    For i As Integer = 0 To dtAcumDes.Rows.Count - 1
                        If i < 5 Then
                            strHtml += "<tr bgcolor='#B1D1A0'>"
                        Else
                            If i < 10 Then
                                strHtml += "<tr bgcolor='#E3CA9E'>"
                            Else
                                strHtml += "<tr>"
                            End If
                        End If
                        strHtml += "<td align='left'><b><font size='1px'>" & i + 1 & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("Despachador").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NumMov").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                        ContMov = ContMov + dtAcumDes.Rows(i)("NumMov")
                    Next

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>TOTAL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & ContMov & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strAcumDes = "Select movimientos_transportes_consolidado.despachador_nombre AS Despachador,  "
                strAcumDes += "COUNT(movimientos_transportes_consolidado.movimiento_id) AS NumMov "
                strAcumDes += "From movimientos_transportes_consolidado "
                strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strAcumDes += "And movimientos_transportes_consolidado.movimiento_operacion Like '%INTERMUN%' "
                strAcumDes += "Group BY movimientos_transportes_consolidado.despachador_nombre "
                strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAcumDes = csinformes.ejecutar_query_bd(strAcumDes)

                If dtAcumDes.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='center' colspan='3'><b><font size='1px'>INTERMUNICIPALES</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left'><b><font size='1px'>POS</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>DESPACHADOR</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContMov As Integer
                    ContMov = 0
                    For i As Integer = 0 To dtAcumDes.Rows.Count - 1
                        If i < 5 Then
                            strHtml += "<tr bgcolor='#B1D1A0'>"
                        Else
                            If i < 10 Then
                                strHtml += "<tr bgcolor='#E3CA9E'>"
                            Else
                                strHtml += "<tr>"
                            End If
                        End If
                        strHtml += "<td align='left'><b><font size='1px'>" & i + 1 & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("Despachador").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NumMov").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                        ContMov = ContMov + dtAcumDes.Rows(i)("NumMov")
                    Next

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>TOTAL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & ContMov & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strAcumDes = "Select movimientos_transportes_consolidado.despachador_nombre AS Despachador,  "
                strAcumDes += "COUNT(movimientos_transportes_consolidado.movimiento_id) AS NumMov "
                strAcumDes += "From movimientos_transportes_consolidado "
                strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strAcumDes += "And movimientos_transportes_consolidado.movimiento_operacion Like '%URBANO%' "
                strAcumDes += "Group BY movimientos_transportes_consolidado.despachador_nombre "
                strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAcumDes = csinformes.ejecutar_query_bd(strAcumDes)

                If dtAcumDes.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='center' colspan='3'><b><font size='1px'>URBANOS</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left'><b><font size='1px'>POS</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>DESPACHADOR</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContMov As Integer
                    ContMov = 0
                    For i As Integer = 0 To dtAcumDes.Rows.Count - 1
                        If i < 5 Then
                            strHtml += "<tr bgcolor='#B1D1A0'>"
                        Else
                            If i < 10 Then
                                strHtml += "<tr bgcolor='#E3CA9E'>"
                            Else
                                strHtml += "<tr>"
                            End If
                        End If
                        strHtml += "<td align='left'><b><font size='1px'>" & i + 1 & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("Despachador").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NumMov").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                        ContMov = ContMov + dtAcumDes.Rows(i)("NumMov")
                    Next

                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='left' colspan='2'><b><font size='1px'>TOTAL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & ContMov & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strAcumDes = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSuc, "
                strAcumDes += "movimientos_transportes_consolidado.sucorigina_nombre AS NomSuc "
                strAcumDes += "From movimientos_transportes_consolidado "
                strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strAcumDes += "AND movimientos_transportes_consolidado.sucorigina_nombre <> 'BARRANCA DE UPIA' "
                strAcumDes += "Group BY movimientos_transportes_consolidado.sucorigina_nombre "
                strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAcumDes = csinformes.ejecutar_query_bd(strAcumDes)

                If dtAcumDes.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                    strHtml += "<tr bgcolor='#E0E0F7'>"
                    strHtml += "<td align='center' colspan='2'><b><font size='1px'>POR AGENCIA</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContMovSuc As Integer
                    ContMovSuc = 0
                    For i As Integer = 0 To dtAcumDes.Rows.Count - 1
                        strHtml += "<tr bgcolor='#E0E0F7'>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NomSuc").ToString & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDes.Rows(i)("NumSuc").ToString & "</font></b></td>"
                        strHtml += "</tr>"

                        strAcumDes = "Select COALESCE(movimientos_transportes_consolidado.despachador_nombre, 'NA') AS Despachador, "
                        strAcumDes += "COUNT(movimientos_transportes_consolidado.movimiento_id) AS NumMov "
                        strAcumDes += "From movimientos_transportes_consolidado "
                        strAcumDes += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                        strAcumDes += "And (movimientos_transportes_consolidado.movimiento_operacion Like '%NACIONAL%' Or movimientos_transportes_consolidado.movimiento_operacion Like '%INTERMUN%' Or movimientos_transportes_consolidado.movimiento_operacion Like '%URBANO%') "
                        strAcumDes += "AND movimientos_transportes_consolidado.sucorigina_nombre = '" & dtAcumDes.Rows(i)("NomSuc").ToString & "' "
                        strAcumDes += "Group BY movimientos_transportes_consolidado.despachador_nombre "
                        strAcumDes += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                        Dim dtAcumDesSuc As New DataTable

                        dtAcumDesSuc = csinformes.ejecutar_query_bd(strAcumDes)

                        If dtAcumDesSuc.Rows.Count > 0 Then
                            For J As Integer = 0 To dtAcumDesSuc.Rows.Count - 1
                                strHtml += "<tr>"
                                strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDesSuc.Rows(J)("Despachador").ToString & "</font></b></td>"
                                strHtml += "<td align='left'><b><font size='1px'>" & dtAcumDesSuc.Rows(J)("NumMov").ToString & "</font></b></td>"
                                strHtml += "</tr>"
                            Next
                        End If

                        ContMovSuc = ContMovSuc + dtAcumDes.Rows(i)("NumSuc").ToString
                    Next
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='1px'>TOTAL</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & ContMovGen & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strHtml += "<Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#E0E0F7'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN OPERACION</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnAnulados_Click(sender As Object, e As EventArgs) Handles btnAnulados.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml, strHtmlPor, strCant As String
                Dim intContMovAnu As Integer
                strCant = ""
                Dim dtCont As New DataTable

                divmostrar.InnerHtml = ""

                strCant += "SELECT COUNT(movimientos_transportes.idmovimientos_transportes) As nummov "
                strCant += "From movimientos_transportes "
                strCant += "Left Join tipo_despachos ON movimientos_transportes.tipo_despachos_idtipo_despachos = tipo_despachos.idtipo_despachos "
                strCant += "WHERE movimientos_transportes.fecha_movimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                'strCant += " And movimientos_transportes.tipo_estados_idtipo_estados <> 9 "
                strCant += "ORDER BY movimientos_transportes.numero "

                dtCont = csinformes.ejecutar_query_bd(strCant)

                If dtCont.Rows.Count > 0 Then
                    intContMovAnu = dtCont.Rows(0)("nummov")
                Else
                    intContMovAnu = 0
                End If

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9DFC2'>"
                strHtml += "<td align='center'><b><font size='1px'>ANULADOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9DFC2'>"
                strHtml += "<td align='center'><b><font size='1px'>" & Year(txtFechaFin.Value) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9DFC2'>"
                strHtml += "<td align='center'><b><font size='1px'>DESPACHADOR</font></b></td>"

                For i As Integer = 1 To Month(txtFechaFin.Value)
                    strHtml += "<td align='center'><b><font size='1px'>" & i & "</font></b></td>"
                Next

                strHtml += "<td align='center'><b><font size='1px'>Totales</font></b></td>"

                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL MES</font></b></td>"

                Dim strAnuMes, strTotMes As String
                Dim intTotMes, intTotMesAnu, intTotalMes As Integer
                Dim intTotMesPor As Decimal

                intTotalMes = 0
                intTotMes = 0
                intTotMesPor = 0
                intTotMesAnu = 0

                strHtmlPor = ""
                For i As Integer = 1 To Month(txtFechaFin.Value)
                    Dim dtTotMes As New DataTable

                    strTotMes = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumMov "
                    strTotMes += "From movimientos_transportes_consolidado "
                    strTotMes += "Where MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & i & " "
                    strTotMes += "And YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(txtFechaFin.Value) & " "
                    'strTotMes += "And movimientos_transportes_consolidado.movimiento_estado = 'ANULADO' "

                    dtTotMes = csinformes.ejecutar_query_bd(strTotMes)

                    If dtTotMes.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtTotMes.Rows(0)("NumMov") & "</font></b></td>"

                        intTotMes = intTotMes + dtTotMes.Rows(0)("NumMov")
                    End If
                Next

                strHtml += "<td align='center'><b><font size='1px'>" & intTotMes & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL ANU MES</font></b></td>"

                For i As Integer = 1 To Month(txtFechaFin.Value)
                    Dim dtAnuMes As New DataTable

                    strAnuMes = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumMov "
                    strAnuMes += "From movimientos_transportes_consolidado "
                    strAnuMes += "Where MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & i & " "
                    strAnuMes += "And YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(txtFechaFin.Value) & " "
                    strAnuMes += "And movimientos_transportes_consolidado.movimiento_estado = 'ANULADO' "

                    dtAnuMes = csinformes.ejecutar_query_bd(strAnuMes)

                    If dtAnuMes.Rows.Count > 0 Then
                        Dim dtTotMes As New DataTable

                        strTotMes = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumMov "
                        strTotMes += "From movimientos_transportes_consolidado "
                        strTotMes += "Where MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & i & " "
                        strTotMes += "And YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(txtFechaFin.Value) & " "
                        'strTotMes += "And movimientos_transportes_consolidado.movimiento_estado = 'ANULADO' "

                        dtTotMes = csinformes.ejecutar_query_bd(strTotMes)

                        strHtml += "<td align='center'><b><font size='1px'>" & dtAnuMes.Rows(0)("NumMov") & "</font></b></td>"
                        strHtmlPor += "<td align='center'><b><font size='1px'>" & Math.Round((dtAnuMes.Rows(0)("NumMov") / dtTotMes.Rows(0)("NumMov")) * 100, 2) & "%</font></b></td>"

                        intTotMesAnu = intTotMesAnu + dtAnuMes.Rows(0)("NumMov")

                        intTotMesPor = intTotMesPor + Math.Round((dtAnuMes.Rows(0)("NumMov") / dtTotMes.Rows(0)("NumMov")) * 100, 2)
                    End If
                Next

                intTotMesPor = Math.Round((intTotMesAnu / intTotMes) * 100, 2)

                strHtml += "<td align='center'><b><font size='1px'>" & intTotMesAnu & "</font></b></td>"

                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL %</font></b></td>"
                strHtml += strHtmlPor
                strHtml += "<td align='center'><b><font size='1px'>" & intTotMesPor & "%</font></b></td>"
                strHtml += "</tr>"

                Dim strAnuAño As String
                Dim dtAnuAño As New DataTable

                strAnuAño = ""
                strAnuAño = "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumMov, "
                'strAnuAño += "movimientos_transportes_consolidado.despachador_id, "
                strAnuAño += "movimientos_transportes_consolidado.despachador_nombre "
                strAnuAño += "From movimientos_transportes_consolidado "
                strAnuAño += "WHERE YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(txtFechaFin.Value) & " "
                strAnuAño += "And movimientos_transportes_consolidado.movimiento_estado = 'ANULADO' "
                'strAnuAño += "GROUP BY movimientos_transportes_consolidado.despachador_id, "
                strAnuAño += "GROUP BY movimientos_transportes_consolidado.despachador_nombre "
                strAnuAño += "HAVING COUNT(movimientos_transportes_consolidado.movimiento_id) > 0 "
                strAnuAño += "ORDER BY COUNT(movimientos_transportes_consolidado.movimiento_id) DESC "

                dtAnuAño = csinformes.ejecutar_query_bd(strAnuAño)

                If dtAnuAño.Rows.Count > 0 Then
                    For i As Integer = 0 To dtAnuAño.Rows.Count - 1
                        If i < 5 Then
                            strHtml += "<tr bgcolor='#E0E0F7'>"
                        Else
                            strHtml += "<tr>"
                        End If

                        Dim intTotAnuDes As Integer
                        intTotAnuDes = 0
                        For J As Integer = 1 To Month(txtFechaFin.Value)
                            Dim strAnuAñoMes As String
                            Dim dtAnuAñoMes As New DataTable

                            strAnuAñoMes = ""
                            strAnuAñoMes = "Select COALESCE(COUNT(movimientos_transportes_consolidado.movimiento_id), 0) As NumMov, "
                            strAnuAñoMes += "movimientos_transportes_consolidado.despachador_nombre "
                            strAnuAñoMes += "From movimientos_transportes_consolidado "
                            strAnuAñoMes += "Where MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & J & " "
                            strAnuAñoMes += "And YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & Year(txtFechaFin.Value) & " "
                            strAnuAñoMes += "And movimientos_transportes_consolidado.movimiento_estado = 'ANULADO' "
                            strAnuAñoMes += "And movimientos_transportes_consolidado.despachador_nombre =  '" & dtAnuAño.Rows(i)("despachador_nombre") & "' "
                            strAnuAñoMes += "GROUP BY movimientos_transportes_consolidado.despachador_nombre "

                            dtAnuAñoMes = csinformes.ejecutar_query_bd(strAnuAñoMes)

                            If dtAnuAñoMes.Rows.Count > 0 Then
                                If J = 1 Then
                                    strHtml += "<td align='center'><b><font size='1px'>" & dtAnuAñoMes.Rows(0)("despachador_nombre").ToString & "</font></b></td>"
                                    strHtml += "<td align='center'><b><font size='1px'>" & dtAnuAñoMes.Rows(0)("NumMov") & "</font></b></td>"
                                Else
                                    strHtml += "<td align='center'><b><font size='1px'>" & dtAnuAñoMes.Rows(0)("NumMov") & "</font></b></td>"
                                End If

                                intTotAnuDes = intTotAnuDes + dtAnuAñoMes.Rows(0)("NumMov")
                            Else
                                If J = 1 Then
                                    strHtml += "<td align='center'><b><font size='1px'>" & dtAnuAño.Rows(i)("despachador_nombre").ToString & "</font></b></td>"
                                    strHtml += "<td align='center'><b><font size='1px'>0</font></b></td>"
                                Else
                                    strHtml += "<td align='center'><b><font size='1px'>0</font></b></td>"
                                End If

                                intTotAnuDes = intTotAnuDes + 0
                            End If
                        Next

                        strHtml += "<td align='center'><b><font size='1px'>" & intTotAnuDes & "</font></b></td>"

                        strHtml += "</tr>"
                    Next
                End If

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9DFC2'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN ANULADOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSinFacturar_Click(sender As Object, e As EventArgs) Handles btnSinFacturar.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#9398F9'>"
                strHtml += "<td align='center'><b><font size='1px'>SIN FACTURAR</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"
                strHtml += "<tr bgcolor='#9398F9'>"
                strHtml += "<td align='center' colspan='3'><b><font size='1px'>SIN FACTURAR</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr bgcolor='#9398F9'>"
                strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(Month(txtFechaInicio.Value)) & " - " & Year(txtFechaInicio.Value) & "</font></b></td>"
                If Month(txtFechaInicio.Value) = 1 Then
                    strHtml += "<td align='center'><b><font size='1px'>Diciembre - " & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                Else
                    strHtml += "<td align='center'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(Month(txtFechaInicio.Value) - 1) & " - " & Year(txtFechaInicio.Value) & "</font></b></td>"
                End If
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='1px'>SIN FACTURAR</font></b></td>"

                Dim strSinFacturar, strSinFacturarMesAnt, strConAnticipo, strConAnticipoMesAnt As String
                strSinFacturar = ""
                strSinFacturarMesAnt = ""
                strConAnticipo = ""
                strConAnticipoMesAnt = ""

                strSinFacturar += "SELECT COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSinFac, "
                strSinFacturar += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0)), 0) As ValAntSinFac "
                strSinFacturar += "From movimientos_transportes_consolidado "
                strSinFacturar += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strSinFacturar += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSinFacturar += "And COALESCE(ventas_control.idel, 0) = 0 "
                strSinFacturar += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strSinFacturar += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "

                Dim dtSinFact As New DataTable

                dtSinFact = csinformes.ejecutar_query_bd(strSinFacturar)

                If dtSinFact.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtSinFact.Rows(0)("NumSinFac") & "</font></b></td>"
                End If

                strSinFacturarMesAnt += "SELECT COUNT(movimientos_transportes_consolidado.movimiento_id) As NumSinFac, "
                strSinFacturarMesAnt += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0)), 0) As ValAntSinFac "
                strSinFacturarMesAnt += "From movimientos_transportes_consolidado "
                strSinFacturarMesAnt += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strSinFacturarMesAnt += "WHERE MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & Month(txtFechaFin.Value) - 1 & " AND YEAR(" & txtFechaFin.Value & ") "
                strSinFacturarMesAnt += "And COALESCE(ventas_control.idel, 0) = 0 "
                strSinFacturarMesAnt += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strSinFacturarMesAnt += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "

                Dim dtSinFacMesAnt As New DataTable

                dtSinFacMesAnt = csinformes.ejecutar_query_bd(strSinFacturarMesAnt)

                If dtSinFacMesAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtSinFacMesAnt.Rows(0)("NumSinFac") & "</font></b></td>"
                End If

                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='1px'>CON ANTICIPO</font></b></td>"

                strConAnticipo += "SELECT COUNT(movimientos_transportes_consolidado.movimiento_id) As NumConAnt "
                strConAnticipo += "From movimientos_transportes_consolidado "
                strConAnticipo += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strConAnticipo += "WHERE movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strConAnticipo += "And COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0) > 0 "
                strConAnticipo += "And COALESCE(ventas_control.idel, 0) = 0 "
                strConAnticipo += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strConAnticipo += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "

                Dim dtContAnt As New DataTable

                dtContAnt = csinformes.ejecutar_query_bd(strConAnticipo)

                If dtContAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtContAnt.Rows(0)("NumConAnt") & "</font></b></td>"
                End If

                strConAnticipoMesAnt += "SELECT COUNT(movimientos_transportes_consolidado.movimiento_id) As NumConAnt "
                strConAnticipoMesAnt += "From movimientos_transportes_consolidado "
                strConAnticipoMesAnt += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strConAnticipoMesAnt += "WHERE MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & Month(txtFechaFin.Value) - 1 & " AND YEAR(" & txtFechaFin.Value & ") "
                strConAnticipoMesAnt += "And COALESCE(movimientos_transportes_consolidado.movimiento_anticipos, 0) > 0 "
                strConAnticipoMesAnt += "And COALESCE(ventas_control.idel, 0) = 0 "
                strConAnticipoMesAnt += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strConAnticipoMesAnt += "And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "

                Dim dtContAntMesAnt As New DataTable

                dtContAntMesAnt = csinformes.ejecutar_query_bd(strConAnticipoMesAnt)

                If dtContAntMesAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtContAntMesAnt.Rows(0)("NumConAnt") & "</font></b></td>"
                End If

                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='1px'>LIQUIDADOS</font></b></td>"

                Dim strCantLiq As String
                strCantLiq = ""

                strCantLiq += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumLiq, "
                strCantLiq += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0)), 0) As ValLiqSinFac "
                strCantLiq += "From movimientos_transportes_consolidado "
                strCantLiq += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strCantLiq += "Where movimientos_transportes_consolidado.movimiento_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strCantLiq += "And COALESCE(ventas_control.idel, 0) = 0 "
                strCantLiq += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strCantLiq += "And movimientos_transportes_consolidado.movimiento_estado = 'CAUSADO' "

                Dim dtCantLiq As New DataTable

                dtCantLiq = csinformes.ejecutar_query_bd(strCantLiq)

                If dtCantLiq.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtCantLiq.Rows(0)("NumLiq") & "</font></b></td>"
                End If

                Dim strCantLiqMesAnt As String
                strCantLiqMesAnt = ""

                strCantLiqMesAnt += "Select COUNT(movimientos_transportes_consolidado.movimiento_id) As NumLiq, "
                strCantLiqMesAnt += "COALESCE(SUM(COALESCE(movimientos_transportes_consolidado.movimiento_fl_empresa, 0)), 0) As ValLiqSinFac "
                strCantLiqMesAnt += "From movimientos_transportes_consolidado "
                strCantLiqMesAnt += "Left Join ventas_control ON movimientos_transportes_consolidado.movimiento_id = ventas_control.movimientos_transportes_id "
                strCantLiqMesAnt += "WHERE MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & Month(txtFechaFin.Value) - 1 & " AND YEAR(" & txtFechaFin.Value & ") "
                strCantLiqMesAnt += "And COALESCE(ventas_control.idel, 0) = 0 "
                strCantLiqMesAnt += "And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strCantLiqMesAnt += "And movimientos_transportes_consolidado.movimiento_estado = 'CAUSADO' "

                Dim dtCantLiqMesAnt As New DataTable

                dtCantLiqMesAnt = csinformes.ejecutar_query_bd(strCantLiqMesAnt)

                If dtCantLiqMesAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & dtCantLiqMesAnt.Rows(0)("NumLiq") & "</font></b></td>"
                End If

                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='1px'>ANTICIPO</font></b></td>"

                If dtSinFact.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtSinFact.Rows(0)("ValAntSinFac") / 1000) & "</font></b></td>"
                End If

                If dtSinFacMesAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtSinFacMesAnt.Rows(0)("ValAntSinFac") / 1000) & "</font></b></td>"
                End If

                strHtml += "</tr>"

                strHtml += "<tr>"
                strHtml += "<td align='left'><b><font size='1px'>FLETE EMP</font></b></td>"

                If dtCantLiq.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantLiq.Rows(0)("ValLiqSinFac") / 1000) & "</font></b></td>"
                End If

                If dtCantLiqMesAnt.Rows.Count > 0 Then
                    strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantLiqMesAnt.Rows(0)("ValLiqSinFac") / 1000) & "</font></b></td>"
                End If

                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#9398F9'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN SIN FACTURAR</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnProveedores_Click(sender As Object, e As EventArgs) Handles btnProveedores.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9C284'>"
                strHtml += "<td align='center'><b><font size='1px'>PROVEEDORES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                'strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='30%'>"
                'strHtml += "<tr bgcolor='#F9C284'>"
                'strHtml += "<td align='left'><b><font size='1px'></font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & Year(txtFechaInicio.Value) & "</font></b></td>"
                'strHtml += "</tr>"

                'Dim strProvActual, strProvMesAnt As String
                'strProvMesAnt = ""

                'Dim Mes As Integer
                'Dim TotalActual, TotalAnterior As Decimal
                'Mes = 0
                'TotalActual = 0
                'TotalAnterior = 0
                'For i As Integer = 1 To Month(txtFechaFin.Value)
                '    strHtml += "<tr>"
                '    strHtml += "<td align='left' bgcolor='#F9C284'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                '    strProvMesAnt = ""
                '    strProvMesAnt += "Select SUM(COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvMesAnt += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 1 And compras_detalles.idel = 0 ), 0) - "
                '    strProvMesAnt += "COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvMesAnt += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 2 And compras_detalles.idel = 0 ), 0)) As ValorProv "
                '    strProvMesAnt += "From compras "
                '    strProvMesAnt += "Left Join compras_detalles cd1 ON compras.id = cd1.compra_id "
                '    strProvMesAnt += "WHERE Month(compras.fecha) = " & i & " "
                '    strProvMesAnt += "And Year(compras.fecha) = " & Year(txtFechaInicio.Value) - 1 & " AND (compras.estado_id = 43 or compras.estado_id = 48) And cd1.idel = 0 "

                '    Dim dtProvMesAnt As New DataTable

                '    dtProvMesAnt = csinformes.ejecutar_query_bd(strProvMesAnt)

                '    If dtProvMesAnt.Rows.Count > 0 Then
                '        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                '    End If

                '    TotalAnterior = TotalAnterior + dtProvMesAnt.Rows(0)("ValorProv")

                '    strProvActual = ""
                '    strProvActual += "Select SUM(COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvActual += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 1 And compras_detalles.idel = 0 ), 0) - "
                '    strProvActual += "COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvActual += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 2 And compras_detalles.idel = 0 ), 0)) As ValorProv "
                '    strProvActual += "From compras "
                '    strProvActual += "Left Join compras_detalles cd1 ON compras.id = cd1.compra_id "
                '    strProvActual += "WHERE Month(compras.fecha) = " & i & " "
                '    strProvActual += "And Year(compras.fecha) = " & Year(txtFechaInicio.Value) & " AND (compras.estado_id = 43 or compras.estado_id = 48) And cd1.idel = 0 "

                '    Dim dtProvActual As New DataTable

                '    dtProvActual = csinformes.ejecutar_query_bd(strProvActual)

                '    If dtProvActual.Rows.Count > 0 Then
                '        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvActual.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                '    End If

                '    TotalActual = TotalActual + dtProvActual.Rows(0)("ValorProv")

                '    strHtml += "</tr>"

                '    Mes = i
                'Next

                'For j As Integer = Mes + 1 To 12
                '    strHtml += "<tr>"
                '    strHtml += "<td align='left' bgcolor='#F9C284'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(j) & "</font></b></td>"

                '    strProvMesAnt = ""
                '    strProvMesAnt += "Select SUM(COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvMesAnt += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 1 And compras_detalles.idel = 0 ), 0) - "
                '    strProvMesAnt += "COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                '    strProvMesAnt += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 2 And compras_detalles.idel = 0 ), 0)) As ValorProv "
                '    strProvMesAnt += "From compras "
                '    strProvMesAnt += "Left Join compras_detalles cd1 ON compras.id = cd1.compra_id "
                '    strProvMesAnt += "WHERE Month(compras.fecha) = " & j & " "
                '    strProvMesAnt += "And Year(compras.fecha) = " & Year(txtFechaInicio.Value) - 1 & " AND (compras.estado_id = 43 or compras.estado_id = 48) And cd1.idel = 0 "

                '    Dim dtProvMesAnt As New DataTable

                '    dtProvMesAnt = csinformes.ejecutar_query_bd(strProvMesAnt)

                '    If dtProvMesAnt.Rows.Count > 0 Then
                '        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                '        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"
                '    End If

                '    TotalAnterior = TotalAnterior + dtProvMesAnt.Rows(0)("ValorProv")

                '    strHtml += "</tr>"
                'Next

                'strHtml += "<tr>"
                'strHtml += "<td align='left' bgcolor='#F9C284'><b><font size='1px'>TOTAL: </font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalAnterior / 1000) & "</font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalActual / 1000) & "</font></b></td>"
                'strHtml += "</tr>"

                'strHtml += "</table>"

                'strHtml += "<br />"

                'Mes = Month(txtFechaFin.Value)
                Dim strDetProv As String
                'For i As Integer = 1 To Month(txtFechaFin.Value)
                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr bgcolor='#F9C284'>"
                strHtml += "<td align='center' colspan='13'><b><font size='1px'>DETALLE PROVEEDORES " & Session("empresa") & " " & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(Month(txtFechaFin.Value)) & " " & Year(txtFechaFin.Value) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#F9C284'>"
                strHtml += "<td align='center'><b><font size='1px'>#</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NUMERO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>COMPROBANTE</font></b></td>"
                strHtml += "<td align='center' colspan='3'><b><font size='1px'>NOMBRE</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NIT</font></b></td>"
                strHtml += "<td align='center' colspan='3'><b><font size='1px'>CONCEPTO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NETO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>GASTO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>TIPO COMPRA</font></b></td>"
                strHtml += "</tr>"

                strDetProv = ""
                strDetProv += "Select compras.numero, "
                strDetProv += "movimientos_contables.numero AS Comprobante, "
                strDetProv += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ',terceros.apellido2) AS NOMBRE, "
                strDetProv += "terceros.documento As NIT, "
                strDetProv += "compras.descripcion AS CONCEPTO, "
                strDetProv += "SUM(COALESCE(cd1.valor_total, 0)) - COALESCE((Select SUM(COALESCE(compras_detalles.valor_total, 0)) "
                strDetProv += "FROM compras_detalles WHERE compras_detalles.compra_id = compras.id And compras_detalles.tipo = 2 And compras_detalles.idel = 0 ), 0) AS Neto, "
                strDetProv += "(SELECT SUM(movimientos_contables_detalles.valor) FROM movimientos_contables_detalles "
                strDetProv += "LEFT JOIN cuentas_contables ON movimientos_contables_detalles.cuentas_contables_idplan_cuentas = cuentas_contables.idplan_cuentas "
                strDetProv += "WHERE movimientos_contables_detalles.movimientos_contables_idcontabilidad = movimientos_contables.idcontabilidad "
                strDetProv += "And movimientos_contables_detalles.tipo = 1 And (SUBSTRING(cuentas_contables.codigo, 1, 1) = 5 Or "
                strDetProv += "SUBSTRING(cuentas_contables.codigo, 1, 1) = 6 OR SUBSTRING(cuentas_contables.codigo, 1, 1) = 7)) AS Gasto, "
                strDetProv += "marcas_compras.nombre AS TIPO_COMPRA "
                strDetProv += "From compras "
                strDetProv += "Left Join compras_detalles cd1 ON compras.id = cd1.compra_id AND cd1.tipo = 1 "
                strDetProv += "Left Join terceros ON compras.tercero_id = terceros.idterceros "
                strDetProv += "Left Join marcas_compras ON compras.id_marca_compras = marcas_compras.id_marca_compras "
                strDetProv += "Left Join movimientos_contables ON compras.contable_id = movimientos_contables.idcontabilidad "
                strDetProv += "WHERE compras.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                'strDetProv += "And YEAR(compras.fecha) = " & Year(txtFechaFin.Value) & " "
                strDetProv += "And (compras.estado_id = 43 or compras.estado_id = 48) And cd1.idel = 0 "
                strDetProv += "Group BY compras.numero, CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ',terceros.apellido2) "
                strDetProv += "ORDER BY compras.fecha "

                Dim dtDetProv As New DataTable

                dtDetProv = csinformes.ejecutar_query_bd(strDetProv)

                For j As Integer = 0 To dtDetProv.Rows.Count - 1
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='1px'>" & j + 1 & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>" & dtDetProv.Rows(j)("NUMERO") & "</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>" & dtDetProv.Rows(j)("Comprobante") & "</font></b></td>"
                    strHtml += "<td align='left' colspan='3'><b><font size='1px'>" & dtDetProv.Rows(j)("NOMBRE") & "</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & dtDetProv.Rows(j)("NIT") & "</font></b></td>"
                    strHtml += "<td align='left' colspan='3'><b><font size='1px'>" & dtDetProv.Rows(j)("CONCEPTO") & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtDetProv.Rows(j)("Neto")) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtDetProv.Rows(j)("Gasto")) & "</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & dtDetProv.Rows(j)("TIPO_COMPRA") & "</font></b></td>"
                    strHtml += "</tr>"
                Next

                strHtml += "</table>"

                strHtml += "<br />"

                'Mes = Mes - 1
                'Next

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#F9C284'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN PROVEEDORES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnGastos_Click(sender As Object, e As EventArgs) Handles btnGastos.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>GASTOS OPERACIONALES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center' colspan='4'><b><font size='1px'>GASTOS OPERACIONALES " & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>PROV</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NOMINA</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL</font></b></td>"
                strHtml += "</tr>"

                Dim strProvMesAnt As String
                strProvMesAnt = ""

                Dim Mes As Integer
                Dim TotalAnterior As Decimal
                Mes = 0
                TotalAnterior = 0
                For i As Integer = 1 To 12
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#FBAAA3'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    strProvMesAnt = ""
                    strProvMesAnt += "Select SUM(compras_detalles.valor_total) As ValorProv "
                    strProvMesAnt += "From compras "
                    strProvMesAnt += "Left Join compras_detalles ON compras.id = compras_detalles.compra_id "
                    strProvMesAnt += "WHERE Month(compras.fecha) = " & i & " "
                    strProvMesAnt += "And Year(compras.fecha) = " & Year(txtFechaInicio.Value) - 1 & " And compras.estado_id = 43 "

                    Dim dtProvMesAnt As New DataTable

                    dtProvMesAnt = csinformes.ejecutar_query_bd(strProvMesAnt)

                    If dtProvMesAnt.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                        TotalAnterior = TotalAnterior + dtProvMesAnt.Rows(0)("ValorProv")
                    Else
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        TotalAnterior = TotalAnterior + 0
                    End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>TOTALES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalAnterior / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalAnterior / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center' colspan='4'><b><font size='1px'>GASTOS OPERACIONALES " & Year(txtFechaInicio.Value) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>PROV</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NOMINA</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL</font></b></td>"
                strHtml += "</tr>"

                Dim strProvActual As String
                strProvActual = ""

                Dim TotalActual As Decimal
                Mes = 0
                TotalActual = 0
                For i As Integer = 1 To Month(txtFechaInicio.Value)
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#FBAAA3'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    strProvActual = ""
                    strProvActual += "Select SUM(compras_detalles.valor_total) As ValorProv "
                    strProvActual += "From compras "
                    strProvActual += "Left Join compras_detalles ON compras.id = compras_detalles.compra_id "
                    strProvActual += "WHERE Month(compras.fecha) = " & i & " "
                    strProvActual += "And Year(compras.fecha) = " & Year(txtFechaInicio.Value) & " And compras.estado_id = 43 "

                    Dim dtProvMesAnt As New DataTable

                    dtProvMesAnt = csinformes.ejecutar_query_bd(strProvActual)

                    If dtProvMesAnt.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtProvMesAnt.Rows(0)("ValorProv") / 1000) & "</font></b></td>"
                        TotalActual = TotalActual + dtProvMesAnt.Rows(0)("ValorProv")
                    Else
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0 / 1000) & "</font></b></td>"
                        TotalActual = TotalActual + 0
                    End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>TOTALES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalActual / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", TotalActual / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr bgcolor='#FBAAA3'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN GASTOS OPERACIONALES</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnFactNC_Click(sender As Object, e As EventArgs) Handles btnFactNC.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'>FACTURAS Y NOTAS CRÉDITO</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center' colspan='5'><b><font size='1px'>" & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>FACT</font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>N.C.</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "</tr>"

                Dim NumNotas As Integer
                Dim ValNota, AcumFact, AcumValFact, AcumNC, AcumValNC As Decimal
                AcumFact = 0
                AcumValFact = 0
                AcumNC = 0
                AcumValNC = 0
                NumNotas = 0
                ValNota = 0
                For i As Integer = 1 To 12
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#F95858'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    Dim strCantFact As String
                    strCantFact = ""

                    strCantFact += "Select COUNT(ventas_consolidado.venta_id) As NumFact, "
                    strCantFact += "SUM(ventas_consolidado.venta_subtotal) As ValFact "
                    strCantFact += "From ventas_consolidado "
                    strCantFact += "Where MONTH(ventas_consolidado.venta_fecha) = " & i & " "
                    strCantFact += "AND YEAR(ventas_consolidado.venta_fecha) = " & Year(txtFechaInicio.Value) - 1 & " "
                    strCantFact += "And ventas_consolidado.venta_estado <> 'ANULADA' "

                    Dim dtCantFact As New DataTable

                    dtCantFact = csinformes.ejecutar_query_bd(strCantFact)

                    If dtCantFact.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantFact.Rows(0)("NumFact") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact.Rows(0)("ValFact") / 1000) & "</font></b></td>"

                        AcumFact = AcumFact + dtCantFact.Rows(0)("NumFact")
                        AcumValFact = AcumValFact + dtCantFact.Rows(0)("ValFact")
                    End If

                    Dim strCantNC As String
                    strCantNC = ""

                    strCantNC += "Select COUNT(notas_credito.id) As NumNC "
                    strCantNC += "From notas_credito "
                    strCantNC += "WHERE Month(notas_credito.fecha) = " & i & " "
                    strCantNC += "And YEAR(notas_credito.fecha) = " & Year(txtFechaInicio.Value) - 1 & " "
                    strCantNC += "And (notas_credito.estado_id = 40 Or notas_credito.estado_id = 41) "

                    Dim dtCantNC As New DataTable

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        NumNotas = dtCantNC.Rows(0)("NumNC")
                        strHtml += "<td align='center'><b><font size='1px'>" & NumNotas & "</font></b></td>"
                        AcumNC = AcumNC + NumNotas
                    Else
                        NumNotas = 0
                        strHtml += "<td align='center'><b><font size='1px'>" & NumNotas & "</font></b></td>"
                        AcumNC = AcumNC + NumNotas
                    End If

                    strCantNC = ""

                    strCantNC += "Select COUNT(notas_credito.id) As NumNC, "
                    strCantNC += "SUM(CASE WHEN notas_credito.estado_id = 41 THEN 0 ELSE notas_credito_detalles.valor END) AS ValNC "
                    strCantNC += "From notas_credito "
                    strCantNC += "Left Join notas_credito_detalles ON notas_credito.id = notas_credito_detalles.nota_credito_id "
                    strCantNC += "WHERE Month(notas_credito.fecha) = " & i & " "
                    strCantNC += "And YEAR(notas_credito.fecha) = " & Year(txtFechaInicio.Value) - 1 & " "
                    strCantNC += "And (notas_credito.estado_id = 40 Or notas_credito.estado_id = 41) "

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        ValNota = dtCantNC.Rows(0)("ValNC")
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"
                        AcumValNC = AcumValNC + ValNota
                    Else
                        ValNota = 0
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"
                        AcumValNC = AcumValNC + ValNota
                    End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' bgcolor='#F95858'><b><font size='1px'>TOTALES: </font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumFact & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValFact / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumNC & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValNC / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center' colspan='5'><b><font size='1px'>" & Year(txtFechaInicio.Value) & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>FACT</font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>N.C.</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "</tr>"

                AcumFact = 0
                AcumValFact = 0
                AcumNC = 0
                AcumValNC = 0
                For i As Integer = 1 To Month(txtFechaInicio.Value)
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#F95858'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    Dim strCantFact As String
                    strCantFact = ""

                    strCantFact += "Select COUNT(ventas_consolidado.venta_id) As NumFact, "
                    strCantFact += "SUM(ventas_consolidado.venta_subtotal) As ValFact "
                    strCantFact += "From ventas_consolidado "
                    strCantFact += "Where MONTH(ventas_consolidado.venta_fecha) = " & i & " "
                    strCantFact += "AND YEAR(ventas_consolidado.venta_fecha) = " & Year(txtFechaInicio.Value) & " "
                    strCantFact += "And ventas_consolidado.venta_estado <> 'ANULADA' "

                    Dim dtCantFact As New DataTable

                    dtCantFact = csinformes.ejecutar_query_bd(strCantFact)

                    If dtCantFact.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantFact.Rows(0)("NumFact") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact.Rows(0)("ValFact") / 1000) & "</font></b></td>"

                        AcumFact = AcumFact + dtCantFact.Rows(0)("NumFact")
                        AcumValFact = AcumValFact + dtCantFact.Rows(0)("ValFact")
                    End If

                    Dim strCantNC As String
                    strCantNC = ""

                    strCantNC += "Select COUNT(notas_credito.id) As NumNC "
                    strCantNC += "From notas_credito "
                    strCantNC += "WHERE Month(notas_credito.fecha) = " & i & " "
                    strCantNC += "And YEAR(notas_credito.fecha) = " & Year(txtFechaInicio.Value) & " "
                    strCantNC += "And (notas_credito.estado_id = 40 Or notas_credito.estado_id = 41) "

                    Dim dtCantNC As New DataTable

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        NumNotas = dtCantNC.Rows(0)("NumNC")
                        strHtml += "<td align='center'><b><font size='1px'>" & NumNotas & "</font></b></td>"
                        AcumNC = AcumNC + NumNotas
                    Else
                        NumNotas = 0
                        strHtml += "<td align='center'><b><font size='1px'>" & NumNotas & "</font></b></td>"
                        AcumNC = AcumNC + NumNotas
                    End If

                    strCantNC = ""

                    strCantNC += "Select COUNT(notas_credito.id) As NumNC, "
                    strCantNC += "SUM(CASE WHEN notas_credito.estado_id = 41 THEN 0 ELSE notas_credito_detalles.valor END) AS ValNC "
                    strCantNC += "From notas_credito "
                    strCantNC += "Left Join notas_credito_detalles ON notas_credito.id = notas_credito_detalles.nota_credito_id "
                    strCantNC += "WHERE Month(notas_credito.fecha) = " & i & " "
                    strCantNC += "And YEAR(notas_credito.fecha) = " & Year(txtFechaInicio.Value) & " "
                    strCantNC += "And (notas_credito.estado_id = 40 Or notas_credito.estado_id = 41) "

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        ValNota = dtCantNC.Rows(0)("ValNC")
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"
                        AcumValNC = AcumValNC + ValNota
                    Else
                        ValNota = 0
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"
                        AcumValNC = AcumValNC + ValNota
                    End If

                    'If dtCantNC.Rows.Count > 0 Then
                    '    strHtml += "<td align='center'><b><font size='1px'>" & NumNotas & "</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"

                    '    AcumNC = AcumNC + NumNotas
                    '    AcumValNC = AcumValNC + ValNota
                    'End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' bgcolor='#F95858'><b><font size='1px'>TOTALES: </font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumFact & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValFact / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumNC & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValNC / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F95858'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN FACTURAS Y NOTAS CRÉDITO</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnRecaudo_Click(sender As Object, e As EventArgs) Handles btnRecaudo.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#6068F8'>"
                strHtml += "<td align='center'><b><font size='1px'>RECAUDOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#6068F8'>"
                strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>" & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>" & Year(txtFechaInicio.Value) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#6068F8'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "</tr>"

                Dim AcumFact, AcumValFact, AcumNC, AcumValNC As Decimal
                AcumFact = 0
                AcumValFact = 0
                AcumNC = 0
                AcumValNC = 0
                For i As Integer = 1 To 12
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#6068F8'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    Dim strCantFact As String
                    strCantFact = ""

                    strCantFact += "Select COUNT(ventas_recaudos.total) As NumFact, "
                    strCantFact += "SUM(ventas_recaudos.total) As valFact "
                    strCantFact += "From ventas_recaudos "
                    strCantFact += "Where Month(ventas_recaudos.fecha_recaudo) = " & i & " "
                    strCantFact += " And Year(ventas_recaudos.fecha_recaudo) = " & Year(txtFechaInicio.Value) - 1 & " "

                    Dim dtCantFact As New DataTable

                    dtCantFact = csinformes.ejecutar_query_bd(strCantFact)

                    If dtCantFact.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantFact.Rows(0)("NumFact") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact.Rows(0)("ValFact") / 1000) & "</font></b></td>"

                        AcumFact = AcumFact + dtCantFact.Rows(0)("NumFact")
                        AcumValFact = AcumValFact + dtCantFact.Rows(0)("ValFact")
                    End If

                    Dim strCantNC As String
                    strCantNC = ""

                    strCantNC += "Select COUNT(ventas_recaudos.total) As NumNC, "
                    strCantNC += "COALESCE(SUM(ventas_recaudos.total), 0) As ValNC "
                    strCantNC += "From ventas_recaudos "
                    strCantNC += "Where Month(ventas_recaudos.fecha_recaudo) = " & i & " "
                    strCantNC += "And Year(ventas_recaudos.fecha_recaudo) = " & Year(txtFechaInicio.Value) & " "

                    Dim dtCantNC As New DataTable

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantNC.Rows(0)("NumNC") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"

                        AcumNC = AcumNC + dtCantNC.Rows(0)("NumNC")
                        AcumValNC = AcumValNC + dtCantNC.Rows(0)("ValNC")
                    Else
                        strHtml += "<td align='center'><b><font size='1px'>" & 0 & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"

                        AcumNC = AcumNC + 0
                        AcumValNC = AcumValNC + 0
                    End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' bgcolor='#6068F8'><b><font size='1px'>TOTALES: </font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumFact & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValFact / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumNC & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValNC / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#6068F8'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN RECAUDOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnAnticipos_Click(sender As Object, e As EventArgs) Handles btnAnticipos.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F73827'>"
                strHtml += "<td align='center'><b><font size='1px'>ANTICIPOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F73827'>"
                strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>" & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                strHtml += "<td align='center' colspan='2'><b><font size='1px'>" & Year(txtFechaInicio.Value) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#F73827'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANT</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "</tr>"

                Dim AcumFact, AcumValFact, AcumNC, AcumValNC As Decimal
                AcumFact = 0
                AcumValFact = 0
                AcumNC = 0
                AcumValNC = 0
                For i As Integer = 1 To 12
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#F73827'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    Dim strCantFact As String
                    strCantFact = ""

                    If ConfigurationManager.AppSettings("bdsel").ToString = 3 Then
                        strCantFact += "Select COUNT(movimientos_contables_detalles.valor) As NumFact, "
                        strCantFact += "COALESCE(SUM(movimientos_contables_detalles.valor), 0) As valFact "
                        strCantFact += "From movimientos_contables "
                        strCantFact += "Left Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strCantFact += "WHERE Month(movimientos_contables.fecha) = " & i & " "
                        strCantFact += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) - 1 & " "
                        strCantFact += "And (movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2170 Or movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2171) "
                        strCantFact += "And movimientos_contables_detalles.tipo = 2 And movimientos_contables_detalles.valor > 0 "
                    Else
                        strCantFact += "Select COUNT(movimientos_transportes_anticipos.valor) As NumFact, "
                        strCantFact += "SUM(movimientos_transportes_anticipos.valor) As valFact "
                        strCantFact += "From movimientos_transportes_anticipos "
                        strCantFact += "Left Join movimientos_contables ON movimientos_transportes_anticipos.movimientos_contables_idcontabilidad = movimientos_contables.idcontabilidad "
                        strCantFact += "WHERE Month(movimientos_contables.fecha) = " & i & " "
                        strCantFact += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) - 1 & " "
                    End If

                    Dim dtCantFact As New DataTable

                    dtCantFact = csinformes.ejecutar_query_bd(strCantFact)

                    If dtCantFact.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantFact.Rows(0)("NumFact") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact.Rows(0)("ValFact") / 1000) & "</font></b></td>"

                        AcumFact = AcumFact + dtCantFact.Rows(0)("NumFact")
                        AcumValFact = AcumValFact + dtCantFact.Rows(0)("ValFact")
                    End If

                    Dim strCantNC As String
                    strCantNC = ""

                    If ConfigurationManager.AppSettings("bdsel").ToString = 3 Then
                        strCantNC += "Select COUNT(movimientos_contables_detalles.valor) As NumNC, "
                        strCantNC += "COALESCE(SUM(movimientos_contables_detalles.valor), 0) As ValNC "
                        strCantNC += "From movimientos_contables "
                        strCantNC += "Left Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                        strCantNC += "WHERE Month(movimientos_contables.fecha) = " & i & " "
                        strCantNC += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) & " "
                        strCantNC += "And (movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2170 Or movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2171) "
                        strCantNC += "And movimientos_contables_detalles.tipo = 2 And movimientos_contables_detalles.valor > 0 "
                    Else
                        strCantNC += "Select COUNT(movimientos_transportes_anticipos.valor) As NumNC, "
                        strCantNC += "COALESCE(SUM(movimientos_transportes_anticipos.valor), 0) As ValNC "
                        strCantNC += "From movimientos_transportes_anticipos "
                        strCantNC += "Left Join movimientos_contables ON movimientos_transportes_anticipos.movimientos_contables_idcontabilidad = movimientos_contables.idcontabilidad "
                        strCantNC += "WHERE Month(movimientos_contables.fecha) = " & i & " "
                        strCantNC += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) & " "
                    End If

                    Dim dtCantNC As New DataTable

                    dtCantNC = csinformes.ejecutar_query_bd(strCantNC)

                    If dtCantNC.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & dtCantNC.Rows(0)("NumNC") & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantNC.Rows(0)("ValNC") / 1000) & "</font></b></td>"

                        AcumNC = AcumNC + dtCantNC.Rows(0)("NumNC")
                        AcumValNC = AcumValNC + dtCantNC.Rows(0)("ValNC")
                    Else
                        strHtml += "<td align='center'><b><font size='1px'>" & 0 & "</font></b></td>"
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"

                        AcumNC = AcumNC + 0
                        AcumValNC = AcumValNC + 0

                    End If

                    strHtml += "</tr>"
                Next

                strHtml += "<tr>"
                strHtml += "<td align='center' bgcolor='#F73827'><b><font size='1px'>TOTALES: </font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumFact & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValFact / 1000) & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & AcumNC & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValNC / 1000) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                If ConfigurationManager.AppSettings("bdsel").ToString = 3 Then
                    Dim strAntProv, strAntEmp As String
                    strAntProv = ""
                    strAntEmp = ""

                    strAntProv += "Select terceros.documento As documento, "
                    strAntProv += "movimientos_contables.numero As numero, "
                    strAntProv += "movimientos_contables.fecha As fecha, "
                    strAntProv += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS tercero, "
                    strAntProv += "movimientos_contables_detalles.valor AS valor "
                    strAntProv += "From movimientos_contables "
                    strAntProv += "Left Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                    strAntProv += "Left Join terceros ON movimientos_contables_detalles.terceros_idterceros = terceros.idterceros "
                    strAntProv += "WHERE Month(movimientos_contables.fecha) = " & Month(txtFechaInicio.Value) & " "
                    strAntProv += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) & " "
                    strAntProv += "And (movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2170) "
                    strAntProv += "And movimientos_contables_detalles.tipo = 2 And movimientos_contables_detalles.valor > 0 "
                    strAntProv += "ORDER BY movimientos_contables_detalles.valor DESC "

                    Dim dtAntProv As New DataTable

                    dtAntProv = csinformes.ejecutar_query_bd(strAntProv)

                    strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                    strHtml += "<tr bgcolor='#F73827'>"
                    strHtml += "<td align='center' colspan='5'><b><font size='1px'>ANTICIPOS A PROVEEDORES</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F73827'>"
                    strHtml += "<td align='center'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>FECHA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>DOCUMENTO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>TERCERO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                    strHtml += "</tr>"
                    If dtAntProv.Rows.Count > 0 Then
                        For i As Integer = 0 To dtAntProv.Rows.Count - 1
                            strHtml += "</tr>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntProv.Rows(i)("numero") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntProv.Rows(i)("fecha") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntProv.Rows(i)("documento") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntProv.Rows(i)("tercero") & "</font></b></td>"
                            strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtAntProv.Rows(i)("valor")) & "</font></b></td>"
                            strHtml += "</tr>"
                        Next
                    End If
                    strHtml += "</table>"

                    strHtml += "<br />"

                    strAntEmp += "Select terceros.documento As documento, "
                    strAntEmp += "movimientos_contables.numero As numero, "
                    strAntEmp += "movimientos_contables.fecha As fecha, "
                    strAntEmp += "CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) AS tercero, "
                    strAntEmp += "movimientos_contables_detalles.valor AS valor "
                    strAntEmp += "From movimientos_contables "
                    strAntEmp += "Left Join movimientos_contables_detalles ON movimientos_contables.idcontabilidad = movimientos_contables_detalles.movimientos_contables_idcontabilidad "
                    strAntEmp += "Left Join terceros ON movimientos_contables_detalles.terceros_idterceros = terceros.idterceros "
                    strAntEmp += "WHERE Month(movimientos_contables.fecha) = " & Month(txtFechaInicio.Value) & " "
                    strAntEmp += "And YEAR(movimientos_contables.fecha) = " & Year(txtFechaInicio.Value) & " "
                    strAntEmp += "And (movimientos_contables_detalles.cuentas_contables_idplan_cuentas = 2171) "
                    strAntEmp += "And movimientos_contables_detalles.tipo = 2 And movimientos_contables_detalles.valor > 0 "
                    strAntEmp += "ORDER BY movimientos_contables_detalles.valor DESC "

                    Dim dtAntEmp As New DataTable

                    dtAntEmp = csinformes.ejecutar_query_bd(strAntEmp)

                    strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                    strHtml += "<tr bgcolor='#F73827'>"
                    strHtml += "<td align='center' colspan='5'><b><font size='1px'>ANTICIPOS A PROVEEDORES</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#F73827'>"
                    strHtml += "<td align='center'><b><font size='1px'>NUMERO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>FECHA</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>DOCUMENTO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>TERCERO</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                    strHtml += "</tr>"
                    If dtAntEmp.Rows.Count > 0 Then
                        For i As Integer = 0 To dtAntEmp.Rows.Count - 1
                            strHtml += "</tr>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntEmp.Rows(i)("numero") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntEmp.Rows(i)("fecha") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntEmp.Rows(i)("documento") & "</font></b></td>"
                            strHtml += "<td align='left'><b><font size='1px'>" & dtAntEmp.Rows(i)("tercero") & "</font></b></td>"
                            strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtAntEmp.Rows(i)("valor")) & "</font></b></td>"
                            strHtml += "</tr>"
                        Next
                    End If
                    strHtml += "</table>"

                    strHtml += "<br />"
                End If

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F73827'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN ANTICIPOS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnCartera_Click(sender As Object, e As EventArgs) Handles btnCartera.Click
        Try
            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas inválidas...');", True)
            Else
                Dim strHtml As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F9AB54'>"
                strHtml += "<td align='center'><b><font size='1px'>CARTERA</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F9AB54'>"
                strHtml += "<td align='center'><b><font size='1px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & Year(txtFechaInicio.Value) - 1 & "</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>" & Year(txtFechaInicio.Value) & "</font></b></td>"
                strHtml += "</tr>"

                strHtml += "<tr bgcolor='#F9AB54'>"
                strHtml += "<td align='center'><b><font size='1px'>MES</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR</font></b></td>"
                strHtml += "</tr>"

                Dim AcumFact, AcumValFact, AcumNC, AcumValNC As Decimal
                Dim strFechaFin, strFechaFin1 As String
                AcumFact = 0
                AcumValFact = 0
                AcumNC = 0
                AcumValNC = 0
                strFechaFin = "2019-12-31"
                strFechaFin1 = "2020-12-31"
                For i As Integer = 1 To 12
                    strHtml += "<tr>"
                    strHtml += "<td align='left' bgcolor='#F9AB54'><b><font size='1px'>" & CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i) & "</font></b></td>"

                    Dim strCantFact As String
                    strCantFact = ""

                    strCantFact += "Select SUM((vc.venta_total - IFNULL((SELECT  SUM(vrk.valor) "
                    strCantFact += "        From ventas_recaudos vrf inner Join ventas_recaudos_detalle vrk "
                    strCantFact += "        On vrf.idventas_recaudos=vrk.ventas_recaudos_id			where vrf.fecha_recaudo<=DATE_ADD('" & strFechaFin & "', INTERVAL " & i & " MONTH) "
                    strCantFact += "        And vrk.ventas_idventas =vc.venta_id "
                    strCantFact += "        Group BY vrk.ventas_idventas),0))) As 'saldo' "
                    strCantFact += "From ventas_consolidado vc "
                    strCantFact += "Left Join   ventas_recaudos_detalle vrd ON vc.venta_id = vrd.ventas_idventas "
                    strCantFact += "Left Join   ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos "
                    strCantFact += "WHERE vc.venta_vence <= DATE_ADD('" & strFechaFin & "', INTERVAL " & i & " MONTH) "
                    strCantFact += "And vc.venta_estado = 'EMITIDA' "
                    strCantFact += "And (vc.venta_total - IFNULL((SELECT  SUM(vrk.valor)  "
                    strCantFact += "       From ventas_recaudos vrf inner Join ventas_recaudos_detalle vrk "
                    strCantFact += "        On vrf.idventas_recaudos=vrk.ventas_recaudos_id			where vrf.fecha_recaudo<=DATE_ADD('" & strFechaFin & "', INTERVAL " & i & " MONTH) "
                    strCantFact += "        And vrk.ventas_idventas =vc.venta_id "
                    strCantFact += "        Group BY vrk.ventas_idventas),0))>0 "
                    strCantFact += "And vc.dudoso_recaudo = 0  "

                    Dim dtCantFact As New DataTable

                    dtCantFact = csinformes.ejecutar_query_bd(strCantFact)

                    If dtCantFact.Rows.Count > 0 Then
                        strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact.Rows(0)("Saldo") / 1000) & "</font></b></td>"

                        Dim strCantFact1 As String
                        strCantFact1 = ""

                        strCantFact1 += "Select SUM((vc.venta_total - IFNULL((SELECT  SUM(vrk.valor) "
                        strCantFact1 += "        From ventas_recaudos vrf inner Join ventas_recaudos_detalle vrk "
                        strCantFact1 += "        On vrf.idventas_recaudos=vrk.ventas_recaudos_id			where vrf.fecha_recaudo<=DATE_ADD('" & strFechaFin1 & "', INTERVAL " & i & " MONTH) "
                        strCantFact1 += "        And vrk.ventas_idventas =vc.venta_id "
                        strCantFact1 += "        Group BY vrk.ventas_idventas),0))) As 'saldo' "
                        strCantFact1 += "From ventas_consolidado vc "
                        strCantFact1 += "Left Join   ventas_recaudos_detalle vrd ON vc.venta_id = vrd.ventas_idventas "
                        strCantFact1 += "Left Join   ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos "
                        strCantFact1 += "WHERE vc.venta_vence <= DATE_ADD('" & strFechaFin1 & "', INTERVAL " & i & " MONTH) "
                        strCantFact1 += "And vc.venta_estado = 'EMITIDA' "
                        strCantFact1 += "And (vc.venta_total - IFNULL((SELECT  SUM(vrk.valor)  "
                        strCantFact1 += "       From ventas_recaudos vrf inner Join ventas_recaudos_detalle vrk "
                        strCantFact1 += "        On vrf.idventas_recaudos=vrk.ventas_recaudos_id			where vrf.fecha_recaudo<=DATE_ADD('" & strFechaFin1 & "', INTERVAL " & i & " MONTH) "
                        strCantFact1 += "        And vrk.ventas_idventas =vc.venta_id "
                        strCantFact1 += "        Group BY vrk.ventas_idventas),0))>0 "
                        strCantFact1 += "And vc.dudoso_recaudo = 0  "

                        Dim dtCantFact1 As New DataTable

                        dtCantFact1 = csinformes.ejecutar_query_bd(strCantFact1)

                        If dtCantFact1.Rows.Count > 0 Then
                            strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", dtCantFact1.Rows(0)("Saldo") / 1000) & "</font></b></td>"
                            AcumValNC = AcumValNC + dtCantFact1.Rows(0)("Saldo")
                        Else
                            strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", 0) & "</font></b></td>"
                            AcumValNC = AcumValNC + 0
                        End If

                        AcumValFact = AcumValFact + dtCantFact.Rows(0)("Saldo")
                    End If

                    strHtml += "</tr>"
                Next

                'strHtml += "<tr>"
                'strHtml += "<td align='center' bgcolor='#F9AB54'><b><font size='1px'>TOTALES: </font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValFact / 1000) & "</font></b></td>"
                'strHtml += "<td align='center'><b><font size='1px'>" & String.Format("{0:c}", AcumValNC / 1000) & "</font></b></td>"
                'strHtml += "</tr>"

                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#F9AB54'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN CARTERA</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnConsolidado_Click(sender As Object, e As EventArgs) Handles btnConsolidado.Click

    End Sub

    Private Sub btnDetCartera_Click(sender As Object, e As EventArgs) Handles btnDetCartera.Click
        'Response.Redirect("InfParamCarteraXGeneradorConsolidado.aspx")

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
            strSQL = " Select COALESCE(vc.asesor_nombre, "
            strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
            strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
            strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')) as Asesor, "
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
            'strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd WHERE vrd.ventas_idventas=vc.venta_id)Is NULL)"
            strSQL += " And ((vc.venta_total > (SELECT SUM(vrd.valor)FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "')) Or (SELECT SUM(vrd.valor) FROM ventas_recaudos_detalle vrd LEFT JOIN ventas_recaudos vr ON vrd.ventas_recaudos_id = vr.idventas_recaudos WHERE vrd.ventas_idventas=vc.venta_id AND vr.fecha_recaudo <= '" & txtFechaInicio.Value & "')Is NULL)"
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

            'If cboasesores.SelectedValue > 0 Then
            '    strSQL += " AND COALESCE((SELECT generadores_asesores.usuarios_idusuarios FROM generadores_asesores "
            '    strSQL += " Left Join usuarios ON generadores_asesores.usuarios_idusuarios = usuarios.idusuarios "
            '    strSQL += " WHERE generadores_asesores.generadores_idgeneradores = vc.generador_id And generadores_asesores.idel = 0 "
            '    strSQL += " ORDER BY generadores_asesores.idgeneradores_asesores LIMIT 1), 0) = " & cboasesores.SelectedValue & ""
            'End If

            'If cbogeneradores.SelectedValue > 0 Then
            '    strSQL += " AND vc.generador_id = " & cbogeneradores.SelectedValue & ""
            'End If

            strSQL += " Group BY vc.generador_nombre, COALESCE(vc.asesor_nombre, "
            strSQL += " COALESCE((SELECT CONCAT(terceros.nombre1, ' ', terceros.nombre2, ' ', terceros.apellido1, ' ', terceros.apellido2) "
            strSQL += " From generadores_asesores  LEFT Join usuarios On generadores_asesores.usuarios_idusuarios = usuarios.idusuarios LEFT Join terceros On usuarios.idterceros = terceros.idterceros "
            strSQL += " Where terceros.documento = vc.generador_documento And generadores_asesores.idel = 0 And usuarios.idel = 0 LIMIT 1), 'TERCERO ESPECIAL')), DesCart "
            strSQL += " ORDER BY Saldo DESC"

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
                strHtml += "<td align='center' colspan='5'><b><font size='17px'>CARTERA X GENERADOR(Hasta Fecha: " & txtFechaFin.Value & " Tipo: " & strTipo & ")</font></b></td>"
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
                strHtml += "<td align='left' colspan='3'><b><font size='13px'></font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera < 29 dias</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera entre 30 y 59</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera entre 60 y 79</font></b></td>"
                strHtml += "<td align='center'><b><font size='12px'>Cartera 80 dias o mas(" & Year(Now()) & ")</font></b></td>"
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
                strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>CARTERA X GENERADOR(Hasta Fecha: " & txtFechaFin.Value & " Tipo: " & strTipo & ")</font></b></td>"
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
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera < 29 dias</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera entre 30 y 59</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera entre 60 y 79</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera 80 dias o mas(" & Year(Now()) & ")</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera (" & Year(Now()) - 1 & ")</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Cartera Juridica</font></b></td>"
                strHtmlmostrar += "<td align='center'><b><font size='1px'>Total de saldo</font></b></td>"
                strHtmlmostrar += "</tr>"
                strHtmlmostrar += "<tr>"
                strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>" & dtter.Rows(0)("Generador").ToString & "</font></b></td>"

                'strAsesorActual = dtter.Rows(0)("Asesor").ToString
                strGeneradorActual = dtter.Rows(0)("Generador").ToString
                For i As Integer = 0 To dtter.Rows.Count - 1
                    If i Mod 2 = 0 Then
                        color = "#FFFFFF"
                    Else
                        color = "#EFF3FB"
                    End If

                    'If strAsesorActual = dtter.Rows(i)("Asesor").ToString Then
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
                    'Else
                    '    If contTD <= 6 Then
                    '        For j As Integer = contTD To 6
                    '            strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    '            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    '        Next
                    '    End If
                    '    decTotal7 = decTotal7 + decTotalCliente
                    '    decTotTotal7 = decTotTotal7 + decTotal7
                    '    strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                    '    strHtml += "</tr>"
                    '    strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", decTotalCliente) & "</font></td>"
                    '    strHtmlmostrar += "</tr>"
                    '    strHtml += "<tr>"
                    '    strHtml += "<td align='center' colspan='3'><b><font size='13px'>TOTALES</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal6) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:c}", decTotal7) & "</font></b></td>"
                    '    strHtml += "</tr>"
                    '    strHtml += "<tr>"
                    '    strHtml += "<td align='center' colspan='3'><b><font size='13px'>PORCENTAJES</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal1 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal2 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal3 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal4 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal5 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round(decTotal6 / decTotal7, 2)) & "</font></b></td>"
                    '    strHtml += "<td align='right'><b><font size='12px'>" & String.Format("{0:P2}", Round((decTotal1 / decTotal7) + (decTotal2 / decTotal7) + (decTotal3 / decTotal7) + (decTotal4 / decTotal7) + (decTotal5 / decTotal7) + (decTotal6 / decTotal7), 2)) & "</font></b></td>"
                    '    strHtml += "</tr>"
                    '    contTD = 1
                    '    decTotalCliente = 0
                    '    decTotal1 = 0
                    '    decTotal2 = 0
                    '    decTotal3 = 0
                    '    decTotal4 = 0
                    '    decTotal5 = 0
                    '    decTotal6 = 0
                    '    decTotal7 = 0
                    '    strHtml += "</table>"
                    '    strHtml += "<br />"
                    '    strHtml += "<br />"
                    '    strHtml += "<br />"
                    '    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    '    strHtml += "<tr>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'></font></b></td>"
                    '    strHtml += "</tr>"
                    '    strHtml += "<tr>"
                    '    strHtml += "<td align='left' colspan='3'><b><font size='13px'>" & dtter.Rows(i)("Asesor").ToString & "</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera < 29 dias</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera entre 30 y 59</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera entre 60 y 79</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera 80 dias o mas(" & Year(Now()) & ")</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera (" & Year(Now()) - 1 & ")</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Cartera Juridica</font></b></td>"
                    '    strHtml += "<td align='center'><b><font size='12px'>Total de saldo</font></b></td>"
                    '    strHtml += "</tr>"
                    '    strHtml += "<tr>"
                    '    strHtml += "<td align='left' colspan='3'><b><font size='12px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                    '    'strHtmlmostrar += "</tr>"
                    '    strHtmlmostrar += "<tr>"
                    '    strHtmlmostrar += "<td align='left' colspan='3'><b><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></b></td>"
                    '    If dtter.Rows(i)("DesCart") > contTD Then
                    '        For j As Integer = contTD + 1 To dtter.Rows(i)("DesCart")
                    '            strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    '            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                    '        Next
                    '        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                    '        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                    '        contTD = dtter.Rows(i)("DesCart")
                    '    Else
                    '        strHtml += "<td align='right'><font size='12px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                    '        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"
                    '    End If
                    '    contTD = contTD + 1
                    '    decTotalCliente = decTotalCliente + dtter.Rows(i)("Saldo")
                    '    'decTotal7 = decTotal7 + decTotalCliente
                    '    strAsesorActual = dtter.Rows(i)("Asesor").ToString
                    'End If

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
                'divinforme.InnerHtml = strHtml
            End If
        End If
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub btnDetFacturas_Click(sender As Object, e As EventArgs) Handles btnDetFacturas.Click
        Try
            Dim strSQL As String
            strSQL = ""

            strSQL += "Select ventas_consolidado.venta_fecha, "
            strSQL += "ventas_consolidado.venta_numero, "
            strSQL += "ventas_consolidado.generador_nombre, "
            strSQL += "ventas_consolidado.generador_documento, "
            strSQL += "ventas_consolidado.venta_iva, "
            strSQL += "ventas_consolidado.venta_retencion, "
            strSQL += "ventas_consolidado.venta_rteiva, "
            strSQL += "ventas_consolidado.venta_total As total, "
            strSQL += "ventas_consolidado.venta_observaciones, "
            strSQL += "ventas_detalles.iva As iva_detalle, "
            strSQL += "ventas_detalles.rte_fuente as ret_detalle, "
            strSQL += "ventas_detalles.rte_iva As reteiva_detalle, "
            strSQL += "ventas_detalles.cantidad as cantidad_detalle, "
            strSQL += "ventas_detalles.valor_unidad As unidad_detalle, "
            strSQL += "ventas_detalles.valor_total as valor_detalle, "
            strSQL += "ventas_detalles.descripcion as desc_detalle "
            strSQL += "From ventas_consolidado "
            strSQL += "Left Join ventas_detalles ON ventas_consolidado.venta_id = ventas_detalles.ventas_idventas "
            strSQL += "WHERE ventas_consolidado.venta_fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
            strSQL += "And ventas_consolidado.venta_estado <> 'ANULADA' AND ventas_detalles.idel = 0 "
            strSQL += "ORDER BY ventas_consolidado.venta_fecha "

            Dim dtdet As New DataTable
            dtdet = csinformes.ejecutar_query_bd(strSQL)

            If dtdet.Rows.Count > 0 Then
                Dim strHtml, strExcel As String

                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#9F030C'>"
                strHtml += "<td align='center'><b><font size='1px'>DETALLE FACTURA</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#9F030C'>"
                strHtml += "<td align='center'><b><font size='1px'>FECHA</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>NUMERO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CLIENTE</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>DOCUMENTO</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>IVA</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>RETENCION</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>RETEIVA</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>TOTAL</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>OBSERVACION</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>IVA_DETALLE</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>RETENCION_DET</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>RETEIVA_DET</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>CANTIDAD</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>UNIDAD</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>VALOR_DET</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>DESCRIPCION</font></b></td>"
                strHtml += "</tr>"

                strExcel = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strExcel += "<tr bgcolor='#9F030C'>"
                strExcel += "<td align='center'><b><font size='1px'>DETALLE FACTURA</font></b></td>"
                strExcel += "</tr>"
                strExcel += "</table>"

                strExcel += "<br />"

                strExcel += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strExcel += "<tr bgcolor='#9F030C'>"
                strExcel += "<td align='center'><b><font size='1px'>FECHA</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>NUMERO</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>CLIENTE</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>DOCUMENTO</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>IVA</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>RETENCION</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>RETEIVA</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>TOTAL</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>OBSERVACION</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>IVA_DETALLE</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>RETENCION_DET</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>RETEIVA_DET</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>CANTIDAD</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>UNIDAD</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>VALOR_DET</font></b></td>"
                strExcel += "<td align='center'><b><font size='1px'>DESCRIPCION</font></b></td>"
                strExcel += "</tr>"

                If dtdet.Rows.Count > 0 Then
                    For i As Integer = 0 To dtdet.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_fecha") & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_numero") & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("generador_nombre") & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("generador_documento") & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_iva")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_retencion")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_rteiva")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("total")) & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_observaciones") & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("iva_detalle")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("ret_detalle")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("reteiva_detalle")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("cantidad_detalle")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("unidad_detalle")) & "</font></b></td>"
                        strHtml += "<td align='right'><b><font size='1px'>" & CInt(dtdet.Rows(i)("valor_detalle")) & "</font></b></td>"
                        strHtml += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("desc_detalle") & "</font></b></td>"
                        strHtml += "</tr>"

                        strExcel += "<tr>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_fecha") & "</font></b></td>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_numero") & "</font></b></td>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("generador_nombre") & "</font></b></td>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("generador_documento") & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_iva")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_retencion")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("venta_rteiva")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("total")) & "</font></b></td>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("venta_observaciones") & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("iva_detalle")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("ret_detalle")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("reteiva_detalle")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("cantidad_detalle")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", dtdet.Rows(i)("unidad_detalle")) & "</font></b></td>"
                        strExcel += "<td align='right'><b><font size='1px'>" & CInt(dtdet.Rows(i)("valor_detalle")) & "</font></b></td>"
                        strExcel += "<td align='left'><b><font size='1px'>" & dtdet.Rows(i)("desc_detalle") & "</font></b></td>"
                        strExcel += "</tr>"
                    Next
                End If
                strHtml += "</table>"

                strHtml += "<br />"

                strHtml += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strHtml += "<tr bgcolor='#9F030C'>"
                strHtml += "<td align='center'><b><font size='1px'>FIN DETALLE FACTURAS</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strExcel += "</table>"

                strExcel += "<br />"

                strExcel += " <Table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                strExcel += "<tr bgcolor='#9F030C'>"
                strExcel += "<td align='center'><b><font size='1px'>FIN DETALLE FACTURAS</font></b></td>"
                strExcel += "</tr>"
                strExcel += "</table>"

                divmostrar.InnerHtml = strHtml

                'divinforme.InnerHtml = strExcel
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnCXC_Click(sender As Object, e As EventArgs) Handles btnCXC.Click
        Try
            divmostrar.InnerHtml = ""
            divinforme.InnerHtml = ""
            Dim intAño As Integer
            Dim strTipo, strSQL As String

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

            strSQL = " Select CONCAT(terceros.nombre1,' ',terceros.nombre2,' ',terceros.apellido1,' ',terceros.apellido2) as Tercero,"
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
            strSQL += " GROUP BY Tercero, propietarios_cxc.fecha, movimientos_transportes.placa, movimientos_contables.numero, propietarios_cxc.estado, Despachador"
            strSQL += " HAVING propietarios_cxc.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "'"
            strSQL += " And Format(COALESCE(SUM(propietarios_cxc_detalles.valor), 0) - COALESCE(SUM(propietarios_cxc_detalles.valor_abono), 0), 2) > 0 "
            strSQL += " AND propietarios_cxc.estado <> 3 "
            strSQL += " ORDER BY Despachador"

            Dim dtter = csinformes.ejecutar_query_bd(strSQL)

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
                strHtml += "<tr bgcolor='#656DC1'>"
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
                strHtmlmostrar += "<tr bgcolor='#656DC1'>"
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

                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

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
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Total")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Abonos")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Saldo")) & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        decTotal1 = decTotal1 + dtter.Rows(i)("Total")
                        decTotal2 = decTotal2 + dtter.Rows(i)("Abonos")
                        decTotal3 = decTotal3 + dtter.Rows(i)("Saldo")
                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Saldo")
                    Else
                        strHtml += "<tr bgcolor='#656DC1'>"
                        strHtml += "<td align='right' colspan='11'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                        strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr bgcolor='#656DC1'>"
                        strHtmlmostrar += "<td align='right' colspan='11'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
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

                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Tercero").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Observacion").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Mvto Transporte").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Egreso").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Total")) & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Abonos")) & "</font></td>"
                        strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Saldo")) & "</font></td>"

                        strHtml += "</tr>"

                        strHtmlmostrar += "<tr>"

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
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Total")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Abonos")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & CInt(dtter.Rows(i)("Saldo")) & "</font></td>"


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
                strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#656DC1'>"
                strHtml += "<td align='right' colspan='11'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#656DC1'>"
                strHtmlmostrar += "<td align='right' colspan='11'><b><font size='1px'>TOTAL DESPACHADOR</font></b></td>"
                strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                strHtmlmostrar += "</tr>"

                strHtml += "<tr bgcolor='#656DC1'>"
                strHtml += "<td align='right' colspan='7'><b><font size='9px'>TOTALES</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtml += "</tr>"

                strHtmlmostrar += "<tr bgcolor='#656DC1'>"
                strHtmlmostrar += "<td align='right' colspan='7'><b><font size='1px'>TOTALES</font></b></td>"
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

                strHtmlCuadroFinal += "<tr bgcolor='#656DC1'>"
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL CXC</font></b></td>"
                strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                strHtmlCuadroFinal += "</tr>"

                strHtmlCuadroFinalMostrar += "<tr bgcolor='#656DC1'>"
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
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub
End Class
