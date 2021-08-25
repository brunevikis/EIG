using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EigMedicoes.Win.views {
    public partial class viewCadastroAgente : EditorUserControl_Agente {//EditorUserControl<eig.medicao.model.Agente> {

        bool _viewOnly;
        public override bool ViewOnly {
            get { return _viewOnly; }
            set {
                _viewOnly = value;

                btnEditar.Visible = _viewOnly;
                btnAdicionarPonto.Visible = _viewOnly;
                btnGravar.Visible =
                btnRemover.Visible = !_viewOnly;

                txtAgenteNome.ReadOnly = _viewOnly;

                chkAgenteDescontaConsumo.Enabled = !_viewOnly;
            }
        }

        public int? AgenteId { get { return string.IsNullOrWhiteSpace(txtAgenteId.Text) ? (int?)null : int.Parse(txtAgenteId.Text); } set { txtAgenteId.Text = value.HasValue ? value.ToString() : ""; } }
        public string AgenteNome { get { return txtAgenteNome.Text; } set { txtAgenteNome.Text = value; } }
        public bool DescontaConsumo { get { return chkAgenteDescontaConsumo.Checked; } set { chkAgenteDescontaConsumo.Checked = value; } }


        viewCadastros parent = null;
        public viewCadastroAgente(viewCadastros parent)
            : this() {
            this.parent = parent;
        }
        private viewCadastroAgente() {
            InitializeComponent();

            ViewOnly = true;
        }

        public void Clear() {
            AgenteId = null;
            AgenteNome = "";
            DescontaConsumo = false;
        }

        public override void Bind(eig.medicao.model.Agente agente) {

            this.Model = agente;

            if (agente != null) {

                AgenteId = agente.Id;
                AgenteNome = agente.Nome;
                DescontaConsumo = agente.DescontarConsumoDaGeracao;

            } else this.Clear();
        }

        private async void btnEditar_Click(object sender, EventArgs e) {

            var editor = (EditorUserControl_Agente)Activator.CreateInstance(this.GetType(), parent);
            editor.Bind(this.Model);
            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (frm.ShowDialog() == DialogResult.OK) {

                this.Bind(editor.Model);

                if (parent != null) await parent.LoadData();
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

            m.Nome = this.AgenteNome;
            m.DescontarConsumoDaGeracao = this.DescontaConsumo;

            ctx.Entry(this.Model).State = this.Model.Id == 0 ?
                System.Data.Entity.EntityState.Added :
                System.Data.Entity.EntityState.Modified;

            ctx.SaveChanges();

            //this.Model = m;
            this.InvokeActionDone();

        }

        private async void btnAdicionarPonto_Click(object sender, EventArgs e) {
            var editor = new viewCadastroPonto(parent);

            var newM = new eig.medicao.model.Ponto();
            newM.Agente_Id = this.Model.Id;
            editor.Bind(newM);

            FormCadastroEditor frm = new FormCadastroEditor(editor);

            if (frm.ShowDialog() == DialogResult.OK) {

                //this.Model.Agentes.Add(editor.Model);

                if (parent != null) await parent.LoadData();
            }
        }
    }

    public class EditorUserControl_Agente : EditorUserControl<eig.medicao.model.Agente> { }
}
