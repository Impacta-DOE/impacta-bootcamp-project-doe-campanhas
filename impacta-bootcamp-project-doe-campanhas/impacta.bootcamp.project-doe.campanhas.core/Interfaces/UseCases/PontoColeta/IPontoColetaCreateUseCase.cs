using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;

namespace impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.PontoColeta
{
    public interface IPontoColetaCreateUseCase
    {
        public Task<OperationCreatePontoColetaDTO> create(PontoDeColetaDTO request);
    }
}
