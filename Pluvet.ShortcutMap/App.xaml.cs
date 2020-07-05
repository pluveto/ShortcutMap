using Newtonsoft.Json;
using Pluvet.ShortcutMap.Entities;
using Pluvet.ShortcutMap.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Pluvet.ShortcutMap
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static List<ShortcutMapEntity> Maps { get; private set; }
        public App()
        {
            App.LoadMaps();
            CheckUpdate();
        }

        private async void CheckUpdate()
        {
            Updater.GitHubRepo = "/pluveto/ShortcutMap";

            await Task.Run(new Action(() =>
            {
                if (Updater.HasUpdate)
                {
                    var ret = MessageBox.Show("Shortcut Map 检查到新版本，是否更新？", "提示", MessageBoxButton.YesNo);
                    if(ret == MessageBoxResult.Yes)
                    {
                        Process.Start("https://github.com/pluveto/ShortcutMap/releases/latest");
                    }
                }
            }));
        }

        private static void LoadMaps()
        {
            Maps = new List<ShortcutMapEntity>();
            var mapDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/shortcuts");
            var mapJsons = mapDir.GetFiles("*.json");

            foreach (var item in mapJsons)
            {
                var jsonText = File.ReadAllText(item.FullName);
                var newMap = JsonConvert.DeserializeObject<ShortcutMapEntity>(jsonText);

                if (null == newMap) continue;
                var iconPath = System.AppDomain.CurrentDomain.BaseDirectory + $"/shortcuts/images/icon-{Path.GetFileNameWithoutExtension(item.Name)}.png";
                if (!File.Exists(iconPath)) continue;
                var bgPath = System.AppDomain.CurrentDomain.BaseDirectory + $"/shortcuts/images/bg-{Path.GetFileNameWithoutExtension(item.Name)}.png";
                if (File.Exists(bgPath))
                {
                    newMap.BackgroundUri = new Uri(bgPath);
                }
                newMap.IconUri = new Uri(iconPath);
                newMap.FileLocation = item.FullName;
                Maps.Add(newMap);
            }
        }
        public static ShortcutMapEntity MatchMap(List<ShortcutMapEntity> entities, string regex)
        {
            foreach (var item in entities)
            {
                if (new Regex(item.ModuleName).IsMatch(regex))
                {
                    return item;
                }
            }
            return null;
        }
    }
}
