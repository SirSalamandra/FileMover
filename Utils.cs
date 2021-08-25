using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileMover
{
    public class Utils
    {
        public static void MoveFile(string filePath, string destinyPath)
        {
            string fileName = Path.GetFileName(filePath);
            File.Copy(Path.Combine(filePath), Path.Combine(destinyPath, fileName), true);
        }
    }
}
