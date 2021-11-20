using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories
{
    public interface ICreateRepository
    {

        public  Task<OperationCreateDTO> createCampaign(CreateDTO dto);
    }
}
