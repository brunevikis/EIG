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
    public partial class viewCadastroRede : EditorUserControl_Rede {

        bool _viewOnly;
        public override bool ViewOnly {
            get { return _viewOnly; }
            set {
                _viewOnly = value;

                /*btnMover.Visible*/
                txtCodUnidade.ReadOnly =
                txtMedidores.ReadOnly =
                txtNome.ReadOnly =
                txtCdAgente.ReadOnly =
                chkMove.Visible = btnEditar.Visible = _viewOnly;

                btnAdicionarDescendente.Visible = _viewOnly;


                chkAgrupar.Enabled =
                btnVincular.Visible = btnRemover.Visible = !_viewOnly;
                comboBox1.Enabled = !_viewOnly;
            }
        }

        public string Nome { get { return txtNome.Text; } set { txtNome.Text = value; } }
        public string[] Medidores { set { txtMedidores.Lines = value; } get { return txtMedidores.Lines.Select(m => m.Trim()).ToArray(); } }
        public string Tipo {
            set {

                switch (value) {
                    case "G":
                        comboBox1.SelectedIndex = 0;
                        return;
                    case "C":
                        comboBox1.SelectedIndex = 1;
                        return;
                    default:
                        comboBox1.SelectedIndex = -1;
                        return;
                }
            }
            get {
                switch (comboBox1.SelectedIndex) {
                    case 0:
                        return "G";
                    case 1:
                        return "C";
                    default:
                        return null;
                }

            }
        }
        public Dictionary<int, decimal> CodigoUnidade {
            set {
                if (value != null) {
                    txtCodUnidade.Lines = value.Select(x => x.Value.ToString("p") + " - " + x.Key.ToString()).ToArray();
                } else txtCodUnidade.Text = "";

            }
            get {

                var ret = new Dictionary<int, decimal>();
                var r = txtCodUnidade.Lines.Select(x => {

                    int cod;
                    decimal p;

                    var arr = x.Split('-');
                    if (arr.Length == 1) {

                        if (!int.TryParse(arr[0].Trim(), out cod)) return false;
                        p = 100;

                    } else if (arr.Length == 2) {
                        if (!decimal.TryParse(arr[0].Replace('%', ' ').Trim(), out p)) return false;
                        if (!int.TryParse(arr[1].Trim(), out cod)) return false;

                    } else return false;

                    ret.Add(cod, p / 100);
                    return true; //
                }).Where(x => x != null).ToArray();

                 return ret;
                
            }
        }

        public string CodigoAgente_SCDD { get { return txtCdAgente.Text; } set { txtCdAgente.Text = value; } }

        public bool AgruparConsumoGeracao { get { return chkAgrupar.Checked; } set { chkAgrupar.Checked = value; } }

        viewCadastros parent = null;
        public viewCadastroRede(viewCadastros parent)
            : this() {
            this.parent = parent;
        }

        private viewCadastroRede() {
            InitializeComponent();
            ViewOnly = true;
        }

        void Clear() {
            Nome = Tipo = "";
            CodigoUnidade = null;
            Medidores = null;
            AgruparConsumoGeracao = false;
        }

        public override void UpdateModel() {

            Model.Nome = this.Nome;
            Model.Medidores = this.Medidores;
            Model.AgruparConsumoGeracao = this.AgruparConsumoGeracao;
            Model.Ativo = this.Tipo;
            Model.CodigosUnidades = this.CodigoUnidade;
            Model.CD_Agente_SCDD = this.CodigoAgente_SCDD;

            base.UpdateModel();
        }

        private void btnRemover_Click(object sender, EventArgs e) {
            if (
            MessageBox.Show("Tem certeza que deseja remover: " + this.Model.Nome + "?", "EIG - Medições", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes) {

                if (this.Model.ancestral != null) {
                    this.Model.ancestral.Descendentes.Remove(this.Model);
                    this.Model.ancestral = null;

                } else {
                    parent.Dados.Redes.Remove(this.Model);
                }

                ((TreeNode)this.Node).Remove();

                this.InvokeActionDone();
            }

        }

        private void btnEditar_Click(object sender, EventArgs e) {

            if (this.Node == null || this.Model == null) return;

            var editor = (EditorUserControl<Rede>)Activator.CreateInstance(this.GetType(), parent);
            editor.Bind(this.Node);
            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (frm.ShowDialog() == DialogResult.Yes) {
                this.Bind(Node);
            }
        }

        public override Rede Model {
            get {
                return
                    Node == null ? null :
                    Node.BoundObject;
            }
            set {
                if (value != null) {

                   CodigoUnidade = value.CodigosUnidades;
                    Nome = value.Nome;
                    Medidores = value.Medidores;
                    Tipo = value.Ativo;
                    AgruparConsumoGeracao = value.AgruparConsumoGeracao;
                    CodigoAgente_SCDD = value.CD_Agente_SCDD;

                } else {
                    this.Clear();
                }
            }
        }

        private void btnAdicionarDescendente_Click(object sender, EventArgs e) {
            var editor = (EditorUserControl<Rede>)Activator.CreateInstance(this.GetType(), parent);

            var nRede = new Rede() { Nome = "Novo" };
            var nNode = new RedeNode(nRede, parent);

            editor.Bind(nNode);

            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (frm.ShowDialog() == DialogResult.Yes) {

                if (Model != null) Model.Descendentes.Add(nRede);
                else parent.Dados.Redes.Add(nRede);

                ((TreeNode)this.Node).Nodes.Add(nNode);
                parent.SelectedNode = nNode;

            }
        }

        void parent_SelectionChanged(object sender, TreeViewEventArgs e) {
            if (chkMove.Checked &&
                MessageBox.Show("Tem certeza que deseja mover o objeto para:" + ((ITreeNodeEditor)e.Node).Text + " ?", "EIG - Medições", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {

                if (this.Model.ancestral != null) {
                    this.Model.ancestral.Descendentes.Remove(this.Model);
                    this.Model.ancestral = null;
                }

                if (e.Node is RedeNode) {
                    this.Model.ancestral = ((RedeNode)e.Node).BoundObject;
                    this.Model.ancestral.Descendentes.Add(this.Model);
                }

                ((TreeNode)this.Node).Remove();
                e.Node.Nodes.Add((TreeNode)this.Node);

                chkMove.Checked = false;
            }
        }

        private void chkMove_CheckedChanged(object sender, EventArgs e) {
            if (chkMove.Checked) {
                chkMove.Text = "✘ Cancelar";
                parent.SelectionChanged += parent_SelectionChanged;
                btnAdicionarDescendente.Enabled = btnEditar.Enabled = false;
                MessageBox.Show("Selecione o ponto de destino ao lado");

            } else {
                chkMove.Text = "Mover";
                btnAdicionarDescendente.Enabled = btnEditar.Enabled = true;
                parent.SelectionChanged -= parent_SelectionChanged;
            }
        }

        private void btnVincular_Click(object sender, EventArgs e) {
            FormBuscarCadastro<Agente> frm = new FormBuscarCadastro<Agente>(Repositorio.getAgente);
            if (frm.ShowDialog() == DialogResult.OK) {
                int codi = frm.Selecionado.ID_Unidade;
                var curr = this.CodigoUnidade;
                if (!curr.ContainsKey(codi)) {
                    curr.Add(codi, 1);
                    this.CodigoUnidade = curr;
                    this.Tipo = frm.Selecionado.Tipo;
                    if (frm.Selecionado.CD_AGEN_SCDD.HasValue) this.CodigoAgente_SCDD = frm.Selecionado.CD_AGEN_SCDD.Value.ToString();
                }


            }
        }


    }

    public class EditorUserControl_Rede : EditorUserControl<Rede> {
        public override Rede Model {
            get;
            set;
        }
    }
}
