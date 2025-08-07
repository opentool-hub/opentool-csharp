using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OpenToolSDK.DotNet.model
{
    public class Server
    {
        public string Url { get; set; }
        public string Description { get; set; }

        public static Server FromJson(JObject json)
        {
            if (json == null) return null;
            return new Server
            {
                Url = json["url"]?.ToString(),
                Description = json["description"]?.ToString()
            };
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["url"] = Url
            };

            if (!string.IsNullOrEmpty(Description))
                json["description"] = Description;

            return json;
        }
    }
}
