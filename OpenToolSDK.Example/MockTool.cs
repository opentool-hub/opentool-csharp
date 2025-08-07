using OpenToolSDK.DotNet.model;
using OpenToolSDK.DotNet.Tool;
using OpenToolSDK.Example;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet
{
    public class MockTool : ITool
    {
        private readonly MockUtil _mockUtil = new MockUtil();

        public override Task<Dictionary<string, object>> Call(string name, Dictionary<string, object> arguments)
        {
            try
            {
                switch (name)
                {
                    case "count":
                        return Task.FromResult(new Dictionary<string, object> { { "count", _mockUtil.Count() } });

                    case "create":
                        var text = arguments["text"].ToString();
                        int id = _mockUtil.Create(text);
                        return Task.FromResult(new Dictionary<string, object> { { "id", id } });

                    case "read":
                        int readId = Convert.ToInt32(arguments["id"]);
                        string resultText = _mockUtil.Read(readId);
                        return Task.FromResult(new Dictionary<string, object> { { "text", resultText } });

                    case "update":
                        int updateId = Convert.ToInt32(arguments["id"]);
                        string updateText = arguments["text"].ToString();
                        _mockUtil.Update(updateId, updateText);
                        return Task.FromResult(new Dictionary<string, object> { { "result", "Update successfully." } });

                    case "delete":
                        int deleteId = Convert.ToInt32(arguments["id"]);
                        _mockUtil.Delete(deleteId);
                        return Task.FromResult(new Dictionary<string, object> { { "result", "Delete successfully." } });

                    case "run":
                        try
                        {
                            _mockUtil.Run();
                        }
                        catch (Exception e)
                        {
                            throw new ToolBreakException(e.Message);
                        }
                        return Task.FromResult(new Dictionary<string, object> { { "result", "Delete successfully." } });

                    default:
                        throw new FunctionNotSupportedException(name);
                }
            }
            catch (FunctionNotSupportedException fns)
            {
                return Task.FromResult(new Dictionary<string, object>
                {
                    { "code", fns.Code },
                    { "message", fns.Message }
                });
            }
        }

        public override async Task<OpenTool> Load()
        {
            string assemblyPath = Path.GetDirectoryName(typeof(MockTool).Assembly.Location);
            string jsonPath = Path.Combine(assemblyPath, "mock_tool.json");

            var loader = new OpenToolJsonLoader();
            var openTool = loader.LoadFromFile(jsonPath);
            return await Task.FromResult(openTool);
        }
    }
}