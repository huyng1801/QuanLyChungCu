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
    public class HopDongBUS
    {
        public static int InsertHopDong(HopDongDTO hopDong)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HopDongDAO
            return DAO.HopDongDAO.InsertHopDong(hopDong);
        }

        public static int UpdateHopDong(HopDongDTO hopDong)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HopDongDAO
            return DAO.HopDongDAO.UpdateHopDong(hopDong);
        }

        public static int DeleteHopDong(string maHD)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in HopDongDAO
            return DAO.HopDongDAO.DeleteHopDong(maHD);
        }

        public static List<HopDongDTO> GetAllHopDong()
        {
            // Call the corresponding method in HopDongDAO
            return DAO.HopDongDAO.GetAllHopDong();
        }

        public static HopDongDTO GetHopDongByMaHD(string maHD)
        {
            // Call the corresponding method in HopDongDAO
            return DAO.HopDongDAO.GetHopDongByMaHD(maHD);
        }
        public static List<HopDongDTO> SearchHopDongByField(string fieldName, string value)
        {
           return HopDongDAO.SearchHopDongByField(fieldName, value);
        }
        public static int ThanhLyHopDong(string maHD)
        {

            return HopDongDAO.ThanhLyHopDong(maHD);
        }
    }
}
