using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project_doe.campanhas.core.DTOs
{
    public class OperationPontoColetaGetAllDTO
    {
        public bool sucesso { get; set; }
        public ErrorDTO error { get; set; }
        public List<PontoDeColetaDTO> pontosColeta { get; set; }
    }
}
