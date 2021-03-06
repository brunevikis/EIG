using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EigMedicoes.Modelo
{
    public abstract class Modelo
    {
        public virtual string Nome { get; set; }
        public override string ToString()
        {
            return this.Nome;
        }
    }

    public class Medicao
    {
        public DateTime Periodo { get; set; }
        public DateTime PeriodoCorrigido
        {
            get
            {
                return (
                    Periodo.IsDaylightSavingTime()) ? Periodo.AddHours(1) : Periodo;
            }
        }
        public string Medidor { get; set; }
        public decimal Geracao { get; set; }
        public decimal Consumo { get; set; }
        public bool? Consistida { get; set; }
        public bool? Manual { get; set; }
        public DateTime? PeriodoCopiado { get; set; }

        public Medicao()
        {
        }

        public Medicao(DateTime periodo, string medidor, decimal consumo, decimal geracao, bool consistida = true)
        {
            Periodo = periodo;
            Medidor = medidor;
            Geracao = geracao;
            Consumo = consumo;
            Consistida = consistida;
        }

        public override bool Equals(object o)
        {
            return o.GetHashCode() == this.GetHashCode();
        }
        public override int GetHashCode()
        {
            return (this.Medidor + this.Periodo.ToString()).GetHashCode();
        }
    }

    public class Contrato
    {

        public int Id_Unidade { get; set; }
        public string Unidade { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public decimal Montante { get; set; }
        public decimal? FlexMax { get; set; }

    }

    public class Rede : Modelo
    {

        public enum TipoRede
        {
            Consumidora,
            Geradora
        }

        public Rede ancestral;
        public List<Rede> Descendentes { get; private set; }


        public string[] Medidores { get; set; }

        public string Ativo { get; set; }

        /// <summary>
        /// item1 = rateio , item2 = codigo cadastro
        /// </summary>
        public Dictionary<int, decimal>
            //List<Tuple<decimal, int>> 
            CodigosUnidades
        { get; set; }
        public string CD_Agente_SCDD { get; set; }

        public bool AgruparConsumoGeracao { get; set; }

        public Dictionary<DateTime, MedicaoProcessada> MedicoesProcessadas { get; set; }

        public bool Compartilhada
        {
            get
            {
                return Descendentes != null && Descendentes.Count() > 1;
            }
        }

        public Rede()
        {
            //CodigosUnidades = new List<Tuple<decimal, int>>();
            CodigosUnidades = new Dictionary<int, decimal>();
            Descendentes = new List<Rede>();
            MedicoesProcessadas = new Dictionary<DateTime, MedicaoProcessada>();
        }

        public void CarregarMedicao(IEnumerable<Medicao> medicoes)
        {

            var med = medicoes
                .Where(x => Medidores.Contains(x.Medidor))
                .GroupBy(x => x.Periodo).ToList();

            if (med.Count() == 0)
            {

                if (this.ancestral != null)
                {
                    this.ancestral.Descendentes.Remove(this);
                    this.ancestral.Descendentes.AddRange(this.Descendentes);
                }

                this.Descendentes.ForEach(x => x.ancestral = this.ancestral);
                this.Descendentes.Clear();

            }
            else
            {


                med
                    .ForEach(g =>
                    {
                        decimal ger, con;
                        ger = g.Sum(y => y.Geracao) / 1000;
                        con = g.Sum(y => y.Consumo) / 1000;

                        if (AgruparConsumoGeracao)
                        {
                            if (ger > con)
                            {
                                ger -= con;
                                con = 0;
                            }
                            else
                            {
                                con -= ger;
                                ger = 0;
                            }
                        }

                        MedicoesProcessadas[g.Key]
                            = new MedicaoProcessada(this)
                            {
                                Periodo = g.Key,
                                PeriodoCorrigido = g.First().PeriodoCorrigido,
                                M0_G = ger,
                                M0_C = con,
                                Consistida = g.All(y => y.Consistida == true) ? true : (g.Any(y => y.Consistida.HasValue) ? false : (bool?)null),
                                Projetada = g.Any(y => y.PeriodoCopiado.HasValue),
                            };
                    }
                        );
                //.Select(x=> new Tuple<DateTime,decimal,decimal>(x.Key, x.Sum(y=>y.Geracao), x.Sum(y=>y.Consumo)))

                if (Descendentes != null)
                {
                    foreach (var desc in Descendentes.ToList())
                    {
                        desc.CarregarMedicao(medicoes);
                    }
                }
            }
        }

        public class MedicaoProcessada
        {
            private Rede rede;

            internal MedicaoProcessada(Rede rede)
            {
                this.rede = rede;
            }

            //private decimal M0 { get { return Medidores.Sum(x => x.Geracao) - Medidores.Sum(x => x.Consumo); } }
            //public decimal M0_G { get { return M0 > 0 ? M0 : 0; } }
            //public decimal M0_C { get { return M0 < 0 ? -M0 : 0; } }

            public DateTime Periodo { get; internal set; }
            public DateTime PeriodoCorrigido { get; internal set; }

            public decimal M0_G { get; internal set; }
            public decimal M0_C { get; internal set; }

            public decimal M0_G_n1
            {
                get
                {
                    if (rede.Compartilhada)
                    {
                        return rede.Descendentes
                            //.Where(x => x.MedicoesConsolidadas[Periodo].M0_G > x.MedicoesConsolidadas[Periodo].M0_C)
                            .Sum(x => x.MedicoesProcessadas.ContainsKey(Periodo) ? x.MedicoesProcessadas[Periodo].M0_G : 0);
                    }
                    else
                        return 0;
                }
            }

            public decimal M0_C_n1
            {
                get
                {
                    if (rede.Compartilhada)
                    {
                        return rede.Descendentes
                            //.Where(x => x.MedicoesConsolidadas[Periodo].M0_G > x.MedicoesConsolidadas[Periodo].M0_C)
                            .Sum(x => x.MedicoesProcessadas.ContainsKey(Periodo) ? x.MedicoesProcessadas[Periodo].M0_C : 0);
                    }
                    else
                        return 0;
                }
            }

            public decimal PRC
            {
                get
                {
                    if (rede.Compartilhada)
                        return Math.Abs(M0_C - M0_G) - Math.Abs(M0_C_n1 - M0_G_n1);
                    else
                        return 0;
                }
            }

            public decimal PRC_C
            {
                get { return (PRC >= 0) ? PRC : 0; }
            }

            public decimal PRC_G
            {
                get { return (PRC < 0) ? -PRC : 0; }
            }

            public TipoRede Tipo
            {
                get
                {
                    return (PRC >= 0) ? TipoRede.Consumidora : TipoRede.Geradora;
                }
            }

            public decimal PART_C
            {
                get
                {
                    return
                         ((rede.ancestral != null && rede.ancestral.MedicoesProcessadas[Periodo].M0_C_n1 > 0) ?
                         Math.Round((M0_C / rede.ancestral.MedicoesProcessadas[Periodo].M0_C_n1), 6)
                         : 0)
                        ;
                }
            }

            public decimal PART_G
            {
                get
                {
                    return
                         ((rede.ancestral != null && rede.ancestral.MedicoesProcessadas[Periodo].M0_G_n1 > 0) ?
                         Math.Round((M0_G / rede.ancestral.MedicoesProcessadas[Periodo].M0_G_n1), 6)
                         : 0)
                        ;
                }
            }

            public decimal P_G
            {
                get
                {
                    if (rede.ancestral != null && rede.ancestral.Compartilhada && rede.ancestral.MedicoesProcessadas.ContainsKey(Periodo)/*&& (M0_G > M0_C)*/)
                    {
                        return Math.Round((rede.ancestral.MedicoesProcessadas[Periodo].PRC_G + rede.ancestral.MedicoesProcessadas[Periodo].P_G) * PART_G, 6);
                    }
                    else return 0;
                }
            }

            public decimal P_C
            {
                get
                {
                    if (rede.ancestral != null && rede.ancestral.Compartilhada && rede.ancestral.MedicoesProcessadas.ContainsKey(Periodo)/*&& (M0_C > M0_G)*/)
                    {
                        return Math.Round((rede.ancestral.MedicoesProcessadas[Periodo].PRC_C + rede.ancestral.MedicoesProcessadas[Periodo].P_C) * PART_C, 6);
                    }
                    else return 0;
                }
            }

            public decimal M1_G { get { return M0_G - P_G; } }
            public decimal M1_C { get { return M0_C + P_C; } }

            private bool? _consistida;

            public bool? Consistida
            {
                get
                {
                    if (this._consistida == true && (rede.ancestral == null || (
                        rede.ancestral.MedicoesProcessadas.ContainsKey(Periodo) &&
                        rede.ancestral.MedicoesProcessadas[Periodo].Consistida == true)
                        )) return true;
                    else if (this._consistida.HasValue && (rede.ancestral == null || (
                        //else if (this._consistida.HasValue || (rede.ancestral != null && (
                        rede.ancestral.MedicoesProcessadas.ContainsKey(Periodo) &&
                        rede.ancestral.MedicoesProcessadas[Periodo].Consistida.HasValue))
                        ) return false;
                    else return (bool?)null;
                }
                internal set { this._consistida = value; }
            }

            public bool Projetada { get; internal set; }

        }

        public IEnumerable<Rede> ToFlat()
        {
            //if (descendentes != null) {
            return Descendentes
                .Union(Descendentes.SelectMany(x => x.ToFlat()));
            //} else
            //    return descendentes;
        }
    }

    public class Sumario
    {
        public string Cliente { get; set; }

        public string Ativo { get; set; }
        public string TipoAtivo { get; set; }

        public Dictionary<int, decimal>
           //List<Tuple<decimal, int>> 
           CodigosUnidades
        { get; set; }
        public string CD_Agente_SCDD { get; set; }

        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }

        public int HorasFaltantes { get; set; }
        public List<DateTime> PeriodosHorasFaltantes { get; set; }

        public decimal ConsumoMedHora { get; set; }
        public decimal ConsumoMedMes { get; set; }
        public decimal ProjecaoConsumoMedHora { get; set; }
        public decimal ProjecaoConsumoMedMes { get; set; }


        public decimal GeracaoMedHora { get; set; }
        public decimal PerdaCompartilhadaGeracao { get; set; }
        public decimal ProjecaoPerdaCompartilhadaGeracao { get; set; }

        public decimal GeracaoMes { get; set; }
        public decimal ProjecaoGeracaoMes { get; set; }
        public decimal ProjecaoGeracaoHora { get; set; }

        public Dictionary<DateTime, decimal> MedicaoDiaria { get; set; }
        public Dictionary<DateTime, decimal> MedicaoDiariaProjetada { get; set; }

        public string ToHtmlTable()
        {
            var html = @"<table><tr><td>
                        <table class=""table-data"">
	                        <tr class='nome-agente'><td colspan='3'>" + this.Ativo + @"</td></tr>
	                        <tr class='subtitulo'><td colspan='3'>Medição SCDE</td></tr>"
                            + (this.GeracaoMedHora > 0 ? (
                                @"<tr class='linha'><td colspan='2' class='item'>Geração (MWh)</td><td class='value'>" + (this.GeracaoMedHora).ToString("N3") + @"</td></tr>"
                                                                        + (this.PerdaCompartilhadaGeracao != 0 ?
                                    @"<tr class='linha'><td colspan='2' class='item'>Perdas Internas</td><td class='value'>" + this.PerdaCompartilhadaGeracao.ToString("P3") + @"</td></tr>"
                                    : "") +
                                @"<tr class='linha'><td colspan='2' class='item'>Projeção de Geração (MWm)</td><td class='value'>" + (this.ProjecaoGeracaoMes).ToString("N3") + @"</td></tr>                                    
                                <tr class='linha'><td colspan='2' class='item'>Projeção de Geração (MWh)</td><td class='value'>" + (this.ProjecaoGeracaoHora).ToString("N3") + @"</td></tr>")
                                : ""
                            )
                            + (this.ConsumoMedHora > 0 && this.GeracaoMedHora > 0 ? @"<tr class='espaço'><td colspan='3'></td></tr>" : "")
                            + (this.ConsumoMedHora > 0 ? (
                                @"<tr class='linha'><td colspan='2' class='item'>Consumo (MWh)</td><td class='value'>" + (this.ConsumoMedHora).ToString("N3") + @"</td></tr>
	                            <tr class='linha'><td colspan='2' class='item'>Projeção de Consumo (MWm)</td><td class='value'>" + (this.ProjecaoConsumoMedMes).ToString("N3") + @"</td></tr>
                                <tr class='linha'><td colspan='2' class='item'>Projeção de Consumo (MWh)</td><td class='value'>" + (this.ProjecaoConsumoMedHora).ToString("N3") + @"</td></tr>"
                                                                                                    )
                                : ""
                            )
                            + @"</table>" +
                            (this.HorasFaltantes > 0 ? @"<p><em>Possui horas faltantes no(s) dia(s): " + string.Join(", ", this.PeriodosHorasFaltantes.Select(x => x.ToString("dd/MM")).Distinct()) + @"</em></p>" : "")
                            + "</td><td><div id='chart-" + this.Ativo + "'></div></td></tr></table>&nbsp;";


            return html;
        }

        #region HTMLBASE

        public static string HtmlBase = @"
<html>
<head>
<style>
.chart table {
margin: 0;
border: 0;
padding: 0;
border-collapse: collapse;
}

.title {
  text-align: center;
  margin: auto;
}

.chart-area tr{
  background-color: #FFF;
  vertical-align: bottom;
}
.chart-area > table {
  width: 640px;
  height: 400px;
}

.chart-axis 	{
  height: 80%;
  width: 75%;
}

.y-col {
  background-color: #EEE;
  height: 100%;
}

.bar {
  width: 95%;
  margin: 0;
}
.bar-value{
   background-color: #35F;
}
.bar-projection{
   background-color: rgba(204, 204, 255, 0.5);
}

.y-legend {
 text-align: right;
 border-right: 1px solid black;
 font-size: 0.8em;
	vertical-align: top;
}

.y-grid {
  border-top: 1px dashed black;
}

.y-title {
 text-align: right;
  font-size: 1.1em;
  font-weight: bold;
}
.x-legend {
  border-top: 1px solid black;
  text-align: center;
  //width: 1.2em;
  font-size: 0.8em;
  margin: 0;
  height: 1em;
}
.x-title {
  font-size: 1.1em;
  font-weight: bold;
  text-align: center;
	height: 1.2em;
}

.table-data {
    border-collapse: collapse;
    table-layout: fixed;
    width: 270pt;
	margin: 20pt;
    font-size: 11.0pt;
}

.table-data tr {
    height: 1.25em;
    margin: 1pt;
}

.nome-agente {
    padding: 0px;
    mso-ignore: padding;
    color: white;
    font-size: 1.2em;
    font-weight: 700;
    font-style: normal;
    text-decoration: none;
    font-family: Calibri, sans-serif;
    mso-font-charset: 0;
    mso-number-format: General;
    text-align: center;
    vertical-align: bottom;
    border-top: 1.0pt solid windowtext;
    border-right: none;
    border-bottom: 1.0pt solid windowtext;
    border-left: 1.0pt solid windowtext;
    background: #1F497D;
    mso-pattern: black none;
    white-space: nowrap;
}

.subtitulo {
    padding: 0px;
    mso-ignore: padding;
    color: white;
    font-weight: 700;
    font-style: normal;
    text-decoration: none;
    font-family: Calibri, sans-serif;
    mso-font-charset: 0;
    mso-number-format: General;
    text-align: center;
    vertical-align: bottom;
    border-top: 1.0pt solid windowtext;
    border-right: none;
    border-bottom: 1.0pt solid windowtext;
    border-left: 1.0pt solid windowtext;
    background: #1F497D;
    mso-pattern: black none;
    white-space: nowrap;
}

.item {
    padding: 0px;
    mso-ignore: padding;
    color: black;
    font-weight: 400;
    font-style: normal;
    text-decoration: none;
    font-family: Calibri, sans-serif;
    mso-font-charset: 0;
    mso-number-format: General;
    text-align: center;
    vertical-align: bottom;
    border: .5pt solid windowtext;
    mso-background-source: auto;
    mso-pattern: auto;
    white-space: nowrap;
}

.value {
    padding: 1px;
    mso-ignore: padding;
    color: black;
    font-weight: 400;
    font-style: normal;
    text-decoration: none;
    font-family: Calibri, sans-serif;
    mso-font-charset: 0;
    mso-number-format: ""\#\,\#\#0\.000"";
    text-align: center;
    vertical-align: bottom;
    border: .5pt solid windowtext;
    mso-background-source: auto;
    mso-pattern: auto;
    white-space: nowrap;
}

</style>

</head>
<body>

<div id='conteudo-adicional'>{1}</div>

<div>{0}
</div>
<div id='chart'></div>
</body>
</html>
";

        #endregion HTMLBASE



    }

    public class Cliente : Modelo
    {

        //public List<string> Ativos { get; private set; }
        public List<Ativo> Ativos { get; set; }
        public List<string> Emails { get; set; }
        public List<string> CCEmails { get; set; }

        public bool IncluirGrafico { get; set; }
        public bool IncluirGraficoContrato { get; set; }

        public bool IncluirExcel { get; set; }

        public List<bool> AgendaDeEnvio { get; set; }


        public Cliente()
        {
            Ativos = new List<Ativo>();
            Emails = new List<string>();
            CCEmails = new List<string>();
            AgendaDeEnvio = new List<bool>();
        }
    }

    public class Ativo : Modelo
    {

        public Rede Rede { get; set; }
        public Cliente Cliente { get; set; }

        public override string Nome
        {
            get
            {
                return Rede.Nome;
            }
            set
            {
                Rede.Nome = value;
            }
        }
    }

    public class Contexto
    {
        public List<Rede> Redes { get; internal set; }
        public List<Rede> FlatRedes { get; internal set; }
        public List<Cliente> Clientes { get; internal set; }

        internal Contexto()
        {
        }

        public void CarregaMedicoes(IEnumerable<Medicao> med)
        {
            foreach (var rData in FlatRedes)
            {
                rData.CarregarMedicao(med);
            }
        }

        public IEnumerable<Sumario> ConstruirSumarios(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);
            return ConstruirSumarios(d.Item1, d.Item2);
        }

        private IEnumerable<Sumario> ConstruirSumarios(DateTime dtIni, DateTime dtFim)
        {
            var sumarios = new List<Sumario>();

            foreach (var cData in Clientes)
            {
                foreach (var ativo in cData.Ativos)
                {
                    var sum = new Sumario() { Ativo = ativo.Nome, Cliente = cData.Nome, Inicio = dtIni };

                    var r = FlatRedes.FirstOrDefault(x => x.Nome == ativo.Nome);

                    if (r != null && r.MedicoesProcessadas.Count() > 0)
                    {
                        sum.CodigosUnidades = r.CodigosUnidades;
                        sum.CD_Agente_SCDD = r.CD_Agente_SCDD ?? r.Nome;
                        sum.TipoAtivo = r.Ativo;


                        var medicoesConsistidas = r.MedicoesProcessadas.Where(x => x.Value.Consistida == true);
                        var medicoesFaltantes = r.MedicoesProcessadas.Where(x => x.Value.Consistida == false);

                        sum.Fim = r.MedicoesProcessadas.Where(x => x.Value.Consistida.HasValue).Max(x => x.Value.Periodo);
                        sum.PeriodosHorasFaltantes = medicoesFaltantes.Select(x => x.Value.Periodo).Distinct().ToList();

                        var horasMedidas = medicoesConsistidas.Count();
                        var horasPeriodo = (decimal)(dtFim - dtIni).TotalHours;

                        var horasFaltantes = medicoesFaltantes.Count();

                        if (horasMedidas == 0) continue;

                        if (r.AgruparConsumoGeracao)
                        {
                            sum.GeracaoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_G - x.Value.M1_C);
                            if (sum.GeracaoMedHora > 0) sum.PerdaCompartilhadaGeracao = 1 - sum.GeracaoMedHora / medicoesConsistidas.Sum(x => x.Value.M0_G - x.Value.M0_C);

                            //if (cData.IncuirGrafico) {

                            //Sprint 4
                            // (ATIVIDADE) Alterar ou pensar em como guardar as medicoes em base horaria para montar o grafico.
                            sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                            {
                                DateTime dt = x.Key;
                                if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                return new DateTime(dt.Year, dt.Month, dt.Day);
                            }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G - z.Value.M1_C));

                        }
                        else
                        {
                            if (sum.TipoAtivo == "G")
                            {
                                sum.GeracaoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_G);
                                if (sum.GeracaoMedHora > 0) sum.PerdaCompartilhadaGeracao = 1 - sum.GeracaoMedHora / medicoesConsistidas.Sum(x => x.Value.M0_G);


                                sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G));
                            }
                            else if (sum.TipoAtivo == "C")
                            {
                                sum.ConsumoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_C);
                                sum.ConsumoMedMes = Math.Round(sum.ConsumoMedHora / horasPeriodo, 6);
                                sum.ProjecaoConsumoMedHora = Math.Round(sum.ConsumoMedHora * horasPeriodo / horasMedidas, 6);
                                sum.ProjecaoConsumoMedMes = Math.Round(sum.ProjecaoConsumoMedHora / horasPeriodo, 6);

                                sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_C));
                            }
                        }

                        sum.ProjecaoGeracaoMes = Math.Round(sum.GeracaoMedHora / horasMedidas, 6);
                        sum.ProjecaoGeracaoHora = Math.Round(sum.GeracaoMedHora * horasPeriodo / horasMedidas, 6);
                        sum.GeracaoMes = Math.Round(sum.GeracaoMedHora / horasPeriodo, 6);

                        if (horasFaltantes > 0) sum.HorasFaltantes = horasFaltantes;

                        sumarios.Add(sum);
                    }
                }
            }

            return sumarios;
        }

        public IEnumerable<Sumario> ConstruirSumariosComExtensao(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);
            return ConstruirSumariosComExtensao(d.Item1, d.Item2);
        }

        private IEnumerable<Sumario> ConstruirSumariosComExtensao(DateTime dtIni, DateTime dtFim)
        {
            var sumarios = new List<Sumario>();

            foreach (var cData in Clientes)
            {
                foreach (var ativo in cData.Ativos)
                {
                    var sum = new Sumario() { Ativo = ativo.Nome, Cliente = cData.Nome, Inicio = dtIni };

                    var r = FlatRedes.FirstOrDefault(x => x.Nome == ativo.Nome);

                    if (r != null && r.MedicoesProcessadas.Where(x => x.Value.Consistida.HasValue).Count() > 0)
                    {
                        sum.CodigosUnidades = r.CodigosUnidades;
                        sum.CD_Agente_SCDD = r.CD_Agente_SCDD ?? r.Nome;
                        sum.TipoAtivo = r.Ativo;


                        var medicoesConsistidas = r.MedicoesProcessadas.Where(x => x.Value.Consistida == true);
                        var medicoesFaltantes = r.MedicoesProcessadas.Where(x => x.Value.Consistida == false);
                        var medicoesProjetadas = r.MedicoesProcessadas.Where(x => x.Value.Projetada);

                        sum.Fim = r.MedicoesProcessadas.Where(x => x.Value.Consistida.HasValue).Max(x => x.Value.PeriodoCorrigido);
                        sum.PeriodosHorasFaltantes = medicoesFaltantes
                            .Select(x => x.Value.PeriodoCorrigido).Distinct().ToList();

                        var horasMedidas = medicoesConsistidas.Count();
                        var horasPeriodo = (decimal)(dtFim - dtIni).TotalHours;

                        sum.HorasFaltantes = medicoesFaltantes.Count();
                        //medicoesFaltantes.Where(x => x.Value.PeriodoCorrigido < sum.Fim)
                        //   .Select(x => x.Value.PeriodoCorrigido).Count();

                        if (horasMedidas == 0) continue;

                        if (r.AgruparConsumoGeracao)
                        {
                            sum.GeracaoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_G - x.Value.M1_C);
                            if (sum.GeracaoMedHora > 0) sum.PerdaCompartilhadaGeracao = 1 - sum.GeracaoMedHora / medicoesConsistidas.Sum(x => x.Value.M0_G - x.Value.M0_C);

                            sum.ProjecaoGeracaoHora = r.MedicoesProcessadas.Sum(x => x.Value.M1_G - x.Value.M1_C);
                            if (sum.ProjecaoGeracaoHora > 0) sum.ProjecaoPerdaCompartilhadaGeracao = 1 - sum.ProjecaoGeracaoHora / r.MedicoesProcessadas.Sum(x => x.Value.M0_G - x.Value.M0_C);

                            sum.ProjecaoGeracaoMes = Math.Round(sum.ProjecaoGeracaoHora / horasPeriodo, 6);

                            sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                            {
                                DateTime dt = x.Key;
                                if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                return new DateTime(dt.Year, dt.Month, dt.Day);
                            }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G - z.Value.M1_C));

                            sum.MedicaoDiariaProjetada = medicoesProjetadas.GroupBy(x =>
                            {
                                DateTime dt = x.Key;
                                if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                return new DateTime(dt.Year, dt.Month, dt.Day);
                            }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G - z.Value.M1_C));


                        }
                        else
                        {
                            if (sum.TipoAtivo == "G")
                            {
                                sum.GeracaoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_G);
                                if (sum.GeracaoMedHora > 0) sum.PerdaCompartilhadaGeracao = 1 - sum.GeracaoMedHora / medicoesConsistidas.Sum(x => x.Value.M0_G);

                                sum.ProjecaoGeracaoHora = r.MedicoesProcessadas.Sum(x => x.Value.M1_G);
                                if (sum.ProjecaoGeracaoHora > 0) sum.ProjecaoPerdaCompartilhadaGeracao = 1 - sum.ProjecaoGeracaoHora / r.MedicoesProcessadas.Sum(x => x.Value.M0_G - x.Value.M0_C);

                                sum.ProjecaoGeracaoMes = Math.Round(sum.ProjecaoGeracaoHora / horasPeriodo, 6);

                                sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G));

                                sum.MedicaoDiariaProjetada = medicoesProjetadas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_G));

                            }
                            else if (sum.TipoAtivo == "C")
                            {
                                sum.ConsumoMedHora = medicoesConsistidas.Sum(x => x.Value.M1_C);
                                sum.ConsumoMedMes = Math.Round(sum.ConsumoMedHora / horasPeriodo, 6);

                                sum.ProjecaoConsumoMedHora = r.MedicoesProcessadas.Sum(x => x.Value.M1_C);// Math.Round(sum.ConsumoMedHora * horasPeriodo / horasMedidas, 6);
                                sum.ProjecaoConsumoMedMes = Math.Round(sum.ProjecaoConsumoMedHora / horasPeriodo, 6);

                                sum.MedicaoDiaria = medicoesConsistidas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_C));

                                sum.MedicaoDiariaProjetada = medicoesProjetadas.GroupBy(x =>
                                {
                                    DateTime dt = x.Key;
                                    if (dt.IsDaylightSavingTime()) dt = dt.AddHours(1);
                                    return new DateTime(dt.Year, dt.Month, dt.Day);
                                }).ToDictionary(x => x.Key, x => x.Sum(z => z.Value.M1_C));
                            }
                        }

                        sumarios.Add(sum);
                    }
                }
            }

            return sumarios;
        }

        public static Contexto ConfigurarRede(string arquivoConfiguracao)
        {
            var ctx = new Contexto();

            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(arquivoConfiguracao,
              new JsonSerializerSettings { Culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR") });

            ctx.Redes = new List<Rede>();

            foreach (var rData in data.Redes)
            {
                ctx.Redes.Add(ctx.construirRede(rData));
                //redes.Last().CarregarMedicao(med);
            }

            var flatRedes = ctx.Redes.Union(ctx.Redes.SelectMany(x => x.ToFlat()));

            ctx.FlatRedes = flatRedes.ToList();

            ctx.Clientes = new List<Cliente>();
            foreach (var cData in data.Clientes)
            {
                ctx.Clientes.Add(ctx.construirCliente(cData));
            }

            return ctx;
        }

        Cliente construirCliente(JToken cData)
        {
            var clie = new Cliente();

            clie.Nome = (string)cData["Cliente"];

            //Ativos = new List<string>();
            //foreach (var at in cData["Ativos"]) {
            //    Ativos.Add((string)at);
            //}

            foreach (var at in cData["Ativos"])
            {
                // Ativos.Add(new Ativo { Nome = (string)at, Cliente = this });
                clie.Ativos.Add(new Ativo
                {
                    Cliente = clie,
                    Rede = FlatRedes.FirstOrDefault(x => x.Nome == (string)at)
                });
            }


            foreach (var em in cData["Emails"])
            {
                clie.Emails.Add((string)em);
            }


            foreach (var em in cData["CCEmails"])
            {
                clie.CCEmails.Add((string)em);
            }

            if (cData["IncluirGrafico"] != null) clie.IncluirGrafico = (bool)cData["IncluirGrafico"];
            if (cData["IncluirGraficoContrato"] != null) clie.IncluirGraficoContrato = (bool)cData["IncluirGraficoContrato"];
            if (cData["IncluirExcel"] != null) clie.IncluirExcel = (bool)cData["IncluirExcel"];

            if (cData["AgendaDeEnvio"] != null)
            {

                foreach (var em in cData["AgendaDeEnvio"])
                {
                    clie.AgendaDeEnvio.Add(em.ToObject<bool>());
                }
            }

            return clie;
        }

        public void GravarRede(string file)
        {

            JObject json = new JObject();

            JArray redes = new JArray();
            json["Redes"] = redes;

            Func<Rede, JObject> addR = null;

            addR = new Func<Rede, JObject>(rede =>
            {
                var rObj = new JObject();

                rObj["Nome"] = rede.Nome;
                if (!string.IsNullOrWhiteSpace(rede.Ativo)) rObj["Ativo"] = rede.Ativo;
                if (rede.CodigosUnidades != null && rede.CodigosUnidades.Count == 1) rObj["CodigoUnidade"] = rede.CodigosUnidades.First().Key;
                else if (rede.CodigosUnidades != null && rede.CodigosUnidades.Count > 1)
                {
                    var arr = new JArray();
                    rObj["CodigoUnidade"] = arr;
                    rede.CodigosUnidades.Select(cc => { arr.Add(cc.Value); arr.Add(cc.Key); return true; }).ToList();
                }
                if (!string.IsNullOrWhiteSpace(rede.CD_Agente_SCDD)) rObj["CD_AGEN_SCDD"] = rede.CD_Agente_SCDD;
                if (rede.AgruparConsumoGeracao) rObj["AgruparConsumoGeracao"] = rede.AgruparConsumoGeracao;
                rObj["Medidores"] = new JArray(rede.Medidores);

                if (rede.Descendentes != null && rede.Descendentes.Count > 0)
                {
                    var arr = new JArray();
                    rObj["Descendentes"] = arr;
                    rede.Descendentes.ForEach(cc => { arr.Add(addR(cc)); });
                }

                return rObj;
            });

            Redes.ForEach(x => redes.Add(addR(x)));

            JArray clientes = new JArray();
            json["Clientes"] = clientes;

            Func<Cliente, JObject> addC = new Func<Cliente, JObject>(cli =>
            {
                var rObj = new JObject();

                rObj["Cliente"] = cli.Nome;
                rObj["Emails"] = new JArray(cli.Emails);
                rObj["CCEmails"] = new JArray(cli.CCEmails);
                rObj["Ativos"] = new JArray(cli.Ativos.Select(x => x.Nome));
                rObj["IncluirGrafico"] = cli.IncluirGrafico;
                rObj["IncluirGraficoContrato"] = cli.IncluirGraficoContrato;
                rObj["IncluirExcel"] = cli.IncluirExcel;
                rObj["AgendaDeEnvio"] = new JArray(cli.AgendaDeEnvio);



                return rObj;
            });

            Clientes.ForEach(x => clientes.Add(addC(x)));

            System.IO.File.WriteAllText(file, json.ToString());
        }

        Rede construirRede(JToken rede, Rede ancestral = null)
        {
            //          this.rede = rede;
            Rede r = new Rede();

            r.ancestral = ancestral;



            r.Nome = (string)rede["Nome"];

            if (rede["Ativo"] != null) r.Ativo = (string)rede["Ativo"];
            if (rede["CodigoUnidade"] != null)
            {

                if (rede["CodigoUnidade"].Type == JTokenType.Array)
                {

                    for (int cc = 0; cc < rede["CodigoUnidade"].Count() - 1; cc += 2)
                    {

                        //r.CodigosUnidades.Add(new Tuple<decimal, int>((decimal)rede["CodigoUnidade"][cc], (int)rede["CodigoUnidade"][cc + 1]));
                        r.CodigosUnidades.Add((int)rede["CodigoUnidade"][cc + 1], (decimal)rede["CodigoUnidade"][cc]);


                    }


                }
                else if (rede["CodigoUnidade"].Type == JTokenType.Integer)
                {
                    //r.CodigosUnidades.Add(new Tuple<decimal, int>(1, (int)rede["CodigoUnidade"]));
                    r.CodigosUnidades.Add((int)rede["CodigoUnidade"], 1);
                }
            }
            if (rede["CD_AGEN_SCDD"] != null) r.CD_Agente_SCDD = (string)rede["CD_AGEN_SCDD"];


            if (rede["AgruparConsumoGeracao"] != null) r.AgruparConsumoGeracao = (bool)rede["AgruparConsumoGeracao"];


            if (rede["Descendentes"] != null)
            {
                foreach (var desc in rede["Descendentes"])
                {
                    r.Descendentes.Add(construirRede(desc, r));
                }
            }
            if (rede["Medidores"] != null) { }
            r.Medidores = rede["Medidores"].Select(x => (string)((JValue)x).Value).ToArray();

            return r;
        }

    }

    public static class Repositorio
    {

        public static List<Contrato> getContratos(int mes, int ano)
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                return rep.EIGMedicao_View_Contratos3
                    .Where(m => m.MONTANTE.HasValue && m.ID_UNIDADE.HasValue)
                    .Where(
                x => x.INICIO.Year == ano && x.INICIO.Month == mes)
                .Select(m =>
                    new Contrato()
                    {
                        Id_Unidade = m.ID_UNIDADE.Value,
                        Unidade = m.IDENTIFICACAO_UNIDADE,
                        Inicio = m.INICIO,
                        Fim = m.FIM,
                        Montante = m.MONTANTE.Value,
                        FlexMax = m.FLEX_MAX_CONTRATADO
                    }
                    )
                .ToList();
            }
        }

        public static IEnumerable<Medicao> getMedicoes(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);
            return getMedicoes(d.Item1, d.Item2);
        }

        public static IEnumerable<Medicao> getMedicoes(DateTime inicio, DateTime fim)
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                var medicoes = rep.EIGMedicao_View_Medicoes.Where(
                x => x.Periodo >= inicio && x.Periodo < fim)
                .Select(m =>
                     new Medicao() { Periodo = m.Periodo, Medidor = m.Ponto, Consumo = m.Consumo, Geracao = m.Geracao, Consistida = m.Consistido }
                     ).ToList();

                var res = medicoes.GroupBy(x => new { x.PeriodoCorrigido.Date, x.Medidor }, x => x)
                    .Where(x => x.Count() > 1)
                    .SelectMany(x => x).ToList();

                return res;
            }
        }

        public static IEnumerable<Medicao> getMedicoesExtendidas(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);
            return getMedicoesExtendidas(d.Item1, d.Item2);
        }

        public static IEnumerable<Medicao> getMedicoesExtendidas(DateTime inicio, DateTime fim)
        {
            using (var rep = new EIG_MEDIDOREntities())
            {

                DateTime iniExp = inicio.AddDays(-14);

                var med = rep.EIGMedicao_View_Medicoes.Where(
                                x => x.Periodo >= iniExp && x.Periodo < fim)
                                .ToList()
                                .Select(x => new Medicao
                                {
                                    Medidor = x.Ponto,
                                    Periodo = x.Periodo,
                                    Consumo = x.Consumo,
                                    Geracao = x.Geracao,
                                    Consistida = x.Consistido,
                                    Manual = x.Manual,
                                }).ToList();


                var medReal = med
                .Where(x => x.Periodo >= inicio && x.Periodo < fim)
                .GroupBy(x => new { x.PeriodoCorrigido.Date, x.Medidor }, x => x)
                .Where(x => x.Count() >= 23)
                .SelectMany(x => x).Where(x => x.Consistida == true);

                var medRealNaoCons = med
                .Where(x => x.Periodo >= inicio && x.Periodo < fim)
                .GroupBy(x => new { x.PeriodoCorrigido.Date, x.Medidor }, x => x)
                .Where(x => x.Count() >= 23)
                .SelectMany(x => x).Where(x => x.Consistida == false);

                med = med.Where(x => x.Consistida == true).ToList();

                var medEx = medReal.Union(
                        med.Select(x => new Medicao
                        {
                            Medidor = x.Medidor,
                            Periodo = x.Periodo,
                            Consumo = x.Consumo,
                            Geracao = x.Geracao,
                            Consistida = x.Consistida,
                            Manual = x.Manual,
                        }));
                medEx = medEx.Union(
                    med.Select(x => new Medicao
                    {
                        Medidor = x.Medidor,
                        Periodo = x.Periodo.AddDays(7),
                        PeriodoCopiado = x.Periodo,
                        Consumo = x.Consumo,
                        Geracao = x.Geracao,
                    }));
                medEx = medEx.Union(
                    med.Select(x => new Medicao
                    {
                        Medidor = x.Medidor,
                        Periodo = x.Periodo.AddDays(14),
                        PeriodoCopiado = x.Periodo,
                        Consumo = x.Consumo,
                        Geracao = x.Geracao,
                    }));
                medEx = medEx.Union(
                    med.Select(x => new Medicao
                    {
                        Medidor = x.Medidor,
                        Periodo = x.Periodo.AddDays(21),
                        PeriodoCopiado = x.Periodo,
                        Consumo = x.Consumo,
                        Geracao = x.Geracao,
                    }));
                medEx = medEx.Union(
                    med.Select(x => new Medicao
                    {
                        Medidor = x.Medidor,
                        Periodo = x.Periodo.AddDays(28),
                        PeriodoCopiado = x.Periodo,
                        Consumo = x.Consumo,
                        Geracao = x.Geracao,
                    })).Union(
                    med.Select(x => new Medicao
                    {
                        Medidor = x.Medidor,
                        Periodo = x.Periodo.AddDays(35),
                        PeriodoCopiado = x.Periodo,
                        Consumo = x.Consumo,
                        Geracao = x.Geracao,
                    }));

                medEx = from mE in medEx
                        join mNc in medRealNaoCons on new { mE.Periodo, mE.Medidor } equals new { mNc.Periodo, mNc.Medidor } into mNcJoin
                        from nMc in mNcJoin.DefaultIfEmpty()
                        let ret = nMc == null ? mE : new Medicao
                        {
                            Medidor = mE.Medidor,
                            Periodo = mE.Periodo,
                            Consumo = mE.Consumo,
                            Geracao = mE.Geracao,
                            Consistida = nMc.Consistida,
                            Manual = mE.Manual,
                            PeriodoCopiado = mE.PeriodoCopiado,
                        }
                        select ret;

                return medEx.Where(x => x.Periodo >= inicio && x.Periodo < fim).ToList();
            }
        }

        public static IEnumerable<Medicao> getMedicoesManuais(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);
            return getMedicoesManuais(d.Item1, d.Item2);
        }
        public static IEnumerable<Medicao> getMedicoesManuais(DateTime inicio, DateTime fim)
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                return rep.EIGMedicao_Medicoes_Manuais.Where(
                x => x.Periodo >= inicio && x.Periodo < fim)
                .Select(m =>
                    new Medicao() { Periodo = m.Periodo, Medidor = m.Ponto, Consumo = m.Consumo, Geracao = m.Geracao, Consistida = true }
                    )
                .ToList();
            }
        }

        public static void instertMedicoesManuais(List<Medicao> medicoes)
        {


            using (var rep = new EIG_MEDIDOREntities())
            {

                rep.EIGMedicao_Medicoes_Manuais.AddRange(
                    medicoes.Select(x =>
                        new EIGMedicao_Medicoes_Manuais()
                        {
                            Consumo = x.Consumo,
                            Geracao = x.Geracao,
                            Periodo = x.Periodo,
                            Ponto = x.Medidor
                        }));

                rep.SaveChanges();
            }
        }

        public static void limparMedicoesManuais(int mes, int ano)
        {
            var d = Util.getIniFim(mes, ano);

            using (var rep = new EIG_MEDIDOREntities())
            {
                rep.EIGMedicao_Medicoes_Manuais.RemoveRange(

                    rep.EIGMedicao_Medicoes_Manuais.Where(x => x.Periodo >= d.Item1 && x.Periodo < d.Item2)

                    );

                rep.SaveChanges();
            }
        }

        public static int ImportarDadosXMLParaStage(string filename)
        {

            //converter xml para csv

            var csvdata = new System.Text.StringBuilder(
            @"Tipo de Relat¢rio: Medidas Consolidadas;;;;;;;;;;
Tipo de Agente: Conectante | TODOS AGENTES SELECIONADOS PELO USUµRIO;;;;;;;;;;
Periodo Solicitado de 01/07/2017 at‚ 31/07/2017;;;;;;;;;;
Ponto de Medi‡Æo;Data de Consolida‡Æo;Hora de Consolida‡Æo;Tipo de Energia;Ativa Gera‡Æo;Ativa Consumo;Reativa Gera‡Æo;Reativa Consumo;Qt Intervalos Faltantes;Situa‡Æo da Medida;Motivo da Situa‡Æo
");


            //RNCBP2ECBP302;01/07/2017;1;Liquida;1175,047;0;0;436,288;0;Hora Completa Consistente;Consistido


            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            var fs = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            xmldoc.Load(fs);

            System.Xml.XmlNodeList xmlnode;
            int i = 0;

            xmlnode = xmldoc.GetElementsByTagName("CODIGO_PONTO");
            for (i = 0; i < xmlnode.Count; i++)
            {
                var ponto = xmlnode[i].Attributes["id_ponto"].Value;

                for (int dtI = 0; dtI < xmlnode[i].ChildNodes.Count; dtI++)
                {

                    var data = xmlnode[i].ChildNodes[dtI].Attributes["dt_ajuste"].Value;

                    for (int hrI = 0; hrI < xmlnode[i].ChildNodes[dtI].ChildNodes.Count; hrI++)
                    {
                        var medNode = xmlnode[i].ChildNodes[dtI].ChildNodes[hrI];
                        var hora = medNode.Attributes["hr_ajuste"].Value;

                        string consumo = "";
                        string geracao = "";

                        if (medNode.ChildNodes[0].ChildNodes[0].Name == "VALOR_ATIVA_CONSUMO")
                        {
                            consumo = medNode.ChildNodes[0].ChildNodes[0].Attributes["valorAtiva"].Value;
                            geracao = medNode.ChildNodes[0].ChildNodes[1].Attributes["valorGeracao"].Value;
                        }
                        else if (medNode.ChildNodes[0].ChildNodes[0].Name == "VALOR_ATIVA_GERACAO")
                        {
                            consumo = medNode.ChildNodes[0].ChildNodes[1].Attributes["valorAtiva"].Value;
                            geracao = medNode.ChildNodes[0].ChildNodes[0].Attributes["valorGeracao"].Value;
                        }


                        csvdata.AppendLine(
                            string.Join(";", ponto, data, hora, "Liquida", geracao, consumo, "0,0", "0,0", "0", "Hora Completa Consistente", "Consistido")
                            );
                    }

                }
            }

            var filenameN = System.IO.Path.ChangeExtension(filename, ".csv");


            System.IO.File.WriteAllText(filenameN, csvdata.ToString());




            return ImportarDadosParaStage(filenameN);
        }

        public static int ImportarDadosParaStage(string filename)
        {



            var l = "";
            using (var sr = new System.IO.StreamReader(filename)) l = sr.ReadLine();

            var modo = 1;
            if (l.ToUpperInvariant().Contains("MEDIDAS CONSOLIDADAS"))
            {
                modo = 2;
            }



            int count = 0;
            using (var rep = new EIG_MEDIDOREntities())
            {

                Func<string, System.Data.Entity.Core.Objects.ObjectParameter, int> importFunc;
                importFunc = rep.EIGMedicao_IMPORTAR_ARQUIVO_PARA_STAGE;
                System.Data.Entity.Core.Objects.ObjectParameter outputImp = new System.Data.Entity.Core.Objects.ObjectParameter("RowCount", typeof(int));
                importFunc(filename, outputImp);



                rep.Database.CommandTimeout = 60;


                var toInsert = new List<EIGMedicao_Import_Stage2>();
                using (var sr = new System.IO.StreamReader(filename, true))
                {
                    sr.ReadLine();
                    sr.ReadLine();
                    if (modo == 1)
                        while (!sr.EndOfStream)
                        {

                            var data = sr.ReadLine().Split(';');

                            if (data.Length >= 8)
                            {
                                var entity = new EIGMedicao_Import_Stage2()
                                {
                                    Agente = data[0],
                                    Ponto___Grupo = data[1],
                                    Data = data[2],
                                    Hora = data[3],
                                    Energia_Ativa_De_Consumo = data[4],
                                    Energia_Ativa_De_Geração = data[5],
                                    Qualidade = data[6],
                                    Origem = data[7]
                                };
                                toInsert.Add(entity);
                            }

                        }
                    else if (modo == 2)
                        while (!sr.EndOfStream)
                        {

                            var data = sr.ReadLine().Split(';');

                            if (data.Length >= 11)
                            {
                                var entity = new EIGMedicao_Import_Stage2()
                                {
                                    Agente = "",
                                    Ponto___Grupo = data[0],
                                    Data = data[1],
                                    Hora = data[2],
                                    Energia_Ativa_De_Consumo = data[5],
                                    Energia_Ativa_De_Geração = data[4],
                                    Qualidade = data[10],
                                    Origem = ""
                                };
                                toInsert.Add(entity);
                            }

                        }

                }
                count = toInsert.Count;

                EntityFramework.Utilities.EFBatchOperation.For(rep, rep.EIGMedicao_Import_Stage2).InsertAll(toInsert, batchSize: 400);


                /*

                Func<string, System.Data.Entity.Core.Objects.ObjectParameter, int> importFunc;
                if (l.ToUpperInvariant().Contains("MEDIDAS CONSOLIDADAS"))
                {
                    importFunc = rep.EIGMedicao_IMPORTAR_ARQUIVO2_PARA_STAGE;
                }
                else importFunc = rep.EIGMedicao_IMPORTAR_ARQUIVO_PARA_STAGE;

                System.Data.Entity.Core.Objects.ObjectParameter outputImp = new System.Data.Entity.Core.Objects.ObjectParameter("RowCount", typeof(int));

                importFunc(filename, outputImp);

                return (int)outputImp.Value

                */
            }

            return count;
        }

        public static Tuple<int, int> ProcessarDados()
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                System.Data.Entity.Core.Objects.ObjectParameter outputImp = new System.Data.Entity.Core.Objects.ObjectParameter("RowCount", typeof(int));
                System.Data.Entity.Core.Objects.ObjectParameter outputImp2 = new System.Data.Entity.Core.Objects.ObjectParameter("FaltantesCount", typeof(int));

                rep.EIGMedicao_PROCESSAR_DADOS_IMPORTADOS(outputImp, outputImp2);

                return new Tuple<int, int>((int)outputImp.Value, (int)outputImp2.Value);
            }
        }


        public static string GravarMedicaoCliente(int id_unidade, string cd_agen_scdd, DateTime inicio, DateTime fim, decimal medicao, int horasfaltantes, bool possuiProjecao)
        {



            string result = "";

# if !DEBUG

            using (var rep = new EIG_MEDIDOREntities()) {



                var existing = rep.EIGMedicao_MedicaoClientes.FirstOrDefault(
                    x => x.ID_Unidade == id_unidade
                        && x.Inicio == inicio
                        && x.Fim == fim);


                if (existing != null) {
                    existing.MWh = medicao;
                    existing.HorasFaltantes = horasfaltantes;
                    existing.PossuiProjecao = possuiProjecao;
                    existing.CD_AGEN_SCDD = cd_agen_scdd;
                    rep.Entry(existing).State = System.Data.Entity.EntityState.Modified;
                    result = "substituido";
                } else {

                    existing = new EIGMedicao_MedicaoClientes() {
                        ID_Unidade = id_unidade,
                        CD_AGEN_SCDD = cd_agen_scdd,
                        Inicio = inicio,
                        Fim = fim,
                        MWh = medicao,
                        HorasFaltantes = horasfaltantes,
                        PossuiProjecao = possuiProjecao
                    };


                    rep.EIGMedicao_MedicaoClientes.Attach(existing);
                    rep.Entry(existing).State = System.Data.Entity.EntityState.Added;

                    result = "adicionado";

                }


                rep.SaveChanges();



            }
# else
            result = id_unidade.ToString() + "\t "
                + cd_agen_scdd.ToString() + "\t "
                + inicio.ToString() + "\t"
                + fim.ToString() + "\t"
                + medicao.ToString() + "\t"
                + horasfaltantes.ToString() + "\t"
                + possuiProjecao.ToString();


# endif

            return result;

        }

        public static Contexto getContexto()
        {

            var datafile = System.Configuration.ConfigurationManager.AppSettings["datafile"];
            var jsondata = System.IO.File.ReadAllText(datafile);

            return Contexto.ConfigurarRede(jsondata);
        }

        public static List<Cadastro> getCadastro()
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                return rep.EIGMedicao_View_Cadastro
                    .Select(x => new Cadastro { Codigo = x.Codigo_de_Cadastro, Grupo = x.Grupo, Nome = x.Nome, Tipo = x.Tipo })
                     .ToList();
            }
        }
        public static List<Agente> getAgente()
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                return rep.EIGMedicao_View_Agente
                    .Select(x => new Agente
                    {
                        Tipo = x.Tipo,
                        Nome = x.Agente,
                        SG_AGEN_SCDD = x.SG_AGEN_SCDD,
                        CD_AGEN_SCDD = x.CD_AGEN_SCDD,
                        ID_Unidade = x.ID_Unidade,
                        Unidade = x.Unidade,
                        FatorPerda = x.Fator_Perda.Value
                    }).OrderBy(x => x.Nome).ToList();
            }
        }

        public static List<EIGMedicao_View_Contratos4> getParticionamento(DateTime Inicio)
        {
            using (var rep = new EIG_MEDIDOREntities())
            {
                return rep.EIGMedicao_View_Contratos4
                    .Where(x => x.INICIO == Inicio)
                    .OrderBy(x => x.Identificador).ToList();
            }
        }

    }

    public static class Util
    {
        //public static DateTime[] inicioVR = new DateTime[] {
        //    new DateTime(2017,10,15),
        //    new DateTime(2018,11,4),
        //    new DateTime(2019,10,20),
        //    new DateTime(2020,10,18),
        //    new DateTime(2021,10,17),
        //    new DateTime(2022,10,16),
        //    new DateTime(2023,10,15),
        //    new DateTime(2024,10,20),

        //    };

        //public static DateTime[] fimVR = new DateTime[] {
        //    new DateTime(2018,02,17),
        //    new DateTime(2019,02,16),
        //    new DateTime(2020,02,15),
        //    new DateTime(2021,02,20),
        //    new DateTime(2022,02,19),
        //    new DateTime(2023,02,18),
        //    new DateTime(2024,02,17),
        //    new DateTime(2025,02,15),
        //    };


        public static Tuple<DateTime, DateTime> getIniFim(int mes, int ano)
        {

            var dtIni = new DateTime(ano, mes, 1);
            var dtFim = dtIni.AddMonths(1);


            if (dtFim.AddHours(-1).IsDaylightSavingTime())
                dtFim = dtFim.AddHours(-1);

            if (dtIni.IsDaylightSavingTime())
                dtIni = dtIni.AddHours(-1);

            return new Tuple<DateTime, DateTime>(dtIni, dtFim);
        }

    }

    public class Cadastro
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }

        public override string ToString()
        {

            return Codigo.ToString() + " - " + Grupo + " - " + Nome;

        }
    }

    public class Agente
    {
        public int ID_Unidade { get; set; }
        public int? CD_AGEN_SCDD { get; set; }
        public string SG_AGEN_SCDD { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public string Tipo { get; set; }
        public decimal FatorPerda { get; set; }

        public override string ToString()
        {
            return ID_Unidade.ToString() + " - " + Nome + " - " + Unidade;
        }
    }

    public class MedicaoCliente
    {
        public int ID_Unidade { get; set; }
        public string Unidade { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public decimal Montante { get; set; }
        public decimal MontanteProjetado { get; set; }
        public int HorasFaltantes { get; set; }
        public bool PossuiProjecao { get; set; }
        public string CD_AGEN_SCDD { get; set; }
        public decimal Contratado { get; set; }
        public string Tipo { get; set; }
        public decimal FatorPerda { get; set; }


    }

    public class Particionamento
    {

        public static decimal CalculaContratado(DateTime inicio, int id_unidade, decimal MontanteProjetado, decimal fatorPerda, List<EIGMedicao_View_Contratos4> contratos, List<MedicaoCliente> medicaoClientes)
        {

            decimal horas = (decimal)(inicio.AddMonths(1) - inicio).TotalHours;

            if (inicio.Month == 10) horas--;
            else if (inicio.Month == 2) horas++;


            var temp = contratos.Where(x => x.ID_UNIDADE.Value == id_unidade).Select(x =>
            {

                if (x.Particionamento_Automatico == -1) return CalculaParticionamento(x.Identificador, horas, inicio, id_unidade, MontanteProjetado, fatorPerda, contratos, medicaoClientes) * x.MONTANTE.Value;// funcao de particionamento
                else
                {
                    return
                        x.REPRESENTATIVIDADE_MWH.HasValue ? x.REPRESENTATIVIDADE_MWH.Value
                        : x.REPRESENTATIVIDADE_PORCENTAGEM.HasValue ? x.REPRESENTATIVIDADE_PORCENTAGEM.Value * x.MONTANTE.Value
                        : CalculaParticionamento(x.Identificador, horas, inicio, id_unidade, MontanteProjetado, fatorPerda, contratos, medicaoClientes) * x.MONTANTE.Value;// funcao de particionamento;

                }
            });

            return temp.Sum() * horas;
        }


        private static decimal CalculaParticionamento(string identificador, decimal horas, DateTime inicio, int id_unidade, decimal MontanteProjetado, decimal fatorPerda, List<EIGMedicao_View_Contratos4> contratos, List<MedicaoCliente> medicaoClientes)
        {
            string ponta;
            decimal consumoUnidade;
            decimal consumoTotal;

            ponta = contratos.Where(x => x.Identificador == identificador && x.ID_UNIDADE.Value == id_unidade).First().PONTA;


            var tab_1 = contratos.Join(medicaoClientes, x => x.ID_UNIDADE.Value, x => x.ID_Unidade, (epc, emc) => new
            {
                ID_UNIDADE = epc.ID_UNIDADE.Value,
                epc.Identificador,
                epc.INICIO,
                //nr_consumo_com_perda = (emc.Tipo == "G" ? (1 - fatorPerda) * emc.MontanteProjetado : (1 + fatorPerda)) * emc.MontanteProjetado,
                nr_consumo_com_perda = Math.Round((fatorPerda * emc.MontanteProjetado), 3),
                epc.Prioridade_Proinfa,
                epc.Proinfa,
                epc.PONTA,
            }).Where(x => x.Identificador == identificador && x.PONTA == ponta);

            var tab_2 = contratos
                .Where(x => x.PONTA == ponta && x.Proinfa);



            var tab_4 = from x in tab_1
                        join pi in tab_2 on x.ID_UNIDADE equals pi.ID_UNIDADE into contratosPi
                        from pi in contratosPi.DefaultIfEmpty()
                        let nr_montante_proinfa = pi != null ? Math.Round(Math.Abs(pi.MONTANTE.Value * horas), 3) : 0
                        select new
                        {
                            x.ID_UNIDADE,
                            x.Identificador,
                            x.INICIO,
                            x.nr_consumo_com_perda,
                            x.Prioridade_Proinfa,
                            x.Proinfa,
                            nr_montante_proinfa,
                            nr_consumo_com_perda_menosPrioridaProinfa = Math.Abs(x.nr_consumo_com_perda) - (x.Prioridade_Proinfa ? nr_montante_proinfa : 0)
                        };


            //var tab_2 = contratos
            //    .Where(x => x.PONTA == ponta && x.Proinfa)
            //    .Select(x => new
            //    {
            //        nr_montante_proinfa = x.MONTANTE.Value * horas,
            //        ID_UNIDADE = x.ID_UNIDADE.Value
            //    }).FirstOrDefault();

            //var tab_3 = tab_1.Select(x => new
            //{
            //    x.ID_UNIDADE,
            //    x.Identificador,
            //    x.INICIO,
            //    x.nr_consumo_com_perda,
            //    x.Prioridade_Proinfa,
            //    x.Proinfa,
            //    nr_montante_proinfa = tab_2 != null ? tab_2.nr_montante_proinfa : 0,
            //    //nr_consumo_com_perda_menosPrioridaProinfa =
            //    //    (x.nr_consumo_com_perda < 0 ? (x.nr_consumo_com_perda * -1) : x.nr_consumo_com_perda)
            //    //    - (x.Prioridade_Proinfa ? (tab_2 != null ? tab_2.nr_montante_proinfa : 0) : 0)

            //    nr_consumo_com_perda_menosPrioridaProinfa =
            //        (x.nr_consumo_com_perda < 0 ? (x.nr_consumo_com_perda * -1m) : x.nr_consumo_com_perda)
            //        - (x.Prioridade_Proinfa ? (tab_2 != null ? (tab_2.nr_montante_proinfa < 0 ? (tab_2.nr_montante_proinfa * -1) : tab_2.nr_montante_proinfa) : 0) : 0)

            //});

            //consumoTotal = tab_3.Sum(x => x.nr_consumo_com_perda_menosPrioridaProinfa);
            //consumoUnidade = tab_3.Where(x => x.ID_UNIDADE == id_unidade).Sum(x => x.nr_consumo_com_perda_menosPrioridaProinfa);

            consumoTotal = tab_4.Sum(x => x.nr_consumo_com_perda_menosPrioridaProinfa);
            consumoUnidade = tab_4.Where(x => x.ID_UNIDADE == id_unidade).Sum(x => x.nr_consumo_com_perda_menosPrioridaProinfa);

            var representatividade = (consumoTotal != 0 ? Math.Round((consumoUnidade / consumoTotal), 15) : 0);

            return representatividade;
        }
    }

}