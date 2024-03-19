using BUS;
using DTO;
using NPOI.SS.Formula.Functions;
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
    public partial class frmDangNhap : Form
    {
        private static frmGiaoDienChinh frmGiaoDienChinh;
        public frmDangNhap()
        {
            InitializeComponent();
        }
        private int count;
        private void Form1_Load(object sender, EventArgs e)
        {
            count = 0;
        }
        
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string maNV = txtMaNV.Text;
            string matKhau = txtMatKhau.Text;
            if (maNV != "" && matKhau != "")
            {
                Session.nhanVien = NhanVienBUS.Login(maNV, matKhau);
                if (Session.nhanVien != null)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    this.Hide();
                    frmGiaoDienChinh = new frmGiaoDienChinh();
                    frmGiaoDienChinh.Show();
                }
                else
                {
                    MessageBox.Show("Đăng nhập không thành công!");
                    count++;
                    if(count == 3)
                    {
                        MessageBox.Show("Bạn đã vượt quá số lần đăng nhập!");
                        btnDangNhap.Enabled = false;
                    }
                }
            }
            else
            {
                MessageBox.Show("Thông tin đăng nhập không được bỏ trống");
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
