using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using PizzaIllico.Mobile.Dtos;
using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.Services;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Essentials;
using Storm.Mvvm.Navigation;

namespace PizzaIllico.Mobile.ViewModels
{
    public class ShopListViewModel : ViewModelBase
    {
		ActivityIndicator activityIndicator = new ActivityIndicator();
		public string tokenUtilisateur;
	    private ObservableCollection<ShopItem> _shops;

	    public ObservableCollection<ShopItem> Shops
	    {
		    get => _shops;
		    set => SetProperty(ref _shops, value);
	    }

		private bool _running;
		public bool Running
		{
			get => _running;
			set => SetProperty(ref _running, value);
		}

		public ICommand VoirProfil { get; }
		public ICommand SelectedCommand { get; }

		public ICommand GoMapsPage { get; }

		public ObservableCollection<PizzaItem> detailPizzasPanier;

		private Xamarin.Forms.Maps.Position pos;

		public ShopListViewModel()
        {
			VoirProfil = new Command(VoirProf);
			SelectedCommand = new Command<ShopItem>(ListePizzasRestaurant);
			GoMapsPage = new Command(GoPageMaps);
			GetCurrentLocation();
		}

		public async void GetCurrentLocation()
		{
			var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
			var m = await Geolocation.GetLocationAsync(request);
			pos = new Xamarin.Forms.Maps.Position(m.Latitude, m.Longitude);
			
		}

		private async void VoirProf()
        {
			await NavigationService.PushAsync<Pages.ProfilPage>(
						new Dictionary<string, object>()
						{
						{ "Token", Token }
						});
		}

		private async void GoPageMaps()
		{
			Console.WriteLine("eeeeeeeeeee" + pos + " " + Shops);
			await NavigationService.PushAsync<Pages.MapsPage>(
						new Dictionary<string, object>()
						{
						{ "Position", pos },
						{ "Shops", Shops}
						});
		}
		
		//Trie les restaurant du plus proche au plus de notre position
		private ObservableCollection<ShopItem> trierRestaurant(Response<List<ShopItem>> response)
        {
			ObservableCollection<ShopItem> listeTriee = new ObservableCollection<ShopItem>();

			
			int nbElement = response.Data.Count;
			int a = 0;

			while (a < nbElement)				
			{
				double distance = 100000000000000;
				int indiceDistance = 0;
				for (int i = 0; i < response.Data.Count; i++)
				{
					double distanceCourante = Location.CalculateDistance(
											new Location(pos.Latitude,pos.Longitude),
											new Location(response.Data[i].Latitude, response.Data[i].Longitude),
											DistanceUnits.Kilometers);
					if (distanceCourante < distance)
					{
						distance = distanceCourante;
						indiceDistance = i;
					}
					else
					{
						// nothing
					}
				}
				response.Data[indiceDistance].Distance = Math.Ceiling(distance);
				listeTriee.Add(response.Data[indiceDistance]);
				
				response.Data.Remove(response.Data[indiceDistance]);
				a++;
			}
			return listeTriee;
        }

		public override async Task OnResume()
        {
			await base.OnResume();
			Console.WriteLine("positioooon" + pos.Latitude);

			IPizzaApiService service = DependencyService.Get<IPizzaApiService>();

	        Response<List<ShopItem>> response = await service.ListShops();
			
			Console.WriteLine($"Appel HTTP : {response.IsSuccess}");
	        if (response.IsSuccess)
	        {
				Shops = trierRestaurant(response);
				
			}
			activityIndicator.IsRunning = false;
		}

		private async void ListePizzasRestaurant(ShopItem obj)
		{
			
			IPizzaApiService service = DependencyService.Get<IPizzaApiService>();

			Response<List<PizzaItem>> response = await service.ListePizzasRestaurant(Token, obj.Id.ToString());

			 
			if (response.IsSuccess)
			{
				detailPizzasPanier = new ObservableCollection<PizzaItem>();
				ObservableCollection<PizzaItem> listePizzas = new ObservableCollection<PizzaItem>(response.Data);

				Console.WriteLine("Token : " + Token + "Pizzas : " + response.Data + "ShopItem : " + obj + "DetailPizzaPanier : " + detailPizzasPanier);

				await NavigationService.PushAsync<Pages.ListePizzasPage>(
						new Dictionary<string, object>()
						{
						{ "Token", Token },
						{ "ListePizzas", listePizzas },
						{ "Shop", obj},
						{ "DetailPizzaPanier", detailPizzasPanier }
						});
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
			Console.WriteLine("positioooon" + pos.Latitude);
		}
	}
}