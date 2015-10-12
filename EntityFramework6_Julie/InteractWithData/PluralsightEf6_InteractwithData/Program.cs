using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;
using Domain.Classes;

namespace PluralsightEf6_InteractwithData
{
    class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<SaleContext>());
            //InsertCustomer();
            //InsertCustomer();
            //InsertOrders();
            SimpleOrderQueries();
            //Console.ReadKey();
        }

        private static void InsertCustomer()
        {
            var customer = new Customer
            {
                Name = "Tony",
                DateOfBirth = DateTime.Now,
                Id = 1
            };
            using (var db = new SaleContext())
            {
                db.Customers.Add(customer);

                db.SaveChanges();
            }
        }

        private static void InsertOrder()
        {
            var order = new Order
            {
                Cost = 100,
                CustomerId = 1
            };

            using (var db = new SaleContext())
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }
        }

        private static void InsertOrders()
        {
            var orders = new List<Order>
            {
                new Order { CustomerId = 1, Cost = 200},
                new Order { CustomerId = 1, Cost = 300}
            };

            using (var db = new SaleContext())
            {
                db.Orders.AddRange(orders);
                db.SaveChanges();
            }
        }

        private static void SimpleOrderQueries()
        {
            using (var db = new SaleContext())
            {
                db.Database.Log = Console.WriteLine;
                var orders = db.Orders.ToList();
                foreach (var order in orders)
                {
                    Console.WriteLine("List all orders - order Id:{0}, order cost:{1}", order.Id, order.Cost);
                }

                orders = db.Orders.Where(x => x.Cost > 250).ToList();
                foreach (var order in orders)
                {
                    Console.WriteLine("List cost > 250 orders - Id:{0}, order cost:{1}", order.Id, order.Cost);
                }

                int maxCost = 250;
                orders = db.Orders.Where(x => x.Cost > maxCost).ToList();
                foreach (var order in orders)
                {
                    Console.WriteLine("List cost > 250 orders - Id:{0}, order cost:{1}", order.Id, order.Cost);
                }
            }
        }
    }
}
