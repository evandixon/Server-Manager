Imports System.Net.Sockets
Imports System.Net
Imports EncryptionClassLibrary.Encryption.Asymmetric

Public Class ConnectionClient
    Private Client As TcpClient
    Private DecryptKey As PrivateKey
    Private EncryptKey As PublicKey
    Private EndPoint As IPEndPoint
    Public Event ResponseRecieved(sender As Object, r As ResponsePacket)
    Public Function SendRequest(Request As RequestPacket) As ResponsePacket
        Client.Connect(EndPoint)
        Dim s = Client.GetStream
        Dim d = Request.EncryptPacket(EncryptKey)
        s.Write(d, 0, d.Length)
        
        Dim p = ResponsePacket.DecryptPacket(GetBytesFromStream(s, 256), DecryptKey)
        If p.Type = "length" Then
            Dim r = ResponsePacket.DecryptPacket(GetBytesFromStream(s, CInt(p.Response)), DecryptKey)
            Client.Close()
            Return r
        Else
            Client.Close()
            Return p
        End If
    End Function
    ''' <summary>
    ''' Sends a request, then asynchronously waits for response packets.
    ''' Useful when more than one response packet is expected.
    ''' </summary>
    ''' <param name="Request"></param>
    ''' <remarks></remarks>
    Public Async Sub SendRequestAsync(Request As RequestPacket)
        Client.Connect(EndPoint)
        Dim s = Client.GetStream
        Dim d = Request.EncryptPacket(EncryptKey)
        s.Write(d, 0, d.Length)
        Dim close As Boolean = False
        While Not close
            '' p = BitConverter.ToInt32(GetBytesFromStream(s, 4), 0)
            Dim p As Integer = 0
            Dim pbuf(3) As Byte
            s.Read(pbuf, 0, pbuf.Length)
            p = BitConverter.ToInt32(pbuf, 0)
            'If p.Type = "length" Then
            Dim r = ResponsePacket.DecryptPacket(GetBytesFromStream(s, p), DecryptKey)
            RaiseEvent ResponseRecieved(Me, r)
            'ElseIf p.Type = "close" Then
            'Console.WriteLine("Closed.")
            'close = True
            'Else
            'RaiseEvent ResponseRecieved(Me, p)
            'End If
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
    Public Sub New(IP As IPAddress, Port As Integer, DecryptKey As PrivateKey, EncryptKey As PublicKey)
        Client = New TcpClient()
        EndPoint = New IPEndPoint(IP, Port)
        Me.DecryptKey = DecryptKey
        Me.EncryptKey = EncryptKey
    End Sub

End Class
