using Pretzel.App.ViewModels;

namespace Pretzel.App.Views;

public partial class ExplorePage : ContentPage
{
    public ExplorePage(ExploreViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}