using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAn.Models
{
    public class Cart
    {
        QuanLyBanHoaDataContext db = new QuanLyBanHoaDataContext();
        public int iMaSanPham { get; set; }
        public string sTenSanPham { get; set; }
        public string sHinhAnh { get; set; }
        public string sMoTa { get; set; }
        public double dGiaBan { get; set; }
        public string sDonVi { get; set; }
        public int iSoLuong { get; set; }
        public DateTime dtHanSuDung { get; set; }
        public int iMaLoai { get; set; }
        public double dThanhTien
        {
            get { return iSoLuong * dGiaBan; }
        }
        public Cart(int maSanPham)
        {
            iMaSanPham = maSanPham;
            Hoa hoa = db.Hoas.Single(s => s.MaSanPham == iMaSanPham);
            sTenSanPham = hoa.TenSanPham;
            sHinhAnh = hoa.HinhAnh;
            sMoTa = hoa.MoTa;
            dGiaBan = double.Parse(hoa.Gia.ToString());
            sDonVi = hoa.DonVi;
            iSoLuong = 1;
            dtHanSuDung = DateTime.Parse(hoa.HanSuDung.ToString());
            iMaLoai = int.Parse(hoa.MaLoai.ToString());
        }
    }
}