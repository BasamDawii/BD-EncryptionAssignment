using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionAssignment
{
    public class HandleEncryption
    {
        private string passphrase;
        private byte[] salt;

        public HandleEncryption(string passphrase)
        {
            if (passphrase == null || (passphrase.Length != 16 && passphrase.Length != 24 && passphrase.Length != 32))
            {
                throw new ArgumentException("Passphrase must be 16, 24, or 32 characters long.");
            }

            this.passphrase = passphrase;
            this.salt = GenerateSalt();
        }

        public void EncryptAndSave(string message, string filePath = "encrypted.txt")
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GenerateKeyFromPassphrase(passphrase, salt);
                aesAlg.IV = new byte[16];

                using (ICryptoTransform encryptor = aesAlg.CreateEncryptor())
                {
                    using (FileStream fsEncrypt = new FileStream(filePath, FileMode.Create))
                    using (CryptoStream csEncrypt = new CryptoStream(fsEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(message);
                    }
                }
            }
        }

        public string ReadAndDecrypt(string filePath)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = GenerateKeyFromPassphrase(passphrase, salt);
                aesAlg.IV = new byte[16];

                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor())
                {
                    using (FileStream fsDecrypt = new FileStream(filePath, FileMode.Open))
                    using (CryptoStream csDecrypt = new CryptoStream(fsDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(salt);
            }
            return salt;
        }

        private byte[] GenerateKeyFromPassphrase(string passphrase, byte[] salt)
        {
            using (Rfc2898DeriveBytes keyDerivationFunction = new Rfc2898DeriveBytes(passphrase, salt, 10000))
            {
                return keyDerivationFunction.GetBytes(32); // 32 bytes = 256 bits for AES
            }
        }
    }
}
