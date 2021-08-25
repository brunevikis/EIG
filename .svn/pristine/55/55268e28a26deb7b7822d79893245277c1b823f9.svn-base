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

namespace EigMedicoes.Win.views {
    public partial class FormAbrirContabilizacao : Form {

        eig.medicao.model.EIG_MEDIDOREntities ctx = new model.EIG_MEDIDOREntities();

        public IEnumerable<Contabilizacao> Selected;

        public int Ano { get; set; }
        public int Mes { get; set; }

        public FormAbrirContabilizacao(int ano, int mes) {
            InitializeComponent();

            this.Ano = ano;
            this.Mes = mes;

        }

        private async void FormAbrirContabilizacao_Load(object sender, EventArgs e) {

            try {

                comboBox1.EnterLoadingState();
                comboBox1.SetErrorState(null);

                var datas = await Task.Factory.StartNew<object>(() => ctx.Contabilizacoes
                    .Where(x => x.InicioPeriodo.Year == Ano && x.InicioPeriodo.Month == Mes)
                    .Select(x => new { x.DataGeracao, x.InicioPeriodo, x.FimPeriodo })
                    .ToList().Distinct()
                    .Select(x => new {
                        x.DataGeracao,
                        Display = x.DataGeracao.ToString() + ": " +
                            x.InicioPeriodo.ToString() + " - " + x.FimPeriodo.ToString()


                    })
                    .OrderByDescending(x => x.DataGeracao)
                    .ToList()
                    );

                comboBox1.DataSource = datas;
                comboBox1.ValueMember = "DataGeracao";
                comboBox1.DisplayMember = "Display";

            } catch (Exception ex) {
                comboBox1.SetErrorState(ex);
            } finally {
                comboBox1.ExitLoadingState();
            }
        }



        private async void button1_Click(object sender, EventArgs e) {

            button1.EnterLoadingState();

            var data = (DateTime)comboBox1.SelectedValue;

            Selected = await Task.Factory.StartNew<IEnumerable<Contabilizacao>>((() => ctx.Contabilizacoes.Where(x => x.DataGeracao == data).AsEnumerable()));

            button1.ExitLoadingState();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();

        }

        private void button2_Click(object sender, EventArgs e) {

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }



    }
}
