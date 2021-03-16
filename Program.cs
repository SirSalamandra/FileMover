using System;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace FileMover
{
    class Program
    {
        static byte[] hash = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<string> files = new List<string>();

            files.Add(""); //arquivos que serão movidos

            string targetPath = ""; //caminho que sera salvo

            while (true)
            {
                foreach (string file in files)
                {
                    if (FileChanged(file))
                    {
                        Console.WriteLine($"{DateTime.Now} - Mudou");

                        MoveFile(file, targetPath);
                    }
                }

                Thread.Sleep(100);
            }

        }

        static private bool FileChanged(string filePath)
        {
            MD5 sha1 = MD5.Create();
            try
            {
                if (File.Exists(filePath))
                {

                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] newHash = sha1.ComputeHash(stream);

                        if (hash != null && hash.SequenceEqual(newHash))
                        {
                            return false;
                        }

                        hash = newHash;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

            return false;
        }

        static private void MoveFile(string filePath, string fileDestiny)
        {
            string fileName = filePath.Substring(filePath.LastIndexOf("\\") + 1);
            File.Copy(Path.Combine(filePath), Path.Combine(fileDestiny, fileName), true);
        }
    }
}
