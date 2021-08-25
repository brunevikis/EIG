using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eig.medicao.winForm.views {
    public partial class viewCadastroPonto : EditorUserControl_Ponto {

        bool _viewOnly;
        public override bool ViewOnly {
            get { return _viewOnly; }
            set {
                _viewOnly = value;

                btnEditar.Visible = _viewOnly;
                                
                btnGravar.Visible =
                btnRemover.Visible = !_viewOnly;


                txtPontoNome.ReadOnly = _viewOnly;
                chkPontoDeConexao.Enabled = !_viewOnly;

            }
        }

        public int? PontoId { get { return string.IsNullOrWhiteSpace(txtPontoId.Text) ? (int?)null : int.Parse(txtPontoId.Text); } set { txtPontoId.Text = value.HasValue ? value.ToString() : ""; } }
        public string PontoCodigo { get { return txtPontoNome.Text; } set { txtPontoNome.Text = value; } }
        public bool Conexao { get { return chkPontoDeConexao.Checked; } set { chkPontoDeConexao.Checked = value; } }


        viewCadastros parent = null;
        public viewCadastroPonto(viewCadastros parent)
            : this() {
            this.parent = parent;
        }
        private viewCadastroPonto() {
            InitializeComponent();
            ViewOnly = true;
        }

        void Clear() {
            PontoId = null;
            PontoCodigo = "";
            Conexao = false;

        }

        public override void Bind(eig.medicao.model.Ponto ponto) {
            this.Model = ponto;

            if (ponto != null) {

                PontoId = ponto.Id;
                PontoCodigo = ponto.Codigo;
                Conexao = ponto.PontoDeConexao;

            } else {
                this.Clear();
            }


        }


        private void btnRemover_Click(object sender, EventArgs e) {
            if (this.Model != null) {
                var ctx = parent.ctx;

                var entry = ctx.Entry(this.Model);
                entry.State = System.Data.Entity.EntityState.Deleted;

                ctx.SaveChanges();

                this.Model = null;
            }

            InvokeActionDone();
        }

        private void btnGravar_Click(object sender, EventArgs e) {
            var ctx = parent.ctx;

            var m = this.Model;

            m.Codigo = this.PontoCodigo;
            m.PontoDeConexao = this.Conexao;

            ctx.Entry(this.Model).State = this.Model.Id == 0 ?
                System.Data.Entity.EntityState.Added :
                System.Data.Entity.EntityState.Modified;

            ctx.SaveChanges();

            //this.Model = m;
            this.InvokeActionDone();
        }


        private async void btnEditar_Click(object sender, EventArgs e) {
            var editor = (EditorUserControl_Ponto)Activator.CreateInstance(this.GetType(), parent);
            editor.Bind(this.Model);
            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (frm.ShowDialog() == DialogResult.OK) {

                this.Bind(editor.Model);

                if (parent != null) await parent.LoadData();
            }
        }

    }

    public class EditorUserControl_Ponto : EditorUserControl<eig.medicao.model.Ponto> { }

}
