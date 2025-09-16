using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenToolSDK.DotNetFx.Model
{
    public class Info
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }

        public static Info FromJson(JObject json)
        {
            if (json == null) return null;
            return new Info
            {
                Title = json["title"]?.ToString(),
                Description = json["description"]?.ToString(),
                Version = json["version"]?.ToString()
            };
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["title"] = Title,
                ["version"] = Version
            };

            if (!string.IsNullOrEmpty(Description))
                json["description"] = Description;

            return json;
        }
    }
}
