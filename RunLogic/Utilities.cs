using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class Utilities
{
    private static string IV = "SixtEEnCHarsLen%";
    private static string key = "KEYoNLINEeDI501ISGER5161OK?91281";
    public static string GetFolderName(string IpAddress)
    {
        string dictKey = Encrypt(IpAddress);
        if (!string.IsNullOrEmpty(dictKey))
        {
            if (dictionary.ContainsKey(dictKey))
            {
                return dictionary[dictKey];
            }
            else
            {
                dictionary[dictKey] = Guid.NewGuid().ToString();
                return dictionary[dictKey];
            }
        }
        else
        {
            return string.Empty;
        }
    }
    private static Dictionary<string, string> dictionary=new Dictionary<string, string>();
    private static string Encrypt(string originalString)
    {
        byte[] encrypted;
        using (Aes myAes = Aes.Create())
        {
            myAes.Key = Encoding.ASCII.GetBytes(key);
            myAes.IV = Encoding.ASCII.GetBytes(IV);

            // Encrypt the string to an array of bytes.
            encrypted = EncryptStringToBytes_Aes(originalString, myAes.Key, myAes.IV);
            return Encoding.ASCII.GetString(encrypted, 0, encrypted.Length);
        }
    }
    private static string Decrypt(string encryptedString)
    {
        using (Aes myAes = Aes.Create())
        {
            myAes.Key = Encoding.UTF8.GetBytes(key);
            myAes.IV = Encoding.UTF8.GetBytes(IV);

            byte[] byteArray = Encoding.ASCII.GetBytes(encryptedString);
            // Decrypt the bytes to a string.
            return DecryptStringFromBytes_Aes(byteArray, myAes.Key, myAes.IV);
        }
    }
    static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");
        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }


        // Return the encrypted bytes from the memory stream.
        return encrypted;

    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an Aes object
        // with the specified key and IV.
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }

        }

        return plaintext;

    }
}