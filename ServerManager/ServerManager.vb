﻿Imports System.Text
Imports System.Net.Sockets

Public Class Manager
    Public Property Servers As Dictionary(Of String, Service)
    Private Property ServiceConstructors As Dictionary(Of String, CreateService)
    Delegate Function CreateService(ServiceData As String) As Service
    Private Property COutClients As New Dictionary(Of String, ConnectionServerEventArgs)
#Region "Events and Handlers"
    Public Sub HandleRequest(sender As Object, ByRef e As ServerManager.ConnectionServerEventArgs)
        Select Case e.Request.Type
            Case "cin"
                Dim parts As String() = e.Request.Request.Split(" ".ToCharArray, 2)
                If Servers.ContainsKey(parts(0)) Then
                    Servers(parts(0)).SendInput(parts(1))
                    e.SendResponse(New ResponsePacket("string", "Input sent successfully."))
                Else
                    e.SendResponse(New ResponsePacket("string", "Server " & parts(0) & " is not currently loaded or does not exist."))
                End If
            Case "cout"
                If Servers.ContainsKey(e.Request.Request) Then
                    e.Close = False
                    COutClients.Add(e.Request.Request, e)
                    e.SendResponse(New ResponsePacket("string", "You will be sent additional packets for the console's output."))
                Else
                    e.SendResponse(New ResponsePacket("string", "Server " & e.Request.Request & " is not currently loaded or does not exist."))
                End If
        End Select
    End Sub

    Public Event ConsoleDataWritten(sender As Object, args As DataReceivedEventArgs)
    Private Sub OnConsoleDataWritten(sender As Object, args As DataReceivedEventArgs)
        'Because COutCLients may be changed while this runs
        Dim clients As New Dictionary(Of String, ConnectionServerEventArgs)(COutClients)
        For Each item In clients
            If DirectCast(sender, Service).ServiceName = item.Key Then
                item.Value.SendResponse(New ResponsePacket("cout-" & item.Key, args.Data))
            End If
        Next
        RaiseEvent ConsoleDataWritten(sender, args)
    End Sub
#End Region

#Region "Public Methods"
    <Obsolete("Not obsolete, not finished.  Only loads internal code.")>
    Private Sub LoadPlugins()
        ServiceConstructors.Add("service", AddressOf Service.GetServiceFromStartData)
    End Sub
    Private Sub LoadServers()
        LoadPlugins()
        Dim p = GetResourcePath("Servers.txt")
        If Not IO.File.Exists(p) Then
            IO.File.WriteAllText(p, "")
        End If
        Dim serverDefs As String() = IO.File.ReadAllLines(p)
        For Each item In serverDefs
            Dim parts As String() = item.Split(" ".ToCharArray, 3)
            If ServiceConstructors.ContainsKey(parts(0)) Then
                Servers.Add(parts(1), ServiceConstructors(parts(0)).Invoke(parts(2)))
                Servers(parts(1)).ServiceName = parts(1)
            End If
        Next
    End Sub
    Public Sub StartAllServers()
        LoadServers()
        For Each s In Servers.Values
            AddHandler s.OutputDataRecieved, AddressOf OnConsoleDataWritten
            s.StartServer()
        Next
    End Sub
    Public Sub StopAllServers()
        For Each s In Servers.Values
            s.StopServer()
        Next
        SaveServers()
    End Sub
    Public Sub SaveServers()
        Dim p = GetResourcePath("Servers.txt")
        Dim s As New StringBuilder
        For Each item In Servers
            s.AppendLine(String.Format("{0} {1} {2}", item.Value.GetServiceType, item.Key, item.Value.GetConstructorData))
        Next
        IO.File.WriteAllText(p, s.ToString.Trim)
    End Sub
    Public Function GetResourcePath(RelativePath As String) As String
        Return IO.Path.Combine(Environment.CurrentDirectory, RelativePath)
    End Function
#End Region

    Public Sub New()
        Servers = New Dictionary(Of String, Service)
        ServiceConstructors = New Dictionary(Of String, CreateService)
    End Sub
End Class
