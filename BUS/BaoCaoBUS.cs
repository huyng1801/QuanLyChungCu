using DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class BaoCaoBUS
    {
        public static DataTable BaoCaoDoanhThu(DateTime startDate, DateTime endDate)
        {
            // You can add any additional validation or business logic here
            // For example, checking if the start date is before the end date

            // Call the corresponding method in BaoCaoDAO
            return DAO.BaoCaoDAO.BaoCaoDoanhThu(startDate, endDate);
        }

   
        public static DataTable BaoCaoTongHopCanHo()
        {
            // Call the corresponding method in BaoCaoDAO
            return DAO.BaoCaoDAO.BaoCaoTongHopCanHo();
        }

        public static DataTable BaoCaoTongHopKhachThue()
        {
            // Call the corresponding method in BaoCaoDAO
            return DAO.BaoCaoDAO.BaoCaoTongHopKhachThue();
        }

        public static DataTable BaoCaoHopDongSapHetHan()
        {
            // You can add any additional validation or business logic here
            // For example, checking if the end date is in the future

            // Call the corresponding method in BaoCaoDAO
            return DAO.BaoCaoDAO.BaoCaoHopDongSapHetHan();
        }
        public static DataTable BaoCaoThanhToanCham()
        {
         return BaoCaoDAO.BaoCaoThanhToanCham();
        }
        public static DataTable BaoCaoTienCoc(DateTime startDate, DateTime endDate)
        {
           return BaoCaoDAO.BaoCaoTienCoc(startDate, endDate);
        }

    }
}