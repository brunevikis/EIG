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
    
    public partial class EIGMedicao_MedicaoClientes
    {
        public int ID_Unidade { get; set; }
        public int ID_Cadastro { get; set; }
        public System.DateTime Inicio { get; set; }
        public System.DateTime Fim { get; set; }
        public Nullable<decimal> MWh { get; set; }
        public int HorasFaltantes { get; set; }
        public bool PossuiProjecao { get; set; }
        public string SiglaAgente { get; set; }
        public string CD_AGEN_SCDD { get; set; }
    }
}
