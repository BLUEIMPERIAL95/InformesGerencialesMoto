Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Informes
    Dim dbi As New database_informes
    Dim dbsql As New databaseSQL
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 06/12/2019 METODO PARA EJECUTAR CONSULTAS EN LA BD DE MYSQL
    ''' </summary>
    ''' <param name="SQL"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ejecutar_query_bd(ByVal SQL As String) As DataTable
        Dim dtinformes As New DataTable

        Try

            dtinformes = dbi.exec_query(SQL)

        Catch ex As Exception
            Dim strError = ex.Message
        End Try

        Return dtinformes
    End Function

    Function ejecutar_query_bd_sql(ByVal SQL As String, ByVal emp As Integer) As DataTable
        Dim dtinformes As New DataTable

        Try
            If emp = 1 Then
                dtinformes = dbsql.exec_query_tralin(SQL)
            Else
                If emp = 2 Then
                    dtinformes = dbsql.exec_query(SQL)
                Else
                    If emp = 7 Then
                        dtinformes = dbsql.exec_query_cia(SQL)
                    Else
                        If emp = 9 Then
                            dtinformes = dbsql.exec_query_traseg(SQL)
                        Else
                            If emp = 13 Then
                                dtinformes = dbsql.exec_query_labor(SQL)
                            End If
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            Dim strError = ex.Message
        End Try

        Return dtinformes
    End Function

    Function ejecutar_query_cartera_bd_sql(ByVal SQL As String, ByVal emp As Integer) As DataTable
        Dim dtinformes As New DataTable

        Try
            If emp = 6 Then
                dtinformes = dbsql.exec_query(SQL)
            Else
                If emp = 4 Then
                    dtinformes = dbsql.exec_query_traseg(SQL)
                Else
                    If emp = 7 Then
                        dtinformes = dbsql.exec_query_tralin(SQL)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim strError = ex.Message
        End Try

        Return dtinformes
    End Function
End Class
