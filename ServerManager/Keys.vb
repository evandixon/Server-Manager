Imports EncryptionClassLibrary.Encryption.Asymmetric

Public Class Keys
    Public Shared Function GetPublicKey() As PublicKey
        Dim p As New PublicKey
        p.LoadFromXml("public.xml")
        Return p
    End Function
    Public Shared Function GetPrivateKey() As PrivateKey
        Dim p As New PrivateKey
        p.LoadFromXml("private.xml")
        Return p
    End Function
End Class
