using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
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
        public ProfilPageModel()
        {                      
            Deconnexion = new Command(Deco);
            goEditerProfil = new Command(GoEditProfil);
            goEditerMdp = new Command(GoEditMdp);
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

