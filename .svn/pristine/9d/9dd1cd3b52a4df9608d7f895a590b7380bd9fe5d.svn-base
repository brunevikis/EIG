using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EigMedicoes.Modelo;

namespace EigMedicoes.Win.views {
    public partial class viewDados : UserControl {

        Contexto ctx = null;

        public viewDados() {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            cbxMes.SelectedItem = DateTime.Today.Month.ToString();
            cbxAno.SelectedItem = DateTime.Today.Year.ToString();
            cbxConsistida.SelectedIndex = 0;

            ctx = Repositorio.getContexto();



        }

        private async void btnBuscar_Click(object sender, EventArgs e) {
            try {

                btnBuscar.EnterLoadingState();
                btnBuscar.SetErrorState(null);

                var ano = int.Parse(cbxAno.SelectedItem.ToString());
                var mes = int.Parse(cbxMes.SelectedItem.ToString());
                var med = Repositorio.getMedicoes(mes, ano);
                var cons = cbxConsistida.SelectedIndex;

                var viewData = med
                    .Where(m => cons == 0 || ((cons == 1 && m.Consistida == true) || (cons == 2 && !m.Consistida == false)))

                    .Select(m =>
                    new {
                        m.Periodo,
                        m.Consumo,
                        m.Geracao,
                        m.Consistida,
                        Ponto = m.Medidor,
                        Medidor = String.Join(" / ", ctx.FlatRedes.Where(x => x.Medidores.Contains(m.Medidor)).Select(x => x.Nome))
                    }
                    );


                dataGridView1.DataSource = await Task.Factory.StartNew<object>(() => viewData.ToList());
            } catch (Exception ex) {
                btnBuscar.SetErrorState(ex);
            } finally {
                btnBuscar.ExitLoadingState();
            }

        }

        static List<Excel.Tools.WbFaltantes> wbFaltantes = new List<Excel.Tools.WbFaltantes>();

        private async void btnExportFaltantes_Click(object sender, EventArgs e) {
            try {

                btnExportFaltantes.EnterLoadingState();
                btnExportFaltantes.SetErrorState(null);

                var ano = int.Parse(cbxAno.SelectedItem.ToString());
                var mes = int.Parse(cbxMes.SelectedItem.ToString());
                var med = Repositorio.getMedicoes(mes, ano);
                var medMan = Repositorio.getMedicoesManuais(mes, ano);

                var viewData = med
                    .Where(m => m.Consistida == false).Union(medMan).OrderBy(x => x.Medidor).ThenBy(x => x.Periodo);

                var dados = await Task.Factory.StartNew(() => viewData.ToList());


                var wbFaltante = Excel.Tools.faltantes(dados, ano, mes);
                wbFaltante.OnClose += wbFaltantes_OnClose;

                wbFaltantes.Add(wbFaltante);

            } catch (Exception ex) {
                btnExportFaltantes.SetErrorState(ex);
            } finally {
                btnExportFaltantes.ExitLoadingState();
            }
        }



        void wbFaltantes_OnClose(object sender, EigMedicoes.Excel.Tools.WbFaltantes.FaltantesEventArgs e) {



            Application.OpenForms[0].Invoke(new Action(() => {

                Application.OpenForms[0].Activate();

                if (MessageBox.Show(Application.OpenForms[0], "Inserir dados?", "EIG - Medições", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes) {

                    Repositorio.limparMedicoesManuais(e.Mes, e.Ano);
                    Repositorio.instertMedicoesManuais(e.Dados);

                    if (wbFaltantes.Contains((Excel.Tools.WbFaltantes)sender))
                        wbFaltantes.Remove((Excel.Tools.WbFaltantes)sender);

                }


            }));
        }
    }
}
