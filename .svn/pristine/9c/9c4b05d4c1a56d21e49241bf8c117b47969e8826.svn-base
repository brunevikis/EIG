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
    public partial class FormBuscarCadastro<T> : Form {

        Func<IList<T>> DataProvider = null;
        public FormBuscarCadastro(Func<IList<T>> dataProvider) {
            InitializeComponent();
            DataProvider = dataProvider;
           
        }

        //IList<EigMedicoes.Modelo.Cadastro> cads;
        IList<T> cads;

        private void FormBuscarCadastro_Load(object sender, EventArgs e) {
            cads = DataProvider();
            HandleTextChanged(cads);
        }

        public T Selecionado { get { return ((T)comboBox1.SelectedItem); } }

        private bool _canUpdate = true;

        private bool _needUpdate = false;

        //If text has been changed then start timer
        //If the user doesn't change text while the timer runs then start search
        private void combobox1_TextChanged(object sender, EventArgs e) {
            if (_needUpdate) {
                if (_canUpdate) {
                    _canUpdate = false;
                    UpdateData();
                } else {
                    RestartTimer();
                }
            }
        }

        private void UpdateData() {
            if (comboBox1.Text.Length > 1) {
                var searchData = GetData(comboBox1.Text);
                HandleTextChanged(searchData);
            } else {
                HandleTextChanged(cads);
            }
        }

        private IList<T> GetData(string p) {
            return cads.Where(x => x.ToString().ToUpperInvariant().Contains(p.ToUpperInvariant())).ToList();                
        }

        //If an item was selected don't start new search
        private void combobox1_SelectedIndexChanged(object sender, EventArgs e) {
            _needUpdate = false;
        }

        //Update data only when the user (not program) change something
        private void combobox1_TextUpdate(object sender, EventArgs e) {
            _needUpdate = true;
        }

        //While timer is running don't start search
        //timer1.Interval = 1500;
        private void RestartTimer() {
            timer1.Stop();
            _canUpdate = false;
            timer1.Start();
        }

        //Update data when timer stops
        private void timer1_Tick(object sender, EventArgs e) {
            _canUpdate = true;
            timer1.Stop();
            UpdateData();
        }

        //Update combobox with new data
        private void HandleTextChanged(IList<T> dataSource) {
            var text = comboBox1.Text;

            if (dataSource.Count() > 0) {
                comboBox1.DataSource = dataSource;

                var sText = comboBox1.Items[0].ToString();
                comboBox1.SelectionStart = text.Length;
                comboBox1.SelectionLength = sText.Length - text.Length;
                comboBox1.DroppedDown = true;
                Cursor.Current = Cursors.Default;


                return;
            } else {
                comboBox1.DroppedDown = false;
                comboBox1.SelectionStart = text.Length;
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
