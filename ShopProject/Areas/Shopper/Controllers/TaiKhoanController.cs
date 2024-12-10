using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ShopProject.Areas.Shopper.Models;

namespace ShopProject.Areas.Shopper.Controllers
{
    public class TaiKhoanController : Controller
    {
        Models.UserContext db = new Models.UserContext();

        public ActionResult Index()
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return View(new List<Account>());
            }

            var accounts = new List<Account> { user };
            return View(accounts);
        }

        [HttpPost]
        public ActionResult UpdateUser(string fullname, string email, string phone)
        {
            var accountId = Session["AccountID"] as int?;
            if (accountId == null)
            {
                return RedirectToAction("Index", "LoGin");
            }

            var user = db.Accounts.FirstOrDefault(a => a.AccountID == accountId);
            if (user != null)
            {
                user.Username = fullname;
                user.Email = email;
                user.SDT = phone;

                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản để cập nhật!";
            }

            return RedirectToAction("Index");
        }

        private Account GetCurrentUser()
        {
            var accountId = Session["AccountID"] as int?;
            if (accountId == null)
            {
                return null;
            }

            return db.Accounts.FirstOrDefault(a => a.AccountID == accountId);
        }
    }
}
