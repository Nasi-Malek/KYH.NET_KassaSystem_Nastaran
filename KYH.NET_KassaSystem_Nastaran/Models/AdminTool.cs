using System;
using System.Collections.Generic;
using System.Linq;
using KYH.NET_KassaSystem_Nastaran.Services;

namespace KYH.NET_KassaSystem_Nastaran.Models
{
    public class AdminTool
    {
        public List<Product> Products { get; set; } = new List<Product>(); // Lista med produkter i butiken

        public AdminTool()
        {
            try
            {
                // Lägg till exempelprodukter som finns i kassan
                AddProduct(new Product(300, "Banana", 8m, "per styck"));
                AddProduct(new Product(301, "Apple", 10m, "per styck"));
                AddProduct(new Product(302, "Coffee", 35m, "per styck"));
                AddProduct(new Product(303, "Milk",  10m,  "per styck"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid initiering av produkter: {ex.Message}");
            }
        }                             

        /// <summary>
        /// Lägg till en ny produkt i produktlistan.
        /// </summary>
        /// <param name="product">Produkten som ska läggas till.</param>
        public void AddProduct(Product product)
        {
            
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Produkt kan inte vara null.");

            Products.Add(product);
        }

        /// <summary>
        /// Uppdatera produktnamn och pris baserat på produkt-ID.
        /// </summary>
        /// <param name="productId">ID för produkten som ska uppdateras.</param>
        /// <param name="newName">Nytt namn för produkten.</param>
        /// <param name="newPrice">Nytt pris för produkten.</param>
        public void UpdateProduct(int productId, string newName, decimal newPrice)
        {
            var product = Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                Console.WriteLine("Produkt ej funnen.");
                return;
            }

            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Nytt namn kan inte vara tomt.");
                return;
            }

            if (newPrice < 0)
            {
                Console.WriteLine("Nytt pris kan inte vara negativt.");
                return;
            }

            product.Name = newName;
            product.Price = newPrice;
        }

        /// <summary>
        /// Lägg till en kampanj för en specifik produkt.
        /// </summary>
        /// <param name="productId">ID för produkten som ska få en kampanj.</param>
        /// <param name="campaign">Kampanjen som ska läggas till.</param>
        public void AddCampaignToProduct(int productId, Campaign campaign)
        {
            if (campaign == null)
                throw new ArgumentNullException(nameof(campaign), "Kampanj kan inte vara null.");

            var product = Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                Console.WriteLine("Produkt ej funnen.");
                return;
            }

            product.AddCampaign(campaign);
            Console.WriteLine("Kampanj tillagd.");
        }

        /// <summary>
        /// Ta bort en kampanj från en specifik produkt baserat på start- och slutdatum.
        /// </summary>
        /// <param name="productId">ID för produkten som ska uppdateras.</param>
        /// <param name="startDate">Startdatum för kampanjen som ska tas bort.</param>
        /// <param name="endDate">Slutdatum för kampanjen som ska tas bort.</param>
        public void RemoveCampaignFromProduct(int productId, DateTime startDate, DateTime endDate)
        {
            var product = Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
            {
                Console.WriteLine("Produkt ej funnen.");
                return;
            }

            int removedCount = product.Campaigns.RemoveAll(c => c.StartDate == startDate && c.EndDate == endDate);
            Console.WriteLine(removedCount > 0 ? "Kampanj borttagen." : "Ingen kampanj funnen att ta bort.");
        }
    }
}
