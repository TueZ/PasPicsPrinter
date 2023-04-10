using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
using PasPicsPrinter.Models;
using System.Collections.Generic;

namespace PasPicsPrinter
{
    public class PrintJob
    {
        private const bool pdfFotoBackup = true;
        private const bool pdfFakturaBackup = true;
        private const string fotoBackupPath = @"C:\Users\yaday\Desktop\Firma\Kørekort foto app\Backup foto pdf\"; 
        private const string fakturaBackupPath = @"C:\Users\yaday\Desktop\Firma\Kørekort foto app\Backup faktura pdf\";

        private Image image;

        private const int fotoWidth = 150;
        private const int fotoHeight = 170;
        private const int marginWidth = 35;
        private const int marginHeight = 47;

        private const int fotoBasePrice = 50;
        private const string virkNavn = @"Test virksomhed";
        private const string virkAddress = @"Test address";
        private const string virkBy = @"Test by";
        private const string virkKontaktPerson = @"Test person"; 
        private const string virkEmail = @"Test email@email.com";
        private const string virkCVR = @"Test 123456789";
        private const string virkBank = @"Test bank";
        private const string virkPayment = @"Test payment";

        /// <summary>
        /// Use this for faktura print
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="orderModel"></param>
        public PrintJob(string printerName, FakturaModel fakturaModel, DateTime date)
        {
            //image = new Bitmap(fakturaSkabelonPath); 
            image = new Bitmap((marginWidth + fotoWidth) * 4, (marginHeight + fotoHeight) * 5); // TEMP test 

            int y = 135; 

            int count = fakturaModel.imageNames.Count;
            int subTotal = fotoBasePrice * count;
            int moms = subTotal / 4;
            int totalPrice = subTotal + moms;
            string subTotalString = GetPrice(subTotal);
            string TotalString = GetPrice(totalPrice);
            string momsString = GetPrice(moms);

            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            string shippingDato = day + "/" + month + "/" + year;
            string forfaldDato = (day + 8) + "/" + month + "/" + year;

            Graphics g = Graphics.FromImage(image);
            Font font = new Font("Times New Roman", 14);
            Font fontBold = new Font("Times New Roman", 14, FontStyle.Bold);
            //Font font = new Font("Calibri", 12);
            Font fontBig = new Font("Calibri", 24);
            Pen pen = new Pen(Color.Black);

            g.Clear(Color.White);

            g.DrawString("Afsendt af: ", fontBold, Brushes.Black, new Point(500, 130));
            g.DrawString(virkNavn, font, Brushes.Black, new Point(500, 148));
            g.DrawString(virkAddress, font, Brushes.Black, new Point(500, 164));
            g.DrawString(virkBy, font, Brushes.Black, new Point(500, 184));
            g.DrawString(virkKontaktPerson, font, Brushes.Black, new Point(500, 202));
            g.DrawString(virkEmail, font, Brushes.Black, new Point(500, 220));
            g.DrawString(virkCVR, font, Brushes.Black, new Point(500, 238));
            g.DrawString(virkBank, font, Brushes.Black, new Point(500, 256));

            g.DrawString("Sendt til: ", fontBold, Brushes.Black, new Point(100, 130));
            string[] receiver = fakturaModel.customerModel.customerPayment.Split(':');
            for (int i = 0; i < receiver.Length; i++)
            {
                g.DrawString(receiver[i], font, Brushes.Black, new Point(100, 148 + i * 18));
            }

            g.DrawString("Dato: " + shippingDato, font, Brushes.Black, new Point(100, 360));
            g.DrawString("Fakturanr:", font, Brushes.Black, new Point(550, 360));
            g.DrawString(fakturaModel.fakturaNumber, fontBold, Brushes.Black, new Point(637, 360));

            if (count == 1)
            {
                g.DrawString("Her følger faktura for det bestilte pas foto. ", font, Brushes.Black, new Point(100, 390));
            }
            else
            {
                g.DrawString("Her følger faktura for de bestilte pas fotos. ", font, Brushes.Black, new Point(100, 390));
            }

            //g.DrawString("Faktura ", fontBig, Brushes.Black, new Point(100, 510));
            g.DrawString("Tekst ", fontBold, Brushes.Black, new Point(100, 520));
            g.DrawString("Antal ", fontBold, Brushes.Black, new Point(300, 520));
            g.DrawString("stk pris ", fontBold, Brushes.Black, new Point(400, 520));
            g.DrawString("pris ", fontBold, Brushes.Black, new Point(550, 520));
            g.DrawLine(pen, new Point(100, 550), new Point(650, 550));

            if (count == 1)
            {
                g.DrawString("Printede pas foto", font, Brushes.Black, new Point(100, 570)); 
            }
            else
            {
                g.DrawString("Printede pas fotos", font, Brushes.Black, new Point(100, 570));                
            }
            g.DrawString(count.ToString(), font, Brushes.Black, new Point(300, 570));
            g.DrawString(fotoBasePrice.ToString(), font, Brushes.Black, new Point(400, 570));
            g.DrawString(subTotalString, font, Brushes.Black, new Point(550, 570));
            //g.DrawString(subTotalString, font, Brushes.Black, new Point(550, 570));

            //g.DrawString("+25% moms af: " + subTotalString, font, Brushes.Black, new Point(100, 600));
            //g.DrawString(momsString, font, Brushes.Black, new Point(550, 600));
            //g.DrawLine(pen, new Point(550, 619), new Point(650, 619));

            g.DrawLine(pen, new Point(100, 700), new Point(650, 700));

            g.DrawString("Netto ", font, Brushes.Black, new Point(400, 720));
            g.DrawString(subTotalString, font, Brushes.Black, new Point(550, 720));
            //g.DrawLine(pen, new Point(390, 690), new Point(650, 690)); 

            g.DrawString(@"Moms (25%) ", font, Brushes.Black, new Point(400, 750));
            g.DrawString(momsString, font, Brushes.Black, new Point(550, 750));
            g.DrawLine(pen, new Point(390, 770), new Point(650, 770));

            g.DrawString("Total ", fontBold, Brushes.Black, new Point(400, 780));
            g.DrawString(TotalString, fontBold, Brushes.Black, new Point(550, 780));
            g.DrawLine(pen, new Point(390, 800), new Point(650, 800));

            g.DrawString("Betales senest " + forfaldDato, font, Brushes.Black, new Point(100, 900));
            g.DrawString("Betaling kan ske ved overførsel til " + virkPayment, font, Brushes.Black, new Point(100, 930)); 

            // Print 
            if (printerName != "")
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += PrintPage;
                pd.PrinterSettings.PrinterName = printerName;
                pd.Print();
            }

            // Print pdf backup 
            if (pdfFakturaBackup)
            {
                string file = DateTime.Now.ToString().Replace(':', '.');
                PrintDocument pdfPrint = new PrintDocument()
                {
                    PrinterSettings = new PrinterSettings()
                    {
                        PrinterName = "Microsoft Print to PDF",
                        PrintToFile = true,
                        PrintFileName = Path.Combine(fakturaBackupPath, file + ".pdf")
                    }
                };
                pdfPrint.PrintPage += PrintPage;
                pdfPrint.Print();
            }
        }

        /// <summary>
        /// Use this for foto print 
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="imageModels"></param>
        public PrintJob(string printerName, List<ImageModel> imageModels)
        {
            image = new Bitmap((marginWidth + fotoWidth) * 4, (marginHeight + fotoHeight) * 5);

            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White); 

            int x = -1;
            int y = 0; 
            for (int i = 0; i < imageModels.Count; i++)
            {
                // Image location 
                x++;
                if (x == 4)
                {
                    x = 0;
                    y++;
                }
                Point loc = new Point((marginWidth + fotoWidth) * x, (marginHeight + fotoHeight) * y);                

                // Create image 
                byte[] byteBuffer = Convert.FromBase64String(imageModels[i].imageString);
                MemoryStream memoryStream = new MemoryStream(byteBuffer);
                memoryStream.Position = 0;
                Bitmap imageToAdd = (Bitmap)Bitmap.FromStream(memoryStream);
                memoryStream.Close();
                memoryStream = null;
                byteBuffer = null;
                
                // Add to image                 
                g.DrawImage(imageToAdd, loc);

                // Add text 
                Point locTextTop = new Point((marginWidth + fotoWidth) * x, (marginHeight + fotoHeight) * y + fotoHeight + marginHeight / 5 - 2);
                Point locTextBot = new Point((marginWidth + fotoWidth) * x, (marginHeight + fotoHeight) * y + fotoHeight + marginHeight / 5 * 2 + 2);
                Font font = new Font("Arial", 12);
                g.DrawString(imageModels[i].imageName.ToString().Substring(0, 18), font, Brushes.Black, locTextTop); 
                g.DrawString(imageModels[i].imageName.ToString().Substring(18), font, Brushes.Black, locTextBot); 
            }

            // Print 
            if (printerName != "")
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += PrintPage;
                pd.PrinterSettings.PrinterName = printerName;
                pd.Print();
            }            

            // Print pdf backup 
            if (pdfFotoBackup)
            {
                string file = DateTime.Now.ToString().Replace(':', '.'); 
                PrintDocument pdfPrint = new PrintDocument() 
                {
                    PrinterSettings = new PrinterSettings()
                    {
                        PrinterName = "Microsoft Print to PDF", 
                        PrintToFile = true, 
                        PrintFileName = Path.Combine(fotoBackupPath, file + ".pdf") 
                    }
                };
                pdfPrint.PrintPage += PrintPage;
                pdfPrint.Print(); 
            }
        }

        private string GetPrice(int price)
        {
            if (price >= 1000)
            {
                string s = price.ToString();
                return s.Substring(0, s.Length - 3) + "." + s.Substring(3) + ",00"; 
            }
            else
            {
                return price.ToString() + ",00"; 
            }
        }

        private void PrintPage(object o, PrintPageEventArgs e)
        {
            Point loc = new Point(30, 30);
            e.Graphics.DrawImage(image, loc);
        }
    }
}
