using System;
using System.Collections.Generic;
using System.Text;

namespace FileMover
{
    public class FileMoverManager
    {
        private List<FileMoverObserver> observers;
        public FileMoverManager()
        {
            observers = new List<FileMoverObserver>();
        }

        public void AddFileObserver(FileMoverObserver fileObserver)
        {
            observers.Add(fileObserver);
            fileObserver.OnFileChanged += FileObserver_OnFileChanged;
        }
        public void Start()
        {
            foreach(var fileObserver in observers)
            {
                fileObserver.StartWatcher();
            }
        }
        private void FileObserver_OnFileChanged(string filePath, string folderDestiny)
        {
            try
            {
                Utils.MoveFile(filePath, folderDestiny);
                Console.WriteLine($"Arquivo {filePath} foi movido para {folderDestiny}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um erro ao mover o arquivo. {ex.Message}.");
            }
        }
    }
}
