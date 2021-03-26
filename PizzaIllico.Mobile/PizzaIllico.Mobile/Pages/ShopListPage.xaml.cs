using PizzaIllico.Mobile.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace PizzaIllico.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShopListPage
    {
        /* public ShopListPage(string accessToken)
         {
             BindingContext = new ShopListViewModel(accessToken);
             InitializeComponent();
         }*/

         public ShopListPage()
        {
            BindingContext = new ShopListViewModel();
            InitializeComponent();
        }
    }
}