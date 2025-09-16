using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenToolSDK.DotNetFx.Model
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Schema Schema { get; set; }
        public bool RequiredFlag { get; set; }

        public static Parameter FromJson(JObject json)
        {
            if (json == null) return null;

            return new Parameter
            {
                Name = json["name"]?.ToString(),
                Description = json["description"]?.ToString(),
                Schema = Schema.FromJson(json["schema"] as JObject),
                RequiredFlag = json["required"]?.ToObject<bool>() ?? false
            };
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["name"] = Name,
                ["schema"] = Schema?.ToJson(),
                ["required"] = RequiredFlag
            };

            if (!string.IsNullOrEmpty(Description))
                json["description"] = Description;

            return json;
        }
    }
}
