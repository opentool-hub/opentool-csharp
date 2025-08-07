using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet.LLM
{
    public class FunctionCall
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("arguments", Required = Required.Always)]
        public Dictionary<string, object> Arguments { get; set; }

        public FunctionCall() { }

        public FunctionCall(string id, string name, Dictionary<string, object> arguments)
        {
            Id = id;
            Name = name;
            Arguments = arguments;
        }

        public static FunctionCall FromJson(string json)
        {
            return JsonConvert.DeserializeObject<FunctionCall>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public class ToolReturn
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("result", Required = Required.Always)]
        public Dictionary<string, object> Result { get; set; }

        public ToolReturn() { }

        public ToolReturn(string id, Dictionary<string, object> result)
        {
            Id = id;
            Result = result;
        }

        public static ToolReturn FromJson(string json)
        {
            return JsonConvert.DeserializeObject<ToolReturn>(json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
