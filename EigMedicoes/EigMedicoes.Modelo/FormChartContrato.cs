using EigMedicoes.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigMedicoes.Modelo.View {
    public partial class FormChartContrato : Form {
        private FormChartContrato() {
            InitializeComponent();
        }

        MedicaoCliente balanco;


        public FormChartContrato(MedicaoCliente balanco)
            : this() {

            this.balanco = balanco;
            this.Text = "EIG Medicoes - " + balanco.Unidade;
        }

        private void Form1_Load(object sender, EventArgs e) {
            BuildChart();
        }

        public void BuildChart() {
            chart1.Series.Clear();
            chart1.Titles.Add(balanco.Unidade);

            decimal perdaRedeBasica = balanco.FatorPerda;

            {

                var ser = chart1.Series.Add("Medido" + (perdaRedeBasica > 1m ? " + Perda na Rede Básica" : (perdaRedeBasica < 1m ? " - Perda na Rede Básica" : "")));
                ser.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;

                ser.Points.Add((double)(balanco.Montante * perdaRedeBasica));
                ser.IsValueShownAsLabel = true;
                ser.LabelBackColor = System.Drawing.Color.White;
                ser.LabelFormat = "#,###.00";


            }

            if (balanco.PossuiProjecao) {
                var ser = chart1.Series.Add("Projeção");
                ser.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.RangeColumn;

                ser.Points.Add((double)(balanco.MontanteProjetado * perdaRedeBasica));
                ser.IsValueShownAsLabel = true;
                ser.LabelBackColor = System.Drawing.Color.White;
                ser.LabelFormat = "#,###.00";
            }

            {
                var ser = chart1.Series.Add("Contrato");
                ser.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;

                ser.Points.Add((double)Math.Abs(balanco.Contratado));
                ser.BorderColor = System.Drawing.Color.Navy;
                ser.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
                ser.BorderWidth = 2;
                ser.Color = System.Drawing.Color.Transparent;
                ser.LabelBackColor = System.Drawing.Color.White;
                ser.CustomProperties = "LabelStyle=TopRight";
                ser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
                ser.IsValueShownAsLabel = true;
                ser.LabelFormat = "#,###.00";
            }
        }

        public void SaveChart(string file) {
            chart1.SaveImage(file, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
        public void CopyChart() {
            //        var bmp = new Bitmap(400, 240);
            //       chart1.DrawToBitmap(bmp, new Rectangle(0, 0, 400, 240));

            var stream = new System.IO.MemoryStream();
            chart1.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            var bmp = new Bitmap(stream);
            Clipboard.SetDataObject(bmp);
        }

    }

}
