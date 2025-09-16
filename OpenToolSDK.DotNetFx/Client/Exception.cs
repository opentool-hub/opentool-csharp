using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNetFx.Client
{
    public class ResponseNullException : Exception
    {
        [JsonProperty("code")]
        public int Code { get; }

        [JsonProperty("message")]
        public override string Message => "Response is null";

        public ResponseNullException(int code)
        {
            Code = code;
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class ErrorNullException : Exception
    {
        [JsonProperty("code")]
        public int Code { get; }

        [JsonProperty("message")]
        public override string Message => "Error is null";

        public ErrorNullException(int code)
        {
            Code = code;
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class OpenToolServerUnauthorizedException : Exception
    {
        [JsonProperty("code")]
        public int Code => 401;

        [JsonProperty("message")]
        public override string Message => "Please check API Key is VALID or NOT";

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class OpenToolServerNoAccessException : Exception
    {
        [JsonProperty("code")]
        public int Code => 404;

        [JsonProperty("message")]
        public override string Message => "Please check OpenTool Server is RUNNING or NOT";

        public OpenToolServerNoAccessException() { }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    public class OpenToolServerCallException : Exception
    {
        [JsonProperty("message")]
        public override string Message { get; }

        public OpenToolServerCallException(string message)
        {
            Message = message;
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }
}
