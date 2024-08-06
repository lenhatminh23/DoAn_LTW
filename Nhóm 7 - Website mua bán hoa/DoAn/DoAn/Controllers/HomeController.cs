using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        private QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
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
        public ActionResult Contact(PhanHoi PhanHoi, string SoDienThoai, string NoiDung, string GioiTinh)
        {
            KhachHang user = Session["User"] as KhachHang;

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(SoDienThoai) && !Regex.IsMatch(SoDienThoai, @"^\d{10}$"))
                {
                    ModelState.AddModelError("SoDienThoai", "Số điện thoại không hợp lệ.");
                }

                if (string.IsNullOrEmpty(NoiDung))
                {
                    ModelState.AddModelError("NoiDung", "Nội dung không được để trống.");
                }
                if (string.IsNullOrEmpty(GioiTinh))
                {
                    ModelState.AddModelError("GioiTinh", "Giới tính không được để trống.");
                }
                if (!string.IsNullOrEmpty(PhanHoi.Email) && !IsValidEmail(PhanHoi.Email))
                {
                    ModelState.AddModelError("Email", "Email không hợp lệ.");
                }

                if (ModelState.IsValid)
                {
                    PhanHoi ph = new PhanHoi
                    {
                        MaKhachHang = user.MaKhachHang,
                        TenKhachHang = PhanHoi.TenKhachHang,
                        Email = PhanHoi.Email,
                        NoiDung = NoiDung,
                        SoDienThoai = SoDienThoai,
                        ThoiGian = DateTime.Now,
                        GioiTinh = GioiTinh
                    };

                    db.PhanHois.InsertOnSubmit(ph);
                    db.SubmitChanges();

                    ViewBag.SuccessMessage = "Contact Success";
                }
            }


            return RedirectToAction("ContactSuccess");
        }
        public ActionResult ContactSuccess()
        {
            return View();
        }
        
        public ActionResult IndexBG()
        {
            return View();
        }
    }
}