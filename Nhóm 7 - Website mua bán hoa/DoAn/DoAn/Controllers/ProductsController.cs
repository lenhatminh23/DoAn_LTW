using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAn.Models;

namespace DoAn.Controllers
{
    public class ProductsController : Controller
    {
        QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        //
        // GET: /Products/
        public ActionResult ListProducts(string sortOrder, int? page_list)
        {
            int page_size = 8;

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
        public ActionResult FeaturedProducts()
        {
            // Đoạn code lấy tất cả sản phẩm bằng LINQ
            var allProducts = db.Hoas.ToList(); // Replace with your LINQ query

            // Chọn ra 4 sản phẩm đầu tiên bằng LINQ
            var top4Products = allProducts.Take(4).ToList();

            return View(top4Products);
        }
        public ActionResult NewestProducts()
        {
            // Đoạn code lấy tất cả sản phẩm bằng LINQ
            var allProducts = db.Hoas.ToList(); // Thay thế bằng truy vấn LINQ của bạn

            // Sắp xếp sản phẩm theo giá giảm dần và chọn ra 8 sản phẩm đầu tiên
            var top8HighPricedProducts = allProducts.OrderByDescending(p => p.Gia).Take(8).ToList();

            return View(top8HighPricedProducts);
        }
        public ActionResult RandomProducts()
        {
            // Đoạn code lấy tất cả sản phẩm bằng LINQ
            var allProducts = db.Hoas.ToList(); // Thay thế bằng truy vấn LINQ của bạn

            Random random = new Random();

            // Chọn ra 4 sản phẩm ngẫu nhiên
            var randomProducts = allProducts.OrderBy(item => random.Next()).Take(4).ToList();

            return View(randomProducts);
        }

        //
        // GET: /Products/Details/5
        public ActionResult Details(int id)
        {
            var ProductDetail = db.Hoas.FirstOrDefault(s => s.MaSanPham == id);
            return View(ProductDetail);
        }

        //
        // GET: /Products/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Products/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Products/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Products/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
