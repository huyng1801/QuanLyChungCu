create database QuanLyChungCu
go
use QuanLyChungCu
go
CREATE TABLE CanHo (
    MaCH char(6) PRIMARY KEY,
    SoTang nvarchar(20) not null,
    DienTich decimal(10,2) not null,
    PhongNgu int not null,
    NoiThat nvarchar(100),
    TinhTrang nvarchar(50) check (TinhTrang in (N'Sẵn sàng', N'Không sẵn sàng')),
    GiaThue decimal(12,2) not null
);
go
CREATE TABLE KhachHang (
    MaKH char(6) PRIMARY KEY,
    TenKH nvarchar(50) not null,
    CCCD char(12) not null,
    NgaySinh date,
    DiaChi nvarchar(100),
    Sdt char(10),
    Email varchar(50)
);
go
CREATE TABLE NhanVien (
    MaNV char(6) PRIMARY KEY,
	MatKhau varchar(100) not null,
    TenNV nvarchar(50) not null,
    Ngaysinh date,
    Diachi nvarchar(100),
    Sdt char(10)
);
go
CREATE TABLE HopDong (
    MaHD char(6) PRIMARY KEY,
    MaKH char(6) foreign key references KhachHang(MaKH) on delete cascade,
    MaNV char(6) foreign key references NhanVien(MaNV) on delete cascade,
	MaCH char(6) foreign key references CanHo(MaCH) on delete cascade,
    NgayTao date default getdate(),
	NgayThue date not null,
	NgayTra date,
    TienCoc decimal(12,2) default 0,
    TongTien decimal(12,2) default 0,
    Trangthai nvarchar(50)
);
go
CREATE TABLE DichVu (
    MaDV char(6) PRIMARY KEY,
    TenDV nvarchar(50) not null,
    GiaTien decimal(12,2) not null
);
go
CREATE TABLE HoaDon (
    MaHD char(6) PRIMARY KEY,
    MaNV char(6) foreign key references NhanVien(MaNV) on delete cascade,
    MaHopDong char(6) foreign key references HopDong(MaHD) on delete no action,
    NgayTao date default getdate(),
    NgayTT date,
    TongTien decimal(12,2) default 0
);
go

CREATE TABLE ChiTietHoaDon (
	MaChiTietHoaDon int identity(1,1) primary key,
    MaHD char(6) foreign key references HoaDon(MaHD) on delete cascade,
    MaDV char(6) foreign key references DichVu(MaDV) on delete cascade,
    HeSo decimal(12,2) not null
);
go

CREATE TABLE SuCo (
    MaSC char(6) PRIMARY KEY,
    TenSC nvarchar(50) not null,
    NguyenNhan nvarchar(100) not null,
    MucPhat decimal(12,2) not null
);
go
CREATE TABLE BienBanBoiThuong (
    MaBB char(6) PRIMARY KEY,
    MaHD char(6) foreign key references HopDong(MaHD) on delete cascade,
    MaNV char(6) foreign key references NhanVien(MaNV) on delete no action,
    NgayTao date default getdate(),
	TongTien decimal(12,2) not null default 0
);
go

CREATE TABLE ChiTietBienBanBoiThuong (
	Id int identity(1,1) primary key,
    MaBB char(6) foreign key references BienBanBoiThuong(MaBB) on delete cascade,
    MaSC char(6) foreign key references SuCo(MaSC) on delete cascade,
    Soluong int not null
);
go
create TRIGGER Trigger_InsertHoaDon
ON HoaDon
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Update TongTien based on the sum of TongTien from related HopDong records
    UPDATE h
    SET TongTien = h.TongTien + ISNULL((SELECT SUM(TongTien) 
                                       FROM HopDong hd
                                       WHERE hd.MaHD = h.MaHopDong), 0)
    FROM HoaDon h
    INNER JOIN inserted i ON h.MaHD = i.MaHD;
END;

GO
create TRIGGER Trigger_TinhTongTienHoaDon
ON ChiTietHoaDon
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tính toán tổng tiền dựa trên tổng tiền hợp đồng và chi tiết hóa đơn
    UPDATE HoaDon
    SET TongTien = ISNULL(
        (SELECT SUM(ISNULL(i.HeSo, 0) * ISNULL(dv.GiaTien, 0))
         FROM ChiTietHoaDon i
         INNER JOIN DichVu dv ON i.MaDV = dv.MaDV
         WHERE i.MaHD = HoaDon.MaHD), 0)
    WHERE MaHD IN (SELECT DISTINCT MaHD FROM inserted)
    OR MaHD IN (SELECT DISTINCT MaHD FROM deleted);

    -- Cập nhật tổng tiền trong bảng HoaDon bằng tổng tiền của hợp đồng
    UPDATE HoaDon
    SET TongTien = ISNULL(TongTien, 0) + ISNULL(
        (SELECT SUM(ISNULL(HopDong.TongTien, 0))
         FROM HopDong
         WHERE HopDong.MaHD = HoaDon.MaHopDong), 0)
    WHERE MaHD IN (SELECT DISTINCT MaHD FROM inserted)
    OR MaHD IN (SELECT DISTINCT MaHD FROM deleted);
END;
GO

create TRIGGER Trigger_CapNhatTongTienBienBan
ON ChiTietBienBanBoiThuong
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Tính toán tổng tiền cho tất cả biên bản bồi thường liên quan
    UPDATE BienBanBoiThuong
    SET TongTien = ISNULL(
        (SELECT ISNULL(SUM(cb.Soluong * cc.MucPhat), 0)
         FROM ChiTietBienBanBoiThuong cb
         INNER JOIN SuCo cc ON cb.MaSC = cc.MaSC
         WHERE cb.MaBB = BienBanBoiThuong.MaBB), 0)
    WHERE MaBB IN (SELECT DISTINCT MaBB FROM inserted);
END;
GO

CREATE TRIGGER Trigger_CapNhatTongTienHopDong
ON HopDong
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE h
    SET h.TongTien = c.GiaThue
    FROM HopDong h
    INNER JOIN CanHo c ON h.MaCH = c.MaCH
    WHERE h.MaHD IN (SELECT DISTINCT i.MaHD FROM inserted i);
END;
GO
INSERT INTO CanHo (MaCH, SoTang, DienTich, PhongNgu, NoiThat, TinhTrang, GiaThue)
VALUES
    ('CH0001', N'Tầng 1', 75.5, 2, N'Đầy đủ nội thất', N'Sẵn sàng', 25000000.00),
    ('CH0002', N'Tầng 2', 68.0, 2, N'Cơ bản', N'Không sẵn sàng', 21000000.00),
    ('CH0003', N'Tầng 3', 80.0, 3, N'Thiết kế hiện đại', N'Sẵn sàng', 30000000.00),
    ('CH0004', N'Tầng 1', 95.5, 4, N'Nội thất cao cấp', N'Không sẵn sàng', 35000000.00),
    ('CH0005', N'Tầng 2', 60.0, 2, N'Cơ bản', N'Sẵn sàng', 20000000.00),
    ('CH0006', N'Tầng 3', 85.0, 3, N'Thiết kế hiện đại', N'Không sẵn sàng', 28000000.00),
    ('CH0007', N'Tầng 4', 70.5, 2, N'Nội thất đầy đủ', N'Sẵn sàng', 24000000.00),
    ('CH0008', N'Tầng 4', 78.0, 2, N'Cơ bản', N'Không sẵn sàng', 26000000.00),
    ('CH0009', N'Tầng trệt', 92.0, 3, N'Thiết kế cao cấp', N'Sẵn sàng', 32000000.00),
    ('CH0010', N'Tầng trệt', 88.5, 3, N'Nội thất đầy đủ', N'Không sẵn sàng', 31000000.00),
    ('CH0011', N'Tầng 1', 63.5, 2, N'Cơ bản', N'Sẵn sàng', 21000000.00),
    ('CH0012', N'Tầng 2', 72.0, 2, N'Thiết kế hiện đại', N'Không sẵn sàng', 27000000.00),
    ('CH0013', N'Tầng 3', 68.5, 2, N'Đầy đủ nội thất', N'Sẵn sàng', 23000000.00),
    ('CH0014', N'Tầng 4', 90.0, 3, N'Cơ bản', N'Không sẵn sàng', 29000000.00),
    ('CH0015', N'Tầng 4', 82.5, 3, N'Thiết kế cao cấp', N'Sẵn sàng', 33000000.00),
    ('CH0016', N'Tầng 5', 77.0, 2, N'Đầy đủ nội thất', N'Không sẵn sàng', 27000000.00),
    ('CH0017', N'Tầng 1', 64.0, 2, N'Cơ bản', N'Sẵn sàng', 22000000.00),
    ('CH0018', N'Tầng 2', 100.0, 4, N'Thiết kế hiện đại', N'Không sẵn sàng', 36000000.00),
    ('CH0019', N'Tầng 1', 73.5, 2, N'Đầy đủ nội thất', N'Sẵn sàng', 25000000.00),
    ('CH0020', N'Tầng 2', 96.0, 4, N'Cơ bản', N'Không sẵn sàng', 33000000.00);
go
INSERT INTO KhachHang (MaKH, TenKH, CCCD, NgaySinh, DiaChi, Sdt, Email)
VALUES
    ('KH0001', N'Nguyễn Văn A', '123456789012', '1985-05-15', N'Hà Nội', '0123456789', 'nguyenvana@email.com'),
    ('KH0002', N'Nguyễn Thị B', '987654321098', '1980-10-20', N'Hồ Chí Minh', '0987654321', 'nguyenthib@email.com'),
    ('KH0003', N'Lê Văn Cường', '234567890123', '1992-03-12', N'Đà Nẵng', '0367890123', 'levancuong@email.com'),
    ('KH0004', N'Trần Thanh D', '876543210987', '1990-11-05', N'Nghệ An', '0123456789', 'tranthanhd@email.com'),
    ('KH0005', N'Phạm Thị Ngọc', '345678901234', '1987-08-24', N'Thái Bình', '0987654321', 'phamthi@email.com'),
    ('KH0006', N'Lê Minh Tâm', '765432109876', '1995-01-07', N'Cần Thơ', '0367890123', 'leminhtam@email.com'),
    ('KH0007', N'Trương Văn Hòa', '456789012345', '1986-04-18', N'Bắc Ninh', '0123456789', 'truongvanhoa@email.com'),
    ('KH0008', N'Nguyễn Thị Thu', '654321098765', '1991-02-28', N'Quảng Ninh', '0987654321', 'nguyenthi@email.com'),
    ('KH0009', N'Hoàng Văn Đức', '123456789012', '1984-09-09', N'Hải Phòng', '0367890123', 'hoangvanduc@email.com'),
    ('KH0010', N'Nguyễn Văn Nam', '987654321098', '1993-12-16', N'Lào Cai', '0123456789', 'nguyenvannam@email.com'),
    ('KH0011', N'Trần Thị Hằng', '234567890123', '1990-06-21', N'Nam Định', '0987654321', 'tranthi@email.com'),
    ('KH0012', N'Lê Thị Thu Hà', '876543210987', '1992-03-30', N'Hải Dương', '0367890123', 'lethi@email.com'),
    ('KH0013', N'Phạm Văn Hoàng', '345678901234', '1988-10-07', N'Thanh Hóa', '0123456789', 'phamvanhoang@email.com'),
    ('KH0014', N'Hoàng Thị Mai', '765432109876', '1991-07-14', N'Quảng Bình', '0987654321', 'hoangthi@email.com'),
    ('KH0015', N'Vũ Văn Hòa', '456789012345', '1994-02-10', N'Bình Dương', '0123456789', 'vuvanhoa@email.com'),
    ('KH0016', N'Nguyễn Thị Lan', '654321098765', '1989-09-25', N'Bắc Giang', '0367890123', 'nguyenthilan@email.com'),
    ('KH0017', N'Đặng Thanh Tùng', '123456789012', '1993-11-08', N'Quảng Nam', '0123456789', 'dangthanhtung@email.com'),
    ('KH0018', N'Trần Văn Đức', '987654321098', '1987-06-12', N'Ninh Bình', '0987654321', 'tranvanduc@email.com'),
    ('KH0019', N'Nguyễn Văn Thanh', '234567890123', '1983-03-05', N'Hà Nam', '0367890123', 'nguyenvanthanh@email.com'),
    ('KH0020', N'Phạm Thị Lan Anh', '876543210987', '1995-07-20', N'Lâm Đồng', '0987654321', 'phamthilananh@email.com');
go

INSERT INTO NhanVien (MaNV, MatKhau, TenNV, Ngaysinh, Diachi, Sdt)
VALUES
    ('NV0001', '123', N'Lê Thị Dương', '1985-05-15', N'Hà Nội', '0123456789'),
    ('NV0002', '123', N'Phạm Văn Hiếu', '1980-10-20', N'Hồ Chí Minh', '0987654321'),
    ('NV0003', '123', N'Lê Tuấn', '1992-03-12', N'Đà Nẵng', '0367890123'),
    ('NV0004', '123', N'Trần Thị Ái', '1990-11-05', N'Nghệ An', '0123456789'),
    ('NV0005', '123', N'Phạm Văn Sơn', '1987-08-24', N'Thái Bình', '0987654321'),
    ('NV0006', '123', N'Lê Thị Thanh', '1995-01-07', N'Cần Thơ', '0367890123'),
    ('NV0007', '123', N'Nguyễn Văn Nam', '1986-04-18', N'Bắc Ninh', '0123456789'),
    ('NV0008', '123', N'Trương Văn Hà', '1991-02-28', N'Quảng Ninh', '0987654321'),
    ('NV0009', '123', N'Hoàng Văn Đức', '1984-09-09', N'Hải Phòng', '0367890123'),
    ('NV0010', '123', N'Nguyễn Văn Nam', '1993-12-16', N'Lào Cai', '0123456789'),
    ('NV0011', '123', N'Trần Thị Hằng', '1990-06-21', N'Nam Định', '0987654321'),
    ('NV0012', '123', N'Lê Thị Thu Hà', '1992-03-30', N'Hải Dương', '0367890123'),
    ('NV0013', '123', N'Phạm Văn Hoàng', '1988-10-07', N'Thanh Hóa', '0123456789'),
    ('NV0014', '123', N'Hoàng Thị Mai', '1991-07-14', N'Quảng Bình', '0987654321'),
    ('NV0015', '123', N'Vũ Văn Hòa', '1994-02-10', N'Bình Dương', '0123456789'),
    ('NV0016', '123', N'Nguyễn Thị Lan', '1989-09-25', N'Bắc Giang', '0367890123'),
    ('NV0017', '123', N'Đặng Thanh Tùng', '1993-11-08', N'Quảng Nam', '0123456789'),
    ('NV0018', '123', N'Trần Văn Đức', '1987-06-12', N'Ninh Bình', '0987654321'),
    ('NV0019', '123', N'Nguyễn Văn Thanh', '1983-03-05', N'Hà Nam', '0367890123'),
    ('NV0020', '123', N'Phạm Thị Lan Anh', '1995-07-20', N'Lâm Đồng', '0987654321');
go

INSERT INTO HopDong (MaHD, MaKH, MaNV, MaCH, NgayThue, NgayTra, TienCoc, Trangthai)
VALUES
    ('HD0001', 'KH0001', 'NV0001', 'CH0001', '2023-11-01', '2023-11-21', 5000000.00, N'Đã ký'),
    ('HD0002', 'KH0002', 'NV0002', 'CH0002', '2023-11-01', '2023-12-01', 6000000.00, N'Đã ký'),
    ('HD0003', 'KH0003', 'NV0003', 'CH0003', '2023-11-01', '2023-12-02', 4500000.00, N'Đã ký'),
    ('HD0004', 'KH0004', 'NV0004', 'CH0004', '2023-11-01', '2023-12-03', 5500000.00, N'Đã ký'),
    ('HD0005', 'KH0005', 'NV0005', 'CH0005', '2023-11-01', '2023-12-05', 7500000.00, N'Đã ký'),
    ('HD0006', 'KH0006', 'NV0006', 'CH0006', '2023-11-01', '2023-11-25', 7000000.00, N'Đã ký'),
    ('HD0007', 'KH0007', 'NV0007', 'CH0007', '2023-11-01', '2023-11-18', 5800000.00, N'Đã ký'),
    ('HD0008', 'KH0008', 'NV0008', 'CH0008', '2023-11-01', '2023-11-30', 6000000.00, N'Đã ký'),
    ('HD0009', 'KH0009', 'NV0009', 'CH0009', '2023-11-01', '2023-11-10', 8000000.00, N'Đã ký'),
    ('HD0010', 'KH0010', 'NV0010', 'CH0010', '2023-11-01', '2023-11-15', 7000000.00, N'Đã ký'),
    ('HD0011', 'KH0011', 'NV0011', 'CH0011', '2023-11-01', '2023-12-01', 4500000.00, N'Đã ký'),
    ('HD0012', 'KH0012', 'NV0012', 'CH0012', '2023-11-01', '2023-12-05', 6000000.00, N'Đã ký'),
    ('HD0013', 'KH0013', 'NV0013', 'CH0013', '2023-11-01', '2023-12-05', 5500000.00, N'Đã ký'),
    ('HD0014', 'KH0014', 'NV0014', 'CH0014', '2023-11-01', '2023-11-10', 6500000.00, N'Đã ký'),
    ('HD0015', 'KH0015', 'NV0015', 'CH0015', '2023-11-01', '2023-11-20', 8000000.00, N'Đã ký'),
    ('HD0016', 'KH0016', 'NV0016', 'CH0016', '2023-11-01', '2023-11-25', 7000000.00, N'Đã ký'),
    ('HD0017', 'KH0017', 'NV0017', 'CH0017', '2023-11-01', '2023-11-30', 7500000.00, N'Đã ký'),
    ('HD0018', 'KH0018', 'NV0018', 'CH0018', '2023-11-01', '2023-12-05', 5000000.00, N'Đã ký'),
    ('HD0019', 'KH0019', 'NV0019', 'CH0019', '2023-11-01', '2023-11-15', 8500000.00, N'Đã ký'),
    ('HD0020', 'KH0020', 'NV0020', 'CH0020', '2023-11-01', '2023-11-20', 6500000.00, N'Đã ký');
go

INSERT INTO DichVu (MaDV, TenDV, GiaTien)
VALUES
    ('DV0001', N'Dịch vụ dọn dẹp', 500000.00),
    ('DV0002', N'Dịch vụ bảo vệ 24/7', 1000000.00),
    ('DV0003', N'Dịch vụ giặt ủi', 700000.00),
    ('DV0004', N'Dịch vụ Internet', 300000.00),
    ('DV0005', N'Dịch vụ tiệc cưới', 3000000.00),
    ('DV0006', N'Dịch vụ cung cấp điện', 800000.00),
    ('DV0007', N'Dịch vụ thăng máy', 400000.00),
    ('DV0008', N'Dịch vụ hồ bơi', 1500000.00),
    ('DV0009', N'Dịch vụ cung cấp nước', 600000.00),
    ('DV0010', N'Dịch vụ nhà hàng', 1200000.00),
    ('DV0011', N'Dịch vụ sân thể thao', 500000.00),
    ('DV0012', N'Dịch vụ đưa đón', 900000.00),
    ('DV0013', N'Dịch vụ truyền hình cáp', 400000.00),
    ('DV0014', N'Dịch vụ học tập', 700000.00),
    ('DV0015', N'Dịch vụ yoga', 300000.00),
    ('DV0016', N'Dịch vụ trà sữa', 80000.00),
    ('DV0017', N'Dịch vụ phòng gym', 400000.00),
    ('DV0018', N'Dịch vụ xe đạp điện', 100000.00),
    ('DV0019', N'Dịch vụ xe đạp', 80000.00),
    ('DV0020', N'Dịch vụ xe hơi', 1500000.00);
go
INSERT INTO HoaDon (MaHD, MaNV, MaHopDong, NgayTT)
VALUES
    ('HD0001', 'NV0001', 'HD0001', getdate()),
    ('HD0002', 'NV0002', 'HD0002', getdate()),
    ('HD0003', 'NV0003', 'HD0003', getdate()),
    ('HD0004', 'NV0004', 'HD0004', getdate()),
    ('HD0005', 'NV0005', 'HD0005', getdate()),
    ('HD0006', 'NV0006', 'HD0006', getdate()),
    ('HD0007', 'NV0007', 'HD0007', getdate()),
    ('HD0008', 'NV0008', 'HD0008', getdate()),
    ('HD0009', 'NV0009', 'HD0009', getdate()),
    ('HD0010', 'NV0010', 'HD0010', getdate()),
    ('HD0011', 'NV0011', 'HD0011', getdate()),
    ('HD0012', 'NV0012', 'HD0012', getdate()),
    ('HD0013', 'NV0013', 'HD0013', getdate()),
    ('HD0014', 'NV0014', 'HD0014', getdate()),
    ('HD0015', 'NV0015', 'HD0015', getdate()),
    ('HD0016', 'NV0016', 'HD0016', getdate()),
    ('HD0017', 'NV0017', 'HD0017', getdate()),
    ('HD0018', 'NV0018', 'HD0018', getdate()),
    ('HD0019', 'NV0019', 'HD0019', getdate()),
    ('HD0020', 'NV0020', 'HD0020', getdate());
go

INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0001', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0001', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0001', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0001', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0002', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0002', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0002', 'DV0007', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0002', 'DV0010', 1.3);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0003', 'DV0002', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0003', 'DV0005', 2.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0003', 'DV0008', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0003', 'DV0011', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0004', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0004', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0004', 'DV0012', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0005', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0005', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0005', 'DV0013', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0006', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0006', 'DV0005', 2.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0006', 'DV0008', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0006', 'DV0014', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0007', 'DV0002', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0007', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0007', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0007', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0008', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0008', 'DV0005', 2.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0008', 'DV0008', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0008', 'DV0020', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0009', 'DV0002', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0009', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0009', 'DV0010', 1.3);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0009', 'DV0017', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0010', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0010', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0010', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0010', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0011', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0011', 'DV0005', 2.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0011', 'DV0008', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0011', 'DV0020', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0012', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0012', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0012', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0012', 'DV0017', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0013', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0013', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0013', 'DV0010', 1.3);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0013', 'DV0019', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0014', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0014', 'DV0005', 2.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0014', 'DV0013', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0014', 'DV0018', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0015', 'DV0002', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0015', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0015', 'DV0007', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0015', 'DV0011', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0016', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0016', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0016', 'DV0012', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0017', 'DV0003', 2.0);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0017', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0017', 'DV0013', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0018', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0018', 'DV0014', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0018', 'DV0017', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0018', 'DV0020', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0019', 'DV0002', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0019', 'DV0006', 1.8);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0019', 'DV0009', 1.2);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0019', 'DV0019', 1.6);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0020', 'DV0001', 1.5);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0020', 'DV0004', 1.7);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0020', 'DV0007', 1.4);
INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo)
VALUES ('HD0020', 'DV0012', 1.5);


INSERT INTO SuCo (MaSC, TenSC, NguyenNhan, MucPhat)
VALUES
    ('SC0001', N'Gây mất an ninh', N'Khách hàng gây mất an ninh cho cư dân', 1500000.00),
    ('SC0002', N'Quá ồn ào', N'Khách hàng tạo ra tiếng ồn quá lớn', 1000000.00),
    ('SC0003', N'Thất lừ', N'Khách hàng thất lừ tiện ích chung', 2000000.00),
    ('SC0004', N'Làm hỏng thiết bị', N'Khách hàng gây hỏng thiết bị chung', 800000.00),
    ('SC0005', N'Vi phạm quy định', N'Khách hàng vi phạm quy định của chung cư', 1200000.00),
    ('SC0006', N'Quá khám phá', N'Khách hàng vi phạm an ninh chung cư', 1600000.00),
    ('SC0007', N'Tiêu thụ trái phép', N'Khách hàng tiêu thụ trái phép tài sản chung cư', 1800000.00),
    ('SC0008', N'Cháy nổ', N'Khách hàng gây cháy nổ', 2500000.00),
    ('SC0009', N'Hủy hoại cơ sở', N'Khách hàng hủy hoại cơ sở chung cư', 2000000.00),
    ('SC0010', N'Gian lận', N'Khách hàng gian lận trong giao dịch', 1000000.00),
    ('SC0011', N'Xâm hại', N'Khách hàng xâm hại an toàn chung cư', 2000000.00),
    ('SC0012', N'Trộm cắp', N'Khách hàng trộm cắp tài sản chung cư', 1200000.00),
    ('SC0013', N'Quấy rối', N'Khách hàng gây quấy rối cho cư dân', 800000.00),
    ('SC0014', N'Không trả nợ', N'Khách hàng không thanh toán nợ', 1500000.00),
    ('SC0015', N'Lừa đảo', N'Khách hàng lừa đảo trong giao dịch', 1600000.00),
    ('SC0016', N'Vi phạm hợp đồng', N'Khách hàng vi phạm hợp đồng thuê', 1400000.00),
    ('SC0017', N'Gây xung đột', N'Khách hàng gây xung đột với cư dân khác', 1800000.00),
    ('SC0018', N'Thối nát', N'Khách hàng gây thối nát khu chung cư', 2200000.00),
    ('SC0019', N'Vi phạm luật', N'Khách hàng vi phạm luật pháp liên quan', 2500000.00),
    ('SC0020', N'Tiêu thụ ma túy', N'Khách hàng tiêu thụ ma túy tại chung cư', 3000000.00);
go
INSERT INTO BienBanBoiThuong (MaBB, MaHD, MaNV)
VALUES
    ('BB0001', 'HD0001', 'NV0001'),
    ('BB0002', 'HD0002', 'NV0002'),
    ('BB0003', 'HD0003', 'NV0003'),
    ('BB0004', 'HD0004', 'NV0004'),
    ('BB0005', 'HD0005', 'NV0005'),
    ('BB0006', 'HD0006', 'NV0006'),
    ('BB0007', 'HD0007', 'NV0007'),
    ('BB0008', 'HD0008', 'NV0008'),
    ('BB0009', 'HD0009', 'NV0009'),
    ('BB0010', 'HD0010', 'NV0010'),
    ('BB0011', 'HD0011', 'NV0011'),
    ('BB0012', 'HD0012', 'NV0012'),
    ('BB0013', 'HD0013', 'NV0013'),
    ('BB0014', 'HD0014', 'NV0014'),
    ('BB0015', 'HD0015', 'NV0015'),
    ('BB0016', 'HD0016', 'NV0016'),
    ('BB0017', 'HD0017', 'NV0017'),
    ('BB0018', 'HD0018', 'NV0018'),
    ('BB0019', 'HD0019', 'NV0019'),
    ('BB0020', 'HD0020', 'NV0020');
go
INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0001', 'SC0001', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0001', 'SC0002', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0002', 'SC0003', 7);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0002', 'SC0004', 10);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0003', 'SC0005', 2);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0003', 'SC0001', 6);
INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0004', 'SC0002', 4);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0004', 'SC0003', 8);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0005', 'SC0004', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0005', 'SC0005', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0006', 'SC0001', 7);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0006', 'SC0002', 9);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0007', 'SC0003', 2);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0007', 'SC0004', 6);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0008', 'SC0005', 4);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0008', 'SC0001', 8);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0009', 'SC0002', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0009', 'SC0003', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0010', 'SC0004', 7);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0010', 'SC0005', 4);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0011', 'SC0001', 6);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0011', 'SC0002', 2);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0012', 'SC0003', 9);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0012', 'SC0004', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0013', 'SC0005', 8);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0013', 'SC0001', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0014', 'SC0002', 6);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0014', 'SC0003', 4);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0015', 'SC0004', 7);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0015', 'SC0005', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0016', 'SC0001', 9);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0016', 'SC0002', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0017', 'SC0003', 6);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0017', 'SC0004', 8);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0018', 'SC0005', 2);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0018', 'SC0001', 4);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0019', 'SC0002', 3);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0019', 'SC0003', 7);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0020', 'SC0004', 5);

INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong)
VALUES ('BB0020', 'SC0005', 6);


