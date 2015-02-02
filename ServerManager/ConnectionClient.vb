Imports System.Net.Sockets
Imports System.Net
Imports EncryptionClassLibrary.Encryption.Asymmetric

Public Class ConnectionClient
    Private Client As TcpClient
    Private DecryptKey As PrivateKey
    Private EncryptKey As PublicKey
    Private EndPoint As IPEndPoint
    Public Function SendRequest(Request As RequestPacket) As ResponsePacket
        Client.Connect(EndPoint)
        Dim s = Client.GetStream
        Dim d = Request.EncryptPacket(EncryptKey)
        s.Write(d, 0, d.Length)
        Dim bytes As New List(Of Byte)
        Dim buffer(256) As Byte
        Dim i = s.Read(buffer, 0, buffer.Length - 1)
        For count As Integer = 0 To i - 1
            bytes.Add(buffer(count))
        Next
        Client.Close()
        Return ResponsePacket.DecryptPacket(bytes.ToArray, DecryptKey)
    End Function
    Public Sub New(IP As IPAddress, Port As Integer, DecryptKey As PrivateKey, EncryptKey As PublicKey)
        Client = New TcpClient()
        EndPoint = New IPEndPoint(IP, Port)
        Me.DecryptKey = DecryptKey
        Me.EncryptKey = EncryptKey
    End Sub

End Class
