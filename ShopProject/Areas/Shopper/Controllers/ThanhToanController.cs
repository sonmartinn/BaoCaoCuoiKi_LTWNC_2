using ShopProject.Areas.Shopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopProject.Areas.Shopper.Controllers
{
    public class ThanhToanController : Controller
    {
        UserContext db = new UserContext();
        // GET: Shopper/ThanhToan
        public ActionResult Index()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return View(giohang);
        }

        [HttpPost]
        public ActionResult StepEnd()
        {
            //Nhận reqest từ trang index
            string phone = Request.Form["phone"];
            string fullname = Request.Form["fullname"];
            string email = Request.Form["email"];
            string address = Request.Form["address"];
            string note = Request.Form["note"];
            //kiểm tra xem có customer chưa và cập nhật lại
            Customer newCus = new Customer();
            var cus = db.Customers.FirstOrDefault(p => p.cusPhone.Equals(phone));
            if (cus != null)
            {
                //nếu có số điện thoại trong db rồi
                //cập nhật thông tin và lưu
                cus.cusFullName = fullname;
                cus.cusEmail = email;
                cus.cusAddress = address;
                db.Entry(cus).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                //nếu chưa có sđt trong db
                //thêm thông tin và lưu
                newCus.cusPhone = phone;
                newCus.cusFullName = fullname;
                newCus.cusEmail = email;
                newCus.cusAddress = address;
                db.Customers.Add(newCus);
                db.SaveChanges();
            }
            //Thêm thông tin vào order và orderdetail
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            // Thêm order mới
            Order newOrder = new Order();

            // Lấy danh sách các order hiện tại
            var existingOrderIds = db.Orders.Select(o => o.orderID).ToList();

            // Tạo ID mới và đảm bảo không trùng
            int baseId = Int32.Parse(db.Orders
                .ToList()
                .OrderByDescending(p => DateTime.Parse(p.orderDateTime))
                .FirstOrDefault()
                .orderID.Replace("HD", ""));

            string newIDOrder = (baseId + 1).ToString();
            string generatedId = "HD" + newIDOrder;

            // Kiểm tra và tăng ID nếu bị trùng
            while (existingOrderIds.Contains(generatedId))
            {
                baseId++;
                newIDOrder = (baseId).ToString();
                generatedId = "HD" + newIDOrder;
            }

            // Gán ID mới vào order
            newOrder.orderID = generatedId;
            newOrder.cusPhone = phone;
            newOrder.orderMessage = note;
            newOrder.orderDateTime = DateTime.Now.ToString();
            newOrder.orderStatus = "Đang xử lý...";

            // Thêm vào cơ sở dữ liệu
            db.Orders.Add(newOrder);
            db.SaveChanges();

            //thêm details
            for (int i = 0; i < giohang.Count; i++)
            {
                OrderDetail newOrdts = new OrderDetail();
                newOrdts.orderID = newOrder.orderID;
                newOrdts.proID = giohang.ElementAtOrDefault(i).SanPhamID;
                newOrdts.ordtsQuantity = giohang.ElementAtOrDefault(i).SoLuong;
                newOrdts.ordtsThanhTien = giohang.ElementAtOrDefault(i).ThanhTien.ToString();
                db.OrderDetails.Add(newOrdts);
                db.SaveChanges();
            }
            Session["MHD"] = "HD"+newIDOrder;
            Session["Phone"] = phone;
            //xoá sạch giỏ hàng
            giohang.Clear();
            return RedirectToAction("HoaDon", "ThanhToan");
        }

        public ActionResult HoaDon()
        {
            return View();
        }
    }
}