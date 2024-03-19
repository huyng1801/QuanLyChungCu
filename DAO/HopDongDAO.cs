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
    public class HopDongDAO
    {
        public static int InsertHopDong(HopDongDTO hopDong)
        {
            string query = "INSERT INTO HopDong (MaHD, MaKH, MaNV, MaCH, NgayThue, NgayTra, TienCoc, TongTien, Trangthai) " +
                           "VALUES ( @MaHD , @MaKH , @MaNV , @MaCH , @NgayThue , @NgayTra , @TienCoc , @TongTien , @Trangthai )";

            object[] parameters = 
            {
                 hopDong.MaHD,
                 hopDong.MaKH,
                hopDong.MaNV,
                hopDong.MaCH,
                hopDong.NgayThue,
                hopDong.NgayTra,
                hopDong.TienCoc,
                hopDong.TongTien,
                hopDong.Trangthai
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateHopDong(HopDongDTO hopDong)
        {
            string query = "UPDATE HopDong SET MaKH = @MaKH, MaNV = @MaNV, MaCH = @MaCH, NgayThue = @NgayThue, " +
                           "NgayTra = @NgayTra, TienCoc = @TienCoc, TongTien = @TongTien, Trangthai = @Trangthai " +
                           "WHERE MaHD = @MaHD";

            object[] parameters = 
            {
               
                hopDong.MaKH,
                hopDong.MaNV,
                hopDong.MaCH,
                hopDong.NgayThue,
                hopDong.NgayTra,
                hopDong.TienCoc,
                hopDong.TongTien,
                hopDong.Trangthai,
                 hopDong.MaHD,
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteHopDong(string maHD)
        {
            string query = "DELETE FROM HopDong WHERE MaHD = @MaHD";

            object parameter =  maHD;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<HopDongDTO> GetAllHopDong()
        {
            List<HopDongDTO> hopDongs = new List<HopDongDTO>();
            string query = "SELECT * FROM HopDong";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                HopDongDTO hopDong = new HopDongDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaKH = row["MaKH"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaCH = row["MaCH"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayThue = Convert.ToDateTime(row["NgayThue"]),
                    NgayTra = row["NgayTra"] != DBNull.Value ? Convert.ToDateTime(row["NgayTra"]) : (DateTime?)null,
                    TienCoc = Convert.ToDecimal(row["TienCoc"]),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    Trangthai = row["Trangthai"].ToString()
                };

                hopDongs.Add(hopDong);
            }

            return hopDongs;
        }

        public static HopDongDTO GetHopDongByMaHD(string maHD)
        {
            string query = "SELECT * FROM HopDong WHERE MaHD = @MaHD";

            object parameter =  maHD;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                HopDongDTO hopDong = new HopDongDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaKH = row["MaKH"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaCH = row["MaCH"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayThue = Convert.ToDateTime(row["NgayThue"]),
                    NgayTra = row["NgayTra"] != DBNull.Value ? Convert.ToDateTime(row["NgayTra"]) : (DateTime?)null,
                    TienCoc = Convert.ToDecimal(row["TienCoc"]),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    Trangthai = row["Trangthai"].ToString()
                };

                return hopDong;
            }

            return null;
        }
        public static List<HopDongDTO> SearchHopDongByField(string fieldName, string value)
        {
            List<HopDongDTO> hopDongs = new List<HopDongDTO>();

            // Xây dựng truy vấn SQL dựa trên tên trường và giá trị
            string query = "SELECT * FROM HopDong WHERE " + fieldName + " LIKE @" + fieldName;

            // Tạo tham số cho giá trị
            object parameter = value;

            // Thực hiện truy vấn sử dụng DataProvider
            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            foreach (DataRow row in data.Rows)
            {
                HopDongDTO hopDong = new HopDongDTO
                {
                    MaHD = row["MaHD"].ToString(),
                    MaKH = row["MaKH"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    MaCH = row["MaCH"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    NgayThue = Convert.ToDateTime(row["NgayThue"]),
                    NgayTra = row["NgayTra"] != DBNull.Value ? Convert.ToDateTime(row["NgayTra"]) : (DateTime?)null,
                    TienCoc = Convert.ToDecimal(row["TienCoc"]),
                    TongTien = Convert.ToDecimal(row["TongTien"]),
                    Trangthai = row["Trangthai"].ToString()
                };

                hopDongs.Add(hopDong);
            }

            return hopDongs;
        }
        public static int ThanhLyHopDong(string maHD)
        {
            string query = "UPDATE HopDong SET Trangthai = N'Đã thanh lý' WHERE MaHD = @MaHD";

            object parameter = maHD;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }
    }
}
