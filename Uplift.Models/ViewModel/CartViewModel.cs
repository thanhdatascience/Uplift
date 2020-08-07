using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.Models.ViewModel
{
    public class CartViewModel
    {
        public IList<Service> ServiceList { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
