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
        static void Main(string[] args)
        {
            FileMoverObserver observerVmcToBios = new FileMoverObserver(@"C:\Workspace\Piranha\ColibriCurrent\colibri-vmc\dist", @"C:\Workspace\Piranha\ColibriCurrent\colibri-bios\lib");
            observerVmcToBios.AddExtensionObserver(".js");
            FileMoverManager manager = new FileMoverManager();
  
            manager.AddFileObserver(observerVmcToBios);
            manager.Start();

            Console.WriteLine("Pressione qualquer tecla para finalizar");
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
