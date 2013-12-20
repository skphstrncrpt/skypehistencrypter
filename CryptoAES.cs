using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace SkypeHistoryEnc
{
    public class Crypt
    {
        public static string Key;

        public static string Enc(string val)
        {
            return CryptoAES.Encrypt64(Key, Encoding.UTF8.GetBytes(val));
        }

        public static string DecString(string data)
        {
            return Encoding.UTF8.GetString(CryptoAES.Decrypt64(Key, data));
        }

        public static string Enc(Int64 val)
        {
            return CryptoAES.Encrypt64(Key, BitConverter.GetBytes(val));
        }

        public static Int64 DecInt(string data)
        {
            return BitConverter.ToInt64(CryptoAES.Decrypt64(Key, data), 0);
        }

        public static string Enc(DateTime date)
        {
            return Enc(date.ToBinary());
        }

        public static DateTime DecDate(string data)
        {
            return DateTime.FromBinary(DecInt(data));
        }

    }

    public class CryptoAES
    {

        public static string EncodeString(string encKey, string data)
        {
            return Encrypt64(encKey, Encoding.UTF8.GetBytes(data));
        }
        public static string DecodeString(string encKey, string data)
        {
            return Encoding.UTF8.GetString(Decrypt64(encKey, data));
        }

        public static byte[] Decrypt(string encKey, byte[] encrypt)
        {
            byte[] key = null;
            byte[] iv = null;
            GetKeyAndIV(encKey, ref key, ref iv);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor();

            return Transform(encrypt, decryptor);
        }

        public static byte[] Decrypt64(string encKey, string data)
        {
            return Decrypt(encKey, Convert.FromBase64String(data));
        }

        public static byte[] Encrypt(string encKey, byte[] decrypt)
        {
            byte[] key = null;
            byte[] iv = null;
            GetKeyAndIV(encKey, ref key, ref iv);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aes.CreateEncryptor();

            return Transform(decrypt, encryptor);
        }

        public static string Encrypt64(string encKey, byte[] data)
        {
            return Convert.ToBase64String(Encrypt(encKey, data));
        }


        private static byte[] Transform(byte[] bCrypt, ICryptoTransform oCrypt)
        {
            //declare the output variable. 
            byte[] crypt;

            //memoryStream to hold the bytes of the output.
            System.IO.MemoryStream stream = new System.IO.MemoryStream(2048);

            //cryptoStream that will be used to actually write the transformation.
            CryptoStream encryptstream = new CryptoStream(stream, oCrypt, CryptoStreamMode.Write);

            //write the input array values into the crypto stream, and transform.
            encryptstream.Write(bCrypt, 0, (int)bCrypt.Length);
            encryptstream.FlushFinalBlock();

            //get the output to return.
            stream.Position = 0;
            crypt = new byte[(int)stream.Length];
            stream.Read(crypt, 0, (int)crypt.Length);

            //close streams and destroy objects.
            encryptstream.Close();
            stream.Close();

            //cleanup 
            encryptstream = null;
            stream = null;

            return crypt;
        }

        private static void GetKeyAndIV(String pass, ref byte[] key, ref byte[] iv)
        {
            //Password
            byte[] password = Encoding.UTF8.GetBytes(pass);

            MD5 md5 = MD5.Create();
            byte[] aesKey;
            byte[] aesIv;

            //Generate key
            int preKeyLength = password.Length;
            byte[] preKey = new byte[preKeyLength];
            Buffer.BlockCopy(password, 0, preKey, 0, password.Length);
            aesKey = md5.ComputeHash(preKey);

            //Generate Iv
            int preIVLength = aesKey.Length + preKeyLength;
            byte[] preIV = new byte[preIVLength];
            Buffer.BlockCopy(aesKey, 0, preIV, 0, aesKey.Length);
            Buffer.BlockCopy(preKey, 0, preIV, aesKey.Length, preKey.Length);

            aesIv = md5.ComputeHash(preIV);

            md5.Clear();
            md5 = null;

            key = aesKey;
            iv = aesIv;
        }

    }

}
