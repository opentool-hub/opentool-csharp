using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenToolSDK.DotNet.model;
using System;
using System.IO;

namespace OpenToolSDK.DotNet
{
    public class OpenToolJsonLoader
    {
        public JObject SchemasJsonRaw { get; private set; }

        public OpenTool Load(string jsonString)
        {
            try
            {
                var jObject = JObject.Parse(jsonString);

                if (jObject["schemas"] is JObject schemasToken)
                {
                    try
                    {
                        SchemasJsonRaw = schemasToken;
                        SchemasSingleton.InitInstance(schemasToken);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Error] Failed to parse schemas: {ex.Message}");
                    }
                }

                return OpenTool.FromJson(jsonString);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Error] Failed to deserialize OpenTool: {ex.Message}");
                throw;
            }
        }

        public OpenTool LoadFromFile(string jsonPath)
        {
            string jsonString = File.ReadAllText(jsonPath);
            return Load(jsonString);
        }
    }
}
