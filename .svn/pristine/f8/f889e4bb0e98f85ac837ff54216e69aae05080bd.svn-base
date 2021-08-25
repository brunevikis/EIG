using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigMedicoes.Win {
    public partial class FormMain : Form {
        public FormMain() {
            InitializeComponent();
        }

        private void ClearViewPanel() {
            //foreach (Control ctl in panel1.Controls) {
            //    ctl.Dispose();
            //}
            panel1.Controls.Clear();
        }

        private void btnVisualizarDados_Click(object sender, EventArgs e) {

            ClearViewPanel();
            OpenView(typeof(views.viewDados));
           
        }

       

        private void btnCadastros_Click(object sender, EventArgs e) {
            ClearViewPanel();
            OpenView(typeof(views.viewCadastrosR));
            //panel1.SuspendLayout();
            //this.SuspendLayout();
            
            //var view = new views.viewCadastros('R');

            //panel1.Controls.Add(view);
            
            //this.panel1.ResumeLayout(false);
            //this.ResumeLayout(false);
        }

        private void btnCadastrosCli_Click(object sender, EventArgs e) {
            ClearViewPanel();
            OpenView(typeof(views.viewCadastrosC));
            //panel1.SuspendLayout();
            //this.SuspendLayout();
            
            //var view = new views.viewCadastros('C');

            //panel1.Controls.Add(view);
            
            //this.panel1.ResumeLayout(false);
            //this.ResumeLayout(false);
        }

        private void btnContabilizacao_Click(object sender, EventArgs e) {
            ClearViewPanel();
            OpenView(typeof(views.viewContabilizacao));
            
        }

        private void btnImportacao_Click(object sender, EventArgs e) {
            ClearViewPanel();
            OpenView(typeof(views.viewImportacao));
        }

        Dictionary<Type, UserControl> views = new Dictionary<Type, UserControl>();

        private void OpenView(Type viewType) {

            UserControl view;
            if (views.ContainsKey(viewType)) {
                view = views[viewType];
            } else {
                view = (UserControl)Activator.CreateInstance(viewType);
                views.Add(viewType, view);
            }

            panel1.SuspendLayout();
            this.SuspendLayout();

            //var view = new views.viewImportacao();

            panel1.Controls.Add(view);

            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);


        }


        



    }
}
