using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MvvmHelpers.Commands;
using paperviz.Export.Json;
using paperviz.Export.Text;
using Prism.AppModel;
using Prism.Mvvm;
using Xamarin.Forms;

namespace paperviz.ScanPreview
{
    [UsedImplicitly]
    public class ScanPreviewPageViewModel : BindableBase, IPageLifecycleAware
    {
        private readonly CurrentImageService _currentImageService;
        private readonly TextExportService _textExportService;
        private readonly JsonExportService _jsonExportService;

        public ScanPreviewPageViewModel(CurrentImageService currentImageService, TextExportService textExportService, JsonExportService jsonExportService)
        {
            _textExportService = textExportService;
            _jsonExportService = jsonExportService;
            _currentImageService = currentImageService;
            TextCommand = new AsyncCommand(ToText);
            JsonCommand = new AsyncCommand(ToJson);
        }

        private Task ToJson()
        {
            return _jsonExportService.Export(_currentImageService.ScanResult);
        }

        private Task ToText()
        {
            return _textExportService.Export(_currentImageService.ScanResult);
        }

        public List<TextBox> TextBoxes { get; private set; }

        public AsyncCommand TextCommand { get; }
        public AsyncCommand JsonCommand { get; }

        public void OnAppearing()
        {
            var yOffset = _currentImageService.ScanResult.MinimumTop;
            var xOffset = _currentImageService.ScanResult.MinimumLeft;
            var scanWidth = _currentImageService.ScanResult.MaximumRight - xOffset;
            var scanHeight = _currentImageService.ScanResult.MaximumBottom - yOffset;
            var xScale = App.ScreenWidth / scanWidth;
            // TODO: We are assuming portrait photo here...
            var yScale = xScale;

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