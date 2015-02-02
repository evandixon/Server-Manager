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
    ''' <summary>
    ''' In the future, will decrypt a packet.  Currently, returns the string representation of the data.
    ''' </summary>
    ''' <param name="EncryptedData"></param>
    ''' <param name="PrivateKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DecryptPacket(EncryptedData As Byte(), PrivateKey As Asymmetric.PrivateKey) As RequestPacket
        'Dim Encrypted As New EncryptionClassLibrary.Encryption.Data(EncryptedData)
        'Dim a As New EncryptionClassLibrary.Encryption.Asymmetric()
        'Dim Decrypted = a.Decrypt(Encrypted, PrivateKey)
        Dim j As New JavaScriptSerializer
        'Return j.Deserialize(Of RequestPacket)(Decrypted.Text)
        Return j.Deserialize(Of RequestPacket)(Text.Encoding.ASCII.GetString(EncryptedData))
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
        Return Decrypted.Bytes
    End Function
End Class
