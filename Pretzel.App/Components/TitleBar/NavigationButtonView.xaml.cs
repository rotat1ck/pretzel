using Pretzel.App.Components.Buttons;
using System.Windows.Input;

namespace Pretzel.App.Components.TitleBar;

public partial class NavigationButtonView : ButtonView
{
    public static readonly BindableProperty TargetRouteProperty =
        BindableProperty.Create(nameof(TargetRoute), typeof(string), typeof(NavigationButtonView), string.Empty);

    public string TargetRoute
    {
        get => (string)GetValue(TargetRouteProperty);
        set => SetValue(TargetRouteProperty, value);
    }

    private bool IsActive { get; set; }
    private Border? buttonBorder;

    public override ICommand? ButtonCommand
    {
        get => field ??= new Command(async () =>
        {
            if (!string.IsNullOrEmpty(TargetRoute) && Shell.Current is not null)
            {
                var navigateTo = "///" + TargetRoute.TrimStart('/');
                await Shell.Current.GoToAsync(navigateTo);
            }
        });
        set;
    }

    public NavigationButtonView()
    {
        InitializeComponent();
        buttonBorder = FindByName("border") as Border;

        if (buttonBorder is not null)
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        if (Shell.Current is not null)
        {
            Shell.Current.Navigated += OnShellNavigated;
            UpdateActiveState();
        }
    }

    private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        UpdateActiveState();
    }

    private void UpdateActiveState()
    {
        if (!string.IsNullOrEmpty(TargetRoute))
        {
            var currentLocation = Shell.Current.CurrentState.Location.ToString().TrimStart('/').TrimEnd('/');
            var tagetLocation = TargetRoute.TrimStart('/').TrimEnd('/');
            IsActive = currentLocation == tagetLocation;

            var style = IsActive ? Resources["buttonBorderNavigatedStyle"] : Resources["buttonBorderStyle"];
            if (buttonBorder is not null)
            {
                buttonBorder.Style = style as Style;
            }
        }
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        if (Shell.Current is not null)
        {
            Shell.Current.Navigated -= OnShellNavigated;
        }
    }

    protected override void OnPointerEntered(object sender, PointerEventArgs e)
    {
        if (!IsActive)
        {
            base.OnPointerEntered(sender, e);
        }
    }

    protected override void OnPointerExited(object sender, PointerEventArgs e)
    {
        if (!IsActive)
        {
            base.OnPointerExited(sender, e);
        }
    }
}