using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project_doe.campanhas.core.DTOs
{
    public class CreateDTO
    {
        

          
        public string titulo { get; set; }
       
        public string  user { get; set; }
        public string descricao { get; set; }   
        public Nullable<int> tipoCampanhaId { get; set; }
        public Nullable<int> tipoArrecadacaoId { get; set; }
        public string imageCapaBase64 { get; set; }
        public string urlImage { get; set; }
        public double metaArrecadacao { get; set; }
        public int unidadeMedidaId { get; set; }
        public string dataLimite { get; set; }
        public List<PontoDeColetaDTO> pontosColeta { get; set; }
    
    }
}
