namespace Pretzel.App.Components.TitleBar;

public partial class NavigationButtonView : ContentView
{
    public static readonly BindableProperty ButtonImageProperty =
        BindableProperty.Create(nameof(ButtonImage), typeof(ImageSource), typeof(NavigationButtonView));

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(NavigationButtonView));

    public static readonly BindableProperty TargetRouteProperty =
        BindableProperty.Create(nameof(TargetRoute), typeof(string), typeof(NavigationButtonView), string.Empty);

    public static readonly BindableProperty IsActiveProperty =
        BindableProperty.Create(nameof(IsActive), typeof(bool), typeof(NavigationButtonView), false,
            propertyChanged: OnIsActiveChanged);

    public ImageSource? ButtonImage
    {
        get => GetValue(ButtonImageProperty) as ImageSource;
        set => SetValue(ButtonImageProperty, value);
    }

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public string TargetRoute
    {
        get => (string)GetValue(TargetRouteProperty);
        set => SetValue(TargetRouteProperty, value);
    }

    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    private static void OnIsActiveChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var view = (NavigationButtonView)bindable;
        VisualStateManager.GoToState(view, (bool)newValue ? "Navigated" : "NotNavigated");
    }

    public NavigationButtonView()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        Shell.Current.Navigated += OnShellNavigated;
        UpdateActiveState();
    }

    private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        UpdateActiveState();
    }

    private void UpdateActiveState()
    {
        var currentLocation = Shell.Current.CurrentState.Location.ToString();
        IsActive = currentLocation.Contains(TargetRoute);
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        if (Shell.Current is not null)
        {
            Shell.Current.Navigated -= OnShellNavigated;
        }
    }
}