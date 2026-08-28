using System.Windows.Input;

namespace Pretzel.App.Components.Explore;

public partial class ChartView : ContentView
{
    public static readonly BindableProperty DownloadCommandProperty =
        BindableProperty.Create(nameof(DownloadCommand), typeof(ICommand), typeof(ChartView));

    public ICommand DownloadCommand
    {
        get => (ICommand)GetValue(DownloadCommandProperty);
        set => SetValue(DownloadCommandProperty, value);
    }

    public ChartView()
    {
        InitializeComponent();
    }
}