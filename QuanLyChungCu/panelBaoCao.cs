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
    public partial class panelBaoCao : UserControl
    {
        private  int loai;
        public panelBaoCao(int loai)
        {
            InitializeComponent();
            this.loai = loai;
        }

        private void panelBaoCao_Load(object sender, EventArgs e)
        {
            if (loai == 1)
            {
                labelTitle.Text = "BÁO CÁO DOANH THU";
            }
            if (loai == 2)
            {
                labelTitle.Text = "BÁO  CÁO TIỀN CỌC";
            }

        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (loai == 1)
            {
                // Generate a Crystal Report for doanh thu here
                GenerateDoanhThuReport();
            }
            if (loai == 2)
            {
                GenerateTienCoc();
            }
          
        }
        private void GenerateTienCoc()
        {

            DateTime ngayBatDau = dateTimePickerNgayBatDau.Value;
            DateTime ngayKetThuc = dateTimePickerNgayKetThuc.Value;
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            CrystalReport6 report = new CrystalReport6(); // Replace with your report class

            DataTable data = BaoCaoBUS.BaoCaoTienCoc(ngayBatDau, ngayKetThuc);

            report.SetDataSource(data);
            crystalReportViewer.ReportSource = report;
            Form reportForm = new Form();
            crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportForm.Controls.Add(crystalReportViewer);
            reportForm.WindowState = FormWindowState.Maximized;
            reportForm.ShowDialog();

        }
        private void GenerateDoanhThuReport()
        {

            DateTime ngayBatDau = dateTimePickerNgayBatDau.Value;
            DateTime ngayKetThuc = dateTimePickerNgayKetThuc.Value;
            CrystalReportViewer crystalReportViewer = new CrystalReportViewer();
            CrystalReport1 report = new CrystalReport1(); // Replace with your report class

            DataTable data = BaoCaoBUS.BaoCaoDoanhThu(ngayBatDau, ngayKetThuc);

            report.SetDataSource(data);
            crystalReportViewer.ReportSource = report;
            Form reportForm = new Form();
            crystalReportViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            reportForm.Controls.Add(crystalReportViewer);
            reportForm.WindowState = FormWindowState.Maximized;
            reportForm.ShowDialog();

        }

    }
}
