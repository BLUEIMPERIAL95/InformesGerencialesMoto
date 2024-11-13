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
Partial Class InfParamAdministradorCambiadero
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim cscamb As New cambiadero

    Private Sub InfParamAdministradorCambiadero_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3065, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Private Sub combos()
        Dim dtusu As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_usuarios_combo", dtusu, cboUsuario)
    End Sub

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            Dim dtCam As New DataTable

            If txtFechaInicio.Value = "" And txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""

                Dim strHtml, strHtmlCuadroFinal As String
                strHtml = ""
                strHtmlCuadroFinal = ""

                strHtmlCuadroFinal += "<br /><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"

                If cboUsuario.SelectedValue = "0" Then
                    dtCam = cscamb.seleccionar_administrador_cambiadero_por_usuario_fecha(txtFechaInicio.Value, txtFechaFin.Value, Session("id_usua"))
                Else
                    dtCam = cscamb.seleccionar_administrador_cambiadero_por_usuario_fecha(txtFechaInicio.Value, txtFechaFin.Value, cboUsuario.SelectedValue)
                End If

                Dim dtcamdia As New DataTable

                If dtCam.Rows.Count > 0 Then
                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr bgcolor='#F9D2C6'>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Empresa</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Agencia</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Egreso</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Tercero</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>M.Pago</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Porcentaje</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Descuento</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Total</font></b></td>"
                    strHtml += "</tr>"

                    Dim strEmpActual, strFechaActual As String
                    strEmpActual = dtCam.Rows(0)("nombre_emor").ToString
                    strFechaActual = dtCam.Rows(0)("fecha_adca").ToString
                    Dim valorempr, valortot, descemp, desctot, totalemp, totaltot, valordia, valordiades, valordiatot, valordebotener As Integer
                    valorempr = 0
                    valortot = 0
                    descemp = 0
                    desctot = 0
                    totalemp = 0
                    totaltot = 0
                    valordia = 0
                    valordiades = 0
                    valordiatot = 0
                    valordebotener = 0

                    strHtmlCuadroFinal += "<tr bgcolor='#F9D2C6'>"
                    strHtmlCuadroFinal += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>Descuento</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>Total</font></b></td>"
                    'strHtmlCuadroFinal += "<td align='center'><b><font size='1px'>Debo Tener</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    For i As Integer = 0 To dtCam.Rows.Count - 1
                        'If strEmpActual = dtCam.Rows(i)("nombre_emor").ToString Then
                        If strFechaActual = dtCam.Rows(i)("fecha_adca").ToString Then
                            strHtml += "<tr>"
                            strHtml += "<td align='center'><font size='1px'>" & dtCam.Rows(i)("fecha_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_emor").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_agcc").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("egreso_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("tercero_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("medpago_adca").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("valor_adca")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & dtCam.Rows(i)("porcentaje_adca").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("descuento_adca")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("total_adca")) & "</font></td>"
                            strHtml += "</tr>"

                            valordia = valordia + dtCam.Rows(i)("valor_adca")
                            valordiades = valordiades + dtCam.Rows(i)("descuento_adca")
                            valordiatot = valordiatot + dtCam.Rows(i)("total_adca")
                            valorempr = valorempr + dtCam.Rows(i)("valor_adca")
                            valortot = valortot + dtCam.Rows(i)("valor_adca")
                            descemp = descemp + dtCam.Rows(i)("descuento_adca")
                            desctot = desctot + dtCam.Rows(i)("descuento_adca")
                            totalemp = totalemp + dtCam.Rows(i)("total_adca")
                            totaltot = totaltot + dtCam.Rows(i)("total_adca")
                        Else
                            strHtml += "<tr>"
                            strHtml += "<td align='center'><font size='1px'>" & dtCam.Rows(i)("fecha_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_emor").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("nombre_agcc").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("egreso_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("tercero_adca").ToString & "</font></td>"
                            strHtml += "<td align='left'><font size='1px'>" & dtCam.Rows(i)("medpago_adca").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("valor_adca")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & dtCam.Rows(i)("porcentaje_adca").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("descuento_adca")) & "</font></td>"
                            strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtCam.Rows(i)("total_adca")) & "</font></td>"
                            strHtml += "</tr>"

                            dtcamdia = cscamb.seleccionar_saldos_cambiadero_por_fecha_usuario(Session("id_usua"), strFechaActual)

                            If dtcamdia.Rows.Count > 0 Then
                                valordebotener = dtcamdia.Rows(0)("saldoini_saca")
                            Else
                                valordebotener = 0
                            End If

                            'totaltot = totaltot + dtCam.Rows(i)("total_adca")

                            strHtmlCuadroFinal += "<tr>"
                            strHtmlCuadroFinal += "<td align='center'><b><font size='1px'>" & strFechaActual & ": </font></b></td>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordia) & "</font></b></td>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordiades) & "</font></b></td>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordiatot) & "</font></b></td>"
                            'strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordebotener - totaltot) & "</font></b></td>"
                            strHtmlCuadroFinal += "</tr>"

                            valordia = 0
                            valordiades = 0
                            valordiatot = 0
                            valorempr = valorempr + dtCam.Rows(i)("valor_adca")
                            valortot = valortot + dtCam.Rows(i)("valor_adca")
                            descemp = descemp + dtCam.Rows(i)("descuento_adca")
                            desctot = desctot + dtCam.Rows(i)("descuento_adca")
                            totalemp = totalemp + dtCam.Rows(i)("total_adca")
                            totaltot = totaltot + dtCam.Rows(i)("total_adca")
                            valordia = valordia + dtCam.Rows(i)("valor_adca")
                            valordiades = valordiades + dtCam.Rows(i)("descuento_adca")
                            valordiatot = valordiatot + dtCam.Rows(i)("total_adca")

                            strFechaActual = dtCam.Rows(i)("fecha_adca").ToString
                        End If
                    Next

                    dtcamdia = cscamb.seleccionar_saldos_cambiadero_por_fecha_usuario(Session("id_usua"), strFechaActual)

                    If dtcamdia.Rows.Count > 0 Then
                        valordebotener = dtcamdia.Rows(0)("saldoini_saca")
                    Else
                        valordebotener = 0
                    End If

                    strHtmlCuadroFinal += "<tr>"
                    strHtmlCuadroFinal += "<td align='center'><b><font size='1px'>" & strFechaActual & ": </font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordia) & "</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordiades) & "</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordiatot) & "</font></b></td>"
                    'strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valordebotener - totaltot) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='center' colspan='6'><b><font size='1px'>TOTALES</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valortot) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'></font></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", desctot) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", totaltot) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtmlCuadroFinal += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinal += "<td align='center'><b><font size='1px'>TOTALES</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", valortot) & "</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", desctot) & "</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", totaltot) & "</font></b></td>"
                    'strHtmlCuadroFinal += "<td align='right'><b><font size='1px'></font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinal += "</table>"

                    strHtml += strHtmlCuadroFinal

                    Dim saldoInicial As Decimal
                    dtcamdia = cscamb.seleccionar_saldos_cambiadero_por_fecha_usuario(Session("id_usua"), txtFechaInicio.Value)

                    If dtcamdia.Rows.Count > 0 Then
                        saldoInicial = dtcamdia.Rows(0)("saldoini_saca")
                    Else
                        saldoInicial = 0
                    End If

                    strHtml += "<br /><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"

                    strHtml += "<tr bgcolor='#F090C0'>"
                    strHtml += "<td align='center'><b><font size='1px'>UTILIDAD</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", desctot) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#F090C0'>"
                    strHtml += "<td align='center'><b><font size='1px'>DEDUCCIONES</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>" & txtNombresGastos.Text & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", CDec(txtGastos.Text)) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#F090C0'>"
                    strHtml += "<td align='center'><b><font size='1px'>UTILIDAD NETA</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", desctot - CDec(txtGastos.Text)) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#F090C0'>"
                    strHtml += "<td align='center'><b><font size='1px'>CAMBIADERO(" & txtPorcentaje.Text & "%)</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", (desctot - CDec(txtGastos.Text)) * (txtPorcentaje.Text / 100)) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "<tr bgcolor='#F090C0'>"
                    strHtml += "<td align='center'><b><font size='1px'>UTILIDAD DEL MES</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", desctot - CInt(txtGastos.Text) - (desctot - CDec(txtGastos.Text)) * (txtPorcentaje.Text / 100)) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtml += "<br /><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='50%'>"

                    strHtml += "<tr bgcolor='#90DAF0'>"
                    strHtml += "<td align='center'><b><font size='1px'>SALDO MES ANTERIOR</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", saldoInicial) & "</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "<tr bgcolor='#90DAF0'>"
                    strHtml += "<td align='center'><b><font size='1px'>SALDO MES PROXIMO</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", saldoInicial + desctot - CInt(txtGastos.Text) - (desctot - CDec(txtGastos.Text)) * (txtPorcentaje.Text / 100)) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                    divinforme.InnerHtml = strHtml
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
