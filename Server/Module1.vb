Imports System.Net
Module Module1
    WithEvents listener As ServerManager.ConnectionServer
    WithEvents manager As ServerManager.Manager
    Sub Main()
        listener = New ServerManager.ConnectionServer(New IPAddress({127, 0, 0, 1}), 64325, ServerManager.Keys.GetPrivateKey, ServerManager.Keys.GetPublicKey) '198, 245, 60, 203
        manager = New ServerManager.Manager()
        manager.StartAllServers()
        AddHandler listener.ClientConnected, AddressOf manager.HandleRequest
        listener.StartListening()
        Console.ReadLine()
        manager.StopAllServers()
    End Sub
    Private Sub m_ClientConnected(sender As Object, ByRef e As ServerManager.ConnectionServerEventArgs) Handles listener.ClientConnected
        Console.WriteLine(String.Format("Request recieved from {0}.  Request type: {1}.  Request body: {2}", e.Request.Username, e.Request.Type, e.Request.Request))
    End Sub

    'Private Sub manager_ConsoleDataWritten(sender As Object, args As DataReceivedEventArgs) Handles manager.ConsoleDataWritten
    '    'Console.WriteLine(args.Data)
    'End Sub
End Module
