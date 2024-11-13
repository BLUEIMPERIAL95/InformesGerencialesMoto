Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Autocompletar
    Inherits System.Web.Services.WebService

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Function GetCompletionList() As List(Of String)
        Dim cster As New equipos
        Dim dtzon As New DataTable

        Dim countryNames As List(Of String) = New List(Of String)()
        dtzon = cster.seleccionar_zonas_automcompletar("MEDE")

        If dtzon.Rows.Count Then
            For i As Integer = 0 To dtzon.Rows.Count - 1
                countryNames.Add(dtzon.Rows(i)("Zona").ToString)
            Next
        End If

        Return countryNames
    End Function

End Class