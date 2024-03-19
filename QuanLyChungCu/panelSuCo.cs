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
    public partial class panelSuCo : UserControl
    {
        public panelSuCo()
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
            List<SuCoDTO> ds = SuCoBUS.GetSuCoByPage(page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaSC"].HeaderText = "Mã sự cố";
            dataGridView1.Columns["TenSC"].HeaderText = "Tên sự cố";
            dataGridView1.Columns["NguyenNhan"].HeaderText = "Nguyên nhân";
            dataGridView1.Columns["MucPhat"].HeaderText = "Mức phạt";
        }
        private void CalculateTotalPages()
        {
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = SuCoBUS.GetAllSuCo().Count;

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
                string maSC = txtMaSuCo.Text;
                string tenSC = txtTenSuCo.Text;
                string nguyenNhan = txtNguyenNhan.Text;
                decimal mucPhat = decimal.Parse(txtMucPhat.Text);
                SuCoDTO existingSuCo = SuCoBUS.GetSuCoByMaSC(maSC);

                if (existingSuCo != null)
                {
                    MessageBox.Show("Mã sự cố đã tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maSC) && !string.IsNullOrEmpty(tenSC) && !string.IsNullOrEmpty(nguyenNhan))
                {
                    SuCoDTO suCo = new SuCoDTO
                    {
                        MaSC = maSC,
                        TenSC = tenSC,
                        NguyenNhan = nguyenNhan,
                        MucPhat = mucPhat,
                    };

                    if (SuCoBUS.InsertSuCo(suCo) > 0)
                    {
                        MessageBox.Show("Đã thêm sự cố!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Thêm sự cố không thành công!");
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
                    txtMaSuCo.Text = row.Cells[0].Value.ToString();
                    txtTenSuCo.Text = row.Cells[1].Value.ToString();
                    txtNguyenNhan.Text = row.Cells[2].Value.ToString();
                    txtMucPhat.Text = row.Cells[3].Value.ToString();

                }
            }
        }
        private void ClearData()
        {
            txtMaSuCo.Text = string.Empty;
            txtTenSuCo.Text = string.Empty;
            txtNguyenNhan.Text = string.Empty;
            txtMucPhat.Text = string.Empty;

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            string maSC = txtMaSuCo.Text;

            if (!string.IsNullOrWhiteSpace(maSC))
            {
                // Kiểm tra xem căn hộ có tồn tại hay không
                SuCoDTO suCo = SuCoBUS.GetSuCoByMaSC(maSC);

                if (suCo != null)
                {
                    // Căn hộ tồn tại, bạn có thể xóa
                    if (SuCoBUS.DeleteSuCo(maSC) > 0)
                    {

                        MessageBox.Show("Xóa sự cố thành công!");
                        LoadData(currentPage);
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa sự cố không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Sự cố không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sự cố cần xóa!");
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
                string maSC = txtMaSuCo.Text;
                string tenSC = txtTenSuCo.Text;
                string nguyenNhan = txtNguyenNhan.Text;
                decimal mucPhat = decimal.Parse(txtMucPhat.Text);
                SuCoDTO existingSuCo = SuCoBUS.GetSuCoByMaSC(maSC);

                if (existingSuCo == null)
                {
                    MessageBox.Show("Mã sự cố không tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maSC) && !string.IsNullOrEmpty(tenSC) && !string.IsNullOrEmpty(nguyenNhan))
                {
                    SuCoDTO suCo = new SuCoDTO
                    {
                        MaSC = maSC,
                        TenSC = tenSC,
                        NguyenNhan = nguyenNhan,
                        MucPhat = mucPhat,
                    };

                    if (SuCoBUS.UpdateSuCo(suCo) > 0)
                    {
                        MessageBox.Show("Đã sửa sự cố!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Sửa sự cố không thành công!");
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
            List<SuCoDTO> ds = SuCoBUS.SearchSuCoByFieldAndPage(tenTruong, giaTri, page, itemsPerPage);
            dataGridView1.DataSource = ds;


            dataGridView1.Columns["MaSC"].HeaderText = "Mã sự cố";
            dataGridView1.Columns["TenSC"].HeaderText = "Tên sự cố";
            dataGridView1.Columns["NguyenNhan"].HeaderText = "Nguyên nhân";
            dataGridView1.Columns["MucPhat"].HeaderText = "Mức phạt";
        }
        private void CalculateTotalPagesSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = SuCoBUS.SearchSuCoByField(tenTruong, giaTri).Count;

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
                    ISheet worksheet = workbook.CreateSheet("Sự cố");

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

        private void panelSuCo_Load(object sender, EventArgs e)
        {
            currentPage = 1;
            LoadData(currentPage);
            CalculateTotalPages();
        }
    }
}
