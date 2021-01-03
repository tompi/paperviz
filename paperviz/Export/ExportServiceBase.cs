using System;
using System.Threading.Tasks;
using paperviz.Text;

namespace paperviz.Export
{
    public abstract class ExportServiceBase
    {
        protected readonly string _fileFolder;

        public abstract Task Export(ScanResult scanResult);

        public ExportServiceBase()
        {
            _fileFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        protected static string GetFileName(ScanResult scanResult, string suffix)
        {
            var dateString = DateTime.Now.ToString("yy-MM-dd_HH:mm");
            return $"{App.Title}_{dateString}_{scanResult.TallestText}.{suffix}";
        }
    }
}