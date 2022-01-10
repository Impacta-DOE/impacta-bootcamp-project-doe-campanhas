using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using impacta.bootcamp.project.doe.campanhas.api.Models.Errors;
using impacta.bootcamp.project.doe.campanhas.api.Models.PlanoMensal;
using impacta.bootcamp.project_doe.campanhas.core.DTOs;
using impacta.bootcamp.project_doe.campanhas.core.Interfaces.UseCases.Create;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace impacta.bootcamp.project.doe.campanhas.api.Endpoints.PlanoMensal
{
    public class CreatePlan : BaseEndPoint<PlanoMensalRequest, PlanoMensalResponse>
    {
        private readonly ICreateUseCase useCase;
        public CreatePlan(ICreateUseCase createUseCase)

        {
            useCase = createUseCase;
        }

        /// <summary>
        /// Cria uma novo plano
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("planos/create")]

        public async override Task<ActionResult<PlanoMensalResponse>> HandleAsync(PlanoMensalRequest request, CancellationToken cancellationToken = default)
        {
            PlanoMensalResponse response = new PlanoMensalResponse();



            try
            {
                if (!ModelState.IsValid)
                {
                    IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                    string msg = $"Erro ao criar campanha, request invalido" + modelErrors.ElementAt(0).ErrorMessage;

                    var responseError = new PlanoMensalResponse()
                    {
                        status = "400",
                        error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                    };
                    return BadRequest(responseError);

                }
                var model = ModelToDTO(request);

                // model.user = "damian_lindgren@yahoo.com";
                model.userName = ((ClaimsIdentity)User.Identity).Claims.FirstOrDefault(cl => cl.Type == ClaimTypes.Email)?.Value;
                var create = await useCase.createPlan(model);
                var createResponse = dtoToResponse(create);



                if (createResponse.status == "400")
                {
                    return BadRequest(createResponse);
                }
                return Ok(createResponse);
            }
            catch (Exception ex)
            {
                string msg = $"Erro ao criar campanha" + ex.ToString();

                var resp = new PlanoMensalResponse()
                {
                    status = "500",
                    error = new Models.Errors.Error() { errorCode = "MODVAL01", errorMessage = msg }

                };
                return StatusCode(StatusCodes.Status500InternalServerError, resp);

            }
        }
        private PlanoMensalResponse dtoToResponse(OperationCreateDTO operationCreateDTO)
        {
            var response = new PlanoMensalResponse();

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


        private PlanoMensalDTO ModelToDTO(PlanoMensalRequest request)
        {

            PlanoMensalDTO dto = new PlanoMensalDTO()
            {
                nomePlano = request.nomePlano,
                valor = request.valor

            };

      

            if (request.recompensas != null && request.recompensas.Count() > 0)
            {
                List<PlanoMensalRecompensaDTO> pontoDeColetaDTO = new List<PlanoMensalRecompensaDTO>();
                foreach (var ponto in request.recompensas)
                {
                    PlanoMensalRecompensaDTO pontoDeColeta = new PlanoMensalRecompensaDTO()
                    {
                        descricaoRecompensa = ponto.descricaoRecompensa,
                    


                    };

                    pontoDeColetaDTO.Add(pontoDeColeta);
                }

                dto.recompensas = pontoDeColetaDTO;
            }

            return dto;

        }

    }
}
