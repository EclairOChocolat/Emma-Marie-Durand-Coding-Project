using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DURAND_Emma_Marie_Coding_Project
{
    using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            byte[] iv = GenerateRandomIV(16);
            Table(iv);
        }

        static void Table(byte[] iv)
        {
            string choice = "";
            do
            {
                do
                {
                    Console.WriteLine("----------------------------------------------------------\n" +
                                        "Hello, what do you want to do ? :\n" +
                                           "\t1. Encrypt a text\n" +
                                           "\t2. Decrypt a text\n" +
                                           "\t3. End\n");
                    choice = Console.ReadLine();
                } while (choice != "1" && choice != "2" && choice != "3");

                switch (choice)
                {
                    case "1":
                        Console.Write("Write the text you want to encrypt :\n\t");
                        string textToEncrypt = Console.ReadLine();
                        string k;
                        do
                        {
                            Console.Write("Write the key (16 characters) :\n\t");
                            k = Console.ReadLine();

                        } while (k.Length != 16);

                        byte[] encryptedData = Encrypt(textToEncrypt, Encoding.UTF8.GetBytes(k), iv);
                        Console.WriteLine("\nOriginal text :\t" + textToEncrypt);
                        Console.WriteLine("Encrypt text :\t" + Convert.ToBase64String(encryptedData));
                        Console.WriteLine("IV :\t" + Encoding.UTF8.GetString(iv));
                        Console.WriteLine("\n----------------------------------------------------------");
                        break;

                    case "2":
                        Console.Write("Write the text you want to decrypt :\n\t");
                        string textToDecrypt = Console.ReadLine();
                        string key;
                        do
                        {
                            Console.Write("Write the key (16 characters):\n\t");
                            key = Console.ReadLine();

                        } while (key.Length != 16);

                        string decryptData = Decrypt(Convert.FromBase64String(textToDecrypt), Encoding.UTF8.GetBytes(key), iv);
                        Console.WriteLine("\nEncrypt text :\t" + textToDecrypt);
                        Console.WriteLine("Decrypt text :\t" + decryptData);
                        Console.WriteLine("\n----------------------------------------------------------");
                        break;

                    default:
                        break;
                }
            } while (choice != "3");
        }

        static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        static byte[] GenerateRandomIV(int size)
        {
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[size];
                rngCsp.GetBytes(randomBytes);
                Console.WriteLine("\nAn Initial Vector as been generate randomly\n");
                return randomBytes;
            }
        }

        static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
