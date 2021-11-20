using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project.doe.campanhas.api.Models.PontoColeta;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.PontoColeta;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.PontoColeta
{
    public class Create : BaseEndPoint<PontoColetaCreateRequest, PontoColetaResponse>
    {
        private readonly IPontoColetaCreateUseCase useCase;
        public Create(IPontoColetaCreateUseCase createUseCase)

        {
            useCase = createUseCase;
        }
        /// <summary>
        /// Cria um novo ponto de coleta
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("pontocoleta/create")]
        public async override Task<ActionResult<PontoColetaResponse>> HandleAsync(PontoColetaCreateRequest request, CancellationToken cancellationToken = default)
        {
            PontoColetaResponse response = new PontoColetaResponse();



            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar ponto de coleta, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new PontoColetaResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }
                var model = ModelToDTO(request);
                model.user = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.create(model);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao criar ponto de coleta";

                var resp = new PontoColetaResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }

        private PontoColetaResponse dtoToResponse(OperationCreatePontoColetaDTO operationCreateDTO)
        {
            var response = new PontoColetaResponse();

            if (!operationCreateDTO.sucesso)

            {

                if (operationCreateDTO.error != null)
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

        private PontoDeColetaDTO ModelToDTO(PontoColetaCreateRequest ponto)
        {






            PontoDeColetaDTO pontoDeColeta = new PontoDeColetaDTO()
            {
                cep = ponto.cep.Replace("-","").Trim(),
                logradouro = ponto.logradouro,
                numero = ponto.numero,
                bairro = ponto.bairro,
                cidade = ponto.cidade,
                uf = ponto.uf.ToUpper(),
                responsavel = ponto.responsavel,
                complemento = ponto.complemento


            };

 

            return pontoDeColeta;

        }

    }
}
