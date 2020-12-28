using System;
using System.Net;

namespace xpm.Utils
{
    internal class Mirror
    {
        public static void UpdateMirrors()
        {
            Console.WriteLine(Config.messageStart + " Updating Mirrors");
            foreach (string mirror in Config.mirrors)
            {
                string[] mirrorSplit = mirror.Split(new string[] { ";;;" }, StringSplitOptions.None);
                var client = new WebClient();
                Console.Write("\r     Updating " + mirrorSplit[0]);
                client.DownloadFile(mirrorSplit[1], Config.installFolder + "/mirrors/" + mirrorSplit[0]);
                Console.Write("                                                   ");
                Console.Write("\r     Updated " + mirrorSplit[0] + ";\n");
            }
        }
    }
}