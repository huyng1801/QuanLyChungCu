using BUS;
using DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
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
    public partial class panelKhachHang : UserControl
    {
        public panelKhachHang()
        {
            InitializeComponent();
        }
        private int currentPage = 1; // Trang hiện tại
        private int itemsPerPage = 5; // Số dòng trên mỗi trang
        private int totalPageCount; // Tổng số trang
        private int flag = 0;


        private void LoadData(int page)
        {
            CalculateTotalPages();
            List<KhachHangDTO> ds = KhachHangBUS.GetKhachHangByPage(page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dataGridView1.Columns["TenKH"].HeaderText = "Tên khách hàng";
            dataGridView1.Columns["CCCD"].HeaderText = "CCCD";
            dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dataGridView1.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dataGridView1.Columns["Sdt"].HeaderText = "Số điện thoại";
            dataGridView1.Columns["Email"].HeaderText = "Email";
        }
        private void CalculateTotalPages()
        {
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = KhachHangBUS.GetAllKhachHang().Count;

            // Tính tổng số trang
            totalPageCount = (int)Math.Ceiling((double)totalRowCount / itemsPerPage);
        }

  
        private void btnDau_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {
                currentPage = 1; // Chuyển đến trang đầu tiên
                LoadData(currentPage);
            }
            else
            {
                currentPage = 1; // Chuyển đến trang đầu tiên
                LoadDataSearch(currentPage);
            }
        }

        private void btnCuoi_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {
                currentPage = totalPageCount; // Chuyển đến trang cuối cùng
                LoadData(currentPage);
            }
            else
            {
                currentPage = totalPageCount; // Chuyển đến trang cuối cùng
                LoadDataSearch(currentPage);
            }

        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {
                if (currentPage > 1)
                {
                    currentPage--; // Chuyển đến trang trước đó
                    LoadData(currentPage);
                }
            }
            else
            {
                if (currentPage > 1)
                {
                    currentPage--; // Chuyển đến trang trước đó
                    LoadDataSearch(currentPage);
                }
            }

        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (flag == 0)
            {
                if (currentPage < totalPageCount)
                {
                    currentPage++; // Chuyển đến trang kế tiếp
                    LoadData(currentPage);
                }
            }
            else
            {
                if (currentPage < totalPageCount)
                {
                    currentPage++; // Chuyển đến trang trước đó
                    LoadDataSearch(currentPage);
                }
            }

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                string maKH = txtMaKH.Text;
                string tenKH = txtTenKH.Text;
                string CCCD = txtCCCD.Text;
                DateTime ngaySinh = dateTimePickerNgaySinh.Value;
                string diaChi = txtDiaChi.Text;
                string soDienThoai = txtSoDienThoai.Text;
                string email = txtEmail.Text;
                KhachHangDTO existingKhachHang = KhachHangBUS.GetKhachHangByMaKH(maKH);

                if (existingKhachHang != null)
                {
                    MessageBox.Show("Mã khách hàng đã tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maKH) && !string.IsNullOrEmpty(tenKH) && !string.IsNullOrEmpty(CCCD) && !string.IsNullOrEmpty(diaChi) && !string.IsNullOrEmpty(soDienThoai) && !string.IsNullOrEmpty(email))
                {
                    KhachHangDTO khachHang = new KhachHangDTO
                    {
                        MaKH = maKH,
                        TenKH = tenKH,
                        CCCD = CCCD,
                        NgaySinh = ngaySinh,
                        DiaChi = diaChi,
                        Sdt = soDienThoai,
                        Email = email
                    };

                    if (KhachHangBUS.InsertKhachHang(khachHang) > 0)
                    {
                        MessageBox.Show("Đã thêm khách hàng!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Thêm khách hàng không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được bỏ trống!");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (row != null)
                {
                    txtMaKH.Text = row.Cells[0].Value.ToString();
                    txtTenKH.Text = row.Cells[1].Value.ToString();
                    txtCCCD.Text = row.Cells[2].Value.ToString();
                    dateTimePickerNgaySinh.Value = DateTime.Parse(row.Cells[3].Value.ToString());
                    txtDiaChi.Text = row.Cells[4].Value.ToString();
                    txtSoDienThoai.Text = row.Cells[5].Value.ToString();
                    txtEmail.Text = row.Cells[6].Value.ToString();
                }
            }
        }
        private void ClearData()
        {
            txtMaKH.Text = string.Empty;
            txtTenKH.Text = string.Empty;
            txtCCCD.Text = string.Empty;
            dateTimePickerNgaySinh.ResetText(); 
            txtDiaChi.Text = string.Empty;
            txtSoDienThoai.Text = string.Empty; // Chọn mục đầu tiên hoặc giá trị không hợp lệ
            txtEmail.Text = string.Empty;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            string maKH = txtMaKH.Text;

            if (!string.IsNullOrWhiteSpace(maKH))
            {
                // Kiểm tra xem khách hàng có tồn tại hay không
                KhachHangDTO khachHang = KhachHangBUS.GetKhachHangByMaKH(maKH);

                if (khachHang != null)
                {
                    // Khách hàng tồn tại, bạn có thể xóa
                    if (KhachHangBUS.DeleteKhachHang(maKH) > 0)
                    {

                        MessageBox.Show("Xóa khách hàng thành công!");
                        LoadData(currentPage);
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa khách hàng không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Khách hàng không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn khách hàng cần xóa!");
            }
        }

        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                string maKH = txtMaKH.Text;
                string tenKH = txtTenKH.Text;
                string CCCD = txtCCCD.Text;
                DateTime ngaySinh = dateTimePickerNgaySinh.Value;
                string diaChi = txtDiaChi.Text;
                string soDienThoai = txtSoDienThoai.Text;
                string email = txtEmail.Text;
                KhachHangDTO existingKhachHang = KhachHangBUS.GetKhachHangByMaKH(maKH);

                if (existingKhachHang == null)
                {
                    MessageBox.Show("Mã khách hàng không tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maKH) && !string.IsNullOrEmpty(tenKH) && !string.IsNullOrEmpty(CCCD) && !string.IsNullOrEmpty(diaChi) && !string.IsNullOrEmpty(soDienThoai) && !string.IsNullOrEmpty(email))
                {
                    KhachHangDTO khachHang = new KhachHangDTO
                    {
                        MaKH = maKH,
                        TenKH = tenKH,
                        CCCD = CCCD,
                        NgaySinh = ngaySinh,
                        DiaChi = diaChi,
                        Sdt = soDienThoai,
                        Email = email
                    };

                    if (KhachHangBUS.UpdateKhachHang(khachHang) > 0)
                    {
                        MessageBox.Show("Đã sửa khách hàng!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Sửa khách hàng không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Thông tin không được bỏ trống!");
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void LoadDataSearch(int page)
        {
            CalculateTotalPagesSearch();
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            List<KhachHangDTO> ds = KhachHangBUS.SearchKhachHangByFieldAndPage(tenTruong, giaTri, page, itemsPerPage);
            dataGridView1.DataSource = ds;
            dataGridView1.Columns["MaKH"].HeaderText = "Mã khách hàng";
            dataGridView1.Columns["TenKH"].HeaderText = "Tên khách hàng";
            dataGridView1.Columns["CCCD"].HeaderText = "CCCD";
            dataGridView1.Columns["NgaySinh"].HeaderText = "Ngày sinh";
            dataGridView1.Columns["DiaChi"].HeaderText = "Địa chỉ";
            dataGridView1.Columns["Sdt"].HeaderText = "Số điện thoại";
            dataGridView1.Columns["Email"].HeaderText = "Email";
        }
        private void CalculateTotalPagesSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = KhachHangBUS.SearchKhachHangByField(tenTruong, giaTri).Count;

            // Tính tổng số trang
            totalPageCount = (int)Math.Ceiling((double)totalRowCount / itemsPerPage);
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            flag = 1;
            currentPage = 1;
            LoadDataSearch(1);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            flag = 0;
            LoadData(currentPage);
            CalculateTotalPages();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel Files|*.xls";
                saveFileDialog.Title = "Save as Excel File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    if (string.IsNullOrEmpty(filePath))
                    {
                        MessageBox.Show("Please select a location and provide a file name!");
                        return;
                    }

                    HSSFWorkbook workbook = new HSSFWorkbook();
                    ISheet worksheet = workbook.CreateSheet("Khách hàng");

                    int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedRowIndex];

                    IRow headerRow = worksheet.CreateRow(0);
                    for (int i = 0; i < selectedRow.Cells.Count; i++)
                    {
                        headerRow.CreateCell(i).SetCellValue(dataGridView1.Columns[i].HeaderText);
                    }

                    IRow dataRow = worksheet.CreateRow(1);
                    for (int i = 0; i < selectedRow.Cells.Count; i++)
                    {
                        dataRow.CreateCell(i).SetCellValue(selectedRow.Cells[i].Value.ToString());
                    }

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        workbook.Write(fileStream);
                    }

                    MessageBox.Show("In đối tượng thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void panelKhachHang_Load(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData(currentPage);
            CalculateTotalPages();
        }
    }
}
