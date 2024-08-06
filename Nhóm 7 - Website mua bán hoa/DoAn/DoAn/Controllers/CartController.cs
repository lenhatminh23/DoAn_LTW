using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class CartController : Controller
    {
        QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            return View();
        }
        public List<Cart> LayGioHang()
        {
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<Cart>();
                Session["Cart"] = lstGioHang;
            }
            return lstGioHang;
        }

        [HttpPost]
        public ActionResult ThemGioHang(int mSP, string a)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart SanPham = lstGioHang.Find(sp => sp.iMaSanPham == mSP);
            if (SanPham == null)
            {
                SanPham = new Cart(mSP);
                lstGioHang.Add(SanPham);
            }
            else
            {
                SanPham.iSoLuong++;
            }

            Session["Cart"] = lstGioHang;


            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();

            return RedirectToAction("Cart");
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return tsl;
        }
        private double TongThanhTien()
        {
            double ttt = 0;
            List<Cart> lstGioHang = Session["Cart"] as List<Cart>;
            if (lstGioHang != null)
            {
                ttt += lstGioHang.Sum(sp => sp.dThanhTien);
            }
            return ttt;

        }
        public ActionResult Cart()
        {
            List<Cart> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
        }
        [HttpDelete]
        public ActionResult XoaGioHang(int iMaSanPham)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sanPham = lstGioHang.Find(sp => sp.iMaSanPham == iMaSanPham);

            if (sanPham != null)
            {
                lstGioHang.Remove(sanPham);
                Session["Cart"] = lstGioHang;
            }

            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult XoaHetGioHang()
        {
            Session["Cart"] = null;
            return RedirectToAction("Cart");
        }
        [HttpPost]
        public ActionResult CapNhatGioHang(int iMaSanPham, int iSoLuongMoi)
        {
            List<Cart> lstGioHang = LayGioHang();
            Cart sanPham = lstGioHang.Find(sp => sp.iMaSanPham == iMaSanPham);

            if (sanPham != null)
            {

                sanPham.iSoLuong = iSoLuongMoi;

                Session["Cart"] = lstGioHang;
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult Order(string TenKhachHang, string DiaChi, string SoDienThoai)
        {

            if (Session["User"] == null)
            {
                return RedirectToAction("Login", "User");
            }


            KhachHang user = Session["User"] as KhachHang;


            List<Cart> cart = Session["Cart"] as List<Cart>;


            DonHang donHang = new DonHang
            {
                MaKhachHang = user.MaKhachHang,
                NgayDatHang = DateTime.Now,
                DaThanhToan = "Chưa thanh toán",
                TinhTrangGiaoHang = false
            };

            donHang.NgayGiaoHang = donHang.NgayDatHang.AddDays(14);

            db.DonHangs.InsertOnSubmit(donHang);
            db.SubmitChanges();


            foreach (Cart item in cart)
            {
                ChiTietDonHang chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaSanPham = item.iMaSanPham,
                    TenSanPham = item.sTenSanPham,
                    HinhAnh = item.sHinhAnh,
                    SoLuong = item.iSoLuong,
                    GiaBan = (decimal)item.dGiaBan
                };


                db.ChiTietDonHangs.InsertOnSubmit(chiTietDonHang);
            }


            Session["Cart"] = null;


            db.SubmitChanges();

            return RedirectToAction("OrderSuccess");
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        public ActionResult PartialCart()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            return PartialView();
        }
	}
}