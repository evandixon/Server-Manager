Imports System.Web.Script.Serialization
Imports EncryptionClassLibrary.Encryption

Public Class ResponsePacket
    Public Property Type As String
    Public Property Response As String
    Public Sub New(Type As String, Response As String)
        Me.New()
        Me.Type = Type
        Me.Response = Response
    End Sub
    Public Sub New()
        MyBase.New()
    End Sub
    Public Shared Function DecryptPacket(EncryptedData As Byte(), PrivateKey As CryptographyLibrary.AsymmetricKey) As ResponsePacket
        Dim j As New JavaScriptSerializer
        Return j.Deserialize(Of ResponsePacket)(Security.DecryptData(PrivateKey, EncryptedData))
    End Function
    Public Function EncryptPacket(PublicKey As CryptographyLibrary.AsymmetricKey) As Byte()
        Dim j As New JavaScriptSerializer
        Return Security.EncryptData(PublicKey, j.Serialize(Me))
    End Function
End Class
