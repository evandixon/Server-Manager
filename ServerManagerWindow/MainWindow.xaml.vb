Class MainWindow 

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        lbServers.Items.Add("(Internal)")
        lbServers.SelectedIndex = 0
    End Sub
End Class
