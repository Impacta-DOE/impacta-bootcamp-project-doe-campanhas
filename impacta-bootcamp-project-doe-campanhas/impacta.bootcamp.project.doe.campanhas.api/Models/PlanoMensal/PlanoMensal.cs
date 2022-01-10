using System;
using System.Collections.Generic;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.GetAll;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.PlanoMensal
{
    public class PlanoMensalModel
    {
        public int id { get; set; }
        public bool status { get; set; }
        public string nomePlano { get; set; }
        public double valorMensal { get; set; }
        public DateTime dataCriacao { get; set; }
        public Organizacao organizacao { get; set; }
        public List<PlanoMensalRecompensa> recompensas { get; set; }
    }
}
