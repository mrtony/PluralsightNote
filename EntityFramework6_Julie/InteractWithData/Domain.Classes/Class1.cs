using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Classes
{
    public class Class1
    {
    }

    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }

    //public class Order
    //{
    //    public int Id { get; set; }
    //    public int Cost { get; set; }
    //    public Customer customer { get; set; }
    //    public int CustomerId { get; set; }
    //}
}
