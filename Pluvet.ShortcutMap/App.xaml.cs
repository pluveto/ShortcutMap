using Newtonsoft.Json;
using Pluvet.ShortcutMap.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

                newMap.IconUri = new Uri(System.AppDomain.CurrentDomain.BaseDirectory + $"/shortcuts/images/icon-{Path.GetFileNameWithoutExtension(item.Name)}.png");
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
