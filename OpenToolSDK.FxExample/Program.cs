using OpenToolSDK.DotNetFx.Client;
using OpenToolSDK.DotNetFx.LLM;
using OpenToolSDK.DotNetFx.Server;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenToolSDK.FxExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Launching OpenTool Server... ===");

            var tool = new MockTool();
            var server = new OpenToolServer(
                tool: tool,
                version: "1.0.0",
                apiKeys: new List<string>
                {
                    "6621c8a3-2110-4e6a-9d62-70ccd467e789",
                    "bb31b6a6-1fda-4214-8cd6-b1403842070c"
                }
            );

            await server.Start();

            await Task.Delay(300);

            Console.WriteLine("\n=== Running OpenTool Client... ===");

            var client = new OpenToolClient(apiKey: "bb31b6a6-1fda-4214-8cd6-b1403842070c");

            var version = await client.Version();
            Console.WriteLine($"[Client] Server version: {version.VersionNumber}");

            var call = new FunctionCall
            {
                Id = Guid.NewGuid().ToString(),
                Name = "count",
                Arguments = new Dictionary<string, object>()
            };

            var result = await client.Call(call);
            Console.WriteLine($"[Client] Call result: {result.ToJson()}");

            var opentool = await client.Load();
            Console.WriteLine($"[Client] Load result: {opentool.ToJson()}");

            Console.WriteLine("\nPress ENTER to stop server...");
            Console.ReadLine();

            await server.Stop();
        }
    }
}
