namespace Pretzel.App.Components.Explore;

public partial class SearchOptionsInputView : ContentView
{
    public static readonly BindableProperty PlaceholderTextProperty =
        BindableProperty.Create(nameof(PlaceholderText), typeof(string), typeof(SearchOptionsInputView), defaultBindingMode: BindingMode.OneWayToSource);

    public static readonly BindableProperty InputProperty =
        BindableProperty.Create(nameof(Input), typeof(string), typeof(SearchOptionsInputView), defaultBindingMode: BindingMode.OneWayToSource);

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public string Input
    {
        get => (string)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    public SearchOptionsInputView()
    {
        InitializeComponent();
    }
}