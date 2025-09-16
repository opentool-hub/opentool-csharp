using OpenToolSDK.DotNetFx.Server;
using OpenToolSDK.DotNetFx.Tool;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenToolSDK.FxExample
{
    public class ServerMain
    {
        public static async Task Run()
        {
            ITool tool = new MockTool();
            IServer server = new OpenToolServer(
                tool: tool,
                version: "1.0.0",
                apiKeys: new List<string>
                {
                    "6621c8a3-2110-4e6a-9d62-70ccd467e789",
                    "bb31b6a6-1fda-4214-8cd6-b1403842070c"
                }
            );

            await server.Start();

            Console.WriteLine("Server is running. Press ENTER to stop...");
            Console.ReadLine();

            await server.Stop();
        }
    }
}
