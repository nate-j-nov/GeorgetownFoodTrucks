using System;
using System.Collections.Generic;
using System.Text;
using iText;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using System.IO;
using System.Text.RegularExpressions;
namespace FoodTrucksApp
{
    class DataCleaner
    {
        // Member declarations
        public string TotalText { get; set; }
        public string[] DividedText { get; set; }
        public List<FoodTruck> FoodTruckList = new List<FoodTruck>();

        //Constructors
        public DataCleaner() { }
        public DataCleaner(string filePath)
        {
            GetDataFromPDF(filePath);
            CreateArrayOfStrings(TotalText);
            SetSitePermitNumber();
            PrintFoodTruckSiteNumberTESTFUNCTION();
        }

        
        //Extracts data from the foodtruck lottery pdf found online. 
        protected void GetDataFromPDF(string file)
        {
            PdfReader pdfReader = new PdfReader(file);
            PdfDocument pdfDoc = new PdfDocument(pdfReader);
            for (int page = 1; page <= pdfDoc.GetNumberOfPages(); page++)
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                string currentText = PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(page));
                TotalText += currentText;
            }
        }

        //Prints the data
        protected void PrintData(string pdfData)
        {
            Console.WriteLine(pdfData);
        }

        // Separates text into an array of lines, each to represent one food truck. 
        protected void CreateArrayOfStrings(string text)
        {
            DividedText = TotalText.Split(new[] { "\n" }, StringSplitOptions.None);
        }

        protected void PrintDividedText()
        {
            foreach(var s in DividedText)
            {
                Console.WriteLine(s + Environment.NewLine + "New Element");
            }
        }

        protected string GetSitePermitNumber(string lineOfText)
        {
            string pattern = @"VSP-\d{5}";
            Match sitePermitNumber = Regex.Match(lineOfText, pattern);
            if(sitePermitNumber == null)
            {
                return null;
            }
            else
            {
                return sitePermitNumber.ToString();
            }
        }

        protected void SetSitePermitNumber()
        {
            foreach(var t in DividedText)
            {
                string sitePermitNumber = GetSitePermitNumber(t);
                if(sitePermitNumber != null)
                {
                    FoodTruckList.Add(new FoodTruck(sitePermitNumber));
                }
            }
        }

        protected void PrintFoodTruckSiteNumberTESTFUNCTION()
        {
            foreach(var ft in FoodTruckList)
            {
                Console.WriteLine(ft.SitePermit.ToString());
            }
        }
    }
}
