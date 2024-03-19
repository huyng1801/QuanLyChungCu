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
    public class SuCoBUS
    {
        public static int InsertSuCo(SuCoDTO suCo)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in SuCoDAO
            return DAO.SuCoDAO.InsertSuCo(suCo);
        }

        public static int UpdateSuCo(SuCoDTO suCo)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in SuCoDAO
            return DAO.SuCoDAO.UpdateSuCo(suCo);
        }

        public static int DeleteSuCo(string maSC)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in SuCoDAO
            return DAO.SuCoDAO.DeleteSuCo(maSC);
        }

        public static List<SuCoDTO> GetAllSuCo()
        {
            // Call the corresponding method in SuCoDAO
            return DAO.SuCoDAO.GetAllSuCo();
        }

        public static SuCoDTO GetSuCoByMaSC(string maSC)
        {
            // Call the corresponding method in SuCoDAO
            return DAO.SuCoDAO.GetSuCoByMaSC(maSC);
        }
        public static List<SuCoDTO> GetSuCoByPage(int page, int itemsPerPage)
        {
            return SuCoDAO.GetSuCoByPage(page, itemsPerPage);
        }
        public static List<SuCoDTO> SearchSuCoByField(string tenTruong, string tuKhoa)
        {
            return SuCoDAO.SearchSuCoByField(tenTruong, tuKhoa);
        }
        public static List<SuCoDTO> SearchSuCoByFieldAndPage(string tenTruong, string tuKhoa, int page, int itemsPerPage)
        {
           return SuCoDAO.SearchSuCoByFieldAndPage(tenTruong, tuKhoa, page, itemsPerPage);
        }
    }
}
