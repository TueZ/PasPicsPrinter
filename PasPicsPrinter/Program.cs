using PasPicsPrinter.Handlers;
using PasPicsPrinter.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Web;

namespace PasPicsPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            //ImageModel image = new ImageModel() { imageString = @"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" }; 
            //List<ImageModel> images = new List<ImageModel>() { image, image, image, image, image, image, image, image, image, image, image, image, image, image, image, image, image, image, image, image };
            //PrintJob p1 = new PrintJob("", images);
            //return; 

            //FakturaModel fakturaModel = new FakturaModel() { fakturaNumber = "10", imageNames = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() }, customerModel = new CustomerModel() { customerPayment = "line1:line2:line3:line4" } }; 
            //PrintJob p2 = new PrintJob("", fakturaModel, DateTime.UtcNow);
            //return;
            
            Menu();
        }

        #region Menu
        private static void Menu()
        {
            Console.WriteLine("");
            Console.WriteLine("---MENU---");
            while (true)
            {
                Console.WriteLine("");
                Console.WriteLine(" 1: Get new orders");
                Console.WriteLine(" 2: Get new orders and Process");
                Console.WriteLine(" 3: Create new customer");
                Console.WriteLine(" 4: Update existing customer");
                Console.WriteLine(" 5: Check fakturas");
                Console.WriteLine(" 0: Exit program");
                Console.WriteLine("");
                var input = Console.ReadKey();
                var inputString = input.KeyChar.ToString();
                int inputInt = 0;
                if (int.TryParse(inputString, out inputInt) && inputInt >= 0 && inputInt <= 5)
                {
                    if (inputInt == 1)
                    {
                        GetOrderCount();
                    }
                    else if (inputInt == 2)
                    {
                        if (GetOrderCount())
                        {
                            ProcessOrder();
                        }                        
                    }
                    else if (inputInt == 3)
                    {
                        CreateNewCustomer();
                    }
                    else if (inputInt == 4)
                    {
                        UpdateCustomer();
                    }
                    else if (inputInt == 5)
                    {
                        GetFakturas(); 
                    }
                    else if (inputInt == 0)
                    {
                        return;
                    }
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine(" ---MENU---");
                }
                else
                {
                    Console.WriteLine(" Input error from string: " + inputString + " please try again. ");
                    Console.WriteLine("");
                }
            }
        }
        #endregion 

        #region Orders 
        private static void ProcessOrder()
        {
            // Create models. 
            string result = WebHandler.OrderRequest(); 
            var orderModel = JsonHandler.UnPackOrdreModel(result);
            var originalModel = JsonHandler.UnPackOrdreModel(result);
            if (orderModel.images.Count != orderModel.totalImages)
            {
                // ERROR! 
                Console.WriteLine(" Error in request! ");
                Console.ReadKey();
                return;
            }

            // Print and keep printing untill all done. 
            var printedImages = PrintOrder(orderModel, originalModel);            
            while (AwaitInput(orderModel, printedImages, originalModel))
            {
                printedImages = PrintOrder(orderModel, originalModel);
            }

            // Finish up. 
            var date = GetShippingDate();
            var fakturaModel = DBHandler.StoreFinishedOrders(originalModel, date);
            ReportFinishedModels(originalModel);
            PrintFaktura(fakturaModel, date);
        }

        private static bool GetOrderCount()
        {
            string result = WebHandler.OrderCount(); 

            int count = 0; 
            if (!int.TryParse(result, out count))
            {
                // Error
                Console.WriteLine("Error 1 parsing count: " + result);
                Console.ReadKey(); 
            }
            else if (count == 0)
            {
                Console.WriteLine("No new orders. ");
                Console.WriteLine(); 
            }
            else if (count > 0)
            {
                Console.WriteLine("Total new orders: " + count);
                Console.WriteLine();
                return true; 
            }
            else
            {
                Console.WriteLine("Error 2 parsing count: " + result);
                Console.ReadKey();
            }
            return false; 
        }

        private static List<ImageModel> PrintOrder(OrderModel orderModel, OrderModel originalModel)
        {
            var printedImages = new List<ImageModel>(); 
            for (int i = 0; i < orderModel.images.Count && i < 16; i++)
            {
                printedImages.Add(orderModel.images[i]); 
            }
            PrintHandler.PrintImages(printedImages); 

            return printedImages; 
        }

        private static bool AwaitInput(OrderModel orderModel, List<ImageModel> printedImages, OrderModel originalModel)
        {
            DisplayOrderProcessed(printedImages);
            var inputLine = Console.ReadLine();            
            int inputNumber = 0;
            while (true)
            {
                if (!int.TryParse(inputLine, out inputNumber)) {
                    Console.WriteLine(" Input error, only use numbers as input.");
                }
                else { 
                    if (inputNumber == 0)
                    {
                        // Itterate through all images, remove those that are printed correctly. 
                        for (int i = 0; i < printedImages.Count; i++)
                        {
                            for (int n = 0; n < orderModel.images.Count; n++)
                            {
                                if (printedImages[i].imageName == orderModel.images[n].imageName)
                                {
                                    orderModel.images.RemoveAt(n);
                                    break; 
                                }
                            }
                        }

                        if (orderModel.images.Count > 0)
                        {
                            // Print next batch of images, the images not printed correctly last time, are printed again. 
                            return false; 
                        }
                        else
                        {
                            // End                            
                            return true; 
                        }
                    }
                    else if (inputNumber > 0 && inputNumber >= orderModel.images.Count)
                    {
                        Console.WriteLine(" Removing number " + inputNumber + " from finished images. ");
                        printedImages.RemoveAt(inputNumber - 1); 
                        DisplayOrderProcessed(printedImages); 
                    }
                    else
                    {
                        Console.WriteLine(" Input error, number out of range, try again."); 
                    }
                }
            }
        }

        private static void DisplayOrderProcessed(List<ImageModel> printedImages)
        {
            Console.WriteLine(" Press 0 to accept all as finished. ");
            for (int i = 0; i < printedImages.Count; i++)
            {
                Console.WriteLine(" " + (i + 1) + ": " + printedImages[i].imageName);
            }
        }

        private static void ReportFinishedModels(OrderModel originalModel)
        {
            for (int i = 0; i < originalModel.images.Count; i++)
            {
                string parameterString = "imageName=" + originalModel.images[i].imageName;
                string result = WebHandler.FinishOrder(parameterString); 
                if (result != "done")
                {
                    //Error in reporting. 
                    Console.WriteLine(" Error reporting finished image: " + originalModel.images[i].imageName);
                    Console.WriteLine(" This image has been removed from faktura, as it is added to future order request. ");
                    originalModel.images.RemoveAt(i);
                    i--; 
                }
            }
        }

        private static void PrintFaktura(FakturaModel fakturaModel, DateTime date)
        {
            PrintHandler.PrintFaktura(fakturaModel, date); 

            Console.WriteLine(" Print order done. ");
        }

        private static DateTime GetShippingDate()
        {
            int dayInc = 0;
            switch (DateTime.UtcNow.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    {
                        dayInc = 7;
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        dayInc = 6;
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        dayInc = 5;
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        dayInc = 4;
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        dayInc = 3;
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        dayInc = 2;
                        break;
                    }
                case DayOfWeek.Sunday:
                    {
                        dayInc = 1;
                        break;
                    }
            }

            return DateTime.UtcNow.AddDays(dayInc); 
        }
        #endregion

        #region Customers
        private static void CreateNewCustomer()
        {
            while (true)
            {
                if (ProcessCreateNewCustomer())
                {
                    return; 
                }

                Console.WriteLine(" Customer creation failed. Try again y/n");
                char input = Console.ReadKey().KeyChar; 
                if (input != 'y')
                {
                    return; 
                }
            }
        }

        private static bool ProcessCreateNewCustomer()
        {
            Console.WriteLine(); 
            Console.WriteLine(" Enter Username: "); 
            string username = Console.ReadLine();            
            if (string.IsNullOrEmpty(username))
            {
                Console.WriteLine(" Input empty. ");
                return false;
            }
            username = HttpUtility.UrlEncode(username);

            Console.WriteLine(" Enter Contact: ");
            string customerContact = Console.ReadLine();
            if (string.IsNullOrEmpty(customerContact))
            {
                Console.WriteLine(" Input empty. ");
                return false;
            }
            customerContact = HttpUtility.UrlEncode(customerContact);

            Console.WriteLine(" Enter Address: ");
            string customerAddress = Console.ReadLine();
            if (string.IsNullOrEmpty(customerAddress))
            {
                Console.WriteLine(" Input empty. ");
                return false;
            }
            customerAddress = HttpUtility.UrlEncode(customerAddress);

            Console.WriteLine(" Enter PaymentInfo: ");
            string customerPayment = Console.ReadLine();
            if (string.IsNullOrEmpty(customerPayment))
            {
                Console.WriteLine(" Input empty. ");
                return false;
            }
            customerPayment = HttpUtility.UrlEncode(customerPayment);

            string parameterString = "customerContact=" + customerContact + "&customerAddress=" + customerAddress + "&customerPayment=" + customerPayment; 
            string result = WebHandler.NewCustomer(parameterString); 
            
            if (result == "done")
            {
                CustomerModel customerModel = new CustomerModel()
                {
                    customerName = username,
                    customerContact = customerContact,
                    customerAddress = customerAddress,
                    customerPayment = customerPayment
                };
                DBHandler.StoreNewCustomer(customerModel);

                Console.WriteLine();
                Console.WriteLine(" Customer created. ");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(" Server rejected input: " + result);
                Console.WriteLine();
            }

            return true; 
        }

        private static void UpdateCustomer()
        {
            // Get who to change
            // Get what to change
            // Send to server 

            // Name
            // Contact
            // address
            // Payment 
            // Password reset. 

            Console.WriteLine(" Enter username of user to change: "); 
            string username = Console.ReadLine(); 

            string toChange = ""; 
            while(toChange == "")
            {
                Console.WriteLine();
                Console.WriteLine(" Enter the area to change");
                Console.WriteLine(" 1. Address");
                Console.WriteLine(" 2. PaymentInfo");
                Console.WriteLine(" 3. Contact");                

                char input = Console.ReadKey().KeyChar; 
                if (input == '1')
                {
                    toChange = "address"; 
                }
                else if (input == '2')
                {
                    toChange = "payment"; 
                }
                else if (input == '3')
                {
                    toChange = "contact"; 
                }
            }

            string newValue = ""; 
            while (string.IsNullOrEmpty(newValue))
            {
                Console.WriteLine();
                Console.WriteLine(" Enter the new value");
                newValue = Console.ReadLine();
            }
            newValue = HttpUtility.UrlEncode(newValue);

            string parameterString = "username=" + username + "&toChange=" + toChange + "&newValue=" + newValue;
            string result = WebHandler.UpdateCustomer(parameterString); 

            if (result == "done")
            {
                DBHandler.UpdateCustomer(username, toChange, newValue); 
            }
            else
            {
                Console.WriteLine(" Server rejected attepmt: " + result); 
            }
        }

        private static void GetFakturas()
        {
            Console.WriteLine();
            Console.WriteLine();
            while (true)
            {
                var fakturas = DBHandler.GetUnfinishedFakturas();
                for (int i = 0; i < fakturas.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + fakturas[i].dateShipped + " - " + fakturas[i].fakturaNumber + " - " + fakturas[i].customerModel.customerName);
                }
                Console.WriteLine(" Enter the faktura to finish. Enter 0 to return to menu. ");
                var input = Console.ReadLine();
                int inputNumber = 0;
                if (int.TryParse(input, out inputNumber) && inputNumber >= 0 && inputNumber <= fakturas.Count)
                {
                    if (inputNumber == 0)
                    {
                        return; 
                    }
                    else
                    {
                        DBHandler.SetFakturaToDone(fakturas[inputNumber - 1].guid);
                        Console.WriteLine(" Faktura set to done from: " + fakturas[inputNumber - 1].customerModel.customerName);
                        Console.WriteLine();
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine(" Input error. please try again. ");
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }
        }
        #endregion 
    }
}
