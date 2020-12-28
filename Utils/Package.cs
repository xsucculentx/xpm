using System;
using System.IO;
using System.Net;

namespace xpm.Utils
{
    internal class Package
    {
        public static void UninstallPackage(string query, bool askYN)
        {
            if (Directory.Exists(Config.installFolder + @"\packages\" + query))
            {
                if (askYN)
                {
                    Console.Write(Config.messageStart + " Package found, would you like to uninstall? (y/n): ");
                    ConsoleKeyInfo keypress = Console.ReadKey(true);
                    Console.WriteLine(keypress.KeyChar);
                    if (keypress.KeyChar == 'y')
                    {
                        Console.WriteLine("     Removing program files");
                        Directory.Delete(Config.installFolder + @"\packages\" + query, true);
                        Console.WriteLine("     Removing start menu shortcut");
                        File.Delete(@"C:\Users\user\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\" + query + ".lnk");
                        Console.WriteLine("     Removing path variable");
                        var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                        var newValue = currentValue
                            .Replace(";" + Config.installFolder + @"\packages\" + query + @"\bin\", string.Empty)
                            .Replace(";" + Config.installFolder + @"\packages\" + query, string.Empty);
                        Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                        Console.WriteLine(Config.messageStart + " Done");
                    }
                    else { Environment.Exit(1); Console.WriteLine("Operation cancelled."); }
                }
                else
                {
                    Console.WriteLine(Config.messageStart + " Uninstalling package");
                    Console.WriteLine("     Removing program files");
                    Directory.Delete(Config.installFolder + @"\packages\" + query, true);
                    Console.WriteLine("     Removing start menu shortcut");
                    File.Delete(@"C:\Users\user\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\" + query + ".lnk");
                    Console.WriteLine("     Removing path variable");
                    var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                    var newValue = currentValue
                        .Replace(";" + Config.installFolder + @"\packages\" + query + @"\bin\", string.Empty)
                        .Replace(";" + Config.installFolder + @"\packages\" + query, string.Empty);
                    Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                    Console.WriteLine("     Done");
                }
            }
        }

        public static void InstallPackage(string query, bool askYN)
        {
            Utils.Mirror.UpdateMirrors();
            string[] mirrors = Directory.GetFiles(Config.installFolder + @"\mirrors");
            foreach (string mirror in mirrors)
            {
                string[] mirrorContent = File.ReadAllLines(mirror);
                foreach (string line in mirrorContent)
                {
                    string[] lineSplit = line.Split(new string[] { ";;;" }, StringSplitOptions.None);
                    if (lineSplit[0] == query)
                    {
                        var client = new WebClient();
                        client.DownloadFile(lineSplit[1], Config.installFolder + @"\cache\" + lineSplit[0] + ".pkg");
                        //Config.installFolder + @"\cache\" + lineSplit[1] + ".pkg"
                        Extra.ParsePKG(Config.installFolder + @"\cache\" + lineSplit[0] + ".pkg", true);
                        Console.WriteLine(Config.messageStart + " Package Details");
                        Console.WriteLine("     Name: " + lineSplit[0]);
                        Console.WriteLine("     Made by: " + Utils.Extra.METAAUTHOR);
                        Console.WriteLine("     Desc: " + Extra.METADESC);
                        Console.WriteLine("     License: " + Extra.METALICENSE);
                        Console.Write(Config.messageStart + " Would you like to proceed? (y/n): ");
                        if (askYN)
                        {
                            ConsoleKeyInfo keypress = Console.ReadKey(true);
                            Console.WriteLine(keypress.KeyChar);
                            if (keypress.KeyChar == 'y')
                            {
                                Extra.ParsePKG(Config.installFolder + @"\cache\" + lineSplit[0] + ".pkg", false);
                            }
                            else { Console.WriteLine("Operation cancelled."); Environment.Exit(1); }
                        }
                        else { Extra.ParsePKG(Config.installFolder + @"\cache\" + lineSplit[0] + ".pkg", false); }
                    }
                }
            }
            // CommitPackage(Extra.METANAME, Extra.METAAUTHOR);
        }

        public static void CommitPackage(string name, string author)
        {
            if (!File.Exists(Config.installFolder + @"\packages.txt"))
            {
                File.WriteAllText(Config.installFolder + @"\packages.txt", ""); // Creates file without locking it with a fstream;
            }
            File.AppendAllText(Config.installFolder + @"\packages.txt", "\n" + name + ";" + author);
        }

        public static void ListInstalledPackages()
        {
            string[] packages = Directory.GetDirectories(Config.installFolder + @"\packages\");
            Console.Write(Config.messageStart + " Listing all installed packages\n");
            foreach (string package in packages)
            {
                string packageTrimmed = package.Replace(Config.installFolder + @"\packages\", string.Empty);
                Console.WriteLine("     " + packageTrimmed);
            }
        }

        public static void ListInstallablePackages()
        {
            Console.Write(Config.messageStart + " Listing available packages\n");
            string[] mirrors = Directory.GetFiles(Config.installFolder + @"\mirrors\");
            foreach (string mirror in mirrors)
            {
                string[] mirrorContent = File.ReadAllLines(mirror);
                foreach (string line in mirrorContent)
                {
                    Console.WriteLine("     " + line.Split(new string[] { ";;;" }, StringSplitOptions.None)[0]);
                }
            }
        }

        public static void SearchInstallablePackages(string query)
        {
            Console.Write(Config.messageStart + " Listing available packages\n");
            string[] mirrors = Directory.GetFiles(Config.installFolder + @"\mirrors\");
            foreach (string mirror in mirrors)
            {
                string[] mirrorContent = File.ReadAllLines(mirror);
                foreach (string line in mirrorContent)
                {
                    if (line.Split(new string[] { ";;;" }, StringSplitOptions.None)[0] == query)
                    {
                        Console.WriteLine("     " + line.Split(new string[] { ";;;" }, StringSplitOptions.None)[0]);
                    }
                }
            }
        }

        public static void UpdateAll()
        {
            string[] packages = Directory.GetDirectories(Config.installFolder + @"\packages\");
            foreach (string package in packages)
            {
                UninstallPackage(package.Replace(Config.installFolder + @"\packages\", string.Empty), false);
                InstallPackage(package.Replace(Config.installFolder + @"\packages\", string.Empty), false);
            }
        }
    }
}