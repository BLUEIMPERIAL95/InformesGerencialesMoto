Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class descuentoscolanta
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 27/09/2022 METODO PARA SELECCIONAR VALORES DESCUENTOS SYSTRAM LISTADO
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_valores_descuentos_systram_listado(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtdes As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_valores_descuentos_systram_listado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 15, par)

            dbSQL.query_sql(dtdes)
        Catch ex As Exception

        End Try

        Return dtdes

    End Function

    ''' <summary>
    ''' LGUF 27/09/2022 METODO PARA GUARDAR VALORES DESCUENTOS SYSTRAM
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="pla"></param>
    ''' <param name="doc"></param>
    ''' <param name="des"></param>
    ''' <param name="mes"></param>
    ''' <param name="año"></param>
    ''' <param name="val"></param>
    ''' <param name="usu"></param>
    ''' <returns></returns>
    Function guardar_valores_descuentos_systram(ByVal ide As Integer, ByVal pla As String, ByVal doc As String, ByVal des As Integer,
                                                ByVal mes As Integer, ByVal año As Integer, ByVal val As Decimal, ByVal usu As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_guardar_valores_descuentos_systram"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_vad", SqlDbType.BigInt, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_vad", SqlDbType.VarChar, 10, pla)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_vad", SqlDbType.VarChar, 20, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_des", SqlDbType.Int, 11, des)
            dbSQL.add_parameter(ParameterDirection.Input, "@mes_vad", SqlDbType.Int, 11, mes)
            dbSQL.add_parameter(ParameterDirection.Input, "@año_vad", SqlDbType.Int, 11, año)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_vad", SqlDbType.Decimal, 11, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 27/09/2022 METODO PARA ELIMINAR VALORES DESCUENTOS SYSTRAM POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_valores_descuentos_systram_por_id(ByVal ide As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""

        Try
            dbSQL.str_storedprocedure = "sp_eliminar_valores_descuentos_systram_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_vad", SqlDbType.BigInt, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception

        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 29/09/2022 METODO PARA SELECCIONAR VALORES DESCUENTOS COLANTA INFORME
    ''' </summary>
    ''' <param name="mes"></param>
    ''' <param name="año"></param>
    ''' <returns></returns>
    Function seleccionar_valores_descuentos_systram_informe(ByVal mes As Integer, ByVal año As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtdes As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_valores_descuentos_systram_informe"
            dbSQL.add_parameter(ParameterDirection.Input, "@mes_vad", SqlDbType.Int, 11, mes)
            dbSQL.add_parameter(ParameterDirection.Input, "@año_vad", SqlDbType.VarChar, 15, año)

            dbSQL.query_sql(dtdes)
        Catch ex As Exception

        End Try

        Return dtdes

    End Function
End Class
