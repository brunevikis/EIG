namespace EigMedicoes.Win.views {
    partial class viewCadastroRede {
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
            this.txtNome = new System.Windows.Forms.TextBox();
            this.btnAdicionarDescendente = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtCodUnidade = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMedidores = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRemover = new System.Windows.Forms.Button();
            this.btnVincular = new System.Windows.Forms.Button();
            this.chkAgrupar = new System.Windows.Forms.CheckBox();
            this.chkMove = new System.Windows.Forms.CheckBox();
            this.txtCdAgente = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nome";
            // 
            // txtNome
            // 
            this.txtNome.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNome.Location = new System.Drawing.Point(44, 5);
            this.txtNome.Name = "txtNome";
            this.txtNome.Size = new System.Drawing.Size(149, 23);
            this.txtNome.TabIndex = 5;
            // 
            // btnAdicionarDescendente
            // 
            this.btnAdicionarDescendente.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdicionarDescendente.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdicionarDescendente.Location = new System.Drawing.Point(199, 51);
            this.btnAdicionarDescendente.Name = "btnAdicionarDescendente";
            this.btnAdicionarDescendente.Size = new System.Drawing.Size(103, 44);
            this.btnAdicionarDescendente.TabIndex = 1;
            this.btnAdicionarDescendente.Text = "Adicionar Descendente";
            this.btnAdicionarDescendente.UseVisualStyleBackColor = true;
            this.btnAdicionarDescendente.Click += new System.EventHandler(this.btnAdicionarDescendente_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Location = new System.Drawing.Point(199, 5);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(103, 23);
            this.btnEditar.TabIndex = 0;
            this.btnEditar.Text = "✎ Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "[G]erador\t",
            "[C]onsumidor"});
            this.comboBox1.Location = new System.Drawing.Point(94, 177);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(127, 23);
            this.comboBox1.TabIndex = 7;
            // 
            // txtCodUnidade
            // 
            this.txtCodUnidade.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCodUnidade.Location = new System.Drawing.Point(3, 250);
            this.txtCodUnidade.Multiline = true;
            this.txtCodUnidade.Name = "txtCodUnidade";
            this.txtCodUnidade.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtCodUnidade.Size = new System.Drawing.Size(299, 40);
            this.txtCodUnidade.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Tipo de Ativo";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 234);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Códigos das Unidades:\r\n";
            // 
            // txtMedidores
            // 
            this.txtMedidores.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMedidores.Location = new System.Drawing.Point(6, 51);
            this.txtMedidores.Multiline = true;
            this.txtMedidores.Name = "txtMedidores";
            this.txtMedidores.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMedidores.Size = new System.Drawing.Size(187, 102);
            this.txtMedidores.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Medidores:";
            // 
            // btnRemover
            // 
            this.btnRemover.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemover.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemover.Location = new System.Drawing.Point(199, 328);
            this.btnRemover.Name = "btnRemover";
            this.btnRemover.Size = new System.Drawing.Size(103, 23);
            this.btnRemover.TabIndex = 4;
            this.btnRemover.Text = "Remover";
            this.btnRemover.UseVisualStyleBackColor = true;
            this.btnRemover.Click += new System.EventHandler(this.btnRemover_Click);
            // 
            // btnVincular
            // 
            this.btnVincular.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVincular.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVincular.Location = new System.Drawing.Point(3, 296);
            this.btnVincular.Name = "btnVincular";
            this.btnVincular.Size = new System.Drawing.Size(67, 55);
            this.btnVincular.TabIndex = 2;
            this.btnVincular.Text = "Buscar Código";
            this.btnVincular.UseVisualStyleBackColor = true;
            this.btnVincular.Click += new System.EventHandler(this.btnVincular_Click);
            // 
            // chkAgrupar
            // 
            this.chkAgrupar.AutoSize = true;
            this.chkAgrupar.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAgrupar.Location = new System.Drawing.Point(6, 154);
            this.chkAgrupar.Name = "chkAgrupar";
            this.chkAgrupar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkAgrupar.Size = new System.Drawing.Size(176, 17);
            this.chkAgrupar.TabIndex = 25;
            this.chkAgrupar.Text = "Agrupar Consumo e Geração";
            this.chkAgrupar.UseVisualStyleBackColor = true;
            // 
            // chkMove
            // 
            this.chkMove.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMove.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMove.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.chkMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkMove.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMove.Location = new System.Drawing.Point(199, 299);
            this.chkMove.Name = "chkMove";
            this.chkMove.Size = new System.Drawing.Size(103, 23);
            this.chkMove.TabIndex = 3;
            this.chkMove.Text = "Mover";
            this.chkMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMove.UseVisualStyleBackColor = true;
            this.chkMove.CheckedChanged += new System.EventHandler(this.chkMove_CheckedChanged);
            // 
            // txtCdAgente
            // 
            this.txtCdAgente.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCdAgente.Location = new System.Drawing.Point(94, 206);
            this.txtCdAgente.Name = "txtCdAgente";
            this.txtCdAgente.Size = new System.Drawing.Size(208, 23);
            this.txtCdAgente.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "CD_AGENTE_SCDD";
            // 
            // viewCadastroRede
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtCdAgente);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkMove);
            this.Controls.Add(this.chkAgrupar);
            this.Controls.Add(this.btnVincular);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtMedidores);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCodUnidade);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnAdicionarDescendente);
            this.Controls.Add(this.btnRemover);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.label1);
            this.Name = "viewCadastroRede";
            this.Size = new System.Drawing.Size(305, 354);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Button btnAdicionarDescendente;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox txtCodUnidade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMedidores;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRemover;
        private System.Windows.Forms.Button btnVincular;
        private System.Windows.Forms.CheckBox chkAgrupar;
        private System.Windows.Forms.CheckBox chkMove;
        private System.Windows.Forms.TextBox txtCdAgente;
        private System.Windows.Forms.Label label5;
    }
}
