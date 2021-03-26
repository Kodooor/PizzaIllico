using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaIllico.Mobile.Dtos.Pizzas;
using PizzaIllico.Mobile.ViewModels;
using Storm.Mvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PizzaIllico.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListePizzasPage 
    {
        public ListePizzasPage()
        {
            BindingContext = new ListePizzasModel();
            InitializeComponent();
        }
    }
}