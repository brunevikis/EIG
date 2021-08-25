namespace EigMedicoes.Win.views {
    partial class viewContabilizacao {
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbxMes = new System.Windows.Forms.ComboBox();
            this.cbxAno = new System.Windows.Forms.ComboBox();
            this.btnContabilizar = new System.Windows.Forms.Button();
            this.wbResultView = new System.Windows.Forms.WebBrowser();
            this.cbxClientes = new System.Windows.Forms.ComboBox();
            this.btnEnviaOutlook = new System.Windows.Forms.Button();
            this.btnGravarContabil = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.chkIncluiXls = new System.Windows.Forms.CheckBox();
            this.btnChart = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ListBoxCliente = new System.Windows.Forms.CheckedListBox();
            this.btnSelecionarBase = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbxMarcarDesmarcar = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Contabilização";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mês";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(50, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ano";
            // 
            // cbxMes
            // 
            this.cbxMes.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxMes.FormattingEnabled = true;
            this.cbxMes.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cbxMes.Location = new System.Drawing.Point(7, 44);
            this.cbxMes.Name = "cbxMes";
            this.cbxMes.Size = new System.Drawing.Size(40, 23);
            this.cbxMes.TabIndex = 0;
            // 
            // cbxAno
            // 
            this.cbxAno.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxAno.FormattingEnabled = true;
            this.cbxAno.Items.AddRange(new object[] {
            "2015",
            "2016",
            "2017",
            "2018",
            "2019",
            "2020",
            "2021",
            "2022",
            "2023",
            "2024",
            "2025",
            "2026",
            "2027",
            "2028",
            "2029",
            "2030"});
            this.cbxAno.Location = new System.Drawing.Point(53, 44);
            this.cbxAno.Name = "cbxAno";
            this.cbxAno.Size = new System.Drawing.Size(68, 23);
            this.cbxAno.TabIndex = 1;
            // 
            // btnContabilizar
            // 
            this.btnContabilizar.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContabilizar.Location = new System.Drawing.Point(6, 19);
            this.btnContabilizar.Name = "btnContabilizar";
            this.btnContabilizar.Size = new System.Drawing.Size(115, 23);
            this.btnContabilizar.TabIndex = 2;
            this.btnContabilizar.Text = "Contabilizar";
            this.btnContabilizar.UseVisualStyleBackColor = true;
            this.btnContabilizar.Click += new System.EventHandler(this.btnContabilizar_Click);
            // 
            // wbResultView
            // 
            this.wbResultView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbResultView.Location = new System.Drawing.Point(166, 97);
            this.wbResultView.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbResultView.Name = "wbResultView";
            this.wbResultView.Size = new System.Drawing.Size(615, 383);
            this.wbResultView.TabIndex = 6;
            // 
            // cbxClientes
            // 
            this.cbxClientes.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbxClientes.FormattingEnabled = true;
            this.cbxClientes.Location = new System.Drawing.Point(6, 19);
            this.cbxClientes.Name = "cbxClientes";
            this.cbxClientes.Size = new System.Drawing.Size(138, 23);
            this.cbxClientes.TabIndex = 3;
            this.cbxClientes.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnEnviaOutlook
            // 
            this.btnEnviaOutlook.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnviaOutlook.Location = new System.Drawing.Point(6, 21);
            this.btnEnviaOutlook.Name = "btnEnviaOutlook";
            this.btnEnviaOutlook.Size = new System.Drawing.Size(115, 23);
            this.btnEnviaOutlook.TabIndex = 5;
            this.btnEnviaOutlook.Text = "Enviar (Outlook)";
            this.btnEnviaOutlook.UseVisualStyleBackColor = true;
            this.btnEnviaOutlook.Click += new System.EventHandler(this.btnEnviaOutlook_Click);
            // 
            // btnGravarContabil
            // 
            this.btnGravarContabil.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGravarContabil.Location = new System.Drawing.Point(715, 3);
            this.btnGravarContabil.Name = "btnGravarContabil";
            this.btnGravarContabil.Size = new System.Drawing.Size(63, 23);
            this.btnGravarContabil.TabIndex = 6;
            this.btnGravarContabil.Text = "Gravar";
            this.btnGravarContabil.UseVisualStyleBackColor = true;
            this.btnGravarContabil.Visible = false;
            // 
            // btnExcel
            // 
            this.btnExcel.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExcel.Location = new System.Drawing.Point(6, 48);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(115, 23);
            this.btnExcel.TabIndex = 7;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // chkIncluiXls
            // 
            this.chkIncluiXls.AutoSize = true;
            this.chkIncluiXls.Font = new System.Drawing.Font("Consolas", 1F);
            this.chkIncluiXls.Location = new System.Drawing.Point(732, 27);
            this.chkIncluiXls.Name = "chkIncluiXls";
            this.chkIncluiXls.Size = new System.Drawing.Size(34, 14);
            this.chkIncluiXls.TabIndex = 13;
            this.chkIncluiXls.Text = "Incluir Excel";
            this.chkIncluiXls.UseVisualStyleBackColor = true;
            this.chkIncluiXls.Visible = false;
            // 
            // btnChart
            // 
            this.btnChart.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChart.Location = new System.Drawing.Point(127, 19);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(115, 23);
            this.btnChart.TabIndex = 8;
            this.btnChart.Text = "Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.btnChart_Click);
            // 
            // btnAll
            // 
            this.btnAll.Enabled = false;
            this.btnAll.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAll.Location = new System.Drawing.Point(6, 19);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(115, 23);
            this.btnAll.TabIndex = 14;
            this.btnAll.Text = "Ver Tudo";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxClientes);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(7, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(150, 53);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(6, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Contabilizar 2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ListBoxCliente
            // 
            this.ListBoxCliente.CheckOnClick = true;
            this.ListBoxCliente.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.ListBoxCliente.FormattingEnabled = true;
            this.ListBoxCliente.Location = new System.Drawing.Point(7, 219);
            this.ListBoxCliente.Name = "ListBoxCliente";
            this.ListBoxCliente.Size = new System.Drawing.Size(151, 256);
            this.ListBoxCliente.TabIndex = 19;
            // 
            // btnSelecionarBase
            // 
            this.btnSelecionarBase.Enabled = false;
            this.btnSelecionarBase.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnSelecionarBase.Location = new System.Drawing.Point(7, 158);
            this.btnSelecionarBase.Name = "btnSelecionarBase";
            this.btnSelecionarBase.Size = new System.Drawing.Size(144, 23);
            this.btnSelecionarBase.TabIndex = 23;
            this.btnSelecionarBase.Text = "Selecionar Base";
            this.btnSelecionarBase.UseVisualStyleBackColor = true;
            this.btnSelecionarBase.Click += new System.EventHandler(this.btnSelecionarBase_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAll);
            this.groupBox2.Controls.Add(this.btnExcel);
            this.groupBox2.Controls.Add(this.btnChart);
            this.groupBox2.Enabled = false;
            this.groupBox2.Font = new System.Drawing.Font("Consolas", 9F);
            this.groupBox2.Location = new System.Drawing.Point(311, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 80);
            this.groupBox2.TabIndex = 27;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Consulta";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnEnviaOutlook);
            this.groupBox3.Cursor = System.Windows.Forms.Cursors.Default;
            this.groupBox3.Enabled = false;
            this.groupBox3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(577, 11);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(132, 57);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Enviar Dados";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnContabilizar);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Font = new System.Drawing.Font("Consolas", 9F);
            this.groupBox4.Location = new System.Drawing.Point(163, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(132, 83);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Projeção";
            // 
            // cbxMarcarDesmarcar
            // 
            this.cbxMarcarDesmarcar.AutoSize = true;
            this.cbxMarcarDesmarcar.Enabled = false;
            this.cbxMarcarDesmarcar.Location = new System.Drawing.Point(10, 196);
            this.cbxMarcarDesmarcar.Name = "cbxMarcarDesmarcar";
            this.cbxMarcarDesmarcar.Size = new System.Drawing.Size(148, 17);
            this.cbxMarcarDesmarcar.TabIndex = 30;
            this.cbxMarcarDesmarcar.Text = "Marcar/Desmarcar Todos";
            this.cbxMarcarDesmarcar.UseVisualStyleBackColor = true;
            this.cbxMarcarDesmarcar.CheckedChanged += new System.EventHandler(this.cbxMarcarDesmarcar_CheckedChanged);
            // 
            // viewContabilizacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbxMarcarDesmarcar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkIncluiXls);
            this.Controls.Add(this.btnGravarContabil);
            this.Controls.Add(this.btnSelecionarBase);
            this.Controls.Add(this.ListBoxCliente);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.wbResultView);
            this.Controls.Add(this.cbxAno);
            this.Controls.Add(this.cbxMes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "viewContabilizacao";
            this.Size = new System.Drawing.Size(781, 483);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbxMes;
        private System.Windows.Forms.ComboBox cbxAno;
        private System.Windows.Forms.Button btnContabilizar;
        private System.Windows.Forms.WebBrowser wbResultView;
        private System.Windows.Forms.ComboBox cbxClientes;
        private System.Windows.Forms.Button btnEnviaOutlook;
        private System.Windows.Forms.Button btnGravarContabil;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.CheckBox chkIncluiXls;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox ListBoxCliente;
        private System.Windows.Forms.Button btnSelecionarBase;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbxMarcarDesmarcar;
    }
}
