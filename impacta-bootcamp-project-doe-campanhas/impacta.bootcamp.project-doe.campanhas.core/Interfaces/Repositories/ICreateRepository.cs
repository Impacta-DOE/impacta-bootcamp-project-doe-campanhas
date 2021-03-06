using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories
{
    public interface ICreateRepository
    {

        public  Task<OperationCreateDTO> createCampaign(CreateDTO dto);
        public Task<OperationCreateDTO> commentCampaign(CommentDTO dto);
        public Task<OperationCreateDTO> createPlan(PlanoMensalDTO dto);
        public Task<OperationCreateDTO> adPlan(int planoMensalId, string userName);
        public Task<OperationCreateDTO> cancelPlan(int planoMensalId, string userName);

    }
}
