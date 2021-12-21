using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ESH.Models
{
    public class Order
    {
        public int id { get; set; }
        public int NumberOrder { get; set; }
        public Guid Guet { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int CostumerId { get; set; }
        //public Costumer Costumers { get; set; }
        public string Content { get; set; }
        public int? DevelireryId { get; set; }
        public Develirery Develireries { get; set; }
        public decimal Total { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public int PaymentTypeId { get; set; }
        public PaymentType PaymentTypes { get; set; }
        public int StatusOrderId { get; set; }
        public StatusOrder StatusOrders { get; set; }
        [Required]
        [Display(Name = "Имя покупателя")]
        public string Costumer { get; set; }
        public bool PayOnline { get; set; }
        public string AmountDate { get; set; }
        public string Amountid { get; set; }
        public decimal AmountSum { get; set; }
        public decimal ShippingSumm { get; set; }
        public decimal TotalSumm { get; set; }
        [Required]
        [Display(Name = "Телефон")]
        public string contactPhone { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string email { get; set; }
    }

    public class Costumer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }    
        public string Adress { get; set; }
        public string Email { get; set; }
    }

    public class OrderDetails
    {
        public int id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public virtual Product Products { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PaymentType
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class StatusOrder
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    public class Develirery
    {
        public int id { get; set; }
        public string Name { get; set; }

    }

    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tarif { get; set; }
    }

    public class DeveliryPrice
    {
        public int Id { get; set; }
        public string City { get; set; }
        public decimal Tarif { get; set; }
        public decimal sklad_3000 { get; set; }
        public decimal dver3000 { get; set; }
        public decimal sklad30000 { get; set; }
        public decimal dver30000 { get; set; }
    }
   
}