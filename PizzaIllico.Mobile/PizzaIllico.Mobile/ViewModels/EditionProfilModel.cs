using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Services;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class EditionProfilModel : ViewModelBase
    {
        public string tokenUtilisateur;

        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private string _nom;
        public string Nom
        {
            get { return _nom; }
            set { SetProperty(ref _nom, value); }
        }

        private string _prenom;
        public string Prenom
        {
            get { return _prenom; }
            set { SetProperty(ref _prenom, value); }
        }

        private string _telephone;
        public string Telephone
        {
            get { return _telephone; }
            set { SetProperty(ref _telephone, value); }
        }

        public ICommand EditerProfil { get; }
        public EditionProfilModel()
        {
            EditerProfil = new Command(EditionProfil);
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

            if (response.IsSuccess && !Email.Equals("") && !Nom.Equals("") && !Prenom.Equals("") && !Telephone.Equals(""))
            {
                Email = response.Data.Email;
                Nom = response.Data.LastName;
                Prenom = response.Data.FirstName;
                Telephone = response.Data.PhoneNumber;
                await Application.Current.MainPage.DisplayAlert("Indication", "Votre profil a bien été modifié", "Ok");

                await NavigationService.PushAsync<Pages.ProfilPage>(
                        new Dictionary<string, object>()
                        {

                        { "Token", tokenUtilisateur }
                        });
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", "Veuillez remplir tous les champs", "Ok");
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