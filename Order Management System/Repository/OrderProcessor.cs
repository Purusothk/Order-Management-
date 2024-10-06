using Order_Management_System.models;
using Order_Management_System.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Repository
{
    internal class OrderProcessor : IOrderManagementRepository
    {
        private string connectionString;

        // Constructor using DbConnectionUtil to get the connection string
        public OrderProcessor()
        {
            connectionString = DbConnectionUtil.GetConnString(); // Get connection string from utility class
        }

        //public void CreateOrder(Users user, List<Products> products)
        //{
        //    using (var connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        // Check if user exists
        //        SqlCommand checkUser = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UserId = @UserId", connection);
        //        checkUser.Parameters.AddWithValue("@UserId", user.UserId);
        //        int userExists = (int)checkUser.ExecuteScalar();

        //        if (userExists == 0)
        //        {
        //            // Create user if not exists
        //            CreateUser(user);
        //        }

        //        // Insert Order
        //        SqlCommand orderCommand = new SqlCommand("INSERT INTO Orders (UserId) OUTPUT INSERTED.OrderId VALUES (@UserId)", connection);
        //        orderCommand.Parameters.AddWithValue("@UserId", user.UserId);
        //        int orderId = (int)orderCommand.ExecuteScalar();

        //        // Insert Products in OrderProducts Table
        //        foreach (var product in products)
        //        {
        //            SqlCommand orderProductCommand = new SqlCommand("INSERT INTO OrderProducts (OrderId, ProductId) VALUES (@OrderId, @ProductId)", connection);
        //            orderProductCommand.Parameters.AddWithValue("@OrderId", orderId);
        //            orderProductCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
        //            orderProductCommand.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void CancelOrder(int userId, int orderId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Start a transaction to ensure both deletes are atomic
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                         // Delete from OrderProducts first
                        string deleteOrderProductsSql = "DELETE FROM OrderProducts WHERE OrderId = @OrderId";
                        using (var command = new SqlCommand(deleteOrderProductsSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.ExecuteNonQuery();
                        }

                        //   Now delete the order from the Orders table
                        string deleteOrderSql = "DELETE FROM Orders WHERE OrderId = @OrderId AND UserId = @UserId";
                        using (var command = new SqlCommand(deleteOrderSql, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@OrderId", orderId);
                            command.Parameters.AddWithValue("@UserId", userId);  
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected == 0)
                            {
                                Console.WriteLine("No order found with the provided Order ID.");
                            }
                            else
                            {
                                Console.WriteLine("Order cancelled successfully.");
                            }
                        }

                        // Comnit 
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback  
                        transaction.Rollback();
                        Console.WriteLine($"Error canceling order: {ex.Message}");
                    }
                }
            }
        }


        public void CreateProduct(Users user, Products product)
        {
            if (user.Role != "Admin")
            {
                throw new UnauthorizedAccessException("Only admins can create products.");
            }

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Insert Product
                SqlCommand productCommand = new SqlCommand("INSERT INTO Products (ProductName, Description, Price, QuantityInStock, Type) VALUES (@ProductName, @Description, @Price, @QuantityInStock, @Type)", connection);
                productCommand.Parameters.AddWithValue("@ProductName", product.ProductName);
                productCommand.Parameters.AddWithValue("@Description", product.Description);
                productCommand.Parameters.AddWithValue("@Price", product.Price);
                productCommand.Parameters.AddWithValue("@QuantityInStock", product.QuantityInStock);
                productCommand.Parameters.AddWithValue("@Type", product.Type);
                productCommand.ExecuteNonQuery();
            }
        }

        public Users CreateUser(Users user)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "INSERT INTO Users (Username, Password, Role) OUTPUT INSERTED.UserId VALUES (@Username, @Password, @Role)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Password", user.Password); // Ensure this is not null
                    command.Parameters.AddWithValue("@Role", user.Role); // Ensure role is specified

                    // Get the newly created UserId
                    int newUserId = (int)command.ExecuteScalar();

                    return new Users { UserId = newUserId, Username = user.Username };
                }
            }
        }


        //author :Purusothaman
        public List<Products> GetAllProducts()
        {
            List<Products> products = new List<Products>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT ProductId, ProductName, Description, Price, QuantityInStock, Type FROM Products", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Products product = new Products(
                            reader.GetInt32(0),             // ProductId
                            reader.GetString(1),            // ProductName
                            reader.GetString(2),            // Description
                            reader.GetDouble(3),            // Price
                            reader.GetInt32(4),             // QuantityInStock
                            reader.GetString(5)             // Type
                        );
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            List<Order> orders = new List<Order>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Orders WHERE UserId = @UserId";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        // Check if there are rows to read
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                // Create the Order object and handle possible null values
                                int orderId = reader.IsDBNull(0) ? 0 : reader.GetInt32(0); // OrderId
                                int userIdFromDb = reader.IsDBNull(1) ? 0 : reader.GetInt32(1); // UserId
                                DateTime orderDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2); // OrderDate
                                string status = reader.IsDBNull(3) ? string.Empty : reader.GetString(3); // Status
                                int productId = reader.IsDBNull(4) ? 0 : reader.GetInt32(4); // ProductId

                                // Create a new order using the retrieved values
                                Order order = new Order(userIdFromDb, orderId, orderDate, status, productId);
                                orders.Add(order);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No orders found for this user.");
                        }
                    }
                }
            }

            return orders;
        }


        public void CreateOrder(int userId, List<Products> productList)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Assuming there's an Orders table where you first create an order
                string orderSql = "INSERT INTO Orders (UserId, OrderDate) OUTPUT INSERTED.OrderId VALUES (@UserId, @OrderDate)";
                int newOrderId;

                using (var orderCommand = new SqlCommand(orderSql, connection))
                {
                    orderCommand.Parameters.AddWithValue("@UserId", userId);
                    orderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                    newOrderId = (int)orderCommand.ExecuteScalar();
                }

                // Now insert each product with its quantity
                foreach (var product in productList)
                {
                    string orderProductSql = "INSERT INTO OrderProducts (OrderId, ProductId, Quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                    using (var orderProductCommand = new SqlCommand(orderProductSql, connection))
                    {
                        orderProductCommand.Parameters.AddWithValue("@OrderId", newOrderId);
                        orderProductCommand.Parameters.AddWithValue("@ProductId", product.ProductId);
                        orderProductCommand.Parameters.AddWithValue("@Quantity", 1); // Default quantity
                        orderProductCommand.ExecuteNonQuery();
                    }
                }
            }
        }

    }
}
//author :Purusothaman