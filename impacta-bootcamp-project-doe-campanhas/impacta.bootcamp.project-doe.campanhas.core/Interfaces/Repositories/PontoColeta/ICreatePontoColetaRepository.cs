using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta
{
    public interface ICreatePontoColetaRepository
    {
        public Task<OperationCreatePontoColetaDTO> createPontoColeta(PontoDeColetaDTO dto);
    }
}
