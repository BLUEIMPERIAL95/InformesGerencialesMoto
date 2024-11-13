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
Partial Class InfParamTestSalud
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csreu As New reunion

    Private Sub InfParamTestSalud_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(35, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "redirect",
            "alert('" & strRespuestaPer & "'); window.location='" +
            Request.ApplicationPath + "/Default.aspx';", True)
            'Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            Dim dttes, dtmov As New DataTable
            Dim strMovi, strSQL, strHtml As String

            dttes = csreu.seleccionar_test_salud_conductores(txtFechaInicio.Value, txtFechaFin.Value)

            If dttes.Rows.Count > 0 Then
                strHtml = "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                strHtml += "<tr>"
                strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                strHtml += "<td align='center'><b><font size='1px'>Movimiento</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Generador</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Ruta</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Conductor</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Documento</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Esta Enfermo?</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Sintoma</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Dolor Garganta</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Malestar General</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Cercanos Covid</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Fiebre Mayor 38</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Tos Seca</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Dificultad Respirar</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Perdidad Olfato o Gusto</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Vives Alguien Covid</font></b></td>"
                strHtml += "<td align='left'><b><font size='1px'>Contacto Covid</font></b></td>"
                strHtml += "</tr>"
                For i As Integer = 0 To dttes.Rows.Count - 1
                    strMovi = dttes.Rows(i)("movimiento_tesa").ToString

                    strSQL = "Select movimientos_transportes_consolidado.generador_nombre,"
                    strSQL += " CONCAT(movimientos_transportes_consolidado.movimiento_origen, ' - ', movimientos_transportes_consolidado.movimiento_destino) AS ruta,"
                    strSQL += " movimientos_transportes_consolidado.conductor_nombre,"
                    strSQL += " movimientos_transportes_consolidado.conductor_documento"
                    strSQL += " From movimientos_transportes_consolidado"
                    strSQL += " Where movimientos_transportes_consolidado.movimiento_numero = '" & strMovi & "' LIMIT 1"

                    dtmov = csinformes.ejecutar_query_bd(strSQL)

                    If dtmov.Rows.Count > 0 Then
                        strHtml += "<tr>"

                        strHtml += "<td align='center'><font size='1px'>" & dttes.Rows(i)("fecha_tesa").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & strMovi & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtmov.Rows(0)("generador_nombre").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtmov.Rows(0)("ruta").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtmov.Rows(0)("conductor_nombre").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtmov.Rows(0)("conductor_documento").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("primera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("cualprimera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("segunda_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("tercera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("cuarta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("quinta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("sexta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("septima_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("octava_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("novena_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("decima_tesa").ToString & "</font></td>"

                        strHtml += "</tr>"
                    Else
                        strHtml += "<tr>"

                        strHtml += "<td align='center'><font size='1px'>" & dttes.Rows(i)("fecha_tesa").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & strMovi & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>NA</font></td>"
                        strHtml += "<td align='left'><font size='1px'>NA</font></td>"
                        strHtml += "<td align='left'><font size='1px'>NA</font></td>"
                        strHtml += "<td align='left'><font size='1px'>NA</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("primera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("cualprimera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("segunda_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("tercera_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("cuarta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("quinta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("sexta_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("septima_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("octava_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("novena_tesa").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dttes.Rows(i)("decima_tesa").ToString & "</font></td>"

                        strHtml += "</tr>"
                    End If
                Next

                strHtml += "</table>"

                divmostrar.InnerHtml = strHtml
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para el rango seleccionado...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=Test Salud Desde " & txtFechaInicio.Value & " Hasta " & txtFechaFin.Value & ".xls")
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
