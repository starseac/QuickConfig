using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace QuickConfig
{
   public class setBAT
    {

      public static void RunBat(string batPath)
       {

           Process pro = new Process();

           FileInfo file = new FileInfo(batPath);

           pro.StartInfo.WorkingDirectory = file.Directory.FullName;

           pro.StartInfo.FileName = batPath;

           pro.StartInfo.CreateNoWindow = false;

           pro.Start();

           pro.WaitForExit();

       }
    }
}
