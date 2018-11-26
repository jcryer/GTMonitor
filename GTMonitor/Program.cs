using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace GTMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] webhooks = File.ReadAllLines("webhooks.txt");
            Regex r = new Regex("<body.+/body>", RegexOptions.IgnoreCase);
            while (true)
            {
                using (WebClient wc = new WebClient())
                {
                    string response = wc.DownloadString("https://github.com/");
                    string responseRegex = response.Split(new string[] { "<body" }, StringSplitOptions.None)[1].Split(new string[] { "/body>" }, StringSplitOptions.None)[0];

                    if (File.Exists("local.html"))
                    {
                        string file = File.ReadAllText("local.html");

                        if (file != responseRegex)
                        {
                            Console.WriteLine("Change!");

                        //    RunWebhooks(webhooks);
                            File.WriteAllText("local.html", responseRegex);
                        }
                    }
                    else
                    {
                        File.WriteAllText("local.html", responseRegex);
                    }
                }
                Console.WriteLine("Done!");
                Thread.Sleep(2000);
            }
        }
        static void RunWebhooks(string[] channels)
        {
            foreach (string s in channels)
            {
                using (WebClient wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    string response = wc.UploadString(s, File.ReadAllText("test.txt"));
                    Console.WriteLine(response);
                }
            }
        }
    }
}