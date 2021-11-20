using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.Create
{
    public class CreateRequest
    {
        [Required]
        public string titulo { get; set; }
        [Required]
        public string descricao { get; set; }
        [Required]
        public Nullable<int> tipoCampanhaId { get; set; }
        [Required]
        public Nullable<int> tipoArrecadacaoId { get; set; }
        public string   imageCapaBase64 { get; set; }
        public double metaArrecadacao { get; set; }
        public int unidadeMedidaId { get; set; }
        public string dataLimite { get; set; }
        public List<PontosColeta> pontosColeta { get; set; }
      
    }
}
