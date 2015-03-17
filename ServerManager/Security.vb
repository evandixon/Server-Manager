Imports EncryptionClassLibrary
Imports EncryptionClassLibrary.Encryption
Public Class Security
    <Obsolete("Not obsolete, but not implemented.  Always returns true.")>
    Public Shared Function IsValidUser(Username As String, Password As String)
        Return True
    End Function

    Public Shared Function EncryptData(EncryptionKey As Asymmetric.PublicKey, Data As Byte()) As Byte()
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
        Return Data
    End Function
    Public Shared Function DecryptData(DecryptKey As Asymmetric.PrivateKey, EncryptedBytes As Byte()) As Byte()
        'Dim a As New Asymmetric()
        'Dim s As New Symmetric(Symmetric.Provider.Rijndael)
        'Dim length = BitConverter.ToUInt32(EncryptedBytes, 0)
        'Dim encryptedKey As New Data(GenericArrayOperations(Of Byte).CopyOfRange(EncryptedBytes, 4, length + 4))
        'Dim decryptedKey = a.Decrypt(encryptedKey, DecryptKey)
        'Dim encryptedData As New Data(GenericArrayOperations(Of Byte).CopyOfRange(EncryptedBytes, 4 + length, EncryptedBytes.Length - 1))
        'Return s.Decrypt(encryptedData, decryptedKey).Bytes
        Return EncryptedBytes
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