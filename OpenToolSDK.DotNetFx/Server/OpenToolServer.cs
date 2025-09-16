using Microsoft.Owin.Hosting;
using OpenToolSDK.DotNetFx.Daemon;
using OpenToolSDK.DotNetFx.Tool;
using Owin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Host.HttpListener;

namespace OpenToolSDK.DotNetFx.Server
{
    public interface IServer
    {
        Task Start();
        Task Stop();
    }

    public class OpenToolServer : IServer
    {
        private readonly ITool _tool;
        private readonly string _version;
        private readonly string _ip;
        private readonly int _port;
        private readonly string _prefix;
        private readonly List<string> _apiKeys;
        private IDisposable _webApp;

        public OpenToolServer(
            ITool tool,
            string version,
            string ip = "127.0.0.1",
            int port = Constants.DEFAULT_PORT,
            string prefix = Constants.DEFAULT_PREFIX,
            List<string> apiKeys = null)
        {
            _tool = tool;
            _version = version;
            _ip = ip;
            _port = port;
            _prefix = prefix.StartsWith("/") ? prefix : "/" + prefix;
            _apiKeys = apiKeys ?? new List<string>();
        }

        public Task Start()
        {
            string baseAddress = $"http://{_ip}:{_port}";

            try
            {
                ToolRegistry.ToolInstance = _tool;
                ToolRegistry.Version = _version;
                _webApp = WebApp.Start(url: baseAddress, startup: appBuilder =>
                {
                    if (_apiKeys.Count > 0)
                    {
                        appBuilder.Use(typeof(AuthorizationMiddleware), _apiKeys);
                    }


                    var config = new System.Web.Http.HttpConfiguration();
                    WebApiConfig.Register(config);
                    config.EnsureInitialized();
                    appBuilder.UseWebApi(config);
                });

                Console.WriteLine($"Start Server: {baseAddress}{_prefix}");

                var registerInfo = new RegisterInfo
                {
                    File = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName,
                    Host = IPAddress.Loopback.ToString(),
                    Port = _port,
                    Prefix = _prefix,
                    ApiKeys = _apiKeys,
                    Pid = System.Diagnostics.Process.GetCurrentProcess().Id
                };

                var client = new DaemonClient();
                var result = client.Register(registerInfo).Result;

                if (!string.IsNullOrEmpty(result.Error))
                {
                    Console.WriteLine($"WARNING: Register to daemon failed. ({result.Error})");
                     Console.WriteLine("Tool Running in SOLO mode.");
                }
                else
                {
                    Console.WriteLine($"Register to daemon successfully, id: {result.Id}, pid: {registerInfo.Pid}");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to start server: {ex}");
                throw;
            }
        }

        public Task Stop()
        {
            _webApp?.Dispose();
            Console.WriteLine("Server stopped.");
            return Task.CompletedTask;
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
