using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
using PizzaIllico.Mobile.Services;
using Plugin.Settings;
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
    public class ModificationMdpModel : ViewModelBase
    {

        private string tokenUtilisateur;
        public string MotDePasse { get; set; }
        public string MotDePasseConfirmation { get; set; }
        public string MotDePasseConfirmationConfirmation { get; set; }

        public ICommand ModifierMotDePasse { get; set; }

        public ModificationMdpModel()
        {
            ModifierMotDePasse = new Command(ModificationMotDePasse);
        }

        public async void ModificationMotDePasse()
        {
            if (MotDePasseConfirmation.Equals(MotDePasseConfirmationConfirmation))
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
                    await Application.Current.MainPage.DisplayAlert("Indication", "Votre mot de passe a bien été modifié", "Ok");

                    await NavigationService.PushAsync<Pages.ConnexionPage>();

                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alerte", "Votre mot de passe actuel est incorrect", "Ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Alerte", "Saisissez le même nouveau mdp dans les deux derniers champs ", "Ok");
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
        }
    }
}