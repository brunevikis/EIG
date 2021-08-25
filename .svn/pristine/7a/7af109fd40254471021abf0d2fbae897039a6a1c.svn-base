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
    public partial class FormConfirmaRateio : Form {



        private FormConfirmaRateio() {
            InitializeComponent();
        }

        List<Tuple<decimal, int>> cods;

        private FormConfirmaRateio(EigMedicoes.Modelo.Sumario sum, List<Modelo.Agente> agentes)
            : this() {

            label1.Text = sum.Cliente + " - " + sum.Ativo;

            


            foreach (var item in sum.CodigosUnidades) {

                var txt = new TextBox();
                txt.DataBindings.Add("Text", item, "Value");
                txt.CausesValidation = true;
                txt.Tag = item.Key;
                txt.Validating += (object sender, CancelEventArgs e) => {
                    decimal x;

                    if (!decimal.TryParse(((TextBox)sender).Text, out x)) {
                        e.Cancel = true;
                    } else {
                        sum.CodigosUnidades[(int)((TextBox)sender).Tag] = x;
                    }

                };


                var lbl = new Label();
                lbl.Text = item.Key.ToString() + " - " + agentes.First(x => x.ID_Unidade == item.Key).Unidade;

                flowLayoutPanel1.Controls.Add(lbl);
                flowLayoutPanel1.Controls.Add(txt);
            }
        }

        //void txt_Validating(object sender, CancelEventArgs e) {
        //    decimal x;
        //    var txt = ((TextBox)sender);

        //    if (!decimal.TryParse(txt.Text, out x)) {
        //        e.Cancel = true;
        //    } else {

        //    }

        //}

        private void button4_Click(object sender, EventArgs e) {
            if (this.ValidateChildren()) {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }


        internal static DialogResult Show(EigMedicoes.Modelo.Sumario sum, List<Modelo.Agente> agentes) {
            var f = new FormConfirmaRateio(sum, agentes);
            f.ShowDialog();
            return f.DialogResult;
        }

        private void FormConfirmaRateio_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
