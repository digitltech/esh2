using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ESH.Models
{
    public class Product
    {   
        public int id { get; set; }

        [Display(Name="Артикул")]
        public string Article { get; set; }

        [Display (Name="Артикул поставшика")]
        public string ArticlePos { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

        [Display (Name ="Описание")]
        public string Descriptions { get; set; }

        [Display (Name="Полное описание")]
        public string Full_Descriptions { get; set; }
        public string Obzor { get; set; }
        [Display(Name = "Цена отображение")]
        [DataType (DataType.Currency)]
        public decimal Price { get; set; }
        public int? PriceTypiesId { get; set; }
        public PriceType PriceTypies { get; set; }
        [Display(Name="Цена Оптовая ")]
        [DataType(DataType.Currency)]
        public decimal PriceOpt { get; set; }

        

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public bool Enaible { get; set; }

        public string Url { get; set; }
        public string MetaKey { get; set; }
        public string MetaDescription { get; set; }
        public int? SkladTypesId { get; set; }
        public SkladType SkladTypes { get; set; }
        public int Ostatok { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateUpdate { get; set; }
        public bool PaymentOnline { get; set; }
        public bool OneImg { get; set; }
        public string Img { get; set; }
        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturers { get; set; }
        public bool Complect { get; set; }
    }

    public class PriceType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Proccent { get; set; }
    }   

    public class Img
    {
        public int Id { get; set; }
        public string img { get; set; }
        public string type_img { get; set; }
        public int? ProductId { get; set; }
        public Product Products { get; set; }
        public string path_img { get; set; }
        public bool in_first { get; set; }


    }

    public class PropertyType
    {
        public int id { get; set; }
        public string  Name { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
        public bool Filter { get; set; }
    }

    public class PropertyValue
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? PropertyTypesid { get; set;}
        public PropertyType PropertyTypes { get; set; }
    }

    public class Property
    {
        public int id { get; set; }
        public int PropertyTypeId { get; set; }
        public PropertyType PropertyTypes { get; set; }
        public int PropertyValueId { get; set; }
        public PropertyValue PropertyValues { get; set; }
        public string Value { get; set; }
        public int ProductId { get; set; }
        public Product Products { get; set; }
        public bool Filter { get; set; }
    }

    public class JoinProduct
    {
        public int id { get; set; }
        public int ProductFirst { get; set; }
        public int ProductId { get; set; }
        public Product Products { get; set; }
    }
    public class SkladType
    {
        public int id { get; set; }
        public string Name { get; set; }
    }

    public class Sklad
    {
        public int id { get; set; }
        public int? ProductId { get; set;}
        public Product Products { get; set; }
        public int Count { get; set;}
        public string Content { get; set; }
    }

    public class Manufacturer
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string img { get; set; }
        public string url { get; set; }
    }

    public class ManufacturerSort
    {
       public int id { get; set; }
        public int? ManufacturerId { get; set; }
        public Manufacturer Manufacturers { get; set; }
        public int? CategoryId { get; set; }
        public Category Categogies { get; set; }
    }

    public enum SortProduct
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }

    public class PropertyName
    {
        public int id { get; set; }
        public string PropertyId { get; set; }
        public string Name { get; set; }
    }
    public class PropertyAll
    {
        public int id { get; set; }
        public int? PropertyNameId { get; set; }
        public PropertyName PropertyNames { get; set; }
        public string PropertyValue { get; set; }
        public int? ProductId { get; set; }
        public Product Products { get; set; }
        public int? CategoryId { get; set; }
        public Category Categories { get; set; }
    }

    public class Complect
    {
        public int id { get; set; }
        public int Productid { get; set; }
        public int? ProductComplect { get; set; }
        public Product Products { get; set; }
    }



}