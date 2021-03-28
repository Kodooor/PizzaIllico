using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Accounts;
using PizzaIllico.Mobile.Services;
using Plugin.Settings;
using Storm.Mvvm;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class InscriptionPageModel : ViewModelBase
    {

        public string Email { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string MotDePasse { get; set; }

        public string MotDePasseConfirmation { get; set; }
        public ICommand Inscription { get; }
        public InscriptionPageModel()
        {
            Inscription = new Command(validerInscription);
        }

        public async void validerInscription(Object obj)
        {
            if (MotDePasse.Equals(MotDePasseConfirmation))
            {
                IPizzaApiService service = DependencyService.Get<IPizzaApiService>();
                CreateUserRequest user = new CreateUserRequest();
                user.ClientId = "MOBILE";
                user.ClientSecret = "UNIV";
                user.Email = Email;
                user.FirstName = Prenom;
                user.LastName = Nom;
                user.Password = MotDePasse;
                user.PhoneNumber = Telephone;

                var response = await service.CreateUser(user);

                if (response.IsSuccess)
                {
                    CrossSettings.Current.AddOrUpdateValue("login", "");
                    CrossSettings.Current.AddOrUpdateValue("password", "");
                    CrossSettings.Current.AddOrUpdateValue("token", "");
                    await Application.Current.MainPage.DisplayAlert("Indication", "Votre inscription a bien été prise en compte", "Ok");
                    await NavigationService.PushAsync<Pages.ConnexionPage>();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerte", "Veuillez remplir tous les champs", "ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", "Les mots de passe doivent correspondre", "ok");
            }

        }
    }
}