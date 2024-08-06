using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using DoAn.Models;

namespace DoAn.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
    {
        private QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        //
        // GET: /Admin/HomeAdmin/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(string TaiKhoanQTV, string MatKhauQTV)
        {
            if (string.IsNullOrEmpty(TaiKhoanQTV) || string.IsNullOrEmpty(MatKhauQTV))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên tài khoản và mật khẩu.");
                return View();
            }

            var userAdmin = db.QuanTriViens.FirstOrDefault(u => u.TaiKhoanQTV == TaiKhoanQTV && u.MatKhauQTV == MatKhauQTV);
            if (userAdmin != null)
            {

                Session["UserAdmin"] = userAdmin;
                Session["LoginSuccess"] = true;
                Session["RegisterSuccess"] = null;
                return RedirectToAction("IndexAdmin", "HomeAdmin");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                return View();
            }
        }
        [HttpGet]
        public ActionResult LogoutAdmin()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("LoginAdmin", "UserAdmin");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public ActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterAdmin(QuanTriVien dkAdmin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(dkAdmin.MatKhauQTV) || dkAdmin.MatKhauQTV.Length < 8)
                {
                    ModelState.AddModelError("MatKhauQTV", "Mật khẩu phải có ít nhất 8 ký tự.");
                }



                if (!string.IsNullOrEmpty(dkAdmin.EmailQTV) && !IsValidEmail(dkAdmin.EmailQTV))
                {
                    ModelState.AddModelError("EmailQTV", "Email không hợp lệ.");
                }

                if (ModelState.IsValid)
                {
                    QuanTriVien qtv = new QuanTriVien
                    {
                        TenQTV = dkAdmin.TenQTV,
                        EmailQTV = dkAdmin.EmailQTV,
                        TaiKhoanQTV = dkAdmin.TaiKhoanQTV,
                        MatKhauQTV = dkAdmin.MatKhauQTV
                    };


                    db.QuanTriViens.InsertOnSubmit(qtv);
                    db.SubmitChanges();

                    ViewBag.SuccessMessage = "Register Success";
                    ModelState.Clear(); // Xóa tất cả các lỗi ModelState
                    return View(new QuanTriVien()); // Tạo một đối tượng KhachHang mới để reset form
                }
            }

            return View(dkAdmin);
        }
        //Tài khoản người dùng
        public ActionResult UserList(int? page_list)
        {
            int page_size = 4;

            int page_number = (page_list ?? 1);

            var user_list = db.KhachHangs.OrderByDescending(s => s.TenKhachHang).Skip((page_number - 1) * page_size).Take(page_size).ToList();

            ViewBag.PageCount = (int)Math.Ceiling((double)db.KhachHangs.Count() / page_size);

            ViewBag.CurrentPage = page_number;

            return View(user_list);
        }

        public ActionResult DetailsUser(int id)
        {
            var user = db.KhachHangs.FirstOrDefault(s => s.MaKhachHang == id);
            return View(user);
        }

        public ActionResult EditUser(int id)
        {
            var user = db.KhachHangs.FirstOrDefault(s => s.MaKhachHang == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }
        [HttpPost]
        public ActionResult EditUser(KhachHang user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.KhachHangs.FirstOrDefault(s => s.MaKhachHang == user.MaKhachHang);

                if (existingUser == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin người dùng
                existingUser.TenKhachHang = user.TenKhachHang;
                existingUser.DiaChi = user.DiaChi;
                existingUser.SoDienThoai = user.SoDienThoai;
                existingUser.Email = user.Email;
                existingUser.NgaySinh = user.NgaySinh;
                existingUser.TaiKhoan = user.TaiKhoan;
                existingUser.MatKhau = user.MatKhau;
                existingUser.GioiTinh = user.GioiTinh;

                db.SubmitChanges();

                return RedirectToAction("DetailsUser", new { id = user.MaKhachHang });
            }
            return View(user);
        }
        public ActionResult DeleteUser(int id)
        {
            var user = db.KhachHangs.FirstOrDefault(s => s.MaKhachHang == id);
            return View(user);
        }
        [HttpPost]
        public ActionResult ConfirmDeleteUser(int id)
        {
            var user = db.KhachHangs.FirstOrDefault(s => s.MaKhachHang == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            // Manually delete related records in DonHang table
            var relatedOrders = db.DonHangs.Where(d => d.MaKhachHang == id).ToList();

            // Manually delete related records in ChiTietDonHang table
            foreach (var order in relatedOrders)
            {
                var relatedChiTietDonHangs = db.ChiTietDonHangs.Where(c => c.MaDonHang == order.MaDonHang).ToList();
                db.ChiTietDonHangs.DeleteAllOnSubmit(relatedChiTietDonHangs);
            }

            // Manually delete related records in PhanHoi table
            var relatedPhanHois = db.PhanHois.Where(p => p.MaKhachHang == id).ToList();
            db.PhanHois.DeleteAllOnSubmit(relatedPhanHois);

            // Delete the user
            db.KhachHangs.DeleteOnSubmit(user);

            // Delete related orders in DonHang table
            db.DonHangs.DeleteAllOnSubmit(relatedOrders);

            // Submit changes to the database
            db.SubmitChanges();

            return RedirectToAction("UserList");
        }

        //Tài khoản Admin
        public ActionResult AdminList(int? page_list)
        {
            int page_size = 4;

            int page_number = (page_list ?? 1);

            var admin_list = db.QuanTriViens.OrderBy(s => s.TenQTV).Skip((page_number - 1) * page_size).Take(page_size).ToList();

            ViewBag.PageCount = (int)Math.Ceiling((double)db.QuanTriViens.Count() / page_size);

            ViewBag.CurrentPage = page_number;

            return View(admin_list);
        }
        public ActionResult CreateAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAdmin(QuanTriVien qtv)
        {
            if (ModelState.IsValid)
            {
                bool isEmailExists = db.QuanTriViens.Any(a => a.EmailQTV == qtv.EmailQTV);
                bool isTaiKhoanExists = db.QuanTriViens.Any(a => a.TaiKhoanQTV == qtv.TaiKhoanQTV);

                if (isTaiKhoanExists)
                {
                    ViewBag.ErrorMessage = "Tài khoản đã tồn tại trong hệ thống.";
                    return View("CreateAdmin", qtv);
                }
                else if (isEmailExists)
                {
                    ViewBag.ErrorMessage = "Email đã tồn tại trong hệ thống.";
                    return View("CreateAdmin", qtv);
                }
                else
                {
                    QuanTriVien admin = new QuanTriVien
                    {
                        TenQTV = qtv.TenQTV,
                        TaiKhoanQTV = qtv.TaiKhoanQTV,
                        MatKhauQTV = qtv.MatKhauQTV,
                        EmailQTV = qtv.EmailQTV
                    };

                    db.QuanTriViens.InsertOnSubmit(admin);
                    db.SubmitChanges();
                    ModelState.Clear();

                    return RedirectToAction("AdminList");
                }
            }
            return RedirectToAction("AdminList");
        }

        public ActionResult DetailsAdmin(int id)
        {
            var admin = db.QuanTriViens.FirstOrDefault(s => s.MaQTV == id);
            return View(admin);
        }

        public ActionResult EditAdmin(int id)
        {
            var admin = db.QuanTriViens.FirstOrDefault(s => s.MaQTV == id);

            if (admin == null)
            {
                return HttpNotFound();
            }

            return View(admin);
        }
        [HttpPost]
        public ActionResult EditAdmin(QuanTriVien admin)
        {
            if (ModelState.IsValid)
            {
                var existingAdmin = db.QuanTriViens.FirstOrDefault(s => s.MaQTV == admin.MaQTV);

                if (existingAdmin == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin người dùng
                existingAdmin.TenQTV = admin.TenQTV;
                existingAdmin.TaiKhoanQTV = admin.TaiKhoanQTV;
                existingAdmin.MatKhauQTV = admin.MatKhauQTV;
                existingAdmin.EmailQTV = admin.EmailQTV;

                db.SubmitChanges();

                return RedirectToAction("DetailsAdmin", new { id = admin.MaQTV });
            }
            return View(admin);
        }
        public ActionResult DeleteAdmin(int id)
        {
            var admin = db.QuanTriViens.FirstOrDefault(s => s.MaQTV == id);
            return View(admin);
        }
        [HttpPost]
        public ActionResult ConfirmDeleteAdmin(int id)
        {
            var admin = db.QuanTriViens.FirstOrDefault(s => s.MaQTV == id);

            if (admin == null)
            {
                return HttpNotFound(); // Hoặc bạn có thể trả về một View thông báo lỗi khác tùy ý
            }

            db.QuanTriViens.DeleteOnSubmit(admin);
            db.SubmitChanges();

            return RedirectToAction("AdminList"); // Hoặc chuyển hướng đến một action khác tùy ý sau khi xóa thành công
        }
	}
}