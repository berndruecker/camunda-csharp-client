using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CamundaClient
{
    static class CamundaClientExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
            => await Task.FromResult(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await content.ReadAsStringAsync()));

        public static async Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string uri, object request)
        {
            var content = Newtonsoft.Json.JsonConvert.SerializeObject(
                request,
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

                }
            );
            return await client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
        }
    }
}
