Imports System.Data.Common
Imports Oracle.DataAccess.Client

Public Class Transaction
    Inherits Databasic.Transaction

    Public Overrides Property Instance As DbTransaction
        Get
            Return Me._instance
        End Get
        Set(value As DbTransaction)
            Me._instance = value
        End Set
    End Property
    Private _instance As OracleTransaction

End Class