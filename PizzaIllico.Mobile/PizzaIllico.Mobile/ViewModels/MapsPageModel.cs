using PizzaIllico.Mobile.Dtos.Pizzas;
using Plugin.Geolocator;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PizzaIllico.Mobile.ViewModels
{
    public class MapsPageModel : ViewModelBase
    {
        public ObservableCollection<ShopItem> lesMagasins;
        public Position maPos;

        private Map _map;

        public Map MyMap
        {
            get { return _map; }
            set { SetProperty(ref _map, value); }
        }

        public MapsPageModel()
        {
            MyMap = new Xamarin.Forms.Maps.Map();
            MyMap.IsShowingUser = true;

        }

        public async void GetCurrentLocation()
        {
            var m = await CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromSeconds(1000));
            maPos = new Position(m.Latitude, m.Longitude);

        }


        [NavigationParameter]
        public Position Position
        {
            get { return maPos; }
            set {SetProperty(ref maPos, value);}
        }

        [NavigationParameter]
        public ObservableCollection<ShopItem> Shops
        {
            get { return lesMagasins; }
            set{SetProperty(ref lesMagasins, value);}
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
            Position = GetNavigationParameter<Position>("Position");
            Shops = GetNavigationParameter<ObservableCollection<ShopItem>>("Shops");

            Console.WriteLine("LatituteLongitudeMap" + maPos.Latitude + maPos.Longitude);

            for (int i = 0; i < lesMagasins.Count; i++)
            {
                Pin pin = new Pin
                {
                    Label = lesMagasins[i].Name,
                    Address = lesMagasins[i].Address,
                    Type = PinType.Place,
                    Position = new Position(lesMagasins[i].Latitude, lesMagasins[i].Longitude)
                };
                MyMap.Pins.Add(pin);
            }
            Device.BeginInvokeOnMainThread(() => {
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(maPos.Latitude, maPos.Longitude), Distance.FromMeters(10000)));

            });
        }
    }

}
