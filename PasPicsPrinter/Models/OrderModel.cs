using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPicsPrinter.Models
{
    public class OrderModel
    {
        public CustomerModel customer { get; set; }
        public List<ImageModel> images { get; set; }
        public int totalImages { get; set; }
    }
}
