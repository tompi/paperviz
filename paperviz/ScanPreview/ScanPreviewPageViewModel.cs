using System.Collections.Generic;
using JetBrains.Annotations;
using Prism.AppModel;
using Prism.Mvvm;
using Xamarin.Forms;

namespace paperviz.ScanPreview
{
    [UsedImplicitly]
    public class ScanPreviewPageViewModel : BindableBase, IPageLifecycleAware
    {
        private readonly CurrentImageService _currentImageService;

        public ScanPreviewPageViewModel(CurrentImageService currentImageService)
        {
            _currentImageService = currentImageService;
        }

        public List<TextBox> TextBoxes { get; private set; }

        public void OnAppearing()
        {
            var yOffset = _currentImageService.ScanResult.MinimumTop;
            var xOffset = _currentImageService.ScanResult.MinimumLeft;
            var yScale = App.ScreenHeight / (_currentImageService.ScanResult.MaximumBottom - yOffset);
            var xScale = App.ScreenWidth /(_currentImageService.ScanResult.MaximumRight - xOffset);

            TextBoxes = new List<TextBox>();

            foreach (var block in _currentImageService.ScanResult.Blocks)
            {
                var layoutBounds = new Rectangle
                {
                    Left = (block.BoundingBox.Left - xOffset) * xScale,
                    Right = (block.BoundingBox.Right - xOffset) * xScale,
                    Top = (block.BoundingBox.Top - yOffset) * yScale,
                    Bottom = (block.BoundingBox.Bottom - yOffset) * yScale
                };
                TextBoxes.Add(new TextBox(block.Text, layoutBounds));
            }

            RaisePropertyChanged(nameof(TextBoxes));
        }

        public void OnDisappearing()
        {
        }
    }
}