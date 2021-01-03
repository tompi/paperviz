using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace paperviz.Text
{
    public class ScanResult
    {
        public ScanResult()
        {
            _blocks = new List<Block>();
            MinimumLeft = -1;
            MinimumTop = -1;
        }

        private readonly List<Block> _blocks;
        public double MinimumLeft { get; private set; }
        public double MaximumRight { get; private set; }
        public double MinimumTop { get; private set; }
        public double MaximumBottom { get; private set; }
        public string TallestText { get; private set; }
        private double _tallestTextHeight = 0;
        public IEnumerable<Block> Blocks { get => _blocks; }

        public void Add(Block block)
        {
            _blocks.Add(block);

            var box = block.BoundingBox;
            if (MinimumLeft < 0 || box.Left < MinimumLeft) MinimumLeft = box.Left;
            if (MinimumTop < 0 || box.Top < MinimumTop) MinimumTop = box.Top;
            if (box.Right > MaximumRight) MaximumRight = box.Right;
            if (box.Bottom > MaximumBottom) MaximumBottom = box.Bottom;

            var height = (double)(box.Bottom - box.Top) / block.Lines;
            if (height > _tallestTextHeight)
            {
                _tallestTextHeight = height;
                TallestText = RemoveSpecialCharacters(block.Text);
            }
        }
        
        // https://stackoverflow.com/questions/1120198/most-efficient-way-to-remove-special-characters-from-string
        private static string RemoveSpecialCharacters(string str) {
            return Regex.Replace(str, "[^\\w]+", "", RegexOptions.Compiled);
        }
    }
}