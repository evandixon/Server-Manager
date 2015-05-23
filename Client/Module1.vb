Imports System.Net

Module Module1
    WithEvents c As ServerManager.ConnectionClient
    Sub Main()
        'While True
        'Dim m = New ServerManager.ConnectionClient(New IPAddress({127, 0, 0, 1}), 64325, ServerManager.Keys.GetPrivateKey, ServerManager.Keys.GetPublicKey)
        'Dim r = m.SendRequest(New ServerManager.RequestPacket("evandixon", "passwprd", "cout", Console.ReadLine))
        'Console.WriteLine(r.Response)
        'End While
        'Console.ReadLine()
        c = New ServerManager.ConnectionClient(New IPAddress({127, 0, 0, 1}), 64325, ServerManager.Keys.GetPrivateKey, ServerManager.Keys.GetPublicKey)
        While True
            Dim commandType As String = Console.ReadLine
            If Not commandType = "cout" Then
                c.SendRequest(New ServerManager.RequestPacket("evandixon", "password", commandType, Console.ReadLine))
            Else
                c.SendRequestAsync(New ServerManager.RequestPacket("evandixon", "password", commandType, Console.ReadLine))
            End If
        End While
    End Sub

    Private Sub c_ResponseRecieved(sender As Object, r As ServerManager.ResponsePacket) Handles c.ResponseRecieved
        Console.WriteLine(r.Response)
    End Sub
End Module
