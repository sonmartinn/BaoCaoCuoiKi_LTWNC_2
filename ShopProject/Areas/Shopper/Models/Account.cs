using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopProject.Areas.Shopper.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string Password { get; set; }
    }
}