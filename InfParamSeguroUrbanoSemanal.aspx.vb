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

Partial Class InfParamSeguroUrbanoSemanal
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

        strRespuestaPer = csusua.validar_permiso_usuario(11, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Protected Sub btnPrueba_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrueba.Click
        Try
            Dim strSQL As String
            Dim dtter As New DataTable

            If txtFechaInicio.Value = "" Or txtFechaFin.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Fechas Inválidas...');", True)
            Else
                divmostrar.InnerHtml = ""
                divinforme.InnerHtml = ""
                Dim strPlacaActual As String

                strSQL = "Select completar_ceros(movimientos_transportes.numero, 8) As 'Mvto',"
                strSQL += " vehiculos.placa,"
                strSQL += " DATE_FORMAT(movimientos_transportes.fecha_movimiento, '%Y-%m-%d') as 'Fecha Mvto',"
                strSQL += " DATE_FORMAT(ventas.fecha, '%Y-%m-%d') As 'Fecha Factura', "
                strSQL += " CONCAT(gen.nombre1, ' ', gen.nombre2, ' ', gen.apellido1, ' ', gen.apellido2) As 'Generador',"
                strSQL += " tipo_despachos.descripcion As 'Tipo Operacion',"
                strSQL += " Case WHEN tipo_descuentos_transportes.indicador = 2 THEN"
                strSQL += " ROUND(((movimientos_transportes.flete_final_tercero * tipo_descuentos_transportes.valor_porcentaje) / 100), 0)"
                strSQL += " Else"
                strSQL += " ROUND((tipo_descuentos_transportes.valor_entero), 0) End As 'Valor Aseguradora' "
                strSQL += " From movimientos_transportes"
                strSQL += " Left Join generadores on generadores.idgeneradores = movimientos_transportes.generadores_idgeneradores"
                strSQL += " Left Join terceros gen on gen.idterceros = generadores.terceros_idterceros"
                strSQL += " Left Join generadores_asesores on generadores_asesores.idgeneradores_asesores = movimientos_transportes.generadores_asesores_idgeneradores"
                strSQL += " Left Join usuarios desp on desp.idusuarios = movimientos_transportes.usuarios_idusuarios_ingresa"
                strSQL += " Left Join usuarios on usuarios.idusuarios = generadores_asesores.usuarios_idusuarios"
                strSQL += " Left Join terceros ase on ase.idterceros = usuarios.idterceros"
                strSQL += " Left Join movimientos_transportes_actores on movimientos_transportes_actores.idmovimientos_transportes_actores = movimientos_transportes.movimientos_transportes_actores"
                strSQL += " Left Join vehiculos_carga on vehiculos_carga.idvehiculos_carga = movimientos_transportes_actores.vehiculos_carga_idvehiculos_carga"
                strSQL += " Left Join vehiculos on vehiculos.idvehiculos = vehiculos_carga.vehiculos_idvehiculos"
                strSQL += " Left Join terceros_conductores on  terceros_conductores.idterceros_conductores = movimientos_transportes_actores.terceros_conductores_idterceros_conductores"
                strSQL += " Left Join terceros cond on cond.idterceros = terceros_conductores.terceros_idterceros"
                strSQL += " Left Join sistema_sucursales on sistema_sucursales.idsucursales = movimientos_transportes.sistema_sucursales_origina"
                strSQL += " Left Join tipo_despachos on tipo_despachos.idtipo_despachos = movimientos_transportes.tipo_despachos_idtipo_despachos"
                strSQL += " Left Join movimientos_transportes_detalles on movimientos_transportes_detalles.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
                strSQL += " Left Join ventas_control on (ventas_control.movimientos_transportes_id = movimientos_transportes.idmovimientos_transportes And ventas_control.idel = 0)"
                strSQL += " Left Join ventas_detalles on (ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id And ventas_detalles.idel = 0)"
                strSQL += " Left Join ventas	on ventas.idventas = ventas_detalles.ventas_idventas"
                strSQL += " Left Join  movimientos_transportes_descuentos on movimientos_transportes_descuentos.movimientos_transportes_idmovimientos_transportes = movimientos_transportes.idmovimientos_transportes"
                strSQL += " Left Join tipo_descuentos_transportes on tipo_descuentos_transportes.idtipo_descuentos_transportes = movimientos_transportes_descuentos.tipo_descuentos_transportes_idtipo_descuentos_transportes"
                strSQL += " where movimientos_transportes.fecha_movimiento BETWEEN '"
                strSQL += txtFechaInicio.Value + "' AND '" + txtFechaFin.Value + "'"
                strSQL += " And ((tipo_descuentos_transportes.descripcion_interna Like '%SEGURO TRAYECTOS%') OR (tipo_descuentos_transportes.descripcion_interna Like '%SEGURO DE TRAYECTOS%'))"
                strSQL += " And tipo_despachos.descripcion LIKE '%URBANO%' And (tipo_despachos.descripcion Like '%SEMANAL%' OR tipo_despachos.descripcion Like '%COLANTA%')"
                strSQL += " And movimientos_transportes.flete_final_tercero > 0 And movimientos_transportes.tipo_estados_idtipo_estados <> 9"
                strSQL += " And movimientos_transportes_descuentos.idel = 0"
                strSQL += " Group BY movimientos_transportes.idmovimientos_transportes ORDER BY vehiculos.placa, movimientos_transportes.idmovimientos_transportes"

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
                    strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtml += "<td align='center' colspan='5'><b><font size='4'>SEGURO TRAYECTO URBANO SEMANAL EMPRESA(" & txtFechaInicio.Value & " Hasta " & txtFechaFin.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Mvto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Fecha Mvto</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='9px'>Generador</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='9px'>Tipo Operacion</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Valor</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Valor Aseguradora</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Intermediación</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "</tr>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='60' width='160'></td>"
                    strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>SEGURO TRAYECTO URBANO EMPRESA(" & txtFechaInicio.Value & " Hasta " & txtFechaFin.Value & ")</font></b></td>"
                    strHtmlmostrar += "</tr>"
                    strHtmlmostrar += "</table>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Mvto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Mvto</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Generador</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Tipo Operacion</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Valor Aseguradora</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Intermediación</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    Dim decTotal1, decTotal2, decTotal3, decTotal4, decTotal5, decTotal6 As Decimal
                    Dim ContViajes, ContValor As Integer

                    decTotal1 = 0
                    decTotal2 = 0
                    decTotal3 = 0
                    decTotal4 = 0
                    decTotal5 = 0
                    decTotal6 = 0
                    ContViajes = 0
                    ContValor = 0

                    strPlacaActual = dtter.Rows(0)("placa").ToString
                    For i As Integer = 0 To dtter.Rows.Count - 1
                        If strPlacaActual = dtter.Rows(i)("placa").ToString Then
                            ContValor = ContValor + 1

                            strHtml += "<tr>"

                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Mvto").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Mvto").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            If ContValor Mod 6 = 0 Then
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 8092) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 8092) & "</font></td>"
                                decTotal2 = decTotal2 + 8092
                                decTotal3 = decTotal3 + (dtter.Rows(i)("Valor Aseguradora") - 8092)
                                decTotal5 = decTotal5 + 8092
                                decTotal6 = decTotal6 + (dtter.Rows(i)("Valor Aseguradora") - 8092)
                            Else
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 0) & "</font></td>"

                                decTotal3 = decTotal3 + (dtter.Rows(i)("Valor Aseguradora") - 0)
                                decTotal6 = decTotal6 + (dtter.Rows(i)("Valor Aseguradora") - 0)
                            End If

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            If ContValor Mod 6 = 0 Then
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 8092) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 8092) & "</font></td>"
                            Else
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 0) & "</font></td>"
                            End If

                            strHtmlmostrar += "</tr>"

                            decTotal1 = decTotal1 + dtter.Rows(i)("Valor Aseguradora")
                            decTotal4 = decTotal4 + dtter.Rows(i)("Valor Aseguradora")
                            'decTotal2 = decTotal2 + 8092
                            'decTotal3 = decTotal3 + (dtter.Rows(i)("Valor Aseguradora") - 8092)
                            ContViajes = ContViajes + 1
                        Else
                            ContValor = 1

                            strHtml += "<tr>"
                            strHtml += "<td align='center' colspan='5'><b><font size='9px'>TOTALES PLACA: </font></b></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & ContViajes & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>TOTALES PLACA: </font></b></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & ContViajes & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                            strHtmlmostrar += "</tr>"

                            decTotal1 = 0
                            decTotal2 = 0
                            decTotal3 = 0
                            ContViajes = 0

                            strPlacaActual = dtter.Rows(i)("placa").ToString

                            strHtml += "<tr>"

                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Mvto").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Mvto").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            If ContValor Mod 6 = 0 Then
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 8092) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 8092) & "</font></td>"
                                decTotal2 = decTotal2 + 8092
                                decTotal3 = decTotal3 + (dtter.Rows(i)("Valor Aseguradora") - 8092)
                                decTotal5 = decTotal5 + 8092
                                decTotal6 = decTotal6 + (dtter.Rows(i)("Valor Aseguradora") - 8092)
                            Else
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 0) & "</font></td>"

                                decTotal3 = decTotal3 + (dtter.Rows(i)("Valor Aseguradora") - 0)
                                decTotal6 = decTotal6 + (dtter.Rows(i)("Valor Aseguradora") - 0)
                            End If

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            If ContValor Mod 6 = 0 Then
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 8092) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 8092) & "</font></td>"
                            Else
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora")) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", 0) & "</font></td>"
                                strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Valor Aseguradora") - 0) & "</font></td>"
                            End If

                            strHtmlmostrar += "</tr>"

                            decTotal1 = decTotal1 + dtter.Rows(i)("Valor Aseguradora")
                            decTotal4 = decTotal4 + dtter.Rows(i)("Valor Aseguradora")
                            ContViajes = ContViajes + 1
                        End If
                    Next

                    'strHtml += "<tr>"
                    'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='center'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    'strHtml += "<td align='right'><b><font size='1'></font></b></td>"
                    'strHtml += "</tr>"

                    'strHtmlmostrar += "<tr>"
                    'strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='right'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "<td align='right'><b><font size='1px'></font></b></td>"
                    'strHtmlmostrar += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='5'><b><font size='9px'>TOTALES PLACA: </font></b></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & ContViajes & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>TOTALES PLACA: </font></b></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & ContViajes & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='5'><b><font size='9px'>TOTALES GENERAL: </font></b></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & dtter.Rows.Count & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>" & String.Format("{0:c}", decTotal6) & "</font></b></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>TOTALES GENERAL: </font></b></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & dtter.Rows.Count & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>" & String.Format("{0:c}", decTotal6) & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtmlmostrar += "</table>"

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
            divinforme.RenderControl(hw)
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

            strNombreInforme = "Seguro Urbano Empresa desde " & txtFechaInicio.Value & " hasta " & txtFechaFin.Value

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
