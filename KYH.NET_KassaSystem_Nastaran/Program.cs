using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using KYH.NET_KassaSystem_Nastaran.Enum;
using KYH.NET_KassaSystem_Nastaran.Interface;
using KYH.NET_KassaSystem_Nastaran.Models;
using KYH.NET_KassaSystem_Nastaran.Services;

namespace KYH.NET_KassaSystem_Nastaran
{
    class Program
    {
        static void Main(string[] args)
        {
            // Skapa en instans av ErrorManager för felhantering
            var errorManager = new ErrorManager();

            // Skapa en instans av Receipt och passera in felhanteraren
            var receipt = new Receipt(errorManager);

            // Skapa ett nytt AdminTool för att hantera produkter och kampanjer
            var adminTool = new AdminTool();

            try
            {
                // Skapa produkter
                var apple = new Product(301, "Apple", 10m, "per styck");
                var banana = new Product(300, "Banana", 8m, "per styck");

                // Lägg till kampanjer till produkter
                var appleCampaign = new Campaign(
                    CampaignType.PercentageDiscount, 20m,
                   new DateTime(2024, 11, 01), new DateTime(2024, 12, 25)
                );

                var bananaCampaign = new Campaign(
                    CampaignType.FixedDiscount, 5m,
                   new DateTime(2024, 11, 01), new DateTime(2024, 12, 25)
                );

                // Associera kampanjer med produkter
                apple.AddCampaign(appleCampaign);
                banana.AddCampaign(bananaCampaign);

                // Lägg till produkter i AdminTool (för enkelhetens skull)
                adminTool.AddProduct(apple);
                adminTool.AddProduct(banana);

                // Skapa kvitto och lägg till köpta produkter
                receipt.AddItem(apple, 3); // Köpt 3 äpplen
                receipt.AddItem(banana, 5); // Köpt 5 bananer

                // Skriv ut och spara kvitto
                receipt.PrintAndSaveReceipt();

                // Konsolutdata för att visa kvittot i terminalen
                Console.WriteLine(" (APPLE => 20% rabatt på 10.00 kr -> 8.00 kr per styck)");
                Console.WriteLine(" (BANANA => 5.00 kr rabatt -> 3.00 kr per styck)");
                Console.WriteLine(" -------------------------------------");



                // Starta en ny kassatransaktion på kampanjdatumet
                var cashRegister = new CashRegister();
                cashRegister.StartNewTransactionTest();

                // Starta huvudloopen för kassaflödet
                cashRegister.Start();
            }
            catch (Exception ex)
            {
                // Logga oväntade fel och visa ett generellt felmeddelande
                errorManager.LogError(ex);
                errorManager.DisplayError("Ett oväntat fel inträffade.");
            }
        }
    }
}
