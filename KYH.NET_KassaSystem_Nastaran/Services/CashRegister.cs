using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KYH.NET_KassaSystem_Nastaran.Interface;
using KYH.NET_KassaSystem_Nastaran.Models;
using KYH.NET_KassaSystem_Nastaran.Services;


namespace KYH.NET_KassaSystem_Nastaran.Services
{
    public class CashRegister
    {
        private AdminTool adminTool = new AdminTool(); // Skapa instans av AdminTool för att hantera produkter
        private Receipt currentReceipt; // Kvitto för aktuell transaktion
        private IErrorManager errorManager = new ErrorManager(); // Instans av felhanterare

        // Starta huvudloopen för kassan
        public void Start()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1. Ny Kund\n0. Avsluta"); // Visa alternativ
                var input = Console.ReadLine();
                if (input == "1")
                {
                    StartNewTransactionTest(); // Starta ny kundtransaktion
                }
                else if (input == "0")
                {
                    break; // Avsluta programmet
                }
            }
        }

        // Starta en ny transaktion för en kund
        public void StartNewTransactionTest()
        {
            currentReceipt = new Receipt(errorManager); // Skapa ett nytt kvitto
            Console.WriteLine("KASSA");
            Console.WriteLine($"KVITTO\t\t{DateTime.Now:yyyy-MM-dd HH:mm:ss}"); // Visa kassaskärm med datum

            while (true)
            {
                Console.WriteLine("Kommandon: <productid> <antal> eller PAY"); // Inmatningskommando för produkter eller betalning
                var command = Console.ReadLine()?.Split(' ');

                if (command[0] == "PAY") // Om kunden vill betala
                {
                    currentReceipt.PrintAndSaveReceipt(); // Skriv ut och spara kvittot
                    break; // Avsluta transaktionen
                }
                else if (int.TryParse(command[0], out int productId) && int.TryParse(command[1], out int quantity))
                {
                    try
                    {
                        // Hämta produkten via produkt-ID och kvantitet
                        var product = adminTool.Products.FirstOrDefault(p => p.Id == productId);
                        if (product != null)
                        {
                            currentReceipt.AddItem(product, quantity); // Lägg till produkten i kvittot
                        }
                        else
                        {
                            Console.WriteLine("Produkt ej funnen."); // Felmeddelande om produkten inte finns
                        }
                    }
                    catch (Exception ex)
                    {
                        errorManager.LogError(ex); // Logga fel
                        errorManager.DisplayError("Ett fel inträffade vid tillägg av produkt."); // Visa felmeddelande
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt kommando. Ange produkt-ID och antal, eller PAY för att avsluta transaktionen.");
                }
            }
        }
    }
}
