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
    public class SuCoDAO
    {
        public static int InsertSuCo(SuCoDTO suCo)
        {
            string query = "INSERT INTO SuCo (MaSC, TenSC, NguyenNhan, MucPhat) " +
                           "VALUES ( @MaSC , @TenSC , @NguyenNhan , @MucPhat )";

            object[] parameters = 
            {
                suCo.MaSC,
                 suCo.TenSC,
                 suCo.NguyenNhan,
                suCo.MucPhat
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateSuCo(SuCoDTO suCo)
        {
            string query = "UPDATE SuCo SET TenSC = @TenSC, NguyenNhan = @NguyenNhan, MucPhat = @MucPhat " +
                           "WHERE MaSC = @MaSC";
            object[] parameters =
                     {
                
                 suCo.TenSC,
                 suCo.NguyenNhan,
                suCo.MucPhat,
                suCo.MaSC
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteSuCo(string maSC)
        {
            string query = "DELETE FROM SuCo WHERE MaSC = @MaSC";

            object parameter = maSC;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<SuCoDTO> GetAllSuCo()
        {
            List<SuCoDTO> suCos = new List<SuCoDTO>();
            string query = "SELECT * FROM SuCo";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                SuCoDTO suCo = new SuCoDTO
                {
                    MaSC = row["MaSC"].ToString(),
                    TenSC = row["TenSC"].ToString(),
                    NguyenNhan = row["NguyenNhan"].ToString(),
                    MucPhat = Convert.ToDecimal(row["MucPhat"])
                };

                suCos.Add(suCo);
            }

            return suCos;
        }

        public static SuCoDTO GetSuCoByMaSC(string maSC)
        {
            string query = "SELECT * FROM SuCo WHERE MaSC = @MaSC";

            object parameter = maSC;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                SuCoDTO suCo = new SuCoDTO
                {
                    MaSC = row["MaSC"].ToString(),
                    TenSC = row["TenSC"].ToString(),
                    NguyenNhan = row["NguyenNhan"].ToString(),
                    MucPhat = Convert.ToDecimal(row["MucPhat"])
                };

                return suCo;
            }

            return null;
        }
        public static List<SuCoDTO> GetSuCoByPage(int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaSC) AS Row, * FROM SuCo) AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<SuCoDTO> suCos = new List<SuCoDTO>();

            foreach (DataRow row in data.Rows)
            {
                SuCoDTO suCo = new SuCoDTO
                {
                    MaSC = row["MaSC"].ToString(),
                    TenSC = row["TenSC"].ToString(),
                    NguyenNhan = row["NguyenNhan"].ToString(),
                    MucPhat = Convert.ToDecimal(row["MucPhat"])
                };

                suCos.Add(suCo);
            }

            return suCos;
        }
        public static List<SuCoDTO> SearchSuCoByField(string tenTruong, string tuKhoa)
        {
            string query = $"SELECT * FROM SuCo WHERE {tenTruong} LIKE '%{tuKhoa}%'";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<SuCoDTO> suCos = new List<SuCoDTO>();

            foreach (DataRow row in data.Rows)
            {
                SuCoDTO suCo = new SuCoDTO
                {
                    MaSC = row["MaSC"].ToString(),
                    TenSC = row["TenSC"].ToString(),
                    NguyenNhan = row["NguyenNhan"].ToString(),
                    MucPhat = Convert.ToDecimal(row["MucPhat"])
                };

                suCos.Add(suCo);
            }

            return suCos;
        }
        public static List<SuCoDTO> SearchSuCoByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaSC) AS Row, * FROM SuCo WHERE {tenTruong} LIKE '%{tuKhoa}%') AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<SuCoDTO> suCos = new List<SuCoDTO>();

            foreach (DataRow row in data.Rows)
            {
                SuCoDTO suCo = new SuCoDTO
                {
                    MaSC = row["MaSC"].ToString(),
                    TenSC = row["TenSC"].ToString(),
                    NguyenNhan = row["NguyenNhan"].ToString(),
                    MucPhat = Convert.ToDecimal(row["MucPhat"])
                };

                suCos.Add(suCo);
            }

            return suCos;
        }

    }
}
