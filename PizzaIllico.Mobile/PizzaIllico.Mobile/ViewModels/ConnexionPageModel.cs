using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Newtonsoft.Json;
using PizzaIllico.Mobile.Dtos.Authentications.Credentials;
using PizzaIllico.Mobile.Services;
using Plugin.Settings;
using Storm.Mvvm;
using Xamarin.Forms;

namespace PizzaIllico.Mobile.ViewModels
{
    public class ConnexionPageModel : ViewModelBase
    {

        public string accessToken = "";

        private bool _running;

        public bool Running
        {
            get => _running;
            set => SetProperty(ref _running, value);
        }

        private string _login;
        public string Login
        {
            get => _login;            
            set => SetProperty(ref _login, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }
       
        public ICommand Connexion { get; }
        public ICommand Inscription { get; }

        public ConnexionPageModel()
        {

            string login = CrossSettings.Current.GetValueOrDefault("login", string.Empty);
            string password = CrossSettings.Current.GetValueOrDefault("password", string.Empty);
            string token = CrossSettings.Current.GetValueOrDefault("token", string.Empty);

            if (!login.Equals("") && !password.Equals("") && !token.Equals(""))
            {
                Login = JsonConvert.DeserializeObject<string>(login);
                Password = JsonConvert.DeserializeObject<string>(password);
                accessToken = JsonConvert.DeserializeObject<string>(token);
                Connect();
            }
           
            Connexion = new Command(Connect);
            Inscription = new Command(Inscript);
        }


        public async void Connect()
        {
            Running = true;
            IPizzaApiService service = DependencyService.Get<IPizzaApiService>();
            LoginWithCredentialsRequest userConnexion = new LoginWithCredentialsRequest();
            userConnexion.Login = Login;
            userConnexion.Password = Password;
            userConnexion.ClientId = "MOBILE";
            userConnexion.ClientSecret = "UNIV";
           
            var response = await service.Connexion(userConnexion);

            if (response.IsSuccess)
            {
                string log = CrossSettings.Current.GetValueOrDefault("login", string.Empty);
                string passw = CrossSettings.Current.GetValueOrDefault("password", string.Empty);
                string tok = CrossSettings.Current.GetValueOrDefault("token", string.Empty);

                if (log.Equals("") || passw.Equals("") || tok.Equals(""))
                {
                    var confirmed = await Application.Current.MainPage.DisplayAlert("Alert", "Souhaitez vous rester connecté ?", "Oui", "Non");
                    if (confirmed)
                    {
                        // Gestion du stockage
                        var login = JsonConvert.SerializeObject(_login);
                        var password = JsonConvert.SerializeObject(_password);
                        var token = JsonConvert.SerializeObject(accessToken);

                        CrossSettings.Current.AddOrUpdateValue("login", login);
                        CrossSettings.Current.AddOrUpdateValue("password", password);
                        CrossSettings.Current.AddOrUpdateValue("token", token);
                    }
                }
                accessToken = response.Data.AccessToken;
                await NavigationService.PushAsync<Pages.ShopListPage>(
                    new Dictionary<string, object>()
                    {
                    { "Token", accessToken }
                    });
                 
                // Activity indicator
                Running = false;
            }
            else
            {
                await NavigationService.PushAsync(new Pages.ConnexionPage());
            }
        }

        public void Inscript()
        {
            NavigationService.PushAsync<Pages.InscriptionPage>();
        }
    }
}