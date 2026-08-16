using CommunityToolkit.Maui.Views;
using System.ComponentModel;

namespace Pretzel.App.Components;

public class ExpanderAnimationBehavior : Behavior<Expander>
{
    private Expander expander = null!;
    private VisualElement? content;
    private bool isAnimating;

    public int Duration { get; set; } = 300;
    public Easing Easing { get; set; } = Easing.CubicInOut;

    protected override void OnAttachedTo(Expander bindable)
    {
        base.OnAttachedTo(bindable);
        expander = bindable;

        expander.ExpandedChanged += OnExpanderExpandedChanged;
        expander.PropertyChanged += OnExpanderPropertyChanged;
        UpdateContent();
    }

    protected override void OnDetachingFrom(Expander bindable)
    {
        if (expander is not null)
        {
            expander.ExpandedChanged -= OnExpanderExpandedChanged;
            expander.PropertyChanged -= OnExpanderPropertyChanged;
        }
        base.OnDetachingFrom(bindable);
    }

    private void UpdateContent() => content = expander?.Content as VisualElement;

    private void OnExpanderPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Expander.Content))
        {
            UpdateContent();
        }
    }

    /*
     * i dont like this, this "animation" is such a problem
     * the opacity trick to remove content flickering, height resets etc..
     * just hope it'll not cause issues with reusing it later
    */
    private async void OnExpanderExpandedChanged(object? sender, EventArgs e)
    {
        if (isAnimating || content is null)
        {
            return;
        }

        content.AbortAnimation("ExpandAnimation");
        isAnimating = true;

        try
        {
            if (!expander.IsExpanded)
            {
                return;
            }

            content.Opacity = 0;
            content.ClearValue(VisualElement.HeightRequestProperty);

            await Task.Delay(10);

            double targetHeight = content.Measure(content.Width, double.PositiveInfinity).Height;
            if (targetHeight > 0)
            {
                content.HeightRequest = 0;
                content.Opacity = 1;
                content.Animate(
                    "ExpandAnimation",
                    v => content.HeightRequest = v,
                    0, targetHeight,
                    16, (uint)Duration, Easing);

                content.ClearValue(VisualElement.HeightRequestProperty);
            }
        }
        finally
        {
            isAnimating = false;
        }
    }
}
