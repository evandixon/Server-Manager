Imports System.Net

Module Module1

    Sub Main()
        While True
            Dim m = New ServerManager.ConnectionClient(New IPAddress({127, 0, 0, 1}), 64325, ServerManager.Keys.GetPrivateKey, ServerManager.Keys.GetPublicKey)
            Dim r = m.SendRequest(New ServerManager.RequestPacket("evandixon", "passwprd", "cin", Console.ReadLine))
            Console.WriteLine(r.Response)
        End While
    End Sub

End Module
