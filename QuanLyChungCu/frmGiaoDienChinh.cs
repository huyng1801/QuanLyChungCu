using BUS;
using CrystalDecisions.Windows.Forms;
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
    public partial class frmGiaoDienChinh : Form
    {
        public frmGiaoDienChinh()
        {
            InitializeComponent();
        }

        private void frmGiaoDienChinh_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void cănHộToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelCanHo gui = new panelCanHo();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelKhachHang gui = new panelKhachHang();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void nhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelNhanVien gui = new panelNhanVien();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void dịchVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelDichVu gui = new panelDichVu();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void sựCốToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelSuCo gui = new panelSuCo();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void hợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelHopDong gui = new panelHopDong();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelHoaDon gui = new panelHoaDon();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void bồiThườngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBienBanBoiThuong gui = new panelBienBanBoiThuong();
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void frmGiaoDienChinh_Load(object sender, EventArgs e)
        {
            this.Text = "Quản lý chung cư - Nhân viên: " + Session.nhanVien.TenNV;
        }

        private void trangChủToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            this.panel1.BackgroundImage = Properties.Resources._0000521_650;
            this.panel1.Dock = DockStyle.Fill;
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void doanhThuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBaoCao gui = new panelBaoCao(1);
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }

        private void hợpĐồngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
                CrystalReport4 report = new CrystalReport4(); // Replace with your report class
                DataTable data = BaoCaoBUS.BaoCaoHopDongSapHetHan();
                report.SetDataSource(data);
                crystalReportViewer.ReportSource = report;
                Form reportForm = new Form();
                crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                reportForm.Controls.Add(crystalReportViewer);
                reportForm.WindowState = FormWindowState.Maximized;
                reportForm.ShowDialog();

        }

        private void cănHộToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            CrystalReport2 report = new CrystalReport2(); // Replace with your report class

            DataTable data = BaoCaoBUS.BaoCaoTongHopCanHo();

            report.SetDataSource(data);
            crystalReportViewer.ReportSource = report;
            Form reportForm = new Form();
            crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportForm.Controls.Add(crystalReportViewer);
            reportForm.WindowState = FormWindowState.Maximized;
            reportForm.ShowDialog();
        }

        private void kháchThuêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            CrystalReport3 report = new CrystalReport3(); // Replace with your report class

            DataTable data = BaoCaoBUS.BaoCaoTongHopKhachThue();

            report.SetDataSource(data);
            crystalReportViewer.ReportSource = report;
            Form reportForm = new Form();
            crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportForm.Controls.Add(crystalReportViewer);
            reportForm.WindowState = FormWindowState.Maximized;
            reportForm.ShowDialog();
        }

        private void thanhToánChậmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            CrystalReport5 report = new CrystalReport5(); // Replace with your report class

            DataTable data = BaoCaoBUS.BaoCaoThanhToanCham();

            report.SetDataSource(data);
            crystalReportViewer.ReportSource = report;
            Form reportForm = new Form();
            crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportForm.Controls.Add(crystalReportViewer);
            reportForm.WindowState = FormWindowState.Maximized;
            reportForm.ShowDialog();

        }

        private void báoCáoTiềnCọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelBaoCao gui = new panelBaoCao(2);
            gui.Dock = DockStyle.Fill;
            panel1.Controls.Clear();
            panel1.Controls.Add(gui);
        }
    }
}
