Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Amazon
Imports Amazon.SimpleEmail
Imports Amazon.SimpleEmail.Model
Partial Class FormatoPendientesReunion
    Inherits System.Web.UI.Page
    Dim csreu As New reunion

    Private Sub FormatoPendientesReunion_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Me.IsPostBack = False Then

            export_pdf()
        End If
    End Sub

    Sub export_pdf()
        Try
            'Dim urlPdf = Server.MapPath("..") & "\QRManifiesto\" & Request.QueryString("pen") & ".pdf"
            Dim imagenc1 As Image
            Dim strCorreo, strCorreoActual As String
            strCorreo = ""
            strCorreoActual = ""
            Dim dtter As New DataTable
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Pendiente Reunion.pdf")
            Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            Me.Page.RenderControl(hw)
            Dim sr As New StringReader(sw.ToString())
            Dim myMemoryStream As New MemoryStream()
            Dim pdfDoc As New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            Dim htmlparser As New HTMLWorker(pdfDoc)
            PdfWriter.GetInstance(pdfDoc, myMemoryStream)

            pdfDoc.Open()

            htmlparser.Parse(sr)

            'imagenc1 = iTextSharp.text.Image.GetInstance(urlFotoCabeza1)
            'imagenc1.BorderWidth = 1
            'imagenc1.Alignment = Element.ALIGN_LEFT

            'imagenc1.ScaleAbsoluteWidth(135)
            'imagenc1.ScaleAbsoluteHeight(80)

            'imagenc1.SetAbsolutePosition(35, 670)
            'pdfDoc.Add(imagenc1)

            pdfDoc.Close()

            Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)

            'If Request.QueryString("idter") <> Nothing Then
            '    dtter = cster.capturar_datos_terceros_por_id(Request.QueryString("idter"))
            '    If dtter.Rows.Count > 0 Then
            '        strCorreo = dtter.Rows(0).Item("correo").ToString
            '    Else
            '        strCorreo = "comunicaciones@refrilogistica.com"
            '    End If
            'Else
            'strCorreo = "comunicaciones@refrilogistica.com"
            'End If

            'If strCorreo = "" Then
            '    strCorreo = "desarrollo@mototransportar.com.co"
            'End If
            'strCorreo = "desarrollo@mototransportar.com.co"
            'strCorreo = "dirsistemas@mototransportar.com.co"
            'strCorreo = Request.QueryString("correoprop")

            Dim dtReu, dtact, dtactact, dtcla As New DataTable
            Dim strClave1, strClave2 As String

            strClave1 = ""
            strClave2 = ""

            dtcla = csreu.seleccionar_claves_empresariales("AMAZON 1")

            If dtcla.Rows.Count > 0 Then
                strClave1 = dtcla.Rows(0)("clave1_clav").ToString
                strClave2 = dtcla.Rows(0)("clave2_clav").ToString
            End If

            dtReu = csreu.seleccionar_enviar_correos_automaticos_reuniones_correo_amazon()

            If dtReu.Rows.Count > 0 Then
                Dim strHtml, asunto, correo As String
                Dim idterc, idteract As Integer
                strHtml = ""
                asunto = ""
                correo = ""

                asunto = "Pendientes Reuniones Organización Mototransportar SAS"
                strHtml = "<html><head></head><body>"

                idteract = dtReu.Rows(0)("id_TERC")
                strCorreoActual = dtReu.Rows(0)("correo_TERC").ToString
                'idteract = 18

                For i As Integer = 0 To dtReu.Rows.Count - 1
                    strCorreo = dtReu.Rows(i)("correo_TERC").ToString
                    idterc = dtReu.Rows(i)("id_TERC")
                    'strCorreo = "desarrollo@mototransportar.com.co"
                    'idterc = 18
                    correo = strCorreo

                    'dtactact = csreu.seleccionar_correos_activos_pendientes_reuniones(idteract, strCorreoActual)
                    'dtact = csreu.seleccionar_correos_activos_pendientes_reuniones(idterc, strCorreo)

                    'If dtact.Rows.Count > 0 Then
                    'If dtactact.Rows(0)("activo_cope") = 1 Then

                    If idteract = idterc Then
                            strHtml = strHtml & "<table id='Table3' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor ='#F4664F'>"
                            strHtml = strHtml & "<td align='center'colspan='3'><b><font size='2px'>INFORMACIÓN REUNIÓN</font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>CODIGO: </font></b><font size='2px'>" & dtReu.Rows(i)("codigo_reun").ToString & "</font></font></td>"
                            strHtml = strHtml & "<td align='left'><b><font size='2px'>NOMBRE: </font></b><font size='2px'>" & dtReu.Rows(i)("nombre_reun").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>FECHA: </font></b><font size='2px'>" & dtReu.Rows(i)("fecha_reun").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"
                            strHtml = strHtml & "<table id ='Table1' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor='#BDBDBD'>"
                            strHtml = strHtml & "<td align=' center' colspan=' 3'><b><font size=' 2px'>INFORMACIÓN PENDIENTE</font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='center' colspan='3'><b><font size='2px'>SOLICITA: </font></b><font size='2px'>" & dtReu.Rows(i)("nombre_SOLI").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>CODIGO: </font></b><font size='2px'>" & dtReu.Rows(i)("codigo_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align=' left'><b><font size=' 2px'>NOMBRE: </font></b><font size=' 2px'>" & dtReu.Rows(i)("nombre_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>FECHA: </font></b><font size='2px'>" & dtReu.Rows(i)("fecha_peru").ToString & "</font>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>ESTADO: </font></b><font size='2px'>" & dtReu.Rows(i)("estado_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align=' left'><b><font size=' 2px'>PRIORIDAD: </font></b><font size=' 2px'>" & dtReu.Rows(i)("prioridad_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>PLAZO: </font></b><font size='2px'>" & dtReu.Rows(i)("plazo_peru").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left' colspan='3'><b><font size='2px'>DESCRIPCIÓN: </font></b><font size='2px'>" & dtReu.Rows(i)("descripcion_peru").ToString & "</font></td>"
                            strHtml = strHtml & " </tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"
                            strHtml = strHtml & "<table id ='tbl_responsables' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor='#BDBDBD'>"
                            strHtml = strHtml & "<td align=' center' colspan=' 2'><b><font size=' 2px'>RESPONDER EN: </font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='center'><a href='http://192.168.9.250:81/InformesGerencialesMoto/RespuestaReunionPendienteTercero.aspx?idpen=" & dtReu.Rows(i)("id_peru").ToString & "&idter=" & dtReu.Rows(i)("id_terc").ToString & "'>Enviar Respuesta Dentro de Empresa</a></td>"
                            strHtml = strHtml & "<td align=' center'><a href='http://190.242.96.212:81/InformesGerencialesMoto/RespuestaReunionPendienteTercero.aspx?idpen=" & dtReu.Rows(i)("id_peru").ToString & "&idter=" & dtReu.Rows(i)("id_terc").ToString & "'>Enviar Respuesta Afuera de Empresa</a></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"
                        Else
                            strHtml = strHtml & "</body></html>"

                        correo = strCorreoActual

                        Dim message1 = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
                                                vbLf & "From: no-reply-refrilogistica@cargacontrol.com.co" &
                                                vbLf & "Subject: " & asunto &
                                                vbLf & "MIME-Version: 1.0" &
                                                vbLf & "Content-Type: multipart/mixed;" &
                                                vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                                                vbLf & "Content-Transfer-Encoding: 7bit" &
                                                vbLf & "Content-Disposition: inline" &
                                                vbLf & "" &
                                                vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                                                vbLf & "Content-Type: text/html; charset=us-ascii" &
                                                vbLf & "" &
                                                vbLf & strHtml &
                                                vbLf & "" &
                                                vbLf & "" &
                                                vbLf & "" &
                                                vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))
                        'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                        'vbLf & "Content-Transfer-Encoding: base64" &
                        'vbLf & "Content-Type: application/pdf;" &
                        'vbLf & "Content-Disposition: attachment; filename=PendienteReunion.pdf" &
                        'vbLf & "" &
                        'vbLf & archivoBase64 &
                        'vbLf & "" &
                        'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                        Dim senderAddress1 = "no-reply-refrilogistica@cargacontrol.com.co"
                        Dim receiverAddress1 = correo

                        Dim lista1 = New List(Of String) From {
                                receiverAddress1
                                }
                        Dim awsCredentials1 = New Amazon.Runtime.BasicAWSCredentials(strClave1, strClave2)

                        Using client = New AmazonSimpleEmailServiceClient(awsCredentials1, RegionEndpoint.USEast1)
                            Dim sendRequest = New SendRawEmailRequest With {
                                         .Source = senderAddress1,
                                         .Destinations = lista1,
                                        .RawMessage = New RawMessage With {
                                         .Data = message1
                                         }
                                        }

                            Try
                                Dim response = client.SendRawEmail(sendRequest)
                                Dim respuesta_api = response.MessageId
                                csreu.guardar_log_correos(correo, "EXITOSO", "ENVIO PENDIENTES", 0, idterc)
                            Catch ex As Exception
                                Dim strMensaje = ex.Message
                                csreu.guardar_log_correos(correo, "NO EXITOSO", "ENVIO PENDIENTES", 0, idterc)
                            End Try
                        End Using

                        asunto = "Pendientes Reuniones Organización Mototransportar SAS"
                            strHtml = "<html><head></head><body>"

                            strHtml = strHtml & "<table id='Table3' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor ='#F4664F'>"
                            strHtml = strHtml & "<td align='center'colspan='3'><b><font size='2px'>INFORMACIÓN REUNIÓN</font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>CODIGO: </font></b><font size='2px'>" & dtReu.Rows(i)("codigo_reun").ToString & "</font></font></td>"
                            strHtml = strHtml & "<td align='left'><b><font size='2px'>NOMBRE: </font></b><font size='2px'>" & dtReu.Rows(i)("nombre_reun").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>FECHA: </font></b><font size='2px'>" & dtReu.Rows(i)("fecha_reun").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"
                            strHtml = strHtml & "<table id ='Table1' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor='#BDBDBD'>"
                            strHtml = strHtml & "<td align=' center' colspan=' 3'><b><font size=' 2px'>INFORMACIÓN PENDIENTE</font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='center' colspan='3'><b><font size='2px'>SOLICITA: </font></b><font size='2px'>" & dtReu.Rows(i)("nombre_SOLI").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>CODIGO: </font></b><font size='2px'>" & dtReu.Rows(i)("codigo_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align=' left'><b><font size=' 2px'>NOMBRE: </font></b><font size=' 2px'>" & dtReu.Rows(i)("nombre_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>FECHA: </font></b><font size='2px'>" & dtReu.Rows(i)("fecha_peru").ToString & "</font>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>ESTADO: </font></b><font size='2px'>" & dtReu.Rows(i)("estado_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align=' left'><b><font size=' 2px'>PRIORIDAD: </font></b><font size=' 2px'>" & dtReu.Rows(i)("prioridad_peru").ToString & "</font></td>"
                            strHtml = strHtml & "<td align ='left'><b><font size='2px'>PLAZO: </font></b><font size='2px'>" & dtReu.Rows(i)("plazo_peru").ToString & "</font></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='left' colspan='3'><b><font size='2px'>DESCRIPCIÓN: </font></b><font size='2px'>" & dtReu.Rows(i)("descripcion_peru").ToString & "</font></td>"
                            strHtml = strHtml & " </tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"
                            strHtml = strHtml & "<table id ='tbl_responsables' runat='server' width='100%' border='1'>"
                            strHtml = strHtml & "<tr bgcolor='#BDBDBD'>"
                            strHtml = strHtml & "<td align=' center' colspan=' 2'><b><font size=' 2px'>RESPONDER EN: </font></b></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "<tr>"
                            strHtml = strHtml & "<td align ='center'><a href='http://192.168.9.250:81/InformesGerencialesMoto/RespuestaReunionPendienteTercero.aspx?idpen=" & dtReu.Rows(i)("id_peru").ToString & "&idter=" & dtReu.Rows(i)("id_terc").ToString & "'>Enviar Respuesta Dentro de Empresa</a></td>"
                            strHtml = strHtml & "<td align=' center'><a href='http://190.242.96.212:81/InformesGerencialesMoto/RespuestaReunionPendienteTercero.aspx?idpen=" & dtReu.Rows(i)("id_peru").ToString & "&idter=" & dtReu.Rows(i)("id_terc").ToString & "'>Enviar Respuesta Afuera de Empresa</a></td>"
                            strHtml = strHtml & "</tr>"
                            strHtml = strHtml & "</table>"
                            strHtml = strHtml & "<br />"

                            idteract = dtReu.Rows(i)("id_TERC")
                            strCorreoActual = dtReu.Rows(i)("correo_TERC").ToString
                            'idteract = 18
                        End If
                        'Else
                    '    csreu.guardar_log_correos(correo, "Correo no esta activo.", "CORREO INACTIVO", 0, idterc)
                    'End If
                    'Else
                    '    Dim dtreco As New DataTable
                    '    Dim idreco As Integer

                    '    dtreco = csreu.guardar_correos_pendientes_reuniones(idterc, strCorreo)

                    '    If dtreco.Rows.Count > 0 Then
                    '        idreco = dtreco.Rows(0)("id_cope")

                    '        asunto = "Activación Correo Mototransportar SAS"
                    '        strHtml = "<html><head></head><body>"
                    '        strHtml = strHtml & "<table id ='tbl_responsables' runat='server' width='100%' border='1'>"
                    '        strHtml = strHtml & "<tr bgcolor='#BDBDBD'>"
                    '        strHtml = strHtml & "<td align=' center' colspan=' 2'><b><font size=' 2px'>Active su correo dando click al siguiente link</font></b></td>"
                    '        strHtml = strHtml & "</tr>"
                    '        strHtml = strHtml & "<tr>"
                    '        strHtml = strHtml & "<td align ='center'><a href='http://192.168.9.250:81/InformesGerencialesMoto/Activacion_Correo.aspx?idcope=" & idreco & "'>Enviar Respuesta Dentro de Empresa</a></td>"
                    '        strHtml = strHtml & "<td align=' center'><a href='http://190.242.96.212:81/InformesGerencialesMoto/Activacion_Correo.aspx?idcope=" & idreco & "'>Enviar Respuesta Afuera de Empresa</a></td>"
                    '        'strHtml = strHtml & "<td align ='center'><a href='http://localhost:1316/Activacion_Correo.aspx?idcope=" & idreco & "'>Enviar Respuesta Dentro de Empresa</a></td>"
                    '        'strHtml = strHtml & "<td align=' center'><a href='http://localhost:1316/Activacion_Correo.aspx?idcope=" & idreco & "'>Enviar Respuesta Afuera de Empresa</a></td>"
                    '        strHtml = strHtml & "</tr>"
                    '        strHtml = strHtml & "</table>"
                    '        strHtml = strHtml & "</body></html>"

                    '        Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
                    '                              vbLf & "From: no-reply-refrilogistica@cargacontrol.com.co" &
                    '                              vbLf & "Subject: " & asunto &
                    '                             vbLf & "MIME-Version: 1.0" &
                    '                              vbLf & "Content-Type: multipart/mixed;" &
                    '                              vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                    '                              vbLf & "Content-Transfer-Encoding: 7bit" &
                    '                              vbLf & "Content-Disposition: inline" &
                    '                              vbLf & "" &
                    '                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                    '                              vbLf & "Content-Type: text/html; charset=us-ascii" &
                    '                              vbLf & "" &
                    '                              vbLf & strHtml &
                    '                              vbLf & "" &
                    '                              vbLf & "" &
                    '                              vbLf & "" &
                    '                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))
                    '        'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                    '        'vbLf & "Content-Transfer-Encoding: base64" &
                    '        'vbLf & "Content-Type: application/pdf;" &
                    '        'vbLf & "Content-Disposition: attachment; filename=PendienteReunion.pdf" &
                    '        'vbLf & "" &
                    '        'vbLf & archivoBase64 &
                    '        'vbLf & "" &
                    '        'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                    '        Dim senderAddress = "no-reply-refrilogistica@cargacontrol.com.co"
                    '        Dim receiverAddress = correo

                    '        Dim lista = New List(Of String) From {
                    '                    receiverAddress
                    '                   }

                    '        Dim awsCredentials = New Amazon.Runtime.BasicAWSCredentials("AKIAWZ7W7LMSJFWGR55A", "t6+mp8EM3cmq7BISP+jF8kmBnLcjSir+3qEbg5KG")

                    '        Using client = New AmazonSimpleEmailServiceClient(awsCredentials, RegionEndpoint.USEast1)
                    '            Dim sendRequest = New SendRawEmailRequest With {
                    '                         .Source = senderAddress,
                    '                         .Destinations = lista,
                    '                        .RawMessage = New RawMessage With {
                    '                         .Data = message
                    '                         }
                    '                        }

                    '            Try
                    '                Dim response = client.SendRawEmail(sendRequest)
                    '                Dim respuesta_api = response.MessageId
                    '                csreu.envio_correo_pendientes_reuniones(idreco)
                    '                csreu.guardar_log_correos(correo, "EXITOSO", "ACTIVACIÓN CORREO", 0, idterc)
                    '            Catch ex As Exception
                    '                Dim strMensaje = ex.Message
                    '                csreu.guardar_log_correos(correo, "No se envió activación de correo.", "ACTIVACIÓN CORREO", 0, idterc)
                    '            End Try
                    '        End Using

                    '        divPendientes.InnerHtml = strHtml
                    '    End If

                    '    csreu.guardar_log_correos(correo, "No existe en la tabla activación correos.", "CORREO INACTIVO", 0, idterc)
                    'End If
                Next

                strHtml = strHtml & "</body></html>"

                'If dtact.Rows(0)("activo_cope") = 1 Then
                Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
                                                  vbLf & "From: no-reply-refrilogistica@cargacontrol.com.co" &
                                                  vbLf & "Subject: " & asunto &
                                                  vbLf & "MIME-Version: 1.0" &
                                                  vbLf & "Content-Type: multipart/mixed;" &
                                                  vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                                                  vbLf & "Content-Transfer-Encoding: 7bit" &
                                                  vbLf & "Content-Disposition: inline" &
                                                  vbLf & "" &
                                                  vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                                                  vbLf & "Content-Type: text/html; charset=us-ascii" &
                                                  vbLf & "" &
                                                  vbLf & strHtml &
                                                  vbLf & "" &
                                                  vbLf & "" &
                                                  vbLf & "" &
                                                  vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))
                    'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                    'vbLf & "Content-Transfer-Encoding: base64" &
                    'vbLf & "Content-Type: application/pdf;" &
                    'vbLf & "Content-Disposition: attachment; filename=PendienteReunion.pdf" &
                    'vbLf & "" &
                    'vbLf & archivoBase64 &
                    'vbLf & "" &
                    'vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                    Dim senderAddress = "no-reply-refrilogistica@cargacontrol.com.co"
                    Dim receiverAddress = correo

                    Dim lista = New List(Of String) From {
                                        receiverAddress
                                       }

                Dim awsCredentials = New Amazon.Runtime.BasicAWSCredentials(strClave1, strClave2)

                Using client = New AmazonSimpleEmailServiceClient(awsCredentials, RegionEndpoint.USEast1)
                        Dim sendRequest = New SendRawEmailRequest With {
                                             .Source = senderAddress,
                                             .Destinations = lista,
                                            .RawMessage = New RawMessage With {
                                             .Data = message
                                             }
                                            }

                        Try
                            Dim response = client.SendRawEmail(sendRequest)
                            Dim respuesta_api = response.MessageId
                            csreu.guardar_log_correos(correo, "EXITOSO", "ENVIO PENDIENTES", 0, idterc)
                        Catch ex As Exception
                            Dim strMensaje = ex.Message
                            csreu.guardar_log_correos(correo, "NO EXITOSO", "ENVIO PENDIENTES", 0, idterc)
                        End Try
                    End Using

                    divPendientes.InnerHtml = strHtml
                    'End If
                End If

            Response.Write(pdfDoc)
            Response.[End]()

        Catch ex As Exception
            Dim cabeza = "Validación Imagenes", texto = "Error en envío de correo. Consultar administrador de sistema", idmensaje = "warning"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "javascript:mensaje('" & idmensaje & "','" & cabeza & "','" & texto & "','3','','');", True)
        End Try
    End Sub
End Class
