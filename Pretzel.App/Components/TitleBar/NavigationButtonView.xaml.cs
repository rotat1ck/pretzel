namespace Pretzel.App.Components.TitleBar;

public partial class NavigationButtonView : ContentView
{
    public static readonly BindableProperty ButtonImageProperty =
        BindableProperty.Create(nameof(ButtonImage), typeof(ImageSource), typeof(NavigationButtonView));

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(NavigationButtonView));

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

    public NavigationButtonView()
    {
        InitializeComponent();
    }
}