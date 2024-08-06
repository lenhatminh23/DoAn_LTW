using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class UserController : Controller
    {
        private QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        //
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
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

        [HttpPost]
        public ActionResult Register(KhachHang DangKy, string SoDienThoai, DateTime? NgaySinh, string DiaChi, string GioiTinh)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra tài khoản đã tồn tại trong DB hay chưa
                bool isTaiKhoanExists = db.KhachHangs.Any(kh => kh.TaiKhoan == DangKy.TaiKhoan);

                if (isTaiKhoanExists)
                {
                    ViewBag.ErrorMessage = "Tài khoản đã tồn tại trong hệ thống.";
                    return View("Register", DangKy);
                }

                // Kiểm tra số điện thoại đã tồn tại trong DB hay chưa
                bool isPhoneNumberExists = db.KhachHangs.Any(kh => kh.SoDienThoai == SoDienThoai);

                if (isPhoneNumberExists)
                {
                    ViewBag.ErrorMessage = "Số điện thoại đã tồn tại trong hệ thống.";
                    return View("Register", DangKy);
                }

                if (!string.IsNullOrEmpty(SoDienThoai) && !Regex.IsMatch(SoDienThoai, @"^\d{10}$"))
                {
                    ModelState.AddModelError("SoDienThoai", "Số điện thoại không hợp lệ.");
                }

                if (NgaySinh == null)
                {
                    ModelState.AddModelError("NgaySinh", "Ngày sinh không được để trống.");
                }

                if (string.IsNullOrEmpty(DiaChi))
                {
                    ModelState.AddModelError("DiaChi", "Địa chỉ không được để trống.");
                }
                if (string.IsNullOrEmpty(GioiTinh))
                {
                    ModelState.AddModelError("GioiTinh", "Giới tính không được để trống.");
                }

                if (string.IsNullOrEmpty(DangKy.MatKhau) || DangKy.MatKhau.Length < 8)
                {
                    ModelState.AddModelError("MatKhau", "Mật khẩu phải có ít nhất 8 ký tự.");
                }



                if (!string.IsNullOrEmpty(DangKy.Email) && !IsValidEmail(DangKy.Email))
                {
                    ModelState.AddModelError("Email", "Email không hợp lệ.");
                }

                if (ModelState.IsValid)
                {
                    KhachHang kh = new KhachHang
                    {
                        TenKhachHang = DangKy.TenKhachHang,
                        TaiKhoan = DangKy.TaiKhoan,
                        MatKhau = DangKy.MatKhau,
                        Email = DangKy.Email,
                        SoDienThoai = SoDienThoai,
                        NgaySinh = NgaySinh,
                        DiaChi = DiaChi,
                        GioiTinh = GioiTinh
                    };


                    db.KhachHangs.InsertOnSubmit(kh);
                    db.SubmitChanges();

                    ViewBag.SuccessMessage = "Sign Up Success";
                    ModelState.Clear(); // Xóa tất cả các lỗi ModelState
                    return View(new KhachHang()); // Tạo một đối tượng KhachHang mới để reset form
                }
            }

            return View(DangKy);
        }

        public ActionResult PartialSuccessView()
        {
            return PartialView();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string TaiKhoan, string MatKhau)
        {
            if (string.IsNullOrEmpty(TaiKhoan) || string.IsNullOrEmpty(MatKhau))
            {
                ModelState.AddModelError("", "Vui lòng nhập tên tài khoản và mật khẩu.");
                return View();
            }

            var user = db.KhachHangs.FirstOrDefault(u => u.TaiKhoan == TaiKhoan && u.MatKhau == MatKhau);
            if (user != null)
            {

                Session["User"] = user;
                Session["LoginSuccess"] = true;
                Session["RegisterSuccess"] = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
	}
}