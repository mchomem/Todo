namespace Todo.Application.Helpers;

public static class CypherHelper
{
    #region Fields

    private static byte[] IV = new byte[] { 45, 200, 120, 30, 238, 225, 81, 29, 58, 93, 200, 0, 138, 126, 113, 100 };

    private static byte[] key = new byte[]
    {
        201,  78, 108, 157, 80, 32,  13, 111, 41, 81,
        184, 155, 185, 238, 46, 104, 57,  11, 25, 44,
        112, 151,  54, 157, 97, 219, 84, 148, 47, 73,
        81,  202
    };

    #endregion

    #region Methods

    public static string Encrypt(string plainText)
    {
        // Check arguments. 
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("Argument is null or of lenght zero.");

        byte[] encrypted;
        // Create an RijndaelManaged object 
        // with the specified key and IV. 
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = key;
            rijAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

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

        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(String value)
    {
        byte[] cipherText = Convert.FromBase64String(value);

        // Check arguments. 
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("Argument is null or of lenght zero");

        // Declare the string used to hold 
        // the decrypted text. 
        string plaintext = null;

        // Create an RijndaelManaged object 
        // with the specified key and IV. 
        using (RijndaelManaged rijAlg = new RijndaelManaged())
        {
            rijAlg.Key = key;
            rijAlg.IV = IV;

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

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

    #endregion
}
