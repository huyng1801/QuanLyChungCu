using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class DichVuBUS
    {
        public static int InsertDichVu(DichVuDTO dichVu)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in DichVuDAO
            return DAO.DichVuDAO.InsertDichVu(dichVu);
        }

        public static int UpdateDichVu(DichVuDTO dichVu)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in DichVuDAO
            return DAO.DichVuDAO.UpdateDichVu(dichVu);
        }

        public static int DeleteDichVu(string maDV)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in DichVuDAO
            return DAO.DichVuDAO.DeleteDichVu(maDV);
        }

        public static List<DichVuDTO> GetAllDichVu()
        {
            // Call the corresponding method in DichVuDAO
            return DAO.DichVuDAO.GetAllDichVu();
        }

        public static DichVuDTO GetDichVuByMaDV(string maDV)
        {
            // Call the corresponding method in DichVuDAO
            return DAO.DichVuDAO.GetDichVuByMaDV(maDV);
        }
        public static List<DichVuDTO> GetDichVuByPage(int page, int itemsPerPage)
        {
            return DichVuDAO.GetDichVuByPage(page, itemsPerPage);
        }
        public static List<DichVuDTO> SearchDichVuByField(string tenTruong, string tuKhoa)
        {
            return DichVuDAO.SearchDichVuByField(tenTruong, tuKhoa);
        }
        public static List<DichVuDTO> SearchDichVuByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            return DichVuDAO.SearchDichVuByFieldAndPage(tenTruong, tuKhoa, page, itemsPerPage);
        }
    }
}
