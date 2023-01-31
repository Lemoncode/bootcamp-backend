using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemoncode.Azure.Models
{
    public class ScreenshotMessage
    {
        public int GameId { get; set; }
        public int ScreenshotId { get; set; }
        public string Filename { get; set; }
        public string ScreenshotUrl { get; set; }
    }
}
