Imports Microsoft.VisualBasic
Imports System.Data
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Text
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.html.simpleparser
Imports iTextSharp.text.pdf
Imports System.util

Public Class PageEventHelper
    Inherits PdfPageEventHelper

    Private cb As PdfContentByte
    Private template As PdfTemplate

    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        cb = writer.DirectContent
        template = cb.CreateTemplate(50, 50)
    End Sub

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal doc As Document)
        'Dim grey As BaseColor = New BaseColor(128, 128, 128)
        'Dim font As iTextSharp.text.Font = FontFactory.GetFont("Arial", 15, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
        'Dim footerTbl As PdfPTable = New PdfPTable(1)
        'footerTbl.TotalWidth = doc.PageSize.Width
        'Dim myFooter As Chunk = New Chunk("Página " & (doc.PageNumber), FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 15, grey))
        'Dim footer As PdfPCell = New PdfPCell(New Phrase(myFooter))
        'footer.Border = iTextSharp.text.Rectangle.NO_BORDER
        'footer.HorizontalAlignment = Element.ALIGN_CENTER
        'footerTbl.AddCell(footer)
        'footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 80), writer.DirectContent)
    End Sub

    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
    End Sub
End Class
