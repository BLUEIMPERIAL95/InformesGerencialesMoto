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
Partial Class CargueImagenesNovedades
    Inherits System.Web.UI.Page
    Dim csoper As New Operaciones
    Dim csusua As New usuarios
    Dim csterc As New Novedades

    Private Sub CargueImagenesNovedades_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Me.IsPostBack = False Then
                hidImagen.Value = 1
                hidDocumento.Value = Request.QueryString("doc")

                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("ImagenesNovedades"), "Novedad_" & Request.QueryString("doc") & "_" & hidImagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/Novedad_" & Request.QueryString("doc") & "_" & hidImagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/nophoto.jpg"
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub btnSalvar_Click(sender As Object, e As EventArgs) Handles btnSalvar.Click
        Try
            If Request.QueryString("doc") = 0 Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Documento conductor inválido.');", True)
            Else
                If fluImagen.HasFile Then
                    Dim extensión As String = Right(fluImagen.PostedFile.ContentType.Trim, 4)

                    If extensión = "jpeg" Then
                        Dim dtdoc As New DataTable
                        Dim strRuta, strRes As String
                        Dim intConsecutivo As Integer

                        dtdoc = csterc.seleccionar_consecutivo_proximo_imagen_novedades(hidDocumento.Value)

                        If dtdoc.Rows.Count > 0 Then
                            intConsecutivo = dtdoc.Rows(0)("proximo")
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Consecutivo imagen inválido.');", True)
                            Exit Sub
                        End If

                        strRuta = "ImagenesNovedades/Novedad__" & hidDocumento.Value & "_" & intConsecutivo & ".jpeg"

                        Dim pathimg As String
                        pathimg = Path.Combine(Server.MapPath("ImagenesNovedades"), "Novedad__" & hidDocumento.Value & "_" & intConsecutivo & ".jpeg")

                        If File.Exists(pathimg) Then
                            File.Delete(pathimg)
                        End If

                        strRes = csterc.guardar_imagenes_novedades_consecutivo(hidDocumento.Value, intConsecutivo, 1)

                        If strRes = "" Then
                            fluImagen.SaveAs(Server.MapPath(strRuta))

                            hidDocumento.Value = 0

                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Imagen documento guardado con exito.');javascript:window.parent.location.reload();", True)
                        Else
                            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Imagen documento No guardado con exito.');", True)
                        End If
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Extensión inválida. Solo jpeg.');", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Debe seleccionar imagen.');", True)
                End If
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgAnterior_Click(sender As Object, e As ImageClickEventArgs) Handles imgAnterior.Click
        Try
            If hidImagen.Value > 1 And hidDocumento.Value > 0 Then
                hidImagen.Value = hidImagen.Value - 1
                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("ImagenesNovedades"), "Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/nophoto.jpg"
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire('Invalido...');", True)
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub imgSiguiente_Click(sender As Object, e As ImageClickEventArgs) Handles imgSiguiente.Click
        Try
            If hidImagen.Value >= 2 And hidDocumento.Value > 0 Then
                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("ImagenesNovedades"), "Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/nophoto.jpg"
                End If
                hidImagen.Value = hidImagen.Value + 1
            Else
                Dim pathimg As String
                pathimg = Path.Combine(Server.MapPath("ImagenesNovedades"), "Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg")

                If File.Exists(pathimg) Then
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/Novedad__" & hidDocumento.Value & "_" & hidImagen.Value & ".jpeg"
                Else
                    imgEquipo.Visible = True
                    imgEquipo.ImageUrl = "ImagenesNovedades/nophoto.jpg"
                End If
                hidImagen.Value = hidImagen.Value + 1
            End If
        Catch ex As Exception
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "btAceptar_Click", "Swal.fire(" & ex.Message & ");", True)
        End Try
    End Sub

    Private Sub fluImagen_DataBinding(sender As Object, e As EventArgs) Handles fluImagen.DataBinding

    End Sub

    Protected Sub btnCargarArchivo_Click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
End Class
