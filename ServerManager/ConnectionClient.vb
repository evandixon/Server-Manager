Imports System.Net.Sockets
Imports System.Net
Imports EncryptionClassLibrary.Encryption.Asymmetric

Public Class ConnectionClient
    Private Client As TcpClient
    Private DecryptKey As CryptographyLibrary.AsymmetricKey
    Private EncryptKey As CryptographyLibrary.AsymmetricKey
    Private EndPoint As IPEndPoint
    Public Event ResponseRecieved(sender As Object, r As ResponsePacket)
    Public Function SendRequest(Request As RequestPacket) As ResponsePacket
        Client = New TcpClient()
        Client.Connect(EndPoint)
        Dim s = Client.GetStream
        Dim d = Request.EncryptPacket(EncryptKey)
        Dim toSend As New List(Of Byte)
        toSend.AddRange(BitConverter.GetBytes(d.Length))
        toSend.AddRange(d)
        s.Write(toSend.ToArray, 0, toSend.Count)
        Dim p As Integer = 0
        Dim pbuf(3) As Byte
        s.Read(pbuf, 0, pbuf.Length)
        p = BitConverter.ToInt32(pbuf, 0)
        Dim data = GetBytesFromStream(s, p)
        Client.Close()
        Return ResponsePacket.DecryptPacket(data, DecryptKey)
    End Function
    ''' <summary>
    ''' Sends a request, then asynchronously waits for response packets.
    ''' Useful when more than one response packet is expected.
    ''' </summary>
    ''' <param name="Request"></param>
    ''' <remarks></remarks>
    Public Async Sub SendRequestAsync(Request As RequestPacket)
        Client = New TcpClient()
        Client.Connect(EndPoint)
        Dim s = Client.GetStream
        Dim d = Request.EncryptPacket(EncryptKey)
        Dim toSend As New List(Of Byte)
        toSend.AddRange(BitConverter.GetBytes(d.Length))
        toSend.AddRange(d)
        s.Write(toSend.ToArray, 0, toSend.Count)
        Dim close As Boolean = False
        While Not close
            Dim p As Integer = 0
            Dim pbuf(3) As Byte
            s.Read(pbuf, 0, pbuf.Length)
            p = BitConverter.ToInt32(pbuf, 0)
            Dim data = GetBytesFromStream(s, p)
            Dim packet = ResponsePacket.DecryptPacket(data, DecryptKey)
            If packet.Response = "close" Then
                close = True
            Else
                RaiseEvent ResponseRecieved(Me, packet)
            End If
        End While
        Client.Close()
    End Sub
    Private Function GetBytesFromStream(Stream As NetworkStream, Length As Integer) As Byte()
        Dim bytes As New List(Of Byte)
        Dim buffer(Length) As Byte
        Dim i = Stream.Read(buffer, 0, Length)

            For count As Integer = 0 To i - 1
                bytes.Add(buffer(count))
            Next
            Return bytes.ToArray
    End Function
    Public Sub New(IP As IPAddress, Port As Integer, DecryptKey As CryptographyLibrary.AsymmetricKey, EncryptKey As CryptographyLibrary.AsymmetricKey)
        EndPoint = New IPEndPoint(IP, Port)
        Me.DecryptKey = DecryptKey
        Me.EncryptKey = EncryptKey
    End Sub

End Class
