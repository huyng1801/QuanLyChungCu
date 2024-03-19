using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ChiTietBienBanBoiThuongBUS
    {
        public static int InsertChiTietBienBanBoiThuong(ChiTietBienBanBoiThuongDTO chiTietBBBT)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in ChiTietBienBanBoiThuongDAO
            return DAO.ChiTietBienBanBoiThuongDAO.InsertChiTietBienBanBoiThuong(chiTietBBBT);
        }

        public static int DeleteChiTietBienBanBoiThuongByMaBB(int id)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in ChiTietBienBanBoiThuongDAO
            return DAO.ChiTietBienBanBoiThuongDAO.DeleteChiTietBienBanBoiThuongByMaBB(id);
        }

        public static List<ChiTietBienBanBoiThuongDTO> GetChiTietBienBanBoiThuongByMaBB(string maBB)
        {
            // Call the corresponding method in ChiTietBienBanBoiThuongDAO
            return DAO.ChiTietBienBanBoiThuongDAO.GetChiTietBienBanBoiThuongByMaBB(maBB);
        }
    }
}
