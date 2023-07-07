using Tes3EditX.Maui.ViewModels;

namespace Tes3EditX.Maui.Views;

public partial class PluginSelectPage : ContentPage
{
	public PluginSelectPage(PluginSelectViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
	}
}