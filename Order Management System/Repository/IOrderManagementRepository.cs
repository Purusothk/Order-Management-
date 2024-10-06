using Order_Management_System.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Repository
{
    internal interface IOrderManagementRepository
    {
        // Create a new order for a user with a list of products.
        //void CreateOrder(Users user, List<Products> productList);

        //author :Purusothaman
        void CreateOrder(int userId, List<Products> productList);

        // Cancel an existing order by userId and orderId.
        void CancelOrder(int userId, int orderId);

        // Create a new product in the database (only for admin users).
        void CreateProduct(Users user, Products product);

        // Create a new user in the database.
        Users CreateUser(Users user);

        // Retrieve all products from the database.
        List<Products> GetAllProducts();

        // Retrieve all orders associated with a specific user, including their products.
        List<Order> GetOrdersByUser(int userId);
    }

}
