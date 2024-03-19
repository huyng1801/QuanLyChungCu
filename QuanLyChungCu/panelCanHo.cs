using BUS;
using DTO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using OfficeOpenXml.Core.ExcelPackage;
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
    public partial class panelCanHo : UserControl
    {
        private int currentPage = 1; // Trang hiện tại
        private int itemsPerPage = 5; // Số dòng trên mỗi trang
        private int totalPageCount; // Tổng số trang
        private int flag = 0;
        public panelCanHo()
        {
            InitializeComponent();
        }

        private void LoadData(int page)
        {
            CalculateTotalPages();
            List<CanHoDTO> ds = CanHoBUS.GetCanHoByPage(page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaCH"].HeaderText = "Mã căn hộ";
            dataGridView1.Columns["SoTang"].HeaderText = "Số tầng";
            dataGridView1.Columns["DienTich"].HeaderText = "Diện tích";
            dataGridView1.Columns["PhongNgu"].HeaderText = "Phòng ngủ";
            dataGridView1.Columns["NoiThat"].HeaderText = "Nội thất";
            dataGridView1.Columns["TinhTrang"].HeaderText = "Tình trạng";
            dataGridView1.Columns["GiaThue"].HeaderText = "Giá thuê";
        }
        private void CalculateTotalPages()
        {
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = CanHoBUS.GetAllCanHo().Count;

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
            if(flag == 0)
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
                string maCH = txtMaCH.Text;
                string soTang = txtSoTang.Text;
                decimal dienTich = decimal.Parse(txtDienTich.Text);
                int phongNgu = int.Parse(numericUpDownPhongNgu.Value.ToString());
                string noiThat = txtNoiThat.Text;
                string tinhTrang = cboTinhTrang.Text;
                decimal giaThue = decimal.Parse(txtGiaThue.Text);
                CanHoDTO existingCanHo = CanHoBUS.GetCanHoByMaCH(maCH);

                if (existingCanHo != null)
                {
                    MessageBox.Show("Mã căn hộ đã tồn tại!");
                    return; 
                }
                if (!string.IsNullOrEmpty(maCH) && !string.IsNullOrEmpty(soTang) && !string.IsNullOrEmpty(noiThat) && !string.IsNullOrEmpty(tinhTrang))
                {
                    CanHoDTO canHo = new CanHoDTO
                    {
                        MaCH = maCH,
                        SoTang = soTang,
                        DienTich = dienTich,
                        PhongNgu = phongNgu,
                        NoiThat = noiThat,
                        TinhTrang = tinhTrang,
                        GiaThue = giaThue
                    };

                    if (CanHoBUS.InsertCanHo(canHo) > 0)
                    {
                        MessageBox.Show("Đã thêm căn hộ!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Thêm căn hộ không thành công!");
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
                    txtMaCH.Text = row.Cells[0].Value.ToString();
                    txtSoTang.Text = row.Cells[1].Value.ToString();
                    txtDienTich.Text = row.Cells[2].Value.ToString();
                    numericUpDownPhongNgu.Text = row.Cells[3].Value.ToString();
                    txtNoiThat.Text = row.Cells[4].Value.ToString();
                    cboTinhTrang.Text = row.Cells[5].Value.ToString();
                    txtGiaThue.Text = row.Cells[6].Value.ToString();
                }
            }
        }
        private void ClearData()
        {
            txtMaCH.Text = string.Empty;
            txtSoTang.Text = string.Empty;
            txtDienTich.Text = string.Empty;
            numericUpDownPhongNgu.Value = 1; // Giá trị mặc định hoặc 0
            txtNoiThat.Text = string.Empty;
            cboTinhTrang.SelectedIndex = -1; // Chọn mục đầu tiên hoặc giá trị không hợp lệ
            txtGiaThue.Text = string.Empty;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

            string maCH = txtMaCH.Text;

            if (!string.IsNullOrWhiteSpace(maCH))
            {
                // Kiểm tra xem căn hộ có tồn tại hay không
                CanHoDTO canHo = CanHoBUS.GetCanHoByMaCH(maCH);

                if (canHo != null)
                {
                    // Căn hộ tồn tại, bạn có thể xóa
                    if (CanHoBUS.DeleteCanHo(maCH) > 0)
                    {
                       
                        MessageBox.Show("Xóa căn hộ thành công!");
                         LoadData(currentPage);
                        ClearData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa căn hộ không thành công!");
                    }
                }
                else
                {
                    MessageBox.Show("Căn hộ không tồn tại!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn căn hộ cần xóa!");
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
                string maCH = txtMaCH.Text;
                string soTang = txtSoTang.Text;
                decimal dienTich = decimal.Parse(txtDienTich.Text);
                int phongNgu = int.Parse(numericUpDownPhongNgu.Value.ToString());
                string noiThat = txtNoiThat.Text;
                string tinhTrang = cboTinhTrang.Text;
                decimal giaThue = decimal.Parse(txtGiaThue.Text);
                CanHoDTO existingCanHo = CanHoBUS.GetCanHoByMaCH(maCH);

                if (existingCanHo == null)
                {
                    MessageBox.Show("Mã căn hộ không tồn tại!");
                    return;
                }
                if (!string.IsNullOrEmpty(maCH) && !string.IsNullOrEmpty(soTang) && !string.IsNullOrEmpty(noiThat) && !string.IsNullOrEmpty(tinhTrang))
                {
                    CanHoDTO canHo = new CanHoDTO
                    {
                        MaCH = maCH,
                        SoTang = soTang,
                        DienTich = dienTich,
                        PhongNgu = phongNgu,
                        NoiThat = noiThat,
                        TinhTrang = tinhTrang,
                        GiaThue = giaThue
                    };

                    if (CanHoBUS.UpdateCanHo(canHo) > 0)
                    {
                        MessageBox.Show("Đã sửa căn hộ!");
                        LoadData(currentPage);
                        ClearData();


                    }
                    else
                    {
                        MessageBox.Show("Sửa căn hộ không thành công!");
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
            List<CanHoDTO> ds = CanHoBUS.SearchCanHoByFieldAndPage(tenTruong, giaTri, page, itemsPerPage);
            dataGridView1.DataSource = ds;

            dataGridView1.Columns["MaCH"].HeaderText = "Mã căn hộ";
            dataGridView1.Columns["SoTang"].HeaderText = "Số tầng";
            dataGridView1.Columns["DienTich"].HeaderText = "Diện tích";
            dataGridView1.Columns["PhongNgu"].HeaderText = "Phòng ngủ";
            dataGridView1.Columns["NoiThat"].HeaderText = "Nội thất";
            dataGridView1.Columns["TinhTrang"].HeaderText = "Tình trạng";
            dataGridView1.Columns["GiaThue"].HeaderText = "Giá thuê";
        }
        private void CalculateTotalPagesSearch()
        {
            string tenTruong = cboTenTruong.Text;
            string giaTri = txtGiaTri.Text;
            // Lấy tổng số dòng dữ liệu từ cơ sở dữ liệu
            int totalRowCount = CanHoBUS.SearchCanHoByField(tenTruong, giaTri).Count;

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
                    ISheet worksheet = workbook.CreateSheet("Căn hộ");

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
    }
}
