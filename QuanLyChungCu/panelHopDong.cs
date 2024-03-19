using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyChungCu
{
    public partial class panelHopDong : UserControl
    {
        private int selectedRowIndex = -1;

        public panelHopDong()
        {
            InitializeComponent();
        }
        private void loadData()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedRowIndex = dataGridView1.SelectedRows[0].Index;
            }
           
            List<HopDongDTO> ds = HopDongBUS.GetAllHopDong();
            dataGridView1.DataSource = ds;
            if (selectedRowIndex >= 0 && selectedRowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[selectedRowIndex].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = selectedRowIndex;
            }
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hợp đồng";
            dataGridView1.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["MaCH"].HeaderText = "Mã căn hộ";
            dataGridView1.Columns["NgayTao"].HeaderText = "Ngày tạo";
            dataGridView1.Columns["NgayThue"].HeaderText = "Ngày thuê";
            dataGridView1.Columns["NgayTra"].HeaderText = "Ngày trả";
            dataGridView1.Columns["TienCoc"].HeaderText = "Tiền cọc";
            dataGridView1.Columns["TongTien"].HeaderText = "Tổng tiền";
            dataGridView1.Columns["Trangthai"].HeaderText = "Trạng thái";
        }
        private void panelHopDong_Load(object sender, EventArgs e)
        {
            loadData();
            loadComboBox();
        }
        private void loadComboBox()
        {
            // Điền dữ liệu cho ComboBox KhachHang
            List<KhachHangDTO> dsKhachHang = KhachHangBUS.GetAllKhachHang();
            cboMaKH.DataSource = dsKhachHang;
            cboMaKH.DisplayMember = "TenKH"; 
            cboMaKH.ValueMember = "MaKH";   
            List<CanHoDTO> dsCanHo = CanHoBUS.GetAllCanHo();
            cboMaCanHo.Items.Clear();
            foreach (CanHoDTO canHo in dsCanHo)
            {
                cboMaCanHo.Items.Add(canHo.MaCH);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row != null)
                {
                    txtMaHD.Text = row.Cells[0].Value.ToString();
                    cboMaKH.Text = KhachHangBUS.GetKhachHangByMaKH(row.Cells[1].Value.ToString()).TenKH.ToString();
                    txtMaNhanVien.Text =  NhanVienBUS.GetNhanVienByMaNV(row.Cells[2].Value.ToString()).TenNV.ToString();
                    cboMaCanHo.Text = row.Cells[3].Value.ToString();
                    dateTimePickerNgayTao.Value = DateTime.Parse(row.Cells[4].Value.ToString());
                    dateTimePickerNgayThue.Value = DateTime.Parse(row.Cells[5].Value.ToString());
                    dateTimePickerNgayTra.Value = DateTime.Parse(row.Cells[6].Value.ToString());
                    txtTienCoc.Text = row.Cells[7].Value.ToString();
        
                    txtTongTien.Text = row.Cells[8].Value.ToString();
                    cboTrangThai.Text = row.Cells[9].Value.ToString();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện 
                string maHD = txtMaHD.Text;
                string maKH = cboMaKH.SelectedValue.ToString();
                string maNhanVien = Session.nhanVien.MaNV;
                string maCanHo = cboMaCanHo.Text;
                DateTime ngayTao = dateTimePickerNgayTao.Value;
                DateTime ngayThue = dateTimePickerNgayThue.Value;
                DateTime ngayTra = dateTimePickerNgayTra.Value;
                decimal tienCoc = Convert.ToDecimal(txtTienCoc.Text);
   
                decimal tongTien = Convert.ToDecimal(txtTongTien.Text);
                string trangThai = cboTrangThai.Text;
                HopDongDTO existingHopDong = HopDongBUS.GetHopDongByMaHD(maHD);

                if (existingHopDong != null)
                {
                    MessageBox.Show("Mã hợp đồng đã tồn tại!");
                    return;
                }
                if (maHD!= string.Empty && maKH != string.Empty && maNhanVien != string.Empty && maCanHo != string.Empty && trangThai != string.Empty)
                {
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    HopDongDTO hopDong = new HopDongDTO
                    {
                        MaHD = maHD,
                        MaKH = maKH,
                        MaNV = maNhanVien,
                        MaCH = maCanHo,
                        NgayThue = ngayThue,
                        NgayTra = ngayTra,
                        TienCoc = tienCoc,
                      
                        TongTien = tongTien,
                        Trangthai = trangThai
                    };

                    // Gọi phương thức trong lớp BUS để thêm hợp đồng
                    int result = HopDongBUS.InsertHopDong(hopDong);

                    if (result > 0)
                    {
                        MessageBox.Show("Thêm hợp đồng thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                    
                        loadData();
                     

                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Thêm hợp đồng thất bại!");
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
                HopDongDTO hopDong = HopDongBUS.GetHopDongByMaHD(maHD);

                if (hopDong != null)
                {
                    // Khách hàng tồn tại, bạn có thể xóa
                    if (HopDongBUS.DeleteHopDong(maHD) > 0)
                    {

                        MessageBox.Show("Xóa hợp đồng thành công!");
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Xóa hợp đồng không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Hợp đồng không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần xóa!");
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện
                string maHD = txtMaHD.Text;
                string maKH = cboMaKH.SelectedValue.ToString();
                string maNhanVien = Session.nhanVien.MaNV;
                string maCanHo = cboMaCanHo.Text;
                DateTime ngayTao = dateTimePickerNgayTao.Value;
                DateTime ngayThue = dateTimePickerNgayThue.Value;
                DateTime ngayTra = dateTimePickerNgayTra.Value;
                decimal tienCoc = Convert.ToDecimal(txtTienCoc.Text);
                decimal tongTien = Convert.ToDecimal(txtTongTien.Text);
                string trangThai = cboTrangThai.Text;
                HopDongDTO existingHopDong = HopDongBUS.GetHopDongByMaHD(maHD);

                if (existingHopDong == null)
                {
                    MessageBox.Show("Hợp đồng không tồn tại!");
                    return;
                }
                if (maHD != string.Empty && maKH != string.Empty && maNhanVien != string.Empty && maCanHo != string.Empty && trangThai != string.Empty)
                {
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    HopDongDTO hopDong = new HopDongDTO
                    {
                        MaHD = maHD,
                        MaKH = maKH,
                        MaNV = maNhanVien,
                        MaCH = maCanHo,
                        NgayThue = ngayThue,
                        NgayTra = ngayTra,
                        TienCoc = tienCoc,
               
                        TongTien = tongTien,
                        Trangthai = trangThai
                    };

                    // Gọi phương thức trong lớp BUS để thêm hợp đồng
                    int result = HopDongBUS.UpdateHopDong(hopDong);

                    if (result > 0)
                    {
                        MessageBox.Show("Sửa hợp đồng thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Sửa hợp đồng thất bại!");
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
        private void clear()
        {
            txtMaHD.Text = string.Empty;
            cboMaKH.SelectedIndex = -1;
            cboMaCanHo.SelectedIndex = -1;
            txtMaNhanVien.Text = string.Empty;
            cboMaCanHo.SelectedIndex = -1;
            dateTimePickerNgayTao.ResetText();
            dateTimePickerNgayThue.ResetText();
            dateTimePickerNgayTra.ResetText();
            txtTienCoc.Text = string.Empty;
            txtTongTien.Text = string.Empty;
            cboTrangThai.SelectedIndex = -1;
        }
        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            clear();
        }
        private void LoadDataSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            List<HopDongDTO> ds = HopDongBUS.SearchHopDongByField(tenTruong, giaTri);
            dataGridView1.DataSource = ds;
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hợp đồng";
            dataGridView1.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["MaCH"].HeaderText = "Mã căn hộ";
            dataGridView1.Columns["NgayTao"].HeaderText = "Ngày tạo";
            dataGridView1.Columns["NgayThue"].HeaderText = "Ngày thuê";
            dataGridView1.Columns["NgayTra"].HeaderText = "Ngày trả";
            dataGridView1.Columns["TienCoc"].HeaderText = "Tiền cọc";
            dataGridView1.Columns["TongTien"].HeaderText = "Tổng tiền";
            dataGridView1.Columns["Trangthai"].HeaderText = "Trạng thái";
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDataSearch();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            loadData();
        }
        
        private void cboMaCanHo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cboMaCanHo.SelectedItem != null)
            {
                string maCanHo = cboMaCanHo.Text;
                CanHoDTO canHoDTO = CanHoBUS.GetCanHoByMaCH(maCanHo);
                txtTongTien.Text = canHoDTO.GiaThue.ToString();
            }
        }

        private void btnThanhLy_Click(object sender, EventArgs e)
        {
            string maHD = txtMaHD.Text;

            if (!string.IsNullOrWhiteSpace(maHD))
            {
                // Kiểm tra xem khách hàng có tồn tại hay không
                HopDongDTO hopDong = HopDongBUS.GetHopDongByMaHD(maHD);

                if (hopDong != null)
                {
                    // Khách hàng tồn tại, bạn có thể xóa
                    if (HopDongBUS.ThanhLyHopDong(maHD) > 0)
                    {

                        MessageBox.Show("Thanh lý hợp đồng thành công!");
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Thanh lý hợp đồng thành không công!");
                    }
                }
                else
                {
                    MessageBox.Show("Hợp đồng không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn hợp đồng cần thanh lý!");
            }
        }
    }
}
