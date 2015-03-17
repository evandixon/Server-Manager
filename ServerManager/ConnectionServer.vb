Imports System.Net.Sockets
Imports System.Threading
Imports System.Net
Imports EncryptionClassLibrary.Encryption.Asymmetric

Public Class ConnectionServer
    Public Event ClientConnected(sender As Object, ByRef e As ConnectionServerEventArgs)
    Private Listener As TcpListener
    Private Cancel As Boolean
    Private DecryptKey As PrivateKey
    Private EncryptKey As PublicKey
    ''' <summary>
    ''' Listens for incoming connections until an event handler cancels it.
    ''' Runs synchronously.
    ''' </summary>
    ''' <remarks></remarks>
    'Public Sub Listen()
    '    Cancel = False
    '    Listener.Start()
    '    While Not Cancel
    '        Dim close As Boolean = True
    '        Dim client As TcpClient = Listener.AcceptTcpClient
    '        Dim s = client.GetStream
    '        Dim bytes As New List(Of Byte)
    '        Dim buffer(256) As Byte
    '        Dim i = s.Read(buffer, 0, buffer.Length - 1)
    '        For count As Integer = 0 To i - 1
    '            bytes.Add(buffer(count))
    '        Next
    '        Dim packet = RequestPacket.DecryptPacket(bytes.ToArray, DecryptKey)
    '        If Security.IsValidUser(packet.Username, packet.Password) Then
    '            Dim args = New ConnectionServerEventArgs(packet, client, EncryptKey)
    '            RaiseEvent ClientConnected(Me, args)
    '            Cancel = args.Cancel
    '            close = args.close
    '        End If
    '        If close Then client.Close()
    '    End While
    'End Sub
    Public Async Function StartListening() As task
        Cancel = False
        Listener.Start()
        While Not Cancel
            Dim close As Boolean = True
            Dim client As TcpClient = Await Listener.AcceptTcpClientAsync
            Dim s = client.GetStream
            Dim p As Integer = 0
            Dim pbuf(3) As Byte
            s.Read(pbuf, 0, pbuf.Length)
            p = BitConverter.ToInt32(pbuf, 0)
            Dim packet = RequestPacket.DecryptPacket(GetBytesFromStream(s, p), DecryptKey)
            If Security.IsValidUser(packet.Username, packet.Password) Then
                Dim args = New ConnectionServerEventArgs(packet, client, EncryptKey)
                RaiseEvent ClientConnected(Me, args)
                Cancel = args.Cancel
                close = args.Close
            End If
            If close Then client.Close()
        End While
    End Function
    Private Function GetBytesFromStream(Stream As NetworkStream, Length As Integer) As Byte()
        Dim bytes As New List(Of Byte)
        Dim buffer(Length) As Byte
        Dim i = Stream.Read(buffer, 0, Length)

        For count As Integer = 0 To i - 1
            bytes.Add(buffer(count))
        Next
        Return bytes.ToArray
    End Function
    Public Sub StopListening()
        Listener.Stop()
    End Sub
    Public Sub New(IP As IPAddress, Port As Integer, DecryptKey As PrivateKey, EncryptKey As PublicKey)
        Listener = New TcpListener(IP, Port)
        Me.DecryptKey = DecryptKey
        Me.EncryptKey = EncryptKey
    End Sub
End Class
Public Class ConnectionServerEventArgs
    Inherits EventArgs
    Public Property Client As TcpClient
    Private EncryptKey As PublicKey
    Public Property Request As RequestPacket
    Public Property Cancel As Boolean
    ''' <summary>
    ''' Whether or not to close the client connection after the event.
    ''' Useful when responses will be sent after all events fire.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Close As Boolean
    Public Sub SendResponse(Response As ResponsePacket)
        Try
            Dim e = Response.EncryptPacket(EncryptKey)
            Dim s = _Client.GetStream
            'Dim d = (New ResponsePacket("length", e.Length)).EncryptPacket(EncryptKey)
            Dim l = BitConverter.GetBytes(e.Length)
            Dim buffer As New List(Of Byte)
            For Each item In l
                buffer.Add(item)
            Next
            For Each item In e
                buffer.Add(item)
            Next
            Dim bytes = buffer.ToArray
            s.Write(bytes, 0, bytes.Length)
            Console.WriteLine("Response sent: " & Response.Response)
        Catch ex As Exception
            Console.WriteLine(ex)
        End Try
    End Sub
    Public Sub New(Request As RequestPacket, ByRef Client As TcpClient, EncryptKey As PublicKey)
        Me.Request = Request
        Me.Client = Client
        Me.Cancel = False
        Me.Close = True
    End Sub
End Class