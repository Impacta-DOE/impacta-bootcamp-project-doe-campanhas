using System;
namespace impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.GetAll
{
    public class Doacoes
    {
        public object exibirValorDoado { get; set; }
        public double valorDoado { get; set; }
        public object exibirNomeDoador { get; set; }
        public string nomeDoador { get; set; }
        public DateTime dataDoacao { get; set; }
        public int idCampanha { get; set; }
    }
}
