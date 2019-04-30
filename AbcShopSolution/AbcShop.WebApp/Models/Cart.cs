using AbcShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AbcShop.WebApp.Models
{
    public class Cart
    {
        private List<CartLine> _cardLines = new List<CartLine>();

        public List<CartLine> CartLines
        {
            get { return _cardLines; }
        }

        public void AddProduct(Product product, int quantity)
        {
            var line = _cardLines.FirstOrDefault(i => i.Product.Id == product.Id);
            if (line == null)
            {
                _cardLines.Add(new CartLine() { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void ReduceToProductCount(Product product)
        {
            var line = _cardLines.FirstOrDefault(i => i.Product.Id == product.Id);
            if(line.Quantity==1)
            {
                DeleteProduct(product);
            }
            else
            {
                line.Quantity -= 1;
            }

        }

        public void DeleteProduct(Product product)
        {
            _cardLines.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double Total()
        {
            return _cardLines.Sum(i => i.Product.Price * i.Quantity);
        }

        public int ProductCount()
        {
            return _cardLines.Sum(i => i.Quantity);
        }

        public void Clear()
        {
            _cardLines.Clear();
        }
    }

    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
