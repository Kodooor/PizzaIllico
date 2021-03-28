using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Dtos.Authentications;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
using PizzaIllico.Mobile.Dtos.Pizzas;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.Services
{
    public interface IPizzaApiService
    {
        Task<Response<List<ShopItem>>> ListShops();

        Task<Response<List<PizzaItem>>> ListPizzas(int shopId);

        Task<Response<UserProfileResponse>> CreateUser(CreateUserRequest user);

        Task<Response<LoginResponse>> Connexion(LoginWithCredentialsRequest userConnexion);

        Task<Response<UserProfileResponse>> InfosUtilisateur(string tokenUtilisateur);

        Task<Response<UserProfileResponse>> EditerProfilUtilisateur(string tokenUtilisateur, UserProfileResponse editionUser);

        Task<Response<SetPasswordRequest>> ModifierMDP(string tokenUtilisateur, SetPasswordRequest editionMDP);

        Task<Response<List<PizzaItem>>> ListePizzasRestaurant(string tokenUtilisateur, string idPizzeria);

        Task<Response<ShopItem>> AjouterPizzaPanier(string tokenUtilisateur, string idShop, CreateOrderRequest idPizza);

        Task<Response<List<OrderItem>>> MesAnciennesCommandes(string tokenUtilisateur);
    }
    
    public class PizzaApiService : IPizzaApiService
    {
        private readonly IApiService _apiService;

        public PizzaApiService()
        {
            _apiService = DependencyService.Get<IApiService>();
        }

        public async Task<Response<List<ShopItem>>> ListShops()
        {
            return await _apiService.Get<Response<List<ShopItem>>>(Urls.LIST_SHOPS);
        }

        
        public Task<Response<List<PizzaItem>>> ListPizzas(int shopId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<UserProfileResponse>> CreateUser(CreateUserRequest user)
        {
            return await _apiService.Post<Response<UserProfileResponse>>(Urls.CREATE_USER, user);
        }

        public async Task<Response<LoginResponse>> Connexion(LoginWithCredentialsRequest userConnexion)
        {
            return await _apiService.PostConnexion<Response<LoginResponse>>(Urls.LOGIN_WITH_CREDENTIALS, userConnexion);
        }

        public async Task<Response<UserProfileResponse>> InfosUtilisateur(string tokenUtilisateur)
        {
            return await _apiService.GetInfosUtilisateur<Response<UserProfileResponse>>(Urls.USER_PROFILE, tokenUtilisateur);
        }

        public async Task<Response<UserProfileResponse>> EditerProfilUtilisateur(string tokenUtilisateur, UserProfileResponse editionUser)
        {
            return await _apiService.Patch<Response<UserProfileResponse>>(Urls.SET_USER_PROFILE, tokenUtilisateur, editionUser);
        }
        public async Task<Response<SetPasswordRequest>> ModifierMDP(string tokenUtilisateur, SetPasswordRequest editionMDP)
        {
            return await _apiService.PatchMDP<Response<SetPasswordRequest>>(Urls.SET_PASSWORD, tokenUtilisateur, editionMDP);
        }

        public async Task<Response<List<PizzaItem>>> ListePizzasRestaurant(string tokenUtilisateur, string idPizzeria)
        {
            return await _apiService.GetPizzasRestaurant<Response<List<PizzaItem>>> (Urls.LIST_PIZZA, tokenUtilisateur, idPizzeria);
        }
        public async Task<Response<ShopItem>> AjouterPizzaPanier(string tokenUtilisateur, string idShop, CreateOrderRequest idPizza)
        {
            return await _apiService.PostPizzaPanier<Response<ShopItem>>(Urls.DO_ORDER, tokenUtilisateur, idShop, idPizza);
        }

        public async Task<Response<List<OrderItem>>> MesAnciennesCommandes(string tokenUtilisateur)
        {
            return await _apiService.GetAnciennesCommandes<Response<List<OrderItem>>>(Urls.LIST_ORDERS, tokenUtilisateur);
        }

    }
}