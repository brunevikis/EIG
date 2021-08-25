using EigMedicoes.Modelo;
using EigMedicoes.Modelo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xls = Microsoft.Office.Interop.Excel;



namespace EigMedicoes.Excel {
    public static class Tools {
        public static void xlsx(Contexto ctx, string cliente, int mes, int ano, string saveToFile = null) {

            Microsoft.Office.Interop.Excel.Application xl = null;
            Microsoft.Office.Interop.Excel.Workbook xlwb = null;
            try {
                var redes = ctx.FlatRedes.Where(x => ctx.Clientes.First(y => y.Nome == cliente).Ativos.Any(z => z.Nome == x.Nome));

                while (redes.Any(r => r.ancestral != null)) {
                    redes = redes.Where(r => r.ancestral == null).Union(redes.Where(r => r.ancestral != null).Select(r => r.ancestral));
                }

                var dtIni = Util.getIniFim(mes, ano).Item1;
                var dtFim = Util.getIniFim(mes, ano).Item2;

                int i = 0;
                var perArray = new DateTime[(int)(dtFim - dtIni).TotalHours, 1];
                for (DateTime per = dtIni; per < dtFim; per = per.AddHours(1), i++) {
                    perArray[i, 0] = per;
                }

                //if (rede != null) {
                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.ScreenUpdating = false;
                xlwb = xl.Workbooks.Add();
                xl.Calculation = Xls.XlCalculation.xlCalculationManual;
                //xl.ScreenUpdating = true;
                //xl.Visible = true;

                foreach (var rede in redes.Where(r => r.ancestral == null)) {

                    Action<Rede> write = null;
                    write = new Action<Rede>(r => {

                        var xlsh0 = (Xls.Worksheet)xlwb.Worksheets.Add();
                        xlsh0.Name = r.Nome;

                        xlsh0.Cells[2, 1].Value = "Período";
                        xlsh0.Cells[2, 2].Value = "M0_C";
                        xlsh0.Cells[2, 3].Value = "M0_G";
                        xlsh0.Cells[2, 4].Value = "M0_C (n+1)";
                        xlsh0.Cells[2, 5].Value = "M0_G (n+1)";
                        xlsh0.Cells[2, 6].Value = "PRC";
                        xlsh0.Cells[2, 7].Value = "PRC_C";
                        xlsh0.Cells[2, 8].Value = "PRC_G";
                        xlsh0.Cells[2, 9].Value = "PART_C";
                        xlsh0.Cells[2, 10].Value = "PART_G";
                        xlsh0.Cells[2, 11].Value = "P_C";
                        xlsh0.Cells[2, 12].Value = "P_G";
                        xlsh0.Cells[2, 13].Value = "M1_C";
                        xlsh0.Cells[2, 14].Value = "M1_G";
                        xlsh0.Cells[2, 15].Value = "Situação";


                        object[,] m0 = new object[perArray.Length, 2];
                        object[,] consistido = new object[perArray.Length, 1];

                        //for (DateTime per = dtIni; per < dtFim; per = per.AddHours(1), i++) {                            
                        for (i = 0; i < perArray.Length; i++) {
                            if (r.MedicoesProcessadas.ContainsKey(perArray[i, 0])) {
                                m0[i, 0] = r.MedicoesProcessadas[perArray[i, 0]].M0_C;
                                m0[i, 1] = r.MedicoesProcessadas[perArray[i, 0]].M0_G;
                                //VERIFICAR
                                //não está imprimindo no Excel de cada cliente a "Situação" por causa dos IF's abaixo
                                //Verifiquei que há a possibilidade de Consistida e Projetada serem FALSE. Por que???
                                consistido[i, 0] =
                                    r.MedicoesProcessadas[perArray[i, 0]].Consistida == false
                                     && r.MedicoesProcessadas[perArray[i, 0]].Projetada == true
                                    ? "Faltante Projetada" :
                                    (r.MedicoesProcessadas[perArray[i, 0]].Projetada == true ? "Projetada" : "")
                                    ;
                            } else {
                                m0[i, 0] = null;
                                m0[i, 1] = null;
                                consistido[i, 0] = null;
                            }
                        }

                        xlsh0.Range[xlsh0.Cells[3, 1], xlsh0.Cells[3 + perArray.Length - 1, 1]].Value2 = perArray;

                        xlsh0.Range[xlsh0.Cells[3, 1], xlsh0.Cells[3 + perArray.Length - 1, 1]].NumberFormatLocal = "dd/mm hh";

                        xlsh0.Range[xlsh0.Cells[3, 2], xlsh0.Cells[3 + perArray.Length - 1, 3]].Value2 = m0;

                        xlsh0.Range[xlsh0.Cells[3, 15], xlsh0.Cells[3 + perArray.Length - 1, 15]].Value2 = consistido;

                        foreach (var rd in r.Descendentes) {

                            write(rd);
                        }


                        if (r.Descendentes != null && r.Descendentes.Count > 0) {

                            xlsh0.Range[xlsh0.Cells[3, 4], xlsh0.Cells[3 + perArray.Length - 1, 4]]
                                .FormulaLocal = "=" + string.Join(" + ", r.Descendentes.Select(rd => "'" + rd.Nome + "'!B:B"));

                            xlsh0.Range[xlsh0.Cells[3, 5], xlsh0.Cells[3 + perArray.Length - 1, 5]]
                               .FormulaLocal = "=" + string.Join(" + ", r.Descendentes.Select(rd => "'" + rd.Nome + "'!C:C"));

                            xlsh0.Range[xlsh0.Cells[3, 6], xlsh0.Cells[3 + perArray.Length - 1, 6]]
                                .FormulaLocal = "=ABS(C:C-B:B)-ABS(E:E-D:D)";
                            xlsh0.Range[xlsh0.Cells[3, 7], xlsh0.Cells[3 + perArray.Length - 1, 7]]
                               .FormulaLocal = "=SE(F:F>=0;F:F;0)";
                            ((Xls.Range)xlsh0.Range[xlsh0.Cells[3, 8], xlsh0.Cells[3 + perArray.Length - 1, 8]])
                               .FormulaLocal = "=SE(F:F<0;ABS(F:F);0)";
                        }
                        if (r.ancestral != null) {
                            xlsh0.Range[xlsh0.Cells[3, 9], xlsh0.Cells[3 + perArray.Length - 1, 9]]
                                .FormulaLocal = "=ARRED(SEERRO(B:B/'" + r.ancestral.Nome + "'!D:D;0);6)";
                            xlsh0.Range[xlsh0.Cells[3, 10], xlsh0.Cells[3 + perArray.Length - 1, 10]]
                                .FormulaLocal = "=ARRED(SEERRO(C:C/'" + r.ancestral.Nome + "'!E:E;0);6)";

                            xlsh0.Range[xlsh0.Cells[3, 11], xlsh0.Cells[3 + perArray.Length - 1, 11]]
                                .FormulaLocal = "=ARRED(I:I*('" + r.ancestral.Nome + "'!G:G+'" + r.ancestral.Nome + "'!K:K);6)";//ARRED(SEERRO(B:B/'" + r.ancestral.Nome + "'!D:D;0);6)";
                            xlsh0.Range[xlsh0.Cells[3, 12], xlsh0.Cells[3 + perArray.Length - 1, 12]]
                                .FormulaLocal = "=ARRED(J:J*('" + r.ancestral.Nome + "'!H:H+'" + r.ancestral.Nome + "'!L:L);6)";

                        }

                        xlsh0.Range[xlsh0.Cells[3, 13], xlsh0.Cells[3 + perArray.Length - 1, 13]]
                            .FormulaLocal = "=B:B+K:K";

                        xlsh0.Range[xlsh0.Cells[3, 14], xlsh0.Cells[3 + perArray.Length - 1, 14]]
                            .FormulaLocal = "=C:C-L:L";



                    });

                    write(rede);

                }

                if (!string.IsNullOrWhiteSpace(saveToFile)) {

                    xlwb.SaveAs(saveToFile);
                    xlwb.Close(false);
                    xl.Quit();

                }


            } finally {
                if (xl != null && xl.Workbooks != null && xl.Workbooks.Count > 0) {

                    xl.Calculation = Xls.XlCalculation.xlCalculationAutomatic;
                    xl.ScreenUpdating = true;
                    xl.Visible = true;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlwb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xl);

                xlwb = null;
                xl = null;

            }
        }

        public static void xlsx(Dictionary<string, List<Sumario>> clienteSumarios, Dictionary<string, List<MedicaoCliente>> clienteBalancos, Dictionary<string, string> clienteToHtml) {

            Microsoft.Office.Interop.Excel.Application xl = null;
            Microsoft.Office.Interop.Excel.Workbook xlwb = null;
            try {

                xl = new Microsoft.Office.Interop.Excel.Application();
                xl.ScreenUpdating = false;
                xlwb = xl.Workbooks.Add();

                xl.ActiveWindow.Zoom = 75;

                foreach (var cliente in clienteToHtml.Keys) {

                    var xlsh0 = (Xls.Worksheet)xlwb.Worksheets.Add();
                    xlsh0.Name = cliente.PadRight(31).Substring(0, 31).Trim();

                    xl.ActiveWindow.Zoom = 75;

                    int row = 1;


                    System.Windows.Forms.Clipboard.SetText(clienteToHtml[cliente]);
                    xlsh0.Cells[row, 1].Select();
                    xlsh0.PasteSpecial();
                    xlsh0.Cells[row, 1].EntireColumn.AutoFit();
                    xlsh0.Cells[row, 3].EntireColumn.AutoFit();

                    clienteSumarios[cliente].ForEach(s => {
                        FormChart f = new FormChart(s);
                        f.BuildChart();
                        f.CopyChart();
                        xlsh0.Cells[row, 5].Select();
                        xlsh0.PasteSpecial();
                        row = row + 16;

                    });

                    row = 1;

                    clienteBalancos[cliente].ForEach(b => {
                        FormChartContrato f = new FormChartContrato(b);
                        f.BuildChart();
                        f.CopyChart();
                        xlsh0.Cells[row, 13].Select();
                        xlsh0.PasteSpecial();
                        row = row + 16;
                    });
                }


            } finally {
                if (xl != null && xl.Workbooks != null && xl.Workbooks.Count > 0) {

                    xl.Calculation = Xls.XlCalculation.xlCalculationAutomatic;
                    xl.ScreenUpdating = true;
                    xl.Visible = true;
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlwb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xl);

                xlwb = null;
                xl = null;

            }

        }

        public static WbFaltantes faltantes(List<Medicao> dados, int ano, int mes) {
            return new WbFaltantes(dados, ano, mes);
        }

        public class WbFaltantes {

            public class FaltantesEventArgs : EventArgs {
                public List<Medicao> Dados { get; set; }
                public int Ano { get; set; }
                public int Mes { get; set; }
            }

            public event EventHandler<FaltantesEventArgs> OnClose;

            Microsoft.Office.Interop.Excel.Application xl = null;
            Microsoft.Office.Interop.Excel.Workbook xlwb = null;
            Microsoft.Office.Interop.Excel.Worksheet xlsh = null;
            Microsoft.Office.Interop.Excel.Range rng = null;



            public List<Medicao> Dados {
                get {


                    object[,] vals = xlsh.UsedRange.Value;

                    var meds = new List<Medicao>();

                    for (int r = 2; r <= vals.GetLength(0); r++) {

                        if ((bool)vals[r, 6])
                            meds.Add(
                                new Medicao() {
                                    Periodo = ((DateTime)vals[r, 2]).AddHours((double)vals[r, 3] - 1),
                                    Medidor = (string)vals[r, 1],
                                    Consumo = Convert.ToDecimal(vals[r, 5]),
                                    Geracao = Convert.ToDecimal(vals[r, 4]),
                                    Consistida = (bool)vals[r, 6]
                                });
                    }
                    return meds;
                }
            }
            public int Ano { get; set; }
            public int Mes { get; set; }

            internal WbFaltantes(List<Medicao> dados, int ano, int mes) {
                try {
                    Ano = ano;
                    Mes = mes;
                    xl = new Microsoft.Office.Interop.Excel.Application();
                    xl.ScreenUpdating = false;
                    xlwb = xl.Workbooks.Add();

                    xlwb.BeforeClose += xlwb_BeforeClose;


                    var d = new object[dados.Count + 1, 6];

                    d[0, 0] = "Medidor";
                    d[0, 1] = "Data";
                    d[0, 2] = "Hora";
                    d[0, 3] = "Geracao";
                    d[0, 4] = "Consumo";
                    d[0, 5] = "Consistida";
                    for (int i = 1; i <= dados.Count; i++) {
                        d[i, 0] = dados[i - 1].Medidor;
                        d[i, 1] = dados[i - 1].Periodo.Date;
                        d[i, 2] = dados[i - 1].Periodo.Hour + 1;
                        d[i, 3] = dados[i - 1].Geracao;
                        d[i, 4] = dados[i - 1].Consumo;
                        d[i, 5] = dados[i - 1].Consistida;
                    }
                    xlsh = ((Microsoft.Office.Interop.Excel.Worksheet)xlwb.Sheets[1]);
                    rng = xlsh.Range[xlsh.Cells[1, 1], xlsh.Cells[dados.Count + 1, 6]];
                    rng.Value = d;


                } finally {
                    if (xl != null && xl.Workbooks != null && xl.Workbooks.Count > 0) {

                        xl.Calculation = Xls.XlCalculation.xlCalculationAutomatic;
                        xl.ScreenUpdating = true;
                        xl.Visible = true;
                    }
                }
            }

            void xlwb_BeforeClose(ref bool Cancel) {

                if (OnClose != null) OnClose(this, new FaltantesEventArgs() { Dados = Dados, Ano = Ano, Mes = Mes });

                System.Runtime.InteropServices.Marshal.ReleaseComObject(rng);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsh);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlwb);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xl);

                xlwb = null;
                xl = null;
                xlsh = null;
                rng = null;
            }
        }
    }

    public class MedSheet {
        Xls.Worksheet sh;

        public MedSheet(Xls.Worksheet sh) {
            this.sh = sh;
        }
    }
}
