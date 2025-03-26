using System.Diagnostics;

namespace bgpd
{
    public static class Configuration
    {
        public static bool DebugMode { get; private set; }
        public static string GameFolder { get; set; }
        public static string Locale { get; set; }
        public static IntPtr hProc { get; set; }
        public static int RefreshTimeMS { get; set; }
        public static IntPtr HWndPtr { get; set; }

        private static Dictionary<String, String> storedConfig = [];
        public static string Version => System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public static void Init(Process bgProcess)
        {
            GameFolder = bgProcess.MainModule.FileName.ToLower().Replace("\\baldur.exe", "");
            RefreshTimeMS = 300;
            Locale = "en_US";
            DebugMode = false;
            LoadConfig();
            DetectLocale();
        }

        private static void DetectLocale()
        {
            if (Directory.Exists($"{GameFolder}\\lang\\{Locale}"))
            {
                return;
            }
            Locale = "en_US";
        }

        public static void SaveConfig()
        {
            File.WriteAllLines("config.cfg",
            [
                $"Version={Version}",
                $"Locale={Locale}",
                $"RefreshTimeMs={RefreshTimeMS}",
                $"DebugMode={DebugMode}",
            ]);
        }

        private static void LoadConfig()
        {
            if (!File.Exists("config.cfg"))
            {
                Logger.Debug("No config file exits, making a new one");
                SaveConfig();
            }
            try
            {
                Logger.Debug("Reading existing config ...");
                var config = File.ReadAllLines("config.cfg");
                foreach (var line in config)
                {
                    var split = line.Split('=');
                    storedConfig[split[0]] = split[1];
                }

                var version = GetProperty("Version", Configuration.Version); ;
                Locale = GetProperty("Locale", "en_US");
                RefreshTimeMS = int.Parse(GetProperty("RefreshTimeMs", "300"));
                DebugMode = GetProperty("DebugMode", "false").Equals("true");
                if (version != Configuration.Version)
                {
                    Logger.Debug("Outdated config version found - overriding it");
                    File.Delete("config.cfg");
                    LoadConfig();
                    Logger.Debug("Done");
                }
                Logger.Info("Current config:\n" + string.Join("\n", File.ReadAllLines("config.cfg")));

            }
            catch (Exception ex)
            {
                Logger.Fatal("Load config error!", ex);
            }
        }

        private static string GetProperty(string key, string def)
        {
            if (!storedConfig.TryGetValue(key, out var value))
                return def;
            return value.Trim().ToLower();
        }
    }
}