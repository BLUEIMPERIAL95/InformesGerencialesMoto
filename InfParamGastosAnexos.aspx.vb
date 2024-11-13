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
Partial Class InfParamGastosAnexos
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

        strRespuestaPer = csusua.validar_permiso_usuario(16, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Sub combos()
        Dim dtter, dtgen, dtgas As New DataTable
        csoper.LlenarDropDownList("nombrecompleto", "idterceros", "terceros_mostrar_terceros_combo", dtter, cboterceros)
        csoper.LlenarDropDownList("nombrecompleto", "idgeneradores", "generadores_mostrar_todos", dtgen, cbogeneradores)
        csoper.LlenarDropDownList("descripcion", "id", "tipo_gastos_anexos_mostrar", dtgas, cbogastos)
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

                strSQL = "Select completar_ceros(movimientos_transportes.numero, 8) As 'Numero Mvto',"
                strSQL += " DATE_FORMAT(movimientos_transportes.fecha_movimiento, '%Y-%m-%d') As 'Fecha Movimiento',"
                strSQL += " completar_ceros(ventas.numero, 8) As 'Factura Nro', "
                strSQL += " DATE_FORMAT(ventas.fecha, '%Y-%m-%d') As 'Fecha Factura',"
                strSQL += " movimientos_transportes_consolidado.movimiento_operacion As 'Tipo Operacion',"
                strSQL += " movimientos_transportes_consolidado.vehiculo_placa As 'Placa',"
                strSQL += " movimientos_transportes_consolidado.generador_nombre As 'Generador',"
                strSQL += " CASE WHEN movimientos_transportes_consolidado.generador_id = 22 THEN (movimientos_transportes.flete_final_empresa + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) ELSE (movimientos_transportes.flete_final_empresa + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) END As 'Fte Empresa',"
                strSQL += " movimientos_transportes.flete_final_tercero As 'Fte Tercero',"
                strSQL += " COALESCE((Select sum(mga.valor) from movimientos_gastos_anexos mga left join tipo_gastos_anexos tga On mga.tipo_gastos_anexos_idtipo = tga.idtipo_gastos_anexos"
                strSQL += " WHERE mga.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes "
                strSQL += " And tga.descripcion Not Like '%COMISION POR TRANSPORTES%'), 0) as 'Gasto Anexo', "
                strSQL += " movimientos_gastos_anexos.valor As 'comision',"
                strSQL += " (CASE WHEN movimientos_transportes_consolidado.generador_id = 22 THEN (movimientos_transportes.flete_final_empresa + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) ELSE (movimientos_transportes.flete_final_empresa + COALESCE(movimientos_transportes_consolidado.movimiento_cnx_empresa, 0)) END - "
                strSQL += " movimientos_transportes.flete_final_tercero - "
                strSQL += " movimientos_gastos_anexos.valor - "
                strSQL += " COALESCE((select sum(mga.valor) from movimientos_gastos_anexos mga left join tipo_gastos_anexos tga on mga.tipo_gastos_anexos_idtipo = tga.idtipo_gastos_anexos "
                strSQL += " WHERE mga.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes "
                strSQL += " And tga.descripcion Not Like '%COMISION POR TRANSPORTES%'), 0)) as ingreso,"
                strSQL += " CONCAT(gasto.nombre1, ' ', gasto.nombre2, ' ', gasto.apellido1, ' ', gasto.apellido2) As 'Tercero Gasto Anexo'"
                strSQL += " From movimientos_transportes"
                strSQL += " inner Join movimientos_transportes_consolidado on movimientos_transportes_consolidado.movimiento_id = movimientos_transportes.idmovimientos_transportes"
                strSQL += " inner Join movimientos_gastos_anexos on movimientos_gastos_anexos.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
                strSQL += " inner Join tipo_gastos_anexos on tipo_gastos_anexos.idtipo_gastos_anexos = movimientos_gastos_anexos.tipo_gastos_anexos_idtipo"
                strSQL += " inner Join terceros gasto on gasto.idterceros = movimientos_gastos_anexos.terceros_idterceros"
                strSQL += " inner Join movimientos_transportes_detalles on movimientos_transportes_detalles.movimientos_transportes_idmovimientos = movimientos_transportes.idmovimientos_transportes"
                strSQL += " inner Join ventas_control on (ventas_control.movimientos_transportes_id = movimientos_transportes.idmovimientos_transportes And ventas_control.idel = 0)"
                strSQL += " inner Join ventas_detalles on (ventas_detalles.idventas_detalles = ventas_control.ventas_detalles_id And ventas_detalles.idel = 0)"
                strSQL += " inner Join ventas	on ventas.idventas = ventas_detalles.ventas_idventas"
                strSQL += " where ventas.fecha BETWEEN '" & txtFechaInicio.Value & "' and '" & txtFechaFin.Value & "'"
                strSQL += " And movimientos_transportes.flete_final_tercero > 0 And movimientos_transportes.tipo_estados_idtipo_estados <> 9"
                'strSQL += " And tipo_gastos_anexos.descripcion Like '%COMISION POR TRANSPORTES%'"
                strSQL += " And movimientos_gastos_anexos.valor > 0"
                If cboterceros.SelectedValue > 0 Then
                    strSQL += " And movimientos_gastos_anexos.terceros_idterceros = " & cboterceros.SelectedValue & ""
                End If

                If cbogeneradores.SelectedValue > 0 Then
                    strSQL += " And movimientos_transportes_consolidado.generador_id = " & cbogeneradores.SelectedValue & ""
                End If

                If cbogastos.SelectedValue > 0 Then
                    strSQL += " And tipo_gastos_anexos.idtipo_gastos_anexos = " & cbogastos.SelectedValue & ""
                End If
                'strSQL += " Group BY completar_ceros(movimientos_transportes.numero, 8),"
                'strSQL += " movimientos_transportes.fecha_movimiento,"
                'strSQL += " completar_ceros(ventas.numero, 8),"
                'strSQL += " ventas.fecha,"
                'strSQL += " movimientos_transportes_consolidado.movimiento_operacion,"
                'strSQL += " movimientos_transportes_consolidado.vehiculo_placa,"
                'strSQL += " movimientos_transportes_consolidado.generador_nombre"
                strSQL += " ORDER BY CONCAT(gasto.nombre1,' ',gasto.nombre2,' ',gasto.apellido1,' ',gasto.apellido2), movimientos_transportes.numero"

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
                    strHtml += "<td align='left'><img src='" & urlFotoCabeza1 & "' height='80' width='180'></td>"
                    strHtml += "<td align='center' colspan='5'><b><font size='15px'>GASTOS ANEXOS URB, NAL, INTER(" & txtFechaInicio.Value & " Hasta " & txtFechaFin.Value & ")</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='15'><b><font size='11px'>Tercero: " & dtter.Rows(0)("Tercero Gasto Anexo").ToString & "</font></b></td>"
                    strHtml += "</tr>"

                    'strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='9px'>Mvto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Fecha Mvto</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Fact Nro</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Fecha Fact</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='9px'>Oper</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                    strHtml += "<td align='left' colspan='2'><b><font size='9px'>Generador</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Fte Empresa</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Fte Tercero</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Gastos Ane</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Comision</font></b></td>"
                    strHtml += "<td align='right'><b><font size='9px'>Ingreso</font></b></td>"
                    strHtml += "<td align='center'><b><font size='9px'>%</font></b></td>"
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
                    strHtmlmostrar += "<td align='center' colspan='5'><b><font size='1px'>GASTOS ANEXOS URB, NAL, INTER(" & txtFechaInicio.Value & " Hasta " & txtFechaFin.Value & ")</font></b></td>"
                    strHtmlmostrar += "</tr>"
                    strHtmlmostrar += "</table>"

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center' colspan='15'><b><font size='1px'>Tercero: " & dtter.Rows(0)("Tercero Gasto Anexo").ToString & "</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    'strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Mvto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Mvto</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fact Nro</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Fact</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Oper</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Generador</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Tercero</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos Ane</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Comision</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Ingreso</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>%</font></b></td>"
                    strHtmlmostrar += "</tr>"

                    Dim strTerActual As String
                    Dim decTotal1, decTotal2, decTotal3, decTotal4, decTotal5, intvianal, intviaurb As Decimal
                    Dim cont As Integer
                    decTotal1 = 0
                    decTotal2 = 0
                    decTotal3 = 0
                    decTotal4 = 0
                    decTotal5 = 0
                    intvianal = 0
                    intviaurb = 0
                    cont = 0
                    strTerActual = dtter.Rows(0)("Tercero Gasto Anexo").ToString
                    For i As Integer = 0 To dtter.Rows.Count - 1
                        If strTerActual = dtter.Rows(i)("Tercero Gasto Anexo").ToString Then
                            strHtml += "<tr>"

                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Numero Mvto").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Movimiento").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Factura Nro").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Factura").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Empresa")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Tercero")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Gasto Anexo")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("comision")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("ingreso")) & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & Math.Round((dtter.Rows(i)("ingreso") / dtter.Rows(i)("Fte Empresa")) * 100, 1) & "</font></td>"

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Movimiento").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura Nro").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Factura").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Empresa")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Tercero")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Gasto Anexo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("comision")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("ingreso")) & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & Math.Round((dtter.Rows(i)("ingreso") / dtter.Rows(i)("Fte Empresa")) * 100, 1) & "</font></td>"

                            strHtmlmostrar += "</tr>"

                            decTotal1 = decTotal1 + dtter.Rows(i)("Fte Empresa")
                            decTotal2 = decTotal2 + dtter.Rows(i)("Fte Tercero")
                            decTotal3 = decTotal3 + dtter.Rows(i)("Gasto Anexo")
                            decTotal4 = decTotal4 + dtter.Rows(i)("comision")
                            decTotal5 = decTotal5 + dtter.Rows(i)("ingreso")
                            cont = cont + 1
                        Else

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
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                            strHtmlmostrar += "</tr>"

                            strHtml += "<tr>"
                            strHtml += "<td align='center' colspan='2'><b><font size='9px'>TOTALES: </font></b></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & cont & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Fte Empresa</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Fte Tercero</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Gastos Ane</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Comision</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Ingreso</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td></tr></table></td>"
                            strHtml += "<td align='center'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>% Inter</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & Math.Round((decTotal5 / decTotal1) * 100, 1) & "</font></b></td></tr></table></td>"
                            strHtml += "</tr>"

                            strHtml += "</table><p style='page-break-after:always;'></p>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='center' colspan='2'><b><font size='1px'>TOTALES: </font></b></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & cont & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Fte Empresa</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Fte Tercero</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Gastos Ane</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Comision</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Ingreso</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "<td align='center'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>% Inter</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & Math.Round((decTotal5 / decTotal1) * 100, 1) & "</font></b></td></tr></table></td>"
                            strHtmlmostrar += "</tr>"

                            strHtmlmostrar += "</table>"

                            strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                            strHtml += "<tr>"
                            strHtml += "<td align='center' colspan='15'><b><font size='11px'>Tercero: " & dtter.Rows(i)("Tercero Gasto Anexo").ToString & "</font></b></td>"
                            strHtml += "</tr>"

                            strHtml += "<tr>"
                            strHtml += "<td align='center'><b><font size='9px'>Mvto</font></b></td>"
                            strHtml += "<td align='center'><b><font size='9px'>Fecha Mvto</font></b></td>"
                            strHtml += "<td align='center'><b><font size='9px'>Fact Nro</font></b></td>"
                            strHtml += "<td align='center'><b><font size='9px'>Fecha Fact</font></b></td>"
                            strHtml += "<td align='left' colspan='2'><b><font size='9px'>Oper</font></b></td>"
                            strHtml += "<td align='center'><b><font size='9px'>Placa</font></b></td>"
                            strHtml += "<td align='left' colspan='2'><b><font size='9px'>Generador</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>Fte Empresa</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>Fte Tercero</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>Gastos Ane</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>Comision</font></b></td>"
                            strHtml += "<td align='right'><b><font size='9px'>Ingreso</font></b></td>"
                            strHtml += "<td align='center'><b><font size='9px'>%</font></b></td>"
                            strHtml += "</tr>"

                            strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='center' colspan='15'><b><font size='1px'>Tercero: " & dtter.Rows(i)("Tercero Gasto Anexo").ToString & "</font></b></td>"
                            strHtmlmostrar += "</tr>"

                            strHtmlmostrar += "<tr>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>Mvto</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Mvto</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>Fact Nro</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>Fecha Fact</font></b></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Oper</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><b><font size='1px'>Generador</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Empresa</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>Fte Tercero</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>Gastos Ane</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>Comision</font></b></td>"
                            strHtmlmostrar += "<td align='right'><b><font size='1px'>Ingreso</font></b></td>"
                            strHtmlmostrar += "<td align='center'><b><font size='1px'>%</font></b></td>"
                            strHtmlmostrar += "</tr>"

                            strHtml += "<tr>"

                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Numero Mvto").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Movimiento").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Factura Nro").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Fecha Factura").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtml += "<td align='left' colspan='2'><font size='9px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Empresa")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Tercero")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("Gasto Anexo")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("comision")) & "</font></td>"
                            strHtml += "<td align='right'><font size='9px'>" & String.Format("{0:c}", dtter.Rows(i)("ingreso")) & "</font></td>"
                            strHtml += "<td align='center'><font size='9px'>" & Math.Round((dtter.Rows(i)("ingreso") / dtter.Rows(i)("Fte Empresa")) * 100, 1) & "</font></td>"

                            strHtml += "</tr>"

                            strHtmlmostrar += "<tr>"

                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Numero Mvto").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Movimiento").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Factura Nro").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Fecha Factura").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Tipo Operacion").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & dtter.Rows(i)("Placa").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='left' colspan='2'><font size='1px'>" & dtter.Rows(i)("Generador").ToString & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Empresa")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Fte Tercero")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("Gasto Anexo")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("comision")) & "</font></td>"
                            strHtmlmostrar += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtter.Rows(i)("ingreso")) & "</font></td>"
                            strHtmlmostrar += "<td align='center'><font size='1px'>" & Math.Round((dtter.Rows(i)("ingreso") / dtter.Rows(i)("Fte Empresa")) * 100, 1) & "</font></td>"

                            strHtmlmostrar += "</tr>"

                            strTerActual = dtter.Rows(i)("Tercero Gasto Anexo").ToString

                            decTotal1 = 0
                            decTotal2 = 0
                            decTotal3 = 0
                            decTotal4 = 0
                            decTotal5 = 0
                            cont = 0

                            decTotal1 = decTotal1 + dtter.Rows(i)("Fte Empresa")
                            decTotal2 = decTotal2 + dtter.Rows(i)("Fte Tercero")
                            decTotal3 = decTotal3 + dtter.Rows(i)("Gasto Anexo")
                            decTotal4 = decTotal4 + dtter.Rows(i)("comision")
                            decTotal5 = decTotal5 + dtter.Rows(i)("ingreso")
                            cont = cont + 1
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
                    strHtml += "<td align='left'><b><font size='1'></font></b></td>"
                    strHtml += "</tr>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'></font></b></td>"
                    strHtmlmostrar += "</tr>"

                    strHtml += "<tr>"
                    strHtml += "<td align='center' colspan='2'><b><font size='9px'>TOTALES: </font></b></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & cont & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Fte Empresa</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Fte Tercero</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Gastos Ane</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Comision</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>Ingreso</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td></tr></table></td>"
                    strHtml += "<td align='center'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='9px'>% Inter</font></b></td></tr><tr><td align='center'><b><font size='9px'>" & Math.Round((decTotal5 / decTotal1) * 100, 1) & "</font></b></td></tr></table></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center' colspan='2'><b><font size='1px'>TOTALES: </font></b></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Viajes</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & cont & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Fte Empresa</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal1) & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Fte Tercero</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal2) & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Gastos Ane</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal3) & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Comision</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal4) & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center' colspan='2'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>Ingreso</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & String.Format("{0:c}", decTotal5) & "</font></b></td></tr></table></td>"
                    strHtmlmostrar += "<td align='center'><table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'><tr><td align='center'><b><font size='1px'>% Inter</font></b></td></tr><tr><td align='center'><b><font size='1px'>" & Math.Round((decTotal5 / decTotal1) * 100, 1) & "</font></b></td></tr></table></td>"
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

            strNombreInforme = "Gastos Anexos Nacional e Inter desde " & txtFechaInicio.Value & " hasta " & txtFechaFin.Value

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
