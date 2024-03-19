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
    public class ChiTietBienBanBoiThuongDAO
    {
        public static int InsertChiTietBienBanBoiThuong(ChiTietBienBanBoiThuongDTO chiTietBBBT)
        {
            string query = "INSERT INTO ChiTietBienBanBoiThuong (MaBB, MaSC, Soluong) " +
                           "VALUES ( @MaBB , @MaSC , @Soluong )";

            object[] parameters = 
            {   
                chiTietBBBT.MaBB, chiTietBBBT.MaSC, chiTietBBBT.Soluong
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteChiTietBienBanBoiThuongByMaBB(int id)
        {
            string query = "DELETE FROM ChiTietBienBanBoiThuong WHERE Id = @Id";

            object parameter =  id;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<ChiTietBienBanBoiThuongDTO> GetChiTietBienBanBoiThuongByMaBB(string maBB)
        {
            List<ChiTietBienBanBoiThuongDTO> chiTietBBBTs = new List<ChiTietBienBanBoiThuongDTO>();
            string query = "SELECT * FROM ChiTietBienBanBoiThuong WHERE MaBB = @MaBB";

            object parameter =  maBB;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            foreach (DataRow row in data.Rows)
            {
                ChiTietBienBanBoiThuongDTO chiTietBBBT = new ChiTietBienBanBoiThuongDTO
                {
                    Id = Convert.ToInt32(row["Id"]),
                    MaBB = row["MaBB"].ToString(),
                    MaSC = row["MaSC"].ToString(),
                    Soluong = Convert.ToInt32(row["Soluong"])
                };

                chiTietBBBTs.Add(chiTietBBBT);
            }

            return chiTietBBBTs;
        }
    }
}
