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
    public class HoaDonDAO
    {
        public static int InsertHoaDon(HoaDonDTO hoaDon)
        {
            string query = "INSERT INTO HoaDon (MaHD, MaNV, MaHopDong) " +
                           "VALUES ( @MaHD , @MaNV , @MaHopDong)";

            object[] parameters =
            {
                 hoaDon.MaHD,
                 hoaDon.MaNV,
                hoaDon.MaHopDong
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateHoaDon(HoaDonDTO hoaDon)
        {
            string query = "UPDATE HoaDon SET NgayTT = GETDATE() WHERE MaHD = @MaHD";

            object[] parameters = 
            {
                hoaDon.MaHD
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteHoaDon(string maHD)
        {
            string query = "DELETE FROM HoaDon WHERE MaHD = @MaHD";

            object parameter =  maHD;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<HoaDonDTO> GetAllHoaDon()
        {
            List<HoaDonDTO> hoaDons = new List<HoaDonDTO>();
            string query = "SELECT * FROM HoaDon";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                HoaDonDTO hoaDon = new HoaDonDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaHopDong = row["MaHopDong"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayTT = row["NgayTT"] != DBNull.Value ? (DateTime?)row["NgayTT"] : null,
                    TongTien = Convert.ToDecimal(row["TongTien"])
                };

                hoaDons.Add(hoaDon);
            }

            return hoaDons;
        }

        public static HoaDonDTO GetHoaDonByMaHD(string maHD)
        {
            string query = "SELECT * FROM HoaDon WHERE MaHD = @MaHD";

            object parameter =  maHD;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                HoaDonDTO hoaDon = new HoaDonDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaHopDong = row["MaHopDong"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayTT = row["NgayTT"] != DBNull.Value ? (DateTime?)row["NgayTT"] : null,
                    TongTien = Convert.ToDecimal(row["TongTien"])
                };

                return hoaDon;
            }

            return null;
        }
        public static List<HoaDonDTO> SearchHoaDonByField(string fieldName, string value)
        {
            List<HoaDonDTO> hoaDons = new List<HoaDonDTO>();

            // Xây dựng truy vấn SQL dựa trên tên trường và giá trị
            string query = "SELECT * FROM HoaDon WHERE " + fieldName + " LIKE @" + fieldName;

            // Tạo tham số cho giá trị
            object parameter = value;

            // Thực hiện truy vấn sử dụng DataProvider
            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            foreach (DataRow row in data.Rows)
            {
                HoaDonDTO hoaDon = new HoaDonDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaHopDong = row["MaHopDong"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayTT = row["NgayTT"] != DBNull.Value ? (DateTime?)row["NgayTT"] : null,
                    TongTien = Convert.ToDecimal(row["TongTien"])
                };

                hoaDons.Add(hoaDon);
            }

            return hoaDons;
        }

    }
}
