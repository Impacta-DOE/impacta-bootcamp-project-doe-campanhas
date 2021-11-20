using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Enums;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.Repositories.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.PontoColeta;

namespace impacta.bootcamp.project.doe.campanhas.application.UseCases.PontoColeta
{
    public class PontoColetaCreateUseCase : IPontoColetaCreateUseCase
    {
        private readonly ICreatePontoColetaRepository _createPontoColetaRepository;
        public PontoColetaCreateUseCase(ICreatePontoColetaRepository createPontoColetaRepository)
        {
            _createPontoColetaRepository = createPontoColetaRepository;
        }

        public  async Task<OperationCreatePontoColetaDTO> create(PontoDeColetaDTO request)
        {
            OperationCreatePontoColetaDTO operationCreateDTO = await isValidCreate(request);
            if (operationCreateDTO.sucesso)
            {
                operationCreateDTO = await _createPontoColetaRepository.createPontoColeta(request);
            }

            return operationCreateDTO;
        }

        private async Task<OperationCreatePontoColetaDTO> isValidCreate(PontoDeColetaDTO request)
        {
            OperationCreatePontoColetaDTO operationCreateDTO = new OperationCreatePontoColetaDTO()
            {
                sucesso = true
            };
            if (string.IsNullOrEmpty(request.cep))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL01",
                    errorMessage = "cep e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (!Regex.IsMatch(request.cep, @"\d"))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL02",
                    errorMessage = "cep deve ser numerico"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (string.IsNullOrEmpty(request.logradouro))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL03",
                    errorMessage = "logradouro e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (string.IsNullOrEmpty(request.bairro))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL04",
                    errorMessage = "bairro e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (string.IsNullOrEmpty(request.cidade))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL05",
                    errorMessage = "cidade e obrigatoria"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }

            if (string.IsNullOrEmpty(request.uf))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL06",
                    errorMessage = "uf e obrigatorio"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            if (!Enum.IsDefined(typeof(UF),request.uf))
            {
                operationCreateDTO.error = new ErrorDTO()
                {
                    errorCode = "VAL07",
                    errorMessage = "uf invalida"
                };
                operationCreateDTO.sucesso = false;
                return operationCreateDTO;
            }
            return operationCreateDTO;
        }
    }
}
