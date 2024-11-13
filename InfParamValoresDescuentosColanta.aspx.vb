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
Partial Class InfParamValoresDescuentosColanta
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csdesc As New descuentoscolanta

    Private Sub InfParamValoresDescuentosColanta_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(3068, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            If cboMes.SelectedValue = "0" Or cboAño.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros Inválidos...');", True)
            Else
                Dim dtDes, dtVal As New DataTable

                dtDes = csdesc.seleccionar_valores_descuentos_systram_informe(cboMes.SelectedValue, cboAño.SelectedValue)

                If dtDes.Rows.Count > 0 Then
                    divmostrar.InnerHtml = ""
                    divinforme.InnerHtml = ""

                    Dim strHtml, strSQL As String
                    strHtml = ""
                    strSQL = ""

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='left'><b><font size='1px'>Año</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Mes</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Placa</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Documento</font></b></td>"
                    strHtml += "<td align='left'><b><font size='1px'>Descuento</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor Systram</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor Faltante</font></b></td>"
                    strHtml += "</tr>"

                    Dim strValorProgramado, valorprogTotMaxi, valorprogTotMoto, valorprogTotPoli, valorprogTotSate As Decimal
                    Dim valordescTotMaxi, valordescTotMoto, valordescTotPoli, valordescTotSate As Decimal

                    strValorProgramado = 0
                    valorprogTotMaxi = 0
                    valorprogTotMoto = 0
                    valorprogTotPoli = 0
                    valorprogTotSate = 0
                    valordescTotMaxi = 0
                    valordescTotMoto = 0
                    valordescTotPoli = 0
                    valordescTotSate = 0

                    For i As Integer = 0 To dtDes.Rows.Count - 1

                        strSQL = "SELECT COALESCE(SUM(movimientos_transportes_descuentos.valor), 0) AS valor_descuento "
                        strSQL += "FROM movimientos_transportes_descuentos "
                        strSQL += "INNER JOIN movimientos_transportes_consolidado "
                        strSQL += "ON movimientos_transportes_descuentos.movimientos_transportes_idmovimientos_transportes = movimientos_transportes_consolidado.movimiento_id "
                        strSQL += "WHERE YEAR(movimientos_transportes_consolidado.movimiento_fecha) = " & cboAño.SelectedValue & " AND MONTH(movimientos_transportes_consolidado.movimiento_fecha) = " & cboMes.SelectedValue & " "
                        strSQL += "AND movimientos_transportes_descuentos.tipo_descuentos_transportes_idtipo_descuentos_transportes = " & dtDes.Rows(i)("idsystram_desy") & " "
                        'strSQL += "AND movimientos_transportes_consolidado.vehiculo_placa = '" & dtDes.Rows(i)("placa_vads").ToString & "' AND movimientos_transportes_consolidado.propietario_documento = '" & dtDes.Rows(i)("documento_vads").ToString & "' "
                        strSQL += "AND movimientos_transportes_consolidado.vehiculo_placa = '" & dtDes.Rows(i)("placa_vads").ToString & "' "
                        strSQL += "And movimientos_transportes_descuentos.idel = 0 "

                        dtVal = csinformes.ejecutar_query_bd(strSQL)

                        If dtVal.Rows.Count > 0 Then
                            strValorProgramado = dtVal.Rows(0)("valor_descuento")
                        Else
                            strValorProgramado = 0
                        End If

                        If (strValorProgramado - dtDes.Rows(i)("valor_desy")) < 0 Then
                            strHtml += "<tr bgcolor='#FCC8D3'>"
                        Else
                            strHtml += "<tr>"
                        End If

                        strHtml += "<td align='left'><font size='1px'>" & dtDes.Rows(i)("año_vads").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtDes.Rows(i)("mes_vads").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtDes.Rows(i)("placa_vads").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtDes.Rows(i)("documento_vads").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtDes.Rows(i)("nombre_desy").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtDes.Rows(i)("valor_desy")) & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", strValorProgramado) & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", strValorProgramado - dtDes.Rows(i)("valor_desy")) & "</font></td>"
                        strHtml += "</tr>"

                        If dtDes.Rows(i)("idsystram_desy") = 972 Or dtDes.Rows(i)("idsystram_desy") = 989 Then
                            valorprogTotMaxi = valorprogTotMaxi + strValorProgramado
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 973 Or dtDes.Rows(i)("idsystram_desy") = 990 Then
                            valorprogTotMoto = valorprogTotMoto + strValorProgramado
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 974 Or dtDes.Rows(i)("idsystram_desy") = 991 Then
                            valorprogTotPoli = valorprogTotPoli + strValorProgramado
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 976 Or dtDes.Rows(i)("idsystram_desy") = 994 Then
                            valorprogTotSate = valorprogTotSate + strValorProgramado
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 972 Or dtDes.Rows(i)("idsystram_desy") = 989 Then
                            valordescTotMaxi = valordescTotMaxi + dtDes.Rows(i)("valor_desy")
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 973 Or dtDes.Rows(i)("idsystram_desy") = 990 Then
                            valordescTotMoto = valordescTotMoto + dtDes.Rows(i)("valor_desy")
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 974 Or dtDes.Rows(i)("idsystram_desy") = 991 Then
                            valordescTotPoli = valordescTotPoli + dtDes.Rows(i)("valor_desy")
                        End If

                        If dtDes.Rows(i)("idsystram_desy") = 976 Or dtDes.Rows(i)("idsystram_desy") = 994 Then
                            valordescTotSate = valordescTotSate + dtDes.Rows(i)("valor_desy")
                        End If
                    Next

                    strHtml += "</table>"

                    strHtml += "<br />"
                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='60%'>"
                    strHtml += "<tr bgcolor='#BDBDBD'>"
                    strHtml += "<td align='left'><b><font size='1px'>Descuento</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valo Total</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor Systram Total</font></b></td>"
                    strHtml += "<td align='right'><b><font size='1px'>Valor Faltante Total</font></b></td>"
                    strHtml += "</tr>"

                    If (valorprogTotMaxi - valordescTotMaxi) < 0 Then
                        strHtml += "<tr bgcolor='#FCC8D3'>"
                    Else
                        strHtml += "<tr>"
                    End If

                    strHtml += "<td align='left'><font size='1px'>MAXITEMPO</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valordescTotMaxi) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotMaxi) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotMaxi - valordescTotMaxi) & "</font></td>"
                    strHtml += "</tr>"

                    If (valorprogTotMoto - valordescTotMoto) < 0 Then
                        strHtml += "<tr bgcolor='#FCC8D3'>"
                    Else
                        strHtml += "<tr>"
                    End If

                    strHtml += "<td align='left'><font size='1px'>MOTOTAMOS</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valordescTotMoto) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotMoto) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotMoto - valordescTotMoto) & "</font></td>"
                    strHtml += "</tr>"

                    If (valorprogTotPoli - valordescTotPoli) < 0 Then
                        strHtml += "<tr bgcolor='#FCC8D3'>"
                    Else
                        strHtml += "<tr>"
                    End If

                    strHtml += "<td align='left'><font size='1px'>POLIZA OP</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valordescTotPoli) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotPoli) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotPoli - valordescTotPoli) & "</font></td>"
                    strHtml += "</tr>"

                    If (valorprogTotSate - valordescTotSate) < 0 Then
                        strHtml += "<tr bgcolor='#FCC8D3'>"
                    Else
                        strHtml += "<tr>"
                    End If

                    strHtml += "<td align='left'><font size='1px'>SATELITAL</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valordescTotSate) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotSate) & "</font></td>"
                    strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", valorprogTotSate - valordescTotSate) & "</font></td>"
                    strHtml += "</tr>"

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
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
