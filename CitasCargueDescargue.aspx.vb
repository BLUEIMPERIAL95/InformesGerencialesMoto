Imports System.Data
Partial Class CitasCargueDescargue
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim cscita As New citas

    Private Sub CitasCargueDescargue_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(2057, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If

            If Me.IsPostBack = False Then
                combos()
                CargarConveciones()

                txtFechaActual.Text = mFechaAAAAMMDD(DateAndTime.Now)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtsede, dtmuel As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_sedes_combo", dtsede, cboSede)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_muelles_combo", dtmuel, cboMuelle)
    End Sub

    Public Sub CargarConveciones()
        Dim strConvenciones As String
        strConvenciones = ""
        strConvenciones = strConvenciones & "<table align=center border=0 class=StyleTable1><tr>"
        strConvenciones = strConvenciones & "<td bgcolor=#58D3F7>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=left>Programada</td>"
        strConvenciones = strConvenciones & "<td bgcolor=#FF8000>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=left>Cumplida</td>"
        strConvenciones = strConvenciones & "<td bgcolor=#F3F781>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=left>Cancelada</td>"
        'strConvenciones = strConvenciones & "<td bgcolor=#BDBDBD>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=left>Bloqueada</td>"
        strConvenciones = strConvenciones & "<td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td align=left>Disponible</td>"
        strConvenciones = strConvenciones & "</tr><table>"
        tblConvencion.InnerHtml = strConvenciones
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Try
            If cboSede.SelectedValue = "0" Or cboMuelle.SelectedValue = "0" Or cboTipo.SelectedValue = "0" Or (cboClase1.SelectedValue = "0" And cboClase2.SelectedValue = "0") Or txtFecha.Value = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Filtros inválidos...');", True)
            Else
                Dim dtcit As New DataTable

                dtcit = cscita.seleccionar_citas_datos_calendario(txtFecha.Value, cboMuelle.SelectedValue)

                If dtcit.Rows.Count > 0 Then
                    Dim Encabezado As TableRow = New TableRow()

                    TablaFormas.Rows.Clear()
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = ""
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(50)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver

                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(0)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver

                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(1)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver
                    'objDatosCita = Datos(1)
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(2)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver
                    'objDatosCita = Datos(2)
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(3)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver
                    'objDatosCita = Datos(3)
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(4)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver
                    'objDatosCita = Datos(4)
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(5)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver
                    'objDatosCita = Datos(5)
                    Encabezado.Cells.Add(New TableCell())
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Text = dtcit.Rows(6)("strDia").ToString
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Width = Unit.Pixel(140)
                    Encabezado.Cells(Encabezado.Cells.Count - 1).Font.Bold = True
                    Encabezado.Cells(Encabezado.Cells.Count - 1).BackColor = Drawing.Color.Silver

                    TablaFormas.Rows.Add(Encabezado)

                    Dim strFnCelda As String
                    For i As Integer = 0 To dtcit.Rows.Count - 1

                        Dim Fila As TableRow = New TableRow()
                        Fila.Cells.Add(New TableCell())
                        Fila.Cells(Fila.Cells.Count - 1).Text = dtcit.Rows(i)("horaAMPM").ToString
                        Fila.Cells(Fila.Cells.Count - 1).Width = Unit.Pixel(50)

                        For J = 0 To 6
                            Fila.Cells.Add(New TableCell())

                            If dtcit.Rows(i + J)("NombreDocente").ToString IsNot "" Then
                                strFnCelda = "<a id='linkcita' href=" & Chr(34) & "javascript:fnDatos('" + dtcit.Rows(i + J)("horaAMPM").ToString & "', '" + cboSede.SelectedValue & "', '" + cboMuelle.SelectedValue & "', '" + cboTipo.SelectedValue & "', '" + cboClase1.SelectedValue & "', '" + cboClase2.SelectedValue & "', '" + dtcit.Rows(i + J)("dtmFecha").ToString & "', '" + dtcit.Rows(i + J)("id_RESE").ToString & "')" + Chr(34) & " style='text-decoration:none' title='" + dtcit.Rows(i + J)("NombreDocente").ToString & "'>" + dtcit.Rows(i + J)("NombreDocente").ToString & "(" & dtcit.Rows(i + J)("placa_cicd").ToString & ")</a>"
                                If dtcit.Rows(i + J)("strEstado_RESE").ToString = "PR" Then
                                    Fila.Cells(Fila.Cells.Count - 1).BackColor = Drawing.ColorTranslator.FromHtml("#58D3F7")
                                Else
                                    If dtcit.Rows(i + J)("strEstado_RESE").ToString = "CU" Then
                                        Fila.Cells(Fila.Cells.Count - 1).BackColor = Drawing.ColorTranslator.FromHtml("#FF8000")
                                    Else
                                        If dtcit.Rows(i + J)("strEstado_RESE").ToString = "CA" Then
                                            Fila.Cells(Fila.Cells.Count - 1).BackColor = Drawing.ColorTranslator.FromHtml("#F3F781")
                                        Else
                                            If dtcit.Rows(i + J)("strEstado_RESE").ToString = "BQ" Then
                                                Fila.Cells(Fila.Cells.Count - 1).BackColor = Drawing.ColorTranslator.FromHtml("#BDBDBD")
                                            End If
                                        End If
                                    End If
                                End If
                            Else
                                strFnCelda = "<a id='linkcita' href=" & Chr(34) & "javascript:fnDatos('" + dtcit.Rows(i + J)("horaAMPM").ToString & "', '" + cboSede.SelectedValue & "', '" + cboMuelle.SelectedValue & "', '" + cboTipo.SelectedValue & "', '" + cboClase1.SelectedValue & "', '" + cboClase2.SelectedValue & "', '" + dtcit.Rows(i + J)("dtmFecha").ToString & "', 0)" + Chr(34) & " style='text-decoration:none'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a>"
                            End If
                            Fila.Cells(Fila.Cells.Count - 1).Text = strFnCelda
                            Fila.Cells(Fila.Cells.Count - 1).Width = Unit.Pixel(50)
                        Next

                        TablaFormas.Rows.Add(Fila)
                        i = i + 6
                    Next
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Public Shared Function mFechaAAAAMMDD(ByVal Fecha As System.DateTime) As String
        'Convierte la fecha al fromato AAAA/MM/DD
        Dim strFecha As String = Nothing
        strFecha = ""
        If Information.IsDate(Fecha) Then
            strFecha = DateAndTime.Year(CDate((Fecha))).ToString() & "/" + Strings.Right("00" & DateAndTime.Month(CDate((Fecha))).ToString(), 2) & "/" + Strings.Right("00" & DateAndTime.Day(CDate((Fecha))).ToString(), 2)
        End If
        Return strFecha
    End Function

    Private Sub cboTipo_Load(sender As Object, e As EventArgs) Handles cboTipo.Load

    End Sub

    Private Sub cboTipo_TextChanged(sender As Object, e As EventArgs) Handles cboTipo.TextChanged
        If cboTipo.SelectedValue = "1" Then
            cboClase1.Visible = True
            cboClase2.Visible = False
            cboClase1.Width = "200"
            cboClase2.Width = "0"
            cboClase2.SelectedValue = "0"
        Else
            cboClase1.Visible = False
            cboClase2.Visible = True
            cboClase1.Width = "0"
            cboClase2.Width = "200"
            cboClase1.SelectedValue = "0"
        End If
    End Sub
End Class
