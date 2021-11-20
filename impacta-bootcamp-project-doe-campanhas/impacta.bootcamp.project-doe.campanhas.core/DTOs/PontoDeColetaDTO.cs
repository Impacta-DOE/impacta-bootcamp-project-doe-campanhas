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
        public int campanha;
        public int? id { get; set; }
    }
}
