Imports System.Web.Script.Serialization
Imports EncryptionClassLibrary.Encryption
Public Class RequestPacket
    Public Property Username As String
    Public Property Password As String
    Public Property Type As String
    Public Property Request As String
    Public Sub New(Username As String, Password As String, Type As String, Request As String)
        Me.New()
        Me.Username = Username
        Me.Password = Password
        Me.Type = Type
        Me.Request = Request
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Public Shared Function DecryptPacket(EncryptedData As Byte(), PrivateKey As CryptographyLibrary.AsymmetricKey) As RequestPacket
        Dim j As New JavaScriptSerializer
        Return j.Deserialize(Of RequestPacket)(Security.DecryptData(PrivateKey, EncryptedData))
    End Function
    Public Function EncryptPacket(PublicKey As CryptographyLibrary.AsymmetricKey) As Byte()
        Dim j As New JavaScriptSerializer
        Return Security.EncryptData(PublicKey, j.Serialize(Me))
    End Function
End Class
