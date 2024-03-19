using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HoaDonDTO
    {
        public string MaHD { get; set; }
        public string MaNV { get; set; }
        public string MaHopDong { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayTT { get; set; }
        public decimal TongTien { get; set; }
    }
}
