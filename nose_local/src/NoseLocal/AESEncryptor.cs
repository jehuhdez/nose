using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Nose.Core
{
    internal class AESEncryptor : IEncryptor
    {
        private readonly byte[] INIT_VECTOR;

        public AESEncryptor()
        {
            INIT_VECTOR = Encoding.UTF8.GetBytes("pepepecaspicapap");
        }

        public byte[] encrypt(byte[] pText, string pKey)
        {
            byte[] encText;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = Encoding.UTF8.GetBytes(pKey);
                aes.IV = INIT_VECTOR;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs =
                          new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(pText, 0, pText.Length);
                    }

                    encText = ms.ToArray();
                }
            }

            return encText;
        }

        public byte[] decrypt(byte [] pText, string pKey)
        {
            byte[] rawText;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = 128;
                aes.Key = Encoding.UTF8.GetBytes(pKey);
                aes.IV = INIT_VECTOR;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs =
                          new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(pText, 0, pText.Length);
                    }

                    rawText = ms.ToArray();
                }
            }

            return rawText;
        }
    }
}