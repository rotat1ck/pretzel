using System.Windows.Input;

namespace Pretzel.App.Components.Buttons;

public partial class ButtonView : ContentView
{
    public static readonly BindableProperty ButtonImageProperty =
        BindableProperty.Create(nameof(ButtonImage), typeof(ImageSource), typeof(ButtonView));

    public static readonly BindableProperty ButtonTextProperty =
        BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(ButtonView));

    public static readonly BindableProperty ButtonCommandProperty =
        BindableProperty.Create(nameof(ButtonCommand), typeof(ICommand), typeof(ButtonView));

    public static readonly BindableProperty ButtonCommandParameterProperty =
        BindableProperty.Create(nameof(ButtonCommandParameter), typeof(object), typeof(ButtonView));

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

    public ICommand? ButtonCommand
    {
        get => GetValue(ButtonCommandProperty) as ICommand;
        set => SetValue(ButtonCommandProperty, value);
    }

    public object? ButtonCommandParameter
    {
        get => GetValue(ButtonCommandProperty);
        set => SetValue(ButtonCommandProperty, value);
    }

    public ButtonView()
    {
        InitializeComponent();
    }

    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        border.Style = Resources["buttonBorderHoveredStyle"] as Style;
    }

    private void OnPointerExited(object sender, PointerEventArgs e)
    {
        border.Style = Resources["buttonBorderStyle"] as Style;
    }
}