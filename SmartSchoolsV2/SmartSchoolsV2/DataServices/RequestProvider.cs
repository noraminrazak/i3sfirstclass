using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchoolsV2.DataServices
{
    public class RequestProvider : IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }


        public async Task<bool> CheckConnection(string uri)
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            return response.IsSuccessStatusCode;
        }


        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            try
            {
                HttpClient httpClient = CreateHttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    await HandleResponse(response);

                    string serialized = await response.Content.ReadAsStringAsync();
                    TResult result =
                        await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

                    return result;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new KeyNotFoundException();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("---> " + ex.Message);
            }
            throw new Exception();
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data)
        {
            return PostAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data)
        {
            HttpClient httpClient = CreateHttpClient();
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PostAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            return PutAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data)
        {
            HttpClient httpClient = CreateHttpClient();
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PutAsync(uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        private HttpClient CreateHttpClient()
        {
            // Add Authorization Key
            var httpClient = new HttpClient();

            var auth =
                "AAAAr7qlrz4:APA91bGIZnOJgHsgXlGLmx1giLFVm32Llw6DPtytUkNbleW0JyJeNqy9D1k8ponOvYID5qX2aGq4w-OgsJgwxEansF0hwk7YRtpz9g_F-fWppoq4v2kye4WuUNkVsiszYM6Djdfobhpu";
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + auth);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //throw new ServiceAuthenticationException(content);
                }

                throw new HttpRequestException(content);
            }
        }
    }
}
