using FileMover.FileManager.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileMover
{
    public class FileMoverObserver
    {
        public delegate void FileChanged(string filePath, string folderDestiny);
        public event FileChanged OnFileChanged;

        private string folderRootPath;
        private string folderDestiny;
        private List<string> filesToWatch;
        private List<string> extensionsToWatch;
        private FileSystemWatcher fileWatcher;

        private Dictionary<string, DateTime> fileLastChanged;
        public FileMoverObserver(string folderRootPath, string folderDestinyPath)
        {
            this.fileLastChanged = new Dictionary<string, DateTime>();
            this.folderRootPath = folderRootPath;
            this.folderDestiny = folderDestinyPath;
            filesToWatch = new List<string>();
            extensionsToWatch = new List<string>();
        }

        public void StartWatcher(bool includeSubdirectories = true)
        {
            fileWatcher = new FileSystemWatcher(folderRootPath);
            fileWatcher.NotifyFilter = NotifyFilters.LastAccess;
            fileWatcher.Changed += FileWatcher_Changed;
            fileWatcher.IncludeSubdirectories = includeSubdirectories;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                HandleEventFileChanged(e.FullPath, e.Name);
            }
            catch (FloodException ex)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocorreu um problema. {ex.Message}");
            }
        }

        public void AddFilesObserver(params string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                if (!fileName.StartsWith(folderRootPath))
                    filesToWatch.Add($"{Path.Combine(folderRootPath, fileName)}");
                else
                    filesToWatch.Add(fileName);
            }
        }

        public void AddExtensionObserver(params string[] extensions)
        {
            foreach(string extension in extensions)
            {
                if (!extension.StartsWith("."))
                    extensionsToWatch.Add($".{extension}");
                else
                    extensionsToWatch.Add(extension);
            }
        }

        private void HandleEventFileChanged(string filePath, string name)
        {
            string fileName = Path.GetFileName(filePath);
            string extension = Path.GetExtension(filePath);
            if (filesToWatch.Contains(filePath))
            {
                DispatchEventFileChanged(filePath);
            }
            else if (extensionsToWatch.Contains(extension))
            {
                if (filePath == Path.Combine(folderRootPath, fileName))
                {
                    DispatchEventFileChanged(filePath);
                }
            }
        }
        private void DispatchEventFileChanged(string filePath)
        {
            PreventFlood(filePath);
            OnFileChanged(filePath, this.folderDestiny);
        }

        private void PreventFlood(string filePath)
        {
            lock (fileLastChanged)
            {
                var now = DateTime.Now;
                if (fileLastChanged.ContainsKey(filePath))
                {
                    var diferencaSegundos = (now - fileLastChanged[filePath]).TotalSeconds;
                    if (diferencaSegundos < 1)
                    {
                        fileLastChanged[filePath] = DateTime.Now;
                        throw new FloodException();
                    }
                    fileLastChanged[filePath] = DateTime.Now;
                }
                else
                {
                    fileLastChanged.Add(filePath, DateTime.Now);
                }
            }
        }
    }
}
