﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace PlenBotLogUploader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var currProcess = Process.GetCurrentProcess();
            var otherProcesses = Process.GetProcessesByName("PlenBotLogUploader")
                .Where(anon => !anon.Id.Equals(currProcess.Id))
                .ToList();
            var args = Environment.GetCommandLineArgs().ToList();
            var localDir = $"{Path.GetDirectoryName(Application.ExecutablePath.Replace('/', '\\'))}\\";
            if (args.Count == 3)
            {
                if(args[1].ToLower() == "-update")
                {
                    if (otherProcesses.Count == 0)
                    {
                        File.Copy(Application.ExecutablePath.Replace('/', '\\'), localDir + $"{args[2]}", true);
                        Process.Start(localDir + args[2], "-finishupdate");
                        return;
                    }
                    else
                    {
                        foreach (var process in otherProcesses)
                        {
                            process.Kill();
                        }
                        Thread.Sleep(250);
                        File.Copy(Application.ExecutablePath.Replace('/', '\\'), localDir + args[2], true);
                        Process.Start(localDir + args[2], "-finishupdate");
                        return;
                    }
                }
            }
            if (args.Count == 2)
            {
                if (args[1].ToLower() == "-finishupdate")
                {
                    File.Delete(localDir + "PlenBotLogUploader_Update.exe");
                }
            }
            if (otherProcesses.Count == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
        }
    }
}
