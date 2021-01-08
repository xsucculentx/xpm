using IWshRuntimeLibrary;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using File = System.IO.File;

namespace xpm.Utils
{
    internal class Extra
    {
        public static string BUILD = "";
        public static string NAME = "";
        public static string AUTHOR = "";
        public static string DESC = "";
        public static string MAINTAINER = "";
        public static double VERSION = 0.0;
        public static string LICENSE = "";
        public static string HOOKS = "";
        public static string MAINEXE = "";

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

        public static void ParsePKG(string file, bool gatherInfo) /* REMAKE WITH JSON METADATA */
        {
            downloadComplete = false;

            BUILD = "";
            NAME = "";
            AUTHOR = "";
            MAINTAINER = "";
            DESC = "";
            VERSION = 0.0;
            LICENSE = "";
            HOOKS = "";
            MAINEXE = "";

            progressBar = " [--------------------] ";

            try
            {
                string fileData = File.ReadAllText(file);
                PackageMeta pkg = JsonConvert.DeserializeObject<PackageMeta>(fileData);
                BUILD = pkg.Build;
                NAME = pkg.Name;
                AUTHOR = pkg.Author;
                MAINTAINER = pkg.Maintainer;
                DESC = pkg.Description;
                VERSION = pkg.Version;
                LICENSE = pkg.License;
                HOOKS = pkg.Hooks;
                MAINEXE = pkg.MainEXE;
                if (!gatherInfo)
                {
                    currentFileDownloading = NAME;
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += ProgressBar;
                        client.DownloadFileAsync(new System.Uri(BUILD), Config.installFolder + @"\cache\" + NAME + ".zip");
                    }
                    while (!downloadComplete)
                    {
                        if (downloadComplete)
                        {
                            break;
                        }
                    }
                    Console.Write("\r     (Download complete)                                  \n");
                    Thread.Sleep(500);
                    Console.WriteLine("\n" + Config.messageStart + " Running hooks & cleaning up");
                    if (File.Exists(Config.installFolder + @"\packages\" + NAME + ".zip")) { File.Delete(Config.installFolder + @"\packages\" + NAME + ".zip"); }
                    if (Directory.Exists(Config.installFolder + @"\packages\" + NAME)) { Directory.Delete(Config.installFolder + @"\packages\" + NAME, true); }
                    File.Move(Config.installFolder + @"\cache\" + NAME + ".zip", Config.installFolder + @"\packages\" + NAME + ".zip");
                    Directory.CreateDirectory(Config.installFolder + @"\packages\" + NAME);
                    ZipFile.ExtractToDirectory(Config.installFolder + @"\packages\" + NAME + ".zip", Config.installFolder + @"\packages\" + NAME);
                    File.Delete(Config.installFolder + @"\cache\" + NAME + ".pkg");
                    File.Delete(Config.installFolder + @"\packages\" + NAME + ".zip");
                    if (HOOKS.Contains("path-var"))
                    {
                        var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                        var newValue = currentValue + @";" + HOOKS.Substring(HOOKS.IndexOf('(') + 1, HOOKS.IndexOf(')') - HOOKS.IndexOf('(') - 1).Replace(@".\", Config.installFolder + @"\packages\" + NAME + @"\");
                        Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
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
                if (e.ProgressPercentage == 5) { progressBar = " [" + Config.downloadTail + "-------------------] "; }
                if (e.ProgressPercentage == 10) { progressBar = " [#" + Config.downloadTail + "------------------] "; }
                if (e.ProgressPercentage == 15) { progressBar = " [##" + Config.downloadTail + "-----------------] "; }
                if (e.ProgressPercentage == 20) { progressBar = " [###" + Config.downloadTail + "----------------] "; }
                if (e.ProgressPercentage == 25) { progressBar = " [####" + Config.downloadTail + "---------------] "; }
                if (e.ProgressPercentage == 30) { progressBar = " [#####" + Config.downloadTail + "--------------] "; }
                if (e.ProgressPercentage == 35) { progressBar = " [######" + Config.downloadTail + "-------------] "; }
                if (e.ProgressPercentage == 40) { progressBar = " [#######" + Config.downloadTail + "------------] "; }
                if (e.ProgressPercentage == 45) { progressBar = " [########" + Config.downloadTail + "-----------] "; }
                if (e.ProgressPercentage == 50) { progressBar = " [#########" + Config.downloadTail + "----------] "; }
                if (e.ProgressPercentage == 55) { progressBar = " [##########" + Config.downloadTail + "---------] "; }
                if (e.ProgressPercentage == 60) { progressBar = " [###########" + Config.downloadTail + "--------] "; }
                if (e.ProgressPercentage == 65) { progressBar = " [############" + Config.downloadTail + "-------] "; }
                if (e.ProgressPercentage == 70) { progressBar = " [#############" + Config.downloadTail + "------] "; }
                if (e.ProgressPercentage == 75) { progressBar = " [##############" + Config.downloadTail + "-----] "; }
                if (e.ProgressPercentage == 80) { progressBar = " [###############" + Config.downloadTail + "----] "; }
                if (e.ProgressPercentage == 85) { progressBar = " [################" + Config.downloadTail + "---] "; }
                if (e.ProgressPercentage == 90) { progressBar = " [#################" + Config.downloadTail + "--] "; }
                if (e.ProgressPercentage == 95) { progressBar = " [###################-] "; }
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

    public class PackageMeta
    {
        public string Build { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Maintainer { get; set; }
        public string Description { get; set; }
        public double Version { get; set; }
        public string License { get; set; }
        public string Hooks { get; set; }
        public string MainEXE { get; set; }
    }
}