Class MainWindow 
    WithEvents manager As New ServerManager.Manager
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        lbServers.Items.Add("(Internal)")
        lbServers.SelectedIndex = 0
        'manager.startallservers()
    End Sub

    Private Sub manager_ConsoleDataWritten(sender As Object, args As DataReceivedEventArgs) Handles manager.ConsoleDataWritten
        Console.WriteLine(args.Data)
    End Sub
End Class
