Imports System.Text
Imports System.Web.Script.Serialization

Public Class Service
    Private WithEvents _p As New Process
    Public Event OutputDataRecieved(sender As Object, e As DataReceivedEventArgs)
    ''' <summary>
    ''' Set by the server manager to find the service name.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ServiceName As String

    Public Overridable Property Filename As String
    Public Overridable Property Arguments As String
    Public Property OutputLog As StringBuilder
    Public Function GetOutputString() As String
        Return OutputLog.ToString
    End Function

    Private Sub _p_ErrorDataReceived(sender As Object, e As DataReceivedEventArgs) Handles _p.ErrorDataReceived
        OutputHandler(sender, e)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sendingProcess"></param>
    ''' <param name="outLine"></param>
    ''' <remarks>Posted by brendan at http://stackoverflow.com/questions/9996709/read-console-process-output</remarks>
    Private Sub OutputHandler(sendingProcess As Object, outLine As DataReceivedEventArgs) Handles _p.OutputDataReceived
        ' Collect the sort command output.
        If Not String.IsNullOrEmpty(outLine.Data) Then
            'Add the text to the collected output.
            OutputLog.AppendLine(outLine.Data)
            RaiseEvent OutputDataRecieved(Me, outLine)
        End If
    End Sub
    Public Overridable Sub StartServer()
        _p.Start()
        _p.BeginOutputReadLine()
        _p.BeginErrorReadLine()
    End Sub

    Public Overridable Sub StopServer()
        _p.Close()
    End Sub
    Public Overridable Sub KillServer()
        _p.Kill()
    End Sub
    Public Overridable Sub SendInput(ConsoleLine As String)
        _p.StandardInput.WriteLine(ConsoleLine)
    End Sub
    Public Overridable Function ToJson() As String
        Dim j As New JavaScriptSerializer
        Return j.Serialize(Me)
    End Function

    Public Sub New(Filename As String, Arguments As String)
        MyBase.New()
        Me.Filename = Filename
        Me.Arguments = Arguments
        OutputLog = New StringBuilder
        '_p = Process
        With _p
            .StartInfo.FileName = Filename
            .StartInfo.Arguments = Arguments
            .StartInfo.WindowStyle = ProcessWindowStyle.Hidden
            .StartInfo.RedirectStandardOutput = True
            .StartInfo.RedirectStandardInput = True
            .StartInfo.RedirectStandardError = True
            .StartInfo.UseShellExecute = False
            .StartInfo.CreateNoWindow = True
            .EnableRaisingEvents = True
        End With
    End Sub
    Public Shared Function GetServiceFromStartData(ConstructorData As String) As Service
        Dim parts As String() = ConstructorData.Split(";".ToCharArray, 2)
        Return New Service(parts(0), parts(1))
    End Function
    Public Overridable Function GetConstructorData() As String
        Return String.Format("{0};{1}", Filename, Arguments)
    End Function
    Public Overridable Function GetServiceType() As String
        Return "service"
    End Function
End Class
