Public Class ProviderResource
    Inherits Databasic.ProviderResource

	Public Overrides Function GetTableColumns(table As String, connection As Databasic.Connection) As Dictionary(Of String, Boolean)
		Throw New NotImplementedException()
		Dim result As New Dictionary(Of String, Boolean)
		Dim rawData As Dictionary(Of String, String) = Databasic.Statement.Prepare("
				SELECT 
					c.IS_NULLABLE,
					c.COLUMN_NAME
				FROM 
					INFORMATION_SCHEMA.COLUMNS AS c
				WHERE
					c.TABLE_SCHEMA = @database AND
					c.TABLE_NAME = @table
				ORDER BY 
					c.ORDINAL_POSITION
			", connection
		).FetchAll(New With {
			.database = connection.Provider.Database,
			.table = table
		}).ToDictionary(Of String, String)("COLUMN_NAME")
		Dim columnCouldBenull As Boolean
		For Each item In rawData
			columnCouldBenull = item.Value.ToUpper().IndexOf("NO") = -1
			result.Add(item.Key, columnCouldBenull)
		Next
		Return result
	End Function

	Public Overrides Function GetLastInsertedId(ByRef transaction As Databasic.Transaction, Optional ByRef classMetaDescription As MetaDescription = Nothing) As Object
		Throw New NotImplementedException()
		Return Databasic.Statement.Prepare("SELECT LAST_INSERT_ID()", transaction).FetchOne().ToInstance(Of Object)()
	End Function

	'Public Overrides Function GetAll(
	'		connection As Databasic.Connection,
	'		columns As String,
	'		table As String,
	'		Optional offset As Int64? = Nothing,
	'		Optional limit As Int64? = Nothing,
	'		Optional orderByStatement As String = ""
	'	) As Databasic.Statement
	'	Dim sql = $"SELECT {columns} FROM {table}"
	'	offset = If(offset, 0)
	'	limit = If(limit, 0)
	'	If limit > 0 Then
	'		sql += If(orderByStatement.Length > 0, " ORDER BY " + orderByStatement, "") +
	'				$" LIMIT {If(limit = 0, "18446744073709551615", limit.ToString())} OFFSET {offset}"
	'	End If
	'	Return Databasic.Statement.Prepare(sql, connection).FetchAll()
	'End Function

	Public Overridable Function GetList(
		conditionSqlStatement As String,
		conditionParams As Object,
		orderBySqlStatement As String,
		offset As Int64?,
		limit As Int64?,
		connectionOrTransaction As Object,
		ByRef classMetaDescription As MetaDescription
	) As Databasic.Statement
		Throw New NotImplementedException()
		Dim columns As String = String.Join(",", Databasic.ProviderResource.ColumnsArray(classMetaDescription.ClassType, 0))
		Dim sql As String = $"SELECT {columns} FROM {ActiveRecord.Resource.Table(classMetaDescription)}"
		Dim params As Object = Nothing
		If Not String.IsNullOrEmpty(conditionSqlStatement) Then
			sql += " WHERE " + conditionSqlStatement
			params = conditionParams
		End If
		If Not String.IsNullOrEmpty(orderBySqlStatement) Then
			sql += " ORDER BY " + orderBySqlStatement
		End If
		If offset.HasValue Then
			' offset a number, but could be 0, limit is unknown
			sql += $" LIMIT {If(Not limit.HasValue, "18446744073709551615", limit.ToString())} OFFSET {offset}"
		ElseIf limit.HasValue And limit > 0 Then
			' offset is null, limit is a number, but could be 0
			sql += $" LIMIT {limit.ToString()}"
		End If
		Return Databasic.Statement.Prepare(sql, connectionOrTransaction).FetchAll(params)
	End Function

End Class