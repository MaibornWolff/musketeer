using System;
using System.Collections.Generic;
using Musketeer.Extensions;
using Musketeer.Selenium;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Musketeer.Services
{
    public abstract class GenericRestClient
    {
        private string _baseUrl;
        private string _token;

        public GenericRestClient(string token, bool tokenIsMandatory = true)
        {
            if(tokenIsMandatory && String.IsNullOrEmpty(token)) throw new Exception("Token is null");
            _baseUrl = Driver.BaseUrl;
            _token = token;
        }

        protected IRestResponse Get(string path) =>
            SendRequest<dynamic>(path, Method.GET, null);

        protected IRestResponse Post<T>(string path, T body) where T : class =>
            SendRequest<T>(path, Method.POST, body);

        protected IRestResponse Put<T>(string path, T body) where T : class =>
            SendRequest<T>(path, Method.PUT, body);

        protected IRestResponse Delete(string path) =>
            SendRequest<dynamic>(path, Method.DELETE, null);

        private IRestResponse SendRequest<T>(string path, Method method, T body)
        {
            Console.WriteLine($"Sending a {method} request to {_baseUrl.UrlAppend(path)}");
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
            };

            var client = new RestClient(_baseUrl.UrlAppend(path))
                .AddDefaultHeader("Authorization", $"Bearer {_token}")
                .UseNewtonsoftJson(serializerSettings);

            var request = new RestRequest
            {
                Method = method,
                RequestFormat = DataFormat.Json
            };

            if (body != null)
            {
                Console.WriteLine("Add body = " + body);
                request.AddJsonBody(body);
            }

            var response = client.Execute(request);
            Console.WriteLine($"StatusCode == {response.StatusCode}");
            Console.WriteLine($"Content == {response.Content}");
            return response;
        }
    }
}
