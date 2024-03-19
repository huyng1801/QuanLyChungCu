using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class BienBanBoiThuongBUS
    {
        public static int InsertBienBanBoiThuong(BienBanBoiThuongDTO bienBanBoiThuong)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in BienBanBoiThuongDAO
            return DAO.BienBanBoiThuongDAO.InsertBienBanBoiThuong(bienBanBoiThuong);
        }

        public static int UpdateBienBanBoiThuong(BienBanBoiThuongDTO bienBanBoiThuong)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in BienBanBoiThuongDAO
            return DAO.BienBanBoiThuongDAO.UpdateBienBanBoiThuong(bienBanBoiThuong);
        }

        public static int DeleteBienBanBoiThuong(string maBB)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in BienBanBoiThuongDAO
            return DAO.BienBanBoiThuongDAO.DeleteBienBanBoiThuong(maBB);
        }

        public static List<BienBanBoiThuongDTO> GetAllBienBanBoiThuong()
        {
            // Call the corresponding method in BienBanBoiThuongDAO
            return DAO.BienBanBoiThuongDAO.GetAllBienBanBoiThuong();
        }

        public static BienBanBoiThuongDTO GetBienBanBoiThuongByMaBB(string maBB)
        {
            // Call the corresponding method in BienBanBoiThuongDAO
            return DAO.BienBanBoiThuongDAO.GetBienBanBoiThuongByMaBB(maBB);
        }
    }
}
