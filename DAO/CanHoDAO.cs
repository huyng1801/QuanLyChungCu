using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace DAO
{
    public class CanHoDAO
    {
        public static List<CanHoDTO> GetAllCanHo()
        {
            List<CanHoDTO> canHos = new List<CanHoDTO>();
            string query = "SELECT * FROM CanHo";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                canHos.Add(canHo);
            }

            return canHos;
        }
        public static List<CanHoDTO> GetCanHoByPage(int page, int itemsPerPage)
        {
            List<CanHoDTO> canHos = new List<CanHoDTO>();
            int startIndex = (page - 1) * itemsPerPage + 1;
            int endIndex = startIndex + itemsPerPage - 1;

            string query = "SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY MaCH) AS RowNum FROM CanHo) AS CanHoWithRowNum WHERE RowNum BETWEEN @StartIndex AND @EndIndex";
            object[] parameters = 
            {
        startIndex,
         endIndex
            };

            DataTable data = DataProvider.ExecuteQuery(query, parameters);

            foreach (DataRow row in data.Rows)
            {
                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                canHos.Add(canHo);
            }

            return canHos;
        }


        public static int InsertCanHo(CanHoDTO canHo)
        {
            string query = "INSERT INTO CanHo (MaCH, SoTang, DienTich, PhongNgu, NoiThat, TinhTrang, GiaThue) " +
                           "VALUES ( @MaCH, @SoTang , @DienTich , @PhongNgu , @NoiThat , @TinhTrang , @GiaThue )";

            object[] parameters = 
            {
                 canHo.MaCH,
                canHo.SoTang,
                canHo.DienTich,
                canHo.PhongNgu,
                canHo.NoiThat,
                canHo.TinhTrang,
                canHo.GiaThue
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        

        public static int UpdateCanHo(CanHoDTO canHo)
        {
            string query = "UPDATE CanHo SET SoTang = @SoTang, DienTich = @DienTich, PhongNgu = @PhongNgu, " +
                           "NoiThat = @NoiThat, TinhTrang = @TinhTrang, GiaThue = @GiaThue " +
                           "WHERE MaCH = @MaCH";

            object[] parameters = 
            {
                
                canHo.SoTang,
                canHo.DienTich,
                canHo.PhongNgu,
                canHo.NoiThat,
                canHo.TinhTrang,
                    canHo.GiaThue,
                    canHo.MaCH
            };

            return DataProvider.ExecuteNonQuery(query, parameters);
        }

        public static int DeleteCanHo(string maCH)
        {
            string query = "DELETE FROM CanHo WHERE MaCH = @MaCH";

            object parameter =  maCH;

            return DataProvider.ExecuteNonQuery(query, new object[] { parameter });
        }

        public static CanHoDTO GetCanHoByMaCH(string maCH)
        {
            string query = "SELECT * FROM CanHo WHERE MaCH = @MaCH";

            object parameter =  maCH;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { parameter });

            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];

                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                return canHo;
            }

            return null;
        }
        public static List<CanHoDTO> SearchCanHoByField(string fieldName, string keyword)
        {
            List<CanHoDTO> canHos = new List<CanHoDTO>();
            string query = "SELECT * FROM CanHo WHERE ";

            // Xác định trường cần tìm kiếm dựa vào giá trị của fieldName
            switch (fieldName)
            {
                case "MaCH":
                    query += "MaCH LIKE @Keyword";
                    break;
                case "SoTang":
                    query += "SoTang LIKE @Keyword";
                    break;
               
         
                case "NoiThat":
                    query += "NoiThat LIKE @Keyword";
                    break;
                case "TinhTrang":
                    query += "TinhTrang LIKE @Keyword";
                    break;
                // Thêm các trường khác nếu cần
                default:
                    // Trường mặc định là MaCH
                    query += "MaCH LIKE @Keyword";
                    break;
            }

            // Tạo tham số cho keyword
            object keywordParameter = "%" + keyword + "%";

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { keywordParameter });

            foreach (DataRow row in data.Rows)
            {
                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                canHos.Add(canHo);
            }

            return canHos;
        }
        public static List<CanHoDTO> SearchCanHoByFieldAndPage(string fieldName, string keyword, int page, int itemsPerPage)
        {
            List<CanHoDTO> canHos = new List<CanHoDTO>();
            int startIndex = (page - 1) * itemsPerPage + 1;
            int endIndex = startIndex + itemsPerPage - 1;

            string query = "SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY MaCH) AS RowNum FROM CanHo WHERE ";

            // Xác định trường cần tìm kiếm dựa vào giá trị của fieldName
            switch (fieldName)
            {
                case "MaCH":
                    query += "MaCH LIKE @Keyword";
                    break;
                case "SoTang":
                    query += "SoTang LIKE @Keyword";
                    break;
       
                case "NoiThat":
                    query += "NoiThat LIKE @Keyword";
                    break;
                case "TinhTrang":
                    query += "TinhTrang LIKE @Keyword";
                    break;
            
                default:
                    query += "MaCH LIKE @Keyword";
                    break;
            }

            query += " ) AS CanHoWithRowNum WHERE RowNum BETWEEN @StartIndex AND @EndIndex";

            // Tạo tham số cho keyword
            object keywordParameter =  "%" + keyword + "%";
            if (fieldName == "TinhTrang")
                keywordParameter = keyword;
            // Tạo tham số cho startIndex và endIndex
            object startIndexParameter =  startIndex;

            object endIndexParameter = endIndex;

            DataTable data = DataProvider.ExecuteQuery(query, new object[] { keywordParameter, startIndexParameter, endIndexParameter });

            foreach (DataRow row in data.Rows)
            {
                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                canHos.Add(canHo);
            }

            return canHos;
        }
        public static List<CanHoDTO> layCanHoCoTheSuDung()
        {
            List<CanHoDTO> availableApartments = new List<CanHoDTO>();

            string query = @"
        SELECT c.*
        FROM CanHo c
        LEFT JOIN HopDong h ON c.MaCH = h.MaCH
        WHERE h.MaHD IS NULL OR (h.TrangThai = N'Đã thanh lý')
    ";

            DataTable data = DataProvider.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                CanHoDTO canHo = new CanHoDTO
                {
                    MaCH = row["MaCH"].ToString(),
                    SoTang = row["SoTang"].ToString(),
                    DienTich = Convert.ToDecimal(row["DienTich"]),
                    PhongNgu = Convert.ToInt32(row["PhongNgu"]),
                    NoiThat = row["NoiThat"].ToString(),
                    TinhTrang = row["TinhTrang"].ToString(),
                    GiaThue = Convert.ToDecimal(row["GiaThue"])
                };

                availableApartments.Add(canHo);
            }

            return availableApartments;
        }

    }
}
