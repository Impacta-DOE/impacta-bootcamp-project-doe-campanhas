using System;
namespace impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas
{
    public class PontosColeta
    {
       

        public string cep { get; set; }
        public string logradouro { get; set; }
        public int numero { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public int? id { get; set; }
        public int campanhaId { get; set; }
       

    }
}
