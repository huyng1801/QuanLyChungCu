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
    public class HoaDonBUS
    {
        public static int InsertHoaDon(HoaDonDTO hoaDon)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HoaDonDAO
            return DAO.HoaDonDAO.InsertHoaDon(hoaDon);
        }

        public static int UpdateHoaDon(HoaDonDTO hoaDon)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HoaDonDAO
            return DAO.HoaDonDAO.UpdateHoaDon(hoaDon);
        }

        public static int DeleteHoaDon(string maHD)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HoaDonDAO
            return DAO.HoaDonDAO.DeleteHoaDon(maHD);
        }

        public static List<HoaDonDTO> GetAllHoaDon()
        {
            // Call the corresponding method in HoaDonDAO
            return DAO.HoaDonDAO.GetAllHoaDon();
        }

        public static HoaDonDTO GetHoaDonByMaHD(string maHD)
        {
            // Call the corresponding method in HoaDonDAO
            return DAO.HoaDonDAO.GetHoaDonByMaHD(maHD);
        }
        public static List<HoaDonDTO> SearchHoaDonByField(string fieldName, string value)
        {
          return  HoaDonDAO.SearchHoaDonByField(fieldName, value);
        }
    }
}
