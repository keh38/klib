using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace KLib.Crypto.PublicKey
{
    public class PublicKeyCrypto
    {
        private RSACryptoServiceProvider rsa;

        public PublicKeyCrypto(string src, bool isContainerName)
        {
            if (isContainerName)
            {
                KeyFromContainer(src);
            }
            else
            {
                KeyFromXML(src);
            }
        }

        public void KeyFromContainer(string containerName)
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            rsa = new RSACryptoServiceProvider(cp);
        }

        public void KeyFromXML(string xmlString)
        {
            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlString);
        }

        public PublicKeyCrypto(string containerName)
        {
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            rsa = new RSACryptoServiceProvider(cp);
        }

        public static void CreateKeys(string containerName)
        {
            // Create the CspParameters object and set the key container 
            // name used to store the RSA key pair.
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = containerName;

            // Create a new instance of RSACryptoServiceProvider that accesses
            // the key container MyKeyContainerName.
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cp);
        }

        public string GetPublicKey()
        {
            return rsa.ToXmlString(false);
        }

        public string Encrypt(string plaintext)
        {
            //declare a new encoder
            UTF8Encoding UTF8Encoder = new UTF8Encoding();
            //get byte representation of string
            byte[] inputBytes = UTF8Encoder.GetBytes(plaintext);

            //convert back to a string
            return Convert.ToBase64String(rsa.Encrypt(inputBytes, false));
        }

        public string Decrypt(string ciphertext)
        {
            byte[] inputBytes = Convert.FromBase64String(ciphertext);

            //convert back to a string
            return Encoding.UTF8.GetString(rsa.Decrypt(inputBytes, false));
        }
    }
}
