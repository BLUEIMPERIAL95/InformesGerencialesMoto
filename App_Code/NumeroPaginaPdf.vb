Imports Microsoft.VisualBasic
Imports iTextSharp.text.pdf
Imports iTextSharp.text
Imports iTextSharp.text.Font
Imports System.IO

Public Class NumeroPaginaPdf
    Inherits PdfPageEventHelper

    Private cb As PdfContentByte
    Private template As PdfTemplate

    Public Overrides Sub OnOpenDocument(ByVal writer As PdfWriter, ByVal document As Document)
        cb = writer.DirectContent
        template = cb.CreateTemplate(50, 50)
    End Sub

    Public Overrides Sub OnEndPage(ByVal writer As PdfWriter, ByVal doc As Document)
        Dim grey As Color = New Color(128, 128, 128)
        Dim font As iTextSharp.text.Font = FontFactory.GetFont("Verdana", 10, iTextSharp.text.Font.BOLD, grey)
        Dim footerTbl As PdfPTable = New PdfPTable(1)
        footerTbl.TotalWidth = doc.PageSize.Width
        Dim myFooter As Chunk = New Chunk("Página " & (doc.PageNumber), FontFactory.GetFont(FontFactory.COURIER_BOLD, 9, 0))
        Dim footer As PdfPCell = New PdfPCell(New Phrase(myFooter))
        footer.Border = iTextSharp.text.Rectangle.NO_BORDER
        footer.HorizontalAlignment = Element.ALIGN_CENTER
        footerTbl.AddCell(footer)
        footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 80), writer.DirectContent)
    End Sub

    Public Overrides Sub OnCloseDocument(ByVal writer As PdfWriter, ByVal document As Document)
        MyBase.OnCloseDocument(writer, document)
    End Sub
End Class
