using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Pluvet.ShortcutMap.Entities
{

    public partial class ShortcutMapEntity
    {

        public Uri IconUri { get; set; }


        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("moduleName")]
        public string ModuleName { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("groups")]
        public Group[] Groups { get; set; }
    }

    public partial class Group
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortcuts")]
        public Shortcut[] Shortcuts { get; set; }
    }

    public partial class Shortcut
    {
        [JsonProperty("keys")]
        public string[] Keys { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }
    }
}
