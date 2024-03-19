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
    public partial class panelBienBanBoiThuong : UserControl
    {
        public panelBienBanBoiThuong()
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
            List<BienBanBoiThuongDTO> ds = BienBanBoiThuongBUS.GetAllBienBanBoiThuong();
            dataGridView1.DataSource = ds;
            if (selectedRowIndex >= 0 && selectedRowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.Rows[selectedRowIndex].Selected = true;
                dataGridView1.FirstDisplayedScrollingRowIndex = selectedRowIndex;
    
            }
            dataGridView1.Columns["MaBB"].HeaderText = "Mã biên bản";
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hợp đồng";
            dataGridView1.Columns["MaNV"].HeaderText = "Mã nhân viên";
            dataGridView1.Columns["NgayTao"].HeaderText = "Ngày tạo";
           
        }
        private void loadSubData(string maBB)
        {
            List<ChiTietBienBanBoiThuongDTO> ds = ChiTietBienBanBoiThuongBUS.GetChiTietBienBanBoiThuongByMaBB(maBB);
            dataGridView2.DataSource = ds;
            dataGridView2.Columns["Id"].HeaderText = "Id";
            dataGridView2.Columns["MaBB"].HeaderText = "Mã biên bản";
            dataGridView2.Columns["MaSC"].HeaderText = "Mã sự cố";
            dataGridView2.Columns["Soluong"].HeaderText = "Số lượng";
        }
   
        private void loadComboBox()
        {
            // Điền dữ liệu cho ComboBox KhachHang
            List<HopDongDTO> dsHopDong = HopDongBUS.GetAllHopDong();
            cboHopDong.DataSource = dsHopDong;
            cboHopDong.DisplayMember = "MaHD"; 
            cboHopDong.ValueMember = "MaHD";   

            // Điền dữ liệu cho ComboBox DichVu
            List<SuCoDTO> dsSuCo = SuCoBUS.GetAllSuCo();
            cboSuCo.DataSource = dsSuCo;
            cboSuCo.DisplayMember = "TenSC"; // Hiển thị tên dịch vụ
            cboSuCo.ValueMember = "MaSC";   // Lấy mã dịch vụ

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row != null)
                {
                    txtMaBB.Text = row.Cells[0].Value.ToString();
                    txtMaNhanVien.Text = row.Cells[2].Value.ToString();
                    cboHopDong.Text = row.Cells[1].Value.ToString();

                    dateTimePickerNgayTao.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                  
                    loadSubData(txtMaBB.Text);
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện 
                string maBB = txtMaBB.Text;
                string maHD = cboHopDong.Text;
                string maNhanVien = Session.nhanVien.MaNV;

                BienBanBoiThuongDTO existingBienBan = BienBanBoiThuongBUS.GetBienBanBoiThuongByMaBB(maBB);

                if (existingBienBan != null)
                {
                    MessageBox.Show("Mã biên bản đã tồn tại!");
                    return;
                }
                if (maHD != string.Empty && maBB != string.Empty && maNhanVien != string.Empty)
                {
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    BienBanBoiThuongDTO bienBan = new BienBanBoiThuongDTO
                    {
                        MaHD = maHD,
                        MaBB = maBB,
                        MaNV = maNhanVien
                    };



                    if (BienBanBoiThuongBUS.InsertBienBanBoiThuong(bienBan) > 0)
                    {
                        MessageBox.Show("Thêm biên bản bồi thường thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Thêm biên bản bồi thường thất bại!");
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
            string maBB = txtMaBB.Text;

            if (!string.IsNullOrWhiteSpace(maBB))
            {
                // Kiểm tra xem khách hàng có tồn tại hay không
                BienBanBoiThuongDTO bienBan = BienBanBoiThuongBUS.GetBienBanBoiThuongByMaBB(maBB);

                if (bienBan != null)
                {
                    // Khách hàng tồn tại, bạn có thể xóa
                    if (BienBanBoiThuongBUS.DeleteBienBanBoiThuong(maBB) > 0)
                    {

                        MessageBox.Show("Xóa biên bản thành công!");
                        loadData();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Xóa biên bản không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Biên bản không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn biên bản cần xóa!");
            }
        }

        private void btnThemCT_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các controls trên giao diện 
                string maBB = txtMaBB.Text;
                string suCo = cboSuCo.SelectedValue.ToString();
               
         
                if (maBB != string.Empty && suCo != string.Empty && txtSoLuong.Text != string.Empty)
                {
                    int soLuong = int.Parse(txtSoLuong.Text);
                    // Tạo đối tượng HopDongDTO với thông tin vừa lấy
                    ChiTietBienBanBoiThuongDTO chiTietBB = new ChiTietBienBanBoiThuongDTO
                    {
                        MaBB = maBB,
                        MaSC = suCo,
                        Soluong = soLuong
                    };



                    if (ChiTietBienBanBoiThuongBUS.InsertChiTietBienBanBoiThuong(chiTietBB) > 0)
                    {
                        MessageBox.Show("Thêm chi tiết biên bản bồi thường thành công!");
                        // Sau khi thêm thành công, bạn có thể gọi phương thức loadData để cập nhật danh sách hiển thị trên DataGridView
                        loadData();
                        loadSubData(maBB);
                    }
                    else
                    {
                        MessageBox.Show("Thêm chi tiết biên bản thất bại!");
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được để trống!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
            }
        }

        private void btnXoaCT_Click(object sender, EventArgs e)
        {
            try
            {
                string id = txtId.Text;

                if (!string.IsNullOrWhiteSpace(id))
                {

                    // Khách hàng tồn tại, bạn có thể xóa
                    if (ChiTietBienBanBoiThuongBUS.DeleteChiTietBienBanBoiThuongByMaBB(int.Parse(id)) > 0)
                    {

                        MessageBox.Show("Xóa chi tiết biên bản thành công!");
                        loadData();
                        loadSubData(txtMaBB.Text);
                    }
                    else
                    {
                        MessageBox.Show("Xóa chi tiết biên bản không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Chi tiết biên bản không tồn tại!");
                }

            }
            catch (Exception ex)
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
                    cboSuCo.Text = SuCoBUS.GetSuCoByMaSC(row.Cells[2].Value.ToString()).TenSC;

                    txtSoLuong.Text = row.Cells[3].Value.ToString();

                }
            }
        }
        private void panelBienBanBoiThuong_Load(object sender, EventArgs e)
        {
            loadData();
            loadComboBox();
        }
        private void clear()
        {
            txtMaBB.Text = string.Empty;
            txtMaNhanVien.Text = string.Empty;
            cboHopDong.SelectedItem = -1;
            
        }
        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}
