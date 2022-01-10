using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.PlanoMensal
{
    public class PlanoMensalRequest
    {
        public string nomePlano { get; set; }
        public decimal valor  { get; set; }
        public List<PlanoMensalRecompensa> recompensas { get; set; }
    }
}
