using PasPicsPrinter.Models;
using System;
using System.Collections.Generic;

namespace PasPicsPrinter.Handlers
{
    public static class PrintHandler
    {
        private const string printerName = ""; 

        public static void PrintImages(List<ImageModel> imageModels)
        {
            PrintJob printJob = new PrintJob(printerName, imageModels); 
        }

        public static void PrintFaktura(FakturaModel fakturaModel, DateTime date)
        {
            PrintJob printJob = new PrintJob(printerName, fakturaModel, date); 
        }
    }
}
