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
Imports System.Data.OleDb
Partial Class ValoresDescuentosSystram
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csdesc As New descuentoscolanta

    Private Sub ValoresDescuentosSystram_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Dim strRespuesta, strRespuestaPer As String

            strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

            If strRespuesta <> "" Then
                Response.Redirect("login.aspx")
            End If

            strRespuestaPer = csusua.validar_permiso_usuario(3067, Session("id_usua"))

            If strRespuestaPer <> "" Then
                Response.Redirect("Default.aspx")
            End If

            If Me.IsPostBack = False Then
                combos()

                Llenar_Grid_Detalle()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Sub combos()
        Dim dtdesc As New DataTable

        csoper.LlenarDropDownList_Sql("nombre", "id", "sp_seleccionar_descuentos_systram_combo", dtdesc, cboDescuento)
    End Sub

    Private Sub Llenar_Grid_Detalle()
        Try
            Dim dtdes As New DataTable

            dtdes = csdesc.seleccionar_valores_descuentos_systram_listado(0, "")

            If dtdes.Rows.Count > 0 Then
                gridDescuentosSystram.DataSource = dtdes
                gridDescuentosSystram.DataBind()
            Else
                gridDescuentosSystram.DataSource = dtdes
                gridDescuentosSystram.DataBind()
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If txtPlaca.Text = "" Or txtDocumento.Text = "" Or cboDescuento.SelectedValue = "0" Or cboMes.SelectedValue = "0" Or cboAño.SelectedValue = "0" Or txtValor.Text = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Falta información por digitar...');", True)
            Else
                Dim strRes As String

                strRes = csdesc.guardar_valores_descuentos_systram(hidDescuento.Value, txtPlaca.Text, txtDocumento.Text, cboDescuento.SelectedValue,
                                                                   cboMes.SelectedValue, cboAño.SelectedValue, txtValor.Text, Session("id_usua"))

                If strRes = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Actualizado Exitosamente...');", True)
                    Limpiar()
                    Llenar_Grid_Detalle()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Actualizado Exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub Limpiar()
        hidDescuento.Value = "0"
        txtPlaca.Text = ""
        txtDocumento.Text = ""
        cboDescuento.SelectedValue = "0"
        cboMes.SelectedValue = "0"
        cboAño.SelectedValue = "0"
        txtValor.Text = ""

        txtPlaca.Enabled = True
        txtDocumento.Enabled = True
        cboDescuento.Enabled = True
        cboMes.Enabled = True
        cboAño.Enabled = True
    End Sub

    Private Sub gridDescuentosSystram_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gridDescuentosSystram.RowCommand
        Try
            Dim iddes As Integer
            Dim strRespuesta As String
            strRespuesta = ""
            If e.CommandName = "modificar" Then
                iddes = gridDescuentosSystram.DataKeys(e.CommandArgument).Values(0)
                hidDescuento.Value = iddes

                Dim dtdes As New DataTable

                dtdes = csdesc.seleccionar_valores_descuentos_systram_listado(1, hidDescuento.Value)

                If dtdes.Rows.Count > 0 Then
                    hidDescuento.Value = dtdes.Rows(0)("id_vads").ToString
                    txtPlaca.Text = dtdes.Rows(0)("placa_vads").ToString
                    txtDocumento.Text = dtdes.Rows(0)("documento_vads").ToString
                    cboDescuento.SelectedValue = dtdes.Rows(0)("id_desy").ToString
                    cboMes.SelectedValue = dtdes.Rows(0)("mes_vads").ToString
                    cboAño.SelectedValue = dtdes.Rows(0)("año_vads").ToString
                    txtValor.Text = dtdes.Rows(0)("valor_desy").ToString

                    txtPlaca.Enabled = False
                    txtDocumento.Enabled = False
                    cboDescuento.Enabled = False
                    cboMes.Enabled = False
                    cboAño.Enabled = False
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Inválido...');", True)
                End If
            End If

            If e.CommandName = "eliminar" Then
                iddes = gridDescuentosSystram.DataKeys(e.CommandArgument).Values(0)

                strRespuesta = csdesc.eliminar_valores_descuentos_systram_por_id(iddes)

                If strRespuesta = "" Then
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro Eliminado Exitosamente...');", True)
                    Limpiar()
                    Llenar_Grid_Detalle()
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Registro No Eliminado Exitosamente...');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Limpiar()
    End Sub

    Private Sub btnImportar_Click(sender As Object, e As EventArgs) Handles btnImportar.Click
        Try
            'If FileExcel.HasFile Then
            '    If Path.GetExtension(FileExcel.FileName) = ".csv" Then
            '        Dim strRes As String
            '        'Save the uploaded Excel file.
            '        Dim filePath As String = Server.MapPath("Carga_Excel/") + Path.GetFileName(FileExcel.PostedFile.FileName)

            '        If File.Exists(filePath) Then
            '            System.IO.File.Delete(filePath)
            '        End If

            '        FileExcel.PostedFile.SaveAs(filePath)

            '        ' Create an instance of StreamReader to read from a file.
            '        Dim sr As StreamReader = New StreamReader(filePath)
            '        Dim line As String

            '        Dim vec As String()
            '        line = sr.ReadLine()
            '        Dim strPlaca, strDocumento As String
            '        Dim idDes, idMes, idAño, Cont As Integer
            '        Dim intValor As Decimal

            '        Cont = 1
            '        Do
            '            vec = line.Split(";")

            '            If vec.Length < 6 Then
            '                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
            '            Else
            '                strRes = ""
            '                strPlaca = Replace(vec(0), " ", "")
            '                strDocumento = Replace(Replace(vec(1), ".", ""), " ", "")
            '                idDes = vec(2)
            '                idMes = vec(3)
            '                idAño = vec(4)
            '                intValor = (5)

            '                strRes = csdesc.guardar_valores_descuentos_systram(0, strPlaca, strDocumento, idDes, idMes, idAño, intValor, Session("id_usua"))
            '            End If

            '            Cont = Cont + 1

            '            line = sr.ReadLine()
            '        Loop Until line Is Nothing
            '        sr.Close()
            '        System.IO.File.Delete(filePath)

            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con éxito...');", True)
            '    Else
            '        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('El archivo a leer debe tener extensión .csv.');", True)
            '    End If
            'Else
            '    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Archivo Inválido...');", True)
            'End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "javascript:fnDatos();", True)
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

    Private Sub btnRecargar_Click(sender As Object, e As EventArgs) Handles btnRecargar.Click
        Response.Redirect("ValoresDescuentosSystram.aspx")
    End Sub
End Class
