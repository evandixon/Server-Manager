Public Class ServerManager
    Public Event ConsoleDataWritten(sender As Object, args As Service.ConsoleWrittenEventArgs)
    Public Property Servers As List(Of Service)
    Public Sub StartAllServers()
        For Each s In Servers
            AddHandler s.OutputDataRecieved, AddressOf OnConsoleDataWritten
            s.StartServer()
        Next
    End Sub
    Public Sub StopAllServers()
        For Each s In Servers
            s.StopServer()
        Next
    End Sub
    Private Sub OnConsoleDataWritten(sender As Object, args As Service.ConsoleWrittenEventArgs)
        RaiseEvent ConsoleDataWritten(sender, args)
    End Sub
    Public Sub New()
        Servers = New List(Of Service)
    End Sub
End Class
