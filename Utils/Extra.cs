using IWshRuntimeLibrary;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using File = System.IO.File;

namespace xpm.Utils
{
    internal class Extra
    {
        public static string BUILD = "";
        public static string METANAME = "";
        public static string METAAUTHOR = "";
        public static string METADESC = "";
        public static string METALICENSE = "";
        public static string HOOKS = "";

        private static string currentFileDownloading = "";
        private static string progressBar = " [--------------------] ";
        private static bool downloadComplete = false;

        public static void CheckInstall()
        {
            if (!Directory.Exists(Config.installFolder))
            {
                Directory.CreateDirectory(Config.installFolder);
                Directory.CreateDirectory(Config.installFolder + @"\mirrors");
                Directory.CreateDirectory(Config.installFolder + @"\cache");
                Directory.CreateDirectory(Config.installFolder + @"\packages");
            }
        }

        public static void ParsePKG(string file, bool gatherInfo)
        {
            downloadComplete = false;

            BUILD = "";
            METANAME = "";
            METAAUTHOR = "";
            METADESC = "";
            METALICENSE = "";
            HOOKS = "";

            progressBar = " [--------------------] ";

            try
            {
                string[] fileData = File.ReadAllLines(file);
                foreach (string line in fileData)
                {
                    if (!line.StartsWith("#"))
                    {
                        if (line.StartsWith("BUILD="))
                        {
                            BUILD = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("META-NAME="))
                        {
                            METANAME = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("META-AUTHOR="))
                        {
                            METAAUTHOR = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("META-DESC="))
                        {
                            METADESC = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("META-LICENSE="))
                        {
                            METALICENSE = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("HOOKS="))
                        {
                            HOOKS = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                        }
                        if (line.StartsWith("ADD-ENV-VAR="))
                        {
                            var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                            var newValue = currentValue + @";" + line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1).Replace(@".\", Config.installFolder + @"\packages\" + METANAME + @"\");
                            Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                        }
                    }
                }
                if (!gatherInfo)
                {
                    currentFileDownloading = METANAME;
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += ProgressBar;
                        client.DownloadFileAsync(new System.Uri(BUILD), Config.installFolder + @"\cache\" + METANAME + ".zip");
                    }
                    while (!downloadComplete)
                    {
                        if (downloadComplete)
                        {
                            break;
                        }
                    }
                    Console.Write("\r     (Download complete)                                  \n");
                    Console.WriteLine("\n" + Config.messageStart + " Running hooks & cleaning up");
                    if (File.Exists(Config.installFolder + @"\packages\" + METANAME + ".zip")) { File.Delete(Config.installFolder + @"\packages\" + METANAME + ".zip"); }
                    if (Directory.Exists(Config.installFolder + @"\packages\" + METANAME)) { Directory.Delete(Config.installFolder + @"\packages\" + METANAME, true); }
                    File.Move(Config.installFolder + @"\cache\" + METANAME + ".zip", Config.installFolder + @"\packages\" + METANAME + ".zip");
                    Directory.CreateDirectory(Config.installFolder + @"\packages\" + METANAME);
                    ZipFile.ExtractToDirectory(Config.installFolder + @"\packages\" + METANAME + ".zip", Config.installFolder + @"\packages\" + METANAME);
                    File.Delete(Config.installFolder + @"\cache\" + METANAME + ".pkg");
                    File.Delete(Config.installFolder + @"\packages\" + METANAME + ".zip");
                    if (!HOOKS.Contains("skipstartmenu"))
                    {
                        CreateShortcut(METANAME, @"C:\Users\user\AppData\Roaming\Microsoft\Windows\Start Menu\Programs", Config.installFolder + @"\packages\" + METANAME + @"\" + METANAME + ".exe", METADESC);
                    }

                    Console.WriteLine("\r" + Config.messageStart + " Done");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex);
            }
        }

        private static void ProgressBar(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage != 100)
            {
                if (e.ProgressPercentage == 5) { progressBar = " [#-------------------] "; }
                if (e.ProgressPercentage == 10) { progressBar = " [##------------------] "; }
                if (e.ProgressPercentage == 15) { progressBar = " [###-----------------] "; }
                if (e.ProgressPercentage == 20) { progressBar = " [####----------------] "; }
                if (e.ProgressPercentage == 25) { progressBar = " [#####---------------] "; }
                if (e.ProgressPercentage == 30) { progressBar = " [######--------------] "; }
                if (e.ProgressPercentage == 35) { progressBar = " [#######-------------] "; }
                if (e.ProgressPercentage == 40) { progressBar = " [########------------] "; }
                if (e.ProgressPercentage == 45) { progressBar = " [#########-----------] "; }
                if (e.ProgressPercentage == 50) { progressBar = " [##########----------] "; }
                if (e.ProgressPercentage == 55) { progressBar = " [###########---------] "; }
                if (e.ProgressPercentage == 60) { progressBar = " [############--------] "; }
                if (e.ProgressPercentage == 65) { progressBar = " [#############-------] "; }
                if (e.ProgressPercentage == 70) { progressBar = " [##############------] "; }
                if (e.ProgressPercentage == 75) { progressBar = " [###############-----] "; }
                if (e.ProgressPercentage == 80) { progressBar = " [################----] "; }
                if (e.ProgressPercentage == 85) { progressBar = " [#################---] "; }
                if (e.ProgressPercentage == 90) { progressBar = " [##################--] "; }
                if (e.ProgressPercentage == 95) { progressBar = " [####################] "; }
                string fullProgressBar = "\r    " + progressBar + e.ProgressPercentage + "% " + currentFileDownloading;
                Console.Write("\r" + fullProgressBar);
            }
            else
            {
                if (e.ProgressPercentage == 100)
                {
                    progressBar = " [####################] ";
                    downloadComplete = true;
                    string fullProgressBar = "\r    " + progressBar + e.ProgressPercentage + "% " + currentFileDownloading + "\n";
                }
            }
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation, string Desc)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = Desc;
            shortcut.TargetPath = targetFileLocation;
            shortcut.Save();
        }
    }
}