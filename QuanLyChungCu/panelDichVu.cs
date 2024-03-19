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
    public partial class panelDichVu : UserControl
    {
        public panelDichVu()
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
            List<DichVuDTO> ds = DichVuBUS.GetDichVuByPage(page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaDV"].HeaderText = "Mã dịch vụ";
            dataGridView1.Columns["TenDV"].HeaderText = "Dịch vụ";
            dataGridView1.Columns["GiaTien"].HeaderText = "Giá tiền";
        }
        private void CalculateTotalPages()
        {
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = DichVuBUS.GetAllDichVu().Count;

            // Tính tổng số trang
            totalPageCount = (int)Math.Ceiling((double)totalRowCount / itemsPerPage);
        }

        private void panelCanHo_Load(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData(currentPage);
            CalculateTotalPages();
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
                string maDV = txtMaDichVu.Text;
                string tenDV = txtTenDichVu.Text;

                decimal giaTien = decimal.Parse(txtGiaTien.Text);
                DichVuDTO existingDichVu = DichVuBUS.GetDichVuByMaDV(maDV);

                if (existingDichVu != null)
                {
                    MessageBox.Show("Mã dịch vụ đã tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maDV) && !string.IsNullOrEmpty(tenDV))
                {
                    DichVuDTO dichVu = new DichVuDTO
                    {
                        MaDV = maDV,
                        TenDV = tenDV,
                        GiaTien = giaTien
                    };

                    if (DichVuBUS.InsertDichVu(dichVu) > 0)
                    {
                        MessageBox.Show("Đã thêm dịch vụ!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Thêm dịch vụ không thành công!");
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
                    txtMaDichVu.Text = row.Cells[0].Value.ToString();
                    txtTenDichVu.Text = row.Cells[1].Value.ToString();
                    txtGiaTien.Text = row.Cells[2].Value.ToString();

                }
            }
        }
        private void ClearData()
        {
            txtMaDichVu.Text = string.Empty;
            txtTenDichVu.Text = string.Empty;
            txtGiaTien.Text = string.Empty;

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            string maDV = txtMaDichVu.Text;

            if (!string.IsNullOrWhiteSpace(maDV))
            {
                // Kiểm tra xem căn hộ có tồn tại hay không
                DichVuDTO dichVu = DichVuBUS.GetDichVuByMaDV(maDV);

                if (dichVu != null)
                {
                    // Căn hộ tồn tại, bạn có thể xóa
                    if (DichVuBUS.DeleteDichVu(maDV) > 0)
                    {

                        MessageBox.Show("Xóa dịch vụ thành công!");
                        LoadData(currentPage);
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa dịch vụ không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Dịch vụ không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
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
                string maDV = txtMaDichVu.Text;
                string tenDV = txtTenDichVu.Text;

                decimal giaTien = decimal.Parse(txtGiaTien.Text);
                DichVuDTO existingDichVu = DichVuBUS.GetDichVuByMaDV(maDV);

                if (existingDichVu == null)
                {
                    MessageBox.Show("Mã dịch vụ không tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maDV) && !string.IsNullOrEmpty(tenDV))
                {
                    DichVuDTO dichVu = new DichVuDTO
                    {
                        MaDV = maDV,
                        TenDV = tenDV,
                        GiaTien = giaTien
                    };

                    if (DichVuBUS.UpdateDichVu(dichVu) > 0)
                    {
                        MessageBox.Show("Đã sửa dịch vụ!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Sửa dịch vụ không thành công!");
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
            List<DichVuDTO> ds = DichVuBUS.SearchDichVuByFieldAndPage(tenTruong, giaTri, page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaDV"].HeaderText = "Mã dịch vụ";
            dataGridView1.Columns["TenDV"].HeaderText = "Dịch vụ";
            dataGridView1.Columns["GiaTien"].HeaderText = "Giá tiền";
        }
        private void CalculateTotalPagesSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = DichVuBUS.SearchDichVuByField(tenTruong, giaTri).Count;

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
                    ISheet worksheet = workbook.CreateSheet("Dịch vụ");

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
        private void panelDichVu_Load(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData(currentPage);
            CalculateTotalPages();
        }
    }
}
