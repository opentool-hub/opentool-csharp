using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace OpenToolSDK.DotNetFx.Model
{
    public static class SchemaType
    {
        public const string BOOLEAN = "boolean";
        public const string INTEGER = "integer";
        public const string NUMBER = "number";
        public const string STRING = "string";
        public const string ARRAY = "array";
        public const string OBJECT = "object";
    }

    public class Schema
    {
        public string Type { get; set; }    // Required
        public string Description { get; set; }     // Optional
        public Dictionary<string, Schema> Properties { get; set; }  // Optional
        public Schema Items { get; set; }  // Optional
        public List<object> Enum { get; set; }  // Optional
        public List<string> Required { get; set; }  // Optional

        public static Schema FromJson(JObject json)
        {
            if (json == null) return null;

            if (json["$ref"] != null)
            {
                string refStr = json["$ref"].ToString();
                var schemaObj = FromRef(refStr);
                schemaObj.ValidateEnumConsistency();
                return schemaObj;
            }

            var schema = new Schema
            {
                Type = json["type"]?.ToString(),
                Description = json["description"]?.ToString()
            };

            if (json["properties"] is JObject propsObj)
            {
                schema.Properties = new Dictionary<string, Schema>();
                foreach (var prop in propsObj.Properties())
                {
                    if (prop.Value is JObject propSchema)
                    {
                        schema.Properties[prop.Name] = FromJson(propSchema);
                    }
                }
            }

            if (json["items"] is JObject itemsObj)
            {
                schema.Items = FromJson(itemsObj);
            }

            if (json["enum"] is JArray enumArray)
            {
                schema.Enum = enumArray.ToObject<List<object>>();
            }

            if (json["required"] is JArray requiredArray)
            {
                schema.Required = requiredArray.ToObject<List<string>>();
            }

            schema.ValidateEnumConsistency();
            return schema;
        }

        public JObject ToJson()
        {
            var json = new JObject
            {
                ["type"] = Type
            };

            if (!string.IsNullOrEmpty(Description))
                json["description"] = Description;

            if (Properties != null)
            {
                var props = new JObject();
                foreach (var kv in Properties)
                {
                    props[kv.Key] = kv.Value?.ToJson();
                }
                json["properties"] = props;
            }

            if (Items != null)
                json["items"] = Items.ToJson();

            if (Enum != null)
                json["enum"] = JArray.FromObject(Enum);

            if (Required != null)
                json["required"] = JArray.FromObject(Required);

            return json;
        }

        private static Schema FromRef(string refStr)
        {
            var parts = refStr.Split('/');
            if (parts.Length == 3 && parts[0] == "#" && parts[1] == "schemas")
            {
                string refName = parts[2];
                var schemas = SchemasSingleton.GetInstance();
                if (schemas.TryGetValue(refName, out var schema))
                {
                    return schema;
                }
                else
                {
                    throw new FormatException($"#ref not found: {refStr}");
                }
            }
            else
            {
                throw new FormatException($"#ref format exception: {refStr}");
            }
        }

        private void ValidateEnumConsistency()
        {
            if (Enum == null || Enum.Count == 0)
                return;

            for (int i = 0; i < Enum.Count; i++)
            {
                var value = Enum[i];
                if (!IsValueConsistentWithType(value))
                {
                    throw new FormatException($"Enum value at index {i} (\"{value}\") does not match schema type \"{Type}\".");
                }
            }
        }

        private bool IsValueConsistentWithType(object value)
        {
            switch (Type)
            {
                case SchemaType.STRING:
                    return value is string || value == null;
                case SchemaType.INTEGER:
                    return value is int || value is long || value == null;
                case SchemaType.NUMBER:
                    return value is float || value is double || value is decimal || value == null;
                case SchemaType.BOOLEAN:
                    return value is bool || value == null;
                case "null":
                    return value == null;
                default:
                    return true;
            }
        }
    }

    public static class SchemasSingleton
    {
        private static Dictionary<string, Schema> refSchemas = new Dictionary<string, Schema>();

        public static void InitInstance(JObject schemasJson)
        {
            foreach (var property in schemasJson.Properties())
            {
                string schemaName = property.Name;
                if (property.Value is JObject schemaObj)
                {
                    refSchemas[schemaName] = Schema.FromJson(schemaObj);
                }
                else
                {
                    Console.WriteLine($"[Warning] Schema '{schemaName}' is not a valid object.");
                }
            }
        }

        public static Dictionary<string, Schema> GetInstance() => refSchemas;

        public static Schema GetSchema(string name)
        {
            return refSchemas.TryGetValue(name, out var schema) ? schema : null;
        }
    }
}