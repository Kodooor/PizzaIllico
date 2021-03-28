using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Pizzas;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class AnciennesCommandesModel : ViewModelBase
    {
        public string tokenUtilisateur;
        public ICommand GoListeShop { get; }
        private ObservableCollection<OrderItem> _anciennesCommandes;
                  
        public AnciennesCommandesModel()
        {          
            GoListeShop = new Command(goPageListeShop);
        }

        private void goPageListeShop()
        {
             NavigationService.PushAsync<Pages.ShopListPage>(
                        new Dictionary<string, object>()
                        {
                        { "Token", Token }
                        });
        }


        [NavigationParameter]
        public string Token
        {
            get { return tokenUtilisateur; }
            set { SetProperty(ref tokenUtilisateur, value); }
        }

        [NavigationParameter]
        public ObservableCollection<OrderItem> AnciennesCommandes
        {
            get { return _anciennesCommandes; }
            set { SetProperty(ref _anciennesCommandes, value); }
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
            Token = GetNavigationParameter<string>("Token");
            AnciennesCommandes = GetNavigationParameter<ObservableCollection<OrderItem>>("AnciennesCommandes");
        }
    }
}