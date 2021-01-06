using System;

namespace xpm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string version = "v0.2-alpha";
            Console.WriteLine("xpm " + version + "\n");
            Utils.Extra.CheckInstall();
            Utils.Config.Autostart();
            if (args.Length != 0)
            {
                switch (args[0])
                {
                    case "-S":
                        if (args.Length != 1) { Utils.Package.InstallPackage(args[1], true); }
                        else { Console.WriteLine("Please provide an argument."); }
                        break;

                    case "-Sy":
                        Utils.Mirror.UpdateMirrors();
                        break;

                    case "-R":
                        if (args.Length != 1) { Utils.Package.UninstallPackage(args[1], true); }
                        else { Console.WriteLine("Please provide an argument."); }
                        break;

                    case "-Q":
                        Utils.Package.ListInstalledPackages();
                        break;

                    case "-Ss":
                        if (args.Length != 1) { Utils.Package.SearchInstallablePackages(args[1]); }
                        else { Utils.Package.ListInstallablePackages(); }
                        break;

                    case "-Syu":
                        Utils.Package.UpdateAll();
                        break;

                    case "-P":
                        if (args.Length != 1) { Utils.Extra.ParsePKG(args[1], false); }
                        else { Console.WriteLine("Please provide an argument."); }
                        break;

                    case "-Ce": /* Create env var in xpm.exe dir */
                        Console.Write(Utils.Config.messageStart + " Creating path variable\n");
                        var currentValue = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                        var newValue = currentValue + @";" + Environment.CurrentDirectory;
                        Environment.SetEnvironmentVariable("Path", newValue, EnvironmentVariableTarget.User);
                        Console.WriteLine(Utils.Config.messageStart + " Done");
                        break;

                    case "-Re": /* Remove env var */
                        Console.Write(Utils.Config.messageStart + " Removing path variable\n");
                        var currentValue2 = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User);
                        var newValue2 = currentValue2.Replace(@";" + Environment.CurrentDirectory, string.Empty);
                        Environment.SetEnvironmentVariable("Path", newValue2, EnvironmentVariableTarget.User);
                        Console.WriteLine(Utils.Config.messageStart + " Done");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid argument.");
                Environment.Exit(1);
            }
            if (Utils.Config.pauseAtEnd) { Console.ReadKey(true); }
        }
    }
}