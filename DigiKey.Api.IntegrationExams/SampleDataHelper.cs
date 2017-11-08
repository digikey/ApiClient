using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiKey.Api.IntegrationExams
{
    /// <summary>
    ///     Helper function for creating path to files in SampleData folder under Test Project
    /// </summary>
    public static class SampleDataHelper
    {
        private static readonly string _sampleDataPath = Path.Combine(Path.GetFullPath(@"..\..\"), "SampleData");

        public static string PathFor(string sampleFile)
        {
            return Path.Combine(_sampleDataPath, sampleFile);
        }

        public static string GetContent(string fileName)
        {
            return File.ReadAllText(PathFor(fileName));
        }
    }

}
