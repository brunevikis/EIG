namespace eig.medicao.winForm.views {
    partial class viewCadastroAgente {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAgenteDescontaConsumo = new System.Windows.Forms.CheckBox();
            this.txtAgenteId = new System.Windows.Forms.TextBox();
            this.txtAgenteNome = new System.Windows.Forms.TextBox();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnRemover = new System.Windows.Forms.Button();
            this.btnGravar = new System.Windows.Forms.Button();
            this.btnAdicionarPonto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Id";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Nome";
            // 
            // chkAgenteDescontaConsumo
            // 
            this.chkAgenteDescontaConsumo.AutoSize = true;
            this.chkAgenteDescontaConsumo.Location = new System.Drawing.Point(49, 55);
            this.chkAgenteDescontaConsumo.Name = "chkAgenteDescontaConsumo";
            this.chkAgenteDescontaConsumo.Size = new System.Drawing.Size(122, 17);
            this.chkAgenteDescontaConsumo.TabIndex = 5;
            this.chkAgenteDescontaConsumo.Text = "Descontar Consumo";
            this.chkAgenteDescontaConsumo.UseVisualStyleBackColor = true;
            // 
            // txtAgenteId
            // 
            this.txtAgenteId.Location = new System.Drawing.Point(49, 3);
            this.txtAgenteId.Name = "txtAgenteId";
            this.txtAgenteId.ReadOnly = true;
            this.txtAgenteId.Size = new System.Drawing.Size(100, 20);
            this.txtAgenteId.TabIndex = 7;
            // 
            // txtAgenteNome
            // 
            this.txtAgenteNome.Location = new System.Drawing.Point(49, 29);
            this.txtAgenteNome.Name = "txtAgenteNome";
            this.txtAgenteNome.Size = new System.Drawing.Size(172, 20);
            this.txtAgenteNome.TabIndex = 8;
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(227, 3);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(103, 23);
            this.btnEditar.TabIndex = 9;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnRemover
            // 
            this.btnRemover.Location = new System.Drawing.Point(227, 61);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(103, 23);
            this.btnRemover.TabIndex = 10;
            this.btnRemover.Text = "Remover";
            this.btnRemover.UseVisualStyleBackColor = true;
            this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // btnGravar
            // 
            this.btnGravar.Location = new System.Drawing.Point(227, 90);
            this.btnGravar.Name = "btnGravar";
            this.btnGravar.Size = new System.Drawing.Size(103, 23);
            this.btnGravar.TabIndex = 11;
            this.btnGravar.Text = "Gravar";
            this.btnGravar.UseVisualStyleBackColor = true;
            this.btnGravar.Click += new System.EventHandler(this.btnGravar_Click);
            // 
            // btnAdicionarPonto
            // 
            this.btnAdicionarPonto.Location = new System.Drawing.Point(227, 32);
            this.btnAdicionarPonto.Name = "btnAdicionarPonto";
            this.btnAdicionarPonto.Size = new System.Drawing.Size(103, 23);
            this.btnAdicionarPonto.TabIndex = 14;
            this.btnAdicionarPonto.Text = "Adicionar Ponto";
            this.btnAdicionarPonto.UseVisualStyleBackColor = true;
            this.btnAdicionarPonto.Click += new System.EventHandler(this.btnAdicionarPonto_Click);
            // 
            // viewCadastroAgente
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAdicionarPonto);
            this.Controls.Add(this.btnGravar);
            this.Controls.Add(this.btnRemover);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.txtAgenteNome);
            this.Controls.Add(this.txtAgenteId);
            this.Controls.Add(this.chkAgenteDescontaConsumo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "viewCadastroAgente";
            this.Size = new System.Drawing.Size(333, 119);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkAgenteDescontaConsumo;
        private System.Windows.Forms.TextBox txtAgenteId;
        private System.Windows.Forms.TextBox txtAgenteNome;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnRemover;
        private System.Windows.Forms.Button btnGravar;
        private System.Windows.Forms.Button btnAdicionarPonto;
    }
}
