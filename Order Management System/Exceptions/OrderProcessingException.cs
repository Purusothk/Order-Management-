using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_Management_System.Exceptions
{
    public class OrderProcessingException : Exception
    {
        //author :Purusothaman
        public OrderProcessingException() : base("An error occurred while processing the order.")
        {
        }

        public OrderProcessingException(string message) : base(message)
        {
        }

        public OrderProcessingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
