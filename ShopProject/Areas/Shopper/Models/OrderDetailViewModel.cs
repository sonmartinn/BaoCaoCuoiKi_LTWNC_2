using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopProject.Areas.Shopper.Models
{
    public class OrderDetailViewModel
    {
        public string ProductID { get; set; }
        public int? Quantity { get; set; }
        public string TotalPrice { get; set; }
        public string ProductName { get; set; }
    }
}