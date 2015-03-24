Public Class PluginManager
    Public Property ServiceConstructors As New Dictionary(Of String, ServerManager.Manager.CreateService)
    Public Property Plugins As New List(Of iServicePlugin)
    ''' <summary>
    ''' Creates a new PluginManager given the folder plugin files are stored in.
    ''' Plugins should end in _plg.dll or _plg.exe,  Ex. MyPlugin_plg.dll
    ''' </summary>
    ''' <param name="PluginFolder"></param>
    ''' <remarks></remarks>
    Public Sub New(PluginFolder As String)
        If IO.Directory.Exists(PluginFolder) Then
            Dim assemblies As New List(Of String)
            assemblies.AddRange(IO.Directory.GetFiles(PluginFolder, "*_plg.dll"))
            assemblies.AddRange(IO.Directory.GetFiles(PluginFolder, "*_plg.exe"))
            For Each plugin In assemblies
                Console.WriteLine("Opening plugin " & IO.Path.GetFileName(plugin))
                Dim a As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(plugin)
                Dim types As Type() = a.GetTypes
                For Each item In types
                    Dim IsPlugin As Boolean = False
                    For Each intface As Type In item.GetInterfaces
                        If intface Is GetType(iServicePlugin) Then
                            IsPlugin = True
                        End If
                    Next
                    If IsPlugin Then
                        Dim Plg As iServicePlugin = a.CreateInstance(item.ToString)
                        Plugins.Add(Plg)
                    End If
                Next
            Next
        End If
        For Each item In Plugins
            item.Load(Me)
        Next
    End Sub
End Class
