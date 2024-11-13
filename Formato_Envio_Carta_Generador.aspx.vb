Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Amazon
Imports Amazon.SimpleEmail
Imports Amazon.SimpleEmail.Model
Imports System.Globalization
Partial Class Formato_Envio_Carta_Generador
    Inherits System.Web.UI.Page
    Dim csusua As New usuarios
    Dim csinformes As New Informes
    Dim csreu As New reunion
    Dim vecRespuesta As String()

    Private Sub Formato_Envio_Carta_Generador_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        If Me.IsPostBack = False Then
            'lbl_zona.InnerText = Request.QueryString("zon")
            vecRespuesta = Request.QueryString("doc").Split("|")
            If ConfigurationManager.AppSettings("bdsel").ToString = 3 Then
                lbl_Empresa.InnerText = "Refrilogistica SAS"
                lbl_ciudad.InnerText = "Caldas"
                lbl_cuerpo1.InnerText = "La empresa " & vecRespuesta(1) & " Certifica que la empresa Refrilogistica SAS NIT 830.513.756-2, nos prestó uno o varios de los siguientes servicios (Alquiler contenedores, almacenamiento, transporte terrestre, etc) desde " & vecRespuesta(4) & " hasta " & vecRespuesta(5) & ". Por un monto de " & vecRespuesta(2) & "."
            Else
                lbl_Empresa.InnerText = "Mototransportamos SAS"
                lbl_ciudad.InnerText = "Itaguí"
                lbl_cuerpo1.InnerText = "La empresa " & vecRespuesta(1) & " Certifica que la empresa Mototransportamos SAS NIT 800.046.457-2, nos prestó servicios de transporte de carga terrestre desde " & vecRespuesta(4) & " hasta " & vecRespuesta(5) & ". Por un monto de " & String.Format("{0:c}", vecRespuesta(2)) & "."
            End If

            'lbl_fecha_inicio.InnerText = Request.QueryString("fei")
            'lbl_fecha_fin.InnerText = Request.QueryString("fef")
            'lbl_ingreso.InnerText = Request.QueryString("val")
            lbl_fecha_actual.InnerText = Date.Now.ToString("yyyyMMdd")

            export_pdf()
        End If
    End Sub

    Sub export_pdf()
        Try
            ''Dim urlPdf = Server.MapPath("..") & "\QRManifiesto\" & Request.QueryString("pen") & ".pdf"
            'Dim imagenc1 As Image
            'Dim strCorreo As String
            'Dim dtter As New DataTable
            'Response.ContentType = "application/pdf"
            'Response.AddHeader("content-disposition", "attachment;filename=CartaGenerador.pdf")
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            'Dim sw As New StringWriter()
            'Dim hw As New HtmlTextWriter(sw)
            'Me.Page.RenderControl(hw)
            'Dim sr As New StringReader(sw.ToString())
            'Dim myMemoryStream As New MemoryStream()
            'Dim pdfDoc As New Document(PageSize.A3, 5.0F, 5.0F, 5.0F, 0.0F)
            'Dim htmlparser As New HTMLWorker(pdfDoc)
            'PdfWriter.GetInstance(pdfDoc, myMemoryStream)

            'pdfDoc.Open()

            'htmlparser.Parse(sr)

            ''imagenc1 = iTextSharp.text.Image.GetInstance(urlFotoCabeza1)
            ''imagenc1.BorderWidth = 1
            ''imagenc1.Alignment = Element.ALIGN_LEFT

            ''imagenc1.ScaleAbsoluteWidth(135)
            ''imagenc1.ScaleAbsoluteHeight(80)

            ''imagenc1.SetAbsolutePosition(35, 670)
            ''pdfDoc.Add(imagenc1)

            'pdfDoc.Close()

            'Dim archivoBytes As Byte() = myMemoryStream.ToArray()
            'Dim archivoBase64 As String = Convert.ToBase64String(archivoBytes)

            ''strCorreo = "desarrollo@mototransportar.com.co"
            'strCorreo = vecRespuesta(3)
            ''strCorreo = Request.QueryString("correoprop")
            'If strCorreo <> "" Then
            '    Dim asunto = "Carta Calificación Mototransportar"
            '    Dim cuerpo = "<html><head></head><body><p>Este correo es automático, no responder. Si tiene alguna inquietud, por favor comunicarse con el administrador(Mototransportar).</p></body></html>"
            '    Dim correo = strCorreo
            '    Dim vec As String() = ID.Split("|")

            '    'Dim cslog As Logs = New Logs

            '    'Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
            '    '              vbLf & "From: inforefrilogistica@cargacontrol.com.co" &
            '    '              vbLf & "Subject: " & asunto &
            '    '              vbLf & "MIME-Version: 1.0" &
            '    '              vbLf & "Content-Type: multipart/mixed;" &
            '    '              vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '    '              vbLf & "Content-Transfer-Encoding: 7bit" &
            '    '              vbLf & "Content-Disposition: inline" &
            '    '              vbLf & "" &
            '    '              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '    '              vbLf & "Content-Type: text/plain; charset=utf-8" &
            '    '              vbLf & "" &
            '    '              vbLf & "" &
            '    '              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '    '              vbLf & "Content-Type: text/html; charset=us-ascii" &
            '    '              vbLf & "" &
            '    '              vbLf & cuerpo &
            '    '              vbLf & "" &
            '    '              vbLf & "" &
            '    '              vbLf & "" &
            '    Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
            '                                      vbLf & "From: no-reply-refrilogistica@cargacontrol.com.co" &
            '                                      vbLf & "Subject: " & asunto &
            '                                      vbLf & "MIME-Version: 1.0" &
            '                                      vbLf & "Content-Type: multipart/mixed;" &
            '                                      vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '                                      vbLf & "Content-Transfer-Encoding: 7bit" &
            '                                      vbLf & "Content-Disposition: inline" &
            '                                      vbLf & "" &
            '                                      vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '                                      vbLf & "Content-Type: text/plain; charset=utf-8" &
            '                                      vbLf & "" &
            '                                      vbLf & "" &
            '                                      vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '                                      vbLf & "Content-Type: text/html; charset=us-ascii" &
            '                                      vbLf & "" &
            '                                      vbLf & cuerpo &
            '                                      vbLf & "" &
            '                                      vbLf & "" &
            '                                      vbLf & "" &
            '                                      vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
            '                                      vbLf & "Content-Transfer-Encoding: base64" &
            '                                      vbLf & "Content-Type: application/pdf;" &
            '                                      vbLf & "Content-Disposition: attachment; filename=Carta.pdf" &
            '                                      vbLf & "" &
            '                                      vbLf & archivoBase64 &
            '                                      vbLf & "" &
            '                                      vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

            '    Dim senderAddress = "no-reply-refrilogistica@cargacontrol.com.co"
            '    Dim receiverAddress = correo

            '    Dim lista = New List(Of String) From {
            '        receiverAddress
            '       }

            '    Dim awsCredentials = New Amazon.Runtime.BasicAWSCredentials("AKIAWZ7W7LMSJFWGR55A", "t6+mp8EM3cmq7BISP+jF8kmBnLcjSir+3qEbg5KG")

            '    Using client = New AmazonSimpleEmailServiceClient(awsCredentials, RegionEndpoint.USEast1)
            '        Dim sendRequest = New SendRawEmailRequest With {
            '                             .Source = senderAddress,
            '                             .Destinations = lista,
            '                            .RawMessage = New RawMessage With {
            '                             .Data = message
            '                             }
            '                            }

            '        Try
            '            Dim response = client.SendRawEmail(sendRequest)
            '            Dim respuesta_api = response.MessageId
            '            'csreu.guardar_log_correos(correo, "EXITOSO", "ENVIO PENDIENTES", 0, idterc)
            '        Catch ex As Exception
            '            Dim strMensaje = ex.Message
            '            'csreu.guardar_log_correos(correo, "NO EXITOSO", "ENVIO PENDIENTES", 0, idterc)
            '        End Try
            '    End Using
            'End If

            'Response.Write(pdfDoc)
            'Response.[End]()

            Try
                Dim strNombreInforme As String

                strNombreInforme = "Carta Generador "

                Response.ContentType = "application/pdf"
                Response.AddHeader("content-disposition", "attachment;filename=" & strNombreInforme & ".pdf")
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Dim stringWriter As StringWriter = New StringWriter()
                Dim htmlTextWriter As HtmlTextWriter = New HtmlTextWriter(stringWriter)
                Me.Page.RenderControl(htmlTextWriter)
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

        Catch ex As Exception
            Dim cabeza = "Validación Imagenes", texto = "Error en envío de correo. Consultar administrador de sistema", idmensaje = "warning"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "javascript:mensaje('" & idmensaje & "','" & cabeza & "','" & texto & "','3','','');", True)
        End Try
    End Sub
End Class
