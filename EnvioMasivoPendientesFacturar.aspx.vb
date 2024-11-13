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
Imports DPFP
Imports DPFP.Capture
Imports System.Drawing
Imports System.Data.SqlClient
Partial Class EnvioMasivoPendientesFacturar
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csfact As New facturas
    Shared da As SqlDataAdapter
    Shared dt As DataTable
    Shared con As SqlConnection = New SqlConnection("Server=192.168.9.250; Database=Mototransportar; User Id=sa; Password=Adm789**;")
    Dim idTercero As String
    Private Sub EnvioMasivoPendientesFacturar_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strRespuesta, strRespuestaPer As String

        strRespuesta = csusua.validar_session_usuarios(Session("id_usua"), Session.SessionID)

        If strRespuesta <> "" Then
            Response.Redirect("login.aspx")
        End If

        strRespuestaPer = csusua.validar_permiso_usuario(2052, Session("id_usua"))

        If strRespuestaPer <> "" Then
            'ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('No posee permisos para ingresar...');", True)
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Page.GetType(), "msg", "No posee permisos para ingresar...", True)
            Response.Redirect("Default.aspx")
        End If
    End Sub

    <System.Web.Script.Services.ScriptMethod()>
    <System.Web.Services.WebMethod>
    Public Shared Function GetSearch(ByVal prefixText As String) As List(Of String)
        Dim Result As DataTable = New DataTable()
        Dim str As String = "select tbl_terceros.id_TERC, LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') FROM tbl_terceros WHERE LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') + ' - ' + ISNULL(LTRIM(RTRIM(tbl_terceros.documento_TERC)), '') LIKE '%" & prefixText & "%' ORDER BY LTRIM(RTRIM(tbl_terceros.nombre_TERC)) + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.nombre2_TERC)), '') + CASE WHEN LTRIM(RTRIM(ISNULL(tbl_terceros.nombre2_TERC, ''))) = '' THEN '' ELSE ' ' END + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido1_TERC)), '') + ' ' + ISNULL(LTRIM(RTRIM(tbl_terceros.apellido2_TERC)), '') "
        da = New SqlDataAdapter(str, con)
        dt = New DataTable()
        da.Fill(dt)
        Dim Output As List(Of String) = New List(Of String)()

        For i As Integer = 0 To dt.Rows.Count - 1
            Output.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dt.Rows(i)(1).ToString(), dt.Rows(i)(0).ToString()))
        Next

        Return Output
    End Function

    Private Sub btnPrueba_Click(sender As Object, e As EventArgs) Handles btnPrueba.Click
        Try
            Dim strRes As String

            strRes = csfact.enviar_correo_pendientes_facturar_automaticos

            If strRes = "" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada con éxito...');", True)
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Transacción realizada sin éxito...');", True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
