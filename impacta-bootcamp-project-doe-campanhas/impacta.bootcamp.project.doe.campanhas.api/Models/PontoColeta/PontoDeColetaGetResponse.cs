using System;
using System.Collections.Generic;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.PontoColeta
{
    public class PontoDeColetaGetResponse : BaseResponse
    {
        public List<PontoDeColetaDTO> pontosColeta { get; set; }
    }
}
