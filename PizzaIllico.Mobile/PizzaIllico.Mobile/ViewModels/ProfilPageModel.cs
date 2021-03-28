using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.Services;
using Plugin.Settings;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class ProfilPageModel : ViewModelBase
    {
        private string tokenUtilisateur;
        public ICommand Deconnexion { get; }
        public ICommand goEditerProfil { get; }
        public ICommand goEditerMdp { get; }
        public ICommand goAnciennesCommandes { get; }

        private ObservableCollection<OrderItem> _ancCommandes;

        public ObservableCollection<OrderItem> AncCommandes
        {
            get => _ancCommandes;
            set => SetProperty(ref _ancCommandes, value);
        }

        public ProfilPageModel()
        {                      
            Deconnexion = new Command(Deco);
            goEditerProfil = new Command(GoEditProfil);
            goEditerMdp = new Command(GoEditMdp);
            goAnciennesCommandes = new Command(AncienCommandes);
        }
        public async void GoEditProfil()
        {
            await NavigationService.PushAsync<Pages.EditionProfilPage>(
                        new Dictionary<string, object>()
                        {
                                { "Token", Token }
                        });
        }

        public async void GoEditMdp()
        {
            await NavigationService.PushAsync<Pages.ModificationMdpPage>(
                        new Dictionary<string, object>()
                        {
                                { "Token", Token }
                        });
        }

        public async void AncienCommandes()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();

            Response<List<OrderItem>> response = await service.MesAnciennesCommandes(Token);
            Console.WriteLine("response" + Token);
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

        public async void Deco()
        {
            CrossSettings.Current.AddOrUpdateValue("login", "");
            CrossSettings.Current.AddOrUpdateValue("password", "");
            CrossSettings.Current.AddOrUpdateValue("token", "");
            await NavigationService.PushAsync<Pages.ConnexionPage>();
        }


        [NavigationParameter]
        public string Token
        {
            get { return tokenUtilisateur; }
            set { SetProperty(ref tokenUtilisateur, value); }
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
            Token = GetNavigationParameter<string>("Token");
        }
    }
}

