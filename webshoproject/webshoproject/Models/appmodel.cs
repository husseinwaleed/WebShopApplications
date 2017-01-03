using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace webshoproject.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string deliveryadress { get; set; }
        public string deliverycity { get; set; }
        public string billingadress { get; set; }
        public string billingcity { get; set; }
        public string email { get; set; }
        public virtual List<Order> orderhistory { get; set; }
        public string user { get; set; }
       public virtual List<message> messages { get; set; }
        public int messagecounter { get; set; }
        

    }
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public Customer customer { get; set; }
        public DateTime orderdate { get; set; }
        public virtual List<OrderRow> orderrow { get; set; }
    }
    public class OrderRow
    {
        [Key]
        public int Id { get; set; }
        public Order order { get; set; }
        public int price { get; set; }
        public Car car { get; set; }
    }
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string factory { get; set; }
        public string model { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public int price { get; set; }
        public virtual List<OrderRow> orderrow { get; set; }
        public byte[] Image { get; set; }

    }
    public class shoppingcart
    {
        [Key]
        public int Id { get; set; }
        public Customer customer { get; set; }
        public  virtual List<Car> car { get; set; }
  
    }
    public class message
    {
        [Key]
        public int Id { get; set; }
        public Customer customer { get; set; }
        
        public string body { get; set; }



    }
}

