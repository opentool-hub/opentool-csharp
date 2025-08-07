using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet.Daemon
{
    public class DaemonClient
    {
        private const int DAEMON_DEFAULT_PORT = 19627;
        private const string DAEMON_DEFAULT_PREFIX = "/opentool-daemon";

        private readonly string _protocol = "http";
        private readonly string _host = "localhost";
        private readonly int _port;
        private readonly string _prefix;
        private readonly HttpClient _httpClient;

        public DaemonClient(int? port = null)
        {
            _port = port.HasValue && port > 0 ? port.Value : DAEMON_DEFAULT_PORT;
            _prefix = DAEMON_DEFAULT_PREFIX;

            string baseUrl = $"{_protocol}://{_host}:{_port}{_prefix}";
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<RegisterResult> Register(RegisterInfo registerInfo)
        {
            try
            {
                string json = registerInfo.ToJson();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("/register", content);
                response.EnsureSuccessStatusCode();

                string resultJson = await response.Content.ReadAsStringAsync();
                return RegisterResult.FromJson(resultJson);
            }
            catch (HttpRequestException e)
            {
                return new RegisterResult
                {
                    Id = "-1",
                    Error = e.Message
                };
            }
        }
    }
}
