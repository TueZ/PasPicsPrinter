using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPicsPrinter.Models
{
    public class FakturaModel
    {
        public Guid guid { get; set; }
        public string fakturaNumber { get; set; }
        public CustomerModel customerModel { get; set; }
        public List<Guid> imageNames { get; set; }
        public bool isPayed { get; set; } 
        public DateTime datePayed { get; set; }
        public DateTime dateShipped { get; set; }
    }
}
