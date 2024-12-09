using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopProject.Areas.Shopper.Models
{
    public class OrderViewModel
    {
        public string OrderID { get; set; }
        public string CustomerPhone { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public List<OrderDetailViewModel> OrderDetails { get; set; }
    }
}