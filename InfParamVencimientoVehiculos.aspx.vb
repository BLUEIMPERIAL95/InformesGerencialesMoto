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
Partial Class InfParamVencimientoVehiculos
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

        strRespuestaPer = csusua.validar_permiso_usuario(2039, Session("id_usua"))

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

                strSQL = "Select DISTINCT vehiculos.idvehiculos, vehiculos.placa, "
                strSQL += "clases_vehiculares.descripcion AS tipo_vehiculo, "
                strSQL += "vehiculos.modelo, "
                strSQL += "vehiculos.capacidad, "
                strSQL += "vehiculos.configuracion, "
                strSQL += "terpro.documento As documentoProp, "
                strSQL += "CONCAT(terpro.nombre1, terpro.nombre2, terpro.apellido1, terpro.apellido2) As Propietario, "
                strSQL += "Case WHEN COALESCE((SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros = terpro.idterceros And telefonos.idprincipal = 1 LIMIT 1), 'SIN TEL') <> 'SIN TEL' "
                strSQL += "THEN (SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros = terpro.idterceros And telefonos.idprincipal = 1 LIMIT 1) ELSE "
                strSQL += "(SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros = terpro.idterceros And telefonos.idprincipal = 0 LIMIT 1) End As 'tel propietario', "
                strSQL += "tercon.documento as documentoCond, "
                strSQL += "CONCAT(tercon.nombre1, tercon.nombre2, tercon.apellido1, tercon.apellido2) As Conductor, "
                strSQL += "Case WHEN COALESCE((SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros =  tercon.idterceros And telefonos.idprincipal = 1 LIMIT 1), 'SIN TEL') <> 'SIN TEL' "
                strSQL += "THEN (SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros =  tercon.idterceros And telefonos.idprincipal = 1 LIMIT 1) ELSE "
                strSQL += "(SELECT telefonos.telefono FROM telefonos WHERE telefonos.terceros_idterceros =  tercon.idterceros And telefonos.idprincipal = 0 LIMIT 1) End As 'tel conductor', "
                strSQL += "COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 1 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE') AS SOAT, "
                strSQL += "COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 9 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE') AS 'POLIZA HIDROCARBUROS', "
                strSQL += "COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 10 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE') AS 'POLIZA RC' "
                strSQL += "From vehiculos "
                strSQL += "Left Join vehiculos_tenencias vt ON vehiculos.idvehiculos = vt.vehiculos_idvehiculos "
                strSQL += "Left Join terceros_propietarios terprop ON vt.terceros_propietarios_idterceros_propietarios = terprop.idterceros_propietarios "
                strSQL += "Left Join terceros terpro ON terpro.idterceros = terprop.terceros_idterceros "
                strSQL += "Left Join vehiculos_conductores vecon ON vecon.vehiculos_idvehiculos = vehiculos.idvehiculos "
                strSQL += "Left Join terceros_conductores tercond ON vecon.terceros_conductores_idterceros_conductores = tercond.idterceros_conductores "
                strSQL += "Left Join terceros tercon ON tercon.idterceros = tercond.terceros_idterceros "
                strSQL += "Left Join clases_vehiculares ON vehiculos.clases_vehiculares_idclases_vehiculares = clases_vehiculares.idclases_vehiculares "
                strSQL += "WHERE vt.tipo_tenencia = 1 And vt.idel = 0 And vecon.tipo_conductor = 1 And vecon.idel = 0 And vehiculos.idel = 0 "
                strSQL += "And ((COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 1 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE')) <> 'NO VENCE' "
                strSQL += "Or (COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 9 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE')) <> 'NO VENCE' "
                strSQL += "Or (COALESCE((SELECT vehiculos_documentos.fecha_vencimiento FROM vehiculos_documentos LEFT JOIN tipo_documentos_vehiculos ON tipo_documentos_vehiculos.idtipo_documentos_vehiculos = vehiculos_documentos.tipo_documentos_vehiculos_idtipo_documentos_vehiculos "
                strSQL += "WHERE tipo_documentos_vehiculos.idtipo_documentos_vehiculos = 10 And vehiculos_documentos.fecha_vencimiento BETWEEN '" & txtFechaInicio.Value & "' AND '" & txtFechaFin.Value & "' "
                strSQL += "And vehiculos_documentos.vehiculos_idvehiculos = vehiculos.idvehiculos ORDER BY vehiculos_documentos.fecha_vencimiento DESC LIMIT 1), 'NO VENCE')) <> 'NO VENCE') "
                strSQL += "ORDER BY vehiculos.placa "

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

                    strHtmlmostrar += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtmlmostrar += "<tr>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Placa</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Tipo</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Modelo</font></b></td>"
                    strHtmlmostrar += "<td align='right'><b><font size='1px'>Capacidad</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Configuracion</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Propietario</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Documento</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Telefono</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Conductor</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Documento</font></b></td>"
                    strHtmlmostrar += "<td align='left'><b><font size='1px'>Telefono</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Soat</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Poliza Hidro</font></b></td>"
                    strHtmlmostrar += "<td align='center'><b><font size='1px'>Poliza RC</font></b></td>"
                    strHtmlmostrar += "</tr>"


                    For i As Integer = 0 To dtter.Rows.Count - 1
                        strHtmlmostrar += "<tr>"

                        strHtmlmostrar += "<td align='center'><b><font size='1px'>" & dtter.Rows(i)("placa").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("tipo_vehiculo").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("modelo").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='right'><b><font size='1px'>" & dtter.Rows(i)("capacidad").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("configuracion").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("Propietario").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("documentoProp").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("tel propietario").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("Conductor").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("documentoCond").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='left'><b><font size='1px'>" & dtter.Rows(i)("tel conductor").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>" & dtter.Rows(i)("SOAT").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>" & dtter.Rows(i)("POLIZA HIDROCARBUROS").ToString & "</font></b></td>"
                        strHtmlmostrar += "<td align='center'><b><font size='1px'>" & dtter.Rows(i)("POLIZA RC").ToString & "</font></b></td>"

                        strHtmlmostrar += "</tr>"
                    Next

                    strHtmlmostrar += "</table>"

                    divmostrar.InnerHtml = strHtmlmostrar
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=VencimientoDocumentosTramitar.xls")
            Response.ContentEncoding = Encoding.UTF8
            Dim tw As StringWriter = New StringWriter()
            Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
            'Me.Page.RenderControl(hw)
            divmostrar.RenderControl(hw)
            Response.Write(tw.ToString())
            Dim sr As New StringReader(tw.ToString())
            Dim myMemoryStream As New MemoryStream()

            Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)
            Response.[End]()
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    'Private Sub ImageButton2_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton2.Click
    '    Try
    '        Dim strNombreInforme As String

    '        strNombreInforme = "Fondo de Anticipos Nacional e Inter desde " & txtFechaInicio.Value & " hasta " & txtFechaFin.Value

    '        Response.ContentType = "application/pdf"
    '        Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
    '        Response.Cache.SetCacheability(HttpCacheability.NoCache)
    '        Dim stringWriter As StringWriter = New StringWriter()
    '        Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
    '        divinforme.RenderControl(htmlTextWriter)
    '        Dim stringReader As StringReader = New StringReader(stringWriter.ToString())
    '        Dim Doc As Document = New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
    '        Dim htmlparser As HTMLWorker = New HTMLWorker(Doc)
    '        PdfWriter.GetInstance(Doc, Response.OutputStream)
    '        Doc.Open()
    '        htmlparser.Parse(stringReader)
    '        Doc.Close()
    '        Response.Write(Doc)
    '        Response.[End]()
    '    Catch ex As Exception
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
    '    End Try
    'End Sub
End Class
