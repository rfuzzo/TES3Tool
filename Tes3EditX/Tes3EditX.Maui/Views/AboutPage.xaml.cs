using Tes3EditX.Maui.ViewModels;

namespace Tes3EditX.Maui.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel viewModel)
	{
        BindingContext = viewModel;

        InitializeComponent();
	}
}