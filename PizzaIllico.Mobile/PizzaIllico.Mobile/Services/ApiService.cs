using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
using PizzaIllico.Mobile.Dtos.Pizzas;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.Services
{
    public interface IApiService
    {
        Task<TResponse> Get<TResponse>(string url);

        Task<bool> Post<TResponse>(string url, CreateUserRequest myJson);

        Task<TResponse> PostConnexion<TResponse>(string url, LoginWithCredentialsRequest myJson);

        Task<TResponse> GetInfosUtilisateur<TResponse>(string url, string tokenUtilisateur);

        Task<TResponse> Patch<TResponse>(string url, string myJson, UserProfileResponse tokenUtilisateur);

        Task<TResponse> PatchMDP<TResponse>(string url, string tokenUtilisateur, SetPasswordRequest myJson);

        Task<TResponse> GetPizzasRestaurant<TResponse>(string url, string tokenUtilisateur, string idPizzeria);

        Task<TResponse> PostPizzaPanier<TResponse>(string url, string tokenUtilisateur, string idShop, CreateOrderRequest idPizza);

        Task<TResponse> GetAnciennesCommandes<TResponse>(string url, string tokenUtilisateur);
    }
    
    public class ApiService : IApiService
    {
	    private const string HOST = "https://pizza.julienmialon.ovh/";
        private readonly HttpClient _client = new HttpClient();
        
        public async Task<TResponse> Get<TResponse>(string url)
        {
	        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, HOST + url);

	        HttpResponseMessage response = await _client.SendAsync(request);

	        string content = await response.Content.ReadAsStringAsync();

	        return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<bool> Post<TResponse>(string url, CreateUserRequest myJson)
        {
            string json = JsonConvert.SerializeObject(myJson);
            Console.WriteLine($"Body + {json}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, HOST + url);

            HttpResponseMessage response = await _client.PostAsync(request.RequestUri, new StringContent(json, Encoding.UTF8, "application/json"));

            string content = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Contenue + {content}");

            return response.IsSuccessStatusCode;           
        }

        public async Task<TResponse> PostConnexion<TResponse>(string url, LoginWithCredentialsRequest myJson)
        {
            string json = JsonConvert.SerializeObject(myJson);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, HOST + url);

            HttpResponseMessage response = await _client.PostAsync(request.RequestUri, new StringContent(json, Encoding.UTF8, "application/json"));

            string content = await response.Content.ReadAsStringAsync();
          
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> GetInfosUtilisateur<TResponse>(string url, string tokenUtilisateur)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, HOST + url);
            request.Headers.Add("Authorization", "Bearer " + tokenUtilisateur);

            HttpResponseMessage response = await _client.SendAsync(request);
            Console.WriteLine("REPONSESE" + response);
            string content = await response.Content.ReadAsStringAsync();
            Console.WriteLine("CONTENU" + content);

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> Patch<TResponse>(string url, string tokenUtilisateur, UserProfileResponse myJson)
        {
            string json = JsonConvert.SerializeObject(myJson);

            StringContent contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new(new HttpMethod("PATCH"), HOST + url)
            {
                Content = contentJson
            };
            request.Headers.Add("Authorization", "Bearer " + tokenUtilisateur);

            HttpResponseMessage response = await _client.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> PatchMDP<TResponse>(string url, string tokenUtilisateur, SetPasswordRequest myJson)
        {
            string json = JsonConvert.SerializeObject(myJson);

            StringContent contentJson = new StringContent(json, Encoding.UTF8, "application/json");

            HttpRequestMessage request = new(new HttpMethod("PATCH"), HOST + url)
            {
                Content = contentJson
            };
            request.Headers.Add("Authorization", "Bearer " + tokenUtilisateur);

            HttpResponseMessage response = await _client.SendAsync(request);

            string content = await response.Content.ReadAsStringAsync();
           
            Console.WriteLine("CONTENUMDP" + content);
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> GetPizzasRestaurant<TResponse>(string url, string tokenUtilisateur, string idPizzeria)
        {
            string oldchar = "{shopId}";
            url = url.Replace(oldchar, idPizzeria);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, HOST + url);
            request.Headers.Add("Authorization", "Bearer " + tokenUtilisateur);

            HttpResponseMessage response = await _client.SendAsync(request);            
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> PostPizzaPanier<TResponse>(string url, string tokenUtilisateur, string idShop, CreateOrderRequest idPizza)
        {
            string oldchar = "{shopId}";
            url = url.Replace(oldchar, idShop);

            string json = JsonConvert.SerializeObject(idPizza);
            Console.WriteLine("JSON  " + url + "  " + json + "  " + tokenUtilisateur + "  "+ idShop);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenUtilisateur);
            HttpResponseMessage response = await _client.PostAsync(HOST + url, new StringContent(json, Encoding.UTF8, "application/json"));

            string content = await response.Content.ReadAsStringAsync();

            Console.WriteLine("CONTENTPANIER" + content);
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        public async Task<TResponse> GetAnciennesCommandes<TResponse>(string url, string tokenUtilisateur)
        {
            
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, HOST + url);
            request.Headers.Add("Authorization", "Bearer " + tokenUtilisateur);

            HttpResponseMessage response = await _client.SendAsync(request);
            string content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(content);
        }

    }
}