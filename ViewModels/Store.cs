using CustomViewEngine.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomViewEngine.ViewModels
{
    public class Store
    {
        public Store()
        {
            Products = new List<Product>
            {
                new Product {ID = 1, ProductName = "Azúcar", UnitPrice = 10, UnitsInStock = 100},
                new Product {ID = 2, ProductName = "Leche", UnitPrice = 13, UnitsInStock = 100},
                new Product {ID = 3, ProductName = "Huevos", UnitPrice = 167, UnitsInStock = 100},
                new Product {ID = 4, ProductName = "Arroz", UnitPrice = 23, UnitsInStock = 1500},
                new Product {ID = 5, ProductName = "Frijol", UnitPrice = 10, UnitsInStock = 1100}
            };
        }

        public List<Product> Products { get; set; }

        public IEnumerable IDs
        {
            get
            {
                return Products.Select(p => new { p.ID, p.ProductName }).ToArray();
            }
        }
    }
}