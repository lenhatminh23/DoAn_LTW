using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class ValuesController : ApiController
    {
        private QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();

        // GET: api/DonHang/GetProducts
        [HttpGet]
        [Route("api/Products/GetProducts")]
        public IHttpActionResult GetProducts()
        {
            try
            {
                var products = from p in db.Hoas
                                                 select new
                                                 {
                                                     MaSanPham = p.MaSanPham,
                                                     TenSanPham = p.TenSanPham,
                                                     HinhAnh = p.HinhAnh,
                                                     MoTa = p.MoTa,
                                                     Gia = p.Gia,
                                                     DonVi = p.DonVi,
                                                     SoLuong = p.SoLuong,
                                                     MaLoai = p.MaLoai
                                                 };

                return Ok(products);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/DonHang/GetDonHangsAndChiTiet
        [HttpGet]
        [Route("api/Order/GetDonHangsAndChiTiet")]
        public IHttpActionResult GetDonHangsAndChiTiet()
        {
            try
            {
                var donHangsAndChiTietDonHangs = from dh in db.DonHangs
                                                 select new
                                                 {
                                                     MaDonHang = dh.MaDonHang,
                                                     MaKhachHang = dh.MaKhachHang,
                                                     NgayDatHang = dh.NgayDatHang,
                                                     NgayGiaoHang = dh.NgayGiaoHang,
                                                     TinhTrangGiaoHang = dh.TinhTrangGiaoHang,
                                                     DaThanhToan = dh.DaThanhToan
                                                 };

                return Ok(donHangsAndChiTietDonHangs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}