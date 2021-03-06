using System;
namespace impacta.bootcamp.project_doe.campanhas.core.DTOs
{
    public class PontoDeColetaDTO
    {
        public string cep { get; set; }
        public string logradouro { get; set; }
        public int numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string complemento { get; set; }
        public int campanha { get; set; }
        public string responsavel { get; set; }
        public string user { get; set; }
        public int? id { get; set; }
    }
}
