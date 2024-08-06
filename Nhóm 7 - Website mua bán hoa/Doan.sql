CREATE DATABASE QUANLYSHOPHOA;
GO
USE QUANLYSHOPHOA;
GO

CREATE TABLE LoaiSanPham (
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(50) COLLATE Vietnamese_CI_AS NOT NULL
);


CREATE TABLE Hoa (
    MaSanPham INT IDENTITY(1,1) PRIMARY KEY,
    TenSanPham NVARCHAR(255) COLLATE Vietnamese_CI_AS NOT NULL,
	HinhAnh NVARCHAR(255) NULL,
    MoTa NVARCHAR(255) COLLATE Vietnamese_CI_AS,
    Gia DECIMAL(10, 2) NOT NULL,
    DonVi NVARCHAR(10),
    SoLuong INT,
    HanSuDung DATETIME,
    MaLoai INT,
    CONSTRAINT FK_Hoa_LoaiSanPham FOREIGN KEY (MaLoai) REFERENCES LoaiSanPham(MaLoai)
);


CREATE TABLE QuanTriVien (
	MaQTV INT IDENTITY(1,1) PRIMARY KEY,
	TenQTV NVARCHAR(255) NOT NULL,
	TaiKhoanQTV VARCHAR(50) NOT NULL,
	MatKhauQTV VARCHAR(50) NOT NULL,
	EmailQTV VARCHAR(100) UNIQUE,
    CONSTRAINT CK_EmailQTV CHECK (EmailQTV LIKE '%@%.%'),
);


CREATE TABLE KhachHang (
    MaKhachHang INT IDENTITY(1,1) PRIMARY KEY,
    TenKhachHang NVARCHAR(255) NOT NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20) UNIQUE,
    Email VARCHAR(100) UNIQUE,
    NgaySinh DATETIME,
	TaiKhoan VARCHAR(50) NOT NULL,
    MatKhau VARCHAR(50) NOT NULL,
	GioiTinh NVARCHAR(10),
    CONSTRAINT CK_SoDienThoai CHECK (LEN(SoDienThoai) >= 10),
    CONSTRAINT CK_Email CHECK (Email LIKE '%@%.%')
);


CREATE TABLE DonHang (
    MaDonHang INT IDENTITY(1,1) PRIMARY KEY,
    MaKhachHang INT NOT NULL,
    NgayDatHang DATETIME NOT NULL,
    NgayGiaoHang DATETIME,
    DaThanhToan  NVARCHAR(255),
    TinhTrangGiaoHang BIT NOT NULL,
   CONSTRAINT FK_DonHang_KhachHang FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
);


CREATE TABLE ChiTietDonHang (
    MaChiTiet INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
	MaSanPham INT,
	TenSanPham NVARCHAR(255) COLLATE Vietnamese_CI_AS NOT NULL,
	HinhAnh NVARCHAR(255) NULL,
    SoLuong INT NOT NULL,
    GiaBan DECIMAL(10, 2) NOT NULL,
    CONSTRAINT FK_ChiTietDonHang_DonHang FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang),
    CONSTRAINT FK_ChiTietDonHang_Hoa FOREIGN KEY (MaSanPham) REFERENCES Hoa(MaSanPham)
);


CREATE TABLE KhoHang (
    MaSanPham INT NOT NULL,
    SoLuongTrongKho INT NOT NULL,
    CONSTRAINT PK_KhoHang PRIMARY KEY (MaSanPham),
    CONSTRAINT FK_KhoHang_Hoa FOREIGN KEY (MaSanPham) REFERENCES Hoa(MaSanPham)
);


CREATE TABLE NhaCungCap (
    MaNhaCungCap INT IDENTITY(1, 1) PRIMARY KEY,
    TenNhaCungCap NVARCHAR(255) NOT NULL,
	HinhAnh NVARCHAR(255) NULL,
    DiaChi NVARCHAR(255),
    SoDienThoai VARCHAR(20),
    Email NVARCHAR(100),
    MaSanPham INT, 
    CONSTRAINT CK_SoDienThoai_NCC CHECK (LEN(SoDienThoai) >= 10),
    CONSTRAINT CK_Email_NCC CHECK (Email LIKE '%@%.%'),
    CONSTRAINT FK_NhaCungCap_Hoa FOREIGN KEY (MaSanPham) REFERENCES Hoa(MaSanPham)
);
CREATE TABLE PhieuNhapHang (
    MaPhieuNhap INT IDENTITY(1,1) PRIMARY KEY,
    MaNhaCungCap INT NOT NULL,
    NgayNhap DATE NOT NULL,
    TongTien DECIMAL(12, 2) NOT NULL,
    GhiChu NVARCHAR(255),
    CONSTRAINT FK_PhieuNhapHang_NhaCungCap FOREIGN KEY (MaNhaCungCap) REFERENCES NhaCungCap(MaNhaCungCap)
);
CREATE TABLE LichSuGiaoDich (
    MaLichSu INT IDENTITY(1,1) PRIMARY KEY,
    MaDonHang INT NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL,
    NgayCapNhat DATE NOT NULL,
    CONSTRAINT FK_LichSuGiaoDich_DonHang FOREIGN KEY (MaDonHang) REFERENCES DonHang(MaDonHang)
);
CREATE TABLE PhanHoi (
    MaPhanHoi INT IDENTITY(1,1) PRIMARY KEY,
    TenKhachHang NVARCHAR(255) NOT NULL,
	SoDienThoai VARCHAR(20),
    Email NVARCHAR(100),
    NoiDung NVARCHAR(MAX) NOT NULL,
    ThoiGian DATETIME NOT NULL,
	GioiTinh NVARCHAR(10),
	MaKhachHang INT,
	CONSTRAINT FK_PhanHoi_KhachHang FOREIGN KEY (MaKhachHang) REFERENCES KhachHang(MaKhachHang)
);

INSERT INTO LoaiSanPham (TenLoai)
VALUES
    (N'Hoa cảnh'),
    (N'Hoa quà tặng'),
    (N'Hoa sự kiện'),
    (N'Hoa cưới'),
    (N'Hoa chúc mừng');




INSERT INTO Hoa (TenSanPham, MoTa, Gia, DonVi, SoLuong, HanSuDung, MaLoai, HinhAnh)
VALUES
  (N'Hoa baby màu sắc', N'Hoa baby màu sắc tươi tắn', 55.00, N'Bó', 10, '2023-12-31', 4, '1.png'),
  (N'Hoa cúc trắng', N'Hoa cúc trắng đơn giản', 20.00, N'Bó', 18, '2023-12-31', 3, '2.png'),
  (N'Hoa hồng vàng', N'Hoa hồng vàng nổi bật', 380.00, N'Bó', 28, '2023-12-31', 1, '3.png'),
  (N'Hoa violet', N'Hoa violet tím mềm mại', 65.00, N'Bó', 20, '2023-12-31', 4, '4.png'),
  (N'Hoa cẩm chướng', N'Hoa cẩm chướng tươi tắn', 45.00, N'Bó', 15, '2023-12-31', 5, '5.png'),
  (N'Hoa Hồng Trái Tim', N'Hoa hồng đỏ đẹp', 400.00, N'Bó', 20, '2023-12-31', 1, '6.png'),
  (N'Hoa lan trắng', N'Hoa lan trắng tinh khôi', 30.00, N'Bó', 15, '2023-12-31', 2, '7.png'),
  (N'Hoa tulip vàng', N'Hoa tulip vàng nổi bật', 290.00, N'Bó', 25, '2023-12-31', 3, '8.png'),
  (N'Hoa hồng kết hợp', N'Hoa hồng kết hợp tươi đẹp', 450.00, N'Bó', 30, '2023-12-31', 2, '9.png'),
  (N'Hoa cẩm tú cầu', N'Hoa cẩm tú cầu đa dạng sắc màu', 27.25, N'Bó', 22, '2023-12-31', 5, '10.png'),
  (N'Hoa dại màu tự nhiên', N'Hoa dại màu tự nhiên và thơm ngon', 45.00, N'Bó', 3, '2023-12-31', 3, '11.png'),
  (N'Hoa hồng đỏ', N'Hoa hồng đỏ đẹp và quyến rũ', 65.00, N'Bó', 12, '2023-12-31', 5, '12.png'),
  (N'Hoa cẩm chướng hồng', N'Hoa cẩm chướng hồng đa dạng sắc màu', 127.25, N'Bó', 64, '2023-12-31', 2, '13.png'),
  (N'Hoa bay và 1 bông hồng', N'Hoa bay và 1 bông hồng đỏ ở giữa', 43.5, N'Bó', 54, '2023-12-31', 3, '14.png'),
  (N'Hoa hướng dương điểm hoa baby', N'Hoa hướng dương điểm hoa baby nổi bật', 273.2, N'Bó', 65, '2023-12-31', 4, '15.png'),
  (N'Hoa tone trắng hồng', N'Hoa tone trắng hồng đáng yêu', 34.5, N'Bó', 67, '2023-12-31', 5, '16.png'),
  (N'Hoa tông trắng xanh', N'Hoa tông trắng xanh thân thiện', 54.45, N'Bó', 54, '2023-12-31', 1, '17.png'),
  (N'Hoa tình yêu', N'Hoa tình yêu giản đơn', 77.65, N'Bó', 75, '2023-12-31', 3, '18.png'),
  (N'Hoa giáng sinh', N'Hoa giáng sinh đa dạng sắc màu', 265.87, N'Giỏ', 43, '2023-12-31', 4, '19.png'),
  (N'Giỏ hoa lan', N'Giỏ hoa mẫu đơn được xem là loài hoa của hạnh phúc gia đình', 546.2, N'Giỏ', 76, '2023-12-31', 2, '20.png'),
  (N'Hoa ươm nắng', N'Hoa ươm nắng tinh khôi', 255.98, N'Giỏ', 23, '2023-12-31', 3, '21.png'),
  (N'Hoa mẫu đơn', N'Hoa mẫu đơn đa dạng', 437.15, N'Giỏ', 75, '2023-12-31', 5, '22.png'),
  (N'Giỏ hoa mẫu đơn', N'Giỏ hoa mẫu đơn tự nhiên', 50.84, N'Giỏ', 95, '2023-12-31', 2, '23.png'),
  (N'Giỏ hoa tông màu tím', N'Giỏ hoa tông màu tím quyến rũ', 54.5, N'Giỏ', 25, '2023-12-31', 4, '24.png'),
  (N'Giỏ hoa sinh nhật', N'Giỏ hoa sinh nhật', 432.9, N'Giỏ', 87, '2023-12-31', 5, '25.png'),
  (N'Hoa giáng sinh', N'Hoa giáng sinh We Wish You a Merry Christmas', 267.55, N'Giỏ', 42, '2023-12-31', 4, '26.png');

  

INSERT INTO QuanTriVien (TenQTV, TaiKhoanQTV, MatKhauQTV, EmailQTV) 
VALUES 
		(N'Nguyen Van A', 'admmin1', 'matKhau1', 'nguyenvana@gmail.com'),
		(N'Tran Thi B', 'admin2', 'matKhau2', 'tranthib@yahoo.com'),
		(N'Le Van C', 'admin3', 'matKhau3', 'levanc@hotmail.com'),
		(N'Pham Thi D', 'admin4', 'matKhau4', 'phamthid@gmail.com'),
		(N'Hoang Van E', 'admin5', 'matKhau5', 'hoangvane@gmail.com');


INSERT INTO KhachHang (TenKhachHang, DiaChi, SoDienThoai, Email, NgaySinh, TaiKhoan, MatKhau, GioiTinh)
VALUES
    ('Khach Hang 1', 'Dia Chi 1', '0123456789', 'khachhang1@email.com', '2003-09-10', 'taiKhoan1', 'matKhau1',N'Nam'),
    ('Khach Hang 2', 'Dia Chi 2', '0987654321', 'khachhang2@email.com', '2003-09-11', 'taiKhoan2', 'matKhau2',N'Nữ'),
    ('Khach Hang 3', 'Dia Chi 3', '0369852147', 'khachhang3@email.com', '2003-09-12', 'taiKhoan3', 'matKhau3',N'Nam'),
    ('Khach Hang 4', 'Dia Chi 4', '0123456780', 'khachhang4@email.com', '2003-09-13', 'taiKhoan4', 'matKhau4',N'Nam'),
    ('Khach Hang 5', 'Dia Chi 5', '0987654322', 'khachhang5@email.com', '2003-09-14', 'taiKhoan5', 'matKhau5',N'Nữ'),
    ('Khach Hang 6', 'Dia Chi 6', '0369852148', 'khachhang6@email.com', '2003-09-15', 'taiKhoan6', 'matKhau6',N'Nữ'),
    ('Khach Hang 7', 'Dia Chi 7', '0123456781', 'khachhang7@email.com', '2003-09-16', 'taiKhoan7', 'matKhau7',N'Nam'),
    ('Khach Hang 8', 'Dia Chi 8', '0987654323', 'khachhang8@email.com', '2003-09-17', 'taiKhoan8', 'matKhau8',N'Nữ'),
    ('Khach Hang 9', 'Dia Chi 9', '0369852149', 'khachhang9@email.com', '2003-09-18', 'taiKhoan9', 'matKhau9',N'Nam'),
    ('Khach Hang 10', 'Dia Chi 10', '0123456782', 'khachhang10@email.com', '2003-09-19', 'taiKhoan10', 'matKhau10',N'Nam');



INSERT INTO DonHang (MaKhachHang, NgayDatHang, NgayGiaoHang, DaThanhToan, TinhTrangGiaoHang)
VALUES
    (1, '2023-09-10', '2023-09-15', N'Đã Thanh Toán', 1),
    (2, '2023-09-11', NULL, N'chưa Thanh Toán', 0),
    (3, '2023-09-12', '2023-09-17', N'Đã Thanh Toán', 1),
    (4, '2023-09-13', NULL, N'chưa Thanh Toán', 0),
    (5, '2023-09-14', '2023-09-19', N'Đã Thanh Toán', 1),
    (6, '2023-09-15', NULL, N'chưa Thanh Toán', 0),
    (7, '2023-09-16', '2023-09-21', N'Đã Thanh Toán', 1),
    (8, '2023-09-17', NULL, N'chưa Thanh Toán', 0),
    (9, '2023-09-18', '2023-09-23', N'Đã Thanh Toán', 1),
    (10, '2023-09-19', NULL, N'chưa Thanh Toán', 0);



INSERT INTO ChiTietDonHang (MaDonHang, TenSanPham, MaSanPham, HinhAnh, SoLuong, GiaBan)
VALUES
    (10, N'Hoa baby màu sắc', 1, '1.png', 5, 28.75),
    (10, N'Hoa cúc trắng', 2,'2.png', 3, 330.00),
    (2, N'Hoa hồng đỏ', 12,'12.png', 2, 21.50),
    (2, N'Hoa dại màu tự nhiên', 11,'11.png', 4, 243.00),
    (3, N'Hoa tulip vàng', 8,'8.png', 2, 27.25),
    (3, N'Hoa lan trắng', 7,'7.png', 3, 28.75), 
    (4, N'Hoa dại màu tự nhiên', 11,'11.png', 2, 400.00),
    (4, N'Hoa Hồng Trái Tim', 6,'6.png', 3, 30.00),
    (5, N'Hoa hồng đỏ', 12,'12.png', 1, 290.00),
    (5, N'Hoa dại màu tự nhiên', 11,'11.png', 4, 450.00),
    (6, N'Hoa baby màu sắc', 1,'1.png', 2, 27.25),
    (7, N'Hoa Hồng Trái Tim', 6,'6.png', 4, 330.00),  
    (8, N'Hoa dại màu tự nhiên', 11,'11.png', 3, 21.50);  



INSERT INTO KhoHang (MaSanPham, SoLuongTrongKho)
VALUES
    (1, 100),
    (2, 150),
    (3, 200),
    (4, 180),
    (5, 250),
    (6, 220),
    (7, 180),
    (8, 210),
    (9, 150),
    (10, 190);

INSERT INTO NhaCungCap (TenNhaCungCap, HinhAnh, DiaChi, SoDienThoai, Email, MaSanPham)
VALUES
    (N'Nhà Cung Cấp 1', 'NCC001.jpg', N'Địa chỉ 1', '0123456789', 'nhacungcap1@email.com', 1),
    (N'Nhà Cung Cấp 2', 'NCC002.jpg', N'Địa chỉ 2', '0987654321', 'nhacungcap2@email.com', 2),
    (N'Nhà Cung Cấp 3', 'NCC003.jpg', N'Địa chỉ 3', '0369852147', 'nhacungcap3@email.com', 3),
    (N'Nhà Cung Cấp 4', 'NCC004.jpg', N'Địa chỉ 4', '0123456780', 'nhacungcap4@email.com', 4),
    (N'Nhà Cung Cấp 5', 'NCC005.jpg', N'Địa chỉ 5', '0987654322', 'nhacungcap5@email.com', 5);

INSERT INTO PhieuNhapHang (MaNhaCungCap, NgayNhap, TongTien, GhiChu)
VALUES
    (1, '2023-09-10', 750.00, N'Nhập hàng từ nhà cung cấp 1'),
    (2, '2023-09-11', 620.50, N'Nhập hàng từ nhà cung cấp 2');

INSERT INTO LichSuGiaoDich (MaDonHang, TrangThai, NgayCapNhat)
VALUES
    (1, N'Đang xử lý', '2023-09-15'),
    (2, N'Đã thanh toán', '2023-09-12');

INSERT INTO PhanHoi (TenKhachHang, SoDienThoai, Email, NoiDung, ThoiGian, GioiTinh, MaKhachHang)
VALUES 
	(N'Nguyen Van A', '0987654321', 'nguyenvana@gmail.com', N'Rất hài lòng về dịch vụ của bạn', '2021-01-01 10:00:00', N'Nam', 1),
	(N'Tran Thi B', '0909090909', 'tranthib@gmail.com', N'Cần cải thiện chất lượng sản phẩm', '2021-02-15 14:30:00', N'Nữ', 2),
	(N'Le Van C', '0977777777', 'levanc@gmail.com', N'Góp ý về dịch vụ giao hàng', '2021-03-20 09:45:00', N'Nam', 3),
	(N'Pham Thi D', '0912345678', 'phamthid@gmail.com', N'Yêu cầu hỗ trợ kỹ thuật', '2021-04-10 16:20:00', N'Nữ', 4);

DBCC CHECKIDENT ('LoaiSanPham', RESEED, 1)
DBCC CHECKIDENT ('Hoa', RESEED, 1)
DBCC CHECKIDENT ('KhachHang', RESEED, 1)
DBCC CHECKIDENT ('DonHang', RESEED, 1)
DBCC CHECKIDENT ('ChiTietDonHang', RESEED, 1)
DBCC CHECKIDENT ('NhaCungCap', RESEED, 1)
DBCC CHECKIDENT ('PhieuNhapHang', RESEED, 1)
DBCC CHECKIDENT ('LichSuGiaoDich', RESEED, 1)
delete dbo.LoaiSanPham
delete dbo.KhachHang
delete dbo.DonHang
delete dbo.NhaCungCap
delete dbo.PhieuNhapHang
delete dbo.hoa
	
DELETE FROM PhanHoi;