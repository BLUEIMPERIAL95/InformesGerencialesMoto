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
Imports Microsoft.Office.Interop.Excel
Imports System.Data.OleDb
Partial Class importar_informacion_certificados
    Inherits System.Web.UI.Page
    Dim csinformes As New Informes
    Dim csusua As New usuarios
    Dim csoper As New Operaciones
    Dim csterc As New equipos

    Private Sub importar_informacion_certificados_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2049, Session("id_usua"))

        If strRespuestaPer <> "" Then
            Response.Redirect("Default.aspx")
        End If

        If Me.IsPostBack = False Then
            combos()
        End If
    End Sub

    Private Sub combos()
        Dim dtempr, dttipo As New System.Data.DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_empresas_combo", dtempr, cboEmpresas)
        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_tipo_certificados_combo", dttipo, cboTipo)
    End Sub

    Private Sub btnGenerar_Click(sender As Object, e As EventArgs) Handles btnGenerar.Click
        Try
            If FileExcel.HasFile Then
                If Path.GetExtension(FileExcel.FileName) = ".csv" Then
                    If cboEmpresas.SelectedValue = "0" Or cboPeriodo.SelectedValue = "0" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
                    Else
                        Dim strRes As String
                        'Save the uploaded Excel file.
                        Dim filePath As String = Server.MapPath("Carga_Excel/") + Path.GetFileName(FileExcel.PostedFile.FileName)

                        If File.Exists(filePath) Then
                            System.IO.File.Delete(filePath)
                        End If

                        FileExcel.PostedFile.SaveAs(filePath)

                        ' Create an instance of StreamReader to read from a file.
                        Dim sr As StreamReader = New StreamReader(filePath)
                        Dim line As String

                        Dim vec As String()
                        line = sr.ReadLine()
                        Dim strDocumento, strNombre, strCodigo As String
                        Dim id_tice, intLendoc, Cont As Integer
                        Dim intValor, intValorBase As Decimal

                        Cont = 1
                        Do
                            vec = line.Split(";")

                            If vec.Length < 4 Then
                                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
                            Else
                                strRes = ""
                                strCodigo = Replace(vec(0), " ", "")
                                strDocumento = Replace(Replace(vec(1), ".", ""), " ", "")
                                intLendoc = Len(strDocumento)
                                'Try
                                '    strDocumento = strDocumento.Substring(0, intLendoc - 2)
                                'Catch ex As Exception
                                '    strDocumento = ""
                                'End Try
                                strNombre = vec(2).ToString
                                If vec(3) < 0 Then
                                    intValor = vec(3) * -1
                                Else
                                    intValor = vec(3)
                                End If

                                If vec(4) < 0 Then
                                    intValorBase = vec(4) * -1
                                Else
                                    intValorBase = vec(4)
                                End If

                                If strCodigo.Substring(0, 4) = "2368" Then
                                    id_tice = 1
                                Else
                                    id_tice = 2
                                End If

                                'If strDocumento = "3173782" Then
                                '    Dim strEntro As String
                                '    strEntro = ""
                                'End If

                                strRes = csterc.guardar_datos_certificados(id_tice, cboEmpresas.SelectedValue, cboPeriodo.SelectedValue, cboBimestre.SelectedValue,
                                                                           strDocumento, strNombre, strCodigo, intValor, intValorBase, Cont)
                            End If

                            Cont = Cont + 1

                            line = sr.ReadLine()
                        Loop Until line Is Nothing
                        sr.Close()
                        System.IO.File.Delete(filePath)

                        Mostrar_Datos_Certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)
                        Mostrar_Log_Datos_Certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)

                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con éxito...');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El archivo a leer debe tener extensión .csv.');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub Mostrar_Datos_Certificados(ByVal itc As Integer, ByVal ieo As Integer, ByVal per As Integer)
        Try
            Dim dtcer As New System.Data.DataTable

            If cboEmpresas.SelectedValue = "0" Or cboTipo.SelectedValue = "0" Or cboPeriodo.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
            Else
                dtcer = csterc.seleccionar_datos_certificados(cboTipo.SelectedValue, cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)

                If dtcer.Rows.Count > 0 Then
                    Dim strHtml As String

                    strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='4'>DATOS CERTIFICADOS POR TIPO CERTIFICADO, EMPRESA Y PERIODO</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"
                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='center' colspan='2'><b><font size='1px'>T.Certificado</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Empresa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Periodo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Documento</font></b></td>"
                    strHtml += "<td align='center' colspan='4'><b><font size='1px'>Nombre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Codigo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Porcentaje</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Base</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContReg As Integer
                    ContReg = 0
                    For i As Integer = 0 To dtcer.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("fecha_dace").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='1px'>" & dtcer.Rows(i)("nombre_tice").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("nombre_emor").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("periodo_dace").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcer.Rows(i)("documento_dace").ToString & "</font></td>"
                        strHtml += "<td align='center' colspan='4'><font size='1px'>" & dtcer.Rows(i)("nombre_dace").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("codigo_ince").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & dtcer.Rows(i)("porcentaje_dace").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtcer.Rows(i)("valor_dace"), 0) & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtcer.Rows(i)("base_dace"), 0) & "</font></td>"
                        strHtml += "</tr>"

                        ContReg = ContReg + 1
                    Next

                    lblRegistros.Text = "Reg: " & ContReg

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                Else
                    divmostrar.InnerHtml = ""
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para los filtros seleccionados...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub Mostrar_Datos_Certificados_salvar(ByVal ieo As Integer, ByVal per As Integer)
        Try
            Dim dtcer As New System.Data.DataTable

            If cboEmpresas.SelectedValue = "0" Or cboPeriodo.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
            Else
                dtcer = csterc.seleccionar_datos_certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)

                If dtcer.Rows.Count > 0 Then
                    Dim strHtml As String

                    strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='4'>DATOS CERTIFICADOS POR TIPO CERTIFICADO, EMPRESA Y PERIODO</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"
                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='center' colspan='2'><b><font size='1px'>T.Certificado</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Empresa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Periodo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Documento</font></b></td>"
                    strHtml += "<td align='center' colspan='4'><b><font size='1px'>Nombre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Codigo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Porcentaje</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Base</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContReg As Integer
                    ContReg = 0
                    For i As Integer = 0 To dtcer.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("fecha_dace").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='1px'>" & dtcer.Rows(i)("nombre_tice").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("nombre_emor").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("periodo_dace").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcer.Rows(i)("documento_dace").ToString & "</font></td>"
                        strHtml += "<td align='center' colspan='4'><font size='1px'>" & dtcer.Rows(i)("nombre_dace").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("codigo_ince").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & dtcer.Rows(i)("porcentaje_dace").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtcer.Rows(i)("valor_dace"), 0) & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtcer.Rows(i)("base_dace"), 0) & "</font></td>"
                        strHtml += "</tr>"

                        ContReg = ContReg + 1
                    Next

                    lblRegistros.Text = "Reg: " & ContReg

                    strHtml += "</table>"

                    divmostrar.InnerHtml = strHtml
                Else
                    divmostrar.InnerHtml = ""
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para los filtros seleccionados...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub Mostrar_Log_Datos_Certificados_salvar(ByVal ieo As Integer, ByVal per As Integer)
        Try
            Dim dtcer As New System.Data.DataTable

            If cboEmpresas.SelectedValue = "0" Or cboPeriodo.SelectedValue = "0" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por seleccionar...');", True)
            Else
                dtcer = csterc.seleccionar_log_datos_certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)

                If dtcer.Rows.Count > 0 Then
                    Dim strHtml As String

                    strHtml = "<table cellpadding='2' cellspacing='0' border='0' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='4'>DATOS LOG REGISTROS NO SUBIDOS</font></b></td>"
                    strHtml += "</tr>"
                    strHtml += "</table>"
                    strHtml += "<br />"

                    strHtml += "<table cellpadding='2' cellspacing='0' border='1' class='StyleTable1' align='center' width='100%'>"
                    strHtml += "<tr>"
                    strHtml += "<td align='center'><b><font size='1px'>Fecha</font></b></td>"
                    strHtml += "<td align='center' colspan='2'><b><font size='1px'>T.Certificado</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Empresa</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Periodo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Documento</font></b></td>"
                    strHtml += "<td align='center' colspan='4'><b><font size='1px'>Nombre</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Codigo</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Porcentaje</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Valor</font></b></td>"
                    strHtml += "<td align='center'><b><font size='1px'>Reg</font></b></td>"
                    strHtml += "</tr>"

                    Dim ContReg As Integer
                    ContReg = 0
                    For i As Integer = 0 To dtcer.Rows.Count - 1
                        strHtml += "<tr>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("fecha_dace").ToString & "</font></td>"
                        strHtml += "<td align='left' colspan='2'><font size='1px'>" & dtcer.Rows(i)("nombre_tice").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("nombre_emor").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("periodo_dace").ToString & "</font></td>"
                        strHtml += "<td align='left'><font size='1px'>" & dtcer.Rows(i)("documento_dace").ToString & "</font></td>"
                        strHtml += "<td align='center' colspan='4'><font size='1px'>" & dtcer.Rows(i)("nombre_dace").ToString & "</font></td>"
                        strHtml += "<td align='center'><font size='1px'>" & dtcer.Rows(i)("codigo_ince").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & dtcer.Rows(i)("porcentaje_dace").ToString & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & String.Format("{0:c}", dtcer.Rows(i)("valor_dace"), 0) & "</font></td>"
                        strHtml += "<td align='right'><font size='1px'>" & dtcer.Rows(i)("consexcel_dace").ToString & "</font></td>"
                        strHtml += "</tr>"

                        ContReg = ContReg + 1
                    Next

                    lblLogRegistros.Text = "Reg: " & ContReg

                    strHtml += "</table>"

                    divmostrarlog.InnerHtml = strHtml
                Else
                    divmostrarlog.InnerHtml = ""
                    'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No existe información para los filtros seleccionados...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Error " & ex.Message & "');", True)
        End Try
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        'Mostrar_Datos_Certificados(cboTipo.SelectedValue, cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)
        Mostrar_Datos_Certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)
        Mostrar_Log_Datos_Certificados_salvar(cboEmpresas.SelectedValue, cboPeriodo.SelectedValue)
    End Sub

    Private Sub ImageButton1_Click(sender As Object, e As ImageClickEventArgs) Handles ImageButton1.Click
        Try
            Response.ContentType = "application/x-msexcel"
            Response.AddHeader("Content-Disposition", "attachment; filename=Ingreso.xls")
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
