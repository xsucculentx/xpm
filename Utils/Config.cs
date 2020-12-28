using System;

namespace xpm.Utils
{
    internal class Config
    {
        public static string[] mirrors =
            { /* mirrorname;;;url-to-mirror */
              "main.txt;;;https://xpm.pagekite.me/main.txt",
              "extra.txt;;;https://xpm.pagekite.me/extra.txt"
            };

        public static bool pauseAtEnd = false;
        public static string installFolder = @"C:\Users\" + Environment.UserName + @"\xpm";

        public static string messageStart = ">>"; /* :: This shows when a message is displayed. */

        public static void Autostart() /* do this when program starts */
        {
        }
    }
}