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
    public partial class viewCadastroClienteAtivo : EditorUserControl_Ativo { //EditorUserControl<eig.medicao.model.Cliente> {

        bool _viewOnly;
        public override bool ViewOnly {
            get { return _viewOnly; }
            set {
                _viewOnly = value;  

                textBox1.ReadOnly =               _viewOnly;
            }
        }

        public string AtivoNome { get { return textBox1.Text; } set { textBox1.Text = value; } }

        viewCadastros parent = null;
        public viewCadastroClienteAtivo(viewCadastros parent)
            : this() {
            this.parent = parent;
        }

        private viewCadastroClienteAtivo() {
            InitializeComponent();
            ViewOnly = true;
        }

        void Clear() {

            //ClienteNome = "";
            //Emails = null;
            //CCEmails = null;
            //IncluirGrafico = false;
        }

        public override Ativo Model {
            get {
                return
                    Node == null ? null :
                    Node.BoundObject;
            }
            set {
                if (value != null) {
                    AtivoNome = value.Nome;
                } else {
                    this.Clear();
                }
            }
        }


        private void btnRemover_Click(object sender, EventArgs e) {
            if (
            MessageBox.Show("Tem certeza que deseja remover: " + this.Model.Nome + "?", "EIG - Medições", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes) {
                    
                this.Model.Cliente.Ativos.Remove(this.Model);                        

                ((TreeNode)this.Node).Remove();

                this.InvokeActionDone();
            }
        }


    }

    public class EditorUserControl_Ativo : EditorUserControl<Ativo> {
        public override Ativo Model {
            get;
            set;
        }
    }
}
