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
    public partial class FormChart : Form {
        private FormChart() {
            InitializeComponent();
        }

        Sumario sum;

        public FormChart(Sumario sum)
            : this() {

            this.sum = sum;

            this.Text = "EIG Medicoes - " + sum.Ativo;
        }

        private void Form1_Load(object sender, EventArgs e) {
            BuildChart();
        }

        public void BuildChart() {
            chart1.Series.Clear();

            chart1.Titles[0].Text = sum.Ativo + (sum.TipoAtivo == "G" ? " - Geração" : " - Consumo") + "(MWh)";

            List<Ponto> pontos = new List<Ponto>();


            for (var per = sum.MedicaoDiaria.Min(x => x.Key); per <= sum.Fim; per = per.AddDays(1)) {
                pontos.Add(new Ponto {
                    Periodo = per,
                    Valor = sum.MedicaoDiaria.ContainsKey(per) ?
                        sum.MedicaoDiaria[per] : 0
                });
            }
            var ser = chart1.Series.Add("Medido");
            
            if (sum.MedicaoDiariaProjetada != null && sum.MedicaoDiariaProjetada.Count > 0 ) {


                List<Ponto> pontosP = new List<Ponto>();

                for (var per = sum.MedicaoDiaria.Min(x => x.Key); per <= sum.MedicaoDiariaProjetada.Max(x => x.Key); per = per.AddDays(1)) {
                    pontosP.Add(new Ponto {
                        Periodo = per,
                        Valor = sum.MedicaoDiariaProjetada.ContainsKey(per) ?
                            sum.MedicaoDiariaProjetada[per] : 0
                    });

                    if (!pontos.Any(x=>x.Periodo == per)) {
                        pontos.Add(new Ponto { Periodo = per, Valor = 0 });
                    }
                }

                

                var serP = chart1.Series.Add("Projetado");
                serP.Points.DataBind(pontosP, "Periodo", "Valor", "");


                
                serP.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            }
            
            ser.Points.DataBind(pontos, "Periodo", "Valor", "");
            ser.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;

            chart1.ChartAreas[0].AxisX.Minimum = pontos.Min(x => x.Periodo).ToOADate() - 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.IntervalOffset = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = false;


        }
        public void SaveChart(string file) {
            chart1.SaveImage(file, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public void CopyChart() {

            var stream = new System.IO.MemoryStream();
            chart1.SaveImage(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            var bmp = new Bitmap(stream);
            Clipboard.SetDataObject(bmp);
        }

    }

    class Ponto {
        public DateTime Periodo { get; set; }
        public Decimal Valor { get; set; }
    }
}
