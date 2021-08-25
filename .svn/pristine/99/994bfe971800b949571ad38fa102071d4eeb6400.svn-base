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
using EigMedicoes.Modelo.View;
using System.Globalization;

namespace EigMedicoes.Win.views
{
    public partial class viewContabilizacao : UserControl
    {

        Contexto ctx = null;

        public viewContabilizacao()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            cbxMes.SelectedItem = DateTime.Today.Month.ToString();
            cbxAno.SelectedItem = DateTime.Today.Year.ToString();
        }

        Dictionary<string, string> clienteToHtml = new Dictionary<string, string>();
        Dictionary<string, string> clienteEmails = new Dictionary<string, string>();
        Dictionary<string, string> clienteCCEmails = new Dictionary<string, string>();
        Dictionary<string, List<Sumario>> clienteSumarios = new Dictionary<string, List<Sumario>>();
        Dictionary<string, List<MedicaoCliente>> clienteBalancos = new Dictionary<string, List<MedicaoCliente>>();

        private async void btnContabilizar_Click(object sender, EventArgs e)
        {

            try
            {

                clienteToHtml.Clear();
                clienteCCEmails.Clear();
                clienteEmails.Clear();
                clienteSumarios.Clear();
                clienteBalancos.Clear();
                ListBoxCliente.Items.Clear();

                btnContabilizar.EnterLoadingState();
                btnContabilizar.SetErrorState(null);

                var ano = int.Parse(cbxAno.SelectedItem.ToString());
                var mes = int.Parse(cbxMes.SelectedItem.ToString());

                bool cancel = false;

                await Task.Factory.StartNew(() =>
                {

                    var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
                    var jsondata = System.IO.File.ReadAllText(datafile);

                    var med = Repositorio.getMedicoes(mes, ano);
                    //var contratos = Repositorio.getContratos(mes, ano);
                    var contratos = Repositorio.getParticionamento(new DateTime(ano, mes, 1));
                    var agentes = Repositorio.getAgente();


                    ctx = Contexto.ConfigurarRede(jsondata);
                    ctx.CarregaMedicoes(med);

                    var sumarios = ctx.ConstruirSumarios(mes, ano);



                    foreach (var s in sumarios.GroupBy(x => x.Cliente))
                    {

                        clienteToHtml.Add(s.Key,
                            Sumario.HtmlBase.Replace("{0}",
                                String.Join(" ", s.Select(x => x.ToHtmlTable()))).
                                Replace("{1}",
                                @"<p>Prezados,</p><p>Seguem as medições coletadas do SCDE até o dia <strong>" + s.Max(y => y.Fim).ToString("dd/MM/yyyy") + "</strong></p>")
                            );

                        clienteEmails.Add(s.Key,
                            string.Join(";", ctx.Clientes.First(x => x.Nome == s.Key).Emails)
                            );

                        clienteCCEmails.Add(s.Key,
                            string.Join(";", ctx.Clientes.First(x => x.Nome == s.Key).CCEmails)
                            );

                        clienteSumarios.Add(s.Key,
                            s.ToList());



                        var q = s.SelectMany(sum =>
                        {
                            //if (cancel || (sum.CodigosUnidades.Count > 1 &&
                            //    FormConfirmaRateio.Show(sum, agentes) != DialogResult.OK)) {
                            //    cancel = true;
                            //}

                            return sum.CodigosUnidades.Select(und => new { Dados = sum, perc = und.Value, id_unidade = und.Key });
                        })
                        .Where(x => x.Dados.CodigosUnidades.Count > 0 && !string.IsNullOrWhiteSpace(x.Dados.CD_Agente_SCDD))
                        .ToList();


                        clienteBalancos.Add(s.Key,
                            q.Select(sum =>
                            {

                                var inicio = sum.Dados.Inicio.AddHours(2);
                                inicio = new DateTime(inicio.Year, inicio.Month, 1);
                                var fim = inicio.AddMonths(1).AddDays(-1);
                                var medicaoProjetada = sum.Dados.TipoAtivo == "G" ? sum.Dados.ProjecaoGeracaoHora : sum.Dados.ProjecaoConsumoMedHora;
                                var medicao = sum.Dados.TipoAtivo == "G" ? sum.Dados.GeracaoMedHora : sum.Dados.ConsumoMedHora;
                                var possuiProj = sum.Dados.HorasFaltantes > 0 || (sum.Dados.Fim < Util.getIniFim(mes, ano).Item2.AddHours(-1));
                                var tipo = sum.Dados.TipoAtivo;

                                return new
                                {
                                    sum.id_unidade,
                                    sum.Dados.CD_Agente_SCDD,
                                    inicio,
                                    fim,
                                    medida = medicao * sum.perc,
                                    medidaProjetada = medicaoProjetada * sum.perc,
                                    sum.Dados.HorasFaltantes,
                                    possuiProj,
                                    tipo
                                };

                            })
                            .GroupBy(x => x.id_unidade)
                            .Select(x =>
                            {
                                var first = x.First();
                                return new
                                {
                                    id_unidade = x.Key,
                                    first.CD_Agente_SCDD,
                                    first.inicio,
                                    first.fim,

                                    medida = x.Sum(y => y.medida),
                                    medidaProjetada = x.Sum(y => y.medidaProjetada),
                                    horas_falt = x.Sum(y => y.HorasFaltantes),
                                    possui_proj = x.Any(y => y.possuiProj),
                                    tipo = first.tipo
                                };
                            }).Select(sum =>
                                new MedicaoCliente()
                                {
                                    ID_Unidade = sum.id_unidade,
                                    CD_AGEN_SCDD = sum.CD_Agente_SCDD,
                                    Inicio = sum.inicio,
                                    Fim = sum.fim,
                                    Montante = sum.medida,
                                    MontanteProjetado = sum.medidaProjetada,
                                    HorasFaltantes = sum.horas_falt,
                                    PossuiProjecao = sum.possui_proj,
                                    //Contratado = CalcularContratato(contratos, sum.id_unidade, sum.inicio),
                                    Unidade = contratos.Any(x => x.ID_UNIDADE.Value == sum.id_unidade) ? contratos.First(x => x.ID_UNIDADE.Value == sum.id_unidade).IDENTIFICACAO_UNIDADE : string.Empty,
                                    //Unidade = contratos.Any(x => x.ID_UNIDADE == sum.id_unidade) ? contratos.First(x => x.ID_UNIDADE == sum.id_unidade).IDENTIFICACAO_UNIDADE : string.Empty,
                                    Tipo = sum.tipo,
                                    FatorPerda = agentes.First(x => x.ID_Unidade == sum.id_unidade).FatorPerda,
                                }
                            ).ToList()
                        );
                    }

                    var allBalancos = clienteBalancos.SelectMany(x => x.Value).ToList();
                    allBalancos.ForEach(z => z.Contratado = Particionamento.CalculaContratado(z.Inicio, z.ID_Unidade, z.MontanteProjetado, z.FatorPerda, contratos, allBalancos));
                });

                //Preenchimento da listbox antiga
                cbxClientes.DataSource = clienteToHtml.Keys.ToList();

                //CheckListBox nova
                foreach (var x in clienteToHtml.Keys.ToList())
                {
                    ListBoxCliente.Items.Add(x.ToString());
                }

                if (!cancel && MessageBox.Show("Gravar contabilizações?", "EIG Medições", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    var resultados = new StringBuilder();

                    clienteBalancos.Values.SelectMany(x => x).ToList().ForEach(bal =>
                    {
                        try
                        {
                            resultados.AppendLine(bal.ID_Unidade + " " + //""
                                Repositorio.GravarMedicaoCliente(bal.ID_Unidade, bal.CD_AGEN_SCDD,
                                    bal.Inicio, bal.Fim, bal.MontanteProjetado, bal.HorasFaltantes, bal.PossuiProjecao)
                                );
                        }
                        catch (Exception ex)
                        {
                            resultados.AppendLine(ex.Message);
                        }
                    });


                    if (string.IsNullOrWhiteSpace(resultados.ToString()))
                        MessageBox.Show("Nada foi feito, verifique se os ativos possuem um código de cadastro relacionado");
                    else
                    {
                        FormDialog.Show(resultados.ToString());
                    }
                }

                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                //btnSelecionarCliente.Enabled = true;
                //btnDesmarcarCliente.Enabled = true;
                cbxMarcarDesmarcar.Enabled = true;
                btnSelecionarBase.Enabled = true;
                btnAll.Enabled = true;

            }
            catch (Exception ex)
            {

                btnContabilizar.SetErrorState(ex);
            }
            finally
            {
                btnContabilizar.ExitLoadingState();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clienteToHtml.ContainsKey(cbxClientes.SelectedItem.ToString()))
            {
                wbResultView.DocumentText = clienteToHtml[cbxClientes.SelectedItem.ToString()];
            }
        }

        private void btnEnviaOutlook_Click(object sender, EventArgs e)
        {
            List<String> listaClientes = verifClienteSelec();

            if (listaClientes.Count == 0)
            {
                listaClientes.Add(cbxClientes.SelectedItem.ToString());
            }

            foreach (var y in listaClientes)
            {
                var cliente = y;
                //var cliente = cbxClientes.SelectedItem.ToString();

                if (!clienteToHtml.ContainsKey(cliente))
                {
                    MessageBox.Show("Cliente não contabilizado");
                    return;
                }

                var msgBody = clienteToHtml[cliente];

                var to = clienteEmails[cliente];
                var ccto = clienteCCEmails[cliente];
                var subj = "Relatório de Medição - " + (new DateTime(int.Parse(cbxAno.SelectedItem.ToString()), int.Parse(cbxMes.SelectedItem.ToString()), 1)).ToString("MMM/yyyy");

                var attcs = new List<string>();

                //if (chkIncluiXls.Checked)
                if (ctx.Clientes.First(x => x.Nome == cliente).IncluirExcel)
                {

                    var ano = int.Parse(cbxAno.SelectedItem.ToString());
                    var mes = int.Parse(cbxMes.SelectedItem.ToString());
                    var xls = System.IO.Path.GetTempFileName();
                    xls = System.IO.Path.ChangeExtension(xls, "xlsx");

                    EigMedicoes.Excel.Tools.xlsx(ctx, cliente, mes, ano, xls);

                    attcs.Add(xls);

                }




                if (ctx.Clientes.First(x => x.Nome == cliente).IncluirGrafico)
                {



                    foreach (var s in clienteSumarios[cliente])
                    {

                        var jpg = System.IO.Path.GetTempFileName();
                        jpg = System.IO.Path.ChangeExtension(jpg, "jpg");

                        FormChart frmc = new FormChart(s);
                        frmc.BuildChart();
                        frmc.SaveChart(jpg);

                        attcs.Add(jpg);

                        msgBody = msgBody.Replace("<div id='chart-" + s.Ativo + "'>",
                            "<div id='chart-" + s.Ativo + "'><img src='cid:" + jpg.GetHashCode().ToString() + "'>"
                        );
                    }

                    //if (ctx.Clientes.First(x => x.Nome == cliente).IncluirGraficoContrato) {
                    {
                        StringBuilder chartsHtml = new StringBuilder();

                        clienteBalancos[cliente].ForEach(bal =>
                        {
                            var jpg = System.IO.Path.GetTempFileName();
                            jpg = System.IO.Path.ChangeExtension(jpg, "jpg");


                            if (bal.Contratado != 0)
                            {
                                FormChartContrato f = new FormChartContrato(bal);
                                f.BuildChart();
                                f.SaveChart(jpg);

                                attcs.Add(jpg);
                                chartsHtml.AppendLine("<img src='cid:" + jpg.GetHashCode().ToString() + "'>");

                            }
                        });

                        msgBody = msgBody.Replace("<div id='chart'>",
                                    "<div id='chart'>" + chartsHtml.ToString());

                    }
                }

                Mensagem.OpenNewMessage(msgBody, subj, to, ccto, modal: false, attachs: attcs.ToArray());
            }
        }

        private async void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {

                btnExcel.EnterLoadingState();
                btnExcel.SetErrorState(null);

                List<String> listaClientes = verifClienteSelec();

                if (listaClientes.Count == 0)
                {
                    listaClientes.Add(cbxClientes.SelectedItem.ToString());
                }

                foreach (var y in listaClientes)
                {
                    var cliente = y;

                    //var cliente = cbxClientes.SelectedItem.ToString();

                    if (!clienteSumarios.ContainsKey(cliente))
                    {
                        MessageBox.Show("Cliente não contabilizado");
                        return;
                    }

                    var ano = int.Parse(cbxAno.SelectedItem.ToString());
                    var mes = int.Parse(cbxMes.SelectedItem.ToString());

                    await Task.Factory.StartNew(() =>
                    {
                        Excel.Tools.xlsx(ctx, cliente, mes, ano);
                    });
                }

            }
            catch (Exception ex)
            {
                btnExcel.SetErrorState(ex);
            }
            finally
            {
                btnExcel.ExitLoadingState();
            }
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            try
            {

                btnChart.EnterLoadingState();
                btnChart.SetErrorState(null);

                List<String> listaClientes = verifClienteSelec();

                if (listaClientes.Count == 0)
                {
                    listaClientes.Add(cbxClientes.SelectedItem.ToString());
                }

                foreach (var y in listaClientes)
                {
                    var cliente = y;
                    //var cliente = cbxClientes.SelectedItem.ToString();

                    if (!clienteSumarios.ContainsKey(cliente))
                    {
                        MessageBox.Show("Cliente não contabilizado");
                        return;
                    }


                    clienteSumarios[cliente].ForEach(s =>
                    {
                        FormChart f = new FormChart(s);
                        f.Show();
                    });

                    clienteBalancos[cliente].ForEach(b =>
                    {
                        FormChartContrato f = new FormChartContrato(b);
                        f.Show();
                    });



                    //to do: Grafico Consolidado contratos (ATIVIDADES)

                    //var consolidado = new MedicaoCliente() {
                    //     MontanteProjetado = clienteBalancos[cliente].Sum(x=>x.MontanteProjetado),
                    //     Montante = clienteBalancos[cliente].Sum(x=>x.Montante),
                    //      Contratado = clienteBalancos[cliente].Sum(x=>x.Contratado)
                    //};
                    //FormChartContrato fx = new FormChartContrato(consolidado);
                    //fx.Show();



                    //clienteSumarios[cliente].ToList().GroupBy(x => x.CD_Agente_SCDD).ToList().ForEach(ag => {

                    //    if (ag.Any(x => x.Contratado != 0)) {
                    //        FormChartContrato f = new FormChartContrato(ag.ToList());
                    //        f.Show();
                    //    }
                    //});
                }

            }
            catch (Exception ex)
            {
                btnChart.SetErrorState(ex);
            }
            finally
            {
                btnChart.ExitLoadingState();
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            try
            {

                btnAll.EnterLoadingState();
                btnAll.SetErrorState(null);
                //var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                //var tsk = new Task(() => {
                Excel.Tools.xlsx(clienteSumarios, clienteBalancos, clienteToHtml);
                //});
                //tsk.Start(scheduler);

                //await tsk;

                //await Task.Factory.StartNew(() => {
                //    Excel.Tools.xlsx(clienteSumarios, clienteBalancos, clienteToHtml);
                //}, null, null, TaskCreationOptions.LongRunning, scheduler                
                //);

            }
            catch (Exception ex)
            {
                btnAll.SetErrorState(ex);
            }
            finally
            {
                btnAll.ExitLoadingState();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {

            try
            {

                clienteToHtml.Clear();
                clienteCCEmails.Clear();
                clienteEmails.Clear();
                clienteSumarios.Clear();
                clienteBalancos.Clear();
                ListBoxCliente.Items.Clear();

                button1.EnterLoadingState();
                button1.SetErrorState(null);

                var ano = int.Parse(cbxAno.SelectedItem.ToString());
                var mes = int.Parse(cbxMes.SelectedItem.ToString());

                bool cancel = false;

                await Task.Factory.StartNew(() =>
                {

                    var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
                    var jsondata = System.IO.File.ReadAllText(datafile);

                    var med = Repositorio.getMedicoesExtendidas(mes, ano);
                    //var contratos = Repositorio.getContratos(mes, ano);
                    var contratos = Repositorio.getParticionamento(new DateTime(ano, mes, 1));

                    var agentes = Repositorio.getAgente();


                    ctx = Contexto.ConfigurarRede(jsondata);
                    ctx.CarregaMedicoes(med);

                    var sumarios = ctx.ConstruirSumariosComExtensao(mes, ano);



                    foreach (var s in sumarios.GroupBy(x => x.Cliente))
                    {

                        clienteToHtml.Add(s.Key,
                            Sumario.HtmlBase.Replace("{0}",
                                String.Join(" ", s.Select(x => x.ToHtmlTable()))).
                                Replace("{1}",
                                @"<p>Prezados,</p><p>Seguem as medições coletadas do SCDE até o dia <strong>" + s.Max(y => y.Fim).ToString("dd/MM/yyyy") + "</strong></p>")
                            );

                        clienteEmails.Add(s.Key,
                            string.Join(";", ctx.Clientes.First(x => x.Nome == s.Key).Emails)
                            );

                        clienteCCEmails.Add(s.Key,
                            string.Join(";", ctx.Clientes.First(x => x.Nome == s.Key).CCEmails)
                            );

                        clienteSumarios.Add(s.Key,
                            s.ToList());



                        var q = s.SelectMany(sum =>
                        {
                            //if (cancel || (sum.CodigosUnidades.Count > 1 &&
                            //    FormConfirmaRateio.Show(sum, agentes) != DialogResult.OK))
                            //{
                            //    cancel = true;
                            //}

                            return sum.CodigosUnidades.Select(und => new { Dados = sum, perc = und.Value, id_unidade = und.Key });
                        })
                        .Where(x => x.Dados.CodigosUnidades.Count > 0 && !string.IsNullOrWhiteSpace(x.Dados.CD_Agente_SCDD))
                        .ToList();


                        clienteBalancos.Add(s.Key,
                            q.Select(sum =>
                            {

                                var inicio = sum.Dados.Inicio.AddHours(2);
                                inicio = new DateTime(inicio.Year, inicio.Month, 1);
                                var fim = inicio.AddMonths(1).AddDays(-1);
                                var medicaoProjetada = sum.Dados.TipoAtivo == "G" ? sum.Dados.ProjecaoGeracaoHora : sum.Dados.ProjecaoConsumoMedHora;
                                var medicao = sum.Dados.TipoAtivo == "G" ? sum.Dados.GeracaoMedHora : sum.Dados.ConsumoMedHora;
                                var possuiProj = sum.Dados.HorasFaltantes > 0 || (sum.Dados.Fim < Util.getIniFim(mes, ano).Item2.AddHours(-1));
                                var tipo = sum.Dados.TipoAtivo;

                                return new
                                {
                                    sum.id_unidade,
                                    sum.Dados.CD_Agente_SCDD,
                                    inicio,
                                    fim,
                                    medida = medicao * sum.perc,
                                    medidaProjetada = medicaoProjetada * sum.perc,
                                    sum.Dados.HorasFaltantes,
                                    possuiProj,
                                    tipo
                                };

                            })
                            .GroupBy(x => x.id_unidade)
                            .Select(x =>
                            {
                                var first = x.First();
                                return new
                                {
                                    id_unidade = x.Key,
                                    first.CD_Agente_SCDD,
                                    first.inicio,
                                    first.fim,

                                    medida = x.Sum(y => y.medida),
                                    medidaProjetada = x.Sum(y => y.medidaProjetada),
                                    horas_falt = x.Sum(y => y.HorasFaltantes),
                                    possui_proj = x.Any(y => y.possuiProj),
                                    tipo = first.tipo
                                };
                            }).Select(sum =>
                                new MedicaoCliente()
                                {
                                    ID_Unidade = sum.id_unidade,
                                    CD_AGEN_SCDD = sum.CD_Agente_SCDD,
                                    Inicio = sum.inicio,
                                    Fim = sum.fim,
                                    Montante = sum.medida,
                                    MontanteProjetado = sum.medidaProjetada,
                                    HorasFaltantes = sum.horas_falt,
                                    PossuiProjecao = sum.possui_proj,
                                    //Contratado =  Particionamento.CalculaContratado(sum.inicio, sum.id_unidade, sum.medidaProjetada, agentes.First(x => x.ID_Unidade == sum.id_unidade).FatorPerda, contratos),//    contratos.Where(x => x.Id_Unidade == sum.id_unidade).Sum(x => x.Montante),
                                    Unidade = contratos.Any(x => x.ID_UNIDADE.Value == sum.id_unidade) ? contratos.First(x => x.ID_UNIDADE.Value == sum.id_unidade).IDENTIFICACAO_UNIDADE : string.Empty,
                                    Tipo = sum.tipo,
                                    FatorPerda = agentes.First(x => x.ID_Unidade == sum.id_unidade).FatorPerda,
                                }
                            ).ToList()
                        );
                    }

                    var allBalancos = clienteBalancos.SelectMany(x => x.Value).ToList();
                    allBalancos.ForEach(z => z.Contratado = Particionamento.CalculaContratado(z.Inicio, z.ID_Unidade, z.MontanteProjetado, z.FatorPerda, contratos, allBalancos));
                });

                //Preenchimento da listbox antiga
                cbxClientes.DataSource = clienteToHtml.Keys.ToList();

                //CheckListBox nova
                foreach (var x in clienteToHtml.Keys.ToList())
                {
                    ListBoxCliente.Items.Add(x.ToString());
                }

                if (!cancel && MessageBox.Show("Gravar contabilizações?", "EIG Medições", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {

                    var resultados = new StringBuilder();

                    clienteBalancos.Values.SelectMany(x => x).ToList().ForEach(bal =>
                    {
                        try
                        {
                            resultados.AppendLine(bal.ID_Unidade + " " + //""
                                Repositorio.GravarMedicaoCliente(bal.ID_Unidade, bal.CD_AGEN_SCDD,
                                    bal.Inicio, bal.Fim, bal.MontanteProjetado, bal.HorasFaltantes, bal.PossuiProjecao)
                                );
                        }
                        catch (Exception ex)
                        {
                            resultados.AppendLine(ex.Message);
                        }
                    });


                    if (string.IsNullOrWhiteSpace(resultados.ToString()))
                        MessageBox.Show("Nada foi feito, verifique se os ativos possuem um código de cadastro relacionado");
                    else
                    {
                        FormDialog.Show(resultados.ToString());
                    }
                }

                groupBox1.Enabled = true;
                groupBox2.Enabled = true;
                groupBox3.Enabled = true;
                //btnSelecionarCliente.Enabled = true;
                //btnDesmarcarCliente.Enabled = true;
                cbxMarcarDesmarcar.Enabled = true;
                btnSelecionarBase.Enabled = true;
                btnAll.Enabled = true;

            }
            catch (Exception ex)
            {

                button1.SetErrorState(ex);
            }
            finally
            {
                button1.ExitLoadingState();
            }
        }

        //private void btnSelecionarCliente_Click(object sender, EventArgs e) {

        //    //looping para selecionar todos os CheckBox na ListCheckBox
        //    for (int i = 0; i <= (ListBoxCliente.Items.Count - 1); i++)
        //    {
        //        ListBoxCliente.SetItemCheckState(i, CheckState.Checked);
        //    }
        //}

        //private void btnDesmarcarCliente_Click(object sender, EventArgs e)
        //{
        //    //looping para desmarcar todos os CheckBox na ListCheckBox
        //    for (int i = 0; i <= (ListBoxCliente.Items.Count - 1); i++)
        //    {
        //        ListBoxCliente.SetItemCheckState(i, CheckState.Unchecked);
        //    }
        //}

        public List<String> verifClienteSelec()
        {
            List<String> clientesSelecionados = new List<String>();

            //Percorre a "ListBoxCliente" para adicionar todos os clientes no List<String> 
            foreach (string s in ListBoxCliente.CheckedItems)
            {
                clientesSelecionados.Add(s.ToString());
            }
            
            return clientesSelecionados;
        }

        private void btnSelecionarBase_Click(object sender, EventArgs e)
        {
            btnSelecionarBase.EnterLoadingState();
            btnSelecionarBase.SetErrorState(null);

            //Obtem ano e mês atual
            var ano = int.Parse(cbxAno.SelectedItem.ToString());
            var mes = int.Parse(cbxMes.SelectedItem.ToString());

            try
            {
                //looping para desmarcar todos os clientes da CheckedListBox antes de marcar os clientes da base cadastral
                for (int i = 0; i <= (ListBoxCliente.Items.Count - 1); i++)
                {
                    ListBoxCliente.SetItemCheckState(i, CheckState.Unchecked);
                }

                List<bool> agendaDeEnvio = new List<bool>();

                //Captura o número do dia da semana do momento
                int diaDaSemana = (int)DateTime.Now.DayOfWeek;

                //O JSON é chamado novamente para atualizar qualquer alteração realizada na base cadastral.
                var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
                var jsondata = System.IO.File.ReadAllText(datafile);

                var med = Repositorio.getMedicoes(mes, ano);

                ctx = Contexto.ConfigurarRede(jsondata);
                ctx.CarregaMedicoes(med);

                var sumarios = ctx.ConstruirSumarios(mes, ano);


                foreach (var x in ctx.Clientes)
                {
                    //Se o JSON possui valores para o item "AgendaDeEnvio"
                    if (x.AgendaDeEnvio.Count != 0)
                    {
                        //Se a "AgendaDeEnvio" for TRUE no dia da semana e o cliente existe na "ListBoxCliente" 
                        if (x.AgendaDeEnvio.ElementAt(diaDaSemana) && ListBoxCliente.Items.IndexOf(x.Nome) != -1)
                        {
                            ListBoxCliente.SetItemCheckState(ListBoxCliente.Items.IndexOf(x.Nome), CheckState.Checked);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                btnSelecionarBase.SetErrorState(ex);
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnSelecionarBase.ExitLoadingState();
            }
        }

        private void cbxMarcarDesmarcar_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = ((CheckBox)sender).Checked;

            for (int i = 0; i < ListBoxCliente.Items.Count; i++)
            {
                ListBoxCliente.SetItemChecked(i, isChecked);
            }
        }
    }
}