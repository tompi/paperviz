using System.Collections.Generic;

namespace paperviz.Text
{
    public class ScanResult
    {
        public ScanResult()
        {
            Blocks = new List<Block>();
        }

        public List<Block> Blocks { get; }
    }
}