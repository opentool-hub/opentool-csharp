using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenToolSDK.DotNet.model
{
    public class Return
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Schema Schema { get; set; }

        public static Return FromJson(JObject json)
        {
            if (json == null) return null;

            return new Return
            {
                Name = json["name"]?.ToString(),
                Description = json["description"]?.ToString(),
                Schema = Schema.FromJson(json["schema"] as JObject)
            };
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["name"] = Name,
                ["schema"] = Schema?.ToJson()
            };

            if (!string.IsNullOrEmpty(Description))
                json["description"] = Description;

            return json;
        }
    }
}
