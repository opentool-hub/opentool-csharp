using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenToolSDK.DotNetFx.Model
{
    public class OpenTool
    {
        public string OpenToolVersion { get; set; }
        public Info Info { get; set; }
        public Server Server { get; set; }
        public List<Function> Functions { get; set; }
        public Dictionary<string, Schema> Schemas { get; set; }

        public static OpenTool FromJson(string json)
        {
            var jObj = JObject.Parse(json);

            var schemasToken = jObj["schemas"] as JObject;
            var schemas = new Dictionary<string, Schema>();
            if (schemasToken != null)
            {
                foreach (var kv in schemasToken.Properties())
                {
                    if (kv.Value is JObject schemaObj)
                    {
                        schemas[kv.Name] = Schema.FromJson(schemaObj);
                    }
                }
                SchemasSingleton.InitInstance(schemasToken);
            }

            var functions = new List<Function>();
            if (jObj["functions"] is JArray fnArray)
            {
                foreach (var fn in fnArray)
                {
                    if (fn is JObject fnObj)
                        functions.Add(Function.FromJson(fnObj));
                }
            }

            return new OpenTool
            {
                OpenToolVersion = jObj["opentool"]?.ToString(),
                Info = Info.FromJson(jObj["info"] as JObject),
                Server = Server.FromJson(jObj["server"] as JObject),
                Schemas = schemas,
                Functions = functions
            };
        }

        public string ToJson()
        {
            var json = new JObject
            {
                ["opentool"] = OpenToolVersion,
                ["info"] = Info?.ToJson(),
                ["server"] = Server?.ToJson(),
                ["functions"] = new JArray(Functions?.ConvertAll(f => f.ToJson()))
            };

            if (Schemas != null)
            {
                var schemaObj = new JObject();
                foreach (var kv in Schemas)
                {
                    schemaObj[kv.Key] = kv.Value.ToJson();
                }
                json["schemas"] = schemaObj;
            }

            return json.ToString(Newtonsoft.Json.Formatting.Indented);
        }
    }
}
