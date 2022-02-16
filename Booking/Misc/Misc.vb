Imports System.Text.RegularExpressions

Module Misc
    Public Function IsValidEmailFormat(ByVal email As String) As Boolean
        If email = "" Then Return True

        Static emailExpression As New Regex("^[_a-z0-9-]+(.[a-z0-9-]+)@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")

        Dim result = emailExpression.IsMatch(email)
        Return result
    End Function
End Module
