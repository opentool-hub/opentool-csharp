using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet.Daemon
{
    public class RegisterInfo
    {
        [JsonProperty("file", Required = Required.Always)]
        public string File { get; set; }

        [JsonProperty("host", Required = Required.Always)]
        public string Host { get; set; }

        [JsonProperty("port", Required = Required.Always)]
        public int Port { get; set; }

        [JsonProperty("prefix", Required = Required.Always)]
        public string Prefix { get; set; }

        [JsonProperty("apiKeys", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> ApiKeys { get; set; }

        [JsonProperty("pid", Required = Required.Always)]
        public int Pid { get; set; }

        public static RegisterInfo FromJson(string json) =>
            JsonConvert.DeserializeObject<RegisterInfo>(json);

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public class RegisterResult
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error { get; set; }

        public static RegisterResult FromJson(string json) =>
            JsonConvert.DeserializeObject<RegisterResult>(json);

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
