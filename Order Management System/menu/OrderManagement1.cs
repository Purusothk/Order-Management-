using Order_Management_System.models;
using Order_Management_System.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.menu
{
    internal class OrderManagement1
    {
        IOrderManagementRepository orderProcessor = new OrderProcessor();
        bool running = true;
        private Users currentUser; // Ensure this is properly instantiated

        public void Run() // Instance method to run the menu
        {
            while (running)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Get All Products ");
                Console.WriteLine("4. Place Order");
                Console.WriteLine("5. Get Order By User");
                Console.WriteLine("6. Cancel Order");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateUser();
                        break;
                    case "2":
                        CreateProduct();
                        break;
                    case "3":
                        GetAllProducts();
                        break;
                    case "4":                        
                        PlaceOrder();
                        break;
                    case "5":
                        GetOrderByUser();
                        break;
                    case "6":
                        CancelOrder();
                        break;
                    case "7":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private void CreateUser()
        {
            Console.Write("Enter Username: ");
            string username = Console.ReadLine();

            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            Console.Write("Enter Role (Admin/User): ");
            string role = Console.ReadLine();

            // Create a new user object
            Users user = new Users { Username = username, Password = password, Role = role };

            // Call the CreateUser method
            var createdUser = orderProcessor.CreateUser(user);

            if (createdUser != null)
            {
                Console.WriteLine($"User created successfully with ID: {createdUser.UserId}");
                currentUser = createdUser; // Set current user to the newly created user
            }
            else
            {
                Console.WriteLine("Error creating user. Please try again.");
            }
        }

        private void CreateProduct()
        {
            if (currentUser == null)
            {
                Console.WriteLine("You must create a user before creating a product. Please create a user first.");
                return;
            }

            Console.WriteLine($"Current user's role: {currentUser.Role}");

            if (currentUser.Role != "Admin")
            {
                Console.WriteLine("Only admins can create products.");
                return;
            }

            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine();
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Price: ");
            double price;
            while (!double.TryParse(Console.ReadLine(), out price))
            {
                Console.Write("Invalid price. Please enter a valid price: ");
            }

            Console.Write("Enter Quantity in Stock: ");
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity))
            {
                Console.Write("Invalid quantity. Please enter a valid quantity: ");
            }

            Console.Write("Enter Type (Electronics/Clothing): ");
            string type = Console.ReadLine();

            // Create a new product using the constructor
            var product = new Products(0, productName, description, price, quantity, type); // Pass 0 for new products

            try
            {
                orderProcessor.CreateProduct(currentUser, product);
                Console.WriteLine("Product created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating product: {ex.Message}");
            }
        }

        //author :Purusothaman
        private void CancelOrder()
        {
            if (currentUser == null)
            {
                Console.WriteLine("No user created. Please create a user first.");
                return;
            }

            Console.Write("Enter Order ID to cancel: ");
            int orderId;
            if (int.TryParse(Console.ReadLine(), out orderId))
            {
                try
                {
                    orderProcessor.CancelOrder(currentUser.UserId, orderId);
                    Console.WriteLine("Order canceled successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error canceling order: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Order ID.");
            }
        }

        private List<Products> GetAllProducts()
        {
            try
            {
                //var products = orderProcessor.GetAllProducts();
                Console.WriteLine("Available Products:");
                List<Products> productList = orderProcessor.GetAllProducts();

                if (productList.Count == 0)
                {
                    Console.WriteLine("No products available.");
                }
                else
                {
                    foreach (var product in productList)
                    {
                        Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Price: {product.Price}");
                    }
                }
                return productList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching products: {ex.Message}");
                return new List<Products>();
            }
        }

        private void GetOrderByUser()
        {
            if (currentUser == null || currentUser.UserId == 0)
            {
                Console.WriteLine("No valid user created. Please create a user first.");
                return;
            }

            // Fetch orders
            var orders = orderProcessor.GetOrdersByUser(currentUser.UserId);
            if (orders == null || !orders.Any())
            {
                Console.WriteLine("No orders found for this user.");
                return;
            }

            // Process and display orders
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Order Date: {order.OrderDate}");
            }
        }

        private void PlaceOrder()
        {
            if (currentUser == null || currentUser.UserId == 0)
            {
                Console.WriteLine("No valid user created. Please create a user first.");
                return;
            }

            Console.WriteLine("Available Products:");
            List<Products> products = GetAllProducts();
            if (products.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

          //checks if is there
            Console.Write("Enter Product ID to order: ");
            int productId;
            while (!int.TryParse(Console.ReadLine(), out productId) || products.All(p => p.ProductId != productId))
            {
                Console.Write("Invalid Product ID. Please enter a valid Product ID: ");
            }

            Console.WriteLine($"You are ordering Product ID: {productId}");
            Console.WriteLine($"User ID: {currentUser.UserId}");

            try
            {
                // Create a list with the selected product for the order
                var productList = new List<Products>
                {
                    products.First(p => p.ProductId == productId) // Add the selected product to the list
                };

                orderProcessor.CreateOrder(currentUser.UserId, productList); // Pass the product list
                Console.WriteLine("Order placed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error placing order: {ex.Message}");
            }
        }
        //author :Purusothaman


    }
}
