using System;
using System.ComponentModel.DataAnnotations;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.PontoColeta
{
    public class PontoColetaCreateRequest
    {
    
        [Required]
        public string cep { get; set; }
        [Required]
        public string logradouro { get; set; }
        public int numero { get; set; }
        [Required]
        public string bairro { get; set; }
        [Required]
        public string cidade { get; set; }
        [Required]
        public string uf { get; set; }
        public string complemento { get; set; }

        public string responsavel { get; set; }
    }
}
