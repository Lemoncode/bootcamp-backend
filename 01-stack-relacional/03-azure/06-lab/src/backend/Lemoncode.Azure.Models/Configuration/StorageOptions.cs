using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.Azure.Models.Configuration
{
    public class StorageOptions
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public string ConnectionString { get; set; }
        public string ScreenshotsContainer { get; set; }
        public string ScreenshotsQueue { get; set; }
    }
}
