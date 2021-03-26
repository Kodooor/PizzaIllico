using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.Services;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class PanierPageModel : ViewModelBase
    {
        public string tokenUtilisateur;
        public ShopItem shopItem;
        public PizzaItem pizzaItem;

        private ObservableCollection<OrderItem> _ancCommandes;

        public ObservableCollection<OrderItem> AncCommandes
        {
            get => _ancCommandes;
            set => SetProperty(ref _ancCommandes, value);
        }

        public ICommand SelectCommand { get; }

        public List<long> listePizzaPanier;

        public ObservableCollection<PizzaItem> detailPizzaPanier;

        public ICommand ValiderCommande { get; }
        public ICommand AnciennesCommandes { get; }

        public PanierPageModel()
        {           
            listePizzaPanier = new List<long>();
            ValiderCommande = new Command(ValidCommande);
            AnciennesCommandes = new Command(AncienCommandes);
            SelectCommand = new Command<PizzaItem>(RemoveElement);
        }

        public async void RemoveElement(PizzaItem value)
        {
            var confirmed = await Application.Current.MainPage.DisplayAlert("Alert", "Voulez vous vraiment supprimer l'élément sélectionné ?", "Oui", "Non");
            if (confirmed)
            {               
               DetailPizzaPanier.Remove(value);      
            }
        }
        public async void ValidCommande()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();

            
            foreach(PizzaItem pizza in detailPizzaPanier)
            {
                listePizzaPanier.Add(pizza.Id);
            }

            CreateOrderRequest createOrderRequest = new CreateOrderRequest();
            createOrderRequest.PizzaIds = listePizzaPanier;

            Response<ShopItem> response = await service.AjouterPizzaPanier(tokenUtilisateur, shopItem.Id.ToString(), createOrderRequest);

            if (response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", "Votre commande a bien été prise en compte", "OK");
                await NavigationService.PushAsync<Pages.ShopListPage>(
                    new Dictionary<string, object>()
                    {
                    { "Token", Token }
                    });
            }
        }

        public async void AncienCommandes()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();

            Response<List<OrderItem>> response = await service.MesAnciennesCommandes(Token);
            Console.WriteLine("response" + response.Data);
            if (response.IsSuccess)
            {
                Console.WriteLine("responseOK");

                AncCommandes = new ObservableCollection<OrderItem>(response.Data);
                await NavigationService.PushAsync<Pages.AnciennesCommandesPage>(
                      new Dictionary<string, object>()
                      {
                        { "Token", Token },
                        { "AnciennesCommandes", AncCommandes },              
                      });
            }
        }

        [NavigationParameter]
        public string Token
        {
            get { return tokenUtilisateur; }
            set { SetProperty(ref tokenUtilisateur, value); }
        }

        [NavigationParameter]
        public PizzaItem Pizza
        {
            get { return pizzaItem; }
            set { SetProperty(ref pizzaItem, value); }
        }

        [NavigationParameter]
        public ShopItem ShopItem
        {
            get { return shopItem; }
            set { SetProperty(ref shopItem, value); }
        }

        [NavigationParameter]
        public ObservableCollection<PizzaItem> DetailPizzaPanier
        {
            get { return detailPizzaPanier; }
            set { SetProperty(ref detailPizzaPanier, value); }
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
            Token = GetNavigationParameter<string>("Token");
            Pizza = GetNavigationParameter<PizzaItem>("ListePizzas");
            ShopItem = GetNavigationParameter<ShopItem>("Shop");
            DetailPizzaPanier = GetNavigationParameter<ObservableCollection<PizzaItem>>("DetailPizzaPanier");
        }
    }
}
        
    
