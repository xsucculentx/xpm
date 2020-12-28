using System;

namespace xpm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string version = "v0.1-alpha";
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