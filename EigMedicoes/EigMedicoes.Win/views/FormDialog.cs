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
    public partial class FormDialog : Form {

        public string Text { get { return this.textBox1.Text; } set { this.textBox1.Text = value; } }

        public FormDialog() {
            InitializeComponent();

        }

        private void button4_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        internal static void Show(string p) {
            var f = new FormDialog();
            f.Text = p;
            f.ShowDialog();
        }
    }
}
