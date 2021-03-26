using Xamarin.Forms.Xaml;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using PizzaIllico.Mobile.Dtos.Pizzas;

using Plugin.Geolocator;
using Storm.Mvvm.Navigation;
using System.Collections.Generic;
using PizzaIllico.Mobile.ViewModels;

namespace PizzaIllico.Mobile.Pages
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapsPage : ContentPage
    {       

        public MapsPage()
        {
            BindingContext = new MapsPageModel();
            InitializeComponent();
        }

     
    }
}
