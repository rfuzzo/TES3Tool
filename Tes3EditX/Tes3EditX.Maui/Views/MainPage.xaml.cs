﻿using Tes3EditX.Maui.ViewModels;

namespace Tes3EditX.Maui.Views;

public partial class MainPage : FlyoutPage
{
    int count = 0;

    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;

        InitializeComponent();
    }


}