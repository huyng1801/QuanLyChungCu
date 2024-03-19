using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDongDTO
    {
        public string MaHD { get; set; }
        public string MaKH { get; set; }
        public string MaNV { get; set; }
        public string MaCH { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayThue { get; set; }
        public DateTime? NgayTra { get; set; }
        public decimal TienCoc { get; set; }
        public decimal TongTien { get; set; }
        public string Trangthai { get; set; }
    }
}
