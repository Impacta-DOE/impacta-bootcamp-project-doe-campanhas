using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project_doe.campanhas.core.DTOs
{
    public class PlanoMensalDTO
    {
        public int planoMensalId { get; set; }
        public string nomePlano { get; set; }
        public decimal valor { get; set; }
        public string userName { get; set; }
        public List<PlanoMensalRecompensaDTO> recompensas
        {
            get; set;
        }
    }
}
