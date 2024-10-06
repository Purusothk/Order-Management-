
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.models
{
    internal class Clothings : Products
    {
        public string Size { get; set; }
        public string Color { get; set; }

        // Constructor
        public Clothings(int productId, string productName, string description, double price, int quantityInStock, string type, string size, string color)
            : base(productId, productName, description, price, quantityInStock, type)
        {
            Size = size;
            Color = color;
        }
    }
}
