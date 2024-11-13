Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data
Public Class equipos
    Dim dbi As New database_informes
    Dim dbiofi As New database

    ''' <summary>
    ''' LGUF 03/01/2020 CAPTURAR DATOS DE TERCEROS
    ''' </summary>
    ''' <returns></returns>
    Function capturar_datos_terceros() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_terceros_completo"

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 27/01/2020 METODO PARA SELECCIONAR TERCEROS POR BUSQUEDA
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function capturar_datos_terceros_busqueda(ByVal ind As Integer, ByVal par As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_terceros_busqueda"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 100, par)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    '''' <summary>
    '''' LGUF 07/01/2020 METODO PARA GUARDAR TERCERSO
    '''' </summary>
    '''' <param name="idter"></param>
    '''' <param name="tipodoc"></param>
    '''' <param name="doc"></param>
    '''' <param name="nom"></param>
    '''' <param name="dir"></param>
    '''' <param name="tel"></param>
    '''' <param name="cel"></param>
    '''' <param name="cor"></param>
    '''' <param name="act"></param>
    '''' <returns></returns>
    'Function guardar_terceros(ByVal idter As Integer, ByVal tipodoc As String, ByVal doc As String, ByVal dig As Integer, ByVal nom As String, ByVal no2 As String, ByVal ap1 As String,
    '                          ByVal ap2 As String, ByVal dir As String, ByVal tel As String, ByVal cel As String, ByVal cor As String, ByVal zon As Integer, ByVal emp As String,
    '                          ByVal age As String, ByVal are As Integer, ByVal car As Integer, ByVal act As Integer) As String

    '    Dim dbSQL As New databaseSQL
    '    Dim strRespuesta As String
    '    strRespuesta = ""
    '    Try
    '        dbSQL.str_storedprocedure = "sp_guardar_terceros"
    '        dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@tip_doc", SqlDbType.VarChar, 100, tipodoc)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@doc_ter", SqlDbType.VarChar, 50, doc)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@dig_ter", SqlDbType.Int, 11, dig)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@nom_ter", SqlDbType.VarChar, 200, nom)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@no2_ter", SqlDbType.VarChar, 200, no2)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@ap1_ter", SqlDbType.VarChar, 200, ap1)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@ap2_ter", SqlDbType.VarChar, 200, ap2)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@dir_ter", SqlDbType.VarChar, 200, dir)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@tel_ter", SqlDbType.VarChar, 50, tel)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@cel_ter", SqlDbType.VarChar, 50, cel)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@cor_ter", SqlDbType.VarChar, 100, cor)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@ide_zon", SqlDbType.Int, 11, zon)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@emp_ter", SqlDbType.VarChar, 100, emp)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@age_ter", SqlDbType.VarChar, 100, age)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@are_ter", SqlDbType.Int, 11, are)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@car_ter", SqlDbType.Int, 11, car)
    '        dbSQL.add_parameter(ParameterDirection.Input, "@act_ter", SqlDbType.Bit, 1, act)
    '        'dbSQL.add_parameter(ParameterDirection.Input, "@img_ter", SqlDbType.VarBinary, 1, img)

    '        strRespuesta = dbSQL.execute_sql
    '    Catch ex As Exception
    '        strRespuesta = ex.Message
    '    End Try

    '    Return strRespuesta
    'End Function

    ''' <summary>
    ''' LGUF 07/01/2020 METODO PARA GUARDAR TERCERSO
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <param name="tipodoc"></param>
    ''' <param name="doc"></param>
    ''' <param name="nom"></param>
    ''' <param name="dir"></param>
    ''' <param name="tel"></param>
    ''' <param name="cel"></param>
    ''' <param name="cor"></param>
    ''' <param name="act"></param>
    ''' <param name="tip"></param>
    ''' <returns></returns>
    Function guardar_terceros(ByVal idter As Integer, ByVal tipodoc As String, ByVal doc As String, ByVal dig As Integer, ByVal nom As String, ByVal no2 As String, ByVal ap1 As String,
                              ByVal ap2 As String, ByVal dir As String, ByVal tel As String, ByVal cel As String, ByVal cor As String, ByVal zon As Integer, ByVal emp As String,
                              ByVal age As String, ByVal are As Integer, ByVal car As Integer, ByVal act As Integer, ByVal tip As String) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_terceros"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, idter)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_doc", SqlDbType.VarChar, 100, tipodoc)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_ter", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@dig_ter", SqlDbType.Int, 11, dig)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_ter", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@no2_ter", SqlDbType.VarChar, 200, no2)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap1_ter", SqlDbType.VarChar, 200, ap1)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap2_ter", SqlDbType.VarChar, 200, ap2)
            dbSQL.add_parameter(ParameterDirection.Input, "@dir_ter", SqlDbType.VarChar, 200, dir)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_ter", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_ter", SqlDbType.VarChar, 50, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_ter", SqlDbType.VarChar, 100, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_zon", SqlDbType.Int, 11, zon)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_ter", SqlDbType.VarChar, 100, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@age_ter", SqlDbType.VarChar, 100, age)
            dbSQL.add_parameter(ParameterDirection.Input, "@are_ter", SqlDbType.Int, 11, are)
            dbSQL.add_parameter(ParameterDirection.Input, "@car_ter", SqlDbType.Int, 11, car)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_ter", SqlDbType.Bit, 1, act)
            dbSQL.add_parameter(ParameterDirection.Input, "@tipo", SqlDbType.VarChar, 50, tip)
            'dbSQL.add_parameter(ParameterDirection.Input, "@img_ter", SqlDbType.VarBinary, 1, img)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 07/01/2020 METODO PARA SELECCIONAR TERCEROS POR ID
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <returns></returns>
    Function capturar_datos_terceros_por_id(ByVal idter As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_terceros_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_ter", SqlDbType.Int, 11, idter)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 07/01/2020 METODO PARA ELIMINAR TERCEROS POR ID
    ''' </summary>
    ''' <param name="idter"></param>
    ''' <returns></returns>
    Function eliminar_terceros(ByVal idter As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_terceros"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_ter", SqlDbType.Int, 11, idter)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 05/06/2020 METODO PARA GUARDAR EMPLEADOS
    ''' </summary>
    ''' <param name="idemp"></param>
    ''' <param name="tipodoc"></param>
    ''' <param name="doc"></param>
    ''' <param name="dig"></param>
    ''' <param name="nom"></param>
    ''' <param name="dir"></param>
    ''' <param name="tel"></param>
    ''' <param name="cel"></param>
    ''' <param name="cor"></param>
    ''' <param name="emp"></param>
    ''' <param name="age"></param>
    ''' <param name="are"></param>
    ''' <param name="car"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_empleados(ByVal idemp As Integer, ByVal tipodoc As String, ByVal doc As String, ByVal dig As Integer, ByVal nom As String, ByVal dir As String, ByVal tel As String,
                              ByVal cel As String, ByVal cor As String, ByVal emp As String, ByVal age As String, ByVal are As Integer, ByVal car As Integer, ByVal act As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_empleados"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emp", SqlDbType.Int, 11, idemp)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_doc", SqlDbType.VarChar, 100, tipodoc)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_emp", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@dig_emp", SqlDbType.Int, 11, dig)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_emp", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@dir_emp", SqlDbType.VarChar, 200, dir)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_emp", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_emp", SqlDbType.VarChar, 50, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_emp", SqlDbType.VarChar, 100, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_emp", SqlDbType.VarChar, 100, emp)
            dbSQL.add_parameter(ParameterDirection.Input, "@age_emp", SqlDbType.VarChar, 100, age)
            dbSQL.add_parameter(ParameterDirection.Input, "@are_emp", SqlDbType.Int, 11, are)
            dbSQL.add_parameter(ParameterDirection.Input, "@car_emp", SqlDbType.Int, 11, car)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_emp", SqlDbType.Bit, 1, act)
            'dbSQL.add_parameter(ParameterDirection.Input, "@img_ter", SqlDbType.VarBinary, 1, img)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 05/06/2020 METODO PARA SELECCIONAR EMPLEADOS COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function capturar_datos_empleados() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_empleados_completo"

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 05/06/2020 METODO PARA SELECCIONAR EMPLEADOS POR ID
    ''' </summary>
    ''' <param name="idemp"></param>
    ''' <returns></returns>
    Function capturar_datos_empleados_por_id(ByVal idemp As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_empleado_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_emp", SqlDbType.Int, 11, idemp)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 10/07/2020 METODO PARA CAPTURAR EMPLEADOS POR DOCUMENTO
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function capturar_datos_empleados_por_documento(ByVal doc As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_empleado_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_emp", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF METODO PARA ELIMINAR EMPLEADOS
    ''' </summary>
    ''' <param name="idemp"></param>
    ''' <returns></returns>
    Function eliminar_empleados(ByVal idemp As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_empleados"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_emp", SqlDbType.Int, 11, idemp)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 09/06/2020 METODO PARA CAPTURAR TERCEROS POR DOCUMENTO
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function capturar_datos_terceros_por_documento(ByVal doc As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_terceros_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_ter", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter
    End Function

    ''' <summary>
    ''' LGUF 09/06/2020 METODO PARA GUARDAR REGISTRO DE ENTRADA
    ''' </summary>
    ''' <param name="ter"></param>
    ''' <param name="tem"></param>
    ''' <param name="are"></param>
    ''' <returns></returns>
    Function guardar_registro_entrada(ByVal ter As Integer, ByVal tem As Decimal, ByVal are As Integer, ByVal tel As String, ByVal cor As String, ByVal eps As Integer, ByVal arl As Integer, ByVal vad As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_registro_entrada"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@tem_ree", SqlDbType.Decimal, 18, tem)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_are", SqlDbType.Int, 11, are)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_ree", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_ree", SqlDbType.VarChar, 50, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_eps", SqlDbType.Int, 11, eps)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_arl", SqlDbType.Int, 11, arl)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_dat", SqlDbType.Bit, 1, arl)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 09/05/2020 METODO PARA SELECCIONAR EL REGISTRO ENTRADA DIARIO
    ''' </summary>
    ''' <returns></returns>
    Function capturar_datos_registro_entrada_diario(ByVal emp As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_registro_entrada_diario"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_reg", SqlDbType.VarChar, 15, DateTime.Now.ToString("yyyy-MM-dd"))
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_ter", SqlDbType.VarChar, 15, emp)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter
    End Function

    ''' <summary>
    ''' LGUF 04/11/2021 METODO PARA SELECCIONAR REGISTRO DE ENTRADA
    ''' </summary>
    ''' <param name="ter"></param>
    ''' <returns></returns>
    Function seleccionar_registro_entrada(ByVal ter As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_registro_entrada"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ter", SqlDbType.Int, 11, ter)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_reg", SqlDbType.VarChar, 15, DateTime.Now.ToString("yyyy-MM-dd"))

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter
    End Function

    ''' <summary>
    ''' LGUF 17/06/2020 METODO PARA SELECCIONAR REGISTRO DE ENTRADA POR FECHA
    ''' </summary>
    ''' <param name="fec"></param>
    ''' <returns></returns>
    Function capturar_datos_registro_entrada_por_fecha(ByVal fec As String, ByVal fecini As String, ByVal emp As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_registro_entrada_rango"
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ini", SqlDbType.VarChar, 15, fec)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_fin", SqlDbType.VarChar, 15, fecini)
            dbSQL.add_parameter(ParameterDirection.Input, "@emp_ter", SqlDbType.VarChar, 15, emp)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter
    End Function

    ''' <summary>
    ''' METODO PARA SELECCIONAR ZONAS PARA AUTOCOMPLETAR
    ''' </summary>
    ''' <param name="zon"></param>
    ''' <returns></returns>
    Function seleccionar_zonas_automcompletar(ByVal zon As String) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_zona_municipio_dpto_combo"
            dbSQL.add_parameter(ParameterDirection.Input, "@par_zon", SqlDbType.VarChar, 50, zon)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter
    End Function

    ''' <summary>
    ''' LGUF 02/02/2021 METODO PARA SELECCIONAR GENERADORES SYSTRAM COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_generadores_sytram_completo(ByVal tip As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtgen As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_generadores_systram_completo"
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_gen", SqlDbType.Int, 11, tip)

            dbSQL.query_sql(dtgen)
        Catch ex As Exception

        End Try

        Return dtgen

    End Function

    ''' <summary>
    ''' LGUF 02/02/2021 METODO PARA GUARDAR GENERADORES SYSTRAM
    ''' </summary>
    ''' <param name="idgen"></param>
    ''' <param name="tipodoc"></param>
    ''' <param name="doc"></param>
    ''' <param name="dig"></param>
    ''' <param name="nom"></param>
    ''' <param name="no2"></param>
    ''' <param name="ap1"></param>
    ''' <param name="ap2"></param>
    ''' <param name="dir"></param>
    ''' <param name="tel"></param>
    ''' <param name="cel"></param>
    ''' <param name="cor"></param>
    ''' <param name="zon"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_generadores_systram(ByVal idgen As Integer, ByVal tipodoc As String, ByVal doc As String, ByVal dig As Integer, ByVal nom As String, ByVal no2 As String, ByVal ap1 As String,
                              ByVal ap2 As String, ByVal dir As String, ByVal tel As String, ByVal cel As String, ByVal cor As String, ByVal zon As Integer, ByVal tip As Integer, ByVal act As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_generadores_systram"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_gen", SqlDbType.Int, 11, idgen)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_doc", SqlDbType.VarChar, 100, tipodoc)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_gen", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@dig_gen", SqlDbType.Int, 11, dig)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_gen", SqlDbType.VarChar, 200, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@no2_gen", SqlDbType.VarChar, 200, no2)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap1_gen", SqlDbType.VarChar, 200, ap1)
            dbSQL.add_parameter(ParameterDirection.Input, "@ap2_gen", SqlDbType.VarChar, 200, ap2)
            dbSQL.add_parameter(ParameterDirection.Input, "@dir_gen", SqlDbType.VarChar, 200, dir)
            dbSQL.add_parameter(ParameterDirection.Input, "@tel_gen", SqlDbType.VarChar, 50, tel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_gen", SqlDbType.VarChar, 50, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_gen", SqlDbType.VarChar, 100, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_zon", SqlDbType.Int, 11, zon)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_gen", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_gen", SqlDbType.Bit, 1, act)
            'dbSQL.add_parameter(ParameterDirection.Input, "@img_ter", SqlDbType.VarBinary, 1, img)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 02/02/2021 METODO PARA SELECCIONAR 
    ''' </summary>
    ''' <param name="idgen"></param>
    ''' <returns></returns>
    Function seleccionar_generadores_systram_por_id(ByVal idgen As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_generadores_systram_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_gen", SqlDbType.Int, 11, idgen)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 
    ''' </summary>
    ''' <param name="idgen"></param>
    ''' <returns></returns>
    Function eliminar_generadores_systram(ByVal idgen As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_generadores_systram"
            dbSQL.add_parameter(ParameterDirection.Input, "@id_gen", SqlDbType.Int, 11, idgen)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 05/02/2021 METODO PARA SELECCIONAR GENERADORES SYSTRAM POR BUSQUEDA
    ''' </summary>
    ''' <param name="ind"></param>
    ''' <param name="par"></param>
    ''' <returns></returns>
    Function capturar_datos_generadores_systram_busqueda(ByVal ind As Integer, ByVal par As String, ByVal tip As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtter As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_generadores_systram_busqueda"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ind", SqlDbType.Int, 11, ind)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_par", SqlDbType.VarChar, 100, par)
            dbSQL.add_parameter(ParameterDirection.Input, "@tip_gen", SqlDbType.Int, 11, tip)

            dbSQL.query_sql(dtter)
        Catch ex As Exception

        End Try

        Return dtter

    End Function

    ''' <summary>
    ''' LGUF 07/04/2021 METODO PARA GUARDAR LOS DATOS DE LOS CERTIFICADOS
    ''' </summary>
    ''' <param name="idtic"></param>
    ''' <param name="idemo"></param>
    ''' <param name="perda"></param>
    ''' <param name="docda"></param>
    ''' <param name="nomda"></param>
    ''' <param name="codin"></param>
    ''' <param name="valda"></param>
    ''' <returns></returns>
    Function guardar_datos_certificados(ByVal idtic As Integer, ByVal idemo As Integer, ByVal perda As Integer, ByVal bimda As Integer, ByVal docda As String, ByVal nomda As String,
                                        ByVal codin As String, ByVal valda As Decimal, ByVal valba As Decimal, ByVal conex As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_datos_certificados"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tic", SqlDbType.Int, 11, idtic)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, idemo)
            dbSQL.add_parameter(ParameterDirection.Input, "@per_dac", SqlDbType.Int, 11, perda)
            dbSQL.add_parameter(ParameterDirection.Input, "@bim_dac", SqlDbType.Int, 11, bimda)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_dac", SqlDbType.VarChar, 50, docda)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_dac", SqlDbType.VarChar, 200, nomda)
            dbSQL.add_parameter(ParameterDirection.Input, "@cod_inc", SqlDbType.VarChar, 50, codin)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_dac", SqlDbType.Decimal, 18, valda)
            dbSQL.add_parameter(ParameterDirection.Input, "@val_bas", SqlDbType.Decimal, 18, valba)
            dbSQL.add_parameter(ParameterDirection.Input, "@con_exc", SqlDbType.Int, 11, conex)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 07/04/2021 METODO PARA SELECCIONAR DATOS DE CERTIFICADOS
    ''' </summary>
    ''' <param name="itc"></param>
    ''' <param name="ieo"></param>
    ''' <param name="per"></param>
    ''' <returns></returns>
    Function seleccionar_datos_certificados(ByVal itc As Integer, ByVal ieo As Integer, ByVal per As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_datos_certificados"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tic", SqlDbType.Int, 11, itc)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.VarChar, 100, ieo)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pdc", SqlDbType.Int, 11, per)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 19/04/2021 METODO PARA MOSTRAR CERTIFICADOS POR EMPRESA Y PERIODO
    ''' </summary>
    ''' <param name="ieo"></param>
    ''' <param name="per"></param>
    ''' <returns></returns>
    Function seleccionar_datos_certificados_salvar(ByVal ieo As Integer, ByVal per As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_datos_certificados_salvar"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.VarChar, 100, ieo)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pdc", SqlDbType.Int, 11, per)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 19/04/2021 METODO PARA MOSTRAR LOG CERTIFICADOS POR EMPRESA Y PERIODO
    ''' </summary>
    ''' <param name="ieo"></param>
    ''' <param name="per"></param>
    ''' <returns></returns>
    Function seleccionar_log_datos_certificados_salvar(ByVal ieo As Integer, ByVal per As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_log_seleccionar_datos_certificados_salvar"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.VarChar, 100, ieo)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pdc", SqlDbType.Int, 11, per)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 08/04/2021 METODO PARA SELECCIONAR DATOS CERTIFICADO POR DOCUMENTO
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function seleccionar_datos_certificados_por_documento(ByVal doc As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_certificados_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_dac", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 08/04/221 METODO PARA SELECCIONAR DATOS CERTIFICADOS POR TIPO, PERIODO Y DOCUMENTO
    ''' </summary>
    ''' <param name="tip"></param>
    ''' <param name="per"></param>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Function seleccionar_datos_certificados_por_documento_tipo_periodo(ByVal tip As Integer, ByVal per As Integer, ByVal doc As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_datos_certificados_por_documento"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tic", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pdc", SqlDbType.Int, 11, per)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_dac", SqlDbType.VarChar, 50, doc)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 13/04/2021 METODO PARA SELECCIONAR INFORMACION POR DOCUMENTO TIPO EMPRESA PERIODO
    ''' </summary>
    ''' <param name="tip"></param>
    ''' <param name="per"></param>
    ''' <param name="doc"></param>
    ''' <param name="emo"></param>
    ''' <returns></returns>
    Function seleccionar_datos_certificados_por_documento_tipo_periodo_empresa(ByVal tip As Integer, ByVal per As Integer, ByVal emo As Integer, ByVal doc As Integer, ByVal bim As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtcer As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_datos_certificados_por_documento_tipo_empresa_periodo"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_tic", SqlDbType.Int, 11, tip)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_pdc", SqlDbType.Int, 11, per)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_emo", SqlDbType.Int, 11, emo)
            dbSQL.add_parameter(ParameterDirection.Input, "@doc_dac", SqlDbType.VarChar, 50, doc)
            dbSQL.add_parameter(ParameterDirection.Input, "@bim_pdc", SqlDbType.Int, 11, bim)

            dbSQL.query_sql(dtcer)
        Catch ex As Exception

        End Try

        Return dtcer

    End Function

    ''' <summary>
    ''' LGUF 23/11/2021 METODO PARA SELECCIONAR VENCIMIENTO DOCUMENTOS COMPLETO
    ''' </summary>
    ''' <returns></returns>
    Function seleccionar_vencimiento_documentos_completo() As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtven As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_vencimiento_documentos_completo"

            dbSQL.query_sql(dtven)
        Catch ex As Exception

        End Try

        Return dtven
    End Function

    ''' <summary>
    ''' LGUF 24/11/2021 METODO PARA GUARDAR VENCIMIENTO DOCUMENTOS
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <param name="nom"></param>
    ''' <param name="fee"></param>
    ''' <param name="fev"></param>
    ''' <param name="dia"></param>
    ''' <param name="cor"></param>
    ''' <param name="cel"></param>
    ''' <param name="obs"></param>
    ''' <param name="usu"></param>
    ''' <param name="act"></param>
    ''' <returns></returns>
    Function guardar_vencimiento_documentos(ByVal ide As Integer, ByVal nom As String, ByVal fee As String, ByVal fev As String, ByVal dia As Integer,
                                            ByVal cor As String, ByVal cel As String, ByVal obs As String, ByVal usu As Integer, ByVal act As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_guardar_vencimiento_documentos"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ved", SqlDbType.Int, 11, ide)
            dbSQL.add_parameter(ParameterDirection.Input, "@nom_ved", SqlDbType.VarChar, 1000, nom)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_exp", SqlDbType.VarChar, 15, fee)
            dbSQL.add_parameter(ParameterDirection.Input, "@fec_ven", SqlDbType.VarChar, 15, fev)
            dbSQL.add_parameter(ParameterDirection.Input, "@dia_ved", SqlDbType.Int, 11, dia)
            dbSQL.add_parameter(ParameterDirection.Input, "@cor_ved", SqlDbType.VarChar, 2000, cor)
            dbSQL.add_parameter(ParameterDirection.Input, "@cel_ved", SqlDbType.VarChar, 2000, cel)
            dbSQL.add_parameter(ParameterDirection.Input, "@obs_ved", SqlDbType.VarChar, 3000, obs)
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_usu", SqlDbType.Int, 11, usu)
            dbSQL.add_parameter(ParameterDirection.Input, "@act_ved", SqlDbType.Bit, 1, act)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function

    ''' <summary>
    ''' LGUF 25/11/2021 METODO PARA SELECCIONAR VENCIMIENTOS DOCUMENTOS POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function seleccionar_vencimiento_documentos_por_id(ByVal ide As Integer) As DataTable

        Dim dbSQL As New databaseSQL
        Dim dtven As New DataTable
        Try
            dbSQL.str_storedprocedure = "sp_seleccionar_vencimiento_documentos_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ved", SqlDbType.Int, 11, ide)

            dbSQL.query_sql(dtven)
        Catch ex As Exception

        End Try

        Return dtven
    End Function

    ''' <summary>
    ''' LGUF 25/11/2021 METODO PARA ELIMINAR VENCIMIENTOS DOCUMENTOS POR ID
    ''' </summary>
    ''' <param name="ide"></param>
    ''' <returns></returns>
    Function eliminar_vencimiento_documentos_por_id(ByVal ide As Integer) As String

        Dim dbSQL As New databaseSQL
        Dim strRespuesta As String
        strRespuesta = ""
        Try
            dbSQL.str_storedprocedure = "sp_eliminar_vencimiento_documentos_por_id"
            dbSQL.add_parameter(ParameterDirection.Input, "@ide_ved", SqlDbType.Int, 11, ide)

            strRespuesta = dbSQL.execute_sql
        Catch ex As Exception
            strRespuesta = ex.Message
        End Try

        Return strRespuesta
    End Function
End Class
