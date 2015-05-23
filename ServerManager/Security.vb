Imports EncryptionClassLibrary
Imports EncryptionClassLibrary.Encryption
Public Class Security
    <Obsolete("Not obsolete, but not implemented.  Always returns true.")>
    Public Shared Function IsValidUser(Username As String, Password As String)
        Return True
    End Function

    Public Shared Function EncryptData(EncryptionKey As CryptographyLibrary.AsymmetricKey, Data As String) As Byte()
        Static e As New System.Text.UnicodeEncoding
        Dim symmetricKey As String = "GeneratedSymmetricKey"
        Dim keyBytes As Byte() = e.GetBytes(CryptographyLibrary.AsymmetricProvider.Encrypt(symmetricKey, EncryptionKey))
        Dim encryptedString As String = CryptographyLibrary.SymmetricProvider.Encrypt(Data, symmetricKey, CryptographyLibrary.SymmetricEncryptionAlgorithm.Rijndael)
        Dim output As New List(Of Byte)
        output.AddRange(BitConverter.GetBytes(keyBytes.Length))
        output.AddRange(keyBytes)
        output.AddRange(e.GetBytes(encryptedString))
        Return output.ToArray

        'Dim a As New Asymmetric()
        'Dim s As New Symmetric(Symmetric.Provider.Rijndael)
        'Dim rawKey = s.RandomKey
        'Dim encryptedKey = a.Encrypt(rawKey, EncryptionKey)
        'Dim keyLength As Integer = encryptedKey.Bytes.Length
        'Dim dataToEncrypt As New Data(Data)
        'Dim encryptedData = s.Encrypt(dataToEncrypt, rawKey)
        'Dim output As New List(Of Byte)
        'output.AddRange(BitConverter.GetBytes(keyLength))
        'output.AddRange(encryptedKey.Bytes)
        'output.AddRange(encryptedData.Bytes)
        'Return output.ToArray
        'Return Data
    End Function
    Public Shared Function DecryptData(DecryptKey As CryptographyLibrary.AsymmetricKey, EncryptedBytes As Byte()) As String
            Static e As New System.Text.UnicodeEncoding
            Dim keyLength As Integer = BitConverter.ToInt32(EncryptedBytes, 0)
            Dim keyBytes As Byte() = GenericArrayOperations(Of Byte).CopyOfRange(EncryptedBytes, 4, keyLength + 3)
            Dim symmetricKey As String = CryptographyLibrary.AsymmetricProvider.Decrypt(e.GetString(keyBytes), DecryptKey)
            Dim encryptedString = e.GetString(GenericArrayOperations(Of Byte).CopyOfRange(EncryptedBytes, 4 + keyLength, EncryptedBytes.Length - 1))
            Return CryptographyLibrary.SymmetricProvider.Decrypt(encryptedString, symmetricKey, CryptographyLibrary.SymmetricEncryptionAlgorithm.Rijndael)
    End Function
End Class
Public Class GenericArrayOperations(Of T)
    Public Shared Function CopyOfRange(ByteArr As T(), Index As Integer, EndPoint As Integer) As T()
        Dim output(Math.Max(Math.Min(EndPoint, ByteArr.Length) - Index, 0)) As T
        For x As Integer = 0 To output.Length - 1
            output(x) = ByteArr(x + Index)
        Next
        Return output
    End Function
End Class