using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EigMedicoes.Modelo {

    public class GraficoHtml {

        public static string Construir(List<decimal> medidos, decimal projecao, string titulo, int ano, int mes) {
            var html = new StringBuilder();

            #region a

            html.AppendFormat(@"<div class=""chart""><div class=""title"">{0}</div><div class=""legend"">
	<div><div class=""bar-value""></div> Medidos</div>
	<div><div class=""bar-projection""></div> Projetados</div></div>
<div class=""chart-area"">", titulo);

            #endregion a

            var numDias = DateTime.DaysInMonth(ano, mes);
            var maxValue = medidos.Max() * 1.1m;
            var xDivs = 5;

            #region b

            foreach (var m in medidos) {
                html.AppendFormat(@"<div class=""y-col""><div class=""bar bar-value"" style=""height: {0:#}%"" ></div></div>",
                    (m / maxValue * 100));
            }
            for (int i = 0; i < numDias - medidos.Count(); i++) {
                html.AppendFormat(@"<div class=""y-col""><div class=""bar bar-projection"" style=""height: {0:#}%"" ></div></div>",
                   (projecao / maxValue * 100));
            }
            html.Append(@"</div>");

            #endregion b

            #region c

            html.Append(@"<div class=""chart-axis""><div class=""y-axis""><div class=""y-title"">MWh</div>");

            for (int i = 1; i <= xDivs; i++) {
                html.AppendFormat(@"<div class=""y-legend"">{0:N0}</div>", (1 - ((i - 1m) / xDivs)) * maxValue);
            }
            html.Append(@"</div>");
            for (int i = 1; i <= xDivs; i++) {
                html.Append(@"<div class=""y-grid""></div>");
            }

            #endregion c

            #region d

            html.Append(@"<div class=""x-axis""><div class=""x-title"">Dia</div>");

            for (int i = 1; i <= numDias; i++) {
                html.AppendFormat(@"<div class=""x-legend"">{0}</div>", i);
            }

            html.Append(@"</div></div></div></body></html>");

            #endregion d

            //            return @"<table class='chart'>
            //	<tr>
            //		<td class='title'>
            //
            //			  CONSUMO (MWh)
            //
            //		</td>
            //	</tr>
            //	<tr>
            //		<td class='chart-area'>
            //			<table>
            //					<tr><td class='y-legend'>100</td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 20%' ><td style='width: 95%;margin: 0;background-color: #35F;' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 40%' ><td style='width: 95%;margin: 0;background-color: #35F;' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 60%' ><td style='width: 95%;margin: 0;background-color: #35F;' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 80%' ><td style='width: 95%;margin: 0;background-color: #35F;' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //                    <td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //
            //<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //
            //<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //
            //<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //	<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //	<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //	<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //
            //<td rowspan='5' style='height:100%;'><table style='width:100%;height:100%;'><tr><td></td></tr><tr style='height: 75%' ><td style='width: 95%;margin: 0;background-color: rgba(204, 204, 255, 0.5);' ></td></tr></table></td>
            //					</tr>
            //					<tr><td class='y-legend'>80</td></tr>
            //					<tr><td class='y-legend'>60</td></tr>
            //					<tr><td class='y-legend'>40</td></tr>
            //					<tr><td class='y-legend'>20</td></tr>
            //				<tr class='x-axis'>
            //					<td></td>
            //					<td class='x-legend'>01</td>
            //					<td class='x-legend'>02</td>
            //					<td class='x-legend'>03</td>
            //					<td class='x-legend'>04</td>
            //					<td class='x-legend'>05</td>
            //					<td class='x-legend'>06</td>
            //					<td class='x-legend'>07</td>
            //					<td class='x-legend'>08</td>
            //					<td class='x-legend'>09</td>
            //					<td class='x-legend'>10</td>
            //					<td class='x-legend'>11</td>
            //					<td class='x-legend'>12</td>
            //					<td class='x-legend'>13</td>
            //					<td class='x-legend'>14</td>
            //					<td class='x-legend'>15</td>
            //					<td class='x-legend'>16</td>
            //					<td class='x-legend'>17</td>
            //					<td class='x-legend'>18</td>
            //					<td class='x-legend'>19</td>
            //					<td class='x-legend'>20</td>
            //					<td class='x-legend'>21</td>
            //					<td class='x-legend'>22</td>
            //					<td class='x-legend'>23</td>
            //					<td class='x-legend'>24</td>
            //					<td class='x-legend'>25</td>
            //					<td class='x-legend'>26</td>
            //					<td class='x-legend'>27</td>
            //					<td class='x-legend'>28</td>
            //					<td class='x-legend'>29</td>
            //					<td class='x-legend'>30</td>
            //					<td class='x-legend'>31</td>
            //				</tr>
            //				<tr >
            //					<td></td>
            //					<td class='x-title' colspan='31'>Dia</td>
            //				</tr>
            //			</table>
            //		</td>
            //	</tr>
            //
            //</table>";

            return html.ToString();
        }
    }
}