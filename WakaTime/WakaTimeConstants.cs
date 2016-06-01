﻿using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace WakaTime
{
    internal static class WakaTimeConstants
    {
        internal const string NativeName = "iTimeTrack";
        internal const string PluginName = "itimetrack-notepadpp";
        internal static string PluginVersion = string.Format("{0}.{1}.{2}", WakaTime.CoreAssembly.Version.Major, WakaTime.CoreAssembly.Version.Minor, WakaTime.CoreAssembly.Version.Build);
        internal const string EditorName = "notepadpp";
        internal static string EditorVersion
        {
            get
            {
                var ver = (int)Win32.SendMessage(PluginBase.NppData._nppHandle, NppMsg.NPPM_GETNPPVERSION, 0, 0);
                return ver.ToString();
            }
        }

        //internal const string CliUrl = "https://github.com/itimetrack/itimetrack/archive/master.zip";
        internal const string CliUrl = "https://github.com/itimetrack/itimetrack/archive/master.zip";
        internal const string CliFolder = @"itimetrack-master\wakatime\cli.py";

        internal static string PluginConfigDir
        {
            get
            {
                var pluginConfigDir = new StringBuilder(Win32.MAX_PATH);
                Win32.SendMessage(PluginBase.NppData._nppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, pluginConfigDir);
                return pluginConfigDir.ToString();
            }
        }

        internal static Func<string> LatestWakaTimeCliVersion = () =>
        {
            var regex = new Regex(@"(__version_info__ = )(\(( ?\'[0-9]\'\,?){3}\))");

            var client = new WebClient { Proxy = WakaTime.GetProxy() };

            try
            {
                var about = client.DownloadString("https://raw.githubusercontent.com/itimetrack/itimetrack/master/wakatime/__about__.py");
                var match = regex.Match(about);

                if (match.Success)
                {
                    var grp1 = match.Groups[2];
                    var regexVersion = new Regex("([0-9])");
                    var match2 = regexVersion.Matches(grp1.Value);
                    return string.Format("{0}.{1}.{2}", match2[0].Value, match2[1].Value, match2[2].Value);
                }
                else
                {
                    Logger.Warning("Couldn't auto resolve iTimeTrack cli version");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception when checking current iTimeTrack cli version: ", ex);
            }
            return string.Empty;
        };
    }
}
