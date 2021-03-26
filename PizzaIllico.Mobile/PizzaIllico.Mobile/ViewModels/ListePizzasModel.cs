using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.Services;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Essentials;
using Storm.Mvvm.Navigation;

namespace PizzaIllico.Mobile.ViewModels
{
    public class ListePizzasModel : ViewModelBase
    {
        private ObservableCollection<PizzaItem> _pizza;

        public ShopItem shopItem;
        public ICommand SelectedPizza { get; }
        public ICommand VoirPanier { get; }

        public string tokenUtilisateur;

        public ObservableCollection<PizzaItem> detailPizzaPanier;
        public ListePizzasModel()
        {
            SelectedPizza = new Command<PizzaItem>(DetailPizza);
            VoirPanier = new Command(VoirPann);
        }

        public void VoirPann()
        {
            NavigationService.PushAsync<Pages.PanierPage>(
                        new Dictionary<string, object>()
                        {
                    { "Token", Token },
                    { "PizzaItem", "" },
                    { "Shop", shopItem},
                    { "DetailPizzaPanier", DetailPizzaPanier }
                        });
        }
        
        private async void DetailPizza(PizzaItem obj)
        {
        
            await NavigationService.PushAsync<Pages.DetailPizza>(
                        new Dictionary<string, object>()
                        {
                        { "Token", Token },
                        { "PizzaItem", obj },
                        { "Shop", shopItem},
                        { "DetailPizzaPanier", DetailPizzaPanier }
                        });
        }


        [NavigationParameter]
        public string Token
        {
            get { return tokenUtilisateur; }
            set { SetProperty(ref tokenUtilisateur, value); }
        }

        [NavigationParameter]
        public ObservableCollection<PizzaItem> Pizzas
        {
            get { return _pizza; }
            set { SetProperty(ref _pizza, value); }
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
            Pizzas = GetNavigationParameter<ObservableCollection<PizzaItem>>("ListePizzas");
            ShopItem = GetNavigationParameter<ShopItem>("Shop");
            DetailPizzaPanier = GetNavigationParameter<ObservableCollection<PizzaItem>>("DetailPizzaPanier");


        }
    }
}