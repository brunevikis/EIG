namespace EigMedicoes.Win {
    partial class FormMain {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuPanel = new System.Windows.Forms.Panel();
            this.btnCadastrosCli = new System.Windows.Forms.Button();
            this.btnImportacao = new System.Windows.Forms.Button();
            this.btnContabilizacao = new System.Windows.Forms.Button();
            this.btnCadastros = new System.Windows.Forms.Button();
            this.btnVisualizarDados = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.menuPanel.BackColor = System.Drawing.SystemColors.MenuBar;
            this.menuPanel.Controls.Add(this.btnCadastrosCli);
            this.menuPanel.Controls.Add(this.btnImportacao);
            this.menuPanel.Controls.Add(this.btnContabilizacao);
            this.menuPanel.Controls.Add(this.btnCadastros);
            this.menuPanel.Controls.Add(this.btnVisualizarDados);
            this.menuPanel.Location = new System.Drawing.Point(-1, 12);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(201, 483);
            this.menuPanel.TabIndex = 0;
            // 
            // btnCadastrosCli
            // 
            this.btnCadastrosCli.FlatAppearance.BorderSize = 0;
            this.btnCadastrosCli.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastrosCli.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastrosCli.Location = new System.Drawing.Point(0, 114);
            this.btnCadastrosCli.Margin = new System.Windows.Forms.Padding(0);
            this.btnCadastrosCli.Name = "btnCadastrosCli";
            this.btnCadastrosCli.Size = new System.Drawing.Size(201, 38);
            this.btnCadastrosCli.TabIndex = 3;
            this.btnCadastrosCli.Text = "Cadastro Cliente";
            this.btnCadastrosCli.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCadastrosCli.UseVisualStyleBackColor = true;
            this.btnCadastrosCli.Click += new System.EventHandler(this.btnCadastrosCli_Click);
            // 
            // btnImportacao
            // 
            this.btnImportacao.FlatAppearance.BorderSize = 0;
            this.btnImportacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportacao.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportacao.Location = new System.Drawing.Point(0, 0);
            this.btnImportacao.Margin = new System.Windows.Forms.Padding(0);
            this.btnImportacao.Name = "btnImportacao";
            this.btnImportacao.Size = new System.Drawing.Size(201, 38);
            this.btnImportacao.TabIndex = 0;
            this.btnImportacao.Text = "Importação";
            this.btnImportacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportacao.UseVisualStyleBackColor = true;
            this.btnImportacao.Click += new System.EventHandler(this.btnImportacao_Click);
            // 
            // btnContabilizacao
            // 
            this.btnContabilizacao.FlatAppearance.BorderSize = 0;
            this.btnContabilizacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContabilizacao.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContabilizacao.Location = new System.Drawing.Point(0, 152);
            this.btnContabilizacao.Margin = new System.Windows.Forms.Padding(0);
            this.btnContabilizacao.Name = "btnContabilizacao";
            this.btnContabilizacao.Size = new System.Drawing.Size(201, 38);
            this.btnContabilizacao.TabIndex = 4;
            this.btnContabilizacao.Text = "Contabilização";
            this.btnContabilizacao.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnContabilizacao.UseVisualStyleBackColor = true;
            this.btnContabilizacao.Click += new System.EventHandler(this.btnContabilizacao_Click);
            // 
            // btnCadastros
            // 
            this.btnCadastros.FlatAppearance.BorderSize = 0;
            this.btnCadastros.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCadastros.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCadastros.Location = new System.Drawing.Point(0, 76);
            this.btnCadastros.Margin = new System.Windows.Forms.Padding(0);
            this.btnCadastros.Name = "btnCadastros";
            this.btnCadastros.Size = new System.Drawing.Size(201, 38);
            this.btnCadastros.TabIndex = 2;
            this.btnCadastros.Text = "Cadastro Rede";
            this.btnCadastros.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCadastros.UseVisualStyleBackColor = true;
            this.btnCadastros.Click += new System.EventHandler(this.btnCadastros_Click);
            // 
            // btnVisualizarDados
            // 
            this.btnVisualizarDados.FlatAppearance.BorderSize = 0;
            this.btnVisualizarDados.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVisualizarDados.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVisualizarDados.Location = new System.Drawing.Point(0, 38);
            this.btnVisualizarDados.Margin = new System.Windows.Forms.Padding(0);
            this.btnVisualizarDados.Name = "btnVisualizarDados";
            this.btnVisualizarDados.Size = new System.Drawing.Size(201, 38);
            this.btnVisualizarDados.TabIndex = 1;
            this.btnVisualizarDados.Text = "Visualizar Dados";
            this.btnVisualizarDados.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVisualizarDados.UseVisualStyleBackColor = true;
            this.btnVisualizarDados.Click += new System.EventHandler(this.btnVisualizarDados_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(206, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(781, 483);
            this.panel1.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "EIG - Medições";
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel menuPanel;
        private System.Windows.Forms.Button btnVisualizarDados;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCadastros;
        private System.Windows.Forms.Button btnContabilizacao;
        private System.Windows.Forms.Button btnImportacao;
        private System.Windows.Forms.Button btnCadastrosCli;
    }
}

