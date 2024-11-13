Imports Microsoft.VisualBasic
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient

Imports System.IO

Imports System.Data
Imports System.Xml

Public Class database

#Region "ATRIBUTOS"

    'VARIABLES
    Private _connstring As String = ConfigurationManager.ConnectionStrings("systramconnectionstringtamos").ConnectionString
    Private _connstringtar As String = ConfigurationManager.ConnectionStrings("systramconnectionstringtar").ConnectionString
    Private _connstringrefri As String = ConfigurationManager.ConnectionStrings("systramconnectionstringrefri").ConnectionString
    Private _cachedatab As String = ConfigurationManager.AppSettings("cachebd").ToString

    Private _error As String
    Private _connected As Boolean
    Private _combovalue As String
    Private _combotext As String
    Private _storedprocedure As String

    'OBJETOS
    Private _command As MySqlCommand
    Private _connection As MySqlConnection
    Private _reader As MySqlDataReader
    Private _adapter As MySqlDataAdapter
    Private _parameter As MySqlParameter
    Private _transaction As MySqlTransaction
    Private _table As DataTable
    Private _set As DataSet

    'CONTROLES
    Private _combo As DropDownList
    Private _grid As GridView

    Private _ctrparameter As String

#End Region

#Region "PROPIEDADES"

    'VARIABLES
    Public ReadOnly Property str_connstring() As String
        Get
            If ConfigurationManager.AppSettings("bdsel").ToString = 1 Then
                Me._connstring = Me._connstring
            Else
                If ConfigurationManager.AppSettings("bdsel").ToString = 2 Then
                    Me._connstring = Me._connstringtar
                Else
                    Me._connstring = Me._connstringrefri
                End If
            End If

            Return Me._connstring
        End Get
    End Property

    Public Property str_error() As String
        Get
            Return Me._error
        End Get
        Set(ByVal value As String)
            Me._error = value
        End Set
    End Property

    Public Property bool_connected() As Boolean
        Get
            Return Me._connected
        End Get
        Set(ByVal value As Boolean)
            Me._connected = value
        End Set
    End Property

    Public Property str_combovalue() As String
        Get
            Return Me._combovalue
        End Get
        Set(ByVal value As String)
            Me._combovalue = value
        End Set
    End Property

    Public Property str_combotext() As String
        Get
            Return Me._combotext
        End Get
        Set(ByVal value As String)
            Me._combotext = value
        End Set
    End Property

    Public Property str_storedprocedure() As String
        Get
            Return Me._storedprocedure
        End Get
        Set(ByVal value As String)
            Me._storedprocedure = value
        End Set
    End Property


    'OBJETOS
    Public Property obj_command() As MySqlCommand
        Get
            Return Me._command
        End Get
        Set(ByVal value As MySqlCommand)
            Me._command = value
        End Set
    End Property

    Public Property obj_connection() As MySqlConnection
        Get
            Return Me._connection
        End Get
        Set(ByVal value As MySqlConnection)
            Me._connection = value
        End Set
    End Property

    Public Property obj_reader() As MySqlDataReader
        Get
            Return Me._reader
        End Get
        Set(ByVal value As MySqlDataReader)
            Me._reader = value
        End Set
    End Property

    Public Property obj_adapter() As MySqlDataAdapter
        Get
            Return Me._adapter
        End Get
        Set(ByVal value As MySqlDataAdapter)
            Me._adapter = value
        End Set
    End Property

    Public Property obj_parameter() As MySqlParameter
        Get
            Return Me._parameter
        End Get
        Set(ByVal value As MySqlParameter)
            Me._parameter = value
        End Set
    End Property

    Public Property obj_transaction() As MySqlTransaction
        Get
            Return Me._transaction
        End Get
        Set(ByVal value As MySqlTransaction)
            Me._transaction = value
        End Set
    End Property

    Public Property obj_table() As DataTable
        Get
            Return Me._table
        End Get
        Set(ByVal value As DataTable)
            Me._table = value
        End Set
    End Property

    Public Property obj_set() As DataSet
        Get
            Return Me._set
        End Get
        Set(ByVal value As DataSet)
            Me._set = value
        End Set
    End Property


    'CONTROLES
    Public Property ctrl_combo() As DropDownList
        Get
            Return Me._combo
        End Get
        Set(ByVal value As DropDownList)
            Me._combo = value
        End Set
    End Property

    Public Property ctrl_grid() As GridView
        Get
            Return Me._grid
        End Get
        Set(ByVal value As GridView)
            Me._grid = value
        End Set
    End Property

#End Region

#Region "CONSTRUCTORES"

    Public Sub New()

        _command = New MySqlCommand
        _connection = New MySqlConnection
        _adapter = New MySqlDataAdapter
        _parameter = New MySqlParameter
        _ctrparameter = ""

    End Sub

#End Region

#Region "PRIVADOS"

    Private Function open_connection() As String

        _connection.ConnectionString = str_connstring
        Try
            _connection.Open()
            bool_connected = True
            Return ""
        Catch ex As Exception
            bool_connected = False
            Return "No pudo establecerse la Conexión a la Base de Datos. Consultar con el Administrador del Sistema. Error: " & ex.Message
        End Try

    End Function

    Private Function close_connection() As String

        Try

            _connection.Dispose()
            _connection.Close()
            bool_connected = False
            Return ""

        Catch ex As Exception
            Return "La conexión no pudo cerrarse. Error: " & ex.Message
        End Try

    End Function

#End Region

#Region "PUBLICOS"

    Sub setStoredProcedureName(ByVal nomstoredProcedure As String)

        str_storedprocedure = nomstoredProcedure
        'Crear archivo, si no existe, del procedimiento almacenado para quede haga el papel de cache

        Dim path = System.AppDomain.CurrentDomain.BaseDirectory() & "/cache/sp/" & nomstoredProcedure & ".xml"

        If Not System.IO.File.Exists(path) Then
            cache_xml_write(cache_xml_dtschema(nomstoredProcedure), path)
        End If

        cache_xml_read(path)

    End Sub

    Function cache_xml_dtschema(ByVal nomstoredProcedure As String) As DataTable
        Dim sql = "SELECT * FROM information_schema.parameters WHERE SPECIFIC_NAME = '" & nomstoredProcedure & "' AND SPECIFIC_SCHEMA = '" & _cachedatab & "' ;"
        Dim dt As New DataTable
        Dim oconn = New MySqlConnection(_connstring)
        Dim ocomm = New MySqlCommand(sql, oconn)
        Dim adap = New MySqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Function cache_xml_write(ByVal dt As DataTable, ByVal path As String) As String

        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True

        Using writer As XmlWriter = XmlWriter.Create(path, settings)

            writer.WriteStartDocument()
            writer.WriteStartElement("StoredProcedure")

            For i As Integer = 0 To dt.Rows.Count - 1

                writer.WriteStartElement("Parameter")
                writer.WriteElementString("POSITION", dt.Rows(i).Item("ORDINAL_POSITION"))
                writer.WriteElementString("MODE", dt.Rows(i).Item("PARAMETER_MODE"))
                writer.WriteElementString("NAME", dt.Rows(i).Item("PARAMETER_NAME"))
                writer.WriteElementString("TYPE", dt.Rows(i).Item("DATA_TYPE"))
                writer.WriteElementString("LENGTH", dt.Rows(i).Item("CHARACTER_MAXIMUM_LENGTH").ToString)

                writer.WriteEndElement()

            Next

            writer.WriteEndElement()
            writer.WriteEndDocument()

        End Using

    End Function


    Dim arrayList(0) As String
    Dim _dscache As DataSet

    Sub cache_xml_read(ByVal path As String)

        _dscache = New DataSet

        _dscache.ReadXml(path)
        If _dscache.Tables.Count > 0 Then
            ReDim arrayList(_dscache.Tables(0).Rows.Count - 1)
        Else
            ReDim arrayList(0)
        End If

    End Sub

    Public Function add_parameter( _
                                    ByVal direction As System.Data.ParameterDirection, _
                                    ByVal name As String, _
                                    ByVal type As MySqlDbType, _
                                    ByVal size As Integer, _
                                    ByVal value As String) As String

        Try

            Dim index As Integer, mode As String, daty As String
            For i As Integer = 0 To _dscache.Tables(0).Rows.Count - 1
                index = Convert.ToInt32(_dscache.Tables(0).Rows(i).Item("POSITION"))
                mode = _dscache.Tables(0).Rows(i).Item("MODE")
                daty = _dscache.Tables(0).Rows(i).Item("TYPE")

                If name = _dscache.Tables(0).Rows(i).Item("NAME") Then
                    If mode = "INOUT" Or mode = "OUT" Then
                        arrayList(index - 1) = "@"
                    Else

                        If (daty = "int" Or daty = "double" Or daty = "bigint" Or daty = "tinyint") Then
                            If value = Nothing Then
                                arrayList(index - 1) = "NULL"
                            Else
                                Dim csop As New Operaciones
                                arrayList(index - 1) = csop.format_campo_num(value)
                            End If
                        ElseIf daty = "decimal" Then
                            If value = Nothing Then
                                arrayList(index - 1) = "NULL"
                            Else
                                Dim csop As New Operaciones
                                arrayList(index - 1) = "'" & Replace(value, ",", ".") & "'"
                            End If
                        ElseIf daty = "date" Then
                            If value = Nothing Then
                                Dim csop As New Operaciones
                                arrayList(index - 1) = "NULL"
                            Else
                                Dim fecha As DateTime = value
                                arrayList(index - 1) = "'" & fecha.ToString("yyyy-MM-dd") & "'"
                            End If
                        ElseIf daty = "time" Then
                            If value = Nothing Then
                                Dim csop As New Operaciones
                                arrayList(index - 1) = csop.horaActual
                            Else
                                arrayList(index - 1) = "'" & value & "'"
                            End If
                        Else
                            If value = Nothing Then
                                arrayList(index - 1) = "''"
                            Else
                                arrayList(index - 1) = "'" & value.ToString & "'"
                            End If
                        End If

                    End If

                    Exit For
                End If
            Next

            'obj_parameter.Direction = direction
            'obj_parameter.ParameterName = name
            'obj_parameter.MySqlDbType = type
            'obj_parameter.Size = size
            'obj_parameter.Value = value

            'obj_command.Parameters.Add(obj_parameter)
            'obj_parameter = New MySqlParameter

            'str_error = ""

        Catch ex As Exception
            str_error = "Error al ingresar parámetro. Error: " & ex.Message
        End Try

        Return str_error

    End Function

    'Public Function add_parameter( _
    '                                ByVal direction As System.Data.ParameterDirection, _
    '                                ByVal name As String, _
    '                                ByVal type As MySqlDbType, _
    '                                ByVal size As Integer, _
    '                                ByVal value As Object) As String

    '    Try

    '        obj_parameter.Direction = direction
    '        obj_parameter.ParameterName = name
    '        obj_parameter.MySqlDbType = type
    '        obj_parameter.Size = size
    '        obj_parameter.Value = value

    '        obj_command.Parameters.Add(obj_parameter)
    '        obj_parameter = New MySqlParameter

    '        str_error = ""

    '    Catch ex As Exception
    '        str_error = "Error al ingresar parámetro. Error: " & ex.Message
    '    End Try

    '    Return str_error

    'End Function

    'Public Function capture_output(ByVal text As String) As String

    '    Return obj_command.Parameters(text).Value

    'End Function

    Public Function execute_sql() As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            begin_transaction()

            obj_command.Connection = obj_connection
            obj_command.CommandType = CommandType.Text
            obj_command.CommandText = "CALL " & str_storedprocedure & "("

            If Not arrayList(0) Is Nothing Then
                For i As Integer = 0 To arrayList.Length - 1
                    obj_command.CommandText &= arrayList(i).ToString()
                    If i <> arrayList.Length - 1 Then obj_command.CommandText &= ","
                Next
            End If

            obj_command.CommandText &= ");"
            obj_command.ExecuteNonQuery()
            str_error = ""

            commit_transaction()

        Catch ex As Exception

            rollback_transaction()
            str_error = "El proceso no pudo ejecutarse. Error: " & ex.Message
            'csop.log_Errores(ex.Message)

        Finally

            bool_connected = False
            obj_connection.Dispose()
            obj_connection.Close()

        End Try

        Return str_error

    End Function

    'Public Function execute_sql() As String

    '    Try

    '        If bool_connected = False Then

    '            Dim opening_connection = open_connection()
    '            If opening_connection <> "" Then
    '                Return opening_connection
    '            End If

    '        End If

    '        begin_transaction()

    '        obj_command.Connection = obj_connection
    '        obj_command.CommandType = CommandType.StoredProcedure
    '        obj_command.CommandText = str_storedprocedure
    '        obj_command.ExecuteNonQuery()
    '        str_error = ""

    '        commit_transaction()

    '    Catch ex As Exception

    '        rollback_transaction()
    '        str_error = "El proceso no pudo ejecutarse. Error: " & ex.Message
    '        'csop.log_Errores(ex.Message)

    '    Finally

    '        bool_connected = False
    '        obj_connection.Dispose()
    '        obj_connection.Close()

    '    End Try

    '    Return str_error

    'End Function

    Public Function query_sql(ByVal aux_table As DataTable) As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            obj_command.Connection = obj_connection
            obj_command.CommandType = CommandType.Text
            obj_command.CommandText = "CALL " & str_storedprocedure & "("

            If Not arrayList(0) Is Nothing Then
                For i As Integer = 0 To arrayList.Length - 1
                    obj_command.CommandText &= arrayList(i).ToString()
                    If i <> arrayList.Length - 1 Then obj_command.CommandText &= ","
                Next
            End If

            obj_command.CommandText &= ");"

            obj_adapter.SelectCommand = obj_command
            obj_adapter.Fill(aux_table)

            _dscache = Nothing
            arrayList = Nothing

            str_error = ""

        Catch ex As Exception
            str_error = "No se pudo realizar la Consulta. Error: " & ex.Message
            'csop.log_Errores(ex.Message)
        Finally

            bool_connected = False
            obj_connection.Dispose()
            obj_connection.Close()

        End Try

        Return str_error

    End Function

    Public Function query_sql_set(ByVal aux_table As DataSet) As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            obj_command.Connection = obj_connection
            obj_command.CommandType = CommandType.Text
            obj_command.CommandText = "CALL " & str_storedprocedure & "("

            If Not arrayList(0) Is Nothing Then
                For i As Integer = 0 To arrayList.Length - 1
                    obj_command.CommandText &= arrayList(i).ToString()
                    If i <> arrayList.Length - 1 Then obj_command.CommandText &= ","
                Next
            End If

            obj_command.CommandText &= ");"

            obj_adapter.SelectCommand = obj_command
            obj_adapter.Fill(aux_table)

            _dscache = Nothing
            arrayList = Nothing

            str_error = ""

        Catch ex As Exception
            str_error = "No se pudo realizar la Consulta. Error: " & ex.Message
            'csop.log_Errores(ex.Message)
        Finally

            bool_connected = False
            obj_connection.Dispose()
            obj_connection.Close()

        End Try

        Return str_error

    End Function

    Public Function begin_transaction() As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            obj_transaction = _connection.BeginTransaction
            obj_command.Transaction = obj_transaction
            str_error = ""

        Catch ex As Exception
            str_error = "Operación iniciar transaccion fallida. Error: " & ex.Message
            'csop.log_Errores(ex.Message)
        End Try

        Return str_error

    End Function

    Public Function commit_transaction() As String

        Try

            obj_transaction.Commit()
            str_error = ""

        Catch ex_try As Exception

            Try

                str_error = rollback_transaction()

            Catch ex_catch As Exception

                str_error = "La transacción no se realizo, ni tampoco deshizo los cambios. Consulte con el administrador de la aplicación. Error: " & ex_catch.Message
                'csop.log_Errores(str_error)

            End Try

        Finally

            bool_connected = False
            obj_connection.Dispose()
            obj_connection.Close()

        End Try

        Return str_error

    End Function

    Public Function rollback_transaction() As String

        Try

            obj_transaction.Rollback()
            str_error = ""

        Catch ex As Exception
            str_error = "No se deshizo los cambios. Error: " & ex.Message
            'csop.log_Errores(str_error)
        Finally

            bool_connected = False
            obj_connection.Dispose()
            obj_connection.Close()

        End Try

        Return str_error

    End Function

#End Region

End Class

Public Class databaseSQL

#Region "ATRIBUTOS"

    'VARIABLES
    Private _connstring As String = ConfigurationManager.ConnectionStrings("systramconnectionstringSQL").ConnectionString
    Private _connstringMotoseg As String = ConfigurationManager.ConnectionStrings("systramconnectionstringMotosegSQL").ConnectionString
    Private _connstringCia As String = ConfigurationManager.ConnectionStrings("systramconnectionstringCiaSQL").ConnectionString
    Private _connstringTraSeg As String = ConfigurationManager.ConnectionStrings("systramconnectionstringTraSegSQL").ConnectionString
    Private _connstringTraLin As String = ConfigurationManager.ConnectionStrings("systramconnectionstringTraLinSQL").ConnectionString
    Private _connstringLabor As String = ConfigurationManager.ConnectionStrings("systramconnectionstringLaborSQL").ConnectionString
    Private _error As String
    Private _connected As Boolean
    Private _combovalue As String
    Private _combotext As String
    Private _storedprocedure As String

    'OBJETOS
    Private _command As SqlCommand
    Private _connection As SqlConnection
    Private _reader As SqlDataReader
    Private _adapter As SqlDataAdapter
    Private _parameter As SqlParameter
    Private _transaction As SqlTransaction
    Private _table As DataTable
    Private _set As DataSet

    'CONTROLES
    Private _combo As DropDownList
    Private _grid As GridView


#End Region

#Region "PROPIEDADES"

    'VARIABLES
    Public ReadOnly Property str_connstring() As String
        Get
            Return Me._connstring
        End Get
    End Property

    Public ReadOnly Property str_connstringMotoseg() As String
        Get
            Return Me._connstringMotoseg
        End Get
    End Property

    Public ReadOnly Property str_connstringCia() As String
        Get
            Return Me._connstringCia
        End Get
    End Property

    Public ReadOnly Property str_connstringTraSeg() As String
        Get
            Return Me._connstringTraSeg
        End Get
    End Property

    Public ReadOnly Property str_connstringTraLin() As String
        Get
            Return Me._connstringTraLin
        End Get
    End Property

    Public Property str_error() As String
        Get
            Return Me._error
        End Get
        Set(ByVal value As String)
            Me._error = value
        End Set
    End Property

    Public Property bool_connected() As Boolean
        Get
            Return Me._connected
        End Get
        Set(ByVal value As Boolean)
            Me._connected = value
        End Set
    End Property

    Public Property str_combovalue() As String
        Get
            Return Me._combovalue
        End Get
        Set(ByVal value As String)
            Me._combovalue = value
        End Set
    End Property

    Public Property str_combotext() As String
        Get
            Return Me._combotext
        End Get
        Set(ByVal value As String)
            Me._combotext = value
        End Set
    End Property

    Public Property str_storedprocedure() As String
        Get
            Return Me._storedprocedure
        End Get
        Set(ByVal value As String)
            Me._storedprocedure = value
        End Set
    End Property


    'OBJETOS
    Public Property obj_command() As SqlCommand
        Get
            Return Me._command
        End Get
        Set(ByVal value As SqlCommand)
            Me._command = value
        End Set
    End Property

    Public Property obj_connection() As SqlConnection
        Get
            Return Me._connection
        End Get
        Set(ByVal value As SqlConnection)
            Me._connection = value
        End Set
    End Property

    Public Property obj_reader() As SqlDataReader
        Get
            Return Me._reader
        End Get
        Set(ByVal value As SqlDataReader)
            Me._reader = value
        End Set
    End Property

    Public Property obj_adapter() As SqlDataAdapter
        Get
            Return Me._adapter
        End Get
        Set(ByVal value As SqlDataAdapter)
            Me._adapter = value
        End Set
    End Property

    Public Property obj_parameter() As SqlParameter
        Get
            Return Me._parameter
        End Get
        Set(ByVal value As SqlParameter)
            Me._parameter = value
        End Set
    End Property

    Public Property obj_transaction() As SqlTransaction
        Get
            Return Me._transaction
        End Get
        Set(ByVal value As SqlTransaction)
            Me._transaction = value
        End Set
    End Property

    Public Property obj_table() As DataTable
        Get
            Return Me._table
        End Get
        Set(ByVal value As DataTable)
            Me._table = value
        End Set
    End Property

    Public Property obj_set() As DataSet
        Get
            Return Me._set
        End Get
        Set(ByVal value As DataSet)
            Me._set = value
        End Set
    End Property


    'CONTROLES
    Public Property ctrl_combo() As DropDownList
        Get
            Return Me._combo
        End Get
        Set(ByVal value As DropDownList)
            Me._combo = value
        End Set
    End Property

    Public Property ctrl_grid() As GridView
        Get
            Return Me._grid
        End Get
        Set(ByVal value As GridView)
            Me._grid = value
        End Set
    End Property

#End Region

#Region "CONSTRUCTORES"

    Public Sub New()

        _command = New SqlCommand
        _connection = New SqlConnection
        _adapter = New SqlDataAdapter
        _parameter = New SqlParameter

    End Sub

#End Region

#Region "PRIVADOS"

    Private Function open_connection() As String

        _connection.ConnectionString = str_connstring
        Try
            _connection.Open()
            bool_connected = True
            Return ""
        Catch ex As Exception
            bool_connected = False
            Return "No pudo establecerse la Conexión a la Base de Datos. Consultar con el Administrador del Sistema. Error: " & ex.Message
        End Try

    End Function

    Private Function close_connection() As String

        Try

            _connection.Dispose()
            _connection.Close()
            bool_connected = False
            Return ""

        Catch ex As Exception
            Return "La conexión no pudo cerrarse. Error: " & ex.Message
        End Try

    End Function

#End Region

#Region "PUBLICOS"

    Public Function add_parameter( _
                                    ByVal direction As System.Data.ParameterDirection, _
                                    ByVal name As String, _
                                    ByVal type As SqlDbType, _
                                    ByVal size As Integer, _
                                    ByVal value As Object) As String

        Try

            obj_parameter.Direction = direction
            obj_parameter.ParameterName = name
            obj_parameter.SqlDbType = type
            obj_parameter.Size = size
            obj_parameter.Value = value

            obj_command.Parameters.Add(obj_parameter)
            obj_parameter = New SqlParameter

            str_error = ""

        Catch ex As Exception
            str_error = "Error al ingresar parámetro. Error: " & ex.Message
        End Try

        Return str_error

    End Function

    'Public Function capture_output(ByVal text As String) As String

    '    Return obj_command.Parameters(text).Value

    'End Function

    Public Function execute_sql() As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            begin_transaction()

            obj_command.Connection = obj_connection
            obj_command.CommandType = CommandType.StoredProcedure
            obj_command.CommandText = str_storedprocedure
            obj_command.ExecuteNonQuery()
            str_error = ""

            commit_transaction()

        Catch ex As Exception

            rollback_transaction()
            str_error = "El proceso no pudo ejecutarse. Error: " & ex.Message
            'csop.log_Errores(ex.Message)

        Finally

            bool_connected = False

            obj_connection.Close()
            obj_connection.Dispose()

        End Try

        Return str_error

    End Function

    Function exec_query(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        Dim oconn As New SqlConnection

        oconn = New SqlConnection(_connstringMotoseg)

        Dim ocomm = New SqlCommand(sql, oconn)
        Dim adap = New SqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Function exec_query_cia(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        Dim oconn As New SqlConnection

        oconn = New SqlConnection(_connstringCia)

        Dim ocomm = New SqlCommand(sql, oconn)
        Dim adap = New SqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Function exec_query_traseg(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        Dim oconn As New SqlConnection

        oconn = New SqlConnection(_connstringTraSeg)

        Dim ocomm = New SqlCommand(sql, oconn)
        Dim adap = New SqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Function exec_query_tralin(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        Dim oconn As New SqlConnection

        oconn = New SqlConnection(_connstringTraLin)

        Dim ocomm = New SqlCommand(sql, oconn)
        Dim adap = New SqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Function exec_query_labor(ByVal sql As String) As DataTable
        Dim dt As New DataTable
        Dim oconn As New SqlConnection

        oconn = New SqlConnection(_connstringLabor)

        Dim ocomm = New SqlCommand(sql, oconn)
        Dim adap = New SqlDataAdapter(ocomm)
        adap.Fill(dt)
        Return dt

    End Function

    Public Function query_sql(ByVal aux_table As DataTable) As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            obj_command.Connection = obj_connection
            obj_command.CommandType = CommandType.StoredProcedure
            obj_command.CommandText = str_storedprocedure

            obj_adapter.SelectCommand = obj_command
            obj_adapter.Fill(aux_table)

            str_error = ""

        Catch ex As Exception
            str_error = "No se pudo realizar la Consulta. Error: " & ex.Message
            'csop.log_Errores(ex.Message)
        Finally

            bool_connected = False
            obj_connection.Close()
            obj_connection.Dispose()

        End Try

        Return str_error

    End Function

    Public Function begin_transaction() As String

        Try

            If bool_connected = False Then

                Dim opening_connection = open_connection()
                If opening_connection <> "" Then
                    Return opening_connection
                End If

            End If

            obj_transaction = _connection.BeginTransaction
            obj_command.Transaction = obj_transaction
            str_error = ""

        Catch ex As Exception
            str_error = "Operación iniciar transaccion fallida. Error: " & ex.Message
            'csop.log_Errores(ex.Message)
        End Try

        Return str_error

    End Function

    Public Function commit_transaction() As String

        Try

            obj_transaction.Commit()
            str_error = ""

        Catch ex_try As Exception

            Try

                str_error = rollback_transaction()

            Catch ex_catch As Exception

                str_error = "La transacción no se realizo, ni tampoco deshizo los cambios. Consulte con el administrador de la aplicación. Error: " & ex_catch.Message
                'csop.log_Errores(str_error)

            End Try

        Finally

            bool_connected = False
            obj_connection.Close()
            obj_connection.Dispose()

        End Try

        Return str_error

    End Function

    Public Function rollback_transaction() As String

        Try

            obj_transaction.Rollback()
            str_error = ""

        Catch ex As Exception
            str_error = "No se deshizo los cambios. Error: " & ex.Message
            'csop.log_Errores(str_error)
        Finally

            bool_connected = False
            obj_connection.Close()
            obj_connection.Dispose()

        End Try

        Return str_error

    End Function

#End Region

End Class
