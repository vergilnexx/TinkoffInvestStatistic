using Android.Graphics;
using TinkoffInvestStatistic.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TinkoffInvestStatistic.Droid.Effects;

[assembly: ResolutionGroupName("TinkoffInvestStatistic.Effects")]
[assembly: ExportEffect(typeof(AndroidStrikethroughEntryEffect), nameof(StrikethroughEntryEffect))]

namespace TinkoffInvestStatistic.Droid.Effects
{
    public class AndroidStrikethroughEntryEffect : PlatformEffect
    {
        private PaintFlags _originalFlags;

        protected override void OnAttached()
        {
            var editText = Control as FormsTextView;
            if (editText is null)
            {
                return;
            }

            _originalFlags = editText.PaintFlags;

            editText.PaintFlags = PaintFlags.StrikeThruText | PaintFlags.FakeBoldText;
        }

        protected override void OnDetached()
        {
            var editText = Control as FormsTextView;
            if (editText is null)
            {
                return;
            }

            editText.PaintFlags = _originalFlags;
        }
    }
}