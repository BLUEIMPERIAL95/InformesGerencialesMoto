Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

Public Class database_sql
    'Public Variables
    Public Property LastError() As String

    'private variables
    Private cn As New SqlConnection
    Private isOpened As Boolean
    Private dsData As New DataSet
    Private myds(,) As Object
    Private myColumnIndex() As String

    ''' <summary>
    ''' This constructor  set the  working mode based on configuration value.
    ''' Default value of working mode is Normal
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        LastError = ""
        isOpened = False
    End Sub

    ''' <summary>
    ''' Public function to Open Database Connection
    ''' </summary>
    ''' <param name="ConnectionString">Connection String</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function OpenDatabase(ByVal ConnectionString As String) As Boolean
        LastError = ""
        OpenDatabase = False
        If isOpened Then
            'AlarmManager.Log("Previously opened database", AlarmManager.AlarmType.Fatal)
            Exit Function
        End If
        If ConnectionString = "" Then
            'AlarmManager.Log("Invalid connection string", AlarmManager.AlarmType.Security)
            Exit Function
        End If
        Try
            cn.ConnectionString = ConnectionString
            cn.Open()
            OpenDatabase = True
            isOpened = True
            'AlarmManager.Log("Database opened", AlarmManager.AlarmType.Informative)
        Catch ex As Exception
            LastError = ex.Message
            'AlarmManager.Log("Error opening database:" + ex.Message, AlarmManager.AlarmType.Security)
        End Try
    End Function

    ''' <summary>
    ''' Public function to close an opened database
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CloseDatabase() As Boolean
        LastError = ""
        CloseDatabase = False
        If Not isOpened Then
            'AlarmManager.Log("Not previously opened database", AlarmManager.AlarmType.Fatal)
            Exit Function
        End If
        Try
            cn.Close()
            isOpened = False
            CloseDatabase = True
            'AlarmManager.Log("Database closed", AlarmManager.AlarmType.Informative)
        Catch ex As Exception
            LastError = ex.Message
            'AlarmManager.Log("Error closing database:" + ex.Message, AlarmManager.AlarmType.Fatal)
        End Try
    End Function

    ''' <summary>
    ''' Public function to call an stored procedure that returns a result set of data.
    ''' Timeout value is 5 minutes.
    ''' If function succeds it returns the number of rows.
    ''' If not data is returned the function return 0 value.
    ''' Any error during this function return a -999 value.
    ''' For debuging porpouses the time taken between function start and end is logged.
    ''' Data is stored not only in a dataset but also in a matrix.
    ''' After you use this method you can retrieve the returned data using DataResults function
    ''' </summary>
    ''' <param name="SQL">Stored procedure name</param>
    ''' <param name="ArrayParameters">Array parameters of the stored procedure to be called</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>

    Public Function QueryDatabase(ByVal SQL As String, ByRef ArrayParameters() As System.Data.SqlClient.SqlParameter) As Long
        Dim i As Integer
        Dim reader1 As System.Data.SqlClient.SqlDataReader
        Dim read1 As Boolean
        Dim command1 As New System.Data.SqlClient.SqlCommand
        LastError = ""
        command1.CommandTimeout = 5 * 60
        QueryDatabase = -999
        read1 = False
        reader1 = Nothing
        If Not isOpened Then
            'AlarmManager.Log("Not previously opened database", AlarmManager.AlarmType.Fatal)
            Exit Function
        Else
            Try
                dsData.Clear()
                dsData.Tables.Clear()
                dsData.Tables.Add(New DataTable)
                dsData.EnforceConstraints = False
                command1.Connection = cn
                'input
                If Not ArrayParameters Is Nothing Then
                    command1.Parameters.AddRange(ArrayParameters)
                End If
                command1.CommandText = SQL
                command1.CommandType = CommandType.StoredProcedure
                reader1 = command1.ExecuteReader
                read1 = True
                dsData.Tables(0).Load(reader1)
                'output
                If Not ArrayParameters Is Nothing Then
                    For i = 1 To ArrayParameters.Length
                        Dim p As New System.Data.SqlClient.SqlParameter
                        p = command1.Parameters.Item(i - 1)
                        ArrayParameters(i - 1) = p
                    Next
                End If
                'clean
                command1.Parameters.Clear()
                command1 = Nothing
                ConvertToMyds()
                'Remember that if you have set variable nocount=on 
                'it does not return the affected rows number
                QueryDatabase = dsData.Tables(0).Rows.Count
            Catch ex As Exception
                QueryDatabase = -999
                LastError = ex.Message
                'AlarmManager.Log("Error querying database:" + ex.Message, AlarmManager.AlarmType.Fatal)
                Try
                    command1.Parameters.Clear()
                    command1 = Nothing
                Catch ex2 As Exception

                End Try
            End Try
        End If
        If read1 Then
            reader1.Close()
            read1 = False
        End If
    End Function

    ''' <summary>
    ''' Public function To Execute stored procedures that not return any result set.
    ''' Timeout value is 5 minutes.
    ''' If function succeds it returns the number of rows.
    ''' If not data is returned the function return 0 value.
    ''' Any error during this function return a -999 value.
    ''' For debuging porpouses the time taken between function start and end is logged.
    ''' Data is stored not only in a dataset but also in a matrix.
    ''' </summary>
    ''' <param name="SQL">Stored procedure name</param>
    ''' <param name="ArrayParameters">Array parameters of the stored procedure to be called</param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function ExecuteNonQuery(ByVal SQL As String, ByRef ArrayParameters() As System.Data.SqlClient.SqlParameter) As Long
        Dim i As Integer
        Dim reader1 As System.Data.SqlClient.SqlDataReader
        Dim read1 As Boolean
        Dim command1 As New System.Data.SqlClient.SqlCommand
        LastError = ""
        ExecuteNonQuery = -999
        read1 = False
        command1.CommandTimeout = 5 * 60
        reader1 = Nothing
        If Not isOpened Then
            'AlarmManager.Log("Not previously opened database", AlarmManager.AlarmType.Fatal)
            Exit Function
        Else
            Try
                command1.Connection = cn
                'input
                If Not ArrayParameters Is Nothing Then
                    command1.Parameters.AddRange(ArrayParameters)
                End If
                command1.CommandText = SQL
                command1.CommandType = CommandType.StoredProcedure
                'Remember that if you have set variable nocount=on 
                'it does not return the affected rows number
                ExecuteNonQuery = command1.ExecuteNonQuery()
                If ExecuteNonQuery = -1 Then
                    ExecuteNonQuery = 0
                End If
                'output
                If Not ArrayParameters Is Nothing Then
                    For i = 1 To ArrayParameters.Length
                        Dim p As New System.Data.SqlClient.SqlParameter
                        p = command1.Parameters.Item(i - 1)
                        ArrayParameters(i - 1) = p
                    Next
                End If
                'clean
                command1.Parameters.Clear()
                command1 = Nothing
            Catch ex As Exception
                ExecuteNonQuery = -999
                LastError = ex.Message
                'AlarmManager.Log("Error executing nonquery database:" + ex.Message, AlarmManager.AlarmType.Fatal)
                Try
                    command1.Parameters.Clear()
                    command1 = Nothing
                Catch ex2 As Exception

                End Try
            End Try
        End If
        If read1 Then
            reader1.Close()
            read1 = False
        End If
    End Function


    ''' <summary>
    ''' Public method to get dataset of a previous QueryDatabase.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDataSet() As DataSet
        'AlarmManager.Log("GetDataSet ", AlarmManager.AlarmType.Informative)
        GetDataSet = dsData
    End Function

    ''' <summary>
    ''' Private Method to convert dataset into a matrix.
    ''' If any value returned from database is null it is assigned a nothing value.
    ''' It also populates the internal myColumnIndex HashTable
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ConvertToMyds()
        Dim i As Long
        Dim j As Integer
        Dim myRow As DataRow
        ReDim myds(dsData.Tables(0).Rows.Count - 1, dsData.Tables(0).Columns.Count - 1)
        For i = 0 To dsData.Tables(0).Rows.Count - 1
            myRow = dsData.Tables(0).Rows(i)
            For j = 0 To dsData.Tables(0).Columns.Count - 1
                If IsDBNull(myRow.Item(j)) Then
                    myds(i, j) = Nothing
                Else
                    myds(i, j) = myRow.Item(j)
                End If
            Next
        Next
        ReDim myColumnIndex(dsData.Tables(0).Columns.Count - 1)
        For j = 0 To dsData.Tables(0).Columns.Count - 1
            myColumnIndex(j) = dsData.Tables(0).Columns.Item(j).ColumnName.ToUpper.Trim
        Next
    End Sub

    ''' <summary>
    ''' Public Function to get row, column values of results of QueryDatabase.
    ''' Row and Column  are zero index.
    ''' Column can also be Field Name
    ''' </summary>
    ''' <param name="Row">Row (starting from 0)</param>
    ''' <param name="Col">Column (starting from 0) can also be Field Name</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DataResults(ByVal Row As Long, ByVal Col As Object) As Object
        Dim j As Integer
        Dim i As Integer
        Dim hasFound As Boolean
        If IsNumeric(Col) Then
            j = CInt(Col)
            If j > UBound(myColumnIndex, 1) Then
                'AlarmManager.Log("DataResults:invalid column index", AlarmManager.AlarmType.Fatal)
                Throw New Exception("invalid column index")
                Exit Function
            End If
        Else
            hasFound = False
            For i = 0 To UBound(myColumnIndex, 1)
                If myColumnIndex(i) = Col.ToString.ToUpper.Trim Then
                    j = i
                    hasFound = True
                    Exit For
                End If
            Next
            If Not hasFound Then
                'AlarmManager.Log("DataResults:invalid column index", AlarmManager.AlarmType.Fatal)
                Throw New Exception("invalid column index")
                Exit Function
            End If
        End If
        If Row > UBound(myds, 1) Then
            'AlarmManager.Log("DataResults:invalid row index", AlarmManager.AlarmType.Fatal)
            Throw New Exception("invalid row index")
            Exit Function
        End If
        DataResults = myds(Row, j)
    End Function
End Class
