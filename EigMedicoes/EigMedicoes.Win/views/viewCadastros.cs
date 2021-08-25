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

   


    public partial class viewCadastros : UserControl {

        public event EventHandler<TreeViewEventArgs> SelectionChanged;

        public bool OpenEditor { get; set; }

        public TreeNode SelectedNode {
            get {
                return treeView1.SelectedNode;
            }
            set {
                treeView1.SelectedNode = value;
            }
        }

        char mode;

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


        public viewCadastros() {
            InitializeComponent();

            OpenEditor = true;

            this.Dock = DockStyle.Fill;
            
            this.mode = 'R';
        }

        public viewCadastros(char mode)
            : this() {

            this.mode = mode;
        }

        private async void viewCadastros_Load(object sender, EventArgs e) {

            if (mode == 'R') await LoadData();
            else if (mode == 'C') await LoadCliData();

        }

        public Contexto Dados { get; private set; }

        public async Task LoadData() {

            this.SuspendLayout();
            textBox1.SuspendLayout();

            var keys = new List<string>();

            treeView1.Nodes.Clear();

            treeView1.EnterLoadingState();
            textBox1.AutoCompleteCustomSource.Clear();

            var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
            var jsondata = System.IO.File.ReadAllText(datafile);



            //TreeNode mainNode = new InicialNode(this);
            TreeNode mainNode = new TreeNodeEditor<Rede, viewCadastroRede>(null, this) { Text = "REDE" };
            //await Task.Factory.StartNew(() => {

            Dados = Contexto.ConfigurarRede(jsondata);
            Action<TreeNodeCollection, List<Rede>> addRange = null;
            addRange = new Action<TreeNodeCollection, List<Rede>>((nodes, redes) => {

                nodes.AddRange(
                redes.OrderBy(r => r.Nome)
                .Select(r => {
                    var ncli = new RedeNode(r, this);
                    keys.Add(r.Nome);

                    addRange(ncli.Nodes, r.Descendentes);

                    return ncli;
                }).ToArray()
                );
            });

            addRange(mainNode.Nodes, Dados.Redes);


            treeView1.ExitLoadingState();

            treeView1.Nodes.Add(mainNode);
            textBox1.AutoCompleteCustomSource.AddRange(keys.ToArray());
            keys.Clear();
            keys = null;

            textBox1.ResumeLayout();
            textBox1.PerformLayout();

            this.ResumeLayout(true);
            this.PerformLayout();
        }

        public async Task LoadCliData() {

            this.SuspendLayout();
            textBox1.SuspendLayout();

            var keys = new List<string>();

            treeView1.Nodes.Clear();

            treeView1.EnterLoadingState();
            textBox1.AutoCompleteCustomSource.Clear();

            var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
            var jsondata = System.IO.File.ReadAllText(datafile);


            //Seleciona o node principal (clientes)
            TreeNode mainNode = new TreeNodeEditor<Cliente, viewCadastroCliente>(null, this) { Text = "CLIENTES" };
            //await Task.Factory.StartNew(() => {

            //Retorna o nome dos clientes no jsondata
            Dados = Contexto.ConfigurarRede(jsondata);
            Action<TreeNodeCollection, List<Cliente>> addRange = null;
            addRange = new Action<TreeNodeCollection, List<Cliente>>((nodes, clientes) => {

                nodes.AddRange(
                clientes
                .OrderBy(c => c.Nome)
                .Select(r => {
                    var ncli = new ClienteNode(r, this);
                    keys.Add(r.Nome);

                    if (r.Ativos.Count > 0)
                        ncli.Nodes.AddRange(
                            r.Ativos.Select(at => new AtivoNode(at, this)).ToArray()
                            );

                    return ncli;
                }).ToArray()
                );
            });

            addRange(mainNode.Nodes, Dados.Clientes);



            treeView1.ExitLoadingState();

            treeView1.Nodes.Add(mainNode);
            textBox1.AutoCompleteCustomSource.AddRange(keys.ToArray());
            keys.Clear();
            keys = null;

            textBox1.ResumeLayout();
            textBox1.PerformLayout();

            this.ResumeLayout(true);
            this.PerformLayout();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            // if (e.Node is ITreeNodeEditor) ((ITreeNodeEditor)e.Node).ShowEditor();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) {
            if (SelectionChanged != null) {
                SelectionChanged(this, e);
            } else {
                if (e.Node is ITreeNodeEditor) ((ITreeNodeEditor)e.Node).ShowEditor();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {

            if (e.KeyChar == (int)Keys.Return) {
                e.Handled = true;

                if (!string.IsNullOrWhiteSpace(textBox1.Text)) {

                    getNext = false;
                    var n = SearchNode(treeView1.Nodes[0], textBox1.Text.ToUpperInvariant(), treeView1.SelectedNode);

                    if (n != null) treeView1.SelectedNode = n;
                }

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {



            if (!string.IsNullOrWhiteSpace(textBox1.Text)) {

                getNext = true;
                var n = SearchNode(treeView1.Nodes[0], textBox1.Text.ToUpperInvariant());

                if (n != null) treeView1.SelectedNode = n;
            }

        }

        bool getNext = false;
        TreeNode SearchNode(TreeNode root, string key, TreeNode current = null) {
            foreach (TreeNode node in root.Nodes) {

                if (!getNext) {
                    if (node == current) getNext = true;
                } else if (((ITreeNodeEditor)node).SearchKey != null &&
                    (
                        ((ITreeNodeEditor)node).SearchKey.StartsWith(key, StringComparison.OrdinalIgnoreCase) ||
                        (key.Length >= 3 && ((ITreeNodeEditor)node).SearchKey.ToUpperInvariant().Contains(key))
                    )
                    )
                    return node;

                var n = SearchNode(node, key, current);
                if (n != null) return n;

            }

            if (root == treeView1.Nodes[0] && current != null) {
                getNext = true;
                return SearchNode(root, key);
            } else
                return null;
        }

        private void button1_Click(object sender, EventArgs e) {
            if (
                MessageBox.Show("Confirmar gravação dos dados?", "EIG - Medições", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                == DialogResult.OK
                ) {

                var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];

                Dados.GravarRede(datafile);

                MessageBox.Show("Gravado!");
            }
        }
    }

    public class viewCadastrosR : viewCadastros {
        public viewCadastrosR()
            : base('R') {
        }
    }

    public class viewCadastrosC : viewCadastros {
        public viewCadastrosC()
            : base('C') {
        }
    }

    //public class InicialNode : TreeNodeEditor<Rede, viewCadastroRede> {

    //    public InicialNode(viewCadastros parent)
    //        : base(null, parent) {
    //        Name = "nodeRoot";
    //        Text = "REDE BÁSICA";
    //    }

    //}

    public class RedeNode : TreeNodeEditor<Rede, viewCadastroRede> {

        public RedeNode(Rede rede, viewCadastros parent) : base(rede, parent) { }
    }

    public class ClienteNode : TreeNodeEditor<Cliente, viewCadastroCliente> { // TreeNode, ITreeNodeEditor {
        public ClienteNode(Cliente cliente, viewCadastros parent) : base(cliente, parent) { }
    }

    public class AtivoNode : TreeNodeEditor<Ativo, viewCadastroClienteAtivo> {

        public AtivoNode(Ativo ativo, viewCadastros parent)
            : base(ativo, parent) {

        }


    }

}
