using System;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.PontoColeta;

namespace impacta.bootcamp.project.doe.campanhas.application.UseCases.PontoColeta
{
    public class PontoColetaGetAllUseCase : IPontoColetaGetAllUseCase
    {

        private readonly IGetAllPontoColetaRepository _getAllPontoColetaRepository;
        public PontoColetaGetAllUseCase(IGetAllPontoColetaRepository getAllPontoColetaRepository)
        {
            _getAllPontoColetaRepository = getAllPontoColetaRepository;
        }
       

        public async Task<OperationPontoColetaGetAllDTO> getAll(string user)
        {
            if(string.IsNullOrEmpty(user))
            {
                OperationPontoColetaGetAllDTO operationCreateDTO = new OperationPontoColetaGetAllDTO()
                {
                    sucesso = false,
                    error = new ErrorDTO()
                    {
                        errorCode = "VAL01",
                        errorMessage = "cep e obrigatorio"
                    }

                  
            };
                return operationCreateDTO;
            }
            else
            {
                return await _getAllPontoColetaRepository.getAll(user);
            }
        }



    }
}
