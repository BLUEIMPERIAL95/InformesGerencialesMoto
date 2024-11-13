Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data
Imports Newtonsoft.Json
Imports RestSharp
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web
Imports RestSharp.Authenticators
Imports System.Collections
Imports System.Dynamic

Public Class Cuentacob
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 21/11/2020 METODO PARA SELECCIONAR CUENTAS DE COBRO COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_cuentas_cobro_completo(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cuentas_cobro_completo"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc

    End Function

    ''' <summary>
    ''' LGUF 15/02/2021 METODO PARA CAPTURAR CUENTAS DE COBRO POR BUSQUEDA
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="tip"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function seleccionar_datos_cuentas_cobro_busqueda(ByVal ide As Integer, ByVal tip As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cuentas_cobro_busqueda"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_bus", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@par_bus", SqlDbType.VarChar, 50, par)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc

    End Function

    ''' <summary>
    ''' LGUF 21/11/2020 METODO PARA SELECCIONAR RESOLUCION POR AGENCIA
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_resolucion_por_agencia(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtres As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_resolucion_por_agencia_cuenta_cobro"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_agc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtres)
        Catch ex As Exception

        End Try

        Return dtres
    End Function

    ''' <summary>
    ''' LGUF 20/11/2020 METODO PARA SELECCIONAR NUMERO PROXIMO POR RESOLUCION CUENTA COBRO
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_proximo_numero_cuenta_cobro_por_resulucion(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtres As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_numero_proximo_cuenta_cobro_por_resolucion"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_rec", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtres)
        Catch ex As Exception

        End Try

        Return dtres
    End Function

    ''' <summary>
    ''' LGUF 21/11/2020 METODO PARA GUARDAR CUENTAS DE COBRO
    ''' </summary>
    ''' <param name="idcuc"></param>
    ''' <param name="idemc"></param>
    ''' <param name="idagc"></param>
    ''' <param name="idrec"></param>
    ''' <param name="fec"></param>
    ''' <param name="num"></param>
    ''' <param name="ter"></param>
    ''' <param name="aut"></param>
    ''' <param name="obs"></param>
    ''' <returns></returns>
    Function guardar_cuentas_cobro(ByVal idcuc As Integer, ByVal idemc As Integer, ByVal idagc As Integer, ByVal idrec As Integer, ByVal fec As String, ByVal tip As String,
                                   ByVal num As String, ByVal ter As String, ByVal aut As String, ByVal idusu As Integer, ByVal obs As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_cuenta_cobro"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cuc", SqlDbType.BigInt, 11, idcuc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emc", SqlDbType.Int, 11, idemc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_agc", SqlDbType.Int, 11, idagc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_rec", SqlDbType.Int, 11, idrec)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_cuc", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_cuc", SqlDbType.VarChar, 15, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_cuc", SqlDbType.VarChar, 20, num)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_aut", SqlDbType.Int, 11, aut)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, idusu)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_cuc", SqlDbType.VarChar, 1000, obs)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 21/11/2020 METODO PARA SELECCIONAR CUENTAS DE COBRO POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_proximo_numero_cuenta_cobro_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cuentas_cobro_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_icc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc
    End Function

    ''' <summary>
    ''' LGUF 23/11/2020 METODO PARA ELIMINAR CUENTAS COBRO ENCABEZADO
    ''' </summary>
    ''' <param name="idcuc"></param>
    ''' <returns></returns>
    Function eliminar_cuentas_cobro_encabezado(ByVal idcuc As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cuenta_cobro_encabezado"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cuc", SqlDbType.BigInt, 11, idcuc)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 23/11/2020 METODO PARA SELECCIONAR DETALLE CUENTA COBRO POR ID CUENTA
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_cuenta_cobro_detalle_por_id_cuenta(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cuenta_cobro_detalle_por_id_cuenta"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cuc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc
    End Function

    ''' <summary>
    ''' LGUF METODO PARA GUARDAR DETALLE DE CUENTAS DE COBRO
    ''' </summary>
    ''' <param name="idccd"></param>
    ''' <param name="idcuc"></param>
    ''' <param name="coccd"></param>
    ''' <param name="can"></param>
    ''' <param name="cos"></param>
    ''' <param name="tot"></param>
    ''' <param name="obs"></param>
    ''' <returns></returns>
    Function guardar_detalle_cuentas_cobro(ByVal idccd As Integer, ByVal idcuc As Integer, ByVal idrtc As Integer, ByVal coccd As String, ByVal can As Integer,
                                           ByVal cos As Decimal, ByVal ret As Decimal, ByVal tot As Decimal, ByVal obs As String) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_detalle_cuenta_cobro"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ccd", SqlDbType.BigInt, 11, idccd)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_cuc", SqlDbType.BigInt, 11, idcuc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_rtc", SqlDbType.Int, 11, idrtc)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_ccd", SqlDbType.VarChar, 1000, coccd)
            dbSQL.add_parameter(ParameterDirection.Input, "@can_ccd", SqlDbType.Int, 11, can)
            dbSQL.add_parameter(ParameterDirection.Input, "@cos_ccd", SqlDbType.Decimal, 18, cos)
            dbSQL.add_parameter(ParameterDirection.Input, "@por_ccd", SqlDbType.Decimal, 18, ret)
            dbSQL.add_parameter(ParameterDirection.Input, "@tot_ccd", SqlDbType.Decimal, 18, tot)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_ccd", SqlDbType.VarChar, 1000, obs)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 23/11/2020 METODO PARA SELECCIONAR CUENTAS COBRO DETALLE POR ID DETALLE
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_cuenta_cobro_detalle_por_id_detalle(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_cuenta_cobro_detalle_por_id_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ccd", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc
    End Function

    ''' <summary>
    ''' LGUF 23/11/2020 METODO PARA ELIMINAR DETALLE CUENTA COBRO POR ID DETALLE
    ''' </summary>
    ''' <param name="idccd"></param>
    ''' <returns></returns>
    Function eliminar_cuentas_cobro_detalle(ByVal idccd As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_cuenta_cobro_detalle_por_id_detalle"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ccd", SqlDbType.BigInt, 11, idccd)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 30/11/2020 METODO PARA SELECCIONAR RETENCIONES CUENTA COBRO POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_retencion_cuenta_cobro_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_retenciones_cuenta_cobro_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_rtc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc
    End Function

    ''' <summary>
    ''' LGUF 14/07/2022
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_agencias_cuenta_cobro_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcuc As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_agencias_cuenta_cobro_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_agc", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtcuc)
        Catch ex As Exception

        End Try

        Return dtcuc
    End Function
End Class
