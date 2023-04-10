using PasPicsPrinter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasPicsPrinter.Handlers
{
    public static class JsonHandler
    {
        public static string PackModel(object model)
        {
            return JsonSerializer.Serialize(model); 
        }

        public static OrderModel UnPackOrdreModel(string orderString)
        {
            try
            {
                return JsonSerializer.Deserialize<OrderModel>(orderString);
            }
            catch
            {
                return null; 
            }
        }
    }
}
