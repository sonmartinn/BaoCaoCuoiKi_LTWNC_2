using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using ShopProject.Areas.Administrator.Models;

namespace ShopProject.Areas.Administrator.Controllers
{
    public class OrderController : Controller
    {
        private AdminContext db = new AdminContext();

        // GET: Administrator/Order
        [HandleError]
        public ActionResult Index(string error,string id)
        {
            if (Session["accname"] == null)
            {
                Session["accname"] = null;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.OrderError = error;
                var orders = db.Orders.ToList();
                if (!string.IsNullOrEmpty(id))
                {
                    orders = orders.Where(p => p.orderID.Contains(id.ToUpper())).ToList();
                }
                return View(orders);
            }
        }

        // GET: Administrator/Order/Details/{id}
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var order = db.Orders
                          .Include("OrderDetails.Product") // Load thêm thông tin sản phẩm
                          .FirstOrDefault(o => o.orderID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        // POST: Administrator/Order/UpdateStatus/{id}
        [HttpPost]
        public ActionResult UpdateStatus(string id, string status)
        {
            if (id == null || string.IsNullOrEmpty(status))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            order.orderStatus = status;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET: Administrator/Order/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["accname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var order = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.orderID == id);

            if (order == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            ViewBag.Products = db.Products.ToList(); // Danh sách sản phẩm để hiển thị trong dropdown
            return View(order);
        }

        // POST: Administrator/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order updatedOrder, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var order = db.Orders
                              .Include("OrderDetails")
                              .FirstOrDefault(o => o.orderID == updatedOrder.orderID);

                if (order == null)
                {
                    return HttpNotFound("Đơn hàng không tồn tại.");
                }

                // Cập nhật thông tin chung của đơn hàng
                order.orderMessage = updatedOrder.orderMessage;
                order.orderStatus = updatedOrder.orderStatus;
                order.orderDateTime = updatedOrder.orderDateTime;

                // Xóa chi tiết đơn hàng cũ
                db.OrderDetails.RemoveRange(order.OrderDetails);

                // Thêm chi tiết đơn hàng mới
                var productIds = form.GetValues("ProductId");
                var quantities = form.GetValues("Quantity");

                if (productIds != null && quantities != null)
                {
                    for (int i = 0; i < productIds.Length; i++)
                    {
                        var productId = productIds[i];
                        var quantity = int.Parse(quantities[i]);

                        var product = db.Products.FirstOrDefault(p => p.proID == productId);
                        if (product != null)
                        {
                            var newDetail = new OrderDetail
                            {
                                orderID = updatedOrder.orderID,
                                proID = productId,
                                ordtsQuantity = quantity,
                                ordtsThanhTien = (quantity * int.Parse(product.proPrice)).ToString()
                            };
                            db.OrderDetails.Add(newDetail);
                        }
                    }
                }

                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật đơn hàng thành công.";
                return RedirectToAction("Index");
            }

            ViewBag.Products = db.Products.ToList(); // Đưa lại danh sách sản phẩm khi xảy ra lỗi
            return View(updatedOrder);
        }


        // POST: Administrator/Order/Delete/5
        [HandleError]
        public ActionResult Delete(string id)
        {
            var order = db.Orders.Include("OrderDetails").FirstOrDefault(o => o.orderID == id);

            if (order == null)
            {
                return HttpNotFound("Đơn hàng không tồn tại.");
            }

            // Xóa tất cả chi tiết đơn hàng trước
            db.OrderDetails.RemoveRange(order.OrderDetails);

            // Xóa đơn hàng
            db.Orders.Remove(order);

            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa đơn hàng thành công.";
            return RedirectToAction("Index");
        }
    }
}
