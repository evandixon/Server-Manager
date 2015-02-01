Imports ServerManager.Service

Module Module1
    WithEvents manager As ServerManager.ServerManager
    Sub Main()
        manager = New ServerManager.ServerManager
        manager.Servers.Add(New ServerManager.Service("C:\Program Files\Java\jre7\bin\java", "-Xms5G -Xmx5G -XX:PermSize=128m -jar ""C:\Users\Evan\OneDrive\Minecraft\Minecraft Servers\modpacks^Direwolf20_1_6_4^1_0_23^Direwolf20Server\FTBServer-1.6.4-965.jar"" nogui"))
        manager.StartAllServers()
        While True
            Dim line = Console.ReadLine
            If line = "quit" Then
                Exit While
            Else
                manager.Servers(0).SendInput(line)
            End If
        End While
        manager.StopAllServers()
    End Sub

    Private Sub manager_ConsoleDataWritten(sender As Object, args As ConsoleWrittenEventArgs) Handles manager.ConsoleDataWritten
        Console.WriteLine(args.Line)
    End Sub
End Module
