Imports System.Data
Imports System.Web.Services
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports Amazon
Imports Amazon.SimpleEmail
Imports Amazon.SimpleEmail.Model
Partial Class FormatoPendienteDeReunionCorreo
    Inherits System.Web.UI.Page
    Dim csreu As New reunion
    Dim cster As New equipos
    Dim csusua As New usuarios

    Private Sub FormatoPendienteDeReunionCorreo_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(1, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect",
            "alert('" & strRespuestaPer & "'); window.location='" +
            Request.ApplicationPath + "/Default.aspx';", True)
            'Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            Dim dtreu, dtpen, dtres As New DataTable

            dtreu = csreu.capturar_datos_reuniones_por_id(Request.QueryString("idreu"))

            If dtreu.Rows.Count > 0 Then
                lbl_codreunion.InnerText = dtreu.Rows(0).Item("codigo_reun").ToString
                lbl_nomreunion.InnerText = dtreu.Rows(0).Item("nombre_reun").ToString
                lbl_fecreunion.InnerText = dtreu.Rows(0).Item("fecha_reun").ToString
            End If

            dtpen = csreu.capturar_datos_pendiente_reunion_por_id_pendiente(Request.QueryString("idpen"))

            If dtpen.Rows.Count > 0 Then
                lbl_codpendiente.InnerText = dtpen.Rows(0).Item("codigo_peru").ToString
                lbl_nompendiente.InnerText = dtpen.Rows(0).Item("nombre_peru").ToString
                lbl_fecpendiente.InnerText = dtreu.Rows(0).Item("fecha_reun").ToString
                lbl_estpendiente.InnerText = dtpen.Rows(0).Item("estado_peru").ToString
                lbl_pripendiente.InnerText = dtpen.Rows(0).Item("prioridad_peru").ToString
                lbl_plapendiente.InnerText = dtpen.Rows(0).Item("plazo_peru").ToString
                lbl_despendiente.InnerText = dtpen.Rows(0).Item("descripcion_peru").ToString
            End If

            dtres = csreu.capturar_datos_pendiente_reunion_terceros_por_id_pendiente(Request.QueryString("idpen"))

            Dim strHtml As String
            strHtml = ""
            Dim tr As HtmlTableRow
            Dim td1 As HtmlTableCell
            Dim td2 As HtmlTableCell
            Dim td3 As HtmlTableCell
            Dim cel1 = ""
            Dim cel2 = ""
            Dim cel3 = ""

            If dtres.Rows.Count > 0 Then
                For idet As Integer = 0 To dtres.Rows.Count - 1
                    cel1 = "<font size='10px'>&nbsp;&nbsp;" & dtres.Rows(idet).Item("documento_TERC") & "</font>"
                    cel2 = "<font size='10px'>&nbsp;&nbsp;" & dtres.Rows(idet).Item("nombre_TERC") & "</font>"
                    cel3 = "<font size='10px'>&nbsp;&nbsp;" & dtres.Rows(idet).Item("fecha_asig") & "</font>"

                    tr = New HtmlTableRow
                    td1 = New HtmlTableCell
                    td2 = New HtmlTableCell
                    td3 = New HtmlTableCell

                    td1.Attributes("size") = "1"
                    td2.Attributes("size") = "1"
                    td3.Attributes("size") = "1"

                    td1.Attributes("align") = "left"
                    td2.Attributes("align") = "left"
                    td3.Attributes("align") = "center"

                    td1.InnerHtml = cel1
                    td2.InnerHtml = cel2
                    td3.InnerHtml = cel3

                    tr.Cells.Add(td1)
                    tr.Cells.Add(td2)
                    tr.Cells.Add(td3)

                    tbl_responsables.Rows.Add(tr)
                Next
            End If

            export_pdf()
        End If
    End Sub

    Sub export_pdf()
        Try
            'Dim urlPdf = Server.MapPath("..") & "\QRManifiesto\" & Request.QueryString("pen") & ".pdf"
            Dim imagenc1 As Image
            Dim strCorreo As String
            Dim dtter As New DataTable
            Response.ContentType = "application/pdf"
            Response.AddHeader("content-disposition", "attachment;filename=Pendiente Reunion " & Request.QueryString("codpen") & ".pdf")
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

            If Request.QueryString("idter") <> Nothing Then
                dtter = cster.capturar_datos_terceros_por_id(Request.QueryString("idter"))
                If dtter.Rows.Count > 0 Then
                    strCorreo = dtter.Rows(0).Item("correo").ToString
                Else
                    strCorreo = "comunicaciones@refrilogistica.com"
                End If
            Else
                strCorreo = "comunicaciones@refrilogistica.com"
            End If

            If strCorreo = "" Then
                strCorreo = "comunicaciones@refrilogistica.com"
            End If
            'strCorreo = "desarrollo@mototransportar.com.co"
            'strCorreo = "dirsistemas@mototransportar.com.co"
            'strCorreo = Request.QueryString("correoprop")
            If strCorreo <> "" Then
                Dim asunto = "Pendiente " & lbl_nomreunion.InnerText & "(" & lbl_codpendiente.InnerText & ")"
                Dim cuerpo = "<html><head></head><body><p>Este correo es automático, no responder. Si tiene alguna inquietud, por favor comunicarse con el administrador del aplicativo de reuniones(Juan David Montoya).</p></body></html>"
                Dim correo = strCorreo
                Dim vec As String() = ID.Split("|")

                'Dim cslog As Logs = New Logs

                Dim message = New MemoryStream(Encoding.UTF8.GetBytes("To: " & correo &
                              vbLf & "From: comunicaciones@refrilogistica.com" &
                              vbLf & "Subject: " & asunto &
                              vbLf & "MIME-Version: 1.0" &
                              vbLf & "Content-Type: multipart/mixed;" &
                              vbLf & "        boundary=a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: 7bit" &
                              vbLf & "Content-Disposition: inline" &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Type: text/plain; charset=utf-8" &
                              vbLf & "" &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Type: text/html; charset=us-ascii" &
                              vbLf & "" &
                              vbLf & cuerpo &
                              vbLf & "" &
                              vbLf & "" &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a" &
                              vbLf & "Content-Transfer-Encoding: base64" &
                              vbLf & "Content-Type: application/pdf;" &
                              vbLf & "Content-Disposition: attachment; filename=PendienteReunion" & Request.QueryString("codpen") & ".pdf" &
                              vbLf & "" &
                              vbLf & archivoBase64 &
                              vbLf & "" &
                              vbLf & "--a3f166a86b56ff6c37755292d690675717ea3cd9de81228ec2b76ed4a15d6d1a--"))

                Dim senderAddress = "comunicaciones@refrilogistica.com"
                Dim receiverAddress = correo

                Dim lista = New List(Of String) From {
                    receiverAddress
                   }

                Using client = New AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1)
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
                        csreu.guardar_log_correos(correo, "EXITOSO", "ENVIO PENDIENTE REUNION", lbl_codpendiente.InnerText, Request.QueryString("idpen"))
                    Catch ex As Exception
                        Dim strMensaje = ex.Message
                        csreu.guardar_log_correos(correo, strMensaje, "ENVIO PENDIENTE REUNION", lbl_codpendiente.InnerText, Request.QueryString("idpen"))
                    End Try
                End Using
            End If

            Response.Write(pdfDoc)
            Response.[End]()

        Catch ex As Exception
            Dim cabeza = "Validación Imagenes", texto = "Error en envío de correo. Consultar administrador de sistema", idmensaje = "warning"
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "alert", "javascript:mensaje('" & idmensaje & "','" & cabeza & "','" & texto & "','3','','');", True)
        End Try
    End Sub
End Class
