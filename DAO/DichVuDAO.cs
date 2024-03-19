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
    public class DichVuDAO
    {
        public static int InsertDichVu(DichVuDTO dichVu)
        {
            string query = "INSERT INTO DichVu (MaDV, TenDV, GiaTien) " +
                           "VALUES ( @MaDV , @TenDV , @GiaTien )";

            object[] parameters = 
            {
                dichVu.MaDV,
                dichVu.TenDV,
                dichVu.GiaTien
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int UpdateDichVu(DichVuDTO dichVu)
        {
            string query = "UPDATE DichVu SET TenDV = @TenDV, GiaTien = @GiaTien WHERE MaDV = @MaDV";

            object[] parameters = 
            {
                
                dichVu.TenDV,
                 dichVu.GiaTien,
                 dichVu.MaDV
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteDichVu(string maDV)
        {
            string query = "DELETE FROM DichVu WHERE MaDV = @MaDV";

            object parameter =  maDV;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static List<DichVuDTO> GetAllDichVu()
        {
            List<DichVuDTO> dichVus = new List<DichVuDTO>();
            string query = "SELECT * FROM DichVu";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                DichVuDTO dichVu = new DichVuDTO
                {
                    MaDV = row["MaDV"].ToString(),
                    TenDV = row["TenDV"].ToString(),
                    GiaTien = Convert.ToDecimal(row["GiaTien"])
                };

                dichVus.Add(dichVu);
            }

            return dichVus;
        }

        public static DichVuDTO GetDichVuByMaDV(string maDV)
        {
            string query = "SELECT * FROM DichVu WHERE MaDV = @MaDV";

            object parameter = maDV;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                DichVuDTO dichVu = new DichVuDTO
                {
                    MaDV = row["MaDV"].ToString(),
                    TenDV = row["TenDV"].ToString(),
                    GiaTien = Convert.ToDecimal(row["GiaTien"])
                };

                return dichVu;
            }

            return null;
        }
        public static List<DichVuDTO> GetDichVuByPage(int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaDV) AS Row, * FROM DichVu) AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<DichVuDTO> dichVus = new List<DichVuDTO>();

            foreach (DataRow row in data.Rows)
            {
                DichVuDTO dichVu = new DichVuDTO
                {
                    MaDV = row["MaDV"].ToString(),
                    TenDV = row["TenDV"].ToString(),
                    GiaTien = Convert.ToDecimal(row["GiaTien"])
                };

                dichVus.Add(dichVu);
            }

            return dichVus;
        }
        public static List<DichVuDTO> SearchDichVuByField(string tenTruong, string tuKhoa)
        {
            string query = $"SELECT * FROM DichVu WHERE {tenTruong} LIKE '%{tuKhoa}%'";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<DichVuDTO> dichVus = new List<DichVuDTO>();

            foreach (DataRow row in data.Rows)
            {
                DichVuDTO dichVu = new DichVuDTO
                {
                    MaDV = row["MaDV"].ToString(),
                    TenDV = row["TenDV"].ToString(),
                    GiaTien = Convert.ToDecimal(row["GiaTien"])
                };

                dichVus.Add(dichVu);
            }

            return dichVus;
        }
        public static List<DichVuDTO> SearchDichVuByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            int offset = (page - 1) * itemsPerPage;
            string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY MaDV) AS Row, * FROM DichVu WHERE {tenTruong} LIKE '%{tuKhoa}%') AS TempTable " +
                           $"WHERE Row > {offset} AND Row <= {offset + itemsPerPage}";

            DataTable data = DataProvider.ExecuteQuery(query);
            List<DichVuDTO> dichVus = new List<DichVuDTO>();

            foreach (DataRow row in data.Rows)
            {
                DichVuDTO dichVu = new DichVuDTO
                {
                    MaDV = row["MaDV"].ToString(),
                    TenDV = row["TenDV"].ToString(),
                    GiaTien = Convert.ToDecimal(row["GiaTien"])
                };

                dichVus.Add(dichVu);
            }

            return dichVus;
        }

    }
}
