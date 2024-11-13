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
Partial Class Generacion_Cartas
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csterc As New equipos

    Private Sub Generacion_Cartas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If
    End Sub

    Private Sub btnCartaLaboral_Click(sender As Object, e As EventArgs) Handles btnCartaLaboral.Click
        Try
            tbl_construccion.Visible = False

            Dim dtemp As New DataTable

            dtemp = csusua.seleccionar_salario_empresa_por_documento(Session("documento"))

            If dtemp.Rows.Count > 0 Then
                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If dtemp.Rows(0)("empresa_saem").ToString = "TRAMITAR EN LINEA SAS" Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                Else
                    If dtemp.Rows(0)("empresa_saem").ToString = "MOTOSEGURIDAD LTDA" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo2.jpg")
                    Else
                        If dtemp.Rows(0)("empresa_saem").ToString.Contains("MOTOTRANSPORTAMOS") Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo3.jpg")
                        Else
                            If dtemp.Rows(0)("empresa_saem").ToString = "REFRILOGISTICA SAS" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo4.jpg")
                            Else
                                If dtemp.Rows(0)("empresa_saem").ToString.Contains("CASTOMA") Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo5.jpg")
                                Else
                                    If dtemp.Rows(0)("empresa_saem").ToString = "CIA CAPRI" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                    Else
                                        If dtemp.Rows(0)("empresa_saem").ToString = "MOTOTRANSPORTAR SA" Then
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                                        Else
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                                        End If
                                    End If
                                End If
                            End If
                        End If
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

                strHtml = "<br /><br /><br /><br /><br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='right' width='100%'><img src='" & urlFotoCabeza1 & "' height='120' width='360'></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><font size='5'>Itaguí, " & Date.Now.ToString("dddd d \de MMMM \de yyyy") & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='100%'><b><font size='5'>" & dtemp.Rows(0)("nombre_emor").ToString & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='100%'><b><font size='5'>" & dtemp.Rows(0)("nit_emor").ToString & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='100%'><b><font size='5'>CERTIFICA</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='100%'><font size='5'>El o La señor/a " & dtemp.Rows(0)("nombre_saem").ToString & " identificado con cédula de ciudadania No. " & dtemp.Rows(0)("documento_saem").ToString & ", labora para la compañía desde " & CDate(dtemp.Rows(0)("fechaing_saem")).ToString("dddd d \de MMMM \de yyyy") & ", desempeñando el cargo de " & dtemp.Rows(0)("cargo_saem").ToString & ", tiene un contrato por prestación de servicios y un sueldo mensual de " & String.Format("{0:c}", 10195640) & ".</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='100%'><font size='5'>Si quiere información adicional gustosamente le será suministrada en el teléfono 444 54 99 Ext. 149</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>Cordialmente</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>SILVANA M. USUGA H.</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>Directora de Talento Humano</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                divinforme.InnerHtml = strHtml

                export_pdf()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información del empleado logueado...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub export_pdf()
        Try
            Dim strNombreInforme As String

            strNombreInforme = "Carta Laboral " & Session("documento")

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

    Private Sub btnCartaRetiro_Click(sender As Object, e As EventArgs) Handles btnCartaRetiro.Click
        Try
            tbl_construccion.Visible = False

            Dim dtemp As New DataTable

            dtemp = csusua.seleccionar_retiro_empresa_por_documento(Session("documento"))

            If dtemp.Rows.Count > 0 Then
                Dim pathimgCabeza1 As String
                Dim urlFotoCabeza1 As String = ""
                If dtemp.Rows(0)("empresa_reem").ToString = "TRAMITAR EN LINEA SAS" Then
                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo1.jpg")
                Else
                    If dtemp.Rows(0)("empresa_reem").ToString = "MOTOSEGURIDAD LTDA" Then
                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo2.jpg")
                    Else
                        If dtemp.Rows(0)("empresa_reem").ToString.Contains("MOTOTRANSPORTAMOS") Then
                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo3.jpg")
                        Else
                            If dtemp.Rows(0)("empresa_reem").ToString = "REFRILOGISTICA SAS" Then
                                pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo4.jpg")
                            Else
                                If dtemp.Rows(0)("empresa_reem").ToString.Contains("CASTOMA") Then
                                    pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logo5.jpg")
                                Else
                                    If dtemp.Rows(0)("empresa_reem").ToString = "CIA CAPRI" Then
                                        pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logocia.jpeg")
                                    Else
                                        If dtemp.Rows(0)("empresa_reem").ToString = "MOTOTRANSPORTAR SA" Then
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                                        Else
                                            pathimgCabeza1 = Path.Combine(Server.MapPath("images"), "logotar.jpg")
                                        End If
                                    End If
                                End If
                            End If
                        End If
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

                strHtml = "<br /><br /><br /><br /><br />"

                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='right' width='100%'><img src='" & urlFotoCabeza1 & "' height='120' width='360'></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><font size='5'>Itaguí, " & Date.Now.ToString("dddd d \de MMMM \de yyyy") & "</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='100%'><b><font size='5'>" & dtemp.Rows(0)("empresa_reem").ToString & "</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                'strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                'strHtml += "<tr>"
                'strHtml += "<td align='center' width='100%'><b><font size='5'>" & dtemp.Rows(0)("nit_emor").ToString & "</font></b></td>"
                'strHtml += "</tr>"
                'strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center' width='100%'><b><font size='5'>CERTIFICA</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='100%'><font size='5'>El o La señor/a " & dtemp.Rows(0)("nombre_reem").ToString & " identificado con cédula de ciudadania No. " & dtemp.Rows(0)("documento_reem").ToString & ", laboró para la compañía desde " & CDate(dtemp.Rows(0)("fechavin_reem")).ToString("dddd d \de MMMM \de yyyy") & ", hasta " & CDate(dtemp.Rows(0)("fechaliq_reem")).ToString("dddd d \de MMMM \de yyyy") & ", desempeñando el cargo de " & dtemp.Rows(0)("cargo_reem").ToString & ".</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='justify' width='100%'><font size='5'>Si quiere información adicional gustosamente le será suministrada en el teléfono 444 54 99 Ext. 149</font></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>Cordialmente</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<br />"
                strHtml += "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='80%'>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>SILVANA M. USUGA H.</font></b></td>"
                strHtml += "</tr>"
                strHtml += "<tr>"
                strHtml += "<td align='left' width='100%'><b><font size='5'>Directora de Talento Humano</font></b></td>"
                strHtml += "</tr>"
                strHtml += "</table>"

                strHtml += "<br />"
                strHtml += "<br />"

                divinforme.InnerHtml = strHtml

                export_pdf()
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información del empleado logueado...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub
End Class
