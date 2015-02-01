Imports System.Text

Public Class Service
    Dim _p As Process
    Public Event OutputDataRecieved(sender As Object, e As ConsoleWrittenEventArgs)
    Public Overridable Property Filename As String
    Public Overridable Property Arguments As String
    Public Property OutputLog As StringBuilder
    Protected Overridable Sub InitializeProcess()
        _p = New Process
        With _p
            .StartInfo.FileName = Filename
            .StartInfo.Arguments = Arguments
            .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.UseShellExecute = False
            AddHandler .OutputDataReceived, AddressOf OutputHandler
        End With
    End Sub
    ''' <summary>
    ''' Posted by brendan at http://stackoverflow.com/questions/9996709/read-console-process-output
    ''' </summary>
    ''' <param name="sendingProcess"></param>
    ''' <param name="outLine"></param>
    ''' <remarks></remarks>
    Private Sub OutputHandler(sendingProcess As Object, outLine As DataReceivedEventArgs)
        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            'Add the text to the collected output.
            OutputLog.AppendLine(outLine.Data)
            RaiseEvent OutputDataRecieved(Me, New ConsoleWrittenEventArgs(outLine.Data))
        End If
    End Sub
    Public Overridable Sub StartServer()
        _p.Start()
        _p.BeginOutputReadLine()
    End Sub

    Public Overridable Sub StopServer()
        _p.Close()
    End Sub
    Public Overridable Sub SendInput(ConsoleLine As String)
        _p.StandardInput.WriteLine(ConsoleLine)
    End Sub
    Public Sub New(Filename As String, Optional Arguments As String = "")
        MyBase.New()
        Me.Filename = Filename
        Me.Arguments = Arguments
        OutputLog = New StringBuilder
        InitializeProcess()
    End Sub
    Public Class ConsoleWrittenEventArgs
        Inherits EventArgs
        Public Property Line As String
        Public Sub New(Line As String)
            Me.Line = Line
        End Sub
    End Class
End Class
