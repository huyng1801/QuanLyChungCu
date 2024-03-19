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
    public class NhanVienBUS
    {
        public static int InsertNhanVien(NhanVienDTO nhanVien)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.InsertNhanVien(nhanVien);
        }

        public static int UpdateNhanVien(NhanVienDTO nhanVien)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.UpdateNhanVien(nhanVien);
        }

        public static int DeleteNhanVien(string maNV)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.DeleteNhanVien(maNV);
        }

        public static List<NhanVienDTO> GetAllNhanVien()
        {
            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.GetAllNhanVien();
        }

        public static NhanVienDTO GetNhanVienByMaNV(string maNV)
        {
            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.GetNhanVienByMaNV(maNV);
        }

        public static NhanVienDTO Login(string maNV, string matKhau)
        {
            // Call the corresponding method in NhanVienDAO
            return DAO.NhanVienDAO.Login(maNV, matKhau);
        }
        public static List<NhanVienDTO> GetNhanVienByPage(int page, int itemsPerPage)
        {
           return NhanVienDAO.GetNhanVienByPage(page, itemsPerPage);
        }
        public static List<NhanVienDTO> SearchNhanVienByField(string tenTruong, string tuKhoa)
        {
            return NhanVienDAO.SearchNhanVienByField(tenTruong, tuKhoa);
        }
        public static List<NhanVienDTO> SearchNhanVienByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
            return NhanVienDAO.SearchNhanVienByFieldAndPage(tenTruong, tuKhoa, page, itemsPerPage);
        }
    }
}
