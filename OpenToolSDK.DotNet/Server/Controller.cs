using Newtonsoft.Json;
using OpenToolSDK.DotNet.Tool;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace OpenToolSDK.DotNet.Server
{
    public static class ToolRegistry
    {
        public static ITool ToolInstance { get; set; }

        public static string Version { get; set; } = "1.0.0";
    }

    [RoutePrefix("opentool")]
    public class ServerController : ApiController
    {
        private readonly ITool _tool;
        private readonly string _version;

        public ServerController() : this(ToolRegistry.ToolInstance, ToolRegistry.Version) { }

        public ServerController(ITool tool, string version)
        {
            _tool = tool;
            _version = version;
        }

        // GET /version
        [HttpGet]
        [Route("version")]
        public IHttpActionResult GetVersion()
        {
            var versionObj = new Version { VersionNumber = _version };
            return Json(versionObj);
        }

        // POST /call
        [HttpPost]
        [Route("call")]
        public async Task<IHttpActionResult> Call()
        {
            try
            {
                string json = await Request.Content.ReadAsStringAsync();
                var body = JsonConvert.DeserializeObject<JsonRpcHttpRequestBody>(json);

                var result = await _tool.Call(body.Method, body.Params);
                var response = new JsonRpcHttpResponseBody
                {
                    Id = body.Id,
                    Result = result
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                var error = new JsonRpcHttpResponseBodyError
                {
                    Code = 500,
                    Message = ex.ToString()
                };

                var response = new JsonRpcHttpResponseBody
                {
                    Id = "",
                    Result = new System.Collections.Generic.Dictionary<string, object>(),
                    Error = error
                };

                return Json(response);
            }
        }

        // GET /load
        [HttpGet]
        [Route("load")]
        public async Task<IHttpActionResult> Load()
        {
            try
            {
                var openTool = await _tool.Load();
                if (openTool != null) 
                {
                    String aaa = openTool.ToJson();
                    return Json(JsonConvert.DeserializeObject(openTool.ToJson()));
                }
                    
                //return Json(openTool.ToJson());

                var err = new JsonParserException();
                return Json(JsonConvert.DeserializeObject(err.ToJson()));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, new { error = ex.ToString() });
            }
        }
    }
}
