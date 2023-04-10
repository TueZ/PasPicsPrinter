using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PasPicsPrinter.Handlers
{
    public static class WebHandler
    {
        private static string webAddress = @"https://localhost:44372/";
        private static string password = @"derp";

        public static string NewCustomer(string parameterString)
        {
            var wb = new WebClient();
            string url = webAddress + "customer/new" + "?password=" + password + "&" + parameterString;
            string result = wb.DownloadString(url);
            return result; 
        }

        public static string UpdateCustomer(string parameterString)
        {
            var wb = new WebClient();
            string url = webAddress + "customer/update" + "?password=" + password + "&" + parameterString;
            string result = wb.DownloadString(url);
            return result; 
        }

        public static string OrderCount()
        {
            var wb = new WebClient();
            string url = webAddress + "order/count" + "?password=" + password;
            string result = wb.DownloadString(url);
            return result;
        }

        public static string OrderRequest()
        {
            var wb = new WebClient();
            string url = webAddress + "order/request" + "?password=" + password;
            return wb.DownloadString(url);
        }

        public static string FinishOrder(string parameterString)
        {
            var wb = new WebClient();
            string url = webAddress + "order/finish" + "?password=" + password + "&";
            string result = wb.DownloadString(url);
            return result; 
        }
    }
}
