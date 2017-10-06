Public Class SqlError
    Inherits Databasic.SqlError
    Public Property Procedure As String
    Public Property Source As String
    Public Property DataSource As String
    Public Property ArrayBindIndex As Integer

    Public Sub New(oracleError As Global.Oracle.DataAccess.Client.OracleError)
        Me.Message = oracleError.Message
        Me.Code = oracleError.Number

        Me.DataSource = oracleError.DataSource
        Me.Procedure = oracleError.Procedure
        Me.Source = oracleError.Source
        Me.ArrayBindIndex = oracleError.ArrayBindIndex
    End Sub
End Class
