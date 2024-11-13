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
Partial Class InfParamAnticipos
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

        strRespuestaPer = csusua.validar_permiso_usuario(17, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            If (txtFechaInicio.Value = "" And txtFechaFin.Value = "") And cboTipo.SelectedValue = -1 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""
                Dim intAño As Integer
                Dim strTipo As String

                If cboTipo.SelectedValue = 1 Then
                    intAño = Year(Now)
                    strTipo = "Cartera Desde: " & intAño
                Else
                    If cboTipo.SelectedValue = 2 Then
                        intAño = Year(Now) - 1
                        strTipo = "Prejurídica Desde: " & intAño
                    Else
                        If cboTipo.SelectedValue = 3 Then
                            intAño = Year(Now) - 2
                            strTipo = "Jurídica Desde: " & intAño
                        Else
                            strTipo = "Completa"
                        End If
                    End If
                End If

                strSQL = " Select movimientos_transportes_anticipos_consolidado.sucursal_descripcion As Sucursal, "
                strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador,"
                strSQL += " movimientos_transportes_consolidado.movimiento_numero As NumMov, "
                strSQL += " DATE_FORMAT(movimientos_transportes_anticipos_consolidado.anticipo_fecha,'%Y-%m-%d') AS Fecha,"
                strSQL += " movimientos_transportes_consolidado.generador_nombre As Generador, "
                strSQL += " COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') AS Factura,"
                strSQL += " movimientos_transportes_consolidado.vehiculo_placa As Placa, "
                strSQL += " movimientos_transportes_consolidado.movimiento_fl_empresa +movimientos_transportes_consolidado.movimiento_cnx_empresa As FleteEmp,"
                strSQL += " movimientos_transportes_consolidado.movimiento_fl_tercero +movimientos_transportes_consolidado.movimiento_cnx_tercero As FleteTer,"
                strSQL += " movimientos_transportes_anticipos_consolidado.anticipo_valor AS Valor,"
                strSQL += " movimientos_transportes_consolidado.movimiento_anexos AS Gastos,"
                strSQL += " DateDiff(Now(), movimientos_transportes_anticipos_consolidado.anticipo_fecha) As Dias"
                strSQL += " From movimientos_transportes_anticipos_consolidado"
                strSQL += " INNER Join movimientos_transportes_anticipos ON movimientos_transportes_anticipos_consolidado.anticipo_id = movimientos_transportes_anticipos.idmovimientos_transportes_anticipos"
                strSQL += " INNER Join movimientos_transportes_consolidado ON movimientos_transportes_anticipos_consolidado.movimiento_id = movimientos_transportes_consolidado.movimiento_id"
                strSQL += " INNER Join movimientos_contables ON movimientos_transportes_anticipos.movimientos_contables_idcontabilidad = movimientos_contables.idcontabilidad"
                strSQL += " INNER Join generadores_asesores ON movimientos_transportes_consolidado.asesor_id = generadores_asesores.idgeneradores_asesores"
                strSQL += " INNER Join sistema_sucursales ON movimientos_contables.sistema_sucursales_idsucursales = sistema_sucursales.idsucursales"
                If txtFechaInicio.Value <> "" And txtFechaFin.Value <> "" Then
                    strSQL += " WHERE movimientos_contables.fecha BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                    strSQL += " And YEAR(movimientos_contables.fecha) >= 2018"
                    If cboTipo.SelectedValue <> -1 Then
                        If cboTipo.SelectedValue = 1 Or cboTipo.SelectedValue = 2 Or cboTipo.SelectedValue = 3 Then
                            strSQL += " AND YEAR(movimientos_contables.fecha) = " & intAño & ""
                        End If
                    End If
                Else
                    If cboTipo.SelectedValue <> -1 Then
                        If cboTipo.SelectedValue = 1 Or cboTipo.SelectedValue = 2 Or cboTipo.SelectedValue = 3 Then
                            strSQL += " WHERE YEAR(movimientos_contables.fecha) = " & intAño & ""
                            strSQL += " And YEAR(movimientos_contables.fecha) >= 2018"
                        Else
                            strSQL += " WHERE YEAR(movimientos_contables.fecha) >= 2018"
                        End If
                    End If
                End If
                strSQL += " And movimientos_transportes_consolidado.movimiento_estado <> 'ANULADO' "
                strSQL += " AND movimientos_contables.tipo_estados_idtipo_estados = 16 "
                strSQL += " And COALESCE(movimientos_transportes_consolidado.factura_numero, 'NO FACTURADO') = 'NO FACTURADO' "
                strSQL += " ORDER BY movimientos_transportes_anticipos_consolidado.sucursal_descripcion, movimientos_transportes_consolidado.despachador_nombre, "
                strSQL += " movimientos_transportes_anticipos_consolidado.anticipo_fecha"

                'strSQL = " Select DATE_FORMAT(movimientos_transportes_anticipos_consolidado.anticipo_fecha,'%Y-%m-%d') AS Fecha,"
                'strSQL += " movimientos_transportes_anticipos_consolidado.sucursal_descripcion As Sucursal,"
                'strSQL += " movimientos_transportes_consolidado.movimiento_numero AS NumMov,"
                'strSQL += " movimientos_transportes_consolidado.vehiculo_placa As Placa,"
                'strSQL += " movimientos_transportes_consolidado.despachador_nombre AS Despachador,"
                'strSQL += " movimientos_transportes_anticipos_consolidado.anticipo_valor AS Valor"
                'strSQL += " From movimientos_transportes_anticipos_consolidado"
                'strSQL += " Left Join movimientos_transportes_consolidado ON movimientos_transportes_anticipos_consolidado.movimiento_id = movimientos_transportes_consolidado.movimiento_id"

                'If cboTipo.SelectedValue <> -1 Then
                '    If cboTipo.SelectedValue = 1 Or cboTipo.SelectedValue = 2 Or cboTipo.SelectedValue = 3 Then
                '        strSQL += " where YEAR(movimientos_transportes_anticipos_consolidado.anticipo_fecha) = " & intAño & ""
                '    End If
                'Else
                '    strSQL += " where movimientos_transportes_anticipos_consolidado.anticipo_fecha BETWEEN '"
                '    strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "'"
                'End If

                'If cboTipo.SelectedValue <> -1 And cboTipo.SelectedValue = 0 Then
                '    strSQL += " Where movimientos_transportes_consolidado.idtipo_estados = 30 And movimientos_transportes_consolidado.egreso_id Is NULL"
                'Else
                '    strSQL += " And movimientos_transportes_consolidado.idtipo_estados = 30 And movimientos_transportes_consolidado.egreso_id Is NULL"
                'End If

                'strSQL += " And movimientos_transportes_anticipos_consolidado.anticipo_valor > 0"
                'strSQL += " ORDER BY movimientos_transportes_anticipos_consolidado.anticipo_fecha"

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
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "<td align='center'><b><font size='4'></font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</tr>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtml += "<td align='center' colspan='6'><b><font size='4'>Anticipos sin Liquidar(Tipo: " & strTipo & " * " & txtFechaInicio.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='left'><b><font size='7px'>Sucursal</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='7px'>Despachador</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Numero Movimiento</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Fecha Movimiento</font></b></td>"
                    strHtml += "<td align='center' colspan='3'><b><font size='7px'>Generador</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Factura</font></b></td>"
                    strHtml += "<td align='center'><b><font size='7px'>Placa</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>F.Empresa</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>F.Tercero</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Valor</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Gastos</font></b></td>"
                    strHtml += "<td align='right'><b><font size='7px'>Dias</font></b></td>"
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
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Movimiento</font></b></td>"
                    strHtmlmostrar += "<td align='center' colspan='3'><b><font size='1px'>Generador</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Factura</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>F.Empresa</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>F.Tercero</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Dias</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    Dim decTotalAnticipos, decTotalDespachador, decTotalSucursal As Decimal
                    Dim strSucActual, strDespActual As String
                    decTotalDespachador = 0
                    decTotalSucursal = 0
                    decTotalAnticipos = 0
                    strSucActual = dtter.Rows(0)("Sucursal").ToString
                    strDespActual = dtter.Rows(0)("Despachador").ToString
                    For i As Integer = 0 To dtter.Rows.Count - 1
                        If strSucActual = dtter.Rows(i)("Sucursal").ToString Then
                            If strDespActual = dtter.Rows(i)("Despachador").ToString Then
                                strHtml += "<tr>"

                                strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                                strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("NumMov").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                                strHtml += "<td align='left' colspan='3'><font size='7px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteEmp")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteTer")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Gastos")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Dias").ToString & "</font></td>"

                                strHtml += "</tr>"
                            Else
                                strHtml += "<tr bgcolor='#BDBDBD'>"
                                strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                                strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                                strHtml += "</tr>"

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
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("NumMov").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                                strHtml += "<td align='left' colspan='3'><font size='7px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                                strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteEmp")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteTer")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Gastos")) & "</font></td>"
                                strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Dias").ToString & "</font></td>"

                                strHtml += "</tr>"
                            End If
                        Else
                            strHtml += "<tr bgcolor='#BDBDBD'>"
                            strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                            strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                            strHtml += "</tr>"

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

                            strHtml += "<tr bgcolor='#BDBDBD'>"
                            strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL SUCURSAL</font></b></td>"
                            strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                            strHtml += "</tr>"

                            strHtmlCuadroFinal += "<tr bgcolor='#BDBDBD'>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & strSucActual & ": </font></b></td>"
                            strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                            strHtmlCuadroFinal += "</tr>"

                            strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                            strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & strSucActual & ": </font></b></td>"
                            strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                            strHtmlCuadroFinalMostrar += "</tr>"

                            decTotalSucursal = 0

                            strSucActual = dtter.Rows(i)("Sucursal").ToString

                            strHtml += "<tr>"

                            strHtml += "<td align='left'><font size='7px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='7px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("NumMov").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='3'><font size='7px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='7px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteEmp")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteTer")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & String.Format("{0:c}", dtter.Rows(i)("Gastos")) & "</font></td>"
                            strHtml += "<td align='right'><font size='7px'>" & dtter.Rows(i)("Dias").ToString & "</font></td>"

                            strHtml += "</tr>"
                        End If

                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='left'><font size='1px'>" & dtter.Rows(i)("Sucursal").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Despachador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("NumMov").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='left' colspan='3'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteEmp")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("FleteTer")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Gastos")) & "</font></td>"
                        strHtmlmostrar += "<td align='right'><font size='1px'>" & dtter.Rows(i)("Dias").ToString & "</font></td>"

                        strHtmlmostrar += "</tr>"

                        decTotalDespachador = decTotalDespachador + dtter.Rows(i)("Valor")
                        decTotalSucursal = decTotalSucursal + dtter.Rows(i)("Valor")
                        decTotalAnticipos = decTotalAnticipos + dtter.Rows(i)("Valor")
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
                    strHtmlmostrar += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL DESPACHADOR</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlCuadroFinal += "<tr>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & strDespActual & ": </font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinalMostrar += "<tr>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & strDespActual & ": </font></b></td>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalDespachador) & "</font></b></td>"
                    strHtmlCuadroFinalMostrar += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='right' colspan='13'><b><font size='8px'>TOTAL SUCURSAL</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='8px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlCuadroFinal += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & strSucActual & ": </font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='8px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & strSucActual & ": </font></b></td>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalSucursal) & "</font></b></td>"
                    strHtmlCuadroFinalMostrar += "</tr>"

                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='right' colspan='13'><b><font size='9px'>TOTAL ANTICIPOS</font></b></td>"
                    strHtml += "<td align='right' colspan='2'><b><font size='9px'>" & String.Format("{0:c}", decTotalAnticipos) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlCuadroFinal += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>TOTAL ANTICIPOS</font></b></td>"
                    strHtmlCuadroFinal += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotalAnticipos) & "</font></b></td>"
                    strHtmlCuadroFinal += "</tr>"

                    strHtmlCuadroFinalMostrar += "<tr bgcolor='#BDBDBD'>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>TOTAL ANTICIPOS</font></b></td>"
                    strHtmlCuadroFinalMostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotalAnticipos) & "</font></b></td>"
                    strHtmlCuadroFinalMostrar += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center' colspan='13'><b><font size='1px'>TOTAL ANTICIPOS</font></b></td>"
                    strHtmlmostrar += "<td align='right' colspan='2'><b><font size='1px'>" & String.Format("{0:c}", decTotalAnticipos) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

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

            strNombreInforme = "Anticipos desde " & txtFechaInicio.Value

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
