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
    public class DetailPizzaModel : ViewModelBase
    {
        public PizzaItem pizzaItem;
        public ShopItem shopItem;

        public string tokenUtilisateur;
        public ICommand AjouterPanier { get; set; }
        public ICommand VoirPanier { get; set; }

        public ObservableCollection<PizzaItem> detailPizzasPanier;

        private string _ingredients;
        public string Ingredients
        {
            get => _ingredients;
            set => SetProperty(ref _ingredients, value);
        }

        private string _urlImage;
        public string UrlImage
        {
            get => _urlImage;
            set => SetProperty(ref _urlImage, value);
        }
        public DetailPizzaModel()
        {           
            AjouterPanier = new Command(AjouterPann);
            VoirPanier = new Command(VoirPann);                     
        }

        public void AjouterPann()
        {
            if(pizzaItem.OutOfStock == true)
            {
                Application.Current.MainPage.DisplayAlert("Attention", "Cette pizza est en rupture de stock. Vous ne pouvez pas l'ajouter au panier","Ok");
            }
            else 
            {
                detailPizzasPanier.Add(pizzaItem);
                Application.Current.MainPage.DisplayAlert("Info","La pizza a bien été ajoutée au panier", "Ok");
                NavigationService.PopAsync();
            }
            
        }
        public void VoirPann()
        {
            NavigationService.PushAsync<Pages.PanierPage>(
                       new Dictionary<string, object>()
                       {
                        { "Token", Token },
                        { "PizzaItem", pizzaItem },
                        { "Shop", shopItem},
                        { "DetailPizzaPanier", detailPizzasPanier }
                       });
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
            get { return detailPizzasPanier; }
            set { SetProperty(ref detailPizzasPanier, value); }
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
            Token = GetNavigationParameter<string>("Token");
            Console.WriteLine("TokenD : " + Token);

            Pizza = GetNavigationParameter<PizzaItem>("PizzaItem");
            Console.WriteLine("PizzaD : " + Pizza);

            ShopItem = GetNavigationParameter<ShopItem>("Shop");
            Console.WriteLine("ShopItemD : " + ShopItem);

            DetailPizzaPanier = GetNavigationParameter<ObservableCollection<PizzaItem>>("DetailPizzaPanier");
            Console.WriteLine("DetailPizzaPanierD : " + DetailPizzaPanier);

            Ingredients = pizzaItem.Description;
            Console.WriteLine("IngredientsD : " + Ingredients);

            UrlImage = "https://pizza.julienmialon.ovh/" + Urls.GET_IMAGE.Replace("{shopId}", shopItem.Id.ToString()).Replace("{pizzaId}", pizzaItem.Id.ToString());
            Console.WriteLine("UrlImage : " + UrlImage);

        }
    }
}
