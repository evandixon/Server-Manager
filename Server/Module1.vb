Imports System.Net
Module Module1
    WithEvents m As ServerManager.ConnectionServer
    Sub Main()
        m = New ServerManager.ConnectionServer(New IPAddress({127, 0, 0, 1}), 64325, ServerManager.Keys.GetPrivateKey, ServerManager.Keys.GetPublicKey) '198, 245, 60, 203
        m.Listen()
        Console.ReadLine()
    End Sub

    Private Sub m_ClientConnected(sender As Object, e As ServerManager.ConnectionServerEventArgs) Handles m.ClientConnected
        Console.WriteLine("Response recieved.")
        e.SendResponse(New ServerManager.ResponsePacket("string", "Your request was " & e.Request.Request))
    End Sub
End Module
