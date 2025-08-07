using Newtonsoft.Json;
using OpenToolSDK.DotNet;
using OpenToolSDK.DotNet.LLM;
using OpenToolSDK.DotNet.model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenToolSDK.DotNet.Client
{
    public interface IClient
    {
        Task<Version> Version();
        Task<ToolReturn> Call(FunctionCall functionCall);
        Task<OpenTool> Load();
    }

    public class OpenToolClient : IClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public OpenToolClient(bool isSSL = false, string host = "localhost", int port = Constants.DEFAULT_PORT, string apiKey = null)
        {
            string protocol = isSSL ? "https" : "http";
            _baseUrl = $"{protocol}://{host}:{port}{Constants.DEFAULT_PREFIX}/";

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_baseUrl)
            };

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);
            }
        }

        public async Task<Version> Version()
        {
            try
            {
                var response = await _httpClient.GetAsync("version");
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Version>(body);
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                throw new OpenToolServerUnauthorizedException();
            }
            catch
            {
                throw new OpenToolServerNoAccessException();
            }
        }

        public async Task<ToolReturn> Call(FunctionCall functionCall)
        {
            var result = await CallJsonRpcHttp(functionCall.Id, functionCall.Name, functionCall.Arguments);
            return new ToolReturn(functionCall.Id, result);
        }

        private async Task<Dictionary<string, object>> CallJsonRpcHttp(string id, string method, Dictionary<string, object> parameters)
        {
            var body = new JsonRpcHttpRequestBody
            {
                Id = id,
                Method = method,
                Params = parameters
            };

            try
            {
                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("call", content);
                string responseBody = await response.Content.ReadAsStringAsync();

                var resultObj = JsonConvert.DeserializeObject<JsonRpcHttpResponseBody>(responseBody);

                if (resultObj.Error != null)
                    throw new OpenToolServerCallException(resultObj.Error.Message);

                return resultObj.Result;
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                throw new OpenToolServerUnauthorizedException();
            }
            catch
            {
                throw new OpenToolServerNoAccessException();
            }
        }

        public async Task<OpenTool> Load()
        {
            try
            {
                var response = await _httpClient.GetAsync("load");
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<OpenTool>(content);
                return data;
            }
            catch
            {
                return null;
            }
        }
    }
}
