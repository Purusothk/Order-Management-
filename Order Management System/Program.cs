using Order_Management_System.models;
using Order_Management_System.Repository;
using Order_Management_System.models;
using Order_Management_System.menu;

namespace Order_Management_System
{

    internal class Program
    {
        //author :Purusothaman
        static void Main(string[] args)
        {
            OrderManagement1 management = new OrderManagement1();
            management.Run();
        }
    }
}

