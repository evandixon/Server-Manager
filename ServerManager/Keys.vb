Imports CryptographyLibrary

Public Class Keys
    'Public Shared Sub GenerateKeyset()
    '    Dim t = AsymmetricProvider
    '    Dim pu As New PublicKey
    '    Dim pr As New PrivateKey
    '    t.GenerateNewKeyset(pu, pr)
    '    IO.File.WriteAllText("public.xml", pu.ToXml)
    '    IO.File.WriteAllText("private.xml", pr.ToXml)
    'End Sub
    Public Shared Function GetPublicKey() As CryptographyLibrary.AsymmetricKey
        'If Not IO.File.Exists("public.xml") Then
        '    GenerateKeyset()
        'End If
        'Dim p As New PublicKey
        'p.LoadFromXml(IO.File.ReadAllText("public.xml"))
        ''p.Modulus = "uwAOOwOYHdrI9GlN7cxr0AzNwwynrC3j4B/4UgPBi10WDcW7MNHJSpI4vNC3rY7NhIs8p/ZpY0ZHJzePPpX8XCtzO5ulML6sEFrPXrq2GOxUDgIpps8/l2qr9SUKXZBR+DcjX0nHAjBUBuKnnr+JawgOwOVtxCpLKBWklvjh4cs="
        ''p.Exponent = "AQAB"
        'Return p
        Return New CryptographyLibrary.AsymmetricKey(IO.File.ReadAllText("public.xml"))
    End Function
    Public Shared Function GetPrivateKey() As CryptographyLibrary.AsymmetricKey
        'If Not IO.File.Exists("private.xml") Then
        '    GenerateKeyset()
        'End If
        'Dim p As New PrivateKey
        'p.LoadFromXml(IO.File.ReadAllText("private.xml"))
        'Return p
        Return New CryptographyLibrary.AsymmetricKey(IO.File.ReadAllText("private.xml"), True)
    End Function
End Class
