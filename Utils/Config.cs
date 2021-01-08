using System;
using System.IO;

namespace xpm.Utils
{
    internal class Config
    {
        public static string[] mirrors = GetMirrors();

        public static string[] GetMirrors()
        {
            if (File.Exists(installFolder + @"\mirrors.txt")) { return File.ReadAllLines(installFolder + @"\mirrors.txt"); }
            else { return new string[] { "main;;;https://xpm.pagekite.me/main.txt" }; }
        }

        public static bool pauseAtEnd = false;
        public static string installFolder = @"C:\Users\" + Environment.UserName + @"\xpm";

        public static string messageStart = ">>"; /* >> This shows when a message is displayed. */
        public static char downloadTail = '>'; /* [####>-----] */

        public static void Autostart() /* do this when program starts */
        {
        }
    }
}