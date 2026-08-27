using Pretzel.Core.Models.Chart;

namespace Pretzel.App.Components.Explore;

public partial class ChartInstrumentView : ContentView
{
    public ChartInstrumentView()
    {
        InitializeComponent();
    }

    private async void InstrumentImage_Loaded(object sender, EventArgs e)
    {
        if (BindingContext is not ChartInstrument binding)
        {
            return;
        }

        string instrumentName = binding.Instrument.ToString().ToLower();
        instrumentImage.Source = ImageSource.FromFile($"instrument_{instrumentName}.png");
    }
}
