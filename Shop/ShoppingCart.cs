using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int CartUserId { get; set; }
        public int CartProduct { get; set; }
        public int ProductCount { get; set; }
        public int PaymentSum { get; set; }
        public void RemoveFromCart()
        {
            Id = CartUserId = CartProduct = ProductCount = PaymentSum = 0;
        }
    }
}
