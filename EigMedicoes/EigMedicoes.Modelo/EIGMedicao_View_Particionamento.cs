//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EigMedicoes.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class EIGMedicao_View_Particionamento
    {
        public Nullable<int> ID_UNIDADE { get; set; }
        public string Identificador { get; set; }
        public string IDENTIFICACAO_UNIDADE { get; set; }
        public System.DateTime INICIO { get; set; }
        public System.DateTime FIM { get; set; }
        public string PONTA { get; set; }
        public decimal MONTANTE { get; set; }
        public Nullable<decimal> REPRESENTATIVIDADE_MWH { get; set; }
        public Nullable<decimal> REPRESENTATIVIDADE_PORCENTAGEM { get; set; }
        public Nullable<int> Particionamento_Automatico { get; set; }
        public Nullable<int> HORAS { get; set; }
        public decimal Flex_Min { get; set; }
        public decimal Flex_Max { get; set; }
        public Nullable<int> Flex_Tipo { get; set; }
        public bool Proinfa { get; set; }
        public bool Prioridade_Proinfa { get; set; }
        public Nullable<decimal> MONTANTE_EXERCIDO { get; set; }
    }
}
