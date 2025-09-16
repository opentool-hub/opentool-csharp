using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenToolSDK.DotNetFx.Tool
{
    /// <summary>
    /// Function not supported by this Tool.
    /// </summary>
    public class FunctionNotSupportedException : Exception
    {
        public int Code => 405;

        [JsonProperty("message")]
        public override string Message { get; }

        public FunctionNotSupportedException(string functionName)
            : base($"Function Not Supported: {functionName}")
        {
            Message = $"Function Not Supported: {functionName}";
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// <summary>
    /// Invalid arguments passed to a function call.
    /// </summary>
    public class InvalidArgumentsException : Exception
    {
        public int Code => 400;

        [JsonProperty("message")]
        public override string Message { get; }

        public InvalidArgumentsException(Dictionary<string, object> arguments)
            : base($"Invalid Arguments: {JsonConvert.SerializeObject(arguments)}")
        {
            Message = $"Invalid Arguments: {JsonConvert.SerializeObject(arguments)}";
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// <summary>
    /// Tool execution encountered a break-level error; clients should stop retrying.
    /// </summary>
    public class ToolBreakException : Exception
    {
        public int Code => 500;

        [JsonProperty("message")]
        public override string Message { get; }

        public ToolBreakException(string message = null)
            : base(message)
        {
            Message = message ?? "Tool break exception";
        }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }

    /// <summary>
    /// JSON parser not implemented.
    /// </summary>
    public class JsonParserException : Exception
    {
        public int Code => 404;

        [JsonProperty("message")]
        public override string Message => "Json Parser NOT implement";

        public JsonParserException() : base("Json Parser NOT implement") { }

        public string ToJson() =>
            JsonConvert.SerializeObject(this, Formatting.None);
    }
}
