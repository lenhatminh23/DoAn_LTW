using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Areas.Admin.Controllers
{
    public class ProductsAdminController : Controller
    {
        QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        //
        // GET: /Admin/ProductsAdmin/
        public ActionResult Index()
        {
            return View();
        }

        //Danh sách đơn hàng
        public ActionResult OrderList()
        {
            try
            {


                var order_list = new Order
                {
                    DonHangs = db.DonHangs.ToList(),
                    ChiTietDonHangs = db.ChiTietDonHangs.ToList()
                };

                return View(order_list);
            }
            catch (Exception ex)
            {
                // Xử lý và ghi log cho ngoại lệ
                // Trả về một view lỗi hoặc chuyển hướng đến trang lỗi
                return View("Error");
            }
        }
        [HttpPost]
        public ActionResult UpdateTinhTrang(int maDonHang, bool isChecked)
        {
            try
            {
                var donHang = db.DonHangs.FirstOrDefault(d => d.MaDonHang == maDonHang);
                if (donHang != null)
                {
                    // Assuming TinhTrangGiaoHang is a boolean property
                    donHang.TinhTrangGiaoHang = isChecked;
                    donHang.DaThanhToan = isChecked ? "Đã thanh toán" : "Chưa thanh toán";
                    db.SubmitChanges();
                }

                return Json(new { success = "true" });
            }
            catch (Exception ex)
            {
                return Json(new { success = "false", error = ex.Message });
            }
        }

        //Danh sách sản phẩm
        public ActionResult ListProductsAdmin(string sortOrder, int? page_list)
        {
            int page_size = 6;

            int page_number = (page_list ?? 1);

            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "" : "name_desc";
            ViewBag.PriceSortParam = sortOrder == "price" ? "price" : "price_desc";

            var hoa_list = db.Hoas.ToList();

            switch (sortOrder)
            {
                case "name_desc":
                    hoa_list = hoa_list.OrderByDescending(s => s.TenSanPham).ToList();
                    break;
                case "price":
                    hoa_list = hoa_list.OrderBy(s => s.Gia).ToList();
                    break;
                case "price_desc":
                    hoa_list = hoa_list.OrderByDescending(s => s.Gia).ToList();
                    break;
                default:
                    hoa_list = hoa_list.OrderBy(s => s.TenSanPham).ToList();
                    break;
            }

            ViewBag.PageCount = (int)Math.Ceiling((double)db.Hoas.Count() / page_size);
            ViewBag.CurrentPage = page_number;
            hoa_list = hoa_list.Skip((page_number - 1) * page_size).Take(page_size).ToList();
            ViewBag.SortOrder = sortOrder;

            return View(hoa_list);
        }
        public ActionResult CreateListProductsAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateListProductsAdmin(Hoa h)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu số lượng và mã loại không âm
                if (h.SoLuong >= 0 && h.MaLoai >= 1)
                {
                    Hoa hoa = new Hoa
                    {
                        TenSanPham = h.TenSanPham,
                        HinhAnh = h.HinhAnh,
                        MoTa = h.MoTa,
                        Gia = h.Gia,
                        DonVi = h.DonVi,
                        SoLuong = h.SoLuong,
                        HanSuDung = h.HanSuDung,
                        MaLoai = h.MaLoai
                    };

                    db.Hoas.InsertOnSubmit(hoa);
                    db.SubmitChanges();
                    ModelState.Clear(); // Xóa tất cả các lỗi ModelState
                }
                else
                {
                    // Nếu số lượng hoặc mã loại âm, thêm lỗi vào ModelState
                    ModelState.AddModelError("SoLuong", "Số lượng phải là số không âm.");
                    ModelState.AddModelError("MaLoai", "Mã loại phải là số không âm.");
                }
            }

            return RedirectToAction("ListProductsAdmin");
        }
        public ActionResult DetailsListProductsAdmin(int id)
        {
            var listproductsadmin = db.Hoas.FirstOrDefault(s => s.MaSanPham == id);
            return View(listproductsadmin);
        }

        public ActionResult EditListProductsAdmin(int id)
        {
            var listproductsadmin = db.Hoas.FirstOrDefault(s => s.MaSanPham == id);

            if (listproductsadmin == null)
            {
                return HttpNotFound();
            }

            return View(listproductsadmin);
        }
        [HttpPost]
        public ActionResult EditListProductsAdmin(Hoa listproductsadmin)
        {
            if (ModelState.IsValid)
            {
                var existingListproductsadmin = db.Hoas.FirstOrDefault(s => s.MaSanPham == listproductsadmin.MaSanPham);

                if (existingListproductsadmin == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật thông tin người dùng
                existingListproductsadmin.TenSanPham = listproductsadmin.TenSanPham;
                existingListproductsadmin.HinhAnh = listproductsadmin.HinhAnh;
                existingListproductsadmin.MoTa = listproductsadmin.MoTa;
                existingListproductsadmin.Gia = listproductsadmin.Gia;
                existingListproductsadmin.DonVi = listproductsadmin.DonVi;
                existingListproductsadmin.SoLuong = listproductsadmin.SoLuong;
                existingListproductsadmin.HanSuDung = listproductsadmin.HanSuDung;
                existingListproductsadmin.MaLoai = listproductsadmin.MaLoai;

                db.SubmitChanges();

                return RedirectToAction("DetailsListProductsAdmin", new { id = listproductsadmin.MaSanPham });
            }
            return View(listproductsadmin);
        }
        public ActionResult DeleteListProductsAdmin(int id)
        {
            var listproductsadmin = db.Hoas.FirstOrDefault(s => s.MaSanPham == id);
            return View(listproductsadmin);
        }
        [HttpPost]
        public ActionResult ConfirmDeleteListProductsAdmin(int id)
        {
            var listproductsadmin = db.Hoas.FirstOrDefault(s => s.MaSanPham == id);

            if (listproductsadmin == null)
            {
                return HttpNotFound();
            }
            var relatedKhoHangRecords = db.KhoHangs.Where(k => k.MaSanPham == id).ToList();
            foreach (var khoHangRecord in relatedKhoHangRecords)
            {
                db.KhoHangs.DeleteOnSubmit(khoHangRecord);
            }
            var relatedNhaCungCapRecords = db.NhaCungCaps.Where(n => n.MaSanPham == id).ToList();
            foreach (var nhaCungCapRecord in relatedNhaCungCapRecords)
            {
                db.NhaCungCaps.DeleteOnSubmit(nhaCungCapRecord);
            }
            var relatedPhieuNhapHangRecords = db.PhieuNhapHangs.Where(p => p.MaNhaCungCap == id).ToList();
            foreach (var phieuNhapHangRecord in relatedPhieuNhapHangRecords)
            {
                db.PhieuNhapHangs.DeleteOnSubmit(phieuNhapHangRecord);
            }


            // Delete the user
            db.Hoas.DeleteOnSubmit(listproductsadmin);


            // Submit changes to the database
            db.SubmitChanges();

            return RedirectToAction("ListProductsAdmin");
        }
	}
}