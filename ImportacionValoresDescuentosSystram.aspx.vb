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
Partial Class ImportacionValoresDescuentosSystram
    Inherits System.Web.UI.Page
    Dim csdesc As New descuentoscolanta

    Private Sub btnImportar_Click(sender As Object, e As EventArgs) Handles btnImportar.Click
        Try
            If FileExcel.HasFile Then
                If Path.GetExtension(FileExcel.FileName) = ".csv" Then
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
                    Dim strPlaca, strDocumento As String
                    Dim idDes, idMes, idAño, Cont As Integer
                    Dim intValor As Decimal

                    Cont = 1
                    Do
                        vec = line.Split(";")

                        If vec.Length < 6 Then
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
                        Else
                            strRes = ""
                            strPlaca = Replace(vec(0), " ", "")
                            strDocumento = Replace(Replace(vec(1), ".", ""), " ", "")
                            idDes = vec(2)
                            idMes = vec(3)
                            idAño = vec(4)
                            intValor = vec(5)

                            strRes = csdesc.guardar_valores_descuentos_systram(0, strPlaca, strDocumento, idDes, idMes, idAño, intValor, Session("id_usua"))
                        End If

                        Cont = Cont + 1

                        line = sr.ReadLine()
                    Loop Until line Is Nothing
                    sr.Close()
                    System.IO.File.Delete(filePath)

                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con éxito...');", True)
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El archivo a leer debe tener extensión .csv.');", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim Form As HtmlForm = Page.Form
        If Form.Enctype.Length = 0 Then
            Form.Enctype = "multipart/form-data"
        End If
    End Sub
End Class
