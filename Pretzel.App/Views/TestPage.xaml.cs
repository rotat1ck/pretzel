using Pretzel.App.ViewModels;

namespace Pretzel.App.Views;

public partial class TestPage : ContentPage
{
    public TestPage(TestViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}