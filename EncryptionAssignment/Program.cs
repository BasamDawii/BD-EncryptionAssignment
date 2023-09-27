using EncryptionAssignment;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the passphrase (16, 24, or 32 characters): ");
        string passphrase = Console.ReadLine();

        // Validate the passphrase length here if needed.
        if (passphrase == null || (passphrase.Length != 16 && passphrase.Length != 24 && passphrase.Length != 32))
        {
            Console.WriteLine("Passphrase must be 16, 24, or 32 characters long.");
            return;
        }

        HandleEncryption encryptor = new HandleEncryption(passphrase);

        while (true)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Encrypt and Save Message");
            Console.WriteLine("2. Read and Decrypt Message");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        Console.Write("Enter the message to encrypt: ");
                        string messageToEncrypt = Console.ReadLine();
                        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "encrypted.txt");
                        try
                        {
                            encryptor.EncryptAndSave(messageToEncrypt);
                            Console.WriteLine("Message encrypted and saved successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;

                    case 2:
                        Console.Write("Enter the file path to read and decrypt: ");
                        string decryptFilePath = Console.ReadLine();
                        try
                        {
                            string decryptedMessage = encryptor.ReadAndDecrypt(decryptFilePath);
                            if (decryptedMessage != null)
                            {
                                Console.WriteLine("Decrypted message:");
                                Console.WriteLine(decryptedMessage);
                            }
                            else
                            {
                                Console.WriteLine("Decryption failed or file not found.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        break;


                    case 0:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }
    }
}
