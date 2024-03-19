using DTO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class KhachHangDAO
    {
        public static int InsertKhachHang(KhachHangDTO khachHang)
        {
            string query = "INSERT INTO KhachHang (MaKH, TenKH, CCCD, NgaySinh, DiaChi, Sdt, Email) " +
                           "VALUES ( @MaKH , @TenKH , @CCCD , @NgaySinh , @DiaChi , @Sdt , @Email )";

            object[] parameters = 
            {
                khachHang.MaKH,
                khachHang.TenKH,
                khachHang.CCCD,
                khachHang.NgaySinh,
                khachHang.DiaChi    ,
                khachHang.Sdt,
                khachHang.Email
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateKhachHang(KhachHangDTO khachHang)
        {
            string query = "UPDATE KhachHang SET TenKH = @TenKH, CCCD = @CCCD, NgaySinh = @NgaySinh, " +
                           "DiaChi = @DiaChi, Sdt = @Sdt, Email = @Email WHERE MaKH = @MaKH";

            object[] parameters = 
            {
                
                khachHang.TenKH,
                khachHang.CCCD,
                khachHang.NgaySinh,
                khachHang.DiaChi,
                khachHang.Sdt,
                khachHang.Email,
                khachHang.MaKH
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteKhachHang(string maKH)
        {
            string query = "DELETE FROM KhachHang WHERE MaKH = @MaKH";

            object parameter =  maKH;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<KhachHangDTO> GetAllKhachHang()
        {
            List<KhachHangDTO> khachHangs = new List<KhachHangDTO>();
            string query = "SELECT * FROM KhachHang";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO khachHang = new KhachHangDTO
                {
                    MaKH = row["MaKH"].ToString(),
                    TenKH = row["TenKH"].ToString(),
                    CCCD = row["CCCD"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    DiaChi = row["DiaChi"].ToString(),
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString()
                };

                khachHangs.Add(khachHang);
            }

            return khachHangs;
        }

        public static KhachHangDTO GetKhachHangByMaKH(string maKH)
        {
            string query = "SELECT * FROM KhachHang WHERE MaKH = @MaKH";

            object parameter =  maKH;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                KhachHangDTO khachHang = new KhachHangDTO
                {
                    MaKH = row["MaKH"].ToString(),
                    TenKH = row["TenKH"].ToString(),
                    CCCD = row["CCCD"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    DiaChi = row["DiaChi"].ToString(),
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString()
                };

                return khachHang;
            }

            return null;
        }
        public static List<KhachHangDTO> GetKhachHangByPage(int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaKH) AS Row, * FROM KhachHang) AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<KhachHangDTO> khachHangs = new List<KhachHangDTO>();

            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO khachHang = new KhachHangDTO
                {
                    MaKH = row["MaKH"].ToString(),
                    TenKH = row["TenKH"].ToString(),
                    CCCD = row["CCCD"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    DiaChi = row["DiaChi"].ToString(),
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString()
                };

                khachHangs.Add(khachHang);
            }

            return khachHangs;
        }
        public static List<KhachHangDTO> SearchKhachHangByField(string tenTruong, string tuKhoa)
        {
            string query = $"SELECT * FROM KhachHang WHERE {tenTruong} LIKE '%{tuKhoa}%'";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<KhachHangDTO> khachHangs = new List<KhachHangDTO>();

            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO khachHang = new KhachHangDTO
                {
                    MaKH = row["MaKH"].ToString(),
                    TenKH = row["TenKH"].ToString(),
                    CCCD = row["CCCD"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    DiaChi = row["DiaChi"].ToString(),
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString()
                };

                khachHangs.Add(khachHang);
            }

            return khachHangs;
        }
        public static List<KhachHangDTO> SearchKhachHangByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaKH) AS Row, * FROM KhachHang WHERE {tenTruong} LIKE '%{tuKhoa}%') AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<KhachHangDTO> khachHangs = new List<KhachHangDTO>();

            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO khachHang = new KhachHangDTO
                {
                    MaKH = row["MaKH"].ToString(),
                    TenKH = row["TenKH"].ToString(),
                    CCCD = row["CCCD"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    DiaChi = row["DiaChi"].ToString(),
                    Sdt = row["Sdt"].ToString(),
                    Email = row["Email"].ToString()
                };

                khachHangs.Add(khachHang);
            }

            return khachHangs;
        }

    }
}
