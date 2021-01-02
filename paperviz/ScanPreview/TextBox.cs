using paperviz.Text;
using Xamarin.Forms;

namespace paperviz.ScanPreview
{
    public class TextBox
    {
        public TextBox(string text, Rectangle layoutBounds)
        {
            Text = text;
            LayoutBounds = layoutBounds;
        }
        
        public string Text { get; }
        public Rectangle LayoutBounds { get; }
    }
}