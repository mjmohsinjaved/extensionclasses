using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static SharedLibrary.Enums.General;

namespace SharedLibrary.HttpCLientExtension
{
    public class SharedHttpClient
    {
        public HttpClient Client { get; private set; }
        public SharedHttpClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                string token = string.Empty;
                //get token from context and set in header
                if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    token = httpContextAccessor.HttpContext.User.FindFirst("token").Value;
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                }
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                
                Client = client;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<T> GetStringAsync<T>(string url)
        {
            var content = await Client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(content);
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string url)
        {
            var apiResponse = new ApiResponse<T>();
            try
            {
                var response = await Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Content = await response.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject<T>(apiResponse.Content);
                }
                apiResponse.IsSuccess = response.IsSuccessStatusCode;
                apiResponse.StatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {

                apiResponse.IsSuccess = false;
                apiResponse.Message = ex.Message;
            }
            return apiResponse;
        }
        public async Task<ApiResponse> GetAsync(string url)
        {
            var apiResponse = new ApiResponse();
            try
            {
                var response = await Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    apiResponse.Content = await response.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject(apiResponse.Content);
                }
                apiResponse.IsSuccess = response.IsSuccessStatusCode;
                apiResponse.StatusCode = response.StatusCode;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Message = ex.Message;
            }
            return apiResponse;
        }
        public async Task<ApiResponse<T>> SendAsync<T>(string url, object model, RequestType type = RequestType.Post)
        {
            var apiResponse = new ApiResponse<T>();
            HttpResponseMessage responseMessage;
            try
            {
                switch (type)
                {
                    case RequestType.Post:
                        responseMessage = await Client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                    case RequestType.Put:
                        responseMessage = await Client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                    case RequestType.Patch:
                        responseMessage = await Client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json-patch+json"));
                        break;
                    case RequestType.Get:
                        responseMessage = await Client.SendAsync(new HttpRequestMessage() { Method= HttpMethod.Get,RequestUri = new Uri(url.Contains(Client.BaseAddress.AbsoluteUri) ? url :  Client.BaseAddress+url),Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json") });
                        break;
                    default:
                        responseMessage = await Client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    apiResponse.Content = await responseMessage.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject<T>(apiResponse.Content);
                }
                apiResponse.IsSuccess = responseMessage.IsSuccessStatusCode;
                apiResponse.StatusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Content = null;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }
        public async Task<ApiResponse> SendAsync(string url, object model, RequestType type = RequestType.Post)
        {
            var apiResponse = new ApiResponse();
            HttpResponseMessage responseMessage;
            try
            {
                switch (type)
                {
                    case RequestType.Post:
                        responseMessage = await Client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                    case RequestType.Put:
                        responseMessage = await Client.PutAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                    case RequestType.Patch:
                        responseMessage = await Client.PatchAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json-patch+json"));
                        break;
                    case RequestType.Get:
                        responseMessage = await Client.SendAsync(new HttpRequestMessage() { Method = HttpMethod.Get, RequestUri = new Uri(Client.BaseAddress + url), Content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json") });
                        break;
                    default:
                        responseMessage = await Client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
                        break;
                }

                if (responseMessage.IsSuccessStatusCode)
                {
                    apiResponse.Content = await responseMessage.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject(apiResponse.Content);
                }
                apiResponse.IsSuccess = responseMessage.IsSuccessStatusCode;
                apiResponse.StatusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Content = null;
                apiResponse.Message = ex.Message;
            }

            return apiResponse;
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string url)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var responseMessage = await Client.DeleteAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    apiResponse.Content = await responseMessage.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject<T>(apiResponse.Content);
                }
                apiResponse.IsSuccess = responseMessage.IsSuccessStatusCode;
                apiResponse.StatusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                apiResponse.Content = null;
                apiResponse.IsSuccess = false;
                apiResponse.Message = ex.Message;
            }


            return null;
        }
        public async Task<ApiResponse> DeleteAsync(string url)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var responseMessage = await Client.DeleteAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    apiResponse.Content = await responseMessage.Content.ReadAsStringAsync();
                    apiResponse.ResponseObject = JsonConvert.DeserializeObject(apiResponse.Content);
                }
                apiResponse.IsSuccess = responseMessage.IsSuccessStatusCode;
                apiResponse.StatusCode = responseMessage.StatusCode;
            }
            catch (Exception ex)
            {
                apiResponse.Content = null;
                apiResponse.IsSuccess = false;
                apiResponse.Message = ex.Message;
            }


            return apiResponse;
        }
    }
}
