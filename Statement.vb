Imports System.ComponentModel
Imports System.Data.Common
Imports Oracle.DataAccess.Client

Public Class Statement
    Inherits Databasic.Statement






    ''' <summary>
    ''' Currently prepared and executed Oracle command.
    ''' </summary>
    Public Overrides Property Command As DbCommand
        Get
            Return Me._cmd
        End Get
        Set(value As DbCommand)
            Me._cmd = value
        End Set
    End Property
    Private _cmd As OracleCommand
    ''' <summary>
    ''' Currently executed Oracle data reader from Oracle command.
    ''' </summary>
    Public Overrides Property Reader As DbDataReader
        Get
            Return Me._reader
        End Get
        Set(value As DbDataReader)
            Me._reader = value
        End Set
    End Property
    Private _reader As OracleDataReader






    ''' <summary>
    ''' Empty SQL statement constructor.
    ''' </summary>
    ''' <param name="sql">SQL statement code.</param>
    ''' <param name="connection">Connection instance.</param>
    Public Sub New(sql As String, connection As OracleConnection)
        MyBase.New(sql, connection)
        Me._cmd = New OracleCommand(sql, connection)
        Me._cmd.Prepare()
    End Sub
    ''' <summary>
    ''' Empty SQL statement constructor.
    ''' </summary>
    ''' <param name="sql">SQL statement code.</param>
    ''' <param name="transaction">SQL transaction instance with connection instance inside.</param>
    Public Sub New(sql As String, transaction As OracleTransaction)
        MyBase.New(sql, transaction)
        Me._cmd = New OracleCommand(sql, transaction.Connection)
        Me._cmd.Transaction = transaction
        Me._cmd.Prepare()
    End Sub





    ''' <summary>
    ''' Set up all sql params into internal Command instance.
    ''' </summary>
    ''' <param name="sqlParams">Anonymous object with named keys as Oracle statement params without any '@' chars in object keys.</param>
    Protected Overrides Sub addParamsWithValue(sqlParams As Object)
        If (Not sqlParams Is Nothing) Then
            Dim sqlParamValue As Object
            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(sqlParams)
                sqlParamValue = prop.GetValue(sqlParams)
                Me._cmd.Parameters.Add(
                    prop.Name,
                    If((sqlParamValue Is Nothing), DBNull.Value, sqlParamValue)
                )
            Next
        End If
    End Sub
    ''' <summary>
    ''' Set up all sql params into internal Command instance.
    ''' </summary>
    ''' <param name="sqlParams">Dictionary with named keys as Oracle statement params without any '@' chars in dictionary keys.</param>
    Protected Overrides Sub addParamsWithValue(sqlParams As Dictionary(Of String, Object))
        If (Not sqlParams Is Nothing) Then
            For Each pair As KeyValuePair(Of String, Object) In sqlParams
                Me._cmd.Parameters.Add(
                    pair.Key,
                    If((pair.Value Is Nothing), DBNull.Value, pair.Value)
                )
            Next
        End If
    End Sub





End Class