using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PizzaIllico.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PanierPage : ContentPage
    {
        public PanierPage()
        {
            BindingContext = new PanierPageModel();
            InitializeComponent();
        }

    }
}