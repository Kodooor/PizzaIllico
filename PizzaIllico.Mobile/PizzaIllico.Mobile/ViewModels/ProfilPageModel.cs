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
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class ProfilPageModel : ViewModelBase
    {
        public string tokenUtilisateur;

        public string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string _nom;
        public string Nom
        {
            get { return _nom; }
            set { SetProperty(ref _nom, value); }
        }

        public string _prenom;
        public string Prenom
        {
            get { return _prenom; }
            set { SetProperty(ref _prenom, value); }
        }

        public string _telephone;
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        public string MotDePasse { get; set; }
        public string MotDePasseConfirmation { get; set; }

        public ICommand ModifierMotDePasse { get; set; }
        public ICommand EditerProfil { get; }
        public ProfilPageModel()
        {           
            EditerProfil = new Command(EditionProfil);
            ModifierMotDePasse = new Command(ModificationMotDePasse);            
        }

        public async void getInfosUtilisateur()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();
            UserProfileResponse userProfile = new UserProfileResponse();

            var response = await service.InfosUtilisateur(Token);

            if (response.IsSuccess)
            {
                Email = response.Data.Email;
                Nom = response.Data.LastName;
                Prenom = response.Data.FirstName;
                Telephone = response.Data.PhoneNumber;               
            }
        }

        public async void EditionProfil()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();
            UserProfileResponse userProfilResponse = new UserProfileResponse();
            userProfilResponse.Email = Email;
            userProfilResponse.LastName = Nom;
            userProfilResponse.FirstName = Prenom;
            userProfilResponse.PhoneNumber = Telephone;

            Response<UserProfileResponse> response = await service.EditerProfilUtilisateur(tokenUtilisateur, userProfilResponse);

            if (response.IsSuccess)
            {
                Email = response.Data.Email;
                Nom = response.Data.LastName;
                Prenom = response.Data.FirstName;
                Telephone = response.Data.PhoneNumber;
                await NavigationService.PushAsync<Pages.ProfilPage>(
                        new Dictionary<string, object>()
                        {

                        { "Token", tokenUtilisateur }
                        });
            }
        }
        public async void ModificationMotDePasse()
        {
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();
            SetPasswordRequest setPasswordRequest = new SetPasswordRequest();

            setPasswordRequest.OldPassword = MotDePasse;
            setPasswordRequest.NewPassword = MotDePasseConfirmation;

            Response<SetPasswordRequest> response = await service.ModifierMDP(tokenUtilisateur, setPasswordRequest);

            if (response.IsSuccess)
            {
                CrossSettings.Current.AddOrUpdateValue("login", "");
                CrossSettings.Current.AddOrUpdateValue("password", "");
                CrossSettings.Current.AddOrUpdateValue("token", "");
                await NavigationService.PushAsync<Pages.ConnexionPage>();
                       
            }
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
            getInfosUtilisateur();
        }
    }
}