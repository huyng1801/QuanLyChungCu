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
    public class ChiTietHoaDonDAO
    {
        public static int InsertChiTietHoaDon(ChiTietHoaDonDTO chiTietHoaDon)
        {
            string query = "INSERT INTO ChiTietHoaDon (MaHD, MaDV, HeSo) " +
                           "VALUES ( @MaHD , @MaDV , @HeSo )";

            object[] parameters =
            {
                 chiTietHoaDon.MaHD,
                 chiTietHoaDon.MaDV,
               chiTietHoaDon.HeSo
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteChiTietHoaDonByMaHD(string maChiTietHoaDon)
        {
            string query = "DELETE FROM ChiTietHoaDon WHERE MaChiTietHoaDon = @MaChiTietHoaDon";

            object parameter = maChiTietHoaDon;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<ChiTietHoaDonDTO> GetChiTietHoaDonByMaHD(string maHD)
        {
            List<ChiTietHoaDonDTO> chiTietHoaDons = new List<ChiTietHoaDonDTO>();
            string query = "SELECT * FROM ChiTietHoaDon WHERE MaHD = @MaHD";

            object parameter =  maHD;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            foreach (DataRow row in data.Rows)
            {
                ChiTietHoaDonDTO chiTietHoaDon = new ChiTietHoaDonDTO
                {
                    MaChiTietHoaDon = Convert.ToInt32(row["MaChiTietHoaDon"]),
                    MaHD = row["MaHD"].ToString(),
                    MaDV = row["MaDV"].ToString(),
                    HeSo = Convert.ToDecimal(row["HeSo"])
                };

                chiTietHoaDons.Add(chiTietHoaDon);
            }

            return chiTietHoaDons;
        }
    }
}
