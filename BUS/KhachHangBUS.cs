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
    public class KhachHangBUS
    {
        public static int InsertKhachHang(KhachHangDTO khachHang)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in KhachHangDAO
            return DAO.KhachHangDAO.InsertKhachHang(khachHang);
        }

        public static int UpdateKhachHang(KhachHangDTO khachHang)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in KhachHangDAO
            return DAO.KhachHangDAO.UpdateKhachHang(khachHang);
        }

        public static int DeleteKhachHang(string maKH)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in KhachHangDAO
            return DAO.KhachHangDAO.DeleteKhachHang(maKH);
        }

        public static List<KhachHangDTO> GetAllKhachHang()
        {
            // Call the corresponding method in KhachHangDAO
            return DAO.KhachHangDAO.GetAllKhachHang();
        }

        public static KhachHangDTO GetKhachHangByMaKH(string maKH)
        {
            // Call the corresponding method in KhachHangDAO
            return DAO.KhachHangDAO.GetKhachHangByMaKH(maKH);
        }
        public static List<KhachHangDTO> GetKhachHangByPage(int page, int itemsPerPage)
        {
            return KhachHangDAO .GetKhachHangByPage(page, itemsPerPage);
        }
        public static List<KhachHangDTO> SearchKhachHangByField(string tenTruong, string tuKhoa)
        {
            return KhachHangDAO.SearchKhachHangByField(tenTruong, tuKhoa);
        }
        public static List<KhachHangDTO> SearchKhachHangByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            return KhachHangDAO.SearchKhachHangByFieldAndPage(tenTruong, tuKhoa, page, itemsPerPage);
        }
    }
}
