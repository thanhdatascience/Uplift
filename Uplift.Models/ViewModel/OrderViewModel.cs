using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.Models.ViewModel
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}
