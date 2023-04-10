using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPicsPrinter.Models
{
    public class ImageModel
    {
        public Guid imageName { get; set; }
        public string imageString { get; set; }
        public string imageUploadDate { get; set; }
        public string imageDownloadDate { get; set; }
    }
}
