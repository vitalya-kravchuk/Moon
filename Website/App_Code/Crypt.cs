using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Crypt
{
    public static string EncryptString(string InputText, string Password)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(InputText);
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(PlainText, 0, PlainText.Length);
        cryptoStream.FlushFinalBlock();
        byte[] CipherBytes = memoryStream.ToArray();
        memoryStream.Close();
        cryptoStream.Close();
        string EncryptedData = Convert.ToBase64String(CipherBytes);
        return EncryptedData;
    }

    public static string DecryptString(string InputText, string Password)
    {
        RijndaelManaged RijndaelCipher = new RijndaelManaged();
        byte[] EncryptedData = Convert.FromBase64String(InputText);
        byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

        PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
        ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
        MemoryStream memoryStream = new MemoryStream(EncryptedData);
        CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);
        byte[] PlainText = new byte[EncryptedData.Length];
        int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
        memoryStream.Close();
        cryptoStream.Close();
        string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
        return DecryptedData;
    }
}
