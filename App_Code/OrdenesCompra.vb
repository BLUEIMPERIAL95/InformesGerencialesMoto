Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data

Public Class OrdenesCompra

    ''' <summary>
    ''' LGUF 17/07/2020 METODO PARA TRAER ORDENES COMPRA COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_ordenes_compra_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_ordenes_compra_completo"

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    ''' <summary>
    ''' LGUF 17/07/2020 METODO PARA GUARDAR ORDENES COMPRA
    ''' </summary>
    ''' <param name="idord"></param>
    ''' <param name="fec"></param>
    ''' <param name="num"></param>
    ''' <param name="pro"></param>
    ''' <param name="sol"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_ordenes_compra(ByVal idord As Integer, ByVal fec As String, ByVal num As String, ByVal pro As String, ByVal sol As String,
                                    ByVal aut As String, ByVal obs As String, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_ordenes_compra"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_orc", SqlDbType.BigInt, 11, idord)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_orc", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_orc", SqlDbType.VarChar, 20, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, pro)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_sol", SqlDbType.Int, 11, sol)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_aut", SqlDbType.Int, 11, aut)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_orc", SqlDbType.VarChar, 1000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_orc", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 17/07/2020 METODO PARA SELECCIONAR NUMERO PROXIMA ORDEN COMPRA
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_proxima_orden_compra() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtord As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_orden_compra"

            dbSQL.query_sql(dtord)
        Catch ex As Exception

        End Try

        Return dtord

    End Function

    ''' <summary>
    ''' LGUF 17/07/2020 METODO PARA SELECCIONAR ORDEN COMPRA POR ID
    ''' </summary>
    ''' <param name="idord"></param>
    ''' <returns></returns>
    Function seleccionar_orden_compra_por_id(ByVal idord As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtord As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_ordenes_compra_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ord", SqlDbType.BigInt, 11, idord)

            dbSQL.query_sql(dtord)
        Catch ex As Exception

        End Try

        Return dtord

    End Function

    ''' <summary>
    ''' METODO PARA ELIMINAR ORDENES DE COMPRA POR ID
    ''' </summary>
    ''' <param name="idord"></param>
    ''' <returns></returns>
    Function eliminar_ordenes_compra(ByVal idord As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_orden_compra"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ord", SqlDbType.BigInt, 11, idord)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 21/07/2020 METODO PARA SELECCIONAR DETALLE ORDEN COMPRA POR ID COMPRA
    ''' </summary>
    ''' <param name="idord"></param>
    ''' <returns></returns>
    Function seleccionar_detalle_ordenes_compra_por_id_orden(ByVal idord As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_detalle_ordenes_compra_por_id_compra"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_orc", SqlDbType.BigInt, 11, idord)

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    ''' <summary>
    ''' LGUF 21/07/2020 METODO PARA GUARDAR EL DETALLE DE LAS ORDENES DE COMPRA
    ''' </summary>
    ''' <param name="iddor"></param>
    ''' <param name="idord"></param>
    ''' <param name="idequi"></param>
    ''' <param name="can"></param>
    ''' <param name="cos"></param>
    ''' <param name="iva"></param>
    ''' <param name="ret"></param>
    ''' <param name="val"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_detalle_ordenes_compra(ByVal iddor As Integer, ByVal idord As Integer, ByVal idequi As Integer, ByVal can As Decimal, ByVal cos As Decimal, ByVal iva As Decimal,
                                            ByVal ret As Decimal, ByVal val As Decimal, ByVal obs As String, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_detalle_ordenes_compra"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_dor", SqlDbType.BigInt, 11, iddor)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_orc", SqlDbType.BigInt, 11, idord)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_equ", SqlDbType.Int, 11, idequi)
            dbSQL.add_parameter(ParameterDirection.Input, "@can_dor", SqlDbType.Decimal, 18, can)
            dbSQL.add_parameter(ParameterDirection.Input, "@cos_dor", SqlDbType.Decimal, 18, cos)
            dbSQL.add_parameter(ParameterDirection.Input, "@iva_dor", SqlDbType.Decimal, 18, iva)
            dbSQL.add_parameter(ParameterDirection.Input, "@ret_dor", SqlDbType.Decimal, 18, ret)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_dor", SqlDbType.Decimal, 18, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_dor", SqlDbType.VarChar, 1000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_orc", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 21/07/2020 METODO PARA SELECCIONAR EL VALOR TOTAL DE UNA COMPRA
    ''' </summary>
    ''' <param name="idord"></param>
    ''' <returns></returns>
    Function seleccionar_valor_total_por_id_orden(ByVal idord As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_valor_total_orden_compra"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_orc", SqlDbType.BigInt, 11, idord)

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    ''' <summary>
    ''' LGUF 21/07/2020 METODO PARA SELECCIONAR DETALLE ORDEN DE COMPRO POR ID DETALLE
    ''' </summary>
    ''' <param name="iddrd"></param>
    ''' <returns></returns>
    Function seleccionar_detalle_orden_por_id_detalle(ByVal iddrd As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtequ As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_detalle_ordenes_compra_por_id_detall"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_drd", SqlDbType.BigInt, 11, iddrd)

            dbSQL.query_sql(dtequ)
        Catch ex As Exception

        End Try

        Return dtequ

    End Function

    ''' <summary>
    ''' LGUF 21/07/2020 METODO PARA ELIMINAR DETALLE ORDEN COMPRA POR ID DETALLE
    ''' </summary>
    ''' <param name="iddrd"></param>
    ''' <returns></returns>
    Function eliminar_detalle_ordenes_compra(ByVal iddrd As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_detalle_ordenes_compra_por_id_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_drd", SqlDbType.BigInt, 11, iddrd)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function
End Class
