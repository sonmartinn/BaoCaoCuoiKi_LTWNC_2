using ShopProject.Areas.Shopper.Models;
using System;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Principal;
using System.Web.Mvc;

namespace ShopProject.Areas.Shopper.Controllers
{
    public class LoGinController : Controller
    {

        Models.UserContext db = new Models.UserContext();


        public ActionResult Index()
        {
            return View("LoGin");
        }


        [HttpPost]

        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Tên đăng nhập và mật khẩu không được để trống.";
                return View("LoGin");
            }

            // Kiểm tra tính hợp lệ của tài khoản
            if (IsValidUser(username.Trim(), password.Trim()))
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var user = db.Accounts.FirstOrDefault(a => a.Username == username);
                if (user != null)
                {
                    // Lưu AccountID vào Session
                    Session["AccountID"] = user.AccountID;

                    // Điều hướng đến trang Home
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không chính xác.";
            }

            return View("LoGin");
        }


        public ActionResult Register()
        {
            return View("Register");
        }


        [HttpPost]
        public ActionResult Register(string username, string email, string sdt, string password, string confirmPassword)
        {

            if (password != confirmPassword)
            {
                ViewBag.ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View();
            }


            var existingUser = db.Accounts.FirstOrDefault(a => a.Username == username || a.Email == email);
            if (existingUser != null)
            {
                ViewBag.ErrorMessage = "Tên đăng nhập hoặc email đã tồn tại.";
                return View();
            }


            var newAccount = new Account
            {
                Username = username,
                Email = email,
                SDT = sdt,
                Password = password,
            };

            db.Accounts.Add(newAccount);
            db.SaveChanges();


            return RedirectToAction("Index", "LoGin");
        }


        private bool IsValidUser(string username, string password)
        {

            var user = db.Accounts.FirstOrDefault(a => a.Username == username);
            return user != null && user.Password == password;
        }
    }
}