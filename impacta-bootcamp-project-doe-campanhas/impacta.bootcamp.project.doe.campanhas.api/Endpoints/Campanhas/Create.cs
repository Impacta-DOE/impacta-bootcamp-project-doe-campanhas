using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Campanhas.Create;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.Campanhas
{
    public class Create :BaseEndPoint<CreateRequest,CreateResponse>
    {



        private readonly ICreateUseCase useCase;
        public Create(ICreateUseCase createUseCase)

        {
            useCase = createUseCase;
        }

        /// <summary>
        /// Cria uma nova campanha
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("campanhas/create")]
        public async override Task<ActionResult<CreateResponse>> HandleAsync(CreateRequest request, CancellationToken cancellationToken = default)
        {
            CreateResponse response = new CreateResponse();
          


            try
            {
                if(!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar campanha, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new CreateResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }
                var model = ModelToDTO(request);
                model.user = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.create(model);
                var createResponse =  dtoToResponse(create);



                if(createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao criar campanha" ;

                var resp = new CreateResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }
        private  CreateResponse dtoToResponse(OperationCreateDTO operationCreateDTO)
        {
            var response = new CreateResponse();

            if(!operationCreateDTO.sucesso)

            {

                if(operationCreateDTO.error != null)
                {
                    Error error = new Error() { errorCode = operationCreateDTO.error.errorCode, errorMessage = operationCreateDTO.error.errorMessage };

                    response.error = error;
                }
                response.status = "400";
        
            }
            else
            {
                response.status = "200";
            }

            return response;
        }


        private CreateDTO ModelToDTO(CreateRequest request)
        {

            CreateDTO dto = new CreateDTO()
            {
                titulo = request.titulo,
                descricao = request.descricao,
                tipoCampanhaId = request.tipoCampanhaId,
                tipoArrecadacaoId = request.tipoArrecadacaoId,
                imageCapaBase64 = request.imageCapaBase64,
                metaArrecadacao= request.metaArrecadacao,
                unidadeMedidaId = request.unidadeMedidaId,
                dataLimite = request.dataLimite

            };



            if(request.pontosColeta != null && request.pontosColeta.Count() > 0 )
            {
                List<PontoDeColetaDTO> pontoDeColetaDTO = new List<PontoDeColetaDTO>();
                foreach (var ponto in request.pontosColeta)
                {
                    PontoDeColetaDTO pontoDeColeta = new PontoDeColetaDTO()
                    {
                        cep = ponto.cep,
                        logradouro = ponto.logradouro,
                        numero = ponto.numero,
                        bairro = ponto.bairro,
                        cidade =ponto.cidade,
                        uf = ponto.uf,
                        campanha = ponto.campanhaId,
                        id = ponto.id

        
    };

                    pontoDeColetaDTO.Add(pontoDeColeta);
                }

                dto.pontosColeta = pontoDeColetaDTO;
            }

            return dto;

        }
   
    }
}
