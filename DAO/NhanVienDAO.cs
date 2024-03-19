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
    public class NhanVienDAO
    {
        public static int InsertNhanVien(NhanVienDTO nhanVien)
        {
            string query = "INSERT INTO NhanVien (MaNV, MatKhau, TenNV, NgaySinh, Diachi, Sdt) " +
                           "VALUES ( @MaNV, @MatKhau , @TenNV, @NgaySinh , @Diachi , @Sdt )";

            object[] parameters = 
            {
               nhanVien.MaNV,
                nhanVien.MatKhau,
                nhanVien.TenNV,
                nhanVien.NgaySinh,
                nhanVien.Diachi,
                nhanVien.Sdt
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateNhanVien(NhanVienDTO nhanVien)
        {
            string query = "UPDATE NhanVien SET MatKhau = @MatKhau, TenNV = @TenNV, NgaySinh = @NgaySinh, " +
                           "Diachi = @Diachi, Sdt = @Sdt WHERE MaNV = @MaNV";

            object[] parameters =
     {
               
                nhanVien.MatKhau,
                nhanVien.TenNV,
                nhanVien.NgaySinh,
                nhanVien.Diachi,
                nhanVien.Sdt,
                nhanVien.MaNV
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteNhanVien(string maNV)
        {
            string query = "DELETE FROM NhanVien WHERE MaNV = @MaNV";

            object parameter =  maNV;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<NhanVienDTO> GetAllNhanVien()
        {
            List<NhanVienDTO> nhanViens = new List<NhanVienDTO>();
            string query = "SELECT * FROM NhanVien";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                nhanViens.Add(nhanVien);
            }

            return nhanViens;
        }

        public static NhanVienDTO GetNhanVienByMaNV(string maNV)
        {
            string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV";

            object parameter = maNV;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                return nhanVien;
            }

            return null;
        }
        public static NhanVienDTO Login(string maNV, string matKhau)
        {
            string query = "SELECT * FROM NhanVien WHERE MaNV = @MaNV AND MatKhau = @MatKhau";

            object[] parameters = 
            {
                maNV,
                matKhau
            };

            DataTable data = DataProvider.ExecuteQuery(query, parameters);

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                return nhanVien;
            }

            return null; // Login failed, no matching record found
        }
        public static List<NhanVienDTO> GetNhanVienByPage(int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaNV) AS Row, * FROM NhanVien) AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<NhanVienDTO> nhanViens = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                nhanViens.Add(nhanVien);
            }

            return nhanViens;
        }
        public static List<NhanVienDTO> SearchNhanVienByField(string tenTruong, string tuKhoa)
        {
            string query = $"SELECT * FROM NhanVien WHERE {tenTruong} LIKE '%{tuKhoa}%'";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<NhanVienDTO> nhanViens = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                nhanViens.Add(nhanVien);
            }

            return nhanViens;
        }
        public static List<NhanVienDTO> SearchNhanVienByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaNV) AS Row, * FROM NhanVien WHERE {tenTruong} LIKE '%{tuKhoa}%') AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<NhanVienDTO> nhanViens = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nhanVien = new NhanVienDTO
                {
                    MaNV = row["MaNV"].ToString(),
                    MatKhau = row["MatKhau"].ToString(),
                    TenNV = row["TenNV"].ToString(),
                    NgaySinh = Convert.ToDateTime(row["NgaySinh"]),
                    Diachi = row["Diachi"].ToString(),
                    Sdt = row["Sdt"].ToString()
                };

                nhanViens.Add(nhanVien);
            }

            return nhanViens;
        }

    }

}
