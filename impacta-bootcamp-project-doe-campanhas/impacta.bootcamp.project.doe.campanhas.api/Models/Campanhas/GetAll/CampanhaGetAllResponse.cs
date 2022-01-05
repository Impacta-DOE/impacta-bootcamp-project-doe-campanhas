using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.GetAll
{
    public class CampanhaGetAllResponse
    {


        public string img_background { get; set; }
        public string img_background_card { get; set; }
        public int id_campanha { get; set; }
        public string nome_campanha { get; set; }
        public string desc_campanha { get; set; }
        public double valorTotal { get; set; }
        public string tipoCampanha { get; set; }
        public string dataLimite { get; set; }
        public double? valorAtual { get; set; }
        public Organizacao organizacao { get; set; }
        public IEnumerable<Comentarios> comentarios { get; set; }
        public IEnumerable<Doacoes> doacoes { get; set; }
        public string tipo_doacao { get; set; }
        public string unidadeMedida { get; set; }
        public bool status { get; set; }
        public Voluntarios solicitacaoVoluntario { get; set; }
    }


    public class Organizacao
    {
        public string registro { get; set; }
        public string nome_organizacao { get; set; }
        public string subtitulo_organizacao { get; set; }
        public string logo { get; set; }
        public string descricao_organizacao { get; set; }
        public string background_pagina_org { get; set; }
    }

    public class Voluntarios
    {
        public bool? status { get; set; }
        public string descricaoVaga { get; set; }
      
    }


}
