using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace OpenToolSDK.DotNet.model
{
    public class Function
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Parameter> Parameters { get; set; }
        public Return Return { get; set; }

        public static Function FromJson(JObject json)
        {
            if (json == null) return null;

            var parameters = new List<Parameter>();
            if (json["parameters"] is JArray paramArray)
            {
                foreach (var item in paramArray)
                {
                    if (item is JObject paramObj)
                        parameters.Add(Parameter.FromJson(paramObj));
                }
            }

            return new Function
            {
                Name = json["name"]?.ToString(),
                Description = json["description"]?.ToString(),
                Parameters = parameters,
                Return = json["return"] is JObject retObj ? Return.FromJson(retObj) : null
            };
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["name"] = Name,
                ["description"] = Description,
                ["parameters"] = new JArray(Parameters?.ConvertAll(p => p.ToJson()))
            };

            if (Return != null)
                json["return"] = Return.ToJson();
            else
                json["return"] = null; // 明确 null 输出

            return json;
        }
    }
}
