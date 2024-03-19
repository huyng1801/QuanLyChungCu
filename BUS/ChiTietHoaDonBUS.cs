using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ChiTietHoaDonBUS
    {
        public static int InsertChiTietHoaDon(ChiTietHoaDonDTO chiTietHoaDon)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in ChiTietHoaDonDAO
            return DAO.ChiTietHoaDonDAO.InsertChiTietHoaDon(chiTietHoaDon);
        }

        public static int DeleteChiTietHoaDonByMaHD(string maHD)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in ChiTietHoaDonDAO
            return DAO.ChiTietHoaDonDAO.DeleteChiTietHoaDonByMaHD(maHD);
        }

        public static List<ChiTietHoaDonDTO> GetChiTietHoaDonByMaHD(string maHD)
        {
            // Call the corresponding method in ChiTietHoaDonDAO
            return DAO.ChiTietHoaDonDAO.GetChiTietHoaDonByMaHD(maHD);
        }
    }
}
