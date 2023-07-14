using Tes3EditX.Backend.ViewModels;

namespace Tes3EditX.Maui.Views;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }


}