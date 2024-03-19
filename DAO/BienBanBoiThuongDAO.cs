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
    public class BienBanBoiThuongDAO
    {
        public static int InsertBienBanBoiThuong(BienBanBoiThuongDTO bienBanBoiThuong)
        {
            string query = "INSERT INTO BienBanBoiThuong (MaBB, MaHD, MaNV) " +
                           "VALUES ( @MaBB , @MaHD , @MaNV )";

            object[] parameters =
            {
                bienBanBoiThuong.MaBB,
                 bienBanBoiThuong.MaHD,
                 bienBanBoiThuong.MaNV
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateBienBanBoiThuong(BienBanBoiThuongDTO bienBanBoiThuong)
        {
            string query = "UPDATE BienBanBoiThuong SET MaHD = @MaHD, MaNV = @MaNV, " +
                           "NgayTao = @NgayTao WHERE MaBB = @MaBB";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MaBB", bienBanBoiThuong.MaBB),
                new SqlParameter("@MaHD", bienBanBoiThuong.MaHD),
                new SqlParameter("@MaNV", bienBanBoiThuong.MaNV),
                new SqlParameter("@NgayTao", bienBanBoiThuong.NgayTao)
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteBienBanBoiThuong(string maBB)
        {
            string query = "DELETE FROM BienBanBoiThuong WHERE MaBB = @MaBB";

            object parameter =  maBB;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<BienBanBoiThuongDTO> GetAllBienBanBoiThuong()
        {
            List<BienBanBoiThuongDTO> bienBanBoiThuongs = new List<BienBanBoiThuongDTO>();
            string query = "SELECT * FROM BienBanBoiThuong";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                BienBanBoiThuongDTO bienBanBoiThuong = new BienBanBoiThuongDTO
                {
                    MaBB = row["MaBB"].ToString(),
                    MaHD = row["MaHD"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    TongTien = Convert.ToDecimal(row["TongTien"].ToString())
                };

                bienBanBoiThuongs.Add(bienBanBoiThuong);
            }

            return bienBanBoiThuongs;
        }

        public static BienBanBoiThuongDTO GetBienBanBoiThuongByMaBB(string maBB)
        {
            string query = "SELECT * FROM BienBanBoiThuong WHERE MaBB = @MaBB";

            object parameter =  maBB;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                BienBanBoiThuongDTO bienBanBoiThuong = new BienBanBoiThuongDTO
                {
                    MaBB = row["MaBB"].ToString(),
                    MaHD = row["MaHD"].ToString(),
                    MaNV = row["MaNV"].ToString(),
                    NgayTao = Convert.ToDateTime(row["NgayTao"]),
                    TongTien = Convert.ToDecimal(row["TongTien"].ToString()),
                };

                return bienBanBoiThuong;
            }

            return null;
        }
    }
}
