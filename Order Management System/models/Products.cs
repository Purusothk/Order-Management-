using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.models
{
    internal class Products
    {
        
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public int QuantityInStock { get; set; }
            public string Type { get; set; } // Electronics or Clothing

            // Constructor
            public Products(int productId, string productName, string description, double price, int quantityInStock, string type)
            {
                ProductId = productId;
                ProductName = productName;
                Description = description;
                Price = price;
                QuantityInStock = quantityInStock;
                Type = type;
            }
    }
}
