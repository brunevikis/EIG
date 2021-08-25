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
    public partial class viewCadastroCliente : EditorUserControl_Cliente { //EditorUserControl<eig.medicao.model.Cliente> {

        bool _viewOnly;
        public override bool ViewOnly {
            get { return _viewOnly; }
            set {
                _viewOnly = value;

                btnAdicionarEditar.Visible = _viewOnly;
                btnAdicionarAtivo.Visible = _viewOnly;

                btnRemover.Visible = !_viewOnly;

                txtEmails.ReadOnly =
                txtCCEmails.ReadOnly =
                txtClienteNome.ReadOnly = _viewOnly;
                chkIncluirGraficoContrato.Enabled = chkIncluirGrafico.Enabled = !_viewOnly;
                chkIncluirExcel.Enabled = !_viewOnly;
                lbxAgendaDeEnvio.Enabled = !_viewOnly;
            }
        }

        public string ClienteNome { get { return txtClienteNome.Text; } set { txtClienteNome.Text = value; } }
        public List<string> Emails { get { return txtEmails.Lines.SelectMany(x => x.Split(';')).ToList(); } set { txtEmails.Lines = value != null ? value.ToArray() : null; } }
        public List<string> CCEmails { get { return txtCCEmails.Lines.SelectMany(x => x.Split(';')).ToList(); } set { txtCCEmails.Lines = value != null ? value.ToArray() : null; } }

        public bool IncluirGrafico { get { return chkIncluirGrafico.Checked; } set { chkIncluirGrafico.Checked = value; } }
        public bool IncluirGraficoContrato { get { return chkIncluirGraficoContrato.Checked; } set { chkIncluirGraficoContrato.Checked = value; } }

        public bool IncluirExcel { get { return chkIncluirExcel.Checked; } set { chkIncluirExcel.Checked = value; } }

        public List<bool> AgendaDeEnvio { get {return ConstruirAgenda(lbxAgendaDeEnvio); } set { lbxAgendaDeEnvio = ConstruirViewAgenda(lbxAgendaDeEnvio, value); } }
       
        public List<bool> ConstruirAgenda (CheckedListBox checkedListBox)
        {
            List<bool> agenda = new List<bool>();

            for (var i=0 ; i<=checkedListBox.Items.Count - 1; i++)
            {
                agenda.Add(checkedListBox.GetItemChecked(i));
            }

            return agenda;
        } 

        public CheckedListBox ConstruirViewAgenda (CheckedListBox checkedListBox, List<bool> list)
        {
            for (var i=0; i<7; i++)
            {
                try
                {
                    if (list.Count != 0 && list.ElementAt(i))
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Checked);
                    }
                    else
                    {
                        checkedListBox.SetItemCheckState(i, CheckState.Unchecked);
                    }

                } catch (Exception ex)
                {
                    break;
                }
            }

            return checkedListBox;
        }

        viewCadastros parent = null;
        public viewCadastroCliente(viewCadastros parent)
            : this() {
            this.parent = parent;
        }

        private viewCadastroCliente() {
            InitializeComponent();
            ViewOnly = true;
        }

        void Clear() {

            ClienteNome = "";
            Emails = null;
            CCEmails = null;
            IncluirGrafico = false;
            IncluirGraficoContrato = false;
            IncluirExcel = false;
            AgendaDeEnvio = null;
        }

        public override void UpdateModel() {

            Model.Nome = this.ClienteNome;
            Model.Emails = this.Emails;
            Model.CCEmails = this.CCEmails;
            Model.IncluirGrafico = this.IncluirGrafico;
            Model.IncluirGraficoContrato = this.IncluirGraficoContrato;
            Model.IncluirExcel = this.IncluirExcel;
            Model.AgendaDeEnvio = this.AgendaDeEnvio;


            base.UpdateModel();
        }

        public override Cliente Model {
            get {
                return
                    Node == null ? null :
                    Node.BoundObject;
            }
            set {
                if (value != null) {
                    ClienteNome = value.Nome;
                    Emails = value.Emails;
                    CCEmails = value.CCEmails;
                    IncluirGrafico = value.IncluirGrafico;
                    IncluirGraficoContrato = value.IncluirGraficoContrato;
                    IncluirExcel = value.IncluirExcel;
                    AgendaDeEnvio = value.AgendaDeEnvio;


                } else {
                    this.Clear();
                }
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e) {

            var editor = (EditorUserControl<Cliente>)Activator.CreateInstance(this.GetType(), parent);
            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (this.Node == null || this.Model == null) {

                var nCliente = new Cliente() { Nome = "Novo" };
                var nNode = new ClienteNode(nCliente, parent);

                editor.Bind(nNode);

                if (frm.ShowDialog() == DialogResult.Yes) {
                    this.Bind(Node);
                    ((TreeNode)this.Node).Nodes.Add(nNode);
                    parent.Dados.Clientes.Add(nCliente);
                    parent.SelectedNode = nNode;
                }

            } else {

                editor.Bind(this.Node);
                if (frm.ShowDialog() == DialogResult.Yes) {
                    this.Bind(Node);
                }

            }




        }

        private void btnRemover_Click(object sender, EventArgs e) {
            if (
            MessageBox.Show("Tem certeza que deseja remover: " + this.Model.Nome + "?", "EIG - Medições", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes) {

                parent.Dados.Clientes.Remove(this.Model);
                ((TreeNode)this.Node).Remove();

                this.InvokeActionDone();
            }
        }

        private void btnGravar_Click(object sender, EventArgs e) {

            //var ctx = parent.ctx;

            //if (this.Model == null) {
            //    this.Model = new model.Cliente() {
            //        Nome = this.ClienteNome,
            //        Contatos = this.Contatos,
            //        ApuraConsumo = this.Consumo,
            //        ApuraGeracao = this.Geracao,
            //    };
            //    this.Model.Redes.Add(new model.Rede());

            //    ctx.Clientes.Add(this.Model);
            //    ctx.SaveChanges();

            //    this.InvokeActionDone();
            //} else {

            //    var m = this.Model;

            //    m.Nome = this.ClienteNome;
            //    m.Contatos = this.Contatos;
            //    m.ApuraGeracao = this.Geracao;
            //    m.ApuraConsumo = this.Consumo;

            //    ctx.SaveChanges();

            //    //this.Model = m;
            //    this.InvokeActionDone();
            //}
        }

        public List<Ativo> getAtivos() {
            return parent.Dados.FlatRedes.Where(c => !string.IsNullOrWhiteSpace(c.Ativo))
                .Select(x => new Ativo { Rede = x })
                .ToList();
        }

        private void btnAdicionarAtivo_Click(object sender, EventArgs e) {
            if (this.Node == null || this.Model == null) return;
            FormBuscarCadastro<Ativo> frm = new FormBuscarCadastro<Ativo>(getAtivos);
            if (frm.ShowDialog() == DialogResult.OK) {


                //int codi = frm.Cadastro.Codigo;
                var curr = this.Model.Ativos ?? new List<Ativo>();
                if (!curr.Any(a => a.Nome == frm.Selecionado.Nome)) {

                    var nNode = new AtivoNode(frm.Selecionado, parent);
                    curr.Add(frm.Selecionado);
                    this.Model.Ativos = curr;
                    ((TreeNode)this.Node).Nodes.Add(nNode);

                }



            }
        }

        private void viewCadastroCliente_Load(object sender, EventArgs e) {

        }

    }

    public class EditorUserControl_Cliente : EditorUserControl<Cliente> {
        public override Cliente Model {
            get;
            set;
        }
    }
}
