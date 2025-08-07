using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet
{
    public static class Constants
    {
        public const string JSONRPC_VERSION = "2.0";
        public const int DEFAULT_PORT = 9627;
        public const string DEFAULT_PREFIX = "/opentool";
    }

    public class Version
    {
        [JsonProperty("version")]
        public string VersionNumber { get; set; }

        public static Version FromJson(string json) => JsonConvert.DeserializeObject<Version>(json);

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class JsonRpcHttpRequestBody
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = Constants.JSONRPC_VERSION;

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> Params { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public static JsonRpcHttpRequestBody FromJson(string json) =>
            JsonConvert.DeserializeObject<JsonRpcHttpRequestBody>(json);

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class JsonRpcHttpResponseBody
    {
        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = Constants.JSONRPC_VERSION;

        [JsonProperty("result")]
        public Dictionary<string, object> Result { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public JsonRpcHttpResponseBodyError Error { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public static JsonRpcHttpResponseBody FromJson(string json) =>
            JsonConvert.DeserializeObject<JsonRpcHttpResponseBody>(json);

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class JsonRpcHttpResponseBodyError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        public static JsonRpcHttpResponseBodyError FromJson(string json) =>
            JsonConvert.DeserializeObject<JsonRpcHttpResponseBodyError>(json);

        public string ToJson() => JsonConvert.SerializeObject(this, Formatting.None);
    }
}
