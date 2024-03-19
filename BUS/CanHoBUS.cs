using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CanHoBUS
    {
        public static List<CanHoDTO> GetAllCanHo()
        {
           return CanHoDAO.GetAllCanHo();
        }
        public static List<CanHoDTO> GetCanHoByPage(int page, int itemsPerPage)
        {
            return CanHoDAO.GetCanHoByPage(page, itemsPerPage);
        }

        public static int InsertCanHo(CanHoDTO canHo)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in CanHoDAO
            return DAO.CanHoDAO.InsertCanHo(canHo);
        }

        public static int UpdateCanHo(CanHoDTO canHo)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in CanHoDAO
            return DAO.CanHoDAO.UpdateCanHo(canHo);
        }

        public static int DeleteCanHo(string maCH)
        {
            // You can add any additional validation or business logic here

            // Call the corresponding method in CanHoDAO
            return DAO.CanHoDAO.DeleteCanHo(maCH);
        }

        public static CanHoDTO GetCanHoByMaCH(string maCH)
        {
            // Call the corresponding method in CanHoDAO
            return DAO.CanHoDAO.GetCanHoByMaCH(maCH);
        }
        public static List<CanHoDTO> SearchCanHoByField(string fieldName, string keyword)
        {
            return CanHoDAO.SearchCanHoByField(fieldName, keyword);
        }
        public static List<CanHoDTO> SearchCanHoByFieldAndPage(string fieldName, string keyword, int page, int itemsPerPage)
        {
            
            return CanHoDAO.SearchCanHoByFieldAndPage(fieldName, keyword, page, itemsPerPage);
        }
    }
}
