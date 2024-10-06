using Order_Management_System.Repository;

namespace Order_Management_System.models
{
    public class Order  
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }

        // Add a property to hold the list of products associated with the order
        //public List<Products> Products { get; set; } = new List<Products>();

        // Constructor to initialize properties
        public Order(int userId, int orderId, DateTime orderDate, string status,int productId)
        {
            UserId = userId;
            OrderId = orderId;
            OrderDate = orderDate;
            Status = status;
            ProductId = productId;
        }

        public Order( int userId,int  productId, DateTime date , string status) {
            UserId = userId;
            OrderDate = date;
            Status = status;
            ProductId = productId;
        }
        public Order() { }  

    }
}