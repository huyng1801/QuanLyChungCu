using BUS;
using DTO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChungCu
{
    public partial class panelHoaDon : UserControl
    {
        public panelHoaDon()
        {
            InitializeComponent();
        }
        private int selectedRowIndex = -1;
        private void loadData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRowIndex = dataGridView1.SelectedRows[0].Index;
            }
            List<HoaDonDTO> ds = HoaDonBUS.GetAllHoaDon();
            dataGridView1.DataSource = ds;
            if (selectedRowIndex >= 0 && selectedRowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[selectedRowIndex].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = selectedRowIndex;
            }
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["MaHopDong"].HeaderText = "Mã hợp đồng";
            dataGridView1.Columns["NgayTao"].HeaderText = "Ngày tạo";
            dataGridView1.Columns["NgayTT"].HeaderText = "Ngày thanh toán";
            dataGridView1.Columns["TongTien"].HeaderText = "Tổng tiền";
         
        }
        private void loadSubData(string maHD)
        {
            List<ChiTietHoaDonDTO> ds = ChiTietHoaDonBUS.GetChiTietHoaDonByMaHD(maHD);
            dataGridView2.DataSource = ds;
            dataGridView2.Columns["MaChiTietHoaDon"].HeaderText = "Id";
            dataGridView2.Columns["MaHD"].HeaderText = "Mã hóa đơn";
            dataGridView2.Columns["MaDV"].HeaderText = "Mã dịch vụ";
            dataGridView2.Columns["HeSo"].HeaderText = "Hệ số";
        }
        private void panelHoaDon_Load(object sender, EventArgs e)
        {
            loadData();
            loadComboBox();
        }
        private void loadComboBox()
        {
            // Điền dữ liệu cho ComboBox KhachHang
            List<HopDongDTO> dsHopDong = HopDongBUS.GetAllHopDong();
            cboMaHopDong.DataSource = dsHopDong;
            cboMaHopDong.DisplayMember = "MaHD"; // Hiển thị tên khách hàng
            cboMaHopDong.ValueMember = "MaHD";   

            // Điền dữ liệu cho ComboBox DichVu
            List<DichVuDTO> dsDichVu = DichVuBUS.GetAllDichVu();
            cboDichVu.DataSource = dsDichVu;
            cboDichVu.DisplayMember = "TenDV"; // Hiển thị tên dịch vụ
            cboDichVu.ValueMember = "MaDV";   // Lấy mã dịch vụ

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row != null)
                {
                    txtMaHD.Text = row.Cells[0].Value.ToString();
                    txtMaNhanVien.Text = row.Cells[1].Value.ToString();
                    cboMaHopDong.Text = row.Cells[2].Value.ToString();
                    
                    dateTimePickerNgayTao.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                    txtNgayThanhToan.Text = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : "";
                    txtTongTien.Text = row.Cells[5].Value.ToString();
                    loadSubData(txtMaHD.Text);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện 
                string maHD = txtMaHD.Text;
                string maHopDong = cboMaHopDong.SelectedValue.ToString();
                string maNhanVien = Session.nhanVien.MaNV;
      
                HoaDonDTO existingHoaDon= HoaDonBUS.GetHoaDonByMaHD(maHD);

                if (existingHoaDon != null)
                {
                    MessageBox.Show("Mã hóa đơn đã tồn tại!");
                    return;
                }
                if (maHD != string.Empty && maHopDong != string.Empty && maNhanVien != string.Empty)
                {
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    HoaDonDTO hoaDon = new HoaDonDTO
                    {
                        MaHD = maHD,
                        MaHopDong = maHopDong,
                        MaNV = maNhanVien
                    };

             

                    if (HoaDonBUS.InsertHoaDon(hoaDon) > 0)
                    {
                        MessageBox.Show("Thêm hóa đơn thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Thêm hóa đơn thất bại!");
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maHD = txtMaHD.Text;

            if (!string.IsNullOrWhiteSpace(maHD))
            {
                // Kiểm tra xem khách hàng có tồn tại hay không
                HoaDonDTO hoaDon = HoaDonBUS.GetHoaDonByMaHD(maHD);

                if (hoaDon != null)
                {
                    // Khách hàng tồn tại, bạn có thể xóa
                    if (HoaDonBUS.DeleteHoaDon(maHD) > 0)
                    {

                        MessageBox.Show("Xóa hóa đơn thành công!");
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Xóa hóa đơn không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Hóa đơn không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
            }
        }
    
        private void btnThemCT_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện 
                string maHD = txtMaHD.Text;
                string maDV = cboDichVu.SelectedValue.ToString();
                decimal heSo = decimal.Parse(txtHeSo.Text);
                HoaDonDTO existingHoaDon = HoaDonBUS.GetHoaDonByMaHD(maHD);

                if (existingHoaDon == null)
                {
                    MessageBox.Show("Mã hóa đơn không tồn tại!");
                    return;
                }
                if (maHD != string.Empty && maDV != string.Empty)
                {
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    ChiTietHoaDonDTO chiTietHoaDon = new ChiTietHoaDonDTO
                    {
                        MaHD = maHD,
                        MaDV = maDV,
                        HeSo = heSo
                    };



                    if (ChiTietHoaDonBUS.InsertChiTietHoaDon(chiTietHoaDon) > 0)
                    {
                        MessageBox.Show("Thêm chi tiết hóa đơn thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                        loadData();
                       
                        loadSubData(maHD);
                    }
                    else
                    {
                        MessageBox.Show("Thêm chi tiết hóa đơn thất bại!");
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi dữ liệu không hợp lệ!");
            }
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            try
            {
                string maChiTietHoaDon = txtId.Text;

                if (!string.IsNullOrWhiteSpace(maChiTietHoaDon))
                {

                    // Khách hàng tồn tại, bạn có thể xóa
                    if (ChiTietHoaDonBUS.DeleteChiTietHoaDonByMaHD(maChiTietHoaDon) > 0)
                    {

                        MessageBox.Show("Xóa chi tiết hóa đơn thành công!");
                        loadData();
                        loadSubData(txtMaHD.Text);
                    }
                    else
                    {
                        MessageBox.Show("Xóa chi tiết hóa đơn không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Chi tiết hóa đơn không tồn tại!");
                }

            }
catch(Exception ex)
            {
                MessageBox.Show("Vui lòng chọn chi tiết cần xóa!");
            }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                if (row != null)
                {
                    txtId.Text = row.Cells[0].Value.ToString();
                
                    txtHeSo.Text = row.Cells[3].Value.ToString();
                  
                }
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string maHD = txtMaHD.Text;

            if (!string.IsNullOrWhiteSpace(maHD))
            {
                // Kiểm tra xem khách hàng có tồn tại hay không
                HoaDonDTO hoaDon = HoaDonBUS.GetHoaDonByMaHD(maHD);

                if (hoaDon != null)
                {
                    if (hoaDon.NgayTT == null)
                    {
                        HoaDonBUS.UpdateHoaDon(hoaDon);
                        MessageBox.Show("Thanh toán thành công!");
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Thanh toán không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!");
                }
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            // Lấy thông tin hóa đơn từ dataGridView1
            string maKH = KhachHangBUS.GetKhachHangByMaKH(HopDongBUS.GetHopDongByMaHD(cboMaHopDong.Text).MaKH).TenKH; // Tên khách hàng
            string ngayTao = dateTimePickerNgayTao.Text;
            string canho = HopDongBUS.GetHopDongByMaHD(cboMaHopDong.Text).MaCH;
            string tongTien = txtTongTien.Text;

            // Tạo một tệp Excel mới
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("HoaDon");

            // Thêm thông tin công ty ở góc trên bên trái
            IRow companyInfoRow1 = sheet.CreateRow(0);
            companyInfoRow1.CreateCell(0).SetCellValue("CÔNG TY CỔ PHẦN TẬP ĐOÀN BẤT ĐỘNG SẢN T&T");

            IRow companyInfoRow2 = sheet.CreateRow(1);
            companyInfoRow2.CreateCell(0).SetCellValue("Tầng 2, Tòa nhà Vinfor, 127 Lò Đúc, Quận Hai Bà Trưng, Hà Nội");

            // Dòng trống
            sheet.CreateRow(2);

            // Thêm tiêu đề hóa đơn
            IRow invoiceTitleRow = sheet.CreateRow(3);
            invoiceTitleRow.CreateCell(0).SetCellValue("HÓA ĐƠN TIỀN PHÒNG");

            // Thêm ngày tạo hóa đơn
            IRow invoiceDateRow = sheet.CreateRow(4);
            invoiceDateRow.CreateCell(0).SetCellValue(ngayTao);

            // Dòng trống
            sheet.CreateRow(5);

            // Tạo hàng tiêu đề
            IRow headerRow = sheet.CreateRow(6);
            headerRow.CreateCell(0).SetCellValue("Khách hàng");
            headerRow.CreateCell(1).SetCellValue(maKH);
            IRow seventRow = sheet.CreateRow(7);
            seventRow.CreateCell(0).SetCellValue("Căn hộ");
            seventRow.CreateCell(1).SetCellValue(canho);

            // Tạo dòng trống
            sheet.CreateRow(8);

            // Tạo hàng tiêu đề cho chi tiết hóa đơn
            IRow detailsHeaderRow = sheet.CreateRow(9);
            detailsHeaderRow.CreateCell(0).SetCellValue("STT");
            detailsHeaderRow.CreateCell(1).SetCellValue("Tên dịch vụ");
            detailsHeaderRow.CreateCell(2).SetCellValue("Hệ số");
            detailsHeaderRow.CreateCell(3).SetCellValue("Đơn giá");
            detailsHeaderRow.CreateCell(4).SetCellValue("Thành tiền");
            int i = 0;
            // Lấy thông tin chi tiết hóa đơn từ dataGridView2
            DataTable dt = new DataTable();
            if (dataGridView2.DataSource is List<ChiTietHoaDonDTO> chiTietHoaDonList)
            {
                dt.Columns.Add("STT");
                dt.Columns.Add("TenDV");
                dt.Columns.Add("HeSo");
                dt.Columns.Add("GiaTien");
                dt.Columns.Add("ThanhTien");
                foreach (var chiTiet in chiTietHoaDonList)
                {
                    i++;
                    DataRow row = dt.NewRow();
                    row["STT"] = i;
                    row["TenDV"] = DichVuBUS.GetDichVuByMaDV(chiTiet.MaDV).TenDV;
                    row["HeSo"] = chiTiet.HeSo;
                    row["GiaTien"] = DichVuBUS.GetDichVuByMaDV(chiTiet.MaDV).GiaTien;
                    row["ThanhTien"] = DichVuBUS.GetDichVuByMaDV(chiTiet.MaDV).GiaTien * chiTiet.HeSo;
                    dt.Rows.Add(row);
                }
            }

            // Thêm chi tiết hóa đơn vào tệp Excel
            int rowNumber = 10;
            foreach (DataRow row in dt.Rows)
            {
                IRow detailRow = sheet.CreateRow(rowNumber++);
                detailRow.CreateCell(0).SetCellValue(row["STT"].ToString());
                detailRow.CreateCell(1).SetCellValue(row["TenDV"].ToString());
                detailRow.CreateCell(2).SetCellValue(row["HeSo"].ToString());
                detailRow.CreateCell(3).SetCellValue(row["GiaTien"].ToString());
                detailRow.CreateCell(4).SetCellValue(row["ThanhTien"].ToString());
            }
            rowNumber++;
            IRow tt = sheet.CreateRow(rowNumber);
            tt.CreateCell(0).SetCellValue("Tổng tiền");
            tt.CreateCell(1).SetCellValue(tongTien);
            // Cho phép người dùng chọn nơi lưu tệp và đặt tên
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Chọn nơi lưu tệp Excel";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    workbook.Write(fs);
                }
                MessageBox.Show("In hóa đơn thành công!");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nơi lưu tệp Excel!");
            }
        }

        private void clear()
        {
            txtMaHD.Text = string.Empty;
            txtMaNhanVien.Text = string.Empty;
            dateTimePickerNgayTao.ResetText();
            txtNgayThanhToan.Text = string.Empty;
            txtTongTien.Text = string.Empty;
            cboMaHopDong.SelectedIndex = -1;
        }
        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            clear();
        }
  
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDataSearch();
        }
        private void LoadDataSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            List<HoaDonDTO> ds = HoaDonBUS.SearchHoaDonByField(tenTruong, giaTri);
            dataGridView1.DataSource = ds;
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dataGridView1.Columns["NgayTao"].HeaderText = "Ngày tạo";
            dataGridView1.Columns["NgayTT"].HeaderText = "Ngày thanh toán";
            dataGridView1.Columns["TongTien"].HeaderText = "Tổng tiền";
        }
        private void btnHuy_Click(object sender, EventArgs e)
        {
            loadData();
        }
    }
}
