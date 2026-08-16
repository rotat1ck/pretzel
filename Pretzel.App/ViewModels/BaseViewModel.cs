using CommunityToolkit.Mvvm.ComponentModel;

namespace Pretzel.App.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    public async Task NavigateToAsync(string targetRoute, IDictionary<string, object>? parameters = null)
    {
        var navigateTo = "///" + targetRoute.TrimStart('/');

        if (parameters != null)
        {
            await Shell.Current.GoToAsync(navigateTo, true);
        }
        else
        {
            await Shell.Current.GoToAsync(navigateTo, true);
        }
    }
}
