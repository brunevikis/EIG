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
    public partial class viewImportacao : UserControl {

        // eig.medicao.model.EIG_MEDIDOREntities ctx = new eig.medicao.model.EIG_MEDIDOREntities();
        public viewImportacao() {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void btnImportar_Click(object sender, EventArgs e) {

            try {


                OpenFileDialog ofd = new OpenFileDialog() {
                    Filter = "arquivo medicoes|*.csv;*.xml",
                    Multiselect = false,
                };

                var dataFileDirectory = System.Configuration.ConfigurationManager.AppSettings["importfolder"];
                //var dataFileDirectory = @"\\192.168.0.8\Depto\TI - Sistemas\UAT\EIG-Medidor\DataFiles\";

                ofd.InitialDirectory = dataFileDirectory;
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK) {

                    btnImportar.EnterLoadingState();
                    btnImportar.SetErrorState(null);

                    if (MessageBox.Show("Confirmar importação de \'" + ofd.FileName + "\"", "Confirmar importação", MessageBoxButtons.YesNo) == DialogResult.Yes) {

                        var filename = ofd.FileName;
                        if (!ofd.FileName.StartsWith(dataFileDirectory)) {

                            var xt = System.IO.Path.GetExtension(filename);

                            filename = System.IO.Path.Combine(
                                    dataFileDirectory,
                                    System.IO.Path.GetFileNameWithoutExtension(ofd.FileName)
                                    + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") );

                            filename = System.IO.Path.ChangeExtension(filename, xt);

                            System.IO.File.Copy(ofd.FileName,
                                filename
                                    );
                        }

                        Func<string, int> importFunc;
                        
                        


                        if (filename.ToUpper().EndsWith(".CSV")) {
                            importFunc = EigMedicoes.Modelo.Repositorio.ImportarDadosParaStage;
                        } else if (filename.ToUpper().EndsWith(".XML")) {
                            //importFunc = EigMedicoes.Modelo.Repositorio.ImportarDadosParaStage;
                            importFunc = EigMedicoes.Modelo.Repositorio.ImportarDadosXMLParaStage;
                        } else {
                            txtLog.Text = "[" + DateTime.Now.ToString() + "] " + "Arquivo inválido" + Environment.NewLine + txtLog.Text;
                            return;
                        }


                        var outputInt = importFunc(filename);

                        txtLog.Text = "[" + DateTime.Now.ToString() + "] " + outputInt.ToString() +
                            " linhas importadas."
                            + Environment.NewLine + txtLog.Text;
                    }
                }
            } catch (Exception ex) {
                btnImportar.SetErrorState(ex);

                txtLog.Text = "[" + DateTime.Now.ToString() + "] " + ex.Message
                            + Environment.NewLine + txtLog.Text;


            } finally {
                btnImportar.ExitLoadingState();
            }
        }

        private void button1_Click(object sender, EventArgs e) {

            try {

                if (MessageBox.Show("Confirmar processamento dos dados importados?"
                    + Environment.NewLine + "Dados existentes da data informada serão sobrescritos!"

                    , "Confirmar processamento dos dados importados", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes) {

                    button1.EnterLoadingState();
                    button1.SetErrorState(null);

                    var outputInt = EigMedicoes.Modelo.Repositorio.ProcessarDados();

                    txtLog.Text = "[" + DateTime.Now.ToString() + "] " + outputInt.Item1.ToString() +
                        " medicoes processadas." + (outputInt.Item2 > 0 ? " Medições faltantes: " + outputInt.Item2.ToString() : "")
                        + Environment.NewLine + txtLog.Text;
                }
            } catch (Exception ex) {
                button1.SetErrorState(ex);

                txtLog.Text = "[" + DateTime.Now.ToString() + "] " + ex.Message
                            + Environment.NewLine + txtLog.Text;
            } finally {
                button1.ExitLoadingState();
            }
        }
    }
}
