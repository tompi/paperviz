using System.Collections.Generic;

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
        public IEnumerable<Block> Blocks { get => _blocks; }

        public void Add(Block block)
        {
            _blocks.Add(block);

            var box = block.BoundingBox;
            if (MinimumLeft < 0 || box.Left < MinimumLeft) MinimumLeft = box.Left;
            if (MinimumTop < 0 || box.Top < MinimumTop) MinimumTop = box.Top;
            if (box.Right > MaximumRight) MaximumRight = box.Right;
            if (box.Bottom > MaximumBottom) MaximumBottom = box.Bottom;
        }
        
    }
}