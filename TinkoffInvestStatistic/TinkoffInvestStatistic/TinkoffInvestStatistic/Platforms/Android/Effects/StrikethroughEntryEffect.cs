using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Platform;
using TinkoffInvestStatistic.Effects;
using TinkoffInvestStatistic.Platforms.Android.Effects;

[assembly: ResolutionGroupName("TinkoffInvestStatistic.Effects")]
[assembly: ExportEffect(typeof(AndroidStrikethroughEntryEffect), nameof(StrikethroughEntryEffect))]

namespace TinkoffInvestStatistic.Platforms.Android.Effects;

public class AndroidStrikethroughEntryEffect : PlatformEffect
{
    private PaintFlags _originalFlags;

    protected override void OnAttached()
    {
        if (Control is TextView tv)
        {
            _originalFlags = tv.PaintFlags;
            tv.PaintFlags = PaintFlags.StrikeThruText | PaintFlags.FakeBoldText;
        }
    }

    protected override void OnDetached()
    {
        if (Control is TextView tv)
        {
            tv.PaintFlags = _originalFlags;
        }
    }
}
