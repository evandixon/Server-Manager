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
    Public Shared Function DecryptPacket(EncryptedData As Byte(), PrivateKey As Asymmetric.PrivateKey) As ResponsePacket
        'Dim Encrypted As New EncryptionClassLibrary.Encryption.Data(EncryptedData)
        'Dim a As New EncryptionClassLibrary.Encryption.Asymmetric()
        'Dim Decrypted = a.Decrypt(Encrypted, PrivateKey)
        Dim j As New JavaScriptSerializer
        'Return j.Deserialize(Of RequestPacket)(Decrypted.Text)
        Return j.Deserialize(Of ResponsePacket)(Text.Encoding.ASCII.GetString(Security.DecryptData(PrivateKey, EncryptedData)))
    End Function
    ''' <summary>
    ''' In the future, will encrypt the packet using symmetric encryption, with asymetric encryption to encrypt the key.
    ''' Currently jsut returns a byte representation of the packet.
    ''' </summary>
    ''' <param name="PublicKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EncryptPacket(PublicKey As Asymmetric.PublicKey) As Byte()
        Dim j As New JavaScriptSerializer
        Dim Decrypted As New Data(j.Serialize(Me))
        'Dim a As New EncryptionClassLibrary.Encryption.Asymmetric
        'Dim Encrypted = a.Encrypt(Decrypted, PublicKey)
        'Return Encrypted.Bytes
        Return Security.EncryptData(PublicKey, Decrypted.Bytes)
    End Function
End Class
