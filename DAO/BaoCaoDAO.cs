using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;

namespace DAO
{
    public class BaoCaoDAO
    {
        public static DataTable BaoCaoDoanhThu(DateTime startDate, DateTime endDate)
        {
            string startDateString = startDate.ToString("yyyy-MM-dd");
            string endDateString = endDate.ToString("yyyy-MM-dd");

            string query = $@"
        SELECT MaHD, MaHopDong, FORMAT(NgayTao, 'dd/MM/yyyy') as NgayTao, FORMAT(NgayTT, 'dd/MM/yyyy') AS NgayThanhToan, TongTien
        FROM HoaDon
        WHERE NgayTT >= '{startDateString}' AND NgayTT <= '{endDateString}' AND NgayTT IS NOT NULL
    ";

            DataTable data = DataProvider.ExecuteQuery(query);
            return data;
        }






        public static DataTable BaoCaoTongHopCanHo()
    {
        // Truy vấn lấy thông tin tổng hợp căn hộ
        string query = "SELECT CH.MaCH, CH.SoTang, SUM(HD.TongTien) AS TongDoanhThu " +
                       "FROM CanHo CH " +
                       "LEFT JOIN HopDong HD ON CH.MaCH = HD.MaCH " +
                       "GROUP BY CH.MaCH, CH.SoTang";

        DataTable data = DataProvider.ExecuteQuery(query);
        return data;
    }
        public static DataTable BaoCaoTongHopKhachThue()
        {
         
            string query = "SELECT KH.MaKH, KH.TenKH, COUNT(HD.MaHD) AS SoHopDong " +
                           "FROM KhachHang KH " +
                           "LEFT JOIN HopDong HD ON KH.MaKH = HD.MaKH " +
                           "GROUP BY KH.MaKH, KH.TenKH";

            DataTable data = DataProvider.ExecuteQuery(query);
            return data;
        }

        public static DataTable BaoCaoHopDongSapHetHan()
        {
            string query = @"
        SELECT 
            HD.MaHD AS MaHopDong,
            KH.TenKH AS TenKhachHang,
            NV.TenNV AS TenNhanVien,
            HD.MaCH AS MaCanHo,
            FORMAT(HD.NgayThue, 'dd/MM/yyyy') AS NgayThue,
            FORMAT(HD.NgayTra, 'dd/MM/yyyy') AS NgayTra,
            HD.TongTien AS TongTien,
            DATEDIFF(day, GETDATE(), HD.NgayTra) AS SoNgayConLai
        FROM HopDong HD
        JOIN KhachHang KH ON HD.MaKH = KH.MaKH
        JOIN NhanVien NV ON HD.MaNV = NV.MaNV
        WHERE HD.NgayTra >= GETDATE() AND HD.NgayTra <= DATEADD(day, 30, GETDATE())";

            DataTable data = DataProvider.ExecuteQuery(query);

            return data;
        }
        public static DataTable BaoCaoThanhToanCham()
        {
            string query = @"
        SELECT 
            HD.MaHD,
            FORMAT(HD.NgayTao, 'dd/MM/yyyy') as NgayTT,
            HD.MaHopDong AS MaHopDong,
            NV.TenNV,
            DATEDIFF(day, ISNULL(HD.NgayTT, HD.NgayTao), GETDATE()) AS SoNgayQuaHan
        FROM HoaDon HD
        JOIN HopDong ON HopDong.MaHD = HD.MaHD
        JOIN NhanVien NV ON HD.MaNV = NV.MaNV
        WHERE DATEDIFF(day, ISNULL(HD.NgayTT, HD.NgayTao), GETDATE()) > 5";

            DataTable data = DataProvider.ExecuteQuery(query);

            return data;
        }


        public static DataTable BaoCaoTienCoc(DateTime startDate, DateTime endDate)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("MaHD", typeof(string));
            resultTable.Columns.Add("MaKH", typeof(string));
            resultTable.Columns.Add("TenKhachHang", typeof(string));
            resultTable.Columns.Add("NgayTao", typeof(DateTime));
            resultTable.Columns.Add("TienCoc", typeof(decimal));

            string query = $@"
        SELECT
            hd.MaHD,
            hd.MaKH,
            kh.TenKH as TenKhachHang,
            FORMAT(hd.NgayTao, 'dd/MM/yyyy') AS NgayTao,
            hd.TienCoc
        FROM
            HopDong hd
        JOIN
            KhachHang kh ON hd.MaKH = kh.MaKH
        WHERE
            hd.NgayTao BETWEEN '{startDate}' AND '{endDate}'
            AND hd.TrangThai <> N'Đã thanh lý'";

            // Parameters for the query
            object[] parameters = {startDate, endDate };

            // Execute the query using DataProvider
            resultTable = DataProvider.ExecuteQuery(query, parameters);

            return resultTable;
        }


    }
}

