using System;
using System.Collections.Generic;

namespace impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.GetAll
{
    public class OrganizacaoGetAllResponse
    {
        public string registro { get; set; }
        public string nomeOrganizacao { get; set; }
        public string subtituloOrganizacao { get; set; }
        public string logo { get; set; }
        public string descricaoOrganizacao { get; set; }
        public string backgroundPaginaOrg { get; set; }
        public List<CampanhaGetAllResponse> campanhas { get; set; }
        public List<CampanhaGetAllResponse> acoes { get; set; }
    }
}
