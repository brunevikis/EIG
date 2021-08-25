
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
    public partial class FormCadastroEditor : Form {

        public UserControl Editor {
            get {
                if (cadatroEditPanel.Controls.Count > 0)
                    return (UserControl)cadatroEditPanel.Controls[0];
                else
                    return null;

            }
            set {

                if (this.Editor != null)
                    this.Editor.Dispose();

                cadatroEditPanel.Controls.Clear();
                cadatroEditPanel.Controls.Add(value);
            }
        }

        public FormCadastroEditor(IEditorUserControl editor) {
            InitializeComponent();
            editor.ViewOnly = false;
            Editor = (UserControl)editor;

            editor.ActionDone += editor_ActionDone;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        void editor_ActionDone(object sender, EventArgs e) {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void FormCadastroEditor_FormClosing(object sender, FormClosingEventArgs e) {

            if (this.DialogResult != System.Windows.Forms.DialogResult.OK) {


                switch (MessageBox.Show("Aceitar mudanças?", "EIG - Medições", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)) {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                        ((dynamic)Editor).UpdateModel();
                        break;
                    case DialogResult.No:
                    default:
                        this.DialogResult = System.Windows.Forms.DialogResult.No;
                        break;
                }

            }

        }

        private void FormCadastroEditor_Load(object sender, EventArgs e) {

        }
    }
}
