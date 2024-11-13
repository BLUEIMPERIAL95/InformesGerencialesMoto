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

Public Class facturas
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 27/01/2020 METODO PARA SELECCIONAR FACTURAS COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_datos_facturas() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtfac As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_facturas_completo"

            dbSQL.query_sql(dtfac)
        Catch ex As Exception

        End Try

        Return dtfac

    End Function

    ''' <summary>
    ''' LGUF 28/01/2020 METODO PARA GUARDAR FACTURAS.
    ''' </summary>
    ''' <param name="cot"></param>
    ''' <param name="pro"></param>
    ''' <param name="fac"></param>
    ''' <param name="cer"></param>
    ''' <param name="fec"></param>
    ''' <param name="fve"></param>
    ''' <param name="tpr"></param>
    ''' <param name="tcl"></param>
    ''' <param name="veh"></param>
    ''' <param name="usu"></param>
    ''' <param name="cob"></param>
    ''' <param name="val"></param>
    ''' <param name="vle"></param>
    ''' <param name="rec"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_facturas(ByVal cot As Integer, ByVal pro As Integer, ByVal fac As Integer, ByVal cer As Int64, ByVal fec As String, ByVal fve As String,
                              ByVal tpr As Integer, ByVal tcl As Integer, ByVal veh As String, ByVal usu As Integer, ByVal cob As Integer, ByVal val As Decimal,
                              ByVal vle As Decimal, ByVal rec As Integer, ByVal act As Integer) As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_facturas"
            dbSQL.add_parameter(ParameterDirection.Input, "@cot_fac", SqlDbType.BigInt, 11, cot)
            dbSQL.add_parameter(ParameterDirection.Input, "@pro_fac", SqlDbType.BigInt, 11, pro)
            dbSQL.add_parameter(ParameterDirection.Input, "@num_fac", SqlDbType.BigInt, 11, fac)
            dbSQL.add_parameter(ParameterDirection.Input, "@cer_fac", SqlDbType.BigInt, 11, cer)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fac", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@fve_fac", SqlDbType.VarChar, 15, fve)
            dbSQL.add_parameter(ParameterDirection.Input, "@tep_fac", SqlDbType.Int, 11, tpr)
            dbSQL.add_parameter(ParameterDirection.Input, "@tec_fac", SqlDbType.Int, 11, tcl)
            dbSQL.add_parameter(ParameterDirection.Input, "@pla_fac", SqlDbType.VarChar, 20, veh)
            dbSQL.add_parameter(ParameterDirection.Input, "@usu_fac", SqlDbType.Int, 11, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@cob_fac", SqlDbType.Int, 11, cob)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_fac", SqlDbType.Decimal, 11, val)
            dbSQL.add_parameter(ParameterDirection.Input, "@vae_fac", SqlDbType.Decimal, 11, vle)
            dbSQL.add_parameter(ParameterDirection.Input, "@rec_fac", SqlDbType.BigInt, 11, rec)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_fac", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 28/01/2019 METODO PARA SELECCIONAR FACTURAS POR ID FACTURA
    ''' </summary>
    ''' <param name="idfac"></param>
    ''' <returns></returns>
    Function seleccionar_datos_facturas_por_id(ByVal idfac As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtfac As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_facturas_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_fac", SqlDbType.BigInt, 11, idfac)

            dbSQL.query_sql(dtfac)
        Catch ex As Exception

        End Try

        Return dtfac

    End Function

    ''' <summary>
    ''' LGUF 28/01/2020 METODO PARA ELIMINAR FACTURAS POR ID
    ''' </summary>
    ''' <param name="idfac"></param>
    ''' <returns></returns>
    Function eliminar_factura(ByVal idfac As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_facturas"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_fac", SqlDbType.BigInt, 11, idfac)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 05/02/2020 METODO PARA SELECCIONAR INFORMACION DE FACTURAS TRAMITAR POR PROVEEDOR Y RANGO DE FECHA
    ''' </summary>
    ''' <param name="fecini"></param>
    ''' <param name="fecfin"></param>
    ''' <param name="proved"></param>
    ''' <returns></returns>
    Function seleccionar_datos_facturas_informe(ByVal fecini As String, ByVal fecfin As String, ByVal proved As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtfac As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_facturas_responsabilidad_civil"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fecini)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fecfin)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pro", SqlDbType.Int, 11, proved)

            dbSQL.query_sql(dtfac)
        Catch ex As Exception

        End Try

        Return dtfac

    End Function


    ''' <summary>
    ''' LGUF 2020/05/22 METODO POST PARA CAPTURAR INFORMACION DE API ALBATEQ
    ''' </summary>
    ''' <param name="url"></param>
    ''' <param name="json"></param>
    ''' <param name="autorizacion"></param>
    ''' <returns></returns>
    Public Function Post(ByVal url As String, ByVal json As String, ByVal Optional autorizacion As String = Nothing) As String
        Try
            Dim client = New RestClient(url)
            Dim request = New RestRequest(Method.POST)
            request.AddHeader("content-type", "application/json")
            request.AddParameter("application/json", json, ParameterType.RequestBody)

            If autorizacion IsNot Nothing Then
                request.AddHeader("Authorization", autorizacion)
            End If

            Dim response As IRestResponse = client.Execute(request)
            Dim datos As String = JsonConvert.DeserializeObject(response.Content)
            Return datos
        Catch ex As Exception
            Dim strError = ex.Message
            Return "null"
        End Try
    End Function

    ''' <summary>
    ''' LGUF 26/01/2022 METODO PARA ENVIAR CORREOS AUTOMATICOS PENDIENTES POR FACTURAR SYSTRAM
    ''' </summary>
    ''' <returns></returns>
    Function enviar_correo_pendientes_facturar_automaticos() As String
        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_enviar_correo_pendientes_facturar_automaticos"

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function
End Class
